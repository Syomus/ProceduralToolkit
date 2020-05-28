using ProceduralToolkit.Samples.UI;
using UnityEngine;
using UnityEngine.UI;

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
        public MazeGenerator.Config config = new MazeGenerator.Config();

        private const int roomSize = 2;
        private const int wallSize = 1;
        private const float saturation = 0.8f;
        private const float value = 0.8f;

        private Texture2D texture;
        private MazeGenerator mazeGenerator;

        private void Awake()
        {
            int textureWidth = MazeGenerator.GetMapWidth(config.width, wallSize, roomSize);
            int textureHeight = MazeGenerator.GetMapHeight(config.height, wallSize, roomSize);
            texture = PTUtils.CreateTexture(textureWidth, textureHeight, Color.black);
            mazeImage.texture = texture;

            var header = InstantiateControl<TextControl>(algorithmsGroup.transform.parent);
            header.Initialize("Generator algorithm");
            header.transform.SetAsFirstSibling();

            InstantiateToggle(MazeGenerator.Algorithm.RandomTraversal, "Random traversal");
            InstantiateToggle(MazeGenerator.Algorithm.RandomDepthFirstTraversal, "Random depth-first traversal");

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
            mazeGenerator = new MazeGenerator(config);
            var maze = mazeGenerator.Generate();

            GeneratePalette();
            var color = GetMainColorHSV().WithSV(saturation, value).ToColor();

            texture.Clear(Color.black);
            for (int x = 0; x < config.width; x++)
            {
                for (int y = 0; y < config.height; y++)
                {
                    var position = new Vector2Int(x, y);
                    Directions vertex = maze[position];
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
        }

        private void DrawConnection(Vector2Int a, Vector2Int b, Color color)
        {
            var rect = MazeGenerator.ConnectionToRect(a, b, wallSize, roomSize);
            texture.DrawRect(rect, color);
        }

        private void InstantiateToggle(MazeGenerator.Algorithm algorithm, string header)
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
    }
}
