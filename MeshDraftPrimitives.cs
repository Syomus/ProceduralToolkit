﻿using System.Collections.Generic;
using UnityEngine;

namespace ProceduralToolkit
{
    public partial class MeshDraft
    {
        #region Mesh parts

        public static MeshDraft Triangle(Vector3 vertex0, Vector3 vertex1, Vector3 vertex2)
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

        public static MeshDraft Quad(Vector3 origin, Vector3 width, Vector3 length)
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

        public static MeshDraft Quad(Vector3 vertex0, Vector3 vertex1, Vector3 vertex2, Vector3 vertex3)
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

        /// <remarks>
        /// https://en.wikipedia.org/wiki/Triangle_fan
        /// </remarks>
        public static MeshDraft TriangleFan(List<Vector3> vertices)
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

        /// <remarks>
        /// https://en.wikipedia.org/wiki/Triangle_strip
        /// </remarks>
        public static MeshDraft TriangleStrip(List<Vector3> vertices)
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

        public static MeshDraft BaselessPyramid(float radius, int segments, float heignt, bool inverted = false)
        {
            return BaselessPyramid(Vector3.zero, Vector3.up*heignt*(inverted ? -1 : 1), radius, segments, inverted);
        }

        public static MeshDraft BaselessPyramid(Vector3 baseCenter, Vector3 apex, float radius, int segments,
            bool inverted = false)
        {
            float segmentAngle = Mathf.PI*2/segments*(inverted ? -1 : 1);
            float currentAngle = 0f;

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
                draft.Add(Triangle(vertices[0], vertices[i], vertices[i + 1]));
            }
            draft.Add(Triangle(vertices[0], vertices[vertices.Length - 1], vertices[1]));
            return draft;
        }

        public static MeshDraft BaselessPyramid(Vector3 apex, List<Vector3> ring)
        {
            var draft = new MeshDraft {name = "BaselessPyramid"};
            for (var i = 0; i < ring.Count - 1; i++)
            {
                draft.Add(Triangle(apex, ring[i], ring[i + 1]));
            }
            draft.Add(Triangle(apex, ring[ring.Count - 1], ring[0]));
            return draft;
        }

        public static MeshDraft Band(List<Vector3> lowerRing, List<Vector3> upperRing)
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

