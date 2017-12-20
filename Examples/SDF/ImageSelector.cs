using System.Collections.Generic;
using ProceduralToolkit.Examples.UI;
using UnityEngine;
using UnityEngine.UI;

namespace ProceduralToolkit.Examples
{
    public class ImageSelector : ConfiguratorBase
    {
        public ToggleGroup toggleGroup;
        public Image starPolygonImage;
        public Image animationImage;

        private List<Image> images = new List<Image>();

        private void Awake()
        {
            images.Add(starPolygonImage);
            images.Add(animationImage);

            var header = InstantiateControl<TextControl>(toggleGroup.transform.parent);
            header.Initialize("Shader:");
            header.transform.SetAsFirstSibling();

            InstantiateToggle(starPolygonImage, "Star Polygon");
            InstantiateToggle(animationImage, "2D Animation");

            SelectImage(starPolygonImage);
        }

        private void SelectImage(Image image)
        {
            for (int i = 0; i < images.Count; i++)
            {
                images[i].enabled = images[i] == image;
            }
        }

        private void InstantiateToggle(Image image, string header)
        {
            var toggle = InstantiateControl<ToggleControl>(toggleGroup.transform);
            toggle.Initialize(
                header: header,
                value: image == starPolygonImage,
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
