using UnityEngine;

namespace ProceduralToolkit.Buildings
{
    public class HorizontalLayout : Layout
    {
        public override void Add(ILayoutElement element)
        {
            base.Add(element);
            element.origin = Vector2.right*width;
            width += element.width;
            height = Mathf.Max(height, element.height);
        }
    }
}
