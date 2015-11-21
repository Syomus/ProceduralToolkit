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
            controller = new BoidController();
            var mesh = controller.Generate();
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