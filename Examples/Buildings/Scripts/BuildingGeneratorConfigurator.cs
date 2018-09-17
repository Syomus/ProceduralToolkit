using System.Collections.Generic;
using ProceduralToolkit.Buildings;
using ProceduralToolkit.Examples.UI;
using UnityEngine;

namespace ProceduralToolkit.Examples.Buildings
{
    /// <summary>
    /// Configurator for BuildingGenerator with UI and editor controls
    /// </summary>
    public class BuildingGeneratorConfigurator : ConfiguratorBase
    {
        public GameObject building;
        public MeshFilter platformMeshFilter;
        public RectTransform leftPanel;
        public bool constantSeed = false;
        public FacadePlanningStrategy facadePlanningStrategy;
        public FacadeConstructionStrategy facadeConstructionStrategy;
        public RoofPlanningStrategy roofPlanningStrategy;
        public RoofConstructionStrategy roofConstructionStrategy;
        public float width = 12;
        public float length = 36;
        public BuildingGenerator.Config config = new BuildingGenerator.Config();

        private const int minWidth = 10;
        private const int maxWidth = 30;
        private const int minLength = 10;
        private const int maxLength = 60;
        private const int minFloorCount = 1;
        private const int maxFloorCount = 10;

        private const float platformHeight = 0.5f;
        private const float platformRadiusOffset = 2;

        private BuildingGenerator generator;
        private Mesh platformMesh;

        private void Awake()
        {
            Generate();
            SetupSkyboxAndPalette();

            InstantiateControl<SliderControl>(leftPanel)
                .Initialize("Width", minWidth, maxWidth, (int) width, value =>
                {
                    width = value;
                    Generate();
                });

            InstantiateControl<SliderControl>(leftPanel)
                .Initialize("Length", minLength, maxLength, (int) length, value =>
                {
                    length = value;
                    Generate();
                });

            InstantiateControl<SliderControl>(leftPanel)
                .Initialize("Floors", minFloorCount, maxFloorCount, config.floors, value =>
                {
                    config.floors = value;
                    Generate();
                });

            InstantiateControl<ToggleControl>(leftPanel).Initialize("Has attic", config.hasAttic, value =>
            {
                config.hasAttic = value;
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

                config.palette.wallColor = GetMainColorHSV().ToColor();
                config.roofConfig.type = RandomE.GetRandom(RoofType.Flat, RoofType.Hipped, RoofType.Gabled);
            }

            if (generator == null)
            {
                generator = new BuildingGenerator();
            }

            if (building != null)
            {
                if (Application.isPlaying)
                {
                    Destroy(building);
                }
                else
                {
                    DestroyImmediate(building);
                }
            }
            generator.SetFacadePlanningStrategy(facadePlanningStrategy);
            generator.SetFacadeConstructionStrategy(facadeConstructionStrategy);
            generator.SetRoofPlanningStrategy(roofPlanningStrategy);
            generator.SetRoofConstructionStrategy(roofConstructionStrategy);
            var foundationPolygon = new List<Vector2>
            {
                Vector2.left*length/2 + Vector2.down*width/2,
                Vector2.left*length/2 + Vector2.up*width/2,
                Vector2.right*length/2 + Vector2.up*width/2,
                Vector2.right*length/2 + Vector2.down*width/2,
            };
            building = generator.Generate(foundationPolygon, config).gameObject;

            float buildingRadius = Mathf.Sqrt(length/2*length/2 + width/2*width/2);
            float platformRadius = buildingRadius + platformRadiusOffset;

            var platformDraft = Platform(platformRadius, platformHeight);
            AssignDraftToMeshFilter(platformDraft, platformMeshFilter, ref platformMesh);
        }
    }
}
