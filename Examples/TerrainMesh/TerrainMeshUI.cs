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
            var xSizeSlider = InstantiateControl<SliderControl>(leftPanel);
            xSizeSlider.Initialize("X size", 1, 20, xSize, value => xSize = value);

            var zSizeSlider = InstantiateControl<SliderControl>(leftPanel);
            zSizeSlider.Initialize("Z size", 1, 20, zSize, value => zSize = value);

            var xSegmentsSlider = InstantiateControl<SliderControl>(leftPanel);
            xSegmentsSlider.Initialize("X segments", 1, 200, xSegments, value => xSegments = value);

            var zSegmentsSlider = InstantiateControl<SliderControl>(leftPanel);
            zSegmentsSlider.Initialize("Z segments", 1, 200, zSegments, value => zSegments = value);

            var noiseScaleSlider = InstantiateControl<SliderControl>(leftPanel);
            noiseScaleSlider.Initialize("Noise scale", 1, 200, noiseScale, value => noiseScale = value);

            var generateButton = InstantiateControl<ButtonControl>(leftPanel);
            generateButton.Initialize("Generate", Generate);

            Generate();
        }

        private void Generate()
        {
            var mesh = TerrainMesh.TerrainDraft(xSize, zSize, xSegments, zSegments, noiseScale).ToMesh();
            meshFilter.mesh = mesh;
        }
    }
}