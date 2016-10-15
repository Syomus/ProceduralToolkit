using System.Collections.Generic;
using UnityEngine;

namespace ProceduralToolkit
{
    public static partial class MeshE
    {
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

        public static Mesh TriangleFan(List<Vector3> vertices)
        {
            return MeshDraft.TriangleFan(vertices).ToMesh();
        }

        public static Mesh TriangleStrip(List<Vector3> vertices)
        {
            return MeshDraft.TriangleStrip(vertices).ToMesh();
        }

        #endregion Mesh parts

        #region Platonic solids

        public static Mesh Tetrahedron(float radius)
        {
            return MeshDraft.Tetrahedron(radius).ToMesh();
        }

        public static Mesh Hexahedron(float width, float length, float height)
        {
            return MeshDraft.Hexahedron(width, length, height).ToMesh();
        }

        public static Mesh Hexahedron(Vector3 width, Vector3 length, Vector3 height)
        {
            return MeshDraft.Hexahedron(width, length, height).ToMesh();
        }

        public static Mesh Octahedron(float radius)
        {
            return MeshDraft.Octahedron(radius).ToMesh();
        }

        public static Mesh Dodecahedron(float radius)
        {
            return MeshDraft.Dodecahedron(radius).ToMesh();
        }

        public static Mesh Icosahedron(float radius)
        {
            return MeshDraft.Icosahedron(radius).ToMesh();
        }

        #endregion Platonic solids

        public static Mesh Plane(float xSize = 1, float zSize = 1, int xSegments = 1, int zSegments = 1)
        {
            return MeshDraft.Plane(xSize, zSize, xSegments, zSegments).ToMesh();
        }

        public static Mesh Pyramid(float radius, int segments, float height, bool inverted = false)
        {
            return MeshDraft.Pyramid(radius, segments, height, inverted).ToMesh();
        }

        public static Mesh Prism(float radius, int segments, float height)
        {
            return MeshDraft.Prism(radius, segments, height).ToMesh();
        }

        public static Mesh Cylinder(float radius, int segments, float height)
        {
            return MeshDraft.Cylinder(radius, segments, height).ToMesh();
        }

        public static Mesh FlatSphere(float radius, int longitudeSegments, int latitudeSegments)
        {
            return MeshDraft.FlatSphere(radius, longitudeSegments, latitudeSegments).ToMesh();
        }

        public static Mesh Sphere(float radius, int longitudeSegments, int latitudeSegments)
        {
            return MeshDraft.Sphere(radius, longitudeSegments, latitudeSegments).ToMesh();
        }
    }
}