using System.Collections.Generic;
using UnityEngine;

namespace ProceduralToolkit.Examples.UI
{
    public class BuildingGeneratorUI : UIBase
    {
        public MeshFilter buildingMeshFilter;
        public MeshFilter platformMeshFilter;
        public RectTransform leftPanel;
        [Space]
        [Range(minWidth, maxWidth)]
        public int width = 15;
        [Range(minLength, maxLength)]
        public int length = 30;
        [Range(minFloorCount, maxFloorCount)]
        public int floorCount = 5;
        public bool hasAttic = true;

        private const int minWidth = 10;
        private const int maxWidth = 30;
        private const int minLength = 10;
        private const int maxLength = 60;
        private const int minFloorCount = 1;
        private const int maxFloorCount = 10;

        private const float platformBaseOffset = 0.5f;
        private const float platformHeight = 0.5f;
        private const float platformRadiusOffset = 2;
        private const int platformSegments = 128;

        private List<ColorHSV> targetPalette = new List<ColorHSV>();
        private List<ColorHSV> currentPalette = new List<ColorHSV>();

        private void Awake()
        {
            RenderSettings.skybox = new Material(RenderSettings.skybox);

            Generate();
            currentPalette.AddRange(targetPalette);

            InstantiateControl<SliderControl>(leftPanel).Initialize("Width", minWidth, maxWidth, width, value =>
            {
                width = value;
                Generate();
            });

            InstantiateControl<SliderControl>(leftPanel).Initialize("Length", minLength, maxLength, length, value =>
            {
                length = value;
                Generate();
            });

            InstantiateControl<SliderControl>(leftPanel)
                .Initialize("Floor count", minFloorCount, maxFloorCount, floorCount, value =>
                {
                    floorCount = value;
                    Generate();
                });

            InstantiateControl<ToggleControl>(leftPanel).Initialize("Has attic", hasAttic, value =>
            {
                hasAttic = value;
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
            targetPalette = RandomE.TetradicPalette(0.25f, 0.75f);
            targetPalette.Add(ColorHSV.Lerp(targetPalette[2], targetPalette[3], 0.5f));

            var buildingDraft = BuildingGenerator.BuildingDraft(width, length, floorCount, hasAttic,
                targetPalette[0].WithSV(0.8f, 0.8f).ToColor());

            var buildingMesh = buildingDraft.ToMesh();
            buildingMesh.RecalculateBounds();
            buildingMeshFilter.mesh = buildingMesh;

            float buildingRadius = Mathf.Sqrt(length/2f*length/2f + width/2f*width/2f);
            float platformRadius = buildingRadius + platformRadiusOffset;

            var platformMesh = Platform(platformRadius, platformBaseOffset, platformSegments, platformHeight).ToMesh();
            platformMesh.RecalculateBounds();
            platformMeshFilter.mesh = platformMesh;
        }

        private static MeshDraft Platform(float radius, float baseOffset, int segments, float heignt)
        {
            float segmentAngle = 360f/segments;
            float currentAngle = 0;

            var lowerRing = new List<Vector3>(segments);
            var upperRing = new List<Vector3>(segments);
            for (var i = 0; i < segments; i++)
            {
                var lowerPoint = PTUtils.PointOnCircle3XZ(radius + baseOffset, currentAngle);
                lowerRing.Add(lowerPoint + Vector3.down*heignt);

                var upperPoint = PTUtils.PointOnCircle3XZ(radius, currentAngle);
                upperRing.Add(upperPoint);
                currentAngle -= segmentAngle;
            }

            var platform = new MeshDraft {name = "Platform"};
            var bottom = MeshDraft.TriangleFan(lowerRing);
            bottom.Add(MeshDraft.Band(lowerRing, upperRing));
            bottom.Paint(new Color(0.5f, 0.5f, 0.5f, 1));
            platform.Add(bottom);

            upperRing.Reverse();
            var top = MeshDraft.TriangleFan(upperRing);
            top.Paint(new Color(0.8f, 0.8f, 0.8f, 1));
            platform.Add(top);

            return platform;
        }
    }
}