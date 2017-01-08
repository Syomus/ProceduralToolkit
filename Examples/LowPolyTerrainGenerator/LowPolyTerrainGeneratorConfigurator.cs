using System.Collections.Generic;
using UnityEngine;

namespace ProceduralToolkit.Examples.UI
{
    public class LowPolyTerrainGeneratorConfigurator : UIBase
    {
        public MeshFilter meshFilter;
        public RectTransform leftPanel;
        [Space]
        public LowPolyTerrainGenerator.Config config;

        private const int minXSize = 10;
        private const int maxXSize = 30;
        private const int minYSize = 1;
        private const int maxYSize = 5;
        private const int minZSize = 10;
        private const int maxZSize = 30;
        private const float minCellSize = 0.3f;
        private const float maxCellSize = 2;
        private const int minNoiseScale = 1;
        private const int maxNoiseScale = 20;

        private List<ColorHSV> targetPalette = new List<ColorHSV>();
        private List<ColorHSV> currentPalette = new List<ColorHSV>();

        private void Awake()
        {
            RenderSettings.skybox = new Material(RenderSettings.skybox);

            Generate();
            currentPalette.AddRange(targetPalette);

            InstantiateControl<SliderControl>(leftPanel)
                .Initialize("Terrain size X", minXSize, maxXSize, (int) config.terrainSize.x, value =>
                {
                    config.terrainSize.x = value;
                    Generate();
                });

            InstantiateControl<SliderControl>(leftPanel)
                .Initialize("Terrain size Y", minYSize, maxYSize, (int) config.terrainSize.y, value =>
                {
                    config.terrainSize.y = value;
                    Generate();
                });

            InstantiateControl<SliderControl>(leftPanel)
                .Initialize("Terrain size Z", minZSize, maxZSize, (int) config.terrainSize.z, value =>
                {
                    config.terrainSize.z = value;
                    Generate();
                });

            InstantiateControl<SliderControl>(leftPanel)
                .Initialize("Cell size", minCellSize, maxCellSize, config.cellSize, value =>
                {
                    config.cellSize = value;
                    Generate();
                });

            InstantiateControl<SliderControl>(leftPanel)
                .Initialize("Noise scale", minNoiseScale, maxNoiseScale, (int) config.noiseScale, value =>
                {
                    config.noiseScale = value;
                    Generate();
                });

            InstantiateControl<ButtonControl>(leftPanel).Initialize("Generate", Generate);
        }

        private void Update()
        {
            SkyBoxGenerator.LerpSkybox(RenderSettings.skybox, currentPalette, targetPalette, 0, 1, 4, Time.deltaTime);
        }

        public void Generate()
        {
            targetPalette = RandomE.TetradicPalette(0.25f, 0.75f);
            targetPalette.Add(ColorHSV.Lerp(targetPalette[0], targetPalette[1], 0.5f));

            config.gradient = ColorE.Gradient(from: targetPalette[2].WithSV(0.8f, 0.8f),
                to: targetPalette[3].WithSV(0.8f, 0.8f));

            var draft = LowPolyTerrainGenerator.TerrainDraft(config);
            draft.Move(Vector3.left*config.terrainSize.x/2 + Vector3.back*config.terrainSize.z/2);
            meshFilter.mesh = draft.ToMesh();
        }
    }
}