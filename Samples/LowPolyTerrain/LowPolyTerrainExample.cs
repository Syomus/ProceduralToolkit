using ProceduralToolkit.Samples.UI;
using UnityEngine;

namespace ProceduralToolkit.Samples
{
    /// <summary>
    /// Configurator for LowPolyTerrainGenerator with UI and editor controls
    /// </summary>
    public class LowPolyTerrainExample : ConfiguratorBase
    {
        public MeshFilter terrainMeshFilter;
        public MeshCollider terrainMeshCollider;
        public RectTransform leftPanel;
        public bool constantSeed = false;
        public LowPolyTerrainGenerator.Config config = new LowPolyTerrainGenerator.Config();

        private const int minYSize = 1;
        private const int maxYSize = 10;
        private const float minCellSize = 0.3f;
        private const float maxCellSize = 1;
        private const int minNoiseFrequency = 1;
        private const int maxNoiseFrequency = 8;

        private Mesh terrainMesh;

        private void Awake()
        {
            Generate();
            SetupSkyboxAndPalette();

            InstantiateControl<SliderControl>(leftPanel)
                .Initialize("Terrain height", minYSize, maxYSize, (int) config.terrainSize.y, value =>
                {
                    config.terrainSize.y = value;
                    Generate();
                });

            InstantiateControl<SliderControl>(leftPanel)
                .Initialize("Cell size", minCellSize, maxCellSize, config.cellSize, value =>
                {
                    config.cellSize = value;
                    Generate();
                });

            InstantiateControl<SliderControl>(leftPanel)
                .Initialize("Noise frequency", minNoiseFrequency, maxNoiseFrequency, (int) config.noiseFrequency, value =>
                {
                    config.noiseFrequency = value;
                    Generate();
                });

            InstantiateControl<ButtonControl>(leftPanel).Initialize("Generate", () => Generate());
        }

        private void Update()
        {
            UpdateSkybox();
        }

        public void Generate(bool randomizeConfig = true)
        {
            if (constantSeed)
            {
                Random.InitState(0);
            }

            if (randomizeConfig)
            {
                GeneratePalette();

                config.gradient = ColorE.Gradient(from: GetMainColorHSV(), to: GetSecondaryColorHSV());
            }

            var draft = LowPolyTerrainGenerator.TerrainDraft(config);
            draft.Move(Vector3.left*config.terrainSize.x/2 + Vector3.back*config.terrainSize.z/2);
            AssignDraftToMeshFilter(draft, terrainMeshFilter, ref terrainMesh);
            terrainMeshCollider.sharedMesh = terrainMesh;
        }
    }
}
