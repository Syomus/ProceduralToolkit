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

        public static Mesh Pyramid(float radius, int segments, float heignt, bool inverted = false)
        {
            return MeshDraft.Pyramid(radius, segments, heignt, inverted).ToMesh();
        }

        public static Mesh Prism(float radius, int segments, float heignt)
        {
            return MeshDraft.Prism(radius, segments, heignt).ToMesh();
        }

        public static Mesh Cylinder(float radius, int segments, float heignt)
        {
            return MeshDraft.Cylinder(radius, segments, heignt).ToMesh();
        }

        public static Mesh FlatSphere(float radius, int longitudeSegments, int latitudeSegments)
        {
            return MeshDraft.FlatSphere(radius, longitudeSegments, longitudeSegments).ToMesh();
        }

        public static Mesh Sphere(float radius, int longitudeSegments, int latitudeSegments)
        {
            return MeshDraft.Sphere(radius, longitudeSegments, longitudeSegments).ToMesh();
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
                Vector3 v = vertices[i];
                vertices[i] = new Vector3(v.x*scale.x, v.y*scale.y, v.z*scale.z);
                Vector3 n = normals[i];
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