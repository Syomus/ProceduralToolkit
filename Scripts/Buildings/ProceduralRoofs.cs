using System.Collections.Generic;
using ProceduralToolkit.Skeleton;
using UnityEngine;

namespace ProceduralToolkit.Buildings
{
    public abstract class ProceduralRoof : IConstructible<MeshDraft>
    {
        protected readonly List<Vector2> foundationPolygon;
        protected readonly RoofConfig roofConfig;

        protected ProceduralRoof(List<Vector2> foundationPolygon, RoofConfig roofConfig)
        {
            this.foundationPolygon = foundationPolygon;
            this.roofConfig = roofConfig;
        }

        public abstract MeshDraft Construct(Vector2 parentLayoutOrigin);

        protected MeshDraft ConstructRoofBase(out List<Vector2> roofPolygon2, out List<Vector3> roofPolygon3)
        {
            roofPolygon2 = Geometry.OffsetPolygon(foundationPolygon, roofConfig.overhang);
            roofPolygon3 = roofPolygon2.ConvertAll(v => v.ToVector3XZ());

            var roofDraft = new MeshDraft();
            if (roofConfig.thickness > 0)
            {
                roofDraft.Add(ConstructBorder(roofPolygon2, roofPolygon3, roofConfig));
            }
            if (roofConfig.overhang > 0)
            {
                roofDraft.Add(ConstructOverhang(foundationPolygon, roofPolygon3));
            }
            return roofDraft;
        }

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

        protected static float CalculateVertexHeight(Vector2 vertex, Vector2 edgeA, Vector2 edgeDirection, float roofPitch)
        {
            float distance = Distance.PointLine(vertex, edgeA, edgeDirection);
            return Mathf.Tan(roofPitch*Mathf.Deg2Rad)*distance;
        }

        protected static Vector3 CalculateRoofNormal(Vector2 edgeDirection2, float roofPitch)
        {
            return Quaternion.AngleAxis(roofPitch, edgeDirection2.ToVector3XZ())*Vector3.up;
        }
    }

    public class ProceduralFlatRoof : ProceduralRoof
    {
        private readonly Color roofColor;

        public ProceduralFlatRoof(List<Vector2> foundationPolygon, RoofConfig roofConfig, Color roofColor)
            : base(foundationPolygon, roofConfig)
        {
            this.roofColor = roofColor;
        }

        public override MeshDraft Construct(Vector2 parentLayoutOrigin)
        {
            List<Vector2> roofPolygon2;
            List<Vector3> roofPolygon3;
            var roofDraft = ConstructRoofBase(out roofPolygon2, out roofPolygon3);

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
        private const float RoofPitch = 25;

        private readonly Color roofColor;

        public ProceduralHippedRoof(List<Vector2> foundationPolygon, RoofConfig roofConfig, Color roofColor)
            : base(foundationPolygon, roofConfig)
        {
            this.roofColor = roofColor;
        }

        public override MeshDraft Construct(Vector2 parentLayoutOrigin)
        {
            List<Vector2> roofPolygon2;
            List<Vector3> roofPolygon3;
            var roofDraft = ConstructRoofBase(out roofPolygon2, out roofPolygon3);

            var skeletonGenerator = new StraightSkeletonGenerator();
            var skeleton = skeletonGenerator.Generate(roofPolygon2);

            var roofTop = new MeshDraft();
            foreach (var skeletonPolygon2 in skeleton.polygons)
            {
                roofTop.Add(ConstructContourDraft(skeletonPolygon2));
            }
            roofTop.Move(Vector3.up*roofConfig.thickness);

            roofDraft.Add(roofTop)
                .Paint(roofColor);
            return roofDraft;
        }

        private MeshDraft ConstructContourDraft(List<Vector2> skeletonPolygon2)
        {
            Vector2 edgeA = skeletonPolygon2[0];
            Vector2 edgeB = skeletonPolygon2[1];
            Vector2 edgeDirection2 = (edgeB - edgeA).normalized;
            Vector3 roofNormal = CalculateRoofNormal(edgeDirection2, RoofPitch);

            var skeletonPolygon3 = skeletonPolygon2.ConvertAll(v => v.ToVector3XZ());

            var tessellator = new Tessellator();
            tessellator.AddContour(skeletonPolygon3);
            tessellator.Tessellate(normal: Vector3.up);
            var contourDraft = tessellator.ToMeshDraft();

            for (var i = 0; i < contourDraft.vertexCount; i++)
            {
                Vector2 vertex = contourDraft.vertices[i].ToVector2XZ();
                float height = CalculateVertexHeight(vertex, edgeA, edgeDirection2, RoofPitch);
                contourDraft.vertices[i] = new Vector3(vertex.x, height, vertex.y);
                contourDraft.normals.Add(roofNormal);
            }
            return contourDraft;
        }
    }

    public class ProceduralGabledRoof : ProceduralRoof
    {
        private const float GabledRoofHeight = 2;

        private readonly Color roofColor;

        public ProceduralGabledRoof(List<Vector2> foundationPolygon, RoofConfig roofConfig, Color roofColor)
            : base(foundationPolygon, roofConfig)
        {
            this.roofColor = roofColor;
        }

        public override MeshDraft Construct(Vector2 parentLayoutOrigin)
        {
            List<Vector2> roofPolygon2;
            List<Vector3> roofPolygon3;
            var roofDraft = ConstructRoofBase(out roofPolygon2, out roofPolygon3);

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
