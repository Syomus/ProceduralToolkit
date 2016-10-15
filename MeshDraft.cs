﻿using System.Collections.Generic;
using UnityEngine;

namespace ProceduralToolkit
{
    /// <summary>
    /// Helper class for procedural mesh generation
    /// </summary>
    public partial class MeshDraft
    {
        public string name = "";
        public List<Vector3> vertices = new List<Vector3>();
        public List<int> triangles = new List<int>();
        public List<Vector3> normals = new List<Vector3>();
        public List<Vector4> tangents = new List<Vector4>();
        public List<Vector2> uv = new List<Vector2>();
        public List<Vector2> uv2 = new List<Vector2>();
        public List<Vector2> uv3 = new List<Vector2>();
        public List<Vector2> uv4 = new List<Vector2>();
        public List<Color> colors = new List<Color>();

        /// <summary>
        /// Creates an empty MeshDraft
        /// </summary>
        public MeshDraft()
        {
        }

        /// <summary>
        /// Creates new MeshDraft with vertex data from <paramref name="mesh"/>>
        /// </summary>
        public MeshDraft(Mesh mesh)
        {
            name = mesh.name;
            vertices.AddRange(mesh.vertices);
            triangles.AddRange(mesh.triangles);
            normals.AddRange(mesh.normals);
            tangents.AddRange(mesh.tangents);
            uv.AddRange(mesh.uv);
            uv2.AddRange(mesh.uv2);
            uv3.AddRange(mesh.uv3);
            uv4.AddRange(mesh.uv4);
            colors.AddRange(mesh.colors);
        }

        /// <summary>
        /// Adds vertex data from <paramref name="draft"/>
        /// </summary>
        public void Add(MeshDraft draft)
        {
            foreach (var triangle in draft.triangles)
            {
                triangles.Add(triangle + vertices.Count);
            }
            vertices.AddRange(draft.vertices);
            normals.AddRange(draft.normals);
            tangents.AddRange(draft.tangents);
            uv.AddRange(draft.uv);
            uv2.AddRange(draft.uv2);
            uv3.AddRange(draft.uv3);
            uv4.AddRange(draft.uv4);
            colors.AddRange(draft.colors);
        }

        /// <summary>
        /// Clears all vertex data and all triangle indices
        /// </summary>
        public void Clear()
        {
            vertices.Clear();
            triangles.Clear();
            normals.Clear();
            tangents.Clear();
            uv.Clear();
            uv2.Clear();
            uv3.Clear();
            uv4.Clear();
            colors.Clear();
        }

        /// <summary>
        /// Moves draft vertices by <paramref name="vector"/>
        /// </summary>
        public void Move(Vector3 vector)
        {
            for (int i = 0; i < vertices.Count; i++)
            {
                vertices[i] += vector;
            }
        }

        /// <summary>
        /// Rotates draft vertices by <paramref name="rotation"/>
        /// </summary>
        public void Rotate(Quaternion rotation)
        {
            for (int i = 0; i < vertices.Count; i++)
            {
                vertices[i] = rotation*vertices[i];
                normals[i] = rotation*normals[i];
            }
        }

        /// <summary>
        /// Scales draft vertices uniformly by <paramref name="scale"/>
        /// </summary>
        public void Scale(float scale)
        {
            for (int i = 0; i < vertices.Count; i++)
            {
                vertices[i] *= scale;
            }
        }

        /// <summary>
        /// Scales draft vertices non-uniformly by <paramref name="scale"/>
        /// </summary>
        public void Scale(Vector3 scale)
        {
            for (int i = 0; i < vertices.Count; i++)
            {
                vertices[i] = Vector3.Scale(vertices[i], scale);
                normals[i] = Vector3.Scale(normals[i], scale).normalized;
            }
        }

        /// <summary>
        /// Paints draft vertices with <paramref name="color"/>
        /// </summary>
        public void Paint(Color color)
        {
            colors.Clear();
            for (int i = 0; i < vertices.Count; i++)
            {
                colors.Add(color);
            }
        }

        /// <summary>
        /// Flips draft faces
        /// </summary>
        public void FlipFaces()
        {
            FlipTriangles();
            FlipNormals();
        }

        /// <summary>
        /// Reverses winding order of draft triangles
        /// </summary>
        public void FlipTriangles()
        {
            for (int i = 0; i < triangles.Count; i += 3)
            {
                var temp = triangles[i];
                triangles[i] = triangles[i + 1];
                triangles[i + 1] = temp;
            }
        }

        /// <summary>
        /// Reverses direction of draft normals
        /// </summary>
        public void FlipNormals()
        {
            for (int i = 0; i < normals.Count; i++)
            {
                normals[i] = -normals[i];
            }
        }

        /// <summary>
        /// Creates new mesh from information in draft
        /// </summary>
        public Mesh ToMesh()
        {
            var mesh = new Mesh {name = name};
            mesh.SetVertices(vertices);
            mesh.SetTriangles(triangles, 0);
            mesh.SetNormals(normals);
            mesh.SetTangents(tangents);
            mesh.SetUVs(0, uv);
            mesh.SetUVs(1, uv2);
            mesh.SetUVs(2, uv3);
            mesh.SetUVs(3, uv4);
            mesh.SetColors(colors);
            return mesh;
        }

        /// <summary>
        /// Fills <paramref name="mesh"/> with information in draft
        /// </summary>
        public void ToMesh(ref Mesh mesh)
        {
            mesh.Clear(false);
            mesh.name = name;
            mesh.SetVertices(vertices);
            mesh.SetTriangles(triangles, 0);
            mesh.SetNormals(normals);
            mesh.SetTangents(tangents);
            mesh.SetUVs(0, uv);
            mesh.SetUVs(1, uv2);
            mesh.SetUVs(2, uv3);
            mesh.SetUVs(3, uv4);
            mesh.SetColors(colors);
        }
    }
}