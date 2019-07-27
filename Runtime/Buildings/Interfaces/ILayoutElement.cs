using UnityEngine;

namespace ProceduralToolkit.Buildings
{
    public interface ILayoutElement
    {
        Vector2 origin { get; set; }
        float width { get; set; }
        float height { get; set; }
    }
}
