using System.Collections.Generic;
using ProceduralToolkit.Buildings;
using UnityEngine;

namespace ProceduralToolkit.Examples.Buildings
{
    public abstract class ProceduralRoof : IConstructible<MeshDraft>
    {
        public abstract MeshDraft Construct(Vector2 parentLayoutOrigin);

        protected static MeshDraft ConstructBorder(List<Vector2> roofPolygon, RoofConfig roofConfig)
        {
            List<Vector3> lowerRing = roofPolygon.ConvertAll(v => v.ToVector3XZ());
            List<Vector3> upperRing = roofPolygon.ConvertAll(v => v.ToVector3XZ() + Vector3.up*roofConfig.thickness);
            return new MeshDraft().AddFlatQuadBand(lowerRing, upperRing, false);
        }

        protected static MeshDraft ConstructOverhang(List<Vector2> foundationPolygon, List<Vector2> roofPolygon)
        {
            List<Vector3> lowerRing = foundationPolygon.ConvertAll(v => v.ToVector3XZ());
            List<Vector3> upperRing = roofPolygon.ConvertAll(v => v.ToVector3XZ());
            return new MeshDraft().AddFlatQuadBand(lowerRing, upperRing, false);
        }
    }

    public class ProceduralFlatRoof : ProceduralRoof
    {
        private readonly List<Vector2> foundationPolygon;
        private readonly RoofConfig roofConfig;
        private readonly Color roofColor;

        public ProceduralFlatRoof(List<Vector2> foundationPolygon, RoofConfig roofConfig, Color roofColor)
        {
            this.foundationPolygon = foundationPolygon;
            this.roofConfig = roofConfig;
            this.roofColor = roofColor;
        }

        public override MeshDraft Construct(Vector2 parentLayoutOrigin)
        {
            List<Vector2> roofPolygon = Geometry.OffsetPolygon(foundationPolygon, roofConfig.overhang);

            var roofDraft = new MeshDraft();
            if (roofConfig.thickness > 0)
            {
                roofDraft.Add(ConstructBorder(roofPolygon, roofConfig));
            }

            if (roofConfig.overhang > 0)
            {
                roofDraft.Add(ConstructOverhang(foundationPolygon, roofPolygon));
            }

            Vector3 a = roofPolygon[0].ToVector3XZ() + Vector3.up*roofConfig.thickness;
            Vector3 b = roofPolygon[3].ToVector3XZ() + Vector3.up*roofConfig.thickness;
            Vector3 c = roofPolygon[2].ToVector3XZ() + Vector3.up*roofConfig.thickness;
            Vector3 d = roofPolygon[1].ToVector3XZ() + Vector3.up*roofConfig.thickness;
            roofDraft.AddQuad(a, d, c, b, Vector3.up)
                .Paint(roofColor);
            return roofDraft;
        }
    }

    public class ProceduralHippedRoof : ProceduralRoof
    {
        private const float HippedRoofHeight = 2;

        private readonly List<Vector2> foundationPolygon;
        private readonly RoofConfig roofConfig;
        private readonly Color roofColor;

        public ProceduralHippedRoof(List<Vector2> foundationPolygon, RoofConfig roofConfig, Color roofColor)
        {
            this.foundationPolygon = foundationPolygon;
            this.roofConfig = roofConfig;
            this.roofColor = roofColor;
        }

        public override MeshDraft Construct(Vector2 parentLayoutOrigin)
        {
            List<Vector2> roofPolygon = Geometry.OffsetPolygon(foundationPolygon, roofConfig.overhang);

            var roofDraft = new MeshDraft();
            if (roofConfig.thickness > 0)
            {
                roofDraft.Add(ConstructBorder(roofPolygon, roofConfig));
            }

            if (roofConfig.overhang > 0)
            {
                roofDraft.Add(ConstructOverhang(foundationPolygon, roofPolygon));
            }

            Vector3 a = roofPolygon[0].ToVector3XZ() + Vector3.up*roofConfig.thickness;
            Vector3 b = roofPolygon[3].ToVector3XZ() + Vector3.up*roofConfig.thickness;
            Vector3 c = roofPolygon[2].ToVector3XZ() + Vector3.up*roofConfig.thickness;
            Vector3 d = roofPolygon[1].ToVector3XZ() + Vector3.up*roofConfig.thickness;

            Vector3 ridgeHeight = Vector3.up*HippedRoofHeight;
            Vector3 ridgeOffset = (b - a).normalized*2;
            Vector3 ridge0 = (a + d)/2 + ridgeHeight + ridgeOffset;
            Vector3 ridge1 = (b + c)/2 + ridgeHeight - ridgeOffset;

            roofDraft.AddQuad(a, ridge0, ridge1, b, true)
                .AddTriangle(b, ridge1, c, true)
                .AddQuad(c, ridge1, ridge0, d, true)
                .AddTriangle(d, ridge0, a, true)
                .Paint(roofColor);
            return roofDraft;
        }
    }

    public class ProceduralGabledRoof : ProceduralRoof
    {
        private const float GabledRoofHeight = 2;

        private readonly List<Vector2> foundationPolygon;
        private readonly RoofConfig roofConfig;
        private readonly Color roofColor;

        public ProceduralGabledRoof(List<Vector2> foundationPolygon, RoofConfig roofConfig, Color roofColor)
        {
            this.foundationPolygon = foundationPolygon;
            this.roofConfig = roofConfig;
            this.roofColor = roofColor;
        }

        public override MeshDraft Construct(Vector2 parentLayoutOrigin)
        {
            List<Vector2> roofPolygon = Geometry.OffsetPolygon(foundationPolygon, roofConfig.overhang);

            var roofDraft = new MeshDraft();
            if (roofConfig.thickness > 0)
            {
                roofDraft.Add(ConstructBorder(roofPolygon, roofConfig));
            }

            if (roofConfig.overhang > 0)
            {
                roofDraft.Add(ConstructOverhang(foundationPolygon, roofPolygon));
            }

            Vector3 a = roofPolygon[0].ToVector3XZ() + Vector3.up*roofConfig.thickness;
            Vector3 b = roofPolygon[3].ToVector3XZ() + Vector3.up*roofConfig.thickness;
            Vector3 c = roofPolygon[2].ToVector3XZ() + Vector3.up*roofConfig.thickness;
            Vector3 d = roofPolygon[1].ToVector3XZ() + Vector3.up*roofConfig.thickness;

            Vector3 ridgeHeight = Vector3.up*GabledRoofHeight;
            Vector3 ridge0 = (a + d)/2 + ridgeHeight;
            Vector3 ridge1 = (b + c)/2 + ridgeHeight;

            roofDraft.AddQuad(a, ridge0, ridge1, b, true)
                .AddTriangle(b, ridge1, c, true)
                .AddQuad(c, ridge1, ridge0, d, true)
                .AddTriangle(d, ridge0, a, true)
                .Paint(roofColor);
            return roofDraft;
        }
    }
}
