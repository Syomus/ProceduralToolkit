using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProceduralToolkit.Examples.UI
{
    public class BoidControllerConfigurator : UIBase
    {
        public MeshFilter meshFilter;
        public RectTransform leftPanel;
        public BoidController.Config config = new BoidController.Config();

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
                value: (int) config.maxSpeed,
                onValueChanged: value => config.maxSpeed = value);

            InstantiateControl<SliderControl>(leftPanel).Initialize("Interaction radius", 0, 30,
                value: (int) config.interactionRadius,
                onValueChanged: value => config.interactionRadius = value);

            InstantiateControl<SliderControl>(leftPanel).Initialize("Cohesion coefficient", 0, 30,
                value: (int) config.cohesionCoefficient,
                onValueChanged: value => config.cohesionCoefficient = value);

            InstantiateControl<SliderControl>(leftPanel).Initialize("Separation distance", 0, 30,
                value: (int) config.separationDistance,
                onValueChanged: value => config.separationDistance = value);

            InstantiateControl<SliderControl>(leftPanel).Initialize("Separation coefficient", 0, 30,
                value: (int) config.separationCoefficient,
                onValueChanged: value => config.separationCoefficient = value);

            InstantiateControl<SliderControl>(leftPanel).Initialize("Alignment coefficient", 0, 30,
                value: (int) config.alignmentCoefficient,
                onValueChanged: value => config.alignmentCoefficient = value);

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
            controller = new BoidController();

            targetPalette = RandomE.TetradicPalette(0.25f, 0.75f);
            targetPalette.Add(ColorHSV.Lerp(targetPalette[2], targetPalette[3], 0.5f));
            Color colorA = targetPalette[0].WithSV(1, 1).ToColor();
            Color colorB = targetPalette[1].WithSV(0.8f, 0.8f).ToColor();

            config.template = MeshDraft.Tetrahedron(0.3f);
            // Assuming that we are dealing with tetrahedron, first vertex should be boid's "nose"
            config.template.colors.Add(colorA);
            for (int i = 1; i < config.template.vertices.Count; i++)
            {
                config.template.colors.Add(colorB);
            }

            var mesh = controller.Generate(config);
            meshFilter.mesh = mesh;
        }
    }
}