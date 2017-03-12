using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace ProceduralToolkit.Examples
{
    public static class RoofGenerator
    {
        private const float GabledRoofHeight = 2;
        private const float HippedRoofHeight = 2;

        public static MeshDraft Generate(
            List<Vector2> foundationPolygon,
            float roofHeight,
            RoofConfig roofConfig)
        {
            List<Vector2> roofPolygon = OffsetPolygon(foundationPolygon, -roofConfig.overhang);

            MeshDraft roofDraft;
            switch (roofConfig.type)
            {
                case RoofType.Flat:
                    roofDraft = GenerateFlat(roofPolygon, roofConfig);
                    break;
                case RoofType.Gabled:
                    roofDraft = GenerateGabled(roofPolygon, roofConfig);
                    break;
                case RoofType.Hipped:
                    roofDraft = GenerateHipped(roofPolygon, roofConfig);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (roofConfig.thickness > 0)
            {
                roofDraft.Add(GenerateBorder(roofPolygon, roofConfig));
            }

            if (roofConfig.overhang > 0)
            {
                roofDraft.Add(GenerateOverhang(foundationPolygon, roofPolygon));
            }

            roofDraft.Move(Vector3.up*roofHeight);
            roofDraft.uv.Clear();
            return roofDraft;
        }

        private static MeshDraft GenerateFlat(List<Vector2> roofPolygon, RoofConfig roofConfig)
        {
            Vector3 a = roofPolygon[0].ToVector3XZ() + Vector3.up*roofConfig.thickness;
            Vector3 b = roofPolygon[1].ToVector3XZ() + Vector3.up*roofConfig.thickness;
            Vector3 c = roofPolygon[2].ToVector3XZ() + Vector3.up*roofConfig.thickness;
            Vector3 d = roofPolygon[3].ToVector3XZ() + Vector3.up*roofConfig.thickness;

            var roofDraft = MeshDraft.Quad(a, d, c, b);
            return roofDraft;
        }

        public static MeshDraft GenerateGabled(List<Vector2> roofPolygon, RoofConfig roofConfig)
        {
            Vector3 a = roofPolygon[0].ToVector3XZ() + Vector3.up*roofConfig.thickness;
            Vector3 b = roofPolygon[1].ToVector3XZ() + Vector3.up*roofConfig.thickness;
            Vector3 c = roofPolygon[2].ToVector3XZ() + Vector3.up*roofConfig.thickness;
            Vector3 d = roofPolygon[3].ToVector3XZ() + Vector3.up*roofConfig.thickness;

            Vector3 ridgeHeight = Vector3.up*GabledRoofHeight;
            Vector3 ridge0 = (a + d)/2 + ridgeHeight;
            Vector3 ridge1 = (b + c)/2 + ridgeHeight;

            var roofDraft = MeshDraft.Quad(a, ridge0, ridge1, b);
            roofDraft.Add(MeshDraft.Triangle(b, ridge1, c));
            roofDraft.Add(MeshDraft.Quad(c, ridge1, ridge0, d));
            roofDraft.Add(MeshDraft.Triangle(d, ridge0, a));
            return roofDraft;
        }

        public static MeshDraft GenerateHipped(List<Vector2> roofPolygon, RoofConfig roofConfig)
        {
            Vector3 a = roofPolygon[0].ToVector3XZ() + Vector3.up*roofConfig.thickness;
            Vector3 b = roofPolygon[1].ToVector3XZ() + Vector3.up*roofConfig.thickness;
            Vector3 c = roofPolygon[2].ToVector3XZ() + Vector3.up*roofConfig.thickness;
            Vector3 d = roofPolygon[3].ToVector3XZ() + Vector3.up*roofConfig.thickness;

            Vector3 ridgeHeight = Vector3.up*HippedRoofHeight;
            Vector3 ridgeOffset = (b - a).normalized*2;
            Vector3 ridge0 = (a + d)/2 + ridgeHeight + ridgeOffset;
            Vector3 ridge1 = (b + c)/2 + ridgeHeight - ridgeOffset;
            var roofDraft = MeshDraft.Quad(a, ridge0, ridge1, b);
            roofDraft.Add(MeshDraft.Triangle(b, ridge1, c));
            roofDraft.Add(MeshDraft.Quad(c, ridge1, ridge0, d));
            roofDraft.Add(MeshDraft.Triangle(d, ridge0, a));
            return roofDraft;
        }

        private static MeshDraft GenerateBorder(List<Vector2> roofPolygon, RoofConfig roofConfig)
        {
            List<Vector3> lowerRing = roofPolygon.ConvertAll(v => v.ToVector3XZ());
            List<Vector3> upperRing = roofPolygon.ConvertAll(v => v.ToVector3XZ() + Vector3.up*roofConfig.thickness);
            var border = MeshDraft.FlatBand(lowerRing, upperRing);
            return border;
        }

        private static MeshDraft GenerateOverhang(List<Vector2> foundationPolygon, List<Vector2> roofPolygon)
        {
            List<Vector3> lowerRing = foundationPolygon.ConvertAll(v => v.ToVector3XZ());
            List<Vector3> upperRing = roofPolygon.ConvertAll(v => v.ToVector3XZ());
            var overhang = MeshDraft.FlatBand(lowerRing, upperRing);
            return overhang;
        }

        private static List<Vector2> OffsetPolygon(List<Vector2> polygon, float distance)
        {
            var newPolygon = new List<Vector2>();
            for (int i = 0; i < polygon.Count; i++)
            {
                var previous = polygon.GetLooped(i - 1);
                var current = polygon[i];
                var next = polygon.GetLooped(i + 1);
                float angle;
                Vector2 bisector = GetBisector(previous, current, next, out angle);
                float hypotenuse = distance/GetBisectorSin(angle);

                newPolygon.Add(current + bisector*hypotenuse);
            }
            return newPolygon;
        }

        private static Vector2 GetBisector(Vector2 previous, Vector2 current, Vector2 next, out float angle)
        {
            Vector2 toPrevious = (previous - current).normalized;
            Vector2 toNext = (next - current).normalized;

            angle = PTUtils.Angle360(toPrevious, toNext);
            Assert.IsFalse(float.IsNaN(angle));
            return toPrevious.RotateCW(angle/2);
        }

        private static float GetBisectorSin(float angle)
        {
            if (angle > 180)
            {
                angle = 360 - angle;
            }
            return Mathf.Sin(angle/2*Mathf.Deg2Rad);
        }
    }

    public enum RoofType
    {
        Flat,
        Hipped,
        Gabled,
    }

    [Serializable]
    public class RoofConfig
    {
        public RoofType type = RoofType.Flat;
        public float thickness;
        public float overhang;
    }
}