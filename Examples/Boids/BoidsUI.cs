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
            controller = new BoidController(meshFilter);
            controller.Generate();
            StartCoroutine(controller.Simulate());

            var maxSpeedSlider = InstantiateControl<SliderControl>(leftPanel);
            maxSpeedSlider.Initialize("Max speed", 0, 30,
                value: (int) controller.maxSpeed,
                onValueChanged: value => controller.maxSpeed = value);

            var interactionRadiusSlider = InstantiateControl<SliderControl>(leftPanel);
            interactionRadiusSlider.Initialize("Interaction radius", 0, 30,
                value: (int) controller.interactionRadius,
                onValueChanged: value => controller.interactionRadius = value);

            var cohesionCoefficientSlider = InstantiateControl<SliderControl>(leftPanel);
            cohesionCoefficientSlider.Initialize("Cohesion coefficient", 0, 30,
                value: (int) controller.cohesionCoefficient,
                onValueChanged: value => controller.cohesionCoefficient = value);

            var separationDistanceSlider = InstantiateControl<SliderControl>(leftPanel);
            separationDistanceSlider.Initialize("Separation distance", 0, 30,
                value: (int) controller.separationDistance,
                onValueChanged: value => controller.separationDistance = value);

            var separationCoefficientSlider = InstantiateControl<SliderControl>(leftPanel);
            separationCoefficientSlider.Initialize("Separation coefficient", 0, 30,
                value: (int) controller.separationCoefficient,
                onValueChanged: value => controller.separationCoefficient = value);

            var alignmentCoefficientSlider = InstantiateControl<SliderControl>(leftPanel);
            alignmentCoefficientSlider.Initialize("Alignment coefficient", 0, 30,
                value: (int) controller.alignmentCoefficient,
                onValueChanged: value => controller.alignmentCoefficient = value);

            var simulateToggle = InstantiateControl<ToggleControl>(leftPanel);
            simulateToggle.Initialize("Simulate", simulate, value => simulate = value);

            var generateButton = InstantiateControl<ButtonControl>(leftPanel);
            generateButton.Initialize("Generate", controller.Generate);
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