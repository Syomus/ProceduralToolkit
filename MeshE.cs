using System;
using System.Collections.Generic;
using UnityEngine;

namespace ProceduralToolkit
{
    /// <summary>
    /// Mesh extensions and constructors for primitives
    /// </summary>
    public static partial class MeshE
    {
        /// <summary>
        /// Moves mesh vertices by <paramref name="vector"/>
        /// </summary>
        public static void Move(this Mesh mesh, Vector3 vector)
        {
            if (mesh == null)
            {
                throw new ArgumentNullException("mesh");
            }
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
            if (mesh == null)
            {
                throw new ArgumentNullException("mesh");
            }
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
            if (mesh == null)
            {
                throw new ArgumentNullException("mesh");
            }
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
            if (mesh == null)
            {
                throw new ArgumentNullException("mesh");
            }
            var vertices = mesh.vertices;
            var normals = mesh.normals;
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] = Vector3.Scale(vertices[i], scale);
                normals[i] = Vector3.Scale(normals[i], scale).normalized;
            }
            mesh.vertices = vertices;
            mesh.normals = normals;
        }

        /// <summary>
        /// Paints mesh vertices with <paramref name="color"/>
        /// </summary>
        public static void Paint(this Mesh mesh, Color color)
        {
            if (mesh == null)
            {
                throw new ArgumentNullException("mesh");
            }
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
            if (mesh == null)
            {
                throw new ArgumentNullException("mesh");
            }
            mesh.FlipTriangles();
            mesh.FlipNormals();
        }

        /// <summary>
        /// Reverses winding order of mesh triangles
        /// </summary>
        public static void FlipTriangles(this Mesh mesh)
        {
            if (mesh == null)
            {
                throw new ArgumentNullException("mesh");
            }
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
            if (mesh == null)
            {
                throw new ArgumentNullException("mesh");
            }
            var normals = mesh.normals;
            for (int i = 0; i < normals.Length; i++)
            {
                normals[i] = -normals[i];
            }
            mesh.normals = normals;
        }

        /// <summary>
        /// Flips UV map horizontally in selected <paramref name="channel"/>
        /// </summary>
        public static void FlipUVHorizontally(this Mesh mesh, int channel = 0)
        {
            if (mesh == null)
            {
                throw new ArgumentNullException("mesh");
            }
            var list = new List<Vector2>();
            mesh.GetUVs(channel, list);
            for (var i = 0; i < list.Count; i++)
            {
                list[i] = new Vector2(1 - list[i].x, list[i].y);
            }
            mesh.SetUVs(channel, list);
        }

        /// <summary>
        /// Flips UV map vertically in selected <paramref name="channel"/>
        /// </summary>
        public static void FlipUVVertically(this Mesh mesh, int channel = 0)
        {
            if (mesh == null)
            {
                throw new ArgumentNullException("mesh");
            }
            var list = new List<Vector2>();
            mesh.GetUVs(channel, list);
            for (var i = 0; i < list.Count; i++)
            {
                list[i] = new Vector2(list[i].x, 1 - list[i].y);
            }
            mesh.SetUVs(channel, list);
        }

        /// <summary>
        /// Projects vertices on a sphere with given <paramref name="radius"/> and <paramref name="center"/>, recalculates normals
        /// </summary>
        public static void Spherify(this Mesh mesh, float radius, Vector3 center = default(Vector3))
        {
            if (mesh == null)
            {
                throw new ArgumentNullException("mesh");
            }
            var vertices = mesh.vertices;
            var normals = mesh.normals;
            for (var i = 0; i < vertices.Length; i++)
            {
                normals[i] = (vertices[i] - center).normalized;
                vertices[i] = normals[i]*radius;
            }
            mesh.vertices = vertices;
            mesh.normals = normals;
        }
    }
}