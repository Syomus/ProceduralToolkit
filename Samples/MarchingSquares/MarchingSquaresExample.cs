using ProceduralToolkit.FastNoiseLib;
using ProceduralToolkit.MarchingSquares;
using ProceduralToolkit.Samples.UI;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.UI;

namespace ProceduralToolkit.Samples
{
    public class MarchingSquaresExample : ConfiguratorBase
    {
        public MeshFilter meshFilter;
        public RectTransform leftPanel;
        public RawImage image;
        [Space]
        [Range(0, 1)]
        public float threshold = 0.5f;
        public bool animate = true;

        private const int textureSize = 128;
        private const float cellSize = 0.5f;
        private const float animationSpeed = 10;

        private Texture2D texture;
        private Color[] pixels;
        private MeshDraft draft;
        private Mesh mesh;
        private MarchingSquaresTriangulator triangulator;
        private FastNoise noise;
        private float noiseOffset;

        private void Awake()
        {
            texture = PTUtils.CreateTexture(textureSize, textureSize, Color.clear);
            pixels = texture.GetPixels();
            image.texture = texture;

            var slider = InstantiateControl<SliderControl>(leftPanel.transform);
            slider.Initialize("Threshold", 0, 1, threshold, value =>
            {
                threshold = value;
                Generate();
            });

            var toggle = InstantiateControl<ToggleControl>(leftPanel.transform);
            toggle.Initialize("Animate", animate, isOn => animate = isOn);

            noise = new FastNoise();
            noise.SetNoiseType(FastNoise.NoiseType.SimplexFractal);

            UpdateTexture();
            Generate();
            GeneratePalette();
            SetupSkyboxAndPalette();
        }

        private void Update()
        {
            if (animate)
            {
                noiseOffset += Time.deltaTime*animationSpeed;
                UpdateTexture();
                Generate();
            }
            UpdateSkybox();
        }

        private void UpdateTexture()
        {
            for (int y = 0; y < textureSize; y++)
            {
                for (int x = 0; x < textureSize; x++)
                {
                    float value = noise.GetNoise01(x + noiseOffset, y + noiseOffset);
                    pixels[x + y*textureSize] = new Color(value, value, value);
                }
            }
            texture.SetPixels(pixels);
            texture.Apply();
        }

        private void Generate()
        {
            var data = new ColorData(pixels, textureSize, threshold, true);
            var job = new MarchingSquaresJob<ColorData>(data);
            var handle = job.Schedule();

            handle.Complete();
            data.Dispose();

            if (triangulator == null)
            {
                triangulator = new MarchingSquaresTriangulator(cellSize);
            }
            if (draft == null)
            {
                draft = new MeshDraft();
            }
            triangulator.Triangulate(job.contours, draft);
            job.contours.Dispose();

            AssignDraftToMeshFilter(draft, meshFilter, ref mesh);
        }
    }
}
