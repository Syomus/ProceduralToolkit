using System.Collections.Generic;
using ProceduralToolkit.Buildings;
using UnityEngine;

namespace ProceduralToolkit.Examples.Buildings
{
    public abstract class ProceduralRoof : IConstructible<MeshDraft>
    {
        public abstract MeshDraft Construct(Vector2 parentLayoutOrigin);

        protected static MeshDraft ConstructBorder(List<Vector2> roofPolygon2, List<Vector3> roofPolygon3, RoofConfig roofConfig)
        {
            List<Vector3> upperRing = roofPolygon2.ConvertAll(v => v.ToVector3XZ() + Vector3.up*roofConfig.thickness);
            return new MeshDraft().AddFlatQuadBand(roofPolygon3, upperRing, false);
        }

        protected static MeshDraft ConstructOverhang(List<Vector2> foundationPolygon, List<Vector3> roofPolygon3)
        {
            List<Vector3> lowerRing = foundationPolygon.ConvertAll(v => v.ToVector3XZ());
            return new MeshDraft().AddFlatQuadBand(lowerRing, roofPolygon3, false);
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
            List<Vector2> roofPolygon2 = Geometry.OffsetPolygon(foundationPolygon, roofConfig.overhang);
            List<Vector3> roofPolygon3 = roofPolygon2.ConvertAll(v => v.ToVector3XZ());

            var roofDraft = new MeshDraft();
            if (roofConfig.thickness > 0)
            {
                roofDraft.Add(ConstructBorder(roofPolygon2, roofPolygon3, roofConfig));
            }

            if (roofConfig.overhang > 0)
            {
                roofDraft.Add(ConstructOverhang(foundationPolygon, roofPolygon3));
            }

            var tessellator = new Tessellator();
            tessellator.AddContour(roofPolygon3);
            tessellator.Tessellate(normal: Vector3.up);
            var roofTop = tessellator.ToMeshDraft()
                .Move(Vector3.up*roofConfig.thickness);
            for (var i = 0; i < roofTop.vertexCount; i++)
            {
                roofTop.normals.Add(Vector3.up);
            }
            return roofDraft.Add(roofTop)
                .Paint(roofColor);
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
            List<Vector2> roofPolygon2 = Geometry.OffsetPolygon(foundationPolygon, roofConfig.overhang);
            List<Vector3> roofPolygon3 = roofPolygon2.ConvertAll(v => v.ToVector3XZ());

            var roofDraft = new MeshDraft();
            if (roofConfig.thickness > 0)
            {
                roofDraft.Add(ConstructBorder(roofPolygon2, roofPolygon3, roofConfig));
            }

            if (roofConfig.overhang > 0)
            {
                roofDraft.Add(ConstructOverhang(foundationPolygon, roofPolygon3));
            }

            Vector3 a = roofPolygon2[0].ToVector3XZ() + Vector3.up*roofConfig.thickness;
            Vector3 b = roofPolygon2[3].ToVector3XZ() + Vector3.up*roofConfig.thickness;
            Vector3 c = roofPolygon2[2].ToVector3XZ() + Vector3.up*roofConfig.thickness;
            Vector3 d = roofPolygon2[1].ToVector3XZ() + Vector3.up*roofConfig.thickness;

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
            List<Vector2> roofPolygon2 = Geometry.OffsetPolygon(foundationPolygon, roofConfig.overhang);
            List<Vector3> roofPolygon3 = roofPolygon2.ConvertAll(v => v.ToVector3XZ());

            var roofDraft = new MeshDraft();
            if (roofConfig.thickness > 0)
            {
                roofDraft.Add(ConstructBorder(roofPolygon2, roofPolygon3, roofConfig));
            }

            if (roofConfig.overhang > 0)
            {
                roofDraft.Add(ConstructOverhang(foundationPolygon, roofPolygon3));
            }

            Vector3 a = roofPolygon2[0].ToVector3XZ() + Vector3.up*roofConfig.thickness;
            Vector3 b = roofPolygon2[3].ToVector3XZ() + Vector3.up*roofConfig.thickness;
            Vector3 c = roofPolygon2[2].ToVector3XZ() + Vector3.up*roofConfig.thickness;
            Vector3 d = roofPolygon2[1].ToVector3XZ() + Vector3.up*roofConfig.thickness;

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
