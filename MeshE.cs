using System.Collections.Generic;
using UnityEngine;

namespace ProceduralToolkit
{
    public static class MeshE
    {
        #region Primitives

        public static Mesh Triangle(Vector3 vertex0, Vector3 vertex1, Vector3 vertex2)
        {
            var normal = Vector3.Cross((vertex1 - vertex0), (vertex2 - vertex0)).normalized;
            return new Mesh
            {
                vertices = new[] {vertex0, vertex1, vertex2},
                normals = new[] {normal, normal, normal},
                uv = new[] {new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1)},
                triangles = new[] {0, 1, 2},
                name = "Triangle"
            };
        }

        public static MeshDraft TriangleDraft(Vector3 vertex0, Vector3 vertex1, Vector3 vertex2)
        {
            var normal = Vector3.Cross((vertex1 - vertex0), (vertex2 - vertex0)).normalized;
            return new MeshDraft
            {
                vertices = new List<Vector3>(3) {vertex0, vertex1, vertex2},
                normals = new List<Vector3>(3) {normal, normal, normal},
                uv = new List<Vector2>(3) {new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1)},
                triangles = new List<int>(3) {0, 1, 2},
                name = "Triangle"
            };
        }

        public static Mesh Quad(Vector3 origin, Vector3 width, Vector3 length)
        {
            var normal = Vector3.Cross(length, width).normalized;
            return new Mesh
            {
                vertices = new[] {origin, origin + length, origin + length + width, origin + width},
                normals = new[] {normal, normal, normal, normal},
                uv = new[] {new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1), new Vector2(1, 0)},
                triangles = new[] {0, 1, 2, 0, 2, 3},
                name = "Quad"
            };
        }

        public static MeshDraft QuadDraft(Vector3 origin, Vector3 width, Vector3 length)
        {
            var normal = Vector3.Cross(length, width).normalized;
            return new MeshDraft
            {
                vertices = new List<Vector3>(4) {origin, origin + length, origin + length + width, origin + width},
                normals = new List<Vector3>(4) {normal, normal, normal, normal},
                uv = new List<Vector2>(4) {new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1), new Vector2(1, 0)},
                triangles = new List<int>(6) {0, 1, 2, 0, 2, 3},
                name = "Quad"
            };
        }

        public static Mesh Quad(Vector3 vertex0, Vector3 vertex1, Vector3 vertex2, Vector3 vertex3)
        {
            var normal = Vector3.Cross((vertex1 - vertex0), (vertex2 - vertex0)).normalized;
            return new Mesh
            {
                vertices = new[] {vertex0, vertex1, vertex2, vertex3},
                normals = new[] {normal, normal, normal, normal},
                uv = new[] {new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1), new Vector2(1, 0)},
                triangles = new[] {0, 1, 2, 0, 2, 3},
                name = "Quad"
            };
        }

        public static MeshDraft QuadDraft(Vector3 vertex0, Vector3 vertex1, Vector3 vertex2, Vector3 vertex3)
        {
            var normal = Vector3.Cross((vertex1 - vertex0), (vertex2 - vertex0)).normalized;
            return new MeshDraft
            {
                vertices = new List<Vector3>(4) {vertex0, vertex1, vertex2, vertex3},
                normals = new List<Vector3>(4) {normal, normal, normal, normal},
                uv = new List<Vector2>(4) {new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1), new Vector2(1, 0)},
                triangles = new List<int>(6) {0, 1, 2, 0, 2, 3},
                name = "Quad"
            };
        }

        public static Mesh TriangleFan(List<Vector3> vertices)
        {
            return TriangleFanDraft(vertices).ToMesh();
        }

        public static MeshDraft TriangleFanDraft(List<Vector3> vertices)
        {
            var draft = new MeshDraft
            {
                vertices = vertices,
                triangles = new List<int>(vertices.Count - 2),
                normals = new List<Vector3>(vertices.Count),
                uv = new List<Vector2>(vertices.Count),
                name = "TriangleFan"
            };
            for (int i = 1; i < vertices.Count - 1; i++)
            {
                draft.triangles.Add(0);
                draft.triangles.Add(i);
                draft.triangles.Add(i + 1);
            }
            for (int i = 0; i < vertices.Count; i++)
            {
                draft.normals.Add(Vector3.up);
                draft.uv.Add(new Vector2((float) i/vertices.Count, (float) i/vertices.Count));
            }
            return draft;
        }

        public static MeshDraft BaselessPyramidDraft(float radius, int segments, float heignt, bool inverted = false)
        {
            return BaselessPyramidDraft(Vector3.zero, Vector3.up*heignt*(inverted ? -1 : 1), radius, segments, inverted);
        }

        public static MeshDraft BaselessPyramidDraft(Vector3 baseCenter, Vector3 apex, float radius, int segments,
            bool inverted = false)
        {
            var segmentAngle = Mathf.PI*2/segments*(inverted ? -1 : 1);
            var currentAngle = 0f;

            var vertices = new Vector3[segments + 1];
            vertices[0] = apex;
            for (var i = 1; i <= segments; i++)
            {
                vertices[i] = new Vector3(radius*Mathf.Sin(currentAngle), 0, radius*Mathf.Cos(currentAngle)) +
                              baseCenter;
                currentAngle += segmentAngle;
            }

            var draft = new MeshDraft {name = "BaselessPyramid"};
            for (var i = 1; i < segments; i++)
            {
                draft.Add(TriangleDraft(vertices[0], vertices[i], vertices[i + 1]));
            }
            draft.Add(TriangleDraft(vertices[0], vertices[vertices.Length - 1], vertices[1]));
            return draft;
        }

        public static MeshDraft BaselessPyramidDraft(Vector3 apex, List<Vector3> ring)
        {
            var draft = new MeshDraft {name = "BaselessPyramid"};
            for (var i = 0; i < ring.Count - 1; i++)
            {
                draft.Add(TriangleDraft(apex, ring[i], ring[i + 1]));
            }
            draft.Add(TriangleDraft(apex, ring[ring.Count - 1], ring[0]));
            return draft;
        }

        public static MeshDraft BandDraft(List<Vector3> lowerRing, List<Vector3> upperRing)
        {
            var draft = new MeshDraft {name = "Band"};
            Vector3 v0, v1, v2, v3;
            for (int i = 0; i < lowerRing.Count - 1; i++)
            {
                v0 = lowerRing[i];
                v1 = lowerRing[i + 1];
                v2 = upperRing[i];
                v3 = upperRing[i + 1];
                draft.Add(TriangleDraft(v0, v2, v1));
                draft.Add(TriangleDraft(v1, v2, v3));
            }
            v0 = lowerRing[lowerRing.Count - 1];
            v1 = lowerRing[0];
            v2 = upperRing[upperRing.Count - 1];
            v3 = upperRing[0];
            draft.Add(TriangleDraft(v0, v2, v1));
            draft.Add(TriangleDraft(v1, v2, v3));
            return draft;
        }

        #endregion Primitives

        #region Platonic solids

        public static Mesh Tetrahedron(float radius)
        {
            return TetrahedronDraft(radius).ToMesh();
        }

        public static MeshDraft TetrahedronDraft(float radius)
        {
            var tetrahedralAngle = Mathf.PI*109.471220333f/180;
            var segmentAngle = Mathf.PI*2/3;
            var currentAngle = 0f;
            var sinTetrahedralAngle = Mathf.Sin(tetrahedralAngle);
            var cosTetrahedralAngle = Mathf.Cos(tetrahedralAngle);

            var vertices = new List<Vector3>(4) {new Vector3(0, radius, 0)};
            for (var i = 1; i < 4; i++)
            {
                vertices.Add(new Vector3(radius*Mathf.Sin(currentAngle)*sinTetrahedralAngle,
                    radius*cosTetrahedralAngle,
                    radius*Mathf.Cos(currentAngle)*sinTetrahedralAngle));
                currentAngle += segmentAngle;
            }
            var draft = TriangleDraft(vertices[0], vertices[1], vertices[2]);
            draft.Add(TriangleDraft(vertices[1], vertices[3], vertices[2]));
            draft.Add(TriangleDraft(vertices[0], vertices[2], vertices[3]));
            draft.Add(TriangleDraft(vertices[0], vertices[3], vertices[1]));
            draft.name = "Tetrahedron";
            return draft;
        }

        public static Mesh Hexahedron(Vector3 width, Vector3 length, Vector3 height)
        {
            return HexahedronDraft(width, length, height).ToMesh();
        }

        public static MeshDraft HexahedronDraft(Vector3 width, Vector3 length, Vector3 height)
        {
            var corner0 = -width/2 - length/2 - height/2;
            var corner1 = width/2 + length/2 + height/2;

            var draft = QuadDraft(corner0, length, width);
            draft.Add(QuadDraft(corner0, width, height));
            draft.Add(QuadDraft(corner0, height, length));
            draft.Add(QuadDraft(corner1, -width, -length));
            draft.Add(QuadDraft(corner1, -height, -width));
            draft.Add(QuadDraft(corner1, -length, -height));
            draft.name = "Hexahedron";
            return draft;
        }

        public static Mesh Octahedron(float radius)
        {
            return OctahedronDraft(radius).ToMesh();
        }

        public static MeshDraft OctahedronDraft(float radius)
        {
            var draft = BiPyramidDraft(1, 4, 1);
            draft.name = "Octahedron";
            return draft;
        }

        public static Mesh Dodecahedron(float radius)
        {
            return DodecahedronDraft(radius).ToMesh();
        }

        public static MeshDraft DodecahedronDraft(float radius)
        {
            var magicAngle1 = Mathf.PI*52.62263590f/180;
            var magicAngle2 = Mathf.PI*10.81231754f/180;
            var segmentAngle = Mathf.PI*2/5;
            var currentAngle = 0f;
            var lowerCap = new List<Vector3>();
            var lowerRing = new List<Vector3>();
            for (var i = 0; i <= 5; i++)
            {
                lowerCap.Add(new Vector3(radius*Mathf.Sin(currentAngle)*Mathf.Cos(-magicAngle1),
                    radius*Mathf.Sin(-magicAngle1),
                    radius*Mathf.Cos(currentAngle)*Mathf.Cos(-magicAngle1)));
                lowerRing.Add(new Vector3(radius*Mathf.Sin(currentAngle)*Mathf.Cos(-magicAngle2),
                    radius*Mathf.Sin(-magicAngle2),
                    radius*Mathf.Cos(currentAngle)*Mathf.Cos(-magicAngle2)));
                currentAngle -= segmentAngle;
            }

            currentAngle = -segmentAngle/2;
            var upperCap = new List<Vector3>();
            var upperRing = new List<Vector3>();
            for (var i = 0; i <= 5; i++)
            {
                upperCap.Add(new Vector3(radius*Mathf.Sin(currentAngle)*Mathf.Cos(magicAngle1),
                    radius*Mathf.Sin(magicAngle1),
                    radius*Mathf.Cos(currentAngle)*Mathf.Cos(magicAngle1)));
                upperRing.Add(new Vector3(radius*Mathf.Sin(currentAngle)*Mathf.Cos(magicAngle2),
                    radius*Mathf.Sin(magicAngle2),
                    radius*Mathf.Cos(currentAngle)*Mathf.Cos(magicAngle2)));
                currentAngle -= segmentAngle;
            }

            var draft = TriangleFanDraft(lowerCap);
            draft.Add(BandDraft(lowerCap, lowerRing));
            draft.Add(BandDraft(lowerRing, upperRing));
            draft.Add(BandDraft(upperRing, upperCap));
            upperCap.Reverse();
            draft.Add(TriangleFanDraft(upperCap));
            draft.name = "Dodecahedron";
            return draft;
        }

        public static Mesh Icosahedron(float radius)
        {
            return IcosahedronDraft(radius).ToMesh();
        }

        public static MeshDraft IcosahedronDraft(float radius)
        {
            var magicAngle = Mathf.PI*26.56505f/180;
            var segmentAngle = Mathf.PI*72/180;

            var currentAngle = 0f;
            var upperRing = new List<Vector3>(5);
            for (var i = 0; i < 5; i++)
            {
                upperRing.Add(new Vector3(radius*Mathf.Sin(currentAngle)*Mathf.Cos(magicAngle),
                    radius*Mathf.Sin(magicAngle),
                    radius*Mathf.Cos(currentAngle)*Mathf.Cos(magicAngle)));
                currentAngle -= segmentAngle;
            }

            currentAngle = segmentAngle/2;
            var lowerRing = new List<Vector3>(5);
            for (var i = 0; i < 5; i++)
            {
                lowerRing.Add(new Vector3(radius*Mathf.Sin(currentAngle)*Mathf.Cos(-magicAngle),
                    radius*Mathf.Sin(-magicAngle),
                    radius*Mathf.Cos(currentAngle)*Mathf.Cos(-magicAngle)));
                currentAngle -= segmentAngle;
            }

            var draft = BaselessPyramidDraft(new Vector3(0, -radius, 0), lowerRing);
            draft.Add(BandDraft(lowerRing, upperRing));
            upperRing.Reverse();
            draft.Add(BaselessPyramidDraft(new Vector3(0, radius, 0), upperRing));
            draft.name = "Icosahedron";
            return draft;
        }

        #endregion Platonic solids

        public static Mesh Plane(float xSize = 1, float zSize = 1, int xSegments = 1, int zSegments = 1)
        {
            return PlaneDraft(xSize, zSize, xSegments, zSegments).ToMesh();
        }

        public static MeshDraft PlaneDraft(float xSize = 1, float zSize = 1, int xSegments = 1, int zSegments = 1)
        {
            float xStep = xSize/xSegments;
            float zStep = zSize/zSegments;
            var vertexCount = (xSegments + 1)*(zSegments + 1);
            var draft = new MeshDraft
            {
                name = "Plane",
                vertices = new List<Vector3>(vertexCount),
                triangles = new List<int>(xSegments*zSegments*6),
                normals = new List<Vector3>(vertexCount),
                uv = new List<Vector2>(vertexCount)
            };
            for (int z = 0; z <= zSegments; z++)
            {
                for (int x = 0; x <= xSegments; x++)
                {
                    draft.vertices.Add(new Vector3(x*xStep, 0f, z*zStep));
                    draft.normals.Add(Vector3.up);
                    draft.uv.Add(new Vector2((float) x/xSegments, (float) z/zSegments));
                }
            }

            var i = 0;
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

        public static Mesh Pyramid(float radius, int segments, float heignt, bool inverted = false)
        {
            return PyramidDraft(radius, segments, heignt, inverted).ToMesh();
        }

        public static MeshDraft PyramidDraft(float radius, int segments, float heignt, bool inverted = false)
        {
            var draft = BaselessPyramidDraft(radius, segments, heignt, inverted);
            var vertices = new List<Vector3>(segments);
            for (int i = draft.vertices.Count - 2; i >= 0; i -= 3)
            {
                vertices.Add(draft.vertices[i]);
            }
            draft.Add(TriangleFanDraft(vertices));
            draft.name = "Pyramid";
            return draft;
        }

        public static MeshDraft BiPyramidDraft(float radius, int segments, float heignt)
        {
            var draft = BaselessPyramidDraft(radius, segments, heignt);
            draft.Add(BaselessPyramidDraft(radius, segments, heignt, true));
            return draft;
        }

        public static Mesh Prism(float radius, int segments, float heignt)
        {
            return PrismDraft(radius, segments, heignt).ToMesh();
        }

        public static MeshDraft PrismDraft(float radius, int segments, float heignt)
        {
            var segmentAngle = Mathf.PI*2/segments;

            var currentAngle = 0f;
            var lowerRing = new List<Vector3>(segments);
            var upperRing = new List<Vector3>(segments);
            for (var i = 0; i < segments; i++)
            {
                lowerRing.Add(new Vector3(radius*Mathf.Sin(currentAngle),
                    -heignt/2,
                    radius*Mathf.Cos(currentAngle)));
                upperRing.Add(new Vector3(radius*Mathf.Sin(currentAngle),
                    heignt/2,
                    radius*Mathf.Cos(currentAngle)));
                currentAngle -= segmentAngle;
            }

            var draft = TriangleFanDraft(lowerRing);
            draft.Add(BandDraft(lowerRing, upperRing));
            upperRing.Reverse();
            draft.Add(TriangleFanDraft(upperRing));
            draft.name = "Cylinder";
            return draft;
        }
    }
}