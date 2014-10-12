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