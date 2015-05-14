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
    public class Breakout : MonoBehaviour
    {
        private GameObject paddle;
        private GameObject ball;
        private List<GameObject> bricks = new List<GameObject>();
        private Transform bricksContainer;
        private Texture2D brickTexture;
        private Sprite brickSprite;
        private int brickWidth = 100;
        private int brickHeight = 50;
        private int wallWidth = 9;
        private int wallHeight = 7;
        private float paddleSpeed = 25f;
        private PhysicsMaterial2D material;
        private int wallHeightOffset = 5;
        private float ballForce = 200;
        private float ballVelocityMagnitude = 5;

        private Dictionary<BrickSize, float> sizeValues = new Dictionary<BrickSize, float>
        {
            {BrickSize.Narrow, 0.5f},
            {BrickSize.Wide, 1f},
        };

        private void Awake()
        {
            bricksContainer = new GameObject("Bricks").transform;
            brickTexture = TextureE.whitePixel;
            brickTexture.Resize(brickWidth, brickHeight);
            brickSprite = Sprite.Create(brickTexture, new Rect(0, 0, brickWidth, brickHeight), new Vector2(0.5f, 0.5f));
            material = new PhysicsMaterial2D {name = "Bouncy", bounciness = 1, friction = 0};
            GeneratePaddle();
            GenerateBorders();
        }

        private void Start()
        {
            ResetLevel();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                ResetLevel();
            }
            var delta = Input.GetAxis("Horizontal")*Time.deltaTime*paddleSpeed;
            paddle.transform.position += new Vector3(delta, 0);

            var halfWall = (wallWidth - 1)/2f;
            if (paddle.transform.position.x > halfWall)
            {
                paddle.transform.position = new Vector3(halfWall, 0);
            }
            if (paddle.transform.position.x < -halfWall)
            {
                paddle.transform.position = new Vector3(-halfWall, 0);
            }

            ball.GetComponent<Rigidbody2D>().velocity = ball.GetComponent<Rigidbody2D>().velocity.normalized*ballVelocityMagnitude;
            if (ball.transform.position.y < -0.1f)
            {
                ResetLevel();
            }
            var angle = Vector2.Angle(ball.GetComponent<Rigidbody2D>().velocity, Vector2.right);
            if (angle < 30 || angle > 150)
            {
                KickBall();
            }
        }

        private void ResetLevel()
        {
            GenerateLevel();
            GenerateBall();
        }

        private void GenerateBorders()
        {
            var borders = new GameObject("Border");
            var bordersHeight = wallHeightOffset + wallHeight/2 + 1;
            var bordersWidth = wallWidth + 1;

            var colliderDown = borders.AddComponent<BoxCollider2D>();
            colliderDown.sharedMaterial = material;
            colliderDown.offset = new Vector2(0, -1);
            colliderDown.size = new Vector2(bordersWidth, 1);

            var colliderLeft = borders.AddComponent<BoxCollider2D>();
            colliderLeft.sharedMaterial = material;
            colliderLeft.offset = new Vector2(-bordersWidth/2f, bordersHeight/2f - 0.5f);
            colliderLeft.size = new Vector2(1, bordersHeight + 1);

            var colliderRight = borders.AddComponent<BoxCollider2D>();
            colliderRight.sharedMaterial = material;
            colliderRight.offset = new Vector2(bordersWidth/2f, bordersHeight/2f - 0.5f);
            colliderRight.size = new Vector2(1, bordersHeight + 1);

            var colliderTop = borders.AddComponent<BoxCollider2D>();
            colliderTop.sharedMaterial = material;
            colliderTop.offset = new Vector2(0, bordersHeight);
            colliderTop.size = new Vector2(bordersWidth, 1);
        }

        private void GenerateLevel()
        {
            foreach (var brick in bricks)
            {
                Destroy(brick);
            }
            bricks.Clear();

            var gradient = RandomE.gradientHSV;

            for (int y = 0; y < wallHeight; y++)
            {
                var lineColor = gradient.Evaluate(y/(wallHeight - 1f));

                var brickSizes = BrickSizes(wallWidth);

                var x = 0f;
                var previousColor = lineColor;
                for (int i = 0; i < brickSizes.Count; i++)
                {
                    var color = lineColor;
                    while (previousColor == color)
                    {
                        color -= Color.white * RandomE.Range(-0.2f, 0.2f, 3);
                    }
                    previousColor = color;
                    
                    var brickSize = brickSizes[i];
                    if (i >= 0)
                    {
                        x += sizeValues[brickSize]/2;
                    }

                    var position = Vector3.right*(x - wallWidth/2f) + Vector3.up*(wallHeightOffset + y/2f);
                    bricks.Add(GenerateBrick(position, color, brickSize));
                    x += sizeValues[brickSize]/2;
                }
            }
        }

        private List<BrickSize> BrickSizes(float width)
        {
            var knapsack = GetRandomKnapsack(width);
            while (KnapsackWidth(knapsack) > width)
            {
                knapsack = GetRandomKnapsack(width);
            }

            width -= KnapsackWidth(knapsack);
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
            var knapsackWidth = 0f;
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
            brick.transform.localScale = new Vector3(sizeValues[size], 1);
            var brickRenderer = brick.AddComponent<SpriteRenderer>();
            brickRenderer.sprite = brickSprite;
            brickRenderer.color = color;
            var brickCollider = brick.AddComponent<BoxCollider2D>();
            brickCollider.sharedMaterial = material;
            brick.AddComponent<Brick>();
            return brick;
        }

        private void GeneratePaddle()
        {
            paddle = new GameObject("Paddle");
            paddle.transform.position = Vector3.zero;
            var paddleRenderer = paddle.AddComponent<SpriteRenderer>();
            paddleRenderer.sprite = brickSprite;
            paddleRenderer.color = Color.black;
            var paddleCollider = paddle.AddComponent<BoxCollider2D>();
            paddleCollider.sharedMaterial = material;
        }

        private void GenerateBall()
        {
            if (ball == null)
            {
                ball = new GameObject("Ball");
                ball.transform.localScale = new Vector3(0.5f, 1);
                var ballRenderer = ball.AddComponent<SpriteRenderer>();
                ballRenderer.sprite = brickSprite;
                ballRenderer.color = Color.black;
                var ballCollider = ball.AddComponent<CircleCollider2D>();
                ballCollider.radius = 0.3f;
                ballCollider.sharedMaterial = material;
                var ballRigidbody = ball.AddComponent<Rigidbody2D>();
                ballRigidbody.gravityScale = 0;
                ballRigidbody.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            }

            ball.transform.position = Vector3.up;
            ball.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            KickBall();
        }

        private void KickBall()
        {
            var direction = Random.Range(-0.5f, 0.5f)*Vector2.right + Vector2.up;
            ball.GetComponent<Rigidbody2D>().AddForce(direction*ballForce);
        }

        private void OnGUI()
        {
            GUI.color = Color.black;
            GUI.Label(new Rect(20, 20, Screen.width, Screen.height),
                "Click to generate new level. Use A/D or Left/Right to move.");
        }
    }
}