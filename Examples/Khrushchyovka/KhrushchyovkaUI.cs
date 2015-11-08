using UnityEngine;

namespace ProceduralToolkit.Examples.UI
{
    public class KhrushchyovkaUI : UIBase
    {
        public MeshFilter meshFilter;
        public RectTransform leftPanel;

        private int width = 10;
        private int length = 50;
        private int floorCount = 5;
        private bool hasAttic = true;

        private void Awake()
        {
            Generate();

            InstantiateControl<SliderControl>(leftPanel).Initialize("Width", 10, 30, width, value =>
            {
                width = value;
                Generate();
            });

            InstantiateControl<SliderControl>(leftPanel).Initialize("Length", 10, 60, length, value =>
            {
                length = value;
                Generate();
            });

            InstantiateControl<SliderControl>(leftPanel).Initialize("Floor count", 1, 10, floorCount, value =>
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

        private void Generate()
        {
            var draft = Khrushchyovka.KhrushchyovkaDraft(width, length, floorCount, hasAttic);
            meshFilter.mesh = draft.ToMesh();
        }
    }
}