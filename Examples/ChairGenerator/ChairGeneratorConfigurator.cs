using System.Collections.Generic;
using UnityEngine;

namespace ProceduralToolkit.Examples.UI
{
    public class ChairGeneratorConfigurator : UIBase
    {
        public MeshFilter chairMeshFilter;
        public MeshFilter platformMeshFilter;
        public RectTransform leftPanel;
        [Space]
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

        private const float platformBaseOffset = 0.05f;
        private const float platformHeight = 0.05f;
        private const float platformRadiusOffset = 0.5f;
        private const int platformSegments = 128;

        private List<ColorHSV> targetPalette = new List<ColorHSV>();
        private List<ColorHSV> currentPalette = new List<ColorHSV>();

        private void Awake()
        {
            RenderSettings.skybox = new Material(RenderSettings.skybox);

            Generate();
            currentPalette.AddRange(targetPalette);

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

            config.color = targetPalette[0].WithSV(0.8f, 0.8f).ToColor();
            var chairDraft = ChairGenerator.Chair(config);
            var chairMesh = chairDraft.ToMesh();
            chairMesh.RecalculateBounds();
            chairMeshFilter.mesh = chairMesh;

            float chairRadius = Mathf.Sqrt(config.seatWidth*config.seatWidth/4 + config.seatDepth*config.seatDepth/4);
            float platformRadius = chairRadius + platformRadiusOffset;

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