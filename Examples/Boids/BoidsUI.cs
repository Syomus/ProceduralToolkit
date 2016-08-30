using System.Collections;
using UnityEngine;

namespace ProceduralToolkit.Examples.UI
{
    public class BoidsUI : UIBase
    {
        public MeshFilter meshFilter;
        public RectTransform leftPanel;

        private BoidController controller;
        private bool simulate = true;

        private void Awake()
        {
            RenderSettings.skybox = new Material(RenderSettings.skybox);

            Generate();
            StartCoroutine(Simulate());

            InstantiateControl<SliderControl>(leftPanel).Initialize("Max speed", 0, 30,
                value: (int) controller.maxSpeed,
                onValueChanged: value => controller.maxSpeed = value);

            InstantiateControl<SliderControl>(leftPanel).Initialize("Interaction radius", 0, 30,
                value: (int) controller.interactionRadius,
                onValueChanged: value => controller.interactionRadius = value);

            InstantiateControl<SliderControl>(leftPanel).Initialize("Cohesion coefficient", 0, 30,
                value: (int) controller.cohesionCoefficient,
                onValueChanged: value => controller.cohesionCoefficient = value);

            InstantiateControl<SliderControl>(leftPanel).Initialize("Separation distance", 0, 30,
                value: (int) controller.separationDistance,
                onValueChanged: value => controller.separationDistance = value);

            InstantiateControl<SliderControl>(leftPanel).Initialize("Separation coefficient", 0, 30,
                value: (int) controller.separationCoefficient,
                onValueChanged: value => controller.separationCoefficient = value);

            InstantiateControl<SliderControl>(leftPanel).Initialize("Alignment coefficient", 0, 30,
                value: (int) controller.alignmentCoefficient,
                onValueChanged: value => controller.alignmentCoefficient = value);

            InstantiateControl<ToggleControl>(leftPanel).Initialize("Simulate", simulate, value => simulate = value);

            InstantiateControl<ButtonControl>(leftPanel).Initialize("Generate", Generate);
        }

        private void Generate()
        {
            var palette = new ColorHSV(Random.value, 0.5f, 0.75f).GetTetradicPalette();

            RenderSettings.skybox.SetColor("_SkyColor", palette[0].ToColor());
            RenderSettings.skybox.SetColor("_HorizonColor", ColorHSV.Lerp(palette[0], palette[1], 0.5f).ToColor());
            RenderSettings.skybox.SetColor("_GroundColor", palette[1].ToColor());

            controller = new BoidController();
            var mesh = controller.Generate(palette[2].WithS(1).WithV(1).ToColor(),
                palette[3].WithS(0.8f).WithV(0.8f).ToColor());
            meshFilter.mesh = mesh;
        }

        private IEnumerator Simulate()
        {
            while (true)
            {
                yield return StartCoroutine(controller.CalculateVelocities());
            }
        }

        private void Update()
        {
            if (simulate)
            {
                controller.Update();
            }
        }
    }
}