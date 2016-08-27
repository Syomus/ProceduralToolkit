using UnityEngine;

namespace ProceduralToolkit.Examples.UI
{
    public class LowPolyTerrainGeneratorUI : UIBase
    {
        public MeshFilter meshFilter;
        public RectTransform leftPanel;

        [Space]
        [Range(minXSize, maxXSize)]
        public int terrainSizeX = 10;
        [Range(minYSize, maxYSize)]
        public int terrainSizeY = 1;
        [Range(minZSize, maxZSize)]
        public int terrainSizeZ = 10;
        [Range(minCellSize, maxCellSize)]
        public float cellSize = 1;
        [Range(minNoiseScale, maxNoiseScale)]
        public int noiseScale = 5;

        private const int minXSize = 1;
        private const int maxXSize = 20;
        private const int minYSize = 1;
        private const int maxYSize = 5;
        private const int minZSize = 1;
        private const int maxZSize = 20;
        private const float minCellSize = 0.2f;
        private const float maxCellSize = 2;
        private const int minNoiseScale = 1;
        private const int maxNoiseScale = 20;

        private void Awake()
        {
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

            Generate();
        }

        public void Generate()
        {
            Vector3 terrainSize = new Vector3(terrainSizeX, terrainSizeY, terrainSizeZ);
            var draft = LowPolyTerrainGenerator.TerrainDraft(terrainSize, cellSize, noiseScale);
            draft.Move(Vector3.left*terrainSizeX/2 + Vector3.back*terrainSizeZ/2);
            meshFilter.mesh = draft.ToMesh();
        }
    }
}