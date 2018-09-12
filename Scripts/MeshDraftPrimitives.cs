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

        /// <summary>
        /// Constructs a tetrahedron draft
        /// </summary>
        public static MeshDraft Tetrahedron(float radius, bool generateUV = true)
        {
            const float tetrahedralAngle = -19.471220333f;

            var vertex0 = new Vector3(0, radius, 0);
            var vertex1 = Geometry.PointOnSphere(radius, 0, tetrahedralAngle);
            var vertex2 = Geometry.PointOnSphere(radius, 120, tetrahedralAngle);
            var vertex3 = Geometry.PointOnSphere(radius, 240, tetrahedralAngle);

            var draft = new MeshDraft {name = "Tetrahedron"};
            if (generateUV)
            {
                var uv0 = new Vector2(0, 0);
                var uv1 = new Vector2(0.5f, 1);
                var uv2 = new Vector2(1, 0);
                draft.AddTriangle(vertex2, vertex0, vertex1, true, uv0, uv1, uv2)
                    .AddTriangle(vertex2, vertex1, vertex3, true, uv0, uv1, uv2)
                    .AddTriangle(vertex3, vertex0, vertex2, true, uv0, uv1, uv2)
                    .AddTriangle(vertex1, vertex0, vertex3, true, uv0, uv1, uv2);
            }
            else
            {
                draft.AddTriangle(vertex2, vertex0, vertex1, true)
                    .AddTriangle(vertex2, vertex1, vertex3, true)
                    .AddTriangle(vertex3, vertex0, vertex2, true)
                    .AddTriangle(vertex1, vertex0, vertex3, true);
            }
            return draft;
        }

        /// <summary>
        /// Constructs a cube draft
        /// </summary>
        public static MeshDraft Cube(float side, bool generateUV = true)
        {
            var draft = Hexahedron(side, side, side, generateUV);
            draft.name = "Cube";
            return draft;
        }

        /// <summary>
        /// Constructs a hexahedron draft
        /// </summary>
        public static MeshDraft Hexahedron(float width, float length, float height, bool generateUV = true)
        {
            return Hexahedron(Vector3.right*width, Vector3.forward*length, Vector3.up*height, generateUV);
        }

        /// <summary>
        /// Constructs a hexahedron draft
        /// </summary>
        public static MeshDraft Hexahedron(Vector3 width, Vector3 length, Vector3 height, bool generateUV = true)
        {
            Vector3 v000 = -width/2 - length/2 - height/2;
            Vector3 v001 = v000 + height;
            Vector3 v010 = v000 + width;
            Vector3 v011 = v000 + width + height;
            Vector3 v100 = v000 + length;
            Vector3 v101 = v000 + length + height;
            Vector3 v110 = v000 + width + length;
            Vector3 v111 = v000 + width + length + height;

            var draft = new MeshDraft {name = "Hexahedron"};
            if (generateUV)
            {
                Vector2 uv0 = new Vector2(0, 0);
                Vector2 uv1 = new Vector2(0, 1);
                Vector2 uv2 = new Vector2(1, 1);
                Vector2 uv3 = new Vector2(1, 0);
                draft.AddQuad(v100, v101, v001, v000, Vector3.left, uv0, uv1, uv2, uv3)
                    .AddQuad(v010, v011, v111, v110, Vector3.right, uv0, uv1, uv2, uv3)
                    .AddQuad(v010, v110, v100, v000, Vector3.down, uv0, uv1, uv2, uv3)
                    .AddQuad(v111, v011, v001, v101, Vector3.up, uv0, uv1, uv2, uv3)
                    .AddQuad(v000, v001, v011, v010, Vector3.back, uv0, uv1, uv2, uv3)
                    .AddQuad(v110, v111, v101, v100, Vector3.forward, uv0, uv1, uv2, uv3);
            }
            else
            {
                draft.AddQuad(v100, v101, v001, v000, Vector3.left)
                    .AddQuad(v010, v011, v111, v110, Vector3.right)
                    .AddQuad(v010, v110, v100, v000, Vector3.down)
                    .AddQuad(v111, v011, v001, v101, Vector3.up)
                    .AddQuad(v000, v001, v011, v010, Vector3.back)
                    .AddQuad(v110, v111, v101, v100, Vector3.forward);
            }
            return draft;
        }

        /// <summary>
        /// Constructs a octahedron draft
        /// </summary>
        public static MeshDraft Octahedron(float radius, bool generateUV = true)
        {
            var draft = BiPyramid(radius, 4, radius*2, generateUV);
            draft.name = "Octahedron";
            return draft;
        }

        /// <summary>
        /// Constructs a dodecahedron draft
        /// </summary>
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
                lowerCap[i] = Geometry.PointOnSphere(radius, lowerAngle, -magicAngle1);
                lowerRing[i] = Geometry.PointOnSphere(radius, lowerAngle, -magicAngle2);
                upperCap[i] = Geometry.PointOnSphere(radius, upperAngle, magicAngle1);
                upperRing[i] = Geometry.PointOnSphere(radius, upperAngle, magicAngle2);
                lowerAngle += segmentAngle;
                upperAngle += segmentAngle;
            }

            var draft = new MeshDraft {name = "Dodecahedron"}
                .AddTriangleFan(upperCap, Vector3.up)
                .AddFlatTriangleBand(upperRing, upperCap, false)
                .AddFlatTriangleBand(lowerRing, upperRing, false)
                .AddFlatTriangleBand(lowerCap, lowerRing, false)
                .AddTriangleFan(lowerCap, Vector3.down, true);
            return draft;
        }

        /// <summary>
        /// Constructs a icosahedron draft
        /// </summary>
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
                lowerRing[i] = Geometry.PointOnSphere(radius, lowerAngle, -magicAngle);
                upperRing[i] = Geometry.PointOnSphere(radius, upperAngle, magicAngle);
                lowerAngle += segmentAngle;
                upperAngle += segmentAngle;
            }

            var draft = new MeshDraft {name = "Icosahedron"}
                .AddBaselessPyramid(new Vector3(0, radius, 0), upperRing, generateUV)
                .AddFlatTriangleBand(lowerRing, upperRing, generateUV)
                .AddBaselessPyramid(new Vector3(0, -radius, 0), lowerRing, generateUV, true);
            return draft;
        }

        #endregion Platonic solids

        /// <summary>
        /// Constructs a quad draft
        /// </summary>
        public static MeshDraft Quad(Vector3 origin, Vector3 width, Vector3 height, bool generateUV = true)
        {
            var draft = new MeshDraft {name = "Quad"};
            if (generateUV)
            {
                draft.AddQuad(origin, width, height, true, new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1), new Vector2(1, 0));
            }
            else
            {
                draft.AddQuad(origin, width, height, true);
            }
            return draft;
        }

        /// <summary>
        /// Constructs a quad draft
        /// </summary>
        public static MeshDraft Quad(Vector3 vertex0, Vector3 vertex1, Vector3 vertex2, Vector3 vertex3, bool generateUV = true)
        {
            var draft = new MeshDraft {name = "Quad"};
            if (generateUV)
            {
                draft.AddQuad(vertex0, vertex1, vertex2, vertex3, true, new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1), new Vector2(1, 0));
            }
            else
            {
                draft.AddQuad(vertex0, vertex1, vertex2, vertex3, true);
            }
            return draft;
        }

        /// <summary>
        /// Constructs a plane draft
        /// </summary>
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

        /// <summary>
        /// Constructs a pyramid draft
        /// </summary>
        public static MeshDraft Pyramid(float radius, int segments, float height, bool generateUV = true)
        {
            float segmentAngle = 360f/segments;
            float currentAngle = 0;

            var ring = new Vector3[segments];
            for (var i = 0; i < segments; i++)
            {
                ring[i] = Geometry.PointOnCircle3XZ(radius, currentAngle);
                currentAngle += segmentAngle;
            }

            var draft = new MeshDraft().AddBaselessPyramid(Vector3.up*height, ring, generateUV);
            if (generateUV)
            {
                var fanUV = new Vector2[segments];
                currentAngle = 0;
                for (var i = 0; i < segments; i++)
                {
                    Vector2 uv = Geometry.PointOnCircle2(0.5f, currentAngle) + new Vector2(0.5f, 0.5f);
                    uv.x = 1 - uv.x;
                    fanUV[i] = uv;
                    currentAngle += segmentAngle;
                }
                draft.AddTriangleFan(ring, Vector3.down, fanUV, true);
            }
            else
            {
                draft.AddTriangleFan(ring, Vector3.down, true);
            }
            draft.name = "Pyramid";
            return draft;
        }

        /// <summary>
        /// Constructs a bipyramid draft
        /// </summary>
        public static MeshDraft BiPyramid(float radius, int segments, float height, bool generateUV = true)
        {
            float segmentAngle = 360f/segments;
            float currentAngle = 0;

            var ring = new Vector3[segments];
            for (var i = 0; i < segments; i++)
            {
                ring[i] = Geometry.PointOnCircle3XZ(radius, currentAngle);
                currentAngle += segmentAngle;
            }

            var draft = new MeshDraft {name = "Bipyramid"}
                .AddBaselessPyramid(Vector3.up*height/2, ring, generateUV)
                .AddBaselessPyramid(Vector3.down*height/2, ring, generateUV, true);
            return draft;
        }

        /// <summary>
        /// Constructs a prism draft
        /// </summary>
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
                var point = Geometry.PointOnCircle3XZ(radius, currentAngle);
                lowerRing.Add(point - halfHeightUp);
                upperRing.Add(point + halfHeightUp);

                if (generateUV)
                {
                    Vector2 uv = Geometry.PointOnCircle2(0.5f, currentAngle) + new Vector2(0.5f, 0.5f);
                    upperDiskUV.Add(uv);
                    uv.x = 1 - uv.x;
                    lowerDiskUV.Add(uv);
                }
                currentAngle += segmentAngle;
            }

            var draft = new MeshDraft {name = "Prism"}
                .AddFlatQuadBand(lowerRing, upperRing, generateUV);

            if (generateUV)
            {
                draft.AddTriangleFan(upperRing, Vector3.up, upperDiskUV)
                    .AddTriangleFan(lowerRing, Vector3.down, lowerDiskUV, true);
            }
            else
            {
                draft.AddTriangleFan(upperRing, Vector3.up)
                    .AddTriangleFan(lowerRing, Vector3.down, true);
            }
            return draft;
        }

        /// <summary>
        /// Constructs a cylinder draft
        /// </summary>
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

                lowerRing.Add(lowerVertex);
                upperRing.Add(upperVertex);
                if (generateUV)
                {
                    Vector2 uv = Geometry.PointOnCircle2(0.5f, currentAngle) + new Vector2(0.5f, 0.5f);
                    upperDiskUV.Add(uv);
                    uv.x = 1 - uv.x;
                    lowerDiskUV.Add(uv);
                }
                currentAngle += segmentAngle;
            }

            Vector3 lowerSeamVertex;
            Vector3 upperSeamVertex;
            AddCylinderPoints(radius, currentAngle, halfHeightUp, generateUV,
                ref strip, ref stripUV, ref stripNormals, out lowerSeamVertex, out upperSeamVertex);

            if (generateUV)
            {
                draft.AddTriangleFan(lowerRing, Vector3.down, lowerDiskUV, true);
                draft.AddTriangleFan(upperRing, Vector3.up, upperDiskUV);
                draft.AddTriangleStrip(strip, stripNormals, stripUV);
            }
            else
            {
                draft.AddTriangleFan(lowerRing, Vector3.down, true);
                draft.AddTriangleFan(upperRing, Vector3.up);
            }
            return draft;
        }

        private static void AddCylinderPoints(float radius, float currentAngle, Vector3 halfHeightUp, bool generateUV,
            ref List<Vector3> vertices, ref List<Vector2> uv, ref List<Vector3> normals,
            out Vector3 lowerVertex, out Vector3 upperVertex)
        {
            Vector3 normal = Geometry.PointOnCircle3XZ(1, currentAngle);
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

        /// <summary>
        /// Constructs a flat sphere draft
        /// </summary>
        public static MeshDraft FlatSphere(float radius, int horizontalSegments, int verticalSegments, bool generateUV = true)
        {
            var draft = FlatSpheroid(radius, radius, horizontalSegments, verticalSegments, generateUV);
            draft.name = "Flat sphere";
            return draft;
        }

        /// <summary>
        /// Constructs a flat spheroid draft
        /// </summary>
        public static MeshDraft FlatSpheroid(float radius, float height, int horizontalSegments, int verticalSegments, bool generateUV = true)
        {
            var draft = FlatRevolutionSurface(Geometry.PointOnSpheroid, radius, height, horizontalSegments, verticalSegments, generateUV);
            draft.name = "Flat spheroid";
            return draft;
        }

        /// <summary>
        /// Constructs a flat teardrop draft
        /// </summary>
        public static MeshDraft FlatTeardrop(float radius, float height, int horizontalSegments, int verticalSegments, bool generateUV = true)
        {
            var draft = FlatRevolutionSurface(Geometry.PointOnTeardrop, radius, height, horizontalSegments, verticalSegments, generateUV);
            draft.name = "Flat teardrop";
            return draft;
        }

        /// <summary>
        /// Constructs a flat revolution surface draft
        /// </summary>
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
                    draft.AddQuad(v00, v01, v11, v10, true, uv00, uv01, uv11, uv10);
                }
            }
            return draft;
        }

        /// <summary>
        /// Constructs a sphere draft
        /// </summary>
        public static MeshDraft Sphere(float radius, int horizontalSegments, int verticalSegments, bool generateUV = true)
        {
            var draft = Spheroid(radius, radius, horizontalSegments, verticalSegments, generateUV);
            draft.name = "Sphere";
            return draft;
        }

        /// <summary>
        /// Constructs a spheroid draft
        /// </summary>
        public static MeshDraft Spheroid(float radius, float height, int horizontalSegments, int verticalSegments, bool generateUV = true)
        {
            var draft = RevolutionSurface(Geometry.PointOnSpheroid, radius, height, horizontalSegments, verticalSegments, generateUV);
            draft.name = "Spheroid";
            return draft;
        }

        /// <summary>
        /// Constructs a teardrop draft
        /// </summary>
        public static MeshDraft Teardrop(float radius, float height, int horizontalSegments, int verticalSegments, bool generateUV = true)
        {
            var draft = RevolutionSurface(Geometry.PointOnTeardrop, radius, height, horizontalSegments, verticalSegments, generateUV);
            draft.name = "Teardrop";
            return draft;
        }

        /// <summary>
        /// Constructs a revolution surface draft
        /// </summary>
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
                for (int i = 0; i < horizontalCount - 1; i++)
                {
                    int i0 = ring*horizontalCount + i;
                    int i1 = (ring + 1)*horizontalCount + i;
                    int i2 = ring*horizontalCount + i + 1;
                    int i3 = (ring + 1)*horizontalCount + i + 1;

                    draft.triangles.Add(i0);
                    draft.triangles.Add(i1);
                    draft.triangles.Add(i2);

                    draft.triangles.Add(i2);
                    draft.triangles.Add(i1);
                    draft.triangles.Add(i3);
                }
            }
            return draft;
        }

        /// <summary>
        /// Constructs a partial box with specified faces
        /// </summary>
        public static MeshDraft PartialBox(Vector3 width, Vector3 depth, Vector3 height, Directions parts, bool generateUV = true)
        {
            Vector3 v000 = -width/2 - depth/2 - height/2;
            Vector3 v001 = v000 + height;
            Vector3 v010 = v000 + width;
            Vector3 v011 = v000 + width + height;
            Vector3 v100 = v000 + depth;
            Vector3 v101 = v000 + depth + height;
            Vector3 v110 = v000 + width + depth;
            Vector3 v111 = v000 + width + depth + height;

            var draft = new MeshDraft {name = "Partial box"};

            if (generateUV)
            {
                Vector2 uv0 = new Vector2(0, 0);
                Vector2 uv1 = new Vector2(0, 1);
                Vector2 uv2 = new Vector2(1, 1);
                Vector2 uv3 = new Vector2(1, 0);

                if (parts.HasFlag(Directions.Left)) draft.AddQuad(v100, v101, v001, v000, true, uv0, uv1, uv2, uv3);
                if (parts.HasFlag(Directions.Right)) draft.AddQuad(v010, v011, v111, v110, true, uv0, uv1, uv2, uv3);
                if (parts.HasFlag(Directions.Down)) draft.AddQuad(v010, v110, v100, v000, true, uv0, uv1, uv2, uv3);
                if (parts.HasFlag(Directions.Up)) draft.AddQuad(v111, v011, v001, v101, true, uv0, uv1, uv2, uv3);
                if (parts.HasFlag(Directions.Back)) draft.AddQuad(v000, v001, v011, v010, true, uv0, uv1, uv2, uv3);
                if (parts.HasFlag(Directions.Forward)) draft.AddQuad(v110, v111, v101, v100, true, uv0, uv1, uv2, uv3);
            }
            else
            {
                if (parts.HasFlag(Directions.Left)) draft.AddQuad(v100, v101, v001, v000, true);
                if (parts.HasFlag(Directions.Right)) draft.AddQuad(v010, v011, v111, v110, true);
                if (parts.HasFlag(Directions.Down)) draft.AddQuad(v010, v110, v100, v000, true);
                if (parts.HasFlag(Directions.Up)) draft.AddQuad(v111, v011, v001, v101, true);
                if (parts.HasFlag(Directions.Back)) draft.AddQuad(v000, v001, v011, v010, true);
                if (parts.HasFlag(Directions.Forward)) draft.AddQuad(v110, v111, v101, v100, true);
            }
            return draft;
        }
    }
}
