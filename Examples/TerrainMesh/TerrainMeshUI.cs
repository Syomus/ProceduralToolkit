using UnityEngine;

namespace ProceduralToolkit.Examples.UI
{
    public class TerrainMeshUI : UIBase
    {
        public MeshFilter meshFilter;
        public RectTransform leftPanel;

        private int xSize = 10;
        private int zSize = 10;
        private int xSegments = 100;
        private int zSegments = 100;
        private int noiseScale = 10;

        private void Awake()
        {
            InstantiateControl<SliderControl>(leftPanel).Initialize("X size", 1, 20, xSize, value =>
            {
                xSize = value;
                Generate();
            });

            InstantiateControl<SliderControl>(leftPanel).Initialize("Z size", 1, 20, zSize, value =>
            {
                zSize = value;
                Generate();
            });

            InstantiateControl<SliderControl>(leftPanel).Initialize("X segments", 1, 200, xSegments, value =>
            {
                xSegments = value;
                Generate();
            });

            InstantiateControl<SliderControl>(leftPanel).Initialize("Z segments", 1, 200, zSegments, value =>
            {
                zSegments = value;
                Generate();
            });

            InstantiateControl<SliderControl>(leftPanel).Initialize("Noise scale", 1, 200, noiseScale, value =>
            {
                noiseScale = value;
                Generate();
            });

            InstantiateControl<ButtonControl>(leftPanel).Initialize("Generate", Generate);

            Generate();
        }

        private void Generate()
        {
            var mesh = TerrainMesh.TerrainDraft(xSize, zSize, xSegments, zSegments, noiseScale).ToMesh();
            meshFilter.mesh = mesh;
        }
    }
}