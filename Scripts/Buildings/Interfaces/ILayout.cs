using System.Collections.Generic;

namespace ProceduralToolkit.Buildings
{
    public interface ILayout : ILayoutElement, IEnumerable<ILayoutElement>
    {
        void Add(ILayoutElement element);
    }
}
