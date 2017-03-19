using System;
using System.Collections.Generic;
using UnityEngine;

namespace ProceduralToolkit.Examples
{
    public class BuildingGeneratorBase
    {
        public static MeshDraft GenerateFacades(List<Vector2> foundationPolygon, List<FacadeLayout> facadeLayouts)
        {
            var buildingDraft = new MeshDraft {name = "Building"};
            for (int i = 0; i < foundationPolygon.Count; i++)
            {
                Vector3 a = foundationPolygon[i].ToVector3XZ();
                Vector3 b = foundationPolygon.GetLooped(i + 1).ToVector3XZ();
                var facadeLayout = facadeLayouts[i];
                buildingDraft.Add(GenerateFacade(a, b, facadeLayout));
            }
            return buildingDraft;
        }

        private static MeshDraft GenerateFacade(Vector3 a, Vector3 b, FacadeLayout facadeLayout)
        {
            var facadeDraft = facadeLayout.GetMeshDraft();

            Vector3 normal = Vector3.Cross(Vector3.up, (b - a).normalized).normalized;
            var rotation = Quaternion.LookRotation(-normal);
            facadeDraft.Rotate(rotation);
            facadeDraft.Move(a);
            return facadeDraft;
        }

        #region Horizontal

        public static HorizontalLayout ConstructHorizontal(
            List<Func<IFacadePanel>> constructors,
            float height,
            float panelWidth,
            int count)
        {
            var horizontal = new HorizontalLayout {height = height};
            for (int i = 0; i < count; i++)
            {
                IFacadePanel panel = constructors.GetRandom()();
                panel.width = panelWidth;
                horizontal.Add(panel);
            }
            return horizontal;
        }

        public static HorizontalLayout ConstructHorizontal(
            List<Func<IFacadePanel>> constructors,
            float height,
            Func<int, float> getPanelWidth,
            int count)
        {
            var horizontal = new HorizontalLayout {height = height};
            for (int i = 0; i < count; i++)
            {
                IFacadePanel panel = constructors.GetRandom()();
                panel.width = getPanelWidth(i);
                horizontal.Add(panel);
            }
            return horizontal;
        }

        public static HorizontalLayout ConstructHorizontal(
            Func<IFacadePanel> constructor,
            float height,
            float panelWidth,
            int count)
        {
            var horizontal = new HorizontalLayout {height = height};
            for (int i = 0; i < count; i++)
            {
                IFacadePanel panel = constructor();
                panel.width = panelWidth;
                horizontal.Add(panel);
            }
            return horizontal;
        }

        #endregion Horizontal

        #region Vertical

        public static VerticalLayout ConstructVertical(
            List<Func<IFacadePanel>> constructors,
            float width,
            float panelHeight,
            int count)
        {
            var vertical = new VerticalLayout {width = width};
            for (int i = 0; i < count; i++)
            {
                IFacadePanel panel = constructors.GetRandom()();
                panel.height = panelHeight;
                vertical.Add(panel);
            }
            return vertical;
        }

        public static VerticalLayout ConstructVertical(
            List<Func<IFacadePanel>> constructors,
            float width,
            Func<int, float> getPanelHeight,
            int count)
        {
            var vertical = new VerticalLayout {width = width};
            for (int i = 0; i < count; i++)
            {
                IFacadePanel panel = constructors.GetRandom()();
                panel.height = getPanelHeight(i);
                vertical.Add(panel);
            }
            return vertical;
        }

        public static VerticalLayout ConstructVertical(
            Func<IFacadePanel> constructor,
            float width,
            float panelHeight,
            int count)
        {
            var vertical = new VerticalLayout {width = width};
            for (int i = 0; i < count; i++)
            {
                IFacadePanel panel = constructor();
                panel.height = panelHeight;
                vertical.Add(panel);
            }
            return vertical;
        }

        #endregion Vertical
    }
}