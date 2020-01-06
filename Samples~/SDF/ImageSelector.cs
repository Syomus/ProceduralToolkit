using System.Collections.Generic;
using ProceduralToolkit.Samples.UI;
using UnityEngine;
using UnityEngine.UI;

namespace ProceduralToolkit.Samples
{
    public class ImageSelector : ConfiguratorBase
    {
        public Transform leftPanel;
        public ToggleGroup toggleGroup;
        public List<Image> images = new List<Image>();

        private void Awake()
        {
            var header = InstantiateControl<TextControl>(toggleGroup.transform.parent);
            header.Initialize("Shader:");
            header.transform.SetAsFirstSibling();

            foreach (var image in images)
            {
                InstantiateToggle(image);
            }

            var toggle = InstantiateControl<ToggleControl>(leftPanel);
            toggle.Initialize(
                header: "Debug",
                value: true,
                onValueChanged: SetDebug);

            SelectImage(images[0]);
            foreach (var image in images)
            {
                image.material = new Material(image.material);
            }
            SetDebug(true);
        }

        private void SelectImage(Image selectedImage)
        {
            foreach (var image in images)
            {
                image.enabled = image == selectedImage;
            }
        }

        private void InstantiateToggle(Image image)
        {
            var toggle = InstantiateControl<ToggleControl>(toggleGroup.transform);
            toggle.Initialize(
                header: image.name,
                value: image == images[0],
                onValueChanged: isOn =>
                {
                    if (isOn)
                    {
                        SelectImage(image);
                    }
                },
                toggleGroup: toggleGroup);
        }

        private void SetDebug(bool value)
        {
            if (value)
            {
                foreach (var image in images)
                {
                    image.material.EnableKeyword("_DEBUG_ON");
                    image.material.SetFloat("_DEBUG", 1);
                }
            }
            else
            {
                foreach (var image in images)
                {
                    image.material.DisableKeyword("_DEBUG_ON");
                    image.material.SetFloat("_DEBUG", 0);
                }
            }
        }
    }
}
