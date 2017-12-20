using UnityEngine;
using UnityEngine.Assertions;

namespace ProceduralToolkit.Examples
{
    public class HorizontalLayout : FacadeLayout
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
            Assert.IsTrue(width.HasValue);

            float occupiedWidth = 0;
            int count = 0;
            for (int i = 0; i < facadePanels.Count; i++)
            {
                var facadePanel = facadePanels[i];
                if (facadePanel.width.HasValue)
                {
                    occupiedWidth += facadePanel.width.Value;
                    count++;
                }
            }

            float freeWidth = width.Value - occupiedWidth;
            Assert.IsTrue(freeWidth >= 0);
            float panelWidth = freeWidth/(facadePanels.Count - count);

            Vector2 currentPosition = origin.Value;
            for (int i = 0; i < facadePanels.Count; i++)
            {
                var facadePanel = facadePanels[i];
                facadePanel.origin = currentPosition;
                if (!facadePanel.width.HasValue)
                {
                    facadePanel.width = panelWidth;
                }
                if (!facadePanel.height.HasValue)
                {
                    facadePanel.height = height;
                }
                currentPosition += Vector2.right*facadePanel.width.Value;
            }
        }
    }
}
