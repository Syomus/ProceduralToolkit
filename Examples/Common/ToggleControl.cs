using System;
using UnityEngine;
using UnityEngine.UI;

namespace ProceduralToolkit.Examples.UI
{
    public class ToggleControl : MonoBehaviour
    {
        public Text headerText;
        public Toggle toggle;

        public void Initialize(string header, bool value, Action<bool> onValueChanged, ToggleGroup toggleGroup = null)
        {
            name = header;
            headerText.text = header;
            toggle.isOn = value;
            toggle.onValueChanged.AddListener(newValue => onValueChanged(newValue));
            toggle.group = toggleGroup;
        }
    }
}
