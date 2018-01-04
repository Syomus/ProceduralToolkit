using System;
using System.Collections.Generic;
using UnityEngine;

namespace ProceduralToolkit
{
    /// <summary>
    /// Helper class for procedural mesh generation
    /// </summary>
    public partial class MeshDraft
    {
        #region Mesh parts

        public static MeshDraft Band(List<Vector3> lowerRing, List<Vector3> upperRing)
        {
            var draft = new MeshDraft();
            if (lowerRing.Count < 3 || upperRing.Count < 3)
            {
                throw new ArgumentException("Array sizes must be greater than 2");
            }
            if (lowerRing.Count != upperRing.Count)
            {
                throw new ArgumentException("Array sizes must be equal");
            }

            draft.vertices.AddRange(lowerRing);
            draft.vertices.AddRange(upperRing);

            var lowerNormals = new List<Vector3>();
            var upperNormals = new List<Vector3>();
            int i0, i1, i2, i3;
            Vector3 v0, v1, v2, v3;
            for (int i = 0; i < lowerRing.Count - 1; i++)
            {
                i0 = i;
                i1 = i + lowerRing.Count;
                i2 = i + 1;
                i3 = i + 1 + lowerRing.Count;
                v0 = draft.vertices[i0];
                v1 = draft.vertices[i1];
                v2 = draft.vertices[i2];
                v3 = draft.vertices[i3];
                draft.triangles.AddRange(new[] {i0, i1, i2});
                draft.triangles.AddRange(new[] {i2, i1, i3});

                lowerNormals.Add(Vector3.Cross(v1 - v0, v2 - v0).normalized);
                upperNormals.Add(Vector3.Cross(v3 - v1, v0 - v1).normalized);
            }

            i0 = lowerRing.Count - 1;
            i1 = lowerRing.Count*2 - 1;
            i2 = 0;
            i3 = lowerRing.Count;
            v0 = draft.vertices[i0];
            v1 = draft.vertices[i1];
            v2 = draft.vertices[i2];
            v3 = draft.vertices[i3];
            draft.triangles.AddRange(new[] {i0, i1, i2});
            draft.triangles.AddRange(new[] {i2, i1, i3});

            lowerNormals.Add(Vector3.Cross(v1 - v0, v2 - v0).normalized);
            upperNormals.Add(Vector3.Cross(v3 - v1, v0 - v1).normalized);
            draft.normals.AddRange(lowerNormals);
            draft.normals.AddRange(upperNormals);

            return draft;
        }

        public static MeshDraft FlatBand(List<Vector3> lowerRing, List<Vector3> upperRing)
        {
            var draft = new MeshDraft();
            if (lowerRing.Count < 3 || upperRing.Count < 3)
            {
                throw new ArgumentException("Array sizes must be greater than 2");
            }
            if (lowerRing.Count != upperRing.Count)
            {
                throw new ArgumentException("Array sizes must be equal");
            }

            Vector3 v0, v1, v2, v3;
            for (int i = 0; i < lowerRing.Count - 1; i++)
            {
                v0 = lowerRing[i];
                v1 = upperRing[i];
                v2 = lowerRing[i + 1];
                v3 = upperRing[i + 1];
                draft.AddTriangle(v0, v1, v2);
                draft.AddTriangle(v2, v1, v3);
            }

            v0 = lowerRing[lowerRing.Count - 1];
            v1 = upperRing[upperRing.Count - 1];
            v2 = lowerRing[0];
            v3 = upperRing[0];
            draft.AddTriangle(v0, v1, v2);
            draft.AddTriangle(v2, v1, v3);

            return draft;
        }

        #endregion Mesh parts

        #region Platonic solids

