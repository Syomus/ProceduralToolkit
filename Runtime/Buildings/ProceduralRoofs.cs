using System.Collections.Generic;
using ProceduralToolkit.Skeleton;
using UnityEngine;

namespace ProceduralToolkit.Buildings
{
    public abstract class ProceduralRoof : IConstructible<MeshDraft>
    {
        protected readonly List<Vector2> foundationPolygon;
        protected readonly RoofConfig roofConfig;
        protected readonly Color roofColor;

        protected ProceduralRoof(List<Vector2> foundationPolygon, RoofConfig roofConfig, Color roofColor)
        {
            this.foundationPolygon = foundationPolygon;
            this.roofConfig = roofConfig;
            this.roofColor = roofColor;
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

        protected static MeshDraft ConstructContourDraft(List<Vector2> skeletonPolygon2, float roofPitch)
        {
            Vector2 edgeA = skeletonPolygon2[0];
            Vector2 edgeB = skeletonPolygon2[1];
            Vector2 edgeDirection2 = (edgeB - edgeA).normalized;
            Vector3 roofNormal = CalculateRoofNormal(edgeDirection2, roofPitch);

            var skeletonPolygon3 = skeletonPolygon2.ConvertAll(v => v.ToVector3XZ());

            var tessellator = new Tessellator();
            tessellator.AddContour(skeletonPolygon3);
            tessellator.Tessellate(normal: Vector3.up);
            var contourDraft = tessellator.ToMeshDraft();

            for (var i = 0; i < contourDraft.vertexCount; i++)
            {
                Vector2 vertex = contourDraft.vertices[i].ToVector2XZ();
                float height = CalculateVertexHeight(vertex, edgeA, edgeDirection2, roofPitch);
                contourDraft.vertices[i] = new Vector3(vertex.x, height, vertex.y);
                contourDraft.normals.Add(roofNormal);
            }
            return contourDraft;
        }

        protected static MeshDraft ConstructGableDraft(List<Vector2> skeletonPolygon2, float roofPitch)
        {
            Vector2 edgeA2 = skeletonPolygon2[0];
            Vector2 edgeB2 = skeletonPolygon2[1];
            Vector2 peak2 = skeletonPolygon2[2];
            Vector2 edgeDirection2 = (edgeB2 - edgeA2).normalized;

            float peakHeight = CalculateVertexHeight(peak2, edgeA2, edgeDirection2, roofPitch);
            Vector3 edgeA3 = edgeA2.ToVector3XZ();
            Vector3 edgeB3 = edgeB2.ToVector3XZ();
            Vector3 peak3 = new Vector3(peak2.x, peakHeight, peak2.y);
            Vector2 gableTop2 = Closest.PointSegment(peak2, edgeA2, edgeB2);
            Vector3 gableTop3 = new Vector3(gableTop2.x, peakHeight, gableTop2.y);

            return new MeshDraft().AddTriangle(edgeA3, edgeB3, gableTop3, true)
                .AddTriangle(edgeA3, gableTop3, peak3, true)
                .AddTriangle(edgeB3, peak3, gableTop3, true);
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
        public ProceduralFlatRoof(List<Vector2> foundationPolygon, RoofConfig roofConfig, Color roofColor)
            : base(foundationPolygon, roofConfig, roofColor)
        {
        }

        public override MeshDraft Construct(Vector2 parentLayoutOrigin)
        {
            var roofDraft = ConstructRoofBase(out List<Vector2> roofPolygon2, out List<Vector3> roofPolygon3);

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

        public ProceduralHippedRoof(List<Vector2> foundationPolygon, RoofConfig roofConfig, Color roofColor)
            : base(foundationPolygon, roofConfig, roofColor)
        {
        }

        public override MeshDraft Construct(Vector2 parentLayoutOrigin)
        {
            var roofDraft = ConstructRoofBase(out List<Vector2> roofPolygon2, out List<Vector3> roofPolygon3);

            var skeletonGenerator = new StraightSkeletonGenerator();
            var skeleton = skeletonGenerator.Generate(roofPolygon2);

            var roofTop = new MeshDraft();
            foreach (var skeletonPolygon2 in skeleton.polygons)
            {
                roofTop.Add(ConstructContourDraft(skeletonPolygon2, RoofPitch));
            }
            roofTop.Move(Vector3.up*roofConfig.thickness);

            roofDraft.Add(roofTop)
                .Paint(roofColor);
            return roofDraft;
        }
    }

    public class ProceduralGabledRoof : ProceduralRoof
    {
        private const float RoofPitch = 25;

        public ProceduralGabledRoof(List<Vector2> foundationPolygon, RoofConfig roofConfig, Color roofColor)
            : base(foundationPolygon, roofConfig, roofColor)
        {
        }

        public override MeshDraft Construct(Vector2 parentLayoutOrigin)
        {
            var roofDraft = ConstructRoofBase(out List<Vector2> roofPolygon2, out List<Vector3> roofPolygon3);

            var skeletonGenerator = new StraightSkeletonGenerator();
            var skeleton = skeletonGenerator.Generate(roofPolygon2);

            var roofTop = new MeshDraft();
            foreach (var skeletonPolygon2 in skeleton.polygons)
            {
                if (skeletonPolygon2.Count == 3)
                {
                    roofTop.Add(ConstructGableDraft(skeletonPolygon2, RoofPitch));
                }
                else
                {
                    roofTop.Add(ConstructContourDraft(skeletonPolygon2, RoofPitch));
                }
            }
            roofTop.Move(Vector3.up*roofConfig.thickness);

            roofDraft.Add(roofTop)
                .Paint(roofColor);
            return roofDraft;
        }
    }
}
