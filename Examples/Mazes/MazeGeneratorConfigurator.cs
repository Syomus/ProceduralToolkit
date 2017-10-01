using System.Collections;
using ProceduralToolkit.Examples.UI;
using UnityEngine;
using UnityEngine.UI;

namespace ProceduralToolkit.Examples
{
    public class MazeGeneratorConfigurator : ConfiguratorBase
    {
        public RectTransform leftPanel;
        public ToggleGroup algorithmsGroup;
        public RawImage mazeImage;
        [Space]
        public MazeGenerator.Config config = new MazeGenerator.Config();
        public bool useGradient = true;

        private const int cellSize = 2;
        private const int wallSize = 1;
        private const float gradientSaturation = 0.7f;
        private const float gradientSaturationOffset = 0.1f;
        private const float gradientValue = 0.7f;
        private const float gradientValueOffset = 0.1f;
        private const float gradientLength = 30;

        private Texture2D texture;
        private MazeGenerator mazeGenerator;
        private ColorHSV mainColor;

        private void Awake()
        {
            config.drawEdge = DrawEdge;

            int textureWidth = wallSize + config.width*(cellSize + wallSize);
            int textureHeight = wallSize + config.height*(cellSize + wallSize);
            texture = new Texture2D(textureWidth, textureHeight, TextureFormat.ARGB32, false, true)
            {
                filterMode = FilterMode.Point
            };
            mazeImage.texture = texture;

            var header = InstantiateControl<TextControl>(algorithmsGroup.transform.parent);
            header.Initialize("Generator algorithm");
            header.transform.SetAsFirstSibling();

            InstantiateToggle(MazeGenerator.Algorithm.RandomTraversal, "Random traversal");
            InstantiateToggle(MazeGenerator.Algorithm.RandomDepthFirstTraversal, "Random depth-first traversal");
            InstantiateToggle(MazeGenerator.Algorithm.RandomBreadthFirstTraversal, "Random breadth-first traversal");

            InstantiateControl<ToggleControl>(leftPanel).Initialize("Use gradient", useGradient, value =>
            {
                useGradient = value;
                Generate();
            });

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
            StopAllCoroutines();

            texture.Clear(Color.black);
            texture.Apply();

            mazeGenerator = new MazeGenerator(config);

            GeneratePalette();
            mainColor = GetMainColorHSV();

            StartCoroutine(GenerateCoroutine());
        }

        private IEnumerator GenerateCoroutine()
        {
            while (mazeGenerator.Generate(steps: 200))
            {
                texture.Apply();
                yield return null;
            }
        }

        private void DrawEdge(Maze.Edge edge)
        {
            Vector2Int position = ToTexturePosition(new Vector2Int(
                x: Mathf.Min(edge.origin.position.x, edge.exit.position.x),
                y: Mathf.Min(edge.origin.position.y, edge.exit.position.y)));

            int width;
            int height;
            if ((edge.exit.position - edge.origin.position).y == 0)
            {
                width = cellSize*2 + wallSize;
                height = cellSize;
            }
            else
            {
                width = cellSize;
                height = cellSize*2 + wallSize;
            }

            Color color;
            if (useGradient)
            {
                float gradient01 = Mathf.Repeat(edge.origin.depth/gradientLength, 1);
                float gradient010 = Mathf.Abs((gradient01 - 0.5f)*2);

                color = GetColor(gradient010);
            }
            else
            {
                color = GetColor(0.75f);
            }
            texture.DrawRect(position.x, position.y, width, height, color);
        }

        private Vector2Int ToTexturePosition(Vector2Int position)
        {
            return new Vector2Int(
                x: wallSize + position.x*(cellSize + wallSize),
                y: wallSize + position.y*(cellSize + wallSize));
        }

        private Color GetColor(float gradientPosition)
        {
            float saturation = gradientPosition*gradientSaturation + gradientSaturationOffset;
            float value = gradientPosition*gradientValue + gradientValueOffset;
            return mainColor.WithSV(saturation, value).ToColor();
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