        public static MeshDraft Tetrahedron(float radius, bool generateUV = true)
        {
            const float tetrahedralAngle = -19.471220333f;

            var vertex0 = new Vector3(0, radius, 0);
            var vertex1 = PTUtils.PointOnSphere(radius, 0, tetrahedralAngle);
            var vertex2 = PTUtils.PointOnSphere(radius, 120, tetrahedralAngle);
            var vertex3 = PTUtils.PointOnSphere(radius, 240, tetrahedralAngle);

            var draft = new MeshDraft {name = "Tetrahedron"};
            if (generateUV)
            {
                var uv0 = new Vector2(0, 0);
                var uv1 = new Vector2(0.5f, 1);
                var uv2 = new Vector2(1, 0);
                draft.AddTriangle(vertex2, vertex0, vertex1, uv0, uv1, uv2)
                    .AddTriangle(vertex2, vertex1, vertex3, uv0, uv1, uv2)
                    .AddTriangle(vertex3, vertex0, vertex2, uv0, uv1, uv2)
                    .AddTriangle(vertex1, vertex0, vertex3, uv0, uv1, uv2);
            }
            else
            {
                draft.AddTriangle(vertex2, vertex0, vertex1)
                    .AddTriangle(vertex2, vertex1, vertex3)
                    .AddTriangle(vertex3, vertex0, vertex2)
                    .AddTriangle(vertex1, vertex0, vertex3);
            }
            return draft;
        }

        public static MeshDraft Cube(float side, bool generateUV = true)
        {
            var draft = Hexahedron(side, side, side, generateUV);
            draft.name = "Cube";
            return draft;
        }

        public static MeshDraft Hexahedron(float width, float length, float height, bool generateUV = true)
        {
            return Hexahedron(Vector3.right*width, Vector3.forward*length, Vector3.up*height, generateUV);
        }

        public static MeshDraft Hexahedron(Vector3 width, Vector3 length, Vector3 height, bool generateUV = true)
        {
            Vector3 corner0 = -width/2 - length/2 - height/2;
            Vector3 corner1 = width/2 + length/2 + height/2;
            var draft = new MeshDraft {name = "Hexahedron"};
            if (generateUV)
            {
                Vector2 uv0 = new Vector2(0, 0);
                Vector2 uv1 = new Vector2(0, 1);
                Vector2 uv2 = new Vector2(1, 1);
                Vector2 uv3 = new Vector2(1, 0);
                draft.AddQuad(corner0, length, width, uv3, uv0, uv1, uv2)
                    .AddQuad(corner0, width, height, uv2, uv3, uv0, uv1)
                    .AddQuad(corner0, height, length, uv3, uv0, uv1, uv2)
                    .AddQuad(corner1, -width, -length, uv0, uv1, uv2, uv3)
                    .AddQuad(corner1, -height, -width, uv1, uv2, uv3, uv0)
                    .AddQuad(corner1, -length, -height, uv2, uv3, uv0, uv1);
            }
            else
            {
                draft.AddQuad(corner0, length, width)
                    .AddQuad(corner0, width, height)
                    .AddQuad(corner0, height, length)
                    .AddQuad(corner1, -width, -length)
                    .AddQuad(corner1, -height, -width)
                    .AddQuad(corner1, -length, -height);
            }
            return draft;
        }

        public static MeshDraft Octahedron(float radius, bool generateUV = true)
        {
            var draft = BiPyramid(radius, 4, radius*2, generateUV);
            draft.name = "Octahedron";
            return draft;
        }

        public static MeshDraft Dodecahedron(float radius)
        {
            const float magicAngle1 = 52.62263590f;
            const float magicAngle2 = 10.81231754f;
            const float segmentAngle = 72;

            float lowerAngle = 0;
            float upperAngle = segmentAngle/2;

            var lowerCap = new Vector3[5];
            var lowerRing = new Vector3[5];
            var upperCap = new Vector3[5];
            var upperRing = new Vector3[5];
            for (var i = 0; i < 5; i++)
            {
                lowerCap[i] = PTUtils.PointOnSphere(radius, lowerAngle, -magicAngle1);
                lowerRing[i] = PTUtils.PointOnSphere(radius, lowerAngle, -magicAngle2);
                upperCap[i] = PTUtils.PointOnSphere(radius, upperAngle, magicAngle1);
                upperRing[i] = PTUtils.PointOnSphere(radius, upperAngle, magicAngle2);
                lowerAngle += segmentAngle;
                upperAngle += segmentAngle;
            }

            var draft = new MeshDraft {name = "Dodecahedron"}
                .AddTriangleFan(upperCap)
                .AddFlatTriangleBand(upperRing, upperCap, false)
                .AddFlatTriangleBand(lowerRing, upperRing, false)
                .AddFlatTriangleBand(lowerCap, lowerRing, false);
            Array.Reverse(lowerCap);
            draft.AddTriangleFan(lowerCap);
            return draft;
        }

