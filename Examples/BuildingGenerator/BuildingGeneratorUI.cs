using UnityEngine;

namespace ProceduralToolkit.Examples.UI
{
    public class BuildingGeneratorUI : UIBase
    {
        public MeshFilter meshFilter;
        public RectTransform leftPanel;
        [Space]
        [Range(minWidth, maxWidth)]
        public int width = 10;
        [Range(minLength, maxLength)]
        public int length = 50;
        [Range(minFloorCount, maxFloorCount)]
        public int floorCount = 5;
        public bool hasAttic = true;

        private const int minWidth = 10;
        private const int maxWidth = 30;
        private const int minLength = 10;
        private const int maxLength = 60;
        private const int minFloorCount = 1;
        private const int maxFloorCount = 10;

        private void Awake()
        {
            RenderSettings.skybox = new Material(RenderSettings.skybox);

            Generate();

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

        public void Generate()
        {
            var color = new ColorHSV(Random.value, 0.6f, 0.75f);
            var palette = color.GetTriadicPalette();

            RenderSettings.skybox.SetColor("_SkyColor", palette[1].ToColor());
            RenderSettings.skybox.SetColor("_HorizonColor", ColorHSV.Lerp(palette[1], palette[2], 0.5f).ToColor());
            RenderSettings.skybox.SetColor("_GroundColor", palette[2].ToColor());

            var draft = BuildingGenerator.BuildingDraft(width, length, floorCount, hasAttic, color.ToColor());
            meshFilter.mesh = draft.ToMesh();
        }
    }
}