using ProceduralToolkit.Samples.UI;
using UnityEngine;

namespace ProceduralToolkit.Samples
{
    /// <summary>
    /// Configurator for ChairGenerator with UI and editor controls
    /// </summary>
    public class ChairGeneratorConfigurator : ConfiguratorBase
    {
        public MeshFilter chairMeshFilter;
        public MeshFilter platformMeshFilter;
        public RectTransform leftPanel;
        public bool constantSeed = false;
        public ChairGenerator.Config config = new ChairGenerator.Config();

        private const float minLegWidth = 0.05f;
        private const float maxLegWidth = 0.12f;
        private const float minLegHeight = 0.5f;
        private const float maxLegHeight = 1.2f;

        private const float minSeatWidth = 0.5f;
        private const float maxSeatWidth = 1.2f;
        private const float minSeatDepth = 0.3f;
        private const float maxSeatDepth = 1.2f;
        private const float minSeatHeight = 0.03f;
        private const float maxSeatHeight = 0.2f;

        private const float minBackHeight = 0.5f;
        private const float maxBackHeight = 1.3f;

        private const float platformHeight = 0.05f;
        private const float platformRadiusOffset = 0.5f;

        private Mesh chairMesh;
        private Mesh platformMesh;

        private void Awake()
        {
            Generate();
            SetupSkyboxAndPalette();

            InstantiateControl<SliderControl>(leftPanel)
                .Initialize("Leg width", minLegWidth, maxLegWidth, config.legWidth, value =>
                {
                    config.legWidth = value;
                    Generate();
                });
            InstantiateControl<SliderControl>(leftPanel)
                .Initialize("Leg height", minLegHeight, maxLegHeight, config.legHeight, value =>
                {
                    config.legHeight = value;
                    Generate();
                });

            InstantiateControl<SliderControl>(leftPanel)
                .Initialize("Seat width", minSeatWidth, maxSeatWidth, config.seatWidth, value =>
                {
                    config.seatWidth = value;
                    Generate();
                });
            InstantiateControl<SliderControl>(leftPanel)
                .Initialize("Seat depth", minSeatDepth, maxSeatDepth, config.seatDepth, value =>
                {
                    config.seatDepth = value;
                    Generate();
                });
            InstantiateControl<SliderControl>(leftPanel)
                .Initialize("Seat height", minSeatHeight, maxSeatHeight, config.seatHeight, value =>
                {
                    config.seatHeight = value;
                    Generate();
                });

            InstantiateControl<SliderControl>(leftPanel)
                .Initialize("Back height", minBackHeight, maxBackHeight, config.backHeight, value =>
                {
                    config.backHeight = value;
                    Generate();
                });

            InstantiateControl<ToggleControl>(leftPanel).Initialize("Has stretchers", config.hasStretchers, value =>
            {
                config.hasStretchers = value;
                Generate();
            });
            InstantiateControl<ToggleControl>(leftPanel).Initialize("Has armrests", config.hasArmrests, value =>
            {
                config.hasArmrests = value;
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

                config.color = GetMainColorHSV().ToColor();
            }

            var chairDraft = ChairGenerator.Chair(config);
            AssignDraftToMeshFilter(chairDraft, chairMeshFilter, ref chairMesh);

            float platformRadius = Geometry.GetCircumradius(config.seatWidth, config.seatDepth) + platformRadiusOffset;
            var platformDraft = Platform(platformRadius, platformHeight);
            AssignDraftToMeshFilter(platformDraft, platformMeshFilter, ref platformMesh);
        }
    }
}