        public static MeshDraft Icosahedron(float radius, bool generateUV = true)
        {
            const float magicAngle = 26.56505f;
            const float segmentAngle = 72;

            float lowerAngle = 0;
            float upperAngle = segmentAngle/2;

            var lowerRing = new Vector3[5];
            var upperRing = new Vector3[5];
            for (var i = 0; i < 5; i++)
            {
                lowerRing[i] = PTUtils.PointOnSphere(radius, lowerAngle, -magicAngle);
                upperRing[i] = PTUtils.PointOnSphere(radius, upperAngle, magicAngle);
                lowerAngle += segmentAngle;
                upperAngle += segmentAngle;
            }

            var draft = new MeshDraft {name = "Icosahedron"}
                .AddBaselessPyramid(new Vector3(0, radius, 0), upperRing, generateUV)
                .AddFlatTriangleBand(lowerRing, upperRing, generateUV);
            Array.Reverse(lowerRing);
            draft.AddBaselessPyramid(new Vector3(0, -radius, 0), lowerRing, generateUV);
            return draft;
        }

        #endregion Platonic solids

        public static MeshDraft Quad(Vector3 origin, Vector3 width, Vector3 height, bool generateUV = true)
        {
            var draft = new MeshDraft {name = "Quad"};
            if (generateUV)
            {
                draft.AddQuad(origin, width, height, new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1), new Vector2(1, 0));
            }
            else
            {
                draft.AddQuad(origin, width, height);
            }
            return draft;
        }

