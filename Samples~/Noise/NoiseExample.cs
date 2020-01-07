using ProceduralToolkit.FastNoiseLib;
using ProceduralToolkit.Samples.UI;
using UnityEngine;
using UnityEngine.UI;

namespace ProceduralToolkit.Samples
{
    public class NoiseExample : ConfiguratorBase
    {
        public RectTransform leftPanel;
        public ToggleGroup toggleGroup;
        public RawImage image;

        private const int width = 512;
        private const int height = 512;

        private Color[] pixels;
        private Texture2D texture;
        private FastNoise noise;
        private TextControl header;
        private FastNoise.NoiseType currentNoiseType = FastNoise.NoiseType.Perlin;

        private void Awake()
        {
            pixels = new Color[width*height];
            texture = new Texture2D(width, height, TextureFormat.ARGB32, false, true)
            {
                filterMode = FilterMode.Point
            };
            texture.Clear(Color.clear);
            texture.Apply();
            image.texture = texture;

            header = InstantiateControl<TextControl>(leftPanel);
            header.transform.SetAsFirstSibling();
            header.Initialize("Noise type:");

            InstantiateToggle(FastNoise.NoiseType.Perlin);
            InstantiateToggle(FastNoise.NoiseType.PerlinFractal);
            InstantiateToggle(FastNoise.NoiseType.Simplex);
            InstantiateToggle(FastNoise.NoiseType.SimplexFractal);
            InstantiateToggle(FastNoise.NoiseType.Cubic);
            InstantiateToggle(FastNoise.NoiseType.CubicFractal);
            InstantiateToggle(FastNoise.NoiseType.Value);
            InstantiateToggle(FastNoise.NoiseType.ValueFractal);
            InstantiateToggle(FastNoise.NoiseType.Cellular);
            InstantiateToggle(FastNoise.NoiseType.WhiteNoise);

            noise = new FastNoise();
            Generate();
            SetupSkyboxAndPalette();
        }

        private void Update()
        {
            UpdateSkybox();
        }

        private void Generate()
        {
            noise.SetNoiseType(currentNoiseType);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    float value = noise.GetNoise(x, y);
                    value = value*0.5f + 0.5f;
                    pixels[y*width + x] = new Color(value, value, value);
                }
            }

            texture.SetPixels(pixels);
            texture.Apply();

            GeneratePalette();
        }

        private void InstantiateToggle(FastNoise.NoiseType noiseType)
        {
            var toggle = InstantiateControl<ToggleControl>(toggleGroup.transform);
            toggle.Initialize(
                header: noiseType.ToString(),
                value: noiseType == currentNoiseType,
                onValueChanged: isOn =>
                {
                    if (isOn)
                    {
                        currentNoiseType = noiseType;
                        Generate();
                    }
                },
                toggleGroup: toggleGroup);
        }
    }
}
