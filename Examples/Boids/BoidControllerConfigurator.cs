using System.Collections;
using ProceduralToolkit.Examples.UI;
using UnityEngine;

namespace ProceduralToolkit.Examples
{
    public class BoidControllerConfigurator : ConfiguratorBase
    {
        public MeshFilter meshFilter;
        public RectTransform leftPanel;
        public BoidController.Config config = new BoidController.Config();

        private BoidController controller;
        private bool simulate = true;

        private void Awake()
        {
            Generate();
            SetupSkyboxAndPalette();
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
            UpdateSkybox();
        }

        private void Generate()
        {
            controller = new BoidController();

            GeneratePalette();
            Color colorA = GetMainColorHSV().ToColor();
            Color colorB = GetSecondaryColorHSV().ToColor();

            config.template = MeshDraft.Tetrahedron(0.3f);
            // Assuming that we are dealing with tetrahedron, first vertex should be boid's "nose"
            config.template.colors.Add(colorA);
            for (int i = 1; i < config.template.vertexCount; i++)
            {
                config.template.colors.Add(colorB);
            }

            meshFilter.mesh = controller.Generate(config);
        }
    }
}