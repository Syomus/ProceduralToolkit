using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProceduralToolkit.Buildings
{
    public abstract class Layout : ILayout
    {
        public Vector2 origin { get; set; }
        public float width { get; set; }
        public float height { get; set; }

        private readonly List<ILayoutElement> elements = new List<ILayoutElement>();

        public IEnumerator<ILayoutElement> GetEnumerator()
        {
            return elements.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public virtual void Add(ILayoutElement element)
        {
            elements.Add(element);
        }
    }
}
