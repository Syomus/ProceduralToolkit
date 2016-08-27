using UnityEngine;

namespace ProceduralToolkit.Examples
{
    public enum RoofType
    {
        Flat,
        FlatOverhang,
        Gabled,
        Hipped,
    }

    public static class Roofs
    {
        private const float FlatOverhangRoofHeight = 0.3f;
        private const float GabledRoofHeight = 2;
        private const float HippedRoofHeight = 2;

        public static MeshDraft FlatRoof(Vector3 a, Vector3 b, Vector3 c, Vector3 d)
        {
            var draft = MeshDraft.Quad(a, d, c, b);
            draft.Paint(BuildingGenerator.roofColor);
            return draft;
        }

        public static MeshDraft FlatOverhangRoof(Vector3 a, Vector3 b, Vector3 c, Vector3 d)
        {
            Vector3 width = b - a;
            Vector3 length = c - b;
            Vector3 roofHeight = Vector3.up*FlatOverhangRoofHeight;
            var draft = MeshDraft.Hexahedron(width + width.normalized, length + length.normalized, roofHeight);
            draft.Move((a + c)/2 + roofHeight/2);
            draft.Paint(BuildingGenerator.roofColor);
            return draft;
        }

        public static MeshDraft GabledRoof(Vector3 a, Vector3 b, Vector3 c, Vector3 d)
        {
            Vector3 ridgeHeight = Vector3.up*GabledRoofHeight;
            Vector3 ridge0 = (a + d)/2 + ridgeHeight;
            Vector3 ridge1 = (b + c)/2 + ridgeHeight;
            var draft = MeshDraft.Quad(a, ridge0, ridge1, b);
            draft.Add(MeshDraft.Triangle(b, ridge1, c));
            draft.Add(MeshDraft.Quad(c, ridge1, ridge0, d));
            draft.Add(MeshDraft.Triangle(d, ridge0, a));
            draft.Paint(BuildingGenerator.roofColor);
            return draft;
        }

        public static MeshDraft HippedRoof(Vector3 a, Vector3 b, Vector3 c, Vector3 d)
        {
            Vector3 ridgeHeight = Vector3.up*HippedRoofHeight;
            Vector3 ridgeOffset = (b - a).normalized*2;
            Vector3 ridge0 = (a + d)/2 + ridgeHeight + ridgeOffset;
            Vector3 ridge1 = (b + c)/2 + ridgeHeight - ridgeOffset;
            var draft = MeshDraft.Quad(a, ridge0, ridge1, b);
            draft.Add(MeshDraft.Triangle(b, ridge1, c));
            draft.Add(MeshDraft.Quad(c, ridge1, ridge0, d));
            draft.Add(MeshDraft.Triangle(d, ridge0, a));
            draft.Paint(BuildingGenerator.roofColor);
            return draft;
        }
    }
}