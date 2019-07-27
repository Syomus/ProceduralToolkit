using UnityEngine;

namespace ProceduralToolkit.Buildings
{
    public class VerticalLayout : Layout
    {
        public override void Add(ILayoutElement element)
        {
            base.Add(element);
            element.origin = Vector2.up*height;
            height += element.height;
            width = Mathf.Max(width, element.width);
        }
    }
}
