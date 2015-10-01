using System;
using UnityEngine;
using UnityEngine.UI;

namespace ProceduralToolkit.Examples
{
    public class ChairGeneratorUI : MonoBehaviour
    {
        public MeshFilter meshFilter;

        public Slider legWidthSlider;
        public Text legWidthText;

        public Slider legHeightSlider;
        public Text legHeightText;

        public Slider backHeightSlider;
        public Text backHeightText;

        public Toggle hasStretchersToggle;
        public Toggle hasArmrestsToggle;
        public Button generateButton;

        private float legWidth = 0.07f;
        private float legHeight = 0.7f;
        private Vector3 seatLB = new Vector3(0.7f, 0.05f, 0.7f);
        private Vector3 seatUB = new Vector3(1, 0.2f, 0.9f);
        private float backHeight = 0.8f;
        private bool hasStretchers = true;
        private bool hasArmrests = false;

        private void Awake()
        {
            Generate();

            SetupSlider(legWidthSlider, legWidthText, legWidth, value => legWidth = value);
            SetupSlider(legHeightSlider, legHeightText, legHeight, value => legHeight = value);
            SetupSlider(backHeightSlider, backHeightText, backHeight, value => backHeight = value);

            hasStretchersToggle.onValueChanged.AddListener(value => hasStretchers = value);
            hasArmrestsToggle.isOn = hasArmrests;
            hasArmrestsToggle.onValueChanged.AddListener(value => hasArmrests = value);

            generateButton.onClick.AddListener(Generate);
        }

        private void SetupSlider(Slider slider, Text text, float defaultValue, Action<float> onValueChanged)
        {
            slider.value = defaultValue;
            slider.onValueChanged.AddListener(value =>
            {
                onValueChanged(value);
                text.text = value.ToString();
            });
            text.text = defaultValue.ToString();
        }

        private void Generate()
        {
            var draft = ChairGenerator.Chair(legWidth, legHeight, RandomE.Range(seatLB, seatUB), backHeight,
                hasStretchers, hasArmrests);
            meshFilter.mesh = draft.ToMesh();
        }
    }
}