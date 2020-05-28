using ProceduralToolkit.Samples.UI;
using UnityEngine;
using UnityEngine.UI;
using Unity.Jobs;
using URandom = UnityEngine.Random;
using MRandom = Unity.Mathematics.Random;

namespace ProceduralToolkit.Samples
{
    /// <summary>
    /// Configurator for MazeGenerator with UI controls
    /// </summary>
    public class MazeGeneratorConfigurator : ConfiguratorBase
    {
        public RectTransform leftPanel;
        public ToggleGroup algorithmsGroup;
        public RawImage mazeImage;
        [Space]
        public MazeJob.Config config = new MazeJob.Config
        {
            width = 32,
            height = 32,
            algorithm = MazeJob.Algorithm.RandomTraversal,
        };

        private const int roomSize = 2;
        private const int wallSize = 1;
        private const float saturation = 0.8f;
        private const float value = 0.8f;

        private Texture2D texture;
        private MazeJob mazeJob;

        private void Awake()
        {
            int textureWidth = GetTextureWidth(config.width, wallSize, roomSize);
            int textureHeight = GetTextureHeight(config.height, wallSize, roomSize);
            texture = PTUtils.CreateTexture(textureWidth, textureHeight, Color.black);
            mazeImage.texture = texture;

            var header = InstantiateControl<TextControl>(algorithmsGroup.transform.parent);
            header.Initialize("Generator algorithm");
            header.transform.SetAsFirstSibling();

            InstantiateToggle(MazeJob.Algorithm.RandomTraversal, "Random traversal");
            InstantiateToggle(MazeJob.Algorithm.RandomDepthFirstTraversal, "Random depth-first traversal");

            InstantiateControl<ButtonControl>(leftPanel).Initialize("Generate new maze", Generate);

            Generate();
            SetupSkyboxAndPalette();
        }

        private void Update()
        {
            UpdateSkybox();
        }

        private void Generate()
        {
            var random = new MRandom((uint) URandom.Range(0, int.MaxValue));

            mazeJob = new MazeJob(config, ref random);
            var handle = mazeJob.Schedule();
            handle.Complete();

            GeneratePalette();
            var color = GetMainColorHSV().WithSV(saturation, value).ToColor();

            texture.Clear(Color.black);
            for (int x = 0; x < config.width; x++)
            {
                for (int y = 0; y < config.height; y++)
                {
                    var position = new Vector2Int(x, y);
                    Directions vertex = mazeJob.maze[position];
                    if (vertex.HasFlag(Directions.Right))
                    {
                        DrawConnection(position, new Vector2Int(x + 1, y), color);
                    }
                    if (vertex.HasFlag(Directions.Up))
                    {
                        DrawConnection(position, new Vector2Int(x, y + 1), color);
                    }
                }
            }
            texture.Apply();

            mazeJob.maze.vertices.Dispose();
        }

        private void DrawConnection(Vector2Int a, Vector2Int b, Color color)
        {
            var rect = ConnectionToRect(a, b, wallSize, roomSize);
            texture.DrawRect(rect, color);
        }

        private void InstantiateToggle(MazeJob.Algorithm algorithm, string header)
        {
            var toggle = InstantiateControl<ToggleControl>(algorithmsGroup.transform);
            toggle.Initialize(
                header: header,
                value: algorithm == config.algorithm,
                onValueChanged: isOn =>
                {
                    if (isOn)
                    {
                        config.algorithm = algorithm;
                        Generate();
                    }
                },
                toggleGroup: algorithmsGroup);
        }

        private static int GetTextureWidth(int mazeWidth, int wallSize, int roomSize)
        {
            return wallSize + mazeWidth*(roomSize + wallSize);
        }

        private static int GetTextureHeight(int mazeHeight, int wallSize, int roomSize)
        {
            return wallSize + mazeHeight*(roomSize + wallSize);
        }

        private static RectInt ConnectionToRect(Vector2Int a, Vector2Int b, int wallSize, int roomSize)
        {
            var rect = new RectInt
            {
                min = new Vector2Int(
                    x: wallSize + Mathf.Min(a.x, b.x)*(roomSize + wallSize),
                    y: wallSize + Mathf.Min(a.y, b.y)*(roomSize + wallSize))
            };

            if ((b - a).y == 0)
            {
                rect.width = roomSize*2 + wallSize;
                rect.height = roomSize;
            }
            else
            {
                rect.width = roomSize;
                rect.height = roomSize*2 + wallSize;
            }
            return rect;
        }
    }
}
