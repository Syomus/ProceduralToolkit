using System.Collections.Generic;
using UnityEngine;

namespace ProceduralToolkit.Examples
{
    public enum BrickSize
    {
        Narrow,
        Wide,
    }

    /// <summary>
    /// Breakout clone with procedurally generated levels
    /// </summary>
    public class Breakout
    {
        private const float backgroundColorSaturation = 0.25f;
        private const float backgroundColorValue = 0.7f;
        private const float brickColorSaturation = 0.7f;
        private const float brickColorMinValue = 0.6f;
        private const float brickColorMaxValue = 0.8f;

        private const float brickHeight = 0.5f;
        private const float paddleHeight = 0.5f;
        private const float paddleSpeed = 25f;
        private const float ballRadius = 0.5f;
        private const float ballForce = 200;

        private Camera mainCamera;
        private Transform bricksContainer;
        private Sprite whiteSprite;
        private PhysicsMaterial2D bouncyMaterial;
        private GameObject borders;
        private GameObject paddle;
        private Transform paddleTransform;
        private GameObject ball;
        private Transform ballTransform;
        private Rigidbody2D ballRigidbody;
        private List<GameObject> bricks = new List<GameObject>();

        private int wallWidth;
        private int wallHeight;
        private int wallHeightOffset;
        private float paddleWidth;
        private float ballSize;
        private float ballVelocityMagnitude;

        private Dictionary<BrickSize, float> sizeValues = new Dictionary<BrickSize, float>
        {
            {BrickSize.Narrow, 0.5f},
            {BrickSize.Wide, 1f},
        };

        public Breakout(Camera mainCamera)
        {
            this.mainCamera = mainCamera;
            bricksContainer = new GameObject("Bricks").transform;

            // Generate texture and sprite for bricks, paddle and ball
            var texture = Texture2D.whiteTexture;
            whiteSprite = Sprite.Create(texture,
                rect: new Rect(0, 0, texture.width, texture.width),
                pivot: new Vector2(0.5f, 0.5f),
                pixelsPerUnit: texture.width);

            // Bouncy material for walls, paddle and everything else
            bouncyMaterial = new PhysicsMaterial2D {name = "Bouncy", bounciness = 1, friction = 0};
        }

        public void UpdateParameters(
            int wallWidth,
            int wallHeight,
            int wallHeightOffset,
            float paddleWidth,
            float ballSize,
            float ballVelocityMagnitude)
        {
            this.wallWidth = wallWidth;
            this.wallHeight = wallHeight;
            this.wallHeightOffset = wallHeightOffset;
            this.paddleWidth = paddleWidth;
            this.ballSize = ballSize;
            this.ballVelocityMagnitude = ballVelocityMagnitude;
        }

        public void Update()
        {
            float delta = Input.GetAxis("Horizontal")*Time.deltaTime*paddleSpeed;
            paddleTransform.position += new Vector3(delta, 0);

            // Prevent paddle from penetrating walls
            float halfWall = (wallWidth - 1)/2f;
            if (paddleTransform.position.x > halfWall)
            {
                paddleTransform.position = new Vector3(halfWall, 0);
            }
            if (paddleTransform.position.x < -halfWall)
            {
                paddleTransform.position = new Vector3(-halfWall, 0);
            }

            // Ball should move with constant velocity
            ballRigidbody.velocity = ballRigidbody.velocity.normalized*ballVelocityMagnitude;

            if (ballTransform.position.y < -0.1f)
            {
                ResetLevel();
            }

            float angle = Vector2.Angle(ballRigidbody.velocity, Vector2.right);
            if (angle < 30 || angle > 150)
            {
                // Prevent ball from bouncing between walls
                KickBallInRandomDirection();
            }
        }

        public void ResetLevel()
        {
            GenerateBorders();
            GenerateLevel();
            GeneratePaddle();
            GenerateBall();
        }

        private void GenerateBorders()
        {
            if (borders != null)
            {
                Object.Destroy(borders);
            }
            borders = new GameObject("Border");
            float bordersHeight = wallHeightOffset + wallHeight/2 + 1 + ballSize;
            float bordersWidth = wallWidth + 1;

            // Bottom
            CreateBoxCollider(offset: new Vector2(0, -1 - ballSize/2),
                size: new Vector2(bordersWidth, 1));
            // Left
            CreateBoxCollider(offset: new Vector2(-bordersWidth/2f, bordersHeight/2f - 0.5f - ballSize/2),
                size: new Vector2(1, bordersHeight + 1));
            // Right
            CreateBoxCollider(offset: new Vector2(bordersWidth/2f, bordersHeight/2f - 0.5f - ballSize/2),
                size: new Vector2(1, bordersHeight + 1));
            // Top
            CreateBoxCollider(offset: new Vector2(0, bordersHeight - ballSize/2),
                size: new Vector2(bordersWidth, 1));
        }

        private void CreateBoxCollider(Vector2 offset, Vector2 size)
        {
            var collider = borders.AddComponent<BoxCollider2D>();
            collider.sharedMaterial = bouncyMaterial;
            collider.offset = offset;
            collider.size = size;
        }

