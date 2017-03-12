using UnityEngine;
using UnityEngine.Assertions;

namespace ProceduralToolkit.Examples
{
    public class HorizontalLayout : FacadeLayout
    {
        public override MeshDraft GetMeshDraft()
        {
            Assert.IsTrue(origin.HasValue);
            Assert.IsTrue(width.HasValue);

            var draft = new MeshDraft();
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
                draft.Add(facadePanel.GetMeshDraft());
            }
            return draft;
        }
    }
}