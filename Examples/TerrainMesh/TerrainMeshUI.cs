using UnityEngine;

namespace ProceduralToolkit.Examples.UI
{
    public class TerrainMeshUI : UIBase
    {
        public MeshFilter meshFilter;
        public RectTransform leftPanel;

        [Space]
        [Range(minXSize, maxXSize)]
        public int xSize = 10;
        [Range(minZSize, maxZSize)]
        public int zSize = 10;
        [Range(minXSegments, maxXSegments)]
        public int xSegments = 100;
        [Range(minZSegments, maxZSegments)]
        public int zSegments = 100;
        [Range(minNoiseScale, maxNoiseScale)]
        public int noiseScale = 10;

        private const int minXSize = 1;
        private const int maxXSize = 20;
        private const int minZSize = 1;
        private const int maxZSize = 20;
        private const int minXSegments = 1;
        private const int maxXSegments = 200;
        private const int minZSegments = 1;
        private const int maxZSegments = 200;
        private const int minNoiseScale = 1;
        private const int maxNoiseScale = 200;

        private void Awake()
        {
            InstantiateControl<SliderControl>(leftPanel).Initialize("X size", minXSize, maxXSize, xSize, value =>
            {
                xSize = value;
                Generate();
            });

            InstantiateControl<SliderControl>(leftPanel).Initialize("Z size", minZSize, maxZSize, zSize, value =>
            {
                zSize = value;
                Generate();
            });

            InstantiateControl<SliderControl>(leftPanel)
                .Initialize("X segments", minXSegments, maxXSegments, xSegments, value =>
                {
                    xSegments = value;
                    Generate();
                });

            InstantiateControl<SliderControl>(leftPanel)
                .Initialize("Z segments", minZSegments, maxZSegments, zSegments, value =>
                {
                    zSegments = value;
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
            var mesh = TerrainMesh.TerrainDraft(xSize, zSize, xSegments, zSegments, noiseScale).ToMesh();
            meshFilter.mesh = mesh;
        }
    }
}