using System.Collections.Generic;
using UnityEngine;

namespace ProceduralToolkit
{
    /// <summary>
    /// Mesh extensions and constructors for primitives
    /// </summary>
    public static class MeshE
    {
        #region Primitives

        #region Mesh parts

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
            var normal = Vector3.Cross(vertices[1] - vertices[0], vertices[2] - vertices[0]).normalized;
            for (int i = 0; i < vertices.Count; i++)
            {
                draft.normals.Add(normal);
                draft.uv.Add(new Vector2((float) i/vertices.Count, (float) i/vertices.Count));
            }
            return draft;
        }

        public static Mesh TriangleStrip(List<Vector3> vertices)
        {
            return TriangleStripDraft(vertices).ToMesh();
        }

        public static MeshDraft TriangleStripDraft(List<Vector3> vertices)
        {
            var draft = new MeshDraft
            {
                vertices = vertices,
                triangles = new List<int>(vertices.Count - 2),
                normals = new List<Vector3>(vertices.Count),
                uv = new List<Vector2>(vertices.Count),
                name = "TriangleStrip"
            };
            for (int i = 0, j = 1, k = 2; i < vertices.Count - 2; i++, j += i%2*2, k += (i + 1)%2*2)
            {
                draft.triangles.Add(i);
                draft.triangles.Add(j);
                draft.triangles.Add(k);
            }
            var normal = Vector3.Cross(vertices[1] - vertices[0], vertices[2] - vertices[0]).normalized;
            for (int i = 0; i < vertices.Count; i++)
            {
                draft.normals.Add(normal);
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
                vertices[i] = PTUtils.PointOnCircle3(radius, currentAngle) + baseCenter;
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
            if (lowerRing.Count < 3 || upperRing.Count < 3)
            {
                Debug.LogError("Array sizes must be greater than 2");
                return draft;
            }
            if (lowerRing.Count != upperRing.Count)
            {
                Debug.LogError("Array sizes must be equal");
                return draft;
            }

            draft.vertices.AddRange(lowerRing);
            draft.vertices.AddRange(upperRing);

            var lowerNormals = new List<Vector3>();
            var upperNormals = new List<Vector3>();
            var lowerUv = new List<Vector2>();
            var upperUv = new List<Vector2>();
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

                var u = (float) i/(lowerRing.Count - 1);
                lowerUv.Add(new Vector2(u, 0));
                upperUv.Add(new Vector2(u, 1));
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

            lowerUv.Add(new Vector2(1, 0));
            upperUv.Add(new Vector2(1, 1));
            draft.uv.AddRange(lowerUv);
            draft.uv.AddRange(upperUv);

            return draft;
        }

        public static MeshDraft FlatBandDraft(List<Vector3> lowerRing, List<Vector3> upperRing)
        {
            var draft = new MeshDraft {name = "Flat band"};
            if (lowerRing.Count < 3 || upperRing.Count < 3)
            {
                Debug.LogError("Array sizes must be greater than 2");
                return draft;
            }
            if (lowerRing.Count != upperRing.Count)
            {
                Debug.LogError("Array sizes must be equal");
                return draft;
            }

            Vector3 v0, v1, v2, v3;
            for (int i = 0; i < lowerRing.Count - 1; i++)
            {
                v0 = lowerRing[i];
                v1 = upperRing[i];
                v2 = lowerRing[i + 1];
                v3 = upperRing[i + 1];
                draft.Add(TriangleDraft(v0, v1, v2));
                draft.Add(TriangleDraft(v2, v1, v3));
            }

            v0 = lowerRing[lowerRing.Count - 1];
            v1 = upperRing[upperRing.Count - 1];
            v2 = lowerRing[0];
            v3 = upperRing[0];
            draft.Add(TriangleDraft(v0, v1, v2));
            draft.Add(TriangleDraft(v2, v1, v3));

            return draft;
        }

        #endregion Mesh parts

        #region Platonic solids

        public static Mesh Tetrahedron(float radius)
        {
            return TetrahedronDraft(radius).ToMesh();
        }

        public static MeshDraft TetrahedronDraft(float radius)
        {
            var tetrahedralAngle = Mathf.PI*-19.471220333f/180;
            var segmentAngle = Mathf.PI*2/3;
            var currentAngle = 0f;

            var vertices = new List<Vector3>(4) {new Vector3(0, radius, 0)};
            for (var i = 1; i < 4; i++)
            {
                vertices.Add(PTUtils.PointOnSphere(radius, currentAngle, tetrahedralAngle));
                currentAngle += segmentAngle;
            }
            var draft = TriangleDraft(vertices[0], vertices[1], vertices[2]);
            draft.Add(TriangleDraft(vertices[1], vertices[3], vertices[2]));
            draft.Add(TriangleDraft(vertices[0], vertices[2], vertices[3]));
            draft.Add(TriangleDraft(vertices[0], vertices[3], vertices[1]));
            draft.name = "Tetrahedron";
            return draft;
        }

        public static Mesh Hexahedron(float width, float length, float height)
        {
            return HexahedronDraft(width, length, height).ToMesh();
        }

        public static Mesh Hexahedron(Vector3 width, Vector3 length, Vector3 height)
        {
            return HexahedronDraft(width, length, height).ToMesh();
        }

        public static MeshDraft HexahedronDraft(float width, float length, float height)
        {
            return HexahedronDraft(Vector3.right*width, Vector3.forward*length, Vector3.up*height);
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

        public static MeshDraft HexahedronDraft(Vector3 width, Vector3 length, Vector3 height, Directions parts)
        {
            var corner0 = -width/2 - length/2 - height/2;
            var corner1 = width/2 + length/2 + height/2;

            var draft = new MeshDraft {name = "Hexahedron"};
            if ((parts & Directions.Left) == Directions.Left)
            {
                draft.Add(QuadDraft(corner0, height, length));
            }
            if ((parts & Directions.Right) == Directions.Right)
            {
                draft.Add(QuadDraft(corner1, -length, -height));
            }
            if ((parts & Directions.Down) == Directions.Down)
            {
                draft.Add(QuadDraft(corner0, length, width));
            }
            if ((parts & Directions.Up) == Directions.Up)
            {
                draft.Add(QuadDraft(corner1, -width, -length));
            }
            if ((parts & Directions.Back) == Directions.Back)
            {
                draft.Add(QuadDraft(corner0, width, height));
            }
            if ((parts & Directions.Forward) == Directions.Forward)
            {
                draft.Add(QuadDraft(corner1, -height, -width));
            }
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
                lowerCap.Add(PTUtils.PointOnSphere(radius, currentAngle, -magicAngle1));
                lowerRing.Add(PTUtils.PointOnSphere(radius, currentAngle, -magicAngle2));
                currentAngle -= segmentAngle;
            }

            currentAngle = -segmentAngle/2;
            var upperCap = new List<Vector3>();
            var upperRing = new List<Vector3>();
            for (var i = 0; i <= 5; i++)
            {
                upperCap.Add(PTUtils.PointOnSphere(radius, currentAngle, magicAngle1));
                upperRing.Add(PTUtils.PointOnSphere(radius, currentAngle, magicAngle2));
                currentAngle -= segmentAngle;
            }

            var draft = TriangleFanDraft(lowerCap);
            draft.Add(FlatBandDraft(lowerCap, lowerRing));
            draft.Add(FlatBandDraft(lowerRing, upperRing));
            draft.Add(FlatBandDraft(upperRing, upperCap));
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
                upperRing.Add(PTUtils.PointOnSphere(radius, currentAngle, magicAngle));
                currentAngle -= segmentAngle;
            }

            currentAngle = segmentAngle/2;
            var lowerRing = new List<Vector3>(5);
            for (var i = 0; i < 5; i++)
            {
                lowerRing.Add(PTUtils.PointOnSphere(radius, currentAngle, -magicAngle));
                currentAngle -= segmentAngle;
            }

            var draft = BaselessPyramidDraft(new Vector3(0, -radius, 0), lowerRing);
            draft.Add(FlatBandDraft(lowerRing, upperRing));
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
                var point = PTUtils.PointOnCircle3(radius, currentAngle);
                lowerRing.Add(point - Vector3.up*heignt/2);
                upperRing.Add(point + Vector3.up*heignt/2);
                currentAngle -= segmentAngle;
            }

