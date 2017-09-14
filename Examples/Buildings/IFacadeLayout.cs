using System.Collections.Generic;

namespace ProceduralToolkit.Examples
{
    public interface IFacadeLayout : IFacadePanel
    {
        List<IFacadePanel> facadePanels { get; }
        void Add(IFacadePanel facadePanel);
        void Insert(int index, IFacadePanel facadePanel);
    }
}