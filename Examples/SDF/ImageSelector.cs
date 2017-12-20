using System.Collections.Generic;
using ProceduralToolkit.Examples.UI;
using UnityEngine;
using UnityEngine.UI;

namespace ProceduralToolkit.Examples
{
    public class ImageSelector : ConfiguratorBase
    {
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
            SelectImage(images[0]);
        }

        private void SelectImage(Image image)
        {
            for (int i = 0; i < images.Count; i++)
            {
                images[i].enabled = images[i] == image;
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
    }
}
