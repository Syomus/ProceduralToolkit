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

        public static MeshDraft FlatSphere(float radius, int horizontalSegments, int verticalSegments, bool generateUV = true)
        {
            var draft = FlatSpheroid(radius, radius, horizontalSegments, verticalSegments, generateUV);
            draft.name = "Flat sphere";
            return draft;
        }

        public static MeshDraft FlatSpheroid(float radius, float height, int horizontalSegments, int verticalSegments, bool generateUV = true)
        {
            var draft = FlatRevolutionSurface(PTUtils.PointOnSpheroid, radius, height, horizontalSegments, verticalSegments, generateUV);
            draft.name = "Flat spheroid";
            return draft;
        }

        public static MeshDraft FlatTeardrop(float radius, float height, int horizontalSegments, int verticalSegments, bool generateUV = true)
        {
            var draft = FlatRevolutionSurface(PTUtils.PointOnTeardrop, radius, height, horizontalSegments, verticalSegments, generateUV);
            draft.name = "Flat teardrop";
            return draft;
        }

        public static MeshDraft FlatRevolutionSurface(
            Func<float, float, float, float, Vector3> surfaceFunction,
            float radius,
            float height,
            int horizontalSegments,
            int verticalSegments,
            bool generateUV = true)
        {
            float horizontalSegmentAngle = 360f/horizontalSegments;
            float verticalSegmentAngle = 180f/verticalSegments;
            float currentVerticalAngle = -90;
            int horizontalCount = horizontalSegments + 1;

            var ringsVertices = new List<List<Vector3>>(verticalSegments);
            var ringsUV = new List<List<Vector2>>(verticalSegments);
            for (int y = 0; y <= verticalSegments; y++)
            {
                float currentHorizontalAngle = 0f;
                var ringVertices = new List<Vector3>(horizontalCount);
                var ringUV = new List<Vector2>(horizontalCount);

                for (int x = 0; x <= horizontalSegments; x++)
                {
                    var point = surfaceFunction(radius, height, currentHorizontalAngle, currentVerticalAngle);
                    ringVertices.Add(point);
                    if (generateUV)
                    {
                        ringUV.Add(new Vector2(1 - (float) x/horizontalSegments, (float) y/verticalSegments));
                    }
                    currentHorizontalAngle += horizontalSegmentAngle;
                }
                ringsVertices.Add(ringVertices);
                ringsUV.Add(ringUV);
                currentVerticalAngle += verticalSegmentAngle;
            }

            var draft = new MeshDraft {name = "Flat revolution surface"};
            for (int y = 0; y < ringsVertices.Count - 1; y++)
            {
                var lowerRingVertices = ringsVertices[y];
                var upperRingVertices = ringsVertices[y + 1];
                var lowerRingUV = ringsUV[y];
                var upperRingUV = ringsUV[y + 1];
                for (int x = 0; x < horizontalSegments; x++)
                {
                    Vector3 v00 = lowerRingVertices[x + 1];
                    Vector3 v01 = upperRingVertices[x + 1];
                    Vector3 v11 = upperRingVertices[x];
                    Vector3 v10 = lowerRingVertices[x];
                    Vector2 uv00 = lowerRingUV[x + 1];
                    Vector2 uv01 = upperRingUV[x + 1];
                    Vector2 uv11 = upperRingUV[x];
                    Vector2 uv10 = lowerRingUV[x];
                    draft.AddQuad(v00, v01, v11, v10, uv00, uv01, uv11, uv10);
                }
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

            for (int y = 0; y <= verticalSegments; y++)
            {
                float currentHorizontalAngle = 0f;
                for (int x = 0; x <= horizontalSegments; x++)
                {
                    Vector3 point = surfaceFunction(radius, height, currentHorizontalAngle, currentVerticalAngle);
                    draft.vertices.Add(point);
                    draft.normals.Add(point.normalized);
                    if (generateUV)
                    {
                        draft.uv.Add(new Vector2((float) x/horizontalSegments, (float) y/verticalSegments));
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
