using ProceduralToolkit.Examples.UI;
using UnityEngine;

namespace ProceduralToolkit.Examples.Axe
{
    public class AxeGeneratorConfigurator : ConfiguratorBase
    {
        public MeshFilter meshFilter;
        public MeshFilter platformMeshFilter;
        public RectTransform leftPanel;
        public bool constantSeed = false;
        public AxeGenerator.Config config = new AxeGenerator.Config();
        
        private readonly Range<float> handleWidthRange = new Range<float>(0.05f, 0.12f);
        private readonly Range<float> handleHeightRange = new Range<float>(0.5f, 1.2f);

        private readonly Range<float> headHeightPercentRange = new Range<float>(0.1f, 0.3f);
        private readonly Range<float> headTopWidthRange = new Range<float>(0.2f, 0.6f);
        private readonly Range<float> headBottomWidthPercentRange = new Range<float>(0.6f, 1f);

        /// angle in degrees
        private readonly Range<float> headTopAngleRange = new Range<float>(5f, 60f);
        private readonly Range<float> headBottomAngleRange = new Range<float>(5f, 60f);

        private const float platformHeight = 0.05f;
        private const float platformRadiusOffset = 0.5f;

        private Mesh mesh;
        private Mesh platformMesh;

        SliderControl handleWidtSlider;
        SliderControl handleHeightSlider;

        SliderControl headHeightPercentSlider;
        SliderControl headTopWidthSlider;
        SliderControl headBottomWidthPercentSlider;
        SliderControl headTopAngleSlider;
        SliderControl headBottomAngleSlider;

        ToggleControl twoSidedToggle;

        private void Awake() {
            Generate();
            SetupSkyboxAndPalette();
            handleWidtSlider = CreateSlider("Handle width", handleWidthRange, config.handleWidth, (x) => config.handleWidth = x);
            handleHeightSlider = CreateSlider("Handle height", handleHeightRange, config.handleHeight, (x) => config.handleHeight = x);

            headHeightPercentSlider = CreateSlider("Head height %", headHeightPercentRange, config.headHeightPercent, (x) => config.headHeightPercent = x);

            headTopWidthSlider = CreateSlider("Head top width", headTopWidthRange, config.headTopWidth, (x) => config.headTopWidth = x);
            headBottomWidthPercentSlider = CreateSlider("Head bottom width %", headBottomWidthPercentRange, config.headBottomWidthPercent, (x) => config.headBottomWidthPercent = x);

            headTopAngleSlider = CreateSlider("Head top angle º", headTopAngleRange, config.headTopAngle, (x) => config.headTopAngle = x);
            headBottomAngleSlider = CreateSlider("Head bottom angle º", headBottomAngleRange, config.headBottomAngle, (x) => config.headBottomAngle = x);

            twoSidedToggle = InstantiateControl<ToggleControl>(leftPanel);
            twoSidedToggle.Initialize("Is two sided", config.isTwoSided, value => {
                config.isTwoSided = value;
                Generate();
            });

            InstantiateControl<ButtonControl>(leftPanel).Initialize("Randomize", () => Generate(true));
        }

        private SliderControl CreateSlider(string text, Range<float> range, float curentValue, System.Action<float> setValue) {
            var slider = InstantiateControl<SliderControl>(leftPanel);
            slider.Initialize(text, range.Minimum, range.Maximum, curentValue, value => {
                setValue(value);
                Generate();
            });
            return slider;
        }

        public void Generate(bool randomizeConfig = false)
        {
            if (constantSeed)
            {
                Random.InitState(0);
            }

            if (randomizeConfig) {
                RandomizeValueAndUpdateSlider(ref config.handleWidth, handleWidthRange, handleWidtSlider);
                RandomizeValueAndUpdateSlider(ref config.handleHeight, handleHeightRange, handleHeightSlider);
                RandomizeValueAndUpdateSlider(ref config.headHeightPercent, headHeightPercentRange, headHeightPercentSlider);

                RandomizeValueAndUpdateSlider(ref config.headTopWidth, headTopWidthRange, headTopWidthSlider);
                RandomizeValueAndUpdateSlider(ref config.headBottomWidthPercent, headBottomWidthPercentRange, headBottomWidthPercentSlider);
                RandomizeValueAndUpdateSlider(ref config.headTopAngle, headTopAngleRange, headTopAngleSlider);
                RandomizeValueAndUpdateSlider(ref config.headBottomAngle, headBottomAngleRange, headBottomAngleSlider);
                
                config.isTwoSided = Random.value > 0.7;
                twoSidedToggle.toggle.isOn = config.isTwoSided;
            }

            var axeDraft = AxeGenerator.Axe(config);
            AssignDraftToMeshFilter(axeDraft, meshFilter, ref mesh);
            
            float platformRadius = 1.5f;

            var platformDraft = Platform(platformRadius, platformHeight);
            AssignDraftToMeshFilter(platformDraft, platformMeshFilter, ref platformMesh);
        }

        private void RandomizeValueAndUpdateSlider(ref float value, Range<float> range, SliderControl slider) {
            value = GetRandomRangeFloat(range);
            slider.slider.value = value;
        }

        float GetRandomRangeFloat(Range<float> range) {
            return Random.Range(range.Minimum, range.Maximum);
        }
    }
}