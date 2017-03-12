using System.Collections.Generic;
using UnityEngine;

namespace ProceduralToolkit.Examples
{
    public abstract class FacadeLayout : IFacadeLayout
    {
        public Vector2? origin { get; set; }
        public float? width { get; set; }
        public float? height { get; set; }

        public abstract MeshDraft GetMeshDraft();

        public List<IFacadePanel> facadePanels { get; protected set; }

        protected FacadeLayout()
        {
            facadePanels = new List<IFacadePanel>();
        }

        public void Add(IFacadePanel facadePanel)
        {
            facadePanels.Add(facadePanel);
        }

        public void Insert(int index, IFacadePanel facadePanel)
        {
            facadePanels.Insert(index, facadePanel);
        }
    }
}