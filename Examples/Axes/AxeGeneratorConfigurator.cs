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

        private const float minHandleWidth = 0.05f;
        private const float maxHandleWidth = 0.12f;
        private const float minHandleHeight = 0.5f;
        private const float maxHandleHeight = 1.2f;

        private readonly Range<float> headHeightPercentRange = new Range<float>(0.1f, 0.3f);
        private readonly Range<float> headWidthTopRange = new Range<float>(0.2f, 0.6f);
        private readonly Range<float> headWidthBottomPercentRange = new Range<float>(0.6f, 1f);

        /// angle in degrees
        private readonly Range<float> headTopAngleRange = new Range<float>(5f, 60f);
        private readonly Range<float> headBottomAngleRange = new Range<float>(5f, 60f);

        private const float platformHeight = 0.05f;
        private const float platformRadiusOffset = 0.5f;

        private Mesh mesh;
        private Mesh platformMesh;

        private void Awake()
        {
            Generate();
            SetupSkyboxAndPalette();

            InstantiateControl<SliderControl>(leftPanel)
                .Initialize("Handle width", minHandleWidth, maxHandleWidth, config.handleWidth, value =>
                {
                    config.handleWidth = value;
                    Generate();
                });
            InstantiateControl<SliderControl>(leftPanel)
                .Initialize("Handle height", minHandleHeight, maxHandleHeight, config.handleHeight, value =>
                {
                    config.handleHeight = value;
                    Generate();
                });

            InstantiateControl<SliderControl>(leftPanel)
                .Initialize("Head height %", headHeightPercentRange.Minimum, headHeightPercentRange.Maximum, config.headHeightPercent, value => {
                    config.headHeightPercent = value;
                    Generate();
                });

            InstantiateControl<SliderControl>(leftPanel)
                .Initialize("Head width", headWidthTopRange.Minimum, headWidthTopRange.Maximum, config.headWidthTop, value => {
                    config.headWidthTop = value;
                    Generate();
                });

            InstantiateControl<SliderControl>(leftPanel)
                .Initialize("Head width", headWidthBottomPercentRange.Minimum, headWidthBottomPercentRange.Maximum, config.headWidthBottomPercent, value => {
                    config.headWidthBottomPercent = value;
                    Generate();
                });

            InstantiateControl<SliderControl>(leftPanel)
                .Initialize("Head width", headTopAngleRange.Minimum, headTopAngleRange.Maximum, config.headTopAngle, value => {
                    config.headTopAngle = value;
                    Generate();
                });

            InstantiateControl<SliderControl>(leftPanel)
                .Initialize("Head width", headBottomAngleRange.Minimum, headBottomAngleRange.Maximum, config.headBottomAngle, value => {
                    config.headBottomAngle = value;
                    Generate();
                });

            InstantiateControl<ButtonControl>(leftPanel).Initialize("Randomize", () => Generate(true));
        }

        public void Generate(bool randomizeConfig = false)
        {
            if (constantSeed)
            {
                Random.InitState(0);
            }

            if (randomizeConfig)
            {
                config.handleWidth = Random.Range(minHandleWidth, maxHandleWidth);
                config.handleHeight = Random.Range(minHandleHeight, maxHandleHeight);

                config.headWidthTop = GetRandomRangeFloat(headWidthTopRange);
                config.headWidthBottomPercent = GetRandomRangeFloat(headWidthBottomPercentRange);
                config.headHeightPercent = GetRandomRangeFloat(headHeightPercentRange);

                config.headTopAngle = GetRandomRangeFloat(headTopAngleRange);
            }

            var axeDraft = AxeGenerator.Axe(config);
            AssignDraftToMeshFilter(axeDraft, meshFilter, ref mesh);
            
            float platformRadius = 1.5f;

            var platformDraft = Platform(platformRadius, platformHeight);
            AssignDraftToMeshFilter(platformDraft, platformMeshFilter, ref platformMesh);
        }

        float GetRandomRangeFloat(Range<float> range) {
            return Random.Range(range.Minimum, range.Maximum);
        }
    }
}