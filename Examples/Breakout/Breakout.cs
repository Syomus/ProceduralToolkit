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
        private GameObject borders;
        private GameObject paddle;
        private Transform paddleTransform;
        private GameObject ball;
        private Transform ballTransform;
        private Rigidbody2D ballRigidbody;
        private List<GameObject> bricks = new List<GameObject>();
        private Transform bricksContainer;
        private Texture2D texture;
        private Sprite sprite;
        private PhysicsMaterial2D bouncyMaterial;
        private int wallWidth;
        private int wallHeight;
        private int wallHeightOffset;
        private float brickHeight = 0.5f;
        private float paddleWidth;
        private float paddleHeight = 0.5f;
        private float paddleSpeed = 25f;
        private float ballSize;
        private float ballRadius = 0.5f;
        private float ballForce = 200;
        private float ballVelocityMagnitude;

        private Dictionary<BrickSize, float> sizeValues = new Dictionary<BrickSize, float>
        {
            {BrickSize.Narrow, 0.5f},
            {BrickSize.Wide, 1f},
        };

        public Breakout()
        {
            bricksContainer = new GameObject("Bricks").transform;

            // Generate texture and sprite for bricks, paddle and ball
            texture = Texture2D.whiteTexture;
            sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.width),
                pivot: new Vector2(0.5f, 0.5f),
                pixelsPerUnit: texture.width);

            // Bouncy material for walls, paddle and everything else
            bouncyMaterial = new PhysicsMaterial2D {name = "Bouncy", bounciness = 1, friction = 0};
        }

        public void UpdateParameters(int wallWidth, int wallHeight, int wallHeightOffset, float paddleWidth,
            float ballSize, float ballVelocityMagnitude)
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
                KickBall();
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

            var colliderDown = borders.AddComponent<BoxCollider2D>();
            colliderDown.sharedMaterial = bouncyMaterial;
            colliderDown.offset = new Vector2(0, -1 - ballSize/2);
            colliderDown.size = new Vector2(bordersWidth, 1);

            var colliderLeft = borders.AddComponent<BoxCollider2D>();
            colliderLeft.sharedMaterial = bouncyMaterial;
            colliderLeft.offset = new Vector2(-bordersWidth/2f, bordersHeight/2f - 0.5f - ballSize/2);
            colliderLeft.size = new Vector2(1, bordersHeight + 1);

            var colliderRight = borders.AddComponent<BoxCollider2D>();
            colliderRight.sharedMaterial = bouncyMaterial;
            colliderRight.offset = new Vector2(bordersWidth/2f, bordersHeight/2f - 0.5f - ballSize/2);
            colliderRight.size = new Vector2(1, bordersHeight + 1);

            var colliderTop = borders.AddComponent<BoxCollider2D>();
            colliderTop.sharedMaterial = bouncyMaterial;
            colliderTop.offset = new Vector2(0, bordersHeight - ballSize/2);
            colliderTop.size = new Vector2(bordersWidth, 1);
        }

        private void GenerateLevel()
        {
            // Destroy existing bricks
            foreach (var brick in bricks)
            {
                Object.Destroy(brick);
            }
            bricks.Clear();

            var gradient = RandomE.gradientHSV;

            for (int y = 0; y < wallHeight; y++)
            {
                // Select color for current line
                var lineColor = gradient.Evaluate(y/(wallHeight - 1f));
                // Generate brick sizes for current line
                List<BrickSize> brickSizes = BrickSizes(wallWidth);

                float x = 0f;
                var previousColor = lineColor;
                for (int i = 0; i < brickSizes.Count; i++)
                {
                    // Randomize tint of current brick
                    var color = lineColor;
                    while (previousColor == color)
                    {
                        color -= Color.white*RandomE.Range(-0.2f, 0.2f, 3);
                    }
                    previousColor = color;

                    var brickSize = brickSizes[i];
                    if (i >= 0)
                    {
                        x += sizeValues[brickSize]/2;
                    }

                    Vector3 position = Vector3.right*(x - wallWidth/2f) + Vector3.up*(wallHeightOffset + y/2f);
                    bricks.Add(GenerateBrick(position, color, brickSize));
                    x += sizeValues[brickSize]/2;
                }
            }
        }

        private List<BrickSize> BrickSizes(float width)
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
            brickRenderer.sprite = sprite;
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
                paddleRenderer.sprite = sprite;
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
                ballRenderer.sprite = sprite;
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
            KickBall();
        }

        private void KickBall()
        {
            Vector2 direction = Random.Range(-0.5f, 0.5f)*Vector2.right + Vector2.up;
            ballRigidbody.AddForce(direction*ballForce);
        }
    }
}