using UnityEngine;

namespace ProceduralToolkit.Examples.UI
{
    public class ChairGeneratorUI : UIBase
    {
        public MeshFilter meshFilter;
        public RectTransform leftPanel;

        private float legWidth = 0.07f;
        private float legHeight = 0.7f;
        private Vector3 seatLB = new Vector3(0.7f, 0.05f, 0.7f);
        private Vector3 seatUB = new Vector3(1, 0.2f, 0.9f);
        private float backHeight = 0.8f;
        private bool hasStretchers = true;
        private bool hasArmrests = false;

        private void Awake()
        {
            Generate();

            var legWidthSlider = InstantiateControl<SliderControl>(leftPanel);
            legWidthSlider.Initialize("Leg width", 0.05f, 0.12f, legWidth, value => legWidth = value);
            var legHeightSlider = InstantiateControl<SliderControl>(leftPanel);
            legHeightSlider.Initialize("Leg height", 0.5f, 1.2f, legHeight, value => legHeight = value);
            var backHeightSlider = InstantiateControl<SliderControl>(leftPanel);
            backHeightSlider.Initialize("Back height", 0.5f, 1.3f, backHeight, value => backHeight = value);

            var hasStretchersToggle = InstantiateControl<ToggleControl>(leftPanel);
            hasStretchersToggle.Initialize("Has stretchers", hasStretchers, value => hasStretchers = value);
            var hasArmrestsToggle = InstantiateControl<ToggleControl>(leftPanel);
            hasArmrestsToggle.Initialize("Has armrests", hasArmrests, value => hasArmrests = value);

            var generateButton = InstantiateControl<ButtonControl>(leftPanel);
            generateButton.Initialize("Generate", Generate);
        }

        private void Generate()
        {
            var draft = ChairGenerator.Chair(legWidth, legHeight, RandomE.Range(seatLB, seatUB), backHeight,
                hasStretchers, hasArmrests);
            meshFilter.mesh = draft.ToMesh();
        }
    }
}