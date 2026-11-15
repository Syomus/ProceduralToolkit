using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace ProceduralToolkit.Samples
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
        [Serializable]
        public class Config
        {
            public int wallWidth = 9;
            public int wallHeight = 7;
            public int wallHeightOffset = 5;
            public float paddleWidth = 1;
            public float ballSize = 0.5f;
            public float ballVelocityMagnitude = 5;
            public Gradient gradient;
        }

        private const float brickColorMinValue = 0.6f;
        private const float brickColorMaxValue = 0.8f;

        private const float brickHeight = 0.5f;
        private const float paddleHeight = 0.5f;
        private const float paddleSpeed = 25f;
        private const float ballRadius = 0.5f;
        private const float ballForce = 200;

        private Config config;

        private Transform bricksContainer;
        private Sprite whiteSprite;
        private PhysicsMaterial2D bouncyMaterial;
        private GameObject borders;
        private GameObject paddle;
        private Transform paddleTransform;
        private GameObject ball;
        private Transform ballTransform;
        private Rigidbody2D ballRigidbody;
        private List<Brick> activeBricks = new List<Brick>();
        private Queue<Brick> pool = new Queue<Brick>();

        private Dictionary<BrickSize, float> sizeValues = new Dictionary<BrickSize, float>
        {
            {BrickSize.Narrow, 0.5f},
            {BrickSize.Wide, 1f},
        };

        public Breakout()
        {
            bricksContainer = new GameObject("Bricks").transform;

            // Generate the sprite used for bricks, the paddle and the ball
            var texture = Texture2D.whiteTexture;
            whiteSprite = Sprite.Create(texture,
                rect: new Rect(0, 0, texture.width, texture.width),
                pivot: new Vector2(0.5f, 0.5f),
                pixelsPerUnit: texture.width);

            // Bouncy material for everything to prevent the ball from stopping
            bouncyMaterial = new PhysicsMaterial2D {name = "Bouncy", bounciness = 1, friction = 0};
        }

        public void Generate(Config config)
        {
            Assert.IsTrue(config.wallWidth > 0);
            Assert.IsTrue(config.wallHeight > 0);
            Assert.IsTrue(config.paddleWidth > 0);
            Assert.IsTrue(config.ballSize > 0);

            this.config = config;
            ResetLevel();
        }

        public void Update()
        {
            float delta = Input.GetAxis("Horizontal")*Time.deltaTime*paddleSpeed;
            paddleTransform.position += new Vector3(delta, 0);

            // Prevent the paddle from penetrating walls
            float halfWall = (config.wallWidth - 1)/2f;
            if (paddleTransform.position.x > halfWall)
            {
                paddleTransform.position = new Vector3(halfWall, 0);
            }
            if (paddleTransform.position.x < -halfWall)
            {
                paddleTransform.position = new Vector3(-halfWall, 0);
            }

            // The ball should move with a constant velocity
            ballRigidbody.linearVelocity = ballRigidbody.linearVelocity.normalized*config.ballVelocityMagnitude;

            if (ballTransform.position.y < -0.1f || activeBricks.Count == 0)
            {
                ResetLevel();
            }

            float angle = Vector2.Angle(ballRigidbody.linearVelocity, Vector2.right);
            if (angle < 30 || angle > 150)
            {
                // Prevent the ball from bouncing between walls
                KickBallInRandomDirection();
            }
        }

        private void ResetLevel()
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
            float bordersHeight = config.wallHeightOffset + config.wallHeight/2 + 1 + config.ballSize;
            float bordersWidth = config.wallWidth + 1;

            // Bottom
            CreateBoxCollider(offset: new Vector2(0, -1 - config.ballSize/2),
                size: new Vector2(bordersWidth, 1));
            // Left
            CreateBoxCollider(offset: new Vector2(-bordersWidth/2f, bordersHeight/2f - 0.5f - config.ballSize/2),
                size: new Vector2(1, bordersHeight + 1));
            // Right
            CreateBoxCollider(offset: new Vector2(bordersWidth/2f, bordersHeight/2f - 0.5f - config.ballSize/2),
                size: new Vector2(1, bordersHeight + 1));
            // Top
            CreateBoxCollider(offset: new Vector2(0, bordersHeight - config.ballSize/2),
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
            // Return all active bricks to the pool
            foreach (var brick in activeBricks)
            {
                ReturnBrickToPool(brick);
            }
            activeBricks.Clear();

            for (int y = 0; y < config.wallHeight; y++)
            {
                // Select a color for the current line
                var currentColor = new ColorHSV(config.gradient.Evaluate(y/(config.wallHeight - 1f)));

                // Generate brick sizes for the current line
                List<BrickSize> brickSizes = FillWallWithBricks(config.wallWidth);

                Vector3 leftEdge = Vector3.left*config.wallWidth/2 +
                                   Vector3.up*(config.wallHeightOffset + y*brickHeight);
                foreach (var brickSize in brickSizes)
                {
                    var position = leftEdge + Vector3.right*sizeValues[brickSize]/2;

                    // Randomize the tint of the current brick
                    float colorValue = Random.Range(brickColorMinValue, brickColorMaxValue);
                    Color color = currentColor.WithV(colorValue).ToColor();

                    var brick = GetBrick();
                    brick.transform.position = position;
                    brick.transform.localScale = new Vector3(sizeValues[brickSize], brickHeight);
                    brick.spriteRenderer.color = color;

                    activeBricks.Add(brick);

                    leftEdge.x += sizeValues[brickSize];
                }
            }
        }

        private List<BrickSize> FillWallWithBricks(float width)
        {
            // https://en.wikipedia.org/wiki/Knapsack_problem
            // We are using knapsack problem solver to fill a fixed width with bricks of random width
            Dictionary<BrickSize, int> knapsack;
            float knapsackWidth;
            do
            {
                // Prefill the knapsack to get a nicer distribution of widths
                knapsack = GetRandomKnapsack(width);
                // Calculate a sum of widths in the knapsack
                knapsackWidth = CalculateKnapsackWidth(knapsack);
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

        private float CalculateKnapsackWidth(Dictionary<BrickSize, int> knapsack)
        {
            float knapsackWidth = 0f;
            foreach (var key in knapsack.Keys)
            {
                knapsackWidth += knapsack[key]*sizeValues[key];
            }
            return knapsackWidth;
        }

        private Brick GetBrick()
        {
            Brick brick;
            if (pool.Count > 0)
            {
                brick = pool.Dequeue();
                brick.gameObject.SetActive(true);
            }
            else
            {
                brick = GenerateBrick();
                brick.onHit += () =>
                {
                    activeBricks.Remove(brick);
                    ReturnBrickToPool(brick);
                };
            }
            return brick;
        }

        private void ReturnBrickToPool(Brick brick)
        {
            brick.gameObject.SetActive(false);
            pool.Enqueue(brick);
        }

        private Brick GenerateBrick()
        {
            var go = new GameObject("Brick");
            go.transform.parent = bricksContainer;

            var brick = go.AddComponent<Brick>();

            brick.spriteRenderer = go.AddComponent<SpriteRenderer>();
            brick.spriteRenderer.sprite = whiteSprite;

            var brickCollider = go.AddComponent<BoxCollider2D>();
            brickCollider.sharedMaterial = bouncyMaterial;
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
            paddleTransform.localScale = new Vector3(config.paddleWidth, paddleHeight);
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
            ballTransform.localScale = new Vector3(config.ballSize, config.ballSize);
            ballRigidbody.linearVelocity = Vector2.zero;
            KickBallInRandomDirection();
        }

        private void KickBallInRandomDirection()
        {
            Vector2 direction = Random.Range(-0.5f, 0.5f)*Vector2.right + Vector2.up;
            ballRigidbody.AddForce(direction*ballForce);
        }
    }
}
