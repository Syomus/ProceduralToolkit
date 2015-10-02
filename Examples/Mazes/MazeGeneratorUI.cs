using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ProceduralToolkit.Examples.UI
{
    public class MazeGeneratorUI : UIBase
    {
        public RectTransform leftPanel;
        public ToggleGroup algorithmsGroup;
        public RawImage mazeImage;

        private Texture2D texture;
        private int textureWidth = 256;
        private int textureHeight = 256;
        private bool useRainbowGradient = true;
        private MazeGenerator mazeGenerator;
        private MazeGenerator.Algorithm generatorAlgorithm = MazeGenerator.Algorithm.RandomTraversal;
        private int cellSize = 2;
        private int wallSize = 1;

        private MazeGenerator.Algorithm[] algorithms = new[]
        {
            MazeGenerator.Algorithm.None,
            MazeGenerator.Algorithm.RandomTraversal,
            MazeGenerator.Algorithm.RandomDepthFirstTraversal,
            MazeGenerator.Algorithm.RandomBreadthFirstTraversal,
        };

        private Dictionary<MazeGenerator.Algorithm, string> algorithmToString =
            new Dictionary<MazeGenerator.Algorithm, string>
            {
                {MazeGenerator.Algorithm.None, "None"},
                {MazeGenerator.Algorithm.RandomTraversal, "Random traversal"},
                {MazeGenerator.Algorithm.RandomDepthFirstTraversal, "Random depth-first traversal"},
                {MazeGenerator.Algorithm.RandomBreadthFirstTraversal, "Random breadth-first traversal"}
            };

        private void Awake()
        {
            var header = InstantiateControl<TextControl>(algorithmsGroup.transform.parent);
            header.Initialize("Generator algorithm");
            header.transform.SetAsFirstSibling();
            foreach (MazeGenerator.Algorithm algorithm in algorithms)
            {
                var toggle = InstantiateControl<ToggleControl>(algorithmsGroup.transform);
                toggle.Initialize(
                    header: algorithmToString[algorithm],
                    value: algorithm == generatorAlgorithm,
                    onValueChanged: isOn =>
                    {
                        if (isOn)
                        {
                            generatorAlgorithm = algorithm;
                        }
                    },
                    toggleGroup: algorithmsGroup);
            }

            var cellSizeSlider = InstantiateControl<SliderControl>(leftPanel);
            cellSizeSlider.Initialize("Cell size", 1, 10, cellSize, value => cellSize = value);

            var wallSizeSlider = InstantiateControl<SliderControl>(leftPanel);
            wallSizeSlider.Initialize("Wall size", 1, 10, wallSize, value => wallSize = value);

            var useRainbowGradientToggle = InstantiateControl<ToggleControl>(leftPanel);
            useRainbowGradientToggle.Initialize("Use rainbow gradient", useRainbowGradient,
                value => useRainbowGradient = value);

            var generateButton = InstantiateControl<ButtonControl>(leftPanel);
            generateButton.Initialize("Generate new maze", Generate);

            Generate();
        }

        private void Generate()
        {
            StopAllCoroutines();

            texture = new Texture2D(textureWidth, textureHeight, TextureFormat.ARGB32, false, true)
            {
                filterMode = FilterMode.Point
            };
            texture.Clear(Color.black);
            texture.Apply();
            mazeImage.texture = texture;

            mazeGenerator = new MazeGenerator(textureWidth, textureHeight, cellSize, wallSize);

            StartCoroutine(GenerateCoroutine());
        }

        private IEnumerator GenerateCoroutine()
        {
            var algorithm = generatorAlgorithm;
            if (algorithm == MazeGenerator.Algorithm.None)
            {
                algorithm = algorithms.GetRandom();
            }

            switch (algorithm)
            {
                case MazeGenerator.Algorithm.RandomTraversal:
                    yield return StartCoroutine(mazeGenerator.RandomTraversal(DrawEdge, texture.Apply));
                    break;
                case MazeGenerator.Algorithm.RandomDepthFirstTraversal:
                    yield return StartCoroutine(mazeGenerator.RandomDepthFirstTraversal(DrawEdge, texture.Apply));
                    break;
                case MazeGenerator.Algorithm.RandomBreadthFirstTraversal:
                    yield return StartCoroutine(mazeGenerator.RandomBreadthFirstTraversal(DrawEdge, texture.Apply));
                    break;
            }
            texture.Apply();
        }

        private void DrawEdge(Edge edge)
        {
            int x, y, width, height;
            if (edge.origin.direction == Directions.Left || edge.origin.direction == Directions.Down)
            {
                x = Translate(edge.exit.x);
                y = Translate(edge.exit.y);
            }
            else
            {
                x = Translate(edge.origin.x);
                y = Translate(edge.origin.y);
            }

            if (edge.origin.direction == Directions.Left || edge.origin.direction == Directions.Right)
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
            if (useRainbowGradient)
            {
                float hue = Mathf.Repeat(edge.origin.depth/360f, 1);
                color = ColorE.HSVToRGB(hue, 1, 1);
            }
            else
            {
                color = Color.white;
            }
            texture.DrawRect(x, y, width, height, color);
        }

        private int Translate(int x)
        {
            return wallSize + x*(cellSize + wallSize);
        }
    }
}