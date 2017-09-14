using UnityEngine;
using UnityEngine.Assertions;

namespace ProceduralToolkit.Examples
{
    public class VerticalLayout : FacadeLayout
    {
        public override MeshDraft GetMeshDraft()
        {
            Assert.IsTrue(origin.HasValue);
            Assert.IsTrue(height.HasValue);

            var draft = new MeshDraft();
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
                draft.Add(facadePanel.GetMeshDraft());
            }
            return draft;
        }
    }
}