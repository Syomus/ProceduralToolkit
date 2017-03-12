using System;
using UnityEngine;
using UnityEngine.UI;

namespace ProceduralToolkit.Examples.UI
{
    public class SliderControl : MonoBehaviour
    {
        public Text headerText;
        public Slider slider;
        public Text valueText;

        public void Initialize(string header, float minValue, float maxValue, float value, Action<float> onValueChanged)
        {
            name = header;
            headerText.text = header;
            slider.minValue = minValue;
            slider.maxValue = maxValue;
            slider.wholeNumbers = false;
            slider.value = value;
            slider.onValueChanged.AddListener(newValue =>
            {
                onValueChanged(newValue);
                valueText.text = newValue.ToString();
            });
            valueText.text = value.ToString();
        }

        public void Initialize(string header, int minValue, int maxValue, int value, Action<int> onValueChanged)
        {
            name = header;
            headerText.text = header;
            slider.minValue = minValue;
            slider.maxValue = maxValue;
            slider.wholeNumbers = true;
            slider.value = value;
            slider.onValueChanged.AddListener(newValue =>
            {
                int intValue = Mathf.FloorToInt(newValue);
                onValueChanged(intValue);
                valueText.text = intValue.ToString();
            });
            valueText.text = value.ToString();
        }
    }
}