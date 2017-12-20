using System;
using System.Collections.Generic;
using UnityEngine;

namespace ProceduralToolkit
{
    /// <summary>
    /// Helper class for procedural mesh generation
    /// </summary>
    [Serializable]
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
        /// Shortcut for vertices.Count
        /// </summary>
        public int vertexCount
        {
            get { return vertices.Count; }
        }

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
            if (mesh == null) throw new ArgumentNullException("mesh");

            name = mesh.name;
            mesh.GetVertices(vertices);
            mesh.GetTriangles(triangles, 0);
            mesh.GetNormals(normals);
            mesh.GetTangents(tangents);
            mesh.GetUVs(0, uv);
            mesh.GetUVs(1, uv2);
            mesh.GetUVs(2, uv3);
            mesh.GetUVs(3, uv4);
            mesh.GetColors(colors);
        }

        /// <summary>
        /// Adds vertex data from <paramref name="draft"/>
        /// </summary>
        public MeshDraft Add(MeshDraft draft)
        {
            if (draft == null) throw new ArgumentNullException("draft");

            for (var i = 0; i < draft.triangles.Count; i++)
            {
                triangles.Add(draft.triangles[i] + vertices.Count);
            }
            vertices.AddRange(draft.vertices);
            normals.AddRange(draft.normals);
            tangents.AddRange(draft.tangents);
            uv.AddRange(draft.uv);
            uv2.AddRange(draft.uv2);
            uv3.AddRange(draft.uv3);
            uv4.AddRange(draft.uv4);
            colors.AddRange(draft.colors);
            return this;
        }

        #region AddTriangle

        public MeshDraft AddTriangle(Vector3 vertex0, Vector3 vertex1, Vector3 vertex2)
        {
            Vector3 normal = Vector3.Cross(vertex1 - vertex0, vertex2 - vertex0).normalized;
            return AddTriangle(vertex0, vertex1, vertex2, normal, normal, normal);
        }

        public MeshDraft AddTriangle(Vector3 vertex0, Vector3 vertex1, Vector3 vertex2, Vector3 normal)
        {
            return AddTriangle(vertex0, vertex1, vertex2, normal, normal, normal);
        }

        public MeshDraft AddTriangle(Vector3 vertex0, Vector3 vertex1, Vector3 vertex2, Vector3 normal0, Vector3 normal1,
            Vector3 normal2)
        {
            triangles.Add(0 + vertices.Count);
            triangles.Add(1 + vertices.Count);
            triangles.Add(2 + vertices.Count);
            vertices.Add(vertex0);
            vertices.Add(vertex1);
            vertices.Add(vertex2);
            normals.Add(normal0);
            normals.Add(normal1);
            normals.Add(normal2);
            return this;
        }

        public MeshDraft AddTriangle(Vector3 vertex0, Vector3 vertex1, Vector3 vertex2, Vector2 uv0, Vector2 uv1,
            Vector2 uv2)
        {
            uv.Add(uv0);
            uv.Add(uv1);
            uv.Add(uv2);
            return AddTriangle(vertex0, vertex1, vertex2);
        }

        public MeshDraft AddTriangle(Vector3 vertex0, Vector3 vertex1, Vector3 vertex2, Vector3 normal, Vector2 uv0,
            Vector2 uv1, Vector2 uv2)
        {
            uv.Add(uv0);
            uv.Add(uv1);
            uv.Add(uv2);
            return AddTriangle(vertex0, vertex1, vertex2, normal, normal, normal);
        }

        public MeshDraft AddTriangle(Vector3 vertex0, Vector3 vertex1, Vector3 vertex2, Vector3 normal0, Vector3 normal1,
            Vector3 normal2, Vector2 uv0, Vector2 uv1, Vector2 uv2)
        {
            uv.Add(uv0);
            uv.Add(uv1);
            uv.Add(uv2);
            return AddTriangle(vertex0, vertex1, vertex2, normal0, normal1, normal2);
        }

        #endregion AddTriangle

        #region AddQuad

        public MeshDraft AddQuad(Vector3 origin, Vector3 width, Vector3 height)
        {
            Vector3 normal = Vector3.Cross(height, width).normalized;
            return AddQuad(origin, origin + height, origin + height + width, origin + width,
                normal, normal, normal, normal);
        }

        public MeshDraft AddQuad(Vector3 vertex0, Vector3 vertex1, Vector3 vertex2, Vector3 vertex3)
        {
            Vector3 normal = Vector3.Cross(vertex1 - vertex0, vertex2 - vertex0).normalized;
            return AddQuad(vertex0, vertex1, vertex2, vertex3, normal, normal, normal, normal);
        }

        public MeshDraft AddQuad(Vector3 vertex0, Vector3 vertex1, Vector3 vertex2, Vector3 vertex3, Vector3 normal)
        {
            return AddQuad(vertex0, vertex1, vertex2, vertex3, normal, normal, normal, normal);
        }

        public MeshDraft AddQuad(Vector3 vertex0, Vector3 vertex1, Vector3 vertex2, Vector3 vertex3,
            Vector3 normal0, Vector3 normal1, Vector3 normal2, Vector3 normal3)
        {
            triangles.Add(0 + vertices.Count);
            triangles.Add(1 + vertices.Count);
            triangles.Add(2 + vertices.Count);
            triangles.Add(0 + vertices.Count);
            triangles.Add(2 + vertices.Count);
            triangles.Add(3 + vertices.Count);
            vertices.Add(vertex0);
            vertices.Add(vertex1);
            vertices.Add(vertex2);
            vertices.Add(vertex3);
            normals.Add(normal0);
            normals.Add(normal1);
            normals.Add(normal2);
            normals.Add(normal3);
            return this;
        }

        public MeshDraft AddQuad(Vector3 origin, Vector3 width, Vector3 height,
            Vector2 uv0, Vector2 uv1, Vector2 uv2, Vector2 uv3)
        {
            uv.Add(uv0);
            uv.Add(uv1);
            uv.Add(uv2);
            uv.Add(uv3);
            return AddQuad(origin, width, height);
        }

        public MeshDraft AddQuad(Vector3 vertex0, Vector3 vertex1, Vector3 vertex2, Vector3 vertex3,
            Vector2 uv0, Vector2 uv1, Vector2 uv2, Vector2 uv3)
        {
            uv.Add(uv0);
            uv.Add(uv1);
            uv.Add(uv2);
            uv.Add(uv3);
            return AddQuad(vertex0, vertex1, vertex2, vertex3);
        }

        public MeshDraft AddQuad(Vector3 vertex0, Vector3 vertex1, Vector3 vertex2, Vector3 vertex3, Vector3 normal,
            Vector2 uv0, Vector2 uv1, Vector2 uv2, Vector2 uv3)
        {
            uv.Add(uv0);
            uv.Add(uv1);
            uv.Add(uv2);
            uv.Add(uv3);
            return AddQuad(vertex0, vertex1, vertex2, vertex3, normal, normal, normal, normal);
        }

        public MeshDraft AddQuad(Vector3 vertex0, Vector3 vertex1, Vector3 vertex2, Vector3 vertex3,
            Vector3 normal0, Vector3 normal1, Vector3 normal2, Vector3 normal3,
            Vector2 uv0, Vector2 uv1, Vector2 uv2, Vector2 uv3)
        {
            uv.Add(uv0);
            uv.Add(uv1);
            uv.Add(uv2);
            uv.Add(uv3);
            return AddQuad(vertex0, vertex1, vertex2, vertex3, normal0, normal1, normal2, normal3);
        }

        #endregion AddQuad

        #region AddTriangleFan

        /// <remarks>
        /// https://en.wikipedia.org/wiki/Triangle_fan
        /// </remarks>
        public MeshDraft AddTriangleFan(Vector3[] fan)
        {
            Vector3 normal = Vector3.Cross(fan[1] - fan[0], fan[2] - fan[0]).normalized;
            return AddTriangleFan(fan, normal);
        }

        /// <remarks>
        /// https://en.wikipedia.org/wiki/Triangle_fan
        /// </remarks>
        public MeshDraft AddTriangleFan(Vector3[] fan, Vector3 normal)
        {
            for (int i = 1; i < fan.Length - 1; i++)
            {
                triangles.Add(0 + vertices.Count);
                triangles.Add(i + vertices.Count);
                triangles.Add(i + 1 + vertices.Count);
            }
            vertices.AddRange(fan);
            for (int i = 0; i < fan.Length; i++)
            {
                normals.Add(normal);
            }
            return this;
        }

        /// <remarks>
        /// https://en.wikipedia.org/wiki/Triangle_fan
        /// </remarks>
        public MeshDraft AddTriangleFan(Vector3[] fan, Vector3[] normals)
        {
            for (int i = 1; i < fan.Length - 1; i++)
            {
                triangles.Add(0 + vertices.Count);
                triangles.Add(i + vertices.Count);
                triangles.Add(i + 1 + vertices.Count);
            }
            vertices.AddRange(fan);
            this.normals.AddRange(normals);
            return this;
        }

        /// <remarks>
        /// https://en.wikipedia.org/wiki/Triangle_fan
        /// </remarks>
        public MeshDraft AddTriangleFan(Vector3[] fan, Vector2[] uv)
        {
            this.uv.AddRange(uv);
            return AddTriangleFan(fan);
        }

        /// <remarks>
        /// https://en.wikipedia.org/wiki/Triangle_fan
        /// </remarks>
        public MeshDraft AddTriangleFan(Vector3[] fan, Vector3 normal, Vector2[] uv)
        {
            this.uv.AddRange(uv);
            return AddTriangleFan(fan, normal);
        }

        /// <remarks>
        /// https://en.wikipedia.org/wiki/Triangle_fan
        /// </remarks>
        public MeshDraft AddTriangleFan(Vector3[] fan, Vector3[] normals, Vector2[] uv)
        {
            this.uv.AddRange(uv);
            return AddTriangleFan(fan, normals);
        }

        /// <remarks>
        /// https://en.wikipedia.org/wiki/Triangle_fan
        /// </remarks>
        public MeshDraft AddTriangleFan(List<Vector3> fan)
        {
            Vector3 normal = Vector3.Cross(fan[1] - fan[0], fan[2] - fan[0]).normalized;
            return AddTriangleFan(fan, normal);
        }

        /// <remarks>
        /// https://en.wikipedia.org/wiki/Triangle_fan
        /// </remarks>
        public MeshDraft AddTriangleFan(List<Vector3> fan, Vector3 normal)
        {
            for (int i = 1; i < fan.Count - 1; i++)
            {
                triangles.Add(0 + vertices.Count);
                triangles.Add(i + vertices.Count);
                triangles.Add(i + 1 + vertices.Count);
            }
            vertices.AddRange(fan);
            for (int i = 0; i < fan.Count; i++)
            {
                normals.Add(normal);
            }
            return this;
        }

        /// <remarks>
        /// https://en.wikipedia.org/wiki/Triangle_fan
        /// </remarks>
        public MeshDraft AddTriangleFan(List<Vector3> fan, List<Vector3> normals)
        {
            for (int i = 1; i < fan.Count - 1; i++)
            {
                triangles.Add(0 + vertices.Count);
                triangles.Add(i + vertices.Count);
                triangles.Add(i + 1 + vertices.Count);
            }
            vertices.AddRange(fan);
            this.normals.AddRange(normals);
            return this;
        }

        /// <remarks>
        /// https://en.wikipedia.org/wiki/Triangle_fan
        /// </remarks>
        public MeshDraft AddTriangleFan(List<Vector3> fan, List<Vector2> uv)
        {
            this.uv.AddRange(uv);
            return AddTriangleFan(fan);
        }

        /// <remarks>
        /// https://en.wikipedia.org/wiki/Triangle_fan
        /// </remarks>
        public MeshDraft AddTriangleFan(List<Vector3> fan, Vector3 normal, List<Vector2> uv)
        {
            this.uv.AddRange(uv);
            return AddTriangleFan(fan, normal);
        }

        /// <remarks>
        /// https://en.wikipedia.org/wiki/Triangle_fan
        /// </remarks>
        public MeshDraft AddTriangleFan(List<Vector3> fan, List<Vector3> normals, List<Vector2> uv)
        {
            this.uv.AddRange(uv);
            return AddTriangleFan(fan, normals);
        }

        #endregion AddTriangleFan

        #region AddTriangleStrip

        /// <remarks>
        /// https://en.wikipedia.org/wiki/Triangle_strip
        /// </remarks>
        public MeshDraft AddTriangleStrip(Vector3[] strip)
        {
            Vector3 normal = Vector3.Cross(strip[1] - strip[0], strip[2] - strip[0]).normalized;
            return AddTriangleStrip(strip, normal);
        }

        /// <remarks>
        /// https://en.wikipedia.org/wiki/Triangle_strip
        /// </remarks>
        public MeshDraft AddTriangleStrip(Vector3[] strip, Vector3 normal)
        {
            for (int i = 0, j = 1, k = 2;
                i < strip.Length - 2;
                i++, j += i%2*2, k += (i + 1)%2*2)
            {
                triangles.Add(i + vertices.Count);
                triangles.Add(j + vertices.Count);
                triangles.Add(k + vertices.Count);
            }
            vertices.AddRange(strip);
            for (int i = 0; i < strip.Length; i++)
            {
                normals.Add(normal);
            }
            return this;
        }

        /// <remarks>
        /// https://en.wikipedia.org/wiki/Triangle_strip
        /// </remarks>
        public MeshDraft AddTriangleStrip(Vector3[] strip, Vector3[] normals)
        {
            for (int i = 0, j = 1, k = 2;
                i < strip.Length - 2;
                i++, j += i%2*2, k += (i + 1)%2*2)
            {
                triangles.Add(i + vertices.Count);
                triangles.Add(j + vertices.Count);
                triangles.Add(k + vertices.Count);
            }
            vertices.AddRange(strip);
            this.normals.AddRange(normals);
            return this;
        }

        /// <remarks>
        /// https://en.wikipedia.org/wiki/Triangle_strip
        /// </remarks>
        public MeshDraft AddTriangleStrip(Vector3[] strip, Vector2[] uv)
        {
            this.uv.AddRange(uv);
            return AddTriangleStrip(strip);
        }

        /// <remarks>
        /// https://en.wikipedia.org/wiki/Triangle_strip
        /// </remarks>
        public MeshDraft AddTriangleStrip(Vector3[] strip, Vector3 normal, Vector2[] uv)
        {
            this.uv.AddRange(uv);
            return AddTriangleStrip(strip, normal);
        }

        /// <remarks>
        /// https://en.wikipedia.org/wiki/Triangle_strip
        /// </remarks>
        public MeshDraft AddTriangleStrip(Vector3[] strip, Vector3[] normals, Vector2[] uv)
        {
            this.uv.AddRange(uv);
            return AddTriangleStrip(strip, normals);
        }

        /// <remarks>
        /// https://en.wikipedia.org/wiki/Triangle_strip
        /// </remarks>
        public MeshDraft AddTriangleStrip(List<Vector3> strip)
        {
            Vector3 normal = Vector3.Cross(strip[1] - strip[0], strip[2] - strip[0]).normalized;
            return AddTriangleStrip(strip, normal);
        }

        /// <remarks>
        /// https://en.wikipedia.org/wiki/Triangle_strip
        /// </remarks>
        public MeshDraft AddTriangleStrip(List<Vector3> strip, Vector3 normal)
        {
            for (int i = 0, j = 1, k = 2;
                i < strip.Count - 2;
                i++, j += i%2*2, k += (i + 1)%2*2)
            {
                triangles.Add(i + vertices.Count);
                triangles.Add(j + vertices.Count);
                triangles.Add(k + vertices.Count);
            }
            vertices.AddRange(strip);
            for (int i = 0; i < strip.Count; i++)
            {
                normals.Add(normal);
            }
            return this;
        }

        /// <remarks>
        /// https://en.wikipedia.org/wiki/Triangle_strip
        /// </remarks>
        public MeshDraft AddTriangleStrip(List<Vector3> strip, List<Vector3> normals)
        {
            for (int i = 0, j = 1, k = 2;
                i < strip.Count - 2;
                i++, j += i%2*2, k += (i + 1)%2*2)
            {
                triangles.Add(i + vertices.Count);
                triangles.Add(j + vertices.Count);
                triangles.Add(k + vertices.Count);
            }
            vertices.AddRange(strip);
            this.normals.AddRange(normals);
            return this;
        }

        /// <remarks>
        /// https://en.wikipedia.org/wiki/Triangle_strip
        /// </remarks>
        public MeshDraft AddTriangleStrip(List<Vector3> strip, List<Vector2> uv)
        {
            this.uv.AddRange(uv);
            return AddTriangleStrip(strip);
        }

        /// <remarks>
        /// https://en.wikipedia.org/wiki/Triangle_strip
        /// </remarks>
        public MeshDraft AddTriangleStrip(List<Vector3> strip, Vector3 normal, List<Vector2> uv)
        {
            this.uv.AddRange(uv);
            return AddTriangleStrip(strip, normal);
        }

        /// <remarks>
        /// https://en.wikipedia.org/wiki/Triangle_strip
        /// </remarks>
        public MeshDraft AddTriangleStrip(List<Vector3> strip, List<Vector3> normals, List<Vector2> uv)
        {
            this.uv.AddRange(uv);
            return AddTriangleStrip(strip, normals);
        }

        #endregion AddTriangleStrip

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
        public MeshDraft Move(Vector3 vector)
        {
            for (int i = 0; i < vertices.Count; i++)
            {
                vertices[i] += vector;
            }
            return this;
        }

        /// <summary>
        /// Rotates draft vertices by <paramref name="rotation"/>
        /// </summary>
        public MeshDraft Rotate(Quaternion rotation)
        {
            for (int i = 0; i < vertices.Count; i++)
            {
                vertices[i] = rotation*vertices[i];
                normals[i] = rotation*normals[i];
            }
            return this;
        }

        /// <summary>
        /// Scales draft vertices uniformly by <paramref name="scale"/>
        /// </summary>
        public MeshDraft Scale(float scale)
        {
            for (int i = 0; i < vertices.Count; i++)
            {
                vertices[i] *= scale;
            }
            return this;
        }

        /// <summary>
        /// Scales draft vertices non-uniformly by <paramref name="scale"/>
        /// </summary>
        public MeshDraft Scale(Vector3 scale)
        {
            for (int i = 0; i < vertices.Count; i++)
            {
                vertices[i] = Vector3.Scale(vertices[i], scale);
                normals[i] = Vector3.Scale(normals[i], scale).normalized;
            }
            return this;
        }

        /// <summary>
        /// Paints draft vertices with <paramref name="color"/>
        /// </summary>
        public MeshDraft Paint(Color color)
        {
            colors.Clear();
            for (int i = 0; i < vertices.Count; i++)
            {
                colors.Add(color);
            }
            return this;
        }

        /// <summary>
        /// Flips draft faces
        /// </summary>
        public MeshDraft FlipFaces()
        {
            FlipTriangles();
            FlipNormals();
            return this;
        }

        /// <summary>
        /// Reverses winding order of draft triangles
        /// </summary>
        public MeshDraft FlipTriangles()
        {
            for (int i = 0; i < triangles.Count; i += 3)
            {
                var temp = triangles[i];
                triangles[i] = triangles[i + 1];
                triangles[i + 1] = temp;
            }
            return this;
        }

        /// <summary>
        /// Reverses direction of draft normals
        /// </summary>
        public MeshDraft FlipNormals()
        {
            for (int i = 0; i < normals.Count; i++)
            {
                normals[i] = -normals[i];
            }
            return this;
        }

        /// <summary>
        /// Flips UV map horizontally in selected <paramref name="channel"/>
        /// </summary>
        public MeshDraft FlipUVHorizontally(int channel = 0)
        {
            List<Vector2> list;
            switch (channel)
            {
                case 0:
                    list = uv;
                    break;
                case 1:
                    list = uv2;
                    break;
                case 2:
                    list = uv3;
                    break;
                case 3:
                    list = uv4;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("channel");
            }
            for (var i = 0; i < list.Count; i++)
            {
                list[i] = new Vector2(1 - list[i].x, list[i].y);
            }
            return this;
        }

        /// <summary>
        /// Flips UV map vertically in selected <paramref name="channel"/>
        /// </summary>
        public MeshDraft FlipUVVertically(int channel = 0)
        {
            List<Vector2> list;
            switch (channel)
            {
                case 0:
                    list = uv;
                    break;
                case 1:
                    list = uv2;
                    break;
                case 2:
                    list = uv3;
                    break;
                case 3:
                    list = uv4;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("channel");
            }
            for (var i = 0; i < list.Count; i++)
            {
                list[i] = new Vector2(list[i].x, 1 - list[i].y);
            }
            return this;
        }

        /// <summary>
        /// Projects vertices on a sphere with given <paramref name="radius"/> and <paramref name="center"/>, recalculates normals
        /// </summary>
        public MeshDraft Spherify(float radius, Vector3 center = default(Vector3))
        {
            for (var i = 0; i < vertices.Count; i++)
            {
                normals[i] = (vertices[i] - center).normalized;
                vertices[i] = normals[i]*radius;
            }
            return this;
        }

        /// <summary>
        /// Creates new mesh from information in draft
        /// </summary>
        public Mesh ToMesh()
        {
            var mesh = new Mesh();
            FillMesh(ref mesh);
            return mesh;
        }

        /// <summary>
        /// Fills <paramref name="mesh"/> with information in draft
        /// </summary>
        public void ToMesh(ref Mesh mesh)
        {
            if (mesh == null)
            {
                throw new ArgumentNullException("mesh");
            }
            mesh.Clear(false);
            FillMesh(ref mesh);
        }

        private void FillMesh(ref Mesh mesh)
        {
            if (vertexCount > 65000)
            {
                Debug.LogError("A mesh may not have more than 65000 vertices. Vertex count: " + vertexCount);
            }
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

        public override string ToString()
        {
            return name + " (ProceduralToolkit.MeshDraft)";
        }
    }
}
