using UnityEngine;

namespace ProceduralToolkit.Examples.UI
{
    public class KhrushchyovkaUI : UIBase
    {
        public MeshFilter meshFilter;
        public RectTransform leftPanel;

        private int width = 10;
        private int length = 50;
        private int floorCount = 5;
        private bool hasAttic = true;

        private void Awake()
        {
            Generate();

            var widthSlider = InstantiateControl<SliderControl>(leftPanel);
            widthSlider.Initialize("Width", 10, 30, width, value =>
            {
                width = value;
                Generate();
            });

            var lengthSlider = InstantiateControl<SliderControl>(leftPanel);
            lengthSlider.Initialize("Length", 10, 60, length, value =>
            {
                length = value;
                Generate();
            });

            var floorCountSlider = InstantiateControl<SliderControl>(leftPanel);
            floorCountSlider.Initialize("Floor count", 1, 10, floorCount, value =>
            {
                floorCount = value;
                Generate();
            });

            var hasAtticToggle = InstantiateControl<ToggleControl>(leftPanel);
            hasAtticToggle.Initialize("Has attic", hasAttic, value =>
            {
                hasAttic = value;
                Generate();
            });

            var generateButton = InstantiateControl<ButtonControl>(leftPanel);
            generateButton.Initialize("Generate", Generate);
        }

        private void Generate()
        {
            var draft = Khrushchyovka.KhrushchyovkaDraft(width, length, floorCount, hasAttic);
            meshFilter.mesh = draft.ToMesh();
        }
    }
}