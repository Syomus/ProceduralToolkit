using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProceduralToolkit.Examples.UI
{
    public class BoidsUI : UIBase
    {
        public MeshFilter meshFilter;
        public RectTransform leftPanel;

        private BoidController controller;
        private bool simulate = true;
        private List<ColorHSV> targetPalette = new List<ColorHSV>();
        private List<ColorHSV> currentPalette = new List<ColorHSV>();

        private void Awake()
        {
            RenderSettings.skybox = new Material(RenderSettings.skybox);

            Generate();
            currentPalette.AddRange(targetPalette);
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
            SkyBoxGenerator.LerpSkybox(RenderSettings.skybox, currentPalette, targetPalette, 2, 3, 4, Time.deltaTime);
        }

        private void Generate()
        {
            targetPalette = RandomE.TetradicPalette(0.25f, 0.75f);
            targetPalette.Add(ColorHSV.Lerp(targetPalette[2], targetPalette[3], 0.5f));

            controller = new BoidController();
            var mesh = controller.Generate(targetPalette[0].WithSV(1, 1).ToColor(),
                targetPalette[1].WithSV(0.8f, 0.8f).ToColor());
            meshFilter.mesh = mesh;
        }
    }
}