        public static MeshDraft Quad(Vector3 vertex0, Vector3 vertex1, Vector3 vertex2, Vector3 vertex3, bool generateUV = true)
        {
            var draft = new MeshDraft {name = "Quad"};
            if (generateUV)
            {
                draft.AddQuad(vertex0, vertex1, vertex2, vertex3, new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1), new Vector2(1, 0));
            }
            else
            {
                draft.AddQuad(vertex0, vertex1, vertex2, vertex3);
            }
            return draft;
        }

        public static MeshDraft Plane(float xSize = 1, float zSize = 1, int xSegments = 1, int zSegments = 1, bool generateUV = true)
        {
            float xStep = xSize/xSegments;
            float zStep = zSize/zSegments;
            var vertexCount = (xSegments + 1)*(zSegments + 1);
            var draft = new MeshDraft
            {
                name = "Plane",
                vertices = new List<Vector3>(vertexCount),
                triangles = new List<int>(xSegments*zSegments*6),
                normals = new List<Vector3>(vertexCount)
            };

            for (int z = 0; z <= zSegments; z++)
            {
                for (int x = 0; x <= xSegments; x++)
                {
                    draft.vertices.Add(new Vector3(x*xStep, 0f, z*zStep));
                    draft.normals.Add(Vector3.up);
                    if (generateUV)
                    {
                        draft.uv.Add(new Vector2((float) x/xSegments, (float) z/zSegments));
                    }
                }
            }

            int i = 0;
            for (int z = 0; z < zSegments; z++)
            {
                for (int x = 0; x < xSegments; x++)
                {
                    draft.triangles.Add(i);
                    draft.triangles.Add(i + xSegments + 1);
                    draft.triangles.Add(i + 1);
                    draft.triangles.Add(i + 1);
                    draft.triangles.Add(i + xSegments + 1);
                    draft.triangles.Add(i + xSegments + 2);
                    i++;
                }
                i++;
            }
            return draft;
        }

        public static MeshDraft Pyramid(float radius, int segments, float height, bool generateUV = true)
        {
            float segmentAngle = 360f/segments;
            float currentAngle = 0;

            var ring = new Vector3[segments];
            for (var i = 0; i < segments; i++)
            {
                ring[i] = PTUtils.PointOnCircle3XZ(radius, currentAngle);
                currentAngle += segmentAngle;
            }

            var draft = new MeshDraft().AddBaselessPyramid(Vector3.up*height, ring, generateUV);
            Array.Reverse(ring);
            if (generateUV)
            {
                var uv = new Vector2[segments];
                for (int i = segments - 1; i >= 0; i--)
                {
                    uv[i] = PTUtils.PointOnCircle2(0.5f, currentAngle) + new Vector2(0.5f, 0.5f);
                    currentAngle -= segmentAngle;
                }
                draft.AddTriangleFan(ring, uv);
            }
            else
            {
                draft.AddTriangleFan(ring);
            }
            draft.name = "Pyramid";
            return draft;
        }

        public static MeshDraft BiPyramid(float radius, int segments, float height, bool generateUV = true)
        {
            float segmentAngle = 360f/segments;
            float currentAngle = 0;

            var ring = new Vector3[segments];
            for (var i = 0; i < segments; i++)
            {
                ring[i] = PTUtils.PointOnCircle3XZ(radius, currentAngle);
                currentAngle += segmentAngle;
            }

            var draft = new MeshDraft {name = "Bipyramid"}
                .AddBaselessPyramid(Vector3.up*height/2, ring, generateUV);
            Array.Reverse(ring);
            draft.AddBaselessPyramid(Vector3.down*height/2, ring, generateUV);
            return draft;
        }

        public static MeshDraft Prism(float radius, int segments, float height, bool generateUV = true)
        {
            float segmentAngle = 360f/segments;
            float currentAngle = 0;
            Vector3 halfHeightUp = Vector3.up*height/2;

            var lowerRing = new List<Vector3>(segments);
            var lowerDiskUV = new List<Vector2>();
            var upperRing = new List<Vector3>(segments);
            var upperDiskUV = new List<Vector2>();
            for (var i = 0; i < segments; i++)
            {
                var point = PTUtils.PointOnCircle3XZ(radius, currentAngle);
                lowerRing.Add(point - halfHeightUp);
                upperRing.Add(point + halfHeightUp);

                if (generateUV)
                {
                    Vector2 uv = PTUtils.PointOnCircle2(0.5f, currentAngle) + new Vector2(0.5f, 0.5f);
                    upperDiskUV.Add(uv);
                    uv.x = -uv.x;
                    lowerDiskUV.Add(uv);
                }
                currentAngle += segmentAngle;
            }

            var draft = new MeshDraft {name = "Prism"}
                .AddFlatQuadBand(lowerRing, upperRing, generateUV);
            lowerRing.Reverse();
            lowerDiskUV.Reverse();

            if (generateUV)
            {
                draft.AddTriangleFan(upperRing, Vector3.up, upperDiskUV)
                    .AddTriangleFan(lowerRing, Vector3.down, lowerDiskUV);
            }
            else
            {
                draft.AddTriangleFan(upperRing, Vector3.up)
                    .AddTriangleFan(lowerRing, Vector3.down);
            }
            return draft;
        }

        public static MeshDraft Cylinder(float radius, int segments, float height, bool generateUV = true)
        {
            float segmentAngle = 360f/segments;
            float currentAngle = 0;
            Vector3 halfHeightUp = Vector3.up*height/2;

            var draft = new MeshDraft {name = "Cylinder"};
            var lowerRing = new List<Vector3>(segments);
            var lowerDiskUV = new List<Vector2>();
            var upperRing = new List<Vector3>(segments);
            var upperDiskUV = new List<Vector2>();
            var strip = new List<Vector3>();
            var stripNormals = new List<Vector3>();
            var stripUV = new List<Vector2>();
            for (var i = 0; i < segments; i++)
            {
                Vector3 lowerVertex;
                Vector3 upperVertex;
                AddCylinderPoints(radius, currentAngle, halfHeightUp, generateUV,
                    ref strip, ref stripUV, ref stripNormals, out lowerVertex, out upperVertex);

                lowerVertex.z = -lowerVertex.z;
                lowerRing.Add(lowerVertex);
                upperRing.Add(upperVertex);
                if (generateUV)
                {
                    Vector2 uv = PTUtils.PointOnCircle2(0.5f, currentAngle);
                    lowerDiskUV.Add(-uv + new Vector2(0.5f, 0.5f));
                    upperDiskUV.Add(uv + new Vector2(0.5f, 0.5f));
                }
                currentAngle += segmentAngle;
            }

            Vector3 lowerSeamVertex;
            Vector3 upperSeamVertex;
            AddCylinderPoints(radius, currentAngle, halfHeightUp, generateUV,
                ref strip, ref stripUV, ref stripNormals, out lowerSeamVertex, out upperSeamVertex);

            if (generateUV)
            {
                draft.AddTriangleFan(lowerRing, Vector3.down, lowerDiskUV);
                draft.AddTriangleFan(upperRing, Vector3.up, upperDiskUV);
                draft.AddTriangleStrip(strip, stripNormals, stripUV);
            }
            else
            {
                draft.AddTriangleFan(lowerRing, Vector3.down);
                draft.AddTriangleFan(upperRing, Vector3.up);
            }
            return draft;
        }

        private static void AddCylinderPoints(float radius, float currentAngle, Vector3 halfHeightUp, bool generateUV,
            ref List<Vector3> vertices, ref List<Vector2> uv, ref List<Vector3> normals,
            out Vector3 lowerVertex, out Vector3 upperVertex)
        {
            Vector3 normal = PTUtils.PointOnCircle3XZ(1, currentAngle);
            Vector3 point = normal*radius;
            lowerVertex = point - halfHeightUp;
            upperVertex = point + halfHeightUp;

            vertices.Add(upperVertex);
            normals.Add(normal);
            vertices.Add(lowerVertex);
            normals.Add(normal);

            if (generateUV)
            {
                float u = 1 - currentAngle/360;
                uv.Add(new Vector2(u, 1));
                uv.Add(new Vector2(u, 0));
            }
        }

        public static MeshDraft FlatSphere(float radius, int horizontalSegments, int verticalSegments)
        {
            var draft = FlatSpheroid(radius, radius, horizontalSegments, verticalSegments);
            draft.name = "Flat sphere";
            return draft;
        }

        public static MeshDraft FlatSpheroid(float radius, float height, int horizontalSegments, int verticalSegments)
        {
            var draft = FlatRevolutionSurface(PTUtils.PointOnSpheroid, radius, height, horizontalSegments,
                verticalSegments);
            draft.name = "Flat spheroid";
            return draft;
        }

        public static MeshDraft FlatTeardrop(float radius, float height, int horizontalSegments, int verticalSegments)
        {
            var draft = FlatRevolutionSurface(PTUtils.PointOnTeardrop, radius, height, horizontalSegments,
                verticalSegments);
            draft.name = "Flat teardrop";
            return draft;
        }

        public static MeshDraft FlatRevolutionSurface(
            Func<float, float, float, float, Vector3> surfaceFunction,
            float radius,
            float height,
            int horizontalSegments,
            int verticalSegments)
        {
            float horizontalSegmentAngle = 360f/horizontalSegments;
            float verticalSegmentAngle = 180f/verticalSegments;
            float currentVerticalAngle = -90;

            var rings = new List<List<Vector3>>(verticalSegments);
            for (int i = 0; i <= verticalSegments; i++)
            {
                float currentHorizontalAngle = 0f;
                var ring = new List<Vector3>(horizontalSegments);
                for (int j = 0; j < horizontalSegments; j++)
                {
                    ring.Add(surfaceFunction(radius, height, currentHorizontalAngle, currentVerticalAngle));
                    currentHorizontalAngle -= horizontalSegmentAngle;
                }
                rings.Add(ring);
                currentVerticalAngle += verticalSegmentAngle;
            }

            var draft = new MeshDraft {name = "Flat revolution surface"};
            for (int i = 0; i < rings.Count - 1; i++)
            {
                draft.Add(FlatBand(rings[i], rings[i + 1]));
            }
            return draft;
        }

        public static MeshDraft Sphere(float radius, int horizontalSegments, int verticalSegments, bool generateUV = true)
        {
            var draft = Spheroid(radius, radius, horizontalSegments, verticalSegments, generateUV);
            draft.name = "Sphere";
            return draft;
        }

        public static MeshDraft Spheroid(float radius, float height, int horizontalSegments, int verticalSegments, bool generateUV = true)
        {
            var draft = RevolutionSurface(PTUtils.PointOnSpheroid, radius, height, horizontalSegments, verticalSegments, generateUV);
            draft.name = "Spheroid";
            return draft;
        }

        public static MeshDraft Teardrop(float radius, float height, int horizontalSegments, int verticalSegments, bool generateUV = true)
        {
            var draft = RevolutionSurface(PTUtils.PointOnTeardrop, radius, height, horizontalSegments, verticalSegments, generateUV);
            draft.name = "Teardrop";
            return draft;
        }

        public static MeshDraft RevolutionSurface(
            Func<float, float, float, float, Vector3> surfaceFunction,
            float radius,
            float height,
            int horizontalSegments,
            int verticalSegments,
            bool generateUV = true)
        {
            var draft = new MeshDraft {name = "Revolution surface"};

            float horizontalSegmentAngle = 360f/horizontalSegments;
            float verticalSegmentAngle = 180f/verticalSegments;
            float currentVerticalAngle = -90;

            for (int ring = 0; ring <= verticalSegments; ring++)
            {
                float currentHorizontalAngle = 0f;
                for (int i = 0; i <= horizontalSegments; i++)
                {
                    Vector3 point = surfaceFunction(radius, height, currentHorizontalAngle, currentVerticalAngle);
                    draft.vertices.Add(point);
                    draft.normals.Add(point.normalized);
                    if (generateUV)
                    {
                        draft.uv.Add(new Vector2((float) i/horizontalSegments, (float) ring/verticalSegments));
                    }
                    currentHorizontalAngle -= horizontalSegmentAngle;
                }
                currentVerticalAngle += verticalSegmentAngle;
            }

            // Extra vertices due to the uvmap seam
            int horizontalCount = horizontalSegments + 1;
            for (int ring = 0; ring < verticalSegments; ring++)
            {
                int i0, i1, i2, i3;
                for (int i = 0; i < horizontalCount - 1; i++)
                {
                    i0 = ring*horizontalCount + i;
                    i1 = (ring + 1)*horizontalCount + i;
                    i2 = ring*horizontalCount + i + 1;
                    i3 = (ring + 1)*horizontalCount + i + 1;

                    draft.triangles.Add(i0);
                    draft.triangles.Add(i1);
                    draft.triangles.Add(i2);

                    draft.triangles.Add(i2);
                    draft.triangles.Add(i1);
                    draft.triangles.Add(i3);
                }

                i0 = (ring + 1)*horizontalCount - 1;
                i1 = (ring + 2)*horizontalCount - 1;
                i2 = ring*horizontalCount;
                i3 = (ring + 1)*horizontalCount;

                draft.triangles.Add(i0);
                draft.triangles.Add(i1);
                draft.triangles.Add(i2);

                draft.triangles.Add(i2);
                draft.triangles.Add(i1);
                draft.triangles.Add(i3);
            }
            return draft;
        }

        /// <summary>
        /// Constructs partial box with specified faces
        /// </summary>
        public static MeshDraft PartialBox(Vector3 width, Vector3 length, Vector3 height, Directions parts)
        {
            Vector3 corner0 = -width/2 - length/2 - height/2;
            Vector3 corner1 = width/2 + length/2 + height/2;

            var draft = new MeshDraft {name = "Partial box"};
            if (parts.HasFlag(Directions.Left))
            {
                draft.AddQuad(corner0, height, length);
            }
            if (parts.HasFlag(Directions.Right))
            {
                draft.AddQuad(corner1, -length, -height);
            }
            if (parts.HasFlag(Directions.Down))
            {
                draft.AddQuad(corner0, length, width);
            }
            if (parts.HasFlag(Directions.Up))
            {
                draft.AddQuad(corner1, -width, -length);
            }
            if (parts.HasFlag(Directions.Back))
            {
                draft.AddQuad(corner0, width, height);
            }
            if (parts.HasFlag(Directions.Forward))
            {
                draft.AddQuad(corner1, -height, -width);
            }
            return draft;
        }
    }
}