        private void GenerateLevel()
        {
            // Destroy existing bricks
            foreach (var brick in bricks)
            {
                Object.Destroy(brick);
            }
            bricks.Clear();

            float fromHue = Random.value;
            float toHue = fromHue + 30/360f;

            var backgroundColor = new ColorHSV(fromHue, backgroundColorSaturation, backgroundColorValue).complementary;
            mainCamera.backgroundColor = backgroundColor.ToColor();

            for (int y = 0; y < wallHeight; y++)
            {
                // Select color for current line
                float hue = Mathf.Lerp(fromHue, toHue, y/(wallHeight - 1f));

                // Generate brick sizes for current line
                List<BrickSize> brickSizes = FillWallWithBricks(wallWidth);

                Vector3 leftEdge = Vector3.left*wallWidth/2 + Vector3.up*(wallHeightOffset + y*brickHeight);
                for (int i = 0; i < brickSizes.Count; i++)
                {
                    var brickSize = brickSizes[i];
                    var position = leftEdge + Vector3.right*sizeValues[brickSize]/2;

                    // Randomize tint of current brick
                    float colorValue = Random.Range(brickColorMinValue, brickColorMaxValue);
                    Color color = new ColorHSV(hue, brickColorSaturation, colorValue).ToColor();

                    bricks.Add(GenerateBrick(position, color, brickSize));
                    leftEdge.x += sizeValues[brickSize];
                }
            }
        }

        private List<BrickSize> FillWallWithBricks(float width)
        {
            // https://en.wikipedia.org/wiki/Knapsack_problem
            // We are using knapsack problem solver to fill fixed width with bricks of random width
            Dictionary<BrickSize, int> knapsack;
            float knapsackWidth;
            do
            {
                // Prefill knapsack to get nicer distribution of widths
                knapsack = GetRandomKnapsack(width);
                // Calculate sum of brick widths in knapsack
                knapsackWidth = KnapsackWidth(knapsack);
            } while (knapsackWidth > width);

            width -= knapsackWidth;
            knapsack = PTUtils.Knapsack(sizeValues, width, knapsack);
            var brickSizes = new List<BrickSize>();
            foreach (var pair in knapsack)
            {
                for (var i = 0; i < pair.Value; i++)
                {
                    brickSizes.Add(pair.Key);
                }
            }
            brickSizes.Shuffle();
            return brickSizes;
        }

        private Dictionary<BrickSize, int> GetRandomKnapsack(float width)
        {
            var knapsack = new Dictionary<BrickSize, int>();
            foreach (var key in sizeValues.Keys)
            {
                knapsack[key] = (int) Random.Range(0, width/3);
            }
            return knapsack;
        }

        private float KnapsackWidth(Dictionary<BrickSize, int> knapsack)
        {
            float knapsackWidth = 0f;
            foreach (var key in knapsack.Keys)
            {
                knapsackWidth += knapsack[key]*sizeValues[key];
            }
            return knapsackWidth;
        }

        private GameObject GenerateBrick(Vector3 position, Color color, BrickSize size)
        {
            var brick = new GameObject("Brick");
            brick.transform.position = position;
            brick.transform.parent = bricksContainer;
            brick.transform.localScale = new Vector3(sizeValues[size], brickHeight);

            var brickRenderer = brick.AddComponent<SpriteRenderer>();
            brickRenderer.sprite = whiteSprite;
            brickRenderer.color = color;

            var brickCollider = brick.AddComponent<BoxCollider2D>();
            brickCollider.sharedMaterial = bouncyMaterial;
            brick.AddComponent<Brick>();
            return brick;
        }

        private void GeneratePaddle()
        {
            if (paddle == null)
            {
                paddle = new GameObject("Paddle");
                paddleTransform = paddle.transform;

                var paddleRenderer = paddle.AddComponent<SpriteRenderer>();
                paddleRenderer.sprite = whiteSprite;
                paddleRenderer.color = Color.black;

                var paddleCollider = paddle.AddComponent<BoxCollider2D>();
                paddleCollider.sharedMaterial = bouncyMaterial;
            }

            paddleTransform.position = Vector3.zero;
            paddleTransform.localScale = new Vector3(paddleWidth, paddleHeight);
        }

        private void GenerateBall()
        {
            if (ball == null)
            {
                ball = new GameObject("Ball");
                ballTransform = ball.transform;

                var ballRenderer = ball.AddComponent<SpriteRenderer>();
                ballRenderer.sprite = whiteSprite;
                ballRenderer.color = Color.black;

                var ballCollider = ball.AddComponent<CircleCollider2D>();
                ballCollider.radius = ballRadius;
                ballCollider.sharedMaterial = bouncyMaterial;

                ballRigidbody = ball.AddComponent<Rigidbody2D>();
                ballRigidbody.gravityScale = 0;
                ballRigidbody.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
                ballRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
            }

            ballTransform.position = Vector3.up;
            ballTransform.localScale = new Vector3(ballSize, ballSize);
            ballRigidbody.velocity = Vector2.zero;
            KickBallInRandomDirection();
        }

        private void KickBallInRandomDirection()
        {
            Vector2 direction = Random.Range(-0.5f, 0.5f)*Vector2.right + Vector2.up;
            ballRigidbody.AddForce(direction*ballForce);
        }
    }
}