using UnityEngine;

namespace ProceduralToolkit.Examples.UI
{
    public class ChairGeneratorUI : UIBase
    {
        public MeshFilter meshFilter;
        public RectTransform leftPanel;

        private float legWidth = 0.07f;
        private float legHeight = 0.7f;
        private float seatWidth = 0.7f;
        private float seatDepth = 0.7f;
        private float seatHeight = 0.05f;
        private float backHeight = 0.8f;
        private bool hasStretchers = true;
        private bool hasArmrests = false;

        private void Awake()
        {
            Generate();

            InstantiateControl<SliderControl>(leftPanel).Initialize("Leg width", 0.05f, 0.12f, legWidth, value =>
            {
                legWidth = value;
                Generate();
            });
            InstantiateControl<SliderControl>(leftPanel).Initialize("Leg height", 0.5f, 1.2f, legHeight, value =>
            {
                legHeight = value;
                Generate();
            });

            InstantiateControl<SliderControl>(leftPanel).Initialize("Seat width", 0.5f, 1.2f, seatWidth, value =>
            {
                seatWidth = value;
                Generate();
            });
            InstantiateControl<SliderControl>(leftPanel).Initialize("Seat depth", 0.3f, 1.2f, seatDepth, value =>
            {
                seatDepth = value;
                Generate();
            });
            InstantiateControl<SliderControl>(leftPanel).Initialize("Seat height", 0.03f, 0.2f, seatHeight, value =>
            {
                seatHeight = value;
                Generate();
            });

            InstantiateControl<SliderControl>(leftPanel).Initialize("Back height", 0.5f, 1.3f, backHeight, value =>
            {
                backHeight = value;
                Generate();
            });

            InstantiateControl<ToggleControl>(leftPanel).Initialize("Has stretchers", hasStretchers, value =>
            {
                hasStretchers = value;
                Generate();
            });
            InstantiateControl<ToggleControl>(leftPanel).Initialize("Has armrests", hasArmrests, value =>
            {
                hasArmrests = value;
                Generate();
            });

            InstantiateControl<ButtonControl>(leftPanel).Initialize("Generate", Generate);
        }

        private void Generate()
        {
            var draft = ChairGenerator.Chair(legWidth, legHeight, seatWidth, seatDepth, seatHeight, backHeight,
                hasStretchers, hasArmrests);
            meshFilter.mesh = draft.ToMesh();
        }
    }
}