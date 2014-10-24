using System.Collections.Generic;
using UnityEngine;

namespace ProceduralToolkit
{
    public class MeshDraft
    {
        public string name = "";
        public List<Vector3> vertices = new List<Vector3>();
        public List<int> triangles = new List<int>();
        public List<Vector3> normals = new List<Vector3>();
        public List<Vector2> uv = new List<Vector2>();
        public List<Color> colors = new List<Color>();

        public MeshDraft()
        {
        }

        public MeshDraft(Mesh mesh)
        {
            name = mesh.name;
            vertices.AddRange(mesh.vertices);
            triangles.AddRange(mesh.triangles);
            normals.AddRange(mesh.normals);
            uv.AddRange(mesh.uv);
            colors.AddRange(mesh.colors);
        }

        public void Add(MeshDraft draft)
        {
            foreach (var triangle in draft.triangles)
            {
                triangles.Add(triangle + vertices.Count);
            }
            vertices.AddRange(draft.vertices);
            normals.AddRange(draft.normals);
            uv.AddRange(draft.uv);
            colors.AddRange(draft.colors);
        }

        public void Move(Vector3 vector)
        {
            for (int i = 0; i < vertices.Count; i++)
            {
                vertices[i] += vector;
            }
        }

        public void Scale(float scale)
        {
            for (int i = 0; i < vertices.Count; i++)
            {
                vertices[i] *= scale;
            }
        }

        public void Scale(Vector3 scale)
        {
            for (int i = 0; i < vertices.Count; i++)
            {
                var v = vertices[i];
                vertices[i] = new Vector3(v.x*scale.x, v.y*scale.y, v.z*scale.z);
            }
        }

        public void Rotate(Quaternion rotation)
        {
            for (int i = 0; i < vertices.Count; i++)
            {
                vertices[i] = rotation*vertices[i];
            }
        }

        public void Paint(Color color)
        {
            colors.Clear();
            for (int i = 0; i < vertices.Count; i++)
            {
                colors.Add(color);
            }
        }

        public Mesh ToMesh()
        {
            return new Mesh
            {
                name = name,
                vertices = vertices.ToArray(),
                triangles = triangles.ToArray(),
                normals = normals.ToArray(),
                uv = uv.ToArray(),
                colors = colors.ToArray()
            };
        }
    }
}