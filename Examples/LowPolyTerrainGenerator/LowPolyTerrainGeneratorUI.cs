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
            RenderSettings.skybox = new Material(RenderSettings.skybox);

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

            var palette = new ColorHSV(Random.value, 0.5f, 0.75f).GetTetradicPalette();

            RenderSettings.skybox.SetColor("_SkyColor", palette[0].ToColor());
            RenderSettings.skybox.SetColor("_HorizonColor", ColorHSV.Lerp(palette[0], palette[1], 0.5f).ToColor());
            RenderSettings.skybox.SetColor("_GroundColor", palette[1].ToColor());

            var gradient = ColorE.Gradient(palette[2].WithS(0.8f).WithV(0.8f).ToColor(),
                palette[3].WithS(0.8f).WithV(0.8f).ToColor());

            var draft = LowPolyTerrainGenerator.TerrainDraft(terrainSize, cellSize, noiseScale, gradient);
            draft.Move(Vector3.left*terrainSizeX/2 + Vector3.back*terrainSizeZ/2);
            meshFilter.mesh = draft.ToMesh();
        }
    }
}