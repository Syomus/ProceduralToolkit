using System;
using UnityEngine;
using UnityEngine.UI;

namespace ProceduralToolkit.Samples.UI
{
    public class ButtonControl : MonoBehaviour
    {
        public Text headerText;
        public Button button;

        public void Initialize(string header, Action onClick)
        {
            name = header;
            headerText.text = header;
            button.onClick.AddListener(() => onClick());
        }
    }
}
