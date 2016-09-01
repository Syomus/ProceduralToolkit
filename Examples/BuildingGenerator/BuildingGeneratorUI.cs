using System.Collections.Generic;
using UnityEngine;

namespace ProceduralToolkit.Examples.UI
{
    public class BuildingGeneratorUI : UIBase
    {
        public MeshFilter meshFilter;
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
            SkyBoxGenerator.LerpSkybox(RenderSettings.skybox, currentPalette, targetPalette, 1, 2, 3, Time.deltaTime);
        }

        public void Generate()
        {
            targetPalette = RandomE.TriadicPalette(0.5f, 0.75f);
            targetPalette.Add(ColorHSV.Lerp(targetPalette[1], targetPalette[2], 0.5f));

            var draft = BuildingGenerator.BuildingDraft(width, length, floorCount, hasAttic,
                targetPalette[0].WithS(0.8f).WithV(0.8f).ToColor());

            var circle = MeshDraft.TriangleFan(PTUtils.PointsOnCircle3XZ(length/2 + 10, 128));
            circle.Paint(new Color(0.8f, 0.8f, 0.8f, 1));
            draft.Add(circle);

            var mesh = draft.ToMesh();
            mesh.RecalculateBounds();
            meshFilter.mesh = mesh;
        }
    }
}