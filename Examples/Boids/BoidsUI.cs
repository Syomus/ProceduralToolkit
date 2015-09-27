using System;
using UnityEngine;
using UnityEngine.UI;

namespace ProceduralToolkit.Examples
{
    public class BoidsUI : MonoBehaviour
    {
        public MeshFilter meshFilter;

        public Slider maxSpeedSlider;
        public Text maxSpeedText;

        public Slider interactionRadiusSlider;
        public Text interactionRadiusText;

        public Slider cohesionCoefficientSlider;
        public Text cohesionCoefficientText;

        public Slider separationDistanceSlider;
        public Text separationDistanceText;
        public Slider separationCoefficientSlider;
        public Text separationCoefficientText;

        public Slider alignmentCoefficientSlider;
        public Text alignmentCoefficientText;

        public Toggle simulateToggle;
        public Button generateButton;

        private BoidController controller;
        private bool simulate = true;

        private void Awake()
        {
            controller = new BoidController(meshFilter);
            controller.Generate();
            StartCoroutine(controller.Simulate());

            SetupSlider(maxSpeedSlider, maxSpeedText, controller.maxSpeed,
                value => controller.maxSpeed = value);

            SetupSlider(interactionRadiusSlider, interactionRadiusText, controller.interactionRadius,
                value => controller.interactionRadius = value);
            SetupSlider(cohesionCoefficientSlider, cohesionCoefficientText, controller.cohesionCoefficient,
                value => controller.cohesionCoefficient = value);

            SetupSlider(separationDistanceSlider, separationDistanceText, controller.separationDistance,
                value => controller.separationDistance = value);
            SetupSlider(separationCoefficientSlider, separationCoefficientText, controller.separationCoefficient,
                value => controller.separationCoefficient = value);

            SetupSlider(alignmentCoefficientSlider, alignmentCoefficientText, controller.alignmentCoefficient,
                value => controller.alignmentCoefficient = value);

            simulateToggle.onValueChanged.AddListener(value => simulate = value);
            generateButton.onClick.AddListener(controller.Generate);
        }

        private void SetupSlider(Slider slider, Text text, float defaultValue, Action<int> onValueChanged)
        {
            slider.value = defaultValue;
            slider.onValueChanged.AddListener(value =>
            {
                int intValue = Mathf.FloorToInt(value);
                onValueChanged(intValue);
                text.text = intValue.ToString();
            });
            text.text = defaultValue.ToString();
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