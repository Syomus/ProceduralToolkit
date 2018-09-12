using System.Collections.Generic;

namespace ProceduralToolkit.Buildings
{
    public interface ILayout : ILayoutElement
    {
        List<ILayoutElement> elements { get; }
        void Add(ILayoutElement element);
    }
}