        public static MeshDraft FlatBand(List<Vector3> lowerRing, List<Vector3> upperRing)
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
                draft.Add(Triangle(v0, v1, v2));
                draft.Add(Triangle(v2, v1, v3));
            }

            v0 = lowerRing[lowerRing.Count - 1];
            v1 = upperRing[upperRing.Count - 1];
            v2 = lowerRing[0];
            v3 = upperRing[0];
            draft.Add(Triangle(v0, v1, v2));
            draft.Add(Triangle(v2, v1, v3));

            return draft;
        }

        #endregion Mesh parts

        #region Platonic solids

        public static MeshDraft Tetrahedron(float radius)
        {
            float tetrahedralAngle = Mathf.PI*-19.471220333f/180;
            float segmentAngle = Mathf.PI*2/3;
            float currentAngle = 0f;

            var vertices = new List<Vector3>(4) {new Vector3(0, radius, 0)};
            for (var i = 1; i < 4; i++)
            {
                vertices.Add(PTUtils.PointOnSphere(radius, currentAngle, tetrahedralAngle));
                currentAngle += segmentAngle;
            }
            var draft = Triangle(vertices[0], vertices[1], vertices[2]);
            draft.Add(Triangle(vertices[1], vertices[3], vertices[2]));
            draft.Add(Triangle(vertices[0], vertices[2], vertices[3]));
            draft.Add(Triangle(vertices[0], vertices[3], vertices[1]));
            draft.name = "Tetrahedron";
            return draft;
        }

        /// <summary>
        /// Constructs hexahedron aka cube
        /// </summary>
        public static MeshDraft Hexahedron(float width, float length, float height)
        {
            return Hexahedron(Vector3.right*width, Vector3.forward*length, Vector3.up*height);
        }

        /// <summary>
        /// Constructs hexahedron aka cube
        /// </summary>
        public static MeshDraft Hexahedron(Vector3 width, Vector3 length, Vector3 height)
        {
            Vector3 corner0 = -width/2 - length/2 - height/2;
            Vector3 corner1 = width/2 + length/2 + height/2;

            var draft = Quad(corner0, length, width);
            draft.Add(Quad(corner0, width, height));
            draft.Add(Quad(corner0, height, length));
            draft.Add(Quad(corner1, -width, -length));
            draft.Add(Quad(corner1, -height, -width));
            draft.Add(Quad(corner1, -length, -height));
            draft.name = "Hexahedron";
            return draft;
        }

        /// <summary>
        /// Constructs partial hexahedron aka cube with specified faces
        /// </summary>
        public static MeshDraft Hexahedron(Vector3 width, Vector3 length, Vector3 height, Directions parts)
        {
            Vector3 corner0 = -width/2 - length/2 - height/2;
            Vector3 corner1 = width/2 + length/2 + height/2;

            var draft = new MeshDraft {name = "Hexahedron"};
            if ((parts & Directions.Left) == Directions.Left)
            {
                draft.Add(Quad(corner0, height, length));
            }
            if ((parts & Directions.Right) == Directions.Right)
            {
                draft.Add(Quad(corner1, -length, -height));
            }
            if ((parts & Directions.Down) == Directions.Down)
            {
                draft.Add(Quad(corner0, length, width));
            }
            if ((parts & Directions.Up) == Directions.Up)
            {
                draft.Add(Quad(corner1, -width, -length));
            }
            if ((parts & Directions.Back) == Directions.Back)
            {
                draft.Add(Quad(corner0, width, height));
            }
            if ((parts & Directions.Forward) == Directions.Forward)
            {
                draft.Add(Quad(corner1, -height, -width));
            }
            return draft;
        }

        public static MeshDraft Octahedron(float radius)
        {
            var draft = BiPyramid(radius, 4, radius);
            draft.name = "Octahedron";
            return draft;
        }

        public static MeshDraft Dodecahedron(float radius)
        {
            float magicAngle1 = Mathf.PI*52.62263590f/180;
            float magicAngle2 = Mathf.PI*10.81231754f/180;
            float segmentAngle = Mathf.PI*2/5;
            float currentAngle = 0f;
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

            var draft = TriangleFan(lowerCap);
            draft.Add(FlatBand(lowerCap, lowerRing));
            draft.Add(FlatBand(lowerRing, upperRing));
            draft.Add(FlatBand(upperRing, upperCap));
            upperCap.Reverse();
            draft.Add(TriangleFan(upperCap));
            draft.name = "Dodecahedron";
            return draft;
        }

        public static MeshDraft Icosahedron(float radius)
        {
            float magicAngle = Mathf.PI*26.56505f/180;
            float segmentAngle = Mathf.PI*72/180;

            float currentAngle = 0f;
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

            var draft = BaselessPyramid(new Vector3(0, -radius, 0), lowerRing);
            draft.Add(FlatBand(lowerRing, upperRing));
            upperRing.Reverse();
            draft.Add(BaselessPyramid(new Vector3(0, radius, 0), upperRing));
            draft.name = "Icosahedron";
            return draft;
        }

        #endregion Platonic solids

        public static MeshDraft Plane(float xSize = 1, float zSize = 1, int xSegments = 1, int zSegments = 1)
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

        public static MeshDraft Pyramid(float radius, int segments, float heignt, bool inverted = false)
        {
            var draft = BaselessPyramid(radius, segments, heignt, inverted);
            var vertices = new List<Vector3>(segments);
            for (int i = draft.vertices.Count - 2; i >= 0; i -= 3)
            {
                vertices.Add(draft.vertices[i]);
            }
            draft.Add(TriangleFan(vertices));
            draft.name = "Pyramid";
            return draft;
        }

        public static MeshDraft BiPyramid(float radius, int segments, float heignt)
        {
            var draft = BaselessPyramid(radius, segments, heignt);
            draft.Add(BaselessPyramid(radius, segments, heignt, true));
            return draft;
        }

        public static MeshDraft Prism(float radius, int segments, float heignt)
        {
            float segmentAngle = Mathf.PI*2/segments;

            float currentAngle = 0f;
            var lowerRing = new List<Vector3>(segments);
            var upperRing = new List<Vector3>(segments);
            for (var i = 0; i < segments; i++)
            {
                var point = PTUtils.PointOnCircle3(radius, currentAngle);
                lowerRing.Add(point - Vector3.up*heignt/2);
                upperRing.Add(point + Vector3.up*heignt/2);
                currentAngle -= segmentAngle;
            }

            var draft = TriangleFan(lowerRing);
            draft.Add(FlatBand(lowerRing, upperRing));
            upperRing.Reverse();
            draft.Add(TriangleFan(upperRing));
            draft.name = "Prism";
            return draft;
        }

        public static MeshDraft Cylinder(float radius, int segments, float heignt)
        {
            float segmentAngle = Mathf.PI*2/segments;

            float currentAngle = 0f;
            var lowerRing = new List<Vector3>(segments);
            var upperRing = new List<Vector3>(segments);
            for (var i = 0; i < segments; i++)
            {
                var point = PTUtils.PointOnCircle3(radius, currentAngle);
                lowerRing.Add(point - Vector3.up*heignt/2);
                upperRing.Add(point + Vector3.up*heignt/2);
                currentAngle -= segmentAngle;
            }

            var draft = TriangleFan(lowerRing);
            draft.Add(Band(lowerRing, upperRing));
            upperRing.Reverse();
            draft.Add(TriangleFan(upperRing));
            draft.name = "Cylinder";
            return draft;
        }

        public static MeshDraft FlatSphere(float radius, int longitudeSegments, int latitudeSegments)
        {
            float longitudeSegmentAngle = Mathf.PI*2/longitudeSegments;
            float latitudeSegmentAngle = Mathf.PI/latitudeSegments;

            float currentLatitude = -Mathf.PI/2;
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
                draft.Add(FlatBand(rings[i], rings[i + 1]));
            }
            return draft;
        }

        public static MeshDraft Sphere(float radius, int longitudeSegments, int latitudeSegments)
        {
            var draft = new MeshDraft {name = "Sphere"};

            float longitudeSegmentAngle = Mathf.PI*2/longitudeSegments;
            float latitudeSegmentAngle = Mathf.PI/latitudeSegments;

            float currentLatitude = -Mathf.PI/2;
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

        public static MeshDraft FlatTeardrop(float radius, int longitudeSegments, int latitudeSegments, float height)
        {
            float longitudeSegmentAngle = Mathf.PI * 2 / longitudeSegments;
            float latitudeSegmentAngle = Mathf.PI / latitudeSegments;

            float currentLatitude = 0f;
            var rings = new List<List<Vector3>>(latitudeSegments);
            for (var i = 0; i <= latitudeSegments; i++)
            {
                var currentLongitude = Mathf.PI * 2;
                var ring = new List<Vector3>(longitudeSegments);
                for (int j = 0; j < longitudeSegments; j++)
                {
                    ring.Add(PTUtils.PointOnTeardrop(radius, currentLongitude, currentLatitude, height));
                    currentLongitude -= longitudeSegmentAngle;
                }
                rings.Add(ring);
                currentLatitude += latitudeSegmentAngle;
            }

            var draft = new MeshDraft {name = "Flat teardrop"};
            for (int i = 0; i < rings.Count - 1; i++)
            {
                draft.Add(FlatBand(rings[i], rings[i + 1]));
            }
            return draft;
        }

        public static MeshDraft Teardrop(float radius, int longitudeSegments, int latitudeSegments, float height)
        {
            var draft = new MeshDraft { name = "Teardrop" };

            float longitudeSegmentAngle = Mathf.PI * 2 / longitudeSegments;
            float latitudeSegmentAngle = Mathf.PI / latitudeSegments;

            float currentLatitude = 0f;
            for (var ring = 0; ring <= latitudeSegments; ring++)
            {
                var currentLongitude = Mathf.PI * 2;
                for (int i = 0; i < longitudeSegments; i++)
                {
                    var point = PTUtils.PointOnTeardrop(radius, currentLongitude, currentLatitude, height);
                    draft.vertices.Add(point);
                    draft.normals.Add(point.normalized);
                    draft.uv.Add(new Vector2((float)i / longitudeSegments, (float)ring / latitudeSegments));
                    currentLongitude -= longitudeSegmentAngle;
                }
                currentLatitude += latitudeSegmentAngle;
            }

            int i0, i1, i2, i3;
            for (int ring = 0; ring < latitudeSegments; ring++)
            {
                for (int i = 0; i < longitudeSegments - 1; i++)
                {
                    i0 = ring * longitudeSegments + i;
                    i1 = (ring + 1) * longitudeSegments + i;
                    i2 = ring * longitudeSegments + i + 1;
                    i3 = (ring + 1) * longitudeSegments + i + 1;
                    draft.triangles.AddRange(new[] { i0, i1, i2 });
                    draft.triangles.AddRange(new[] { i2, i1, i3 });
                }

                i0 = (ring + 1) * longitudeSegments - 1;
                i1 = (ring + 2) * longitudeSegments - 1;
                i2 = ring * longitudeSegments;
                i3 = (ring + 1) * longitudeSegments;
                draft.triangles.AddRange(new[] { i0, i1, i2 });
                draft.triangles.AddRange(new[] { i2, i1, i3 });
            }

            return draft;
        }
    }
}