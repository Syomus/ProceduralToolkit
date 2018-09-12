using System.Collections.Generic;
using UnityEngine;

namespace ProceduralToolkit.Buildings
{
    public abstract class Layout : ILayout
    {
        public Vector2 origin { get; set; }
        public float width { get; set; }
        public float height { get; set; }

        public List<ILayoutElement> elements { get; protected set; }

        protected Layout()
        {
            elements = new List<ILayoutElement>();
        }

        public virtual void Add(ILayoutElement element)
        {
            elements.Add(element);
        }
    }
}
