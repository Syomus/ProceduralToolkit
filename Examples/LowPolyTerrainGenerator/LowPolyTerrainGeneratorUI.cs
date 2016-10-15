using System.Collections.Generic;
using UnityEngine;

namespace ProceduralToolkit.Examples.UI
{
    public class LowPolyTerrainGeneratorUI : UIBase
    {
        public MeshFilter meshFilter;
        public RectTransform leftPanel;

        [Space]
        [Range(minXSize, maxXSize)]
        public int terrainSizeX = 20;
        [Range(minYSize, maxYSize)]
        public int terrainSizeY = 1;
        [Range(minZSize, maxZSize)]
        public int terrainSizeZ = 20;
        [Range(minCellSize, maxCellSize)]
        public float cellSize = 1;
        [Range(minNoiseScale, maxNoiseScale)]
        public int noiseScale = 5;

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
                .Initialize("Terrain size X", minXSize, maxXSize, terrainSizeX, value =>
                {
                    terrainSizeX = value;
                    Generate();
                });

            InstantiateControl<SliderControl>(leftPanel)
                .Initialize("Terrain size Y", minYSize, maxYSize, terrainSizeY, value =>
                {
                    terrainSizeY = value;
                    Generate();
                });

            InstantiateControl<SliderControl>(leftPanel)
                .Initialize("Terrain size Z", minZSize, maxZSize, terrainSizeZ, value =>
                {
                    terrainSizeZ = value;
                    Generate();
                });

            InstantiateControl<SliderControl>(leftPanel)
                .Initialize("Cell size", minCellSize, maxCellSize, cellSize, value =>
                {
                    cellSize = value;
                    Generate();
                });

            InstantiateControl<SliderControl>(leftPanel)
                .Initialize("Noise scale", minNoiseScale, maxNoiseScale, noiseScale, value =>
                {
                    noiseScale = value;
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
            Vector3 terrainSize = new Vector3(terrainSizeX, terrainSizeY, terrainSizeZ);

            targetPalette = RandomE.TetradicPalette(0.25f, 0.75f);
            targetPalette.Add(ColorHSV.Lerp(targetPalette[0], targetPalette[1], 0.5f));

            var gradient = ColorE.Gradient(from: targetPalette[2].WithSV(0.8f, 0.8f),
                to: targetPalette[3].WithSV(0.8f, 0.8f));

            var draft = LowPolyTerrainGenerator.TerrainDraft(terrainSize, cellSize, noiseScale, gradient);
            draft.Move(Vector3.left*terrainSizeX/2 + Vector3.back*terrainSizeZ/2);
            meshFilter.mesh = draft.ToMesh();
        }
    }
}