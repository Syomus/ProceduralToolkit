using System.Collections.Generic;
using ProceduralToolkit.Examples.UI;
using UnityEngine;

namespace ProceduralToolkit.Examples
{
    public class BuildingGeneratorConfigurator : ConfiguratorBase
    {
        public MeshFilter buildingMeshFilter;
        public MeshFilter platformMeshFilter;
        public RectTransform leftPanel;
        public bool constantSeed = false;
        public BuildingGenerator.Config config;

        private const int minWidth = 10;
        private const int maxWidth = 30;
        private const int minLength = 10;
        private const int maxLength = 60;
        private const int minFloorCount = 1;
        private const int maxFloorCount = 10;

        private const float platformHeight = 0.5f;
        private const float platformRadiusOffset = 2;

        private BuildingGenerator generator;
        private Mesh buildingMesh;
        private Mesh platformMesh;
        private List<ColorHSV> targetPalette = new List<ColorHSV>();
        private List<ColorHSV> currentPalette = new List<ColorHSV>();

        private void Awake()
        {
            RenderSettings.skybox = new Material(RenderSettings.skybox);

            Generate();
            currentPalette.AddRange(targetPalette);

            InstantiateControl<SliderControl>(leftPanel)
                .Initialize("Width", minWidth, maxWidth, (int) config.width, value =>
                {
                    config.width = value;
                    Generate();
                });

            InstantiateControl<SliderControl>(leftPanel)
                .Initialize("Length", minLength, maxLength, (int) config.length, value =>
                {
                    config.length = value;
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

            InstantiateControl<ButtonControl>(leftPanel).Initialize("Generate", Generate);
        }

        private void Update()
        {
            SkyBoxGenerator.LerpSkybox(RenderSettings.skybox, currentPalette, targetPalette, 2, 3, 4, Time.deltaTime);
        }

        public void Generate()
        {
            if (generator == null)
            {
                generator = new BuildingGenerator();
            }

            if (constantSeed)
            {
                Random.InitState(0);
            }

            targetPalette = RandomE.TetradicPalette(0.25f, 0.75f);
            targetPalette.Add(ColorHSV.Lerp(targetPalette[2], targetPalette[3], 0.5f));

            config.palette.wallColor = targetPalette[0].WithSV(0.8f, 0.8f).ToColor();

            config.roofConfig.type = RandomE.GetRandom(RoofType.Flat, RoofType.Hipped, RoofType.Gabled);
            var buildingDraft = generator.Generate(config);

            if (buildingMesh == null)
            {
                buildingMesh = buildingDraft.ToMesh();
            }
            else
            {
                buildingDraft.ToMesh(ref buildingMesh);
            }
            buildingMesh.RecalculateBounds();
            buildingMeshFilter.sharedMesh = buildingMesh;

            float buildingRadius = Mathf.Sqrt(config.length/2f*config.length/2f +
                                              config.width/2f*config.width/2f);
            float platformRadius = buildingRadius + platformRadiusOffset;

            var platformDraft = Platform(platformRadius, platformHeight);
            if (platformMesh == null)
            {
                platformMesh = platformDraft.ToMesh();
            }
            else
            {
                platformDraft.ToMesh(ref platformMesh);
            }
            platformMesh.RecalculateBounds();
            platformMeshFilter.sharedMesh = platformMesh;
        }
    }
}