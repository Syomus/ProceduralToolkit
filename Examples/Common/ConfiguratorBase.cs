using System.Collections.Generic;
using UnityEngine;

namespace ProceduralToolkit.Examples
{
    public class ConfiguratorBase : MonoBehaviour
    {
        private List<ColorHSV> targetPalette = new List<ColorHSV>();
        private List<ColorHSV> currentPalette = new List<ColorHSV>();

        protected static T InstantiateControl<T>(Transform parent) where T : Component
        {
            T prefab = Resources.Load<T>(typeof (T).Name);
            T control = Instantiate(prefab);
            control.transform.SetParent(parent, false);
            control.transform.localPosition = Vector3.zero;
            control.transform.localRotation = Quaternion.identity;
            control.transform.localScale = Vector3.one;
            return control;
        }

        protected static MeshDraft Platform(float radius, float heignt, int segments = 128)
        {
            float segmentAngle = 360f/segments;
            float currentAngle = 0;

            var lowerRing = new List<Vector3>(segments);
            var upperRing = new List<Vector3>(segments);
            for (var i = 0; i < segments; i++)
            {
                var lowerPoint = PTUtils.PointOnCircle3XZ(radius + heignt, currentAngle);
                lowerRing.Add(lowerPoint + Vector3.down*heignt);

                var upperPoint = PTUtils.PointOnCircle3XZ(radius, currentAngle);
                upperRing.Add(upperPoint);
                currentAngle -= segmentAngle;
            }

            var platform = new MeshDraft {name = "Platform"};
            var bottom = MeshDraft.TriangleFan(lowerRing);
            bottom.Add(MeshDraft.Band(lowerRing, upperRing));
            bottom.Paint(new Color(0.5f, 0.5f, 0.5f, 1));
            platform.Add(bottom);

            upperRing.Reverse();
            var top = MeshDraft.TriangleFan(upperRing);
            top.Paint(new Color(0.8f, 0.8f, 0.8f, 1));
            platform.Add(top);

            return platform;
        }

        protected static void AssignDraftToMeshFilter(MeshDraft draft, MeshFilter meshFilter, ref Mesh mesh)
        {
            if (mesh == null)
            {
                mesh = draft.ToMesh();
            }
            else
            {
                draft.ToMesh(ref mesh);
            }
            mesh.RecalculateBounds();
            meshFilter.sharedMesh = mesh;
        }

        protected void GeneratePalette()
        {
            targetPalette = RandomE.TetradicPalette(0.25f, 0.7f);
            targetPalette[0] = targetPalette[0].WithSV(0.8f, 0.6f);
            targetPalette[1] = targetPalette[1].WithSV(0.8f, 0.6f);
            targetPalette.Add(ColorHSV.Lerp(targetPalette[2], targetPalette[3], 0.5f));
        }

        protected ColorHSV GetMainColorHSV()
        {
            return targetPalette[0];
        }

        protected ColorHSV GetSecondaryColorHSV()
        {
            return targetPalette[1];
        }

        protected void SetupSkyboxAndPalette()
        {
            RenderSettings.skybox = new Material(RenderSettings.skybox);
            currentPalette.AddRange(targetPalette);
        }

        protected void UpdateSkybox()
        {
            LerpSkybox(RenderSettings.skybox, currentPalette, targetPalette, 2, 3, 4, Time.deltaTime);
        }

        private static void LerpSkybox(
            Material skybox,
            List<ColorHSV> currentPalette,
            List<ColorHSV> targetPalette,
            int skyColorIndex,
            int horizonColorIndex,
            int groundColorIndex,
            float t)
        {
            for (int i = 0; i < currentPalette.Count; i++)
            {
                currentPalette[i] = ColorHSV.Lerp(currentPalette[i], targetPalette[i], t);
            }

            skybox.SetColor("_SkyColor", currentPalette[skyColorIndex].ToColor());
            skybox.SetColor("_HorizonColor", currentPalette[horizonColorIndex].ToColor());
            skybox.SetColor("_GroundColor", currentPalette[groundColorIndex].ToColor());
        }
    }
}