using System;
using UnityEngine;
using UnityEngine.UI;

namespace ProceduralToolkit.Examples
{
    public class KhrushchyovkaUI : MonoBehaviour
    {
        public MeshFilter meshFilter;

        public Slider widthSlider;
        public Text widthText;

        public Slider lengthSlider;
        public Text lengthText;

        public Slider floorCountSlider;
        public Text floorCountText;

        public Toggle hasAtticToggle;
        public Button generateButton;

        private float width = 10;
        private float length = 50;
        private int floorCount = 5;
        private bool hasAttic = true;

        private void Awake()
        {
            Generate();

            SetupSlider(widthSlider, widthText, width, value => width = value);
            SetupSlider(lengthSlider, lengthText, length, value => length = value);
            SetupSlider(floorCountSlider, floorCountText, floorCount, value => floorCount = value);

            hasAtticToggle.onValueChanged.AddListener(value => hasAttic = value);
            generateButton.onClick.AddListener(Generate);
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

        private void Generate()
        {
            var draft = Khrushchyovka.KhrushchyovkaDraft(width, length, floorCount, hasAttic);
            meshFilter.mesh = draft.ToMesh();
        }
    }
}