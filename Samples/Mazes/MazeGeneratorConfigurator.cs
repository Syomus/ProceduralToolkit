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

        private const float saturation = 0.8f;
        private const float value = 0.8f;

        private Texture2D texture;

        private void Awake()
        {
            int textureWidth = MazeUtility.GetTextureWidth(config.width);
            int textureHeight = MazeUtility.GetTextureHeight(config.height);
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

            var mazeJob = new MazeJob(config, ref random);
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
                        MazeUtility.DrawConnection(position, new Vector2Int(x + 1, y), texture, color);
                    }
                    if (vertex.HasFlag(Directions.Up))
                    {
                        MazeUtility.DrawConnection(position, new Vector2Int(x, y + 1), texture, color);
                    }
                }
            }
            texture.Apply();

            mazeJob.maze.vertices.Dispose();
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
    }
}
