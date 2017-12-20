using UnityEngine;
using UnityEngine.Assertions;

namespace ProceduralToolkit.Examples
{
    public class VerticalLayout : FacadeLayout
    {
        public override MeshDraft GetMeshDraft()
        {
            UpdateLayout();

            var meshDraft = new MeshDraft();
            for (int i = 0; i < facadePanels.Count; i++)
            {
                meshDraft.Add(facadePanels[i].GetMeshDraft());
            }
            return meshDraft;
        }

        public override CompoundMeshDraft GetCompoundMeshDraft()
        {
            UpdateLayout();

            var compoundMeshDraft = new CompoundMeshDraft();
            for (int i = 0; i < facadePanels.Count; i++)
            {
                compoundMeshDraft.Add(facadePanels[i].GetCompoundMeshDraft());
            }
            return compoundMeshDraft;
        }

        private void UpdateLayout()
        {
            Assert.IsTrue(origin.HasValue);
            Assert.IsTrue(height.HasValue);

            float occupiedHeight = 0;
            int count = 0;
            for (int i = 0; i < facadePanels.Count; i++)
            {
                var facadePanel = facadePanels[i];
                if (facadePanel.height.HasValue)
                {
                    occupiedHeight += facadePanel.height.Value;
                    count++;
                }
            }

            float freeHeight = height.Value - occupiedHeight;
            Assert.IsTrue(freeHeight >= 0);
            float panelHeight = freeHeight/(facadePanels.Count - count);

            Vector2 currentPosition = origin.Value;
            for (int i = 0; i < facadePanels.Count; i++)
            {
                var facadePanel = facadePanels[i];
                facadePanel.origin = currentPosition;
                if (!facadePanel.width.HasValue)
                {
                    facadePanel.width = width;
                }
                if (!facadePanel.height.HasValue)
                {
                    facadePanel.height = panelHeight;
                }
                currentPosition += Vector2.up*facadePanel.height.Value;
            }
        }
    }
}