            var draft = TriangleFanDraft(lowerRing);
            draft.Add(FlatBandDraft(lowerRing, upperRing));
            upperRing.Reverse();
            draft.Add(TriangleFanDraft(upperRing));
            draft.name = "Prism";
            return draft;
        }

        public static Mesh Cylinder(float radius, int segments, float heignt)
        {
            return CylinderDraft(radius, segments, heignt).ToMesh();
        }

        public static MeshDraft CylinderDraft(float radius, int segments, float heignt)
        {
            var segmentAngle = Mathf.PI*2/segments;

            var currentAngle = 0f;
            var lowerRing = new List<Vector3>(segments);
            var upperRing = new List<Vector3>(segments);
            for (var i = 0; i < segments; i++)
            {
                var point = PTUtils.PointOnCircle3(radius, currentAngle);
                lowerRing.Add(point - Vector3.up*heignt/2);
                upperRing.Add(point + Vector3.up*heignt/2);
                currentAngle -= segmentAngle;
            }

            var draft = TriangleFanDraft(lowerRing);
            draft.Add(BandDraft(lowerRing, upperRing));
            upperRing.Reverse();
            draft.Add(TriangleFanDraft(upperRing));
            draft.name = "Cylinder";
            return draft;
        }

        public static Mesh FlatSphere(float radius, int longitudeSegments, int latitudeSegments)
        {
            return FlatSphereDraft(radius, longitudeSegments, longitudeSegments).ToMesh();
        }

        public static MeshDraft FlatSphereDraft(float radius, int longitudeSegments, int latitudeSegments)
        {
            var longitudeSegmentAngle = Mathf.PI*2/longitudeSegments;
            var latitudeSegmentAngle = Mathf.PI/latitudeSegments;

            var currentLatitude = -Mathf.PI/2;
            var rings = new List<List<Vector3>>(latitudeSegments);
            for (var i = 0; i <= latitudeSegments; i++)
            {
                var currentLongitude = 0f;
                var ring = new List<Vector3>(longitudeSegments);
                for (int j = 0; j < longitudeSegments; j++)
                {
                    ring.Add(PTUtils.PointOnSphere(radius, currentLongitude, currentLatitude));
                    currentLongitude -= longitudeSegmentAngle;
                }
                rings.Add(ring);
                currentLatitude += latitudeSegmentAngle;
            }

            var draft = new MeshDraft {name = "Flat sphere"};
            for (int i = 0; i < rings.Count - 1; i++)
            {
                draft.Add(FlatBandDraft(rings[i], rings[i + 1]));
            }
            return draft;
        }

        public static Mesh Sphere(float radius, int longitudeSegments, int latitudeSegments)
        {
            return SphereDraft(radius, longitudeSegments, longitudeSegments).ToMesh();
        }

        public static MeshDraft SphereDraft(float radius, int longitudeSegments, int latitudeSegments)
        {
            var draft = new MeshDraft {name = "Sphere"};

            var longitudeSegmentAngle = Mathf.PI*2/longitudeSegments;
            var latitudeSegmentAngle = Mathf.PI/latitudeSegments;

            var currentLatitude = -Mathf.PI/2;
            for (var ring = 0; ring <= latitudeSegments; ring++)
            {
                var currentLongitude = 0f;
                for (int i = 0; i < longitudeSegments; i++)
                {
                    var point = PTUtils.PointOnSphere(radius, currentLongitude, currentLatitude);
                    draft.vertices.Add(point);
                    draft.normals.Add(point.normalized);
                    draft.uv.Add(new Vector2((float) i/longitudeSegments, (float) ring/latitudeSegments));
                    currentLongitude -= longitudeSegmentAngle;
                }
                currentLatitude += latitudeSegmentAngle;
            }

            int i0, i1, i2, i3;
            for (int ring = 0; ring < latitudeSegments; ring++)
            {
                for (int i = 0; i < longitudeSegments - 1; i++)
                {
                    i0 = ring*longitudeSegments + i;
                    i1 = (ring + 1)*longitudeSegments + i;
                    i2 = ring*longitudeSegments + i + 1;
                    i3 = (ring + 1)*longitudeSegments + i + 1;
                    draft.triangles.AddRange(new[] {i0, i1, i2});
                    draft.triangles.AddRange(new[] {i2, i1, i3});
                }

                i0 = (ring + 1)*longitudeSegments - 1;
                i1 = (ring + 2)*longitudeSegments - 1;
                i2 = ring*longitudeSegments;
                i3 = (ring + 1)*longitudeSegments;
                draft.triangles.AddRange(new[] {i0, i1, i2});
                draft.triangles.AddRange(new[] {i2, i1, i3});
            }

            return draft;
        }

        #endregion Primitives

        /// <summary>
        /// Moves mesh vertices by <paramref name="vector"/>
        /// </summary>
        public static void Move(this Mesh mesh, Vector3 vector)
        {
            var vertices = mesh.vertices;
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] += vector;
            }
            mesh.vertices = vertices;
        }

        /// <summary>
        /// Rotates mesh vertices by <paramref name="rotation"/>
        /// </summary>
        public static void Rotate(this Mesh mesh, Quaternion rotation)
        {
            var vertices = mesh.vertices;
            var normals = mesh.normals;
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] = rotation*vertices[i];
                normals[i] = rotation*normals[i];
            }
            mesh.vertices = vertices;
            mesh.normals = normals;
        }

        /// <summary>
        /// Scales mesh vertices uniformly by <paramref name="scale"/>
        /// </summary>
        public static void Scale(this Mesh mesh, float scale)
        {
            var vertices = mesh.vertices;
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] *= scale;
            }
            mesh.vertices = vertices;
        }

        /// <summary>
        /// Scales mesh vertices non-uniformly by <paramref name="scale"/>
        /// </summary>
        public static void Scale(this Mesh mesh, Vector3 scale)
        {
            var vertices = mesh.vertices;
            var normals = mesh.normals;
            for (int i = 0; i < vertices.Length; i++)
            {
                var v = vertices[i];
                vertices[i] = new Vector3(v.x*scale.x, v.y*scale.y, v.z*scale.z);
                var n = normals[i];
                normals[i] = new Vector3(n.x*scale.x, n.y*scale.y, n.z*scale.z).normalized;
            }
            mesh.vertices = vertices;
            mesh.normals = normals;
        }

        /// <summary>
        /// Paints mesh vertices with <paramref name="color"/>
        /// </summary>
        public static void Paint(this Mesh mesh, Color color)
        {
            var colors = new Color[mesh.vertexCount];
            for (int i = 0; i < mesh.vertexCount; i++)
            {
                colors[i] = color;
            }
            mesh.colors = colors;
        }

        /// <summary>
        /// Flips mesh faces
        /// </summary>
        public static void FlipFaces(this Mesh mesh)
        {
            mesh.FlipTriangles();
            mesh.FlipNormals();
        }

        /// <summary>
        /// Reverses winding order of mesh triangles
        /// </summary>
        public static void FlipTriangles(this Mesh mesh)
        {
            for (int i = 0; i < mesh.subMeshCount; i++)
            {
                var triangles = mesh.GetTriangles(i);
                for (int j = 0; j < triangles.Length; j += 3)
                {
                    PTUtils.Swap(ref triangles[j], ref triangles[j + 1]);
                }
                mesh.SetTriangles(triangles, i);
            }
        }

        /// <summary>
        /// Reverses direction of mesh normals
        /// </summary>
        public static void FlipNormals(this Mesh mesh)
        {
            var normals = mesh.normals;
            for (int i = 0; i < normals.Length; i++)
            {
                normals[i] = -normals[i];
            }
            mesh.normals = normals;
        }
    }
}