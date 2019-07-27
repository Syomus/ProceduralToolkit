using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

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
        /// Creates a new MeshDraft with vertex data from the <paramref name="mesh"/>>
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
        /// Adds vertex data from the <paramref name="draft"/>
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

        /// <summary>
        /// Adds a triangle to the draft
        /// </summary>
        public MeshDraft AddTriangle(Vector3 vertex0, Vector3 vertex1, Vector3 vertex2, bool calculateNormal)
        {
            if (calculateNormal)
            {
                Vector3 normal = Vector3.Cross(vertex1 - vertex0, vertex2 - vertex0).normalized;
                return AddTriangle(vertex0, vertex1, vertex2, normal, normal, normal);
            }
            return _AddTriangle(vertex0, vertex1, vertex2);
        }

        /// <summary>
        /// Adds a triangle to the draft
        /// </summary>
        public MeshDraft AddTriangle(Vector3 vertex0, Vector3 vertex1, Vector3 vertex2, Vector3 normal)
        {
            return AddTriangle(vertex0, vertex1, vertex2, normal, normal, normal);
        }

        /// <summary>
        /// Adds a triangle to the draft
        /// </summary>
        public MeshDraft AddTriangle(Vector3 vertex0, Vector3 vertex1, Vector3 vertex2, Vector3 normal0, Vector3 normal1, Vector3 normal2)
        {
            normals.Add(normal0);
            normals.Add(normal1);
            normals.Add(normal2);
            return _AddTriangle(vertex0, vertex1, vertex2);
        }

        /// <summary>
        /// Adds a triangle to the draft
        /// </summary>
        public MeshDraft AddTriangle(Vector3 vertex0, Vector3 vertex1, Vector3 vertex2, bool calculateNormal,
            Vector2 uv0, Vector2 uv1, Vector2 uv2)
        {
            uv.Add(uv0);
            uv.Add(uv1);
            uv.Add(uv2);
            return AddTriangle(vertex0, vertex1, vertex2, calculateNormal);
        }

        /// <summary>
        /// Adds a triangle to the draft
        /// </summary>
        public MeshDraft AddTriangle(Vector3 vertex0, Vector3 vertex1, Vector3 vertex2, Vector3 normal,
            Vector2 uv0, Vector2 uv1, Vector2 uv2)
        {
            uv.Add(uv0);
            uv.Add(uv1);
            uv.Add(uv2);
            return AddTriangle(vertex0, vertex1, vertex2, normal, normal, normal);
        }

        /// <summary>
        /// Adds a triangle to the draft
        /// </summary>
        public MeshDraft AddTriangle(Vector3 vertex0, Vector3 vertex1, Vector3 vertex2, Vector3 normal0, Vector3 normal1, Vector3 normal2,
            Vector2 uv0, Vector2 uv1, Vector2 uv2)
        {
            uv.Add(uv0);
            uv.Add(uv1);
            uv.Add(uv2);
            return AddTriangle(vertex0, vertex1, vertex2, normal0, normal1, normal2);
        }

        private MeshDraft _AddTriangle(Vector3 vertex0, Vector3 vertex1, Vector3 vertex2)
        {
            triangles.Add(0 + vertices.Count);
            triangles.Add(1 + vertices.Count);
            triangles.Add(2 + vertices.Count);
            vertices.Add(vertex0);
            vertices.Add(vertex1);
            vertices.Add(vertex2);
            return this;
        }

        #endregion AddTriangle

        #region AddQuad

        /// <summary>
        /// Adds a quad to the draft
        /// </summary>
        public MeshDraft AddQuad(Vector3 origin, Vector3 width, Vector3 height, bool calculateNormal)
        {
            Vector3 vertex0 = origin;
            Vector3 vertex1 = origin + height;
            Vector3 vertex2 = origin + height + width;
            Vector3 vertex3 = origin + width;
            if (calculateNormal)
            {
                Vector3 normal = Vector3.Cross(height, width).normalized;
                return AddQuad(vertex0, vertex1, vertex2, vertex3, normal, normal, normal, normal);
            }
            return _AddQuad(vertex0, vertex1, vertex2, vertex3);
        }

        /// <summary>
        /// Adds a quad to the draft
        /// </summary>
        public MeshDraft AddQuad(Vector3 vertex0, Vector3 vertex1, Vector3 vertex2, Vector3 vertex3, bool calculateNormal)
        {
            if (calculateNormal)
            {
                Vector3 normal = Vector3.Cross(vertex1 - vertex0, vertex3 - vertex0).normalized;
                return AddQuad(vertex0, vertex1, vertex2, vertex3, normal, normal, normal, normal);
            }
            return _AddQuad(vertex0, vertex1, vertex2, vertex3);
        }

        /// <summary>
        /// Adds a quad to the draft
        /// </summary>
        public MeshDraft AddQuad(Vector3 vertex0, Vector3 vertex1, Vector3 vertex2, Vector3 vertex3, Vector3 normal)
        {
            return AddQuad(vertex0, vertex1, vertex2, vertex3, normal, normal, normal, normal);
        }

        /// <summary>
        /// Adds a quad to the draft
        /// </summary>
        public MeshDraft AddQuad(Vector3 vertex0, Vector3 vertex1, Vector3 vertex2, Vector3 vertex3,
            Vector3 normal0, Vector3 normal1, Vector3 normal2, Vector3 normal3)
        {
            normals.Add(normal0);
            normals.Add(normal1);
            normals.Add(normal2);
            normals.Add(normal3);
            return _AddQuad(vertex0, vertex1, vertex2, vertex3);
        }

        /// <summary>
        /// Adds a quad to the draft
        /// </summary>
        public MeshDraft AddQuad(Vector3 origin, Vector3 width, Vector3 height, bool calculateNormal,
            Vector2 uv0, Vector2 uv1, Vector2 uv2, Vector2 uv3)
        {
            uv.Add(uv0);
            uv.Add(uv1);
            uv.Add(uv2);
            uv.Add(uv3);
            return AddQuad(origin, width, height, calculateNormal);
        }

        /// <summary>
        /// Adds a quad to the draft
        /// </summary>
        public MeshDraft AddQuad(Vector3 vertex0, Vector3 vertex1, Vector3 vertex2, Vector3 vertex3, bool calculateNormal,
            Vector2 uv0, Vector2 uv1, Vector2 uv2, Vector2 uv3)
        {
            uv.Add(uv0);
            uv.Add(uv1);
            uv.Add(uv2);
            uv.Add(uv3);
            return AddQuad(vertex0, vertex1, vertex2, vertex3, calculateNormal);
        }

        /// <summary>
        /// Adds a quad to the draft
        /// </summary>
        public MeshDraft AddQuad(Vector3 vertex0, Vector3 vertex1, Vector3 vertex2, Vector3 vertex3, Vector3 normal,
            Vector2 uv0, Vector2 uv1, Vector2 uv2, Vector2 uv3)
        {
            uv.Add(uv0);
            uv.Add(uv1);
            uv.Add(uv2);
            uv.Add(uv3);
            return AddQuad(vertex0, vertex1, vertex2, vertex3, normal, normal, normal, normal);
        }

        /// <summary>
        /// Adds a quad to the draft
        /// </summary>
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

        private MeshDraft _AddQuad(Vector3 vertex0, Vector3 vertex1, Vector3 vertex2, Vector3 vertex3)
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
            return this;
        }

        #endregion AddQuad

        #region AddTriangleFan

        /// <summary>
        /// Adds a triangle fan to the draft
        /// </summary>
        /// <remarks>
        /// https://en.wikipedia.org/wiki/Triangle_fan
        /// </remarks>
        public MeshDraft AddTriangleFan(IList<Vector3> fan, bool reverseTriangles = false)
        {
            Vector3 normal = Vector3.Cross(fan[1] - fan[0], fan[fan.Count - 1] - fan[0]).normalized;
            return AddTriangleFan(fan, normal, reverseTriangles);
        }

        /// <summary>
        /// Adds a triangle fan to the draft
        /// </summary>
        /// <remarks>
        /// https://en.wikipedia.org/wiki/Triangle_fan
        /// </remarks>
        public MeshDraft AddTriangleFan(IList<Vector3> fan, Vector3 normal, bool reverseTriangles = false)
        {
            AddTriangleFanVertices(fan, reverseTriangles);
            for (int i = 0; i < fan.Count; i++)
            {
                normals.Add(normal);
            }
            return this;
        }

        /// <summary>
        /// Adds a triangle fan to the draft
        /// </summary>
        /// <remarks>
        /// https://en.wikipedia.org/wiki/Triangle_fan
        /// </remarks>
        public MeshDraft AddTriangleFan(IList<Vector3> fan, IList<Vector3> normals, bool reverseTriangles = false)
        {
            AddTriangleFanVertices(fan, reverseTriangles);
            this.normals.AddRange(normals);
            return this;
        }

        /// <summary>
        /// Adds a triangle fan to the draft
        /// </summary>
        /// <remarks>
        /// https://en.wikipedia.org/wiki/Triangle_fan
        /// </remarks>
        public MeshDraft AddTriangleFan(IList<Vector3> fan, IList<Vector2> uv, bool reverseTriangles = false)
        {
            this.uv.AddRange(uv);
            return AddTriangleFan(fan, reverseTriangles);
        }

        /// <summary>
        /// Adds a triangle fan to the draft
        /// </summary>
        /// <remarks>
        /// https://en.wikipedia.org/wiki/Triangle_fan
        /// </remarks>
        public MeshDraft AddTriangleFan(IList<Vector3> fan, Vector3 normal, IList<Vector2> uv, bool reverseTriangles = false)
        {
            this.uv.AddRange(uv);
            return AddTriangleFan(fan, normal, reverseTriangles);
        }

        /// <summary>
        /// Adds a triangle fan to the draft
        /// </summary>
        /// <remarks>
        /// https://en.wikipedia.org/wiki/Triangle_fan
        /// </remarks>
        public MeshDraft AddTriangleFan(IList<Vector3> fan, IList<Vector3> normals, IList<Vector2> uv, bool reverseTriangles = false)
        {
            this.uv.AddRange(uv);
            return AddTriangleFan(fan, normals, reverseTriangles);
        }

        private void AddTriangleFanVertices(IList<Vector3> fan, bool reverseTriangles)
        {
            int count = vertices.Count;
            if (reverseTriangles)
            {
                for (int i = fan.Count - 1; i > 1; i--)
                {
                    triangles.Add(0 + count);
                    triangles.Add(i + count);
                    triangles.Add(i - 1 + count);
                }
            }
            else
            {
                for (int i = 1; i < fan.Count - 1; i++)
                {
                    triangles.Add(0 + count);
                    triangles.Add(i + count);
                    triangles.Add(i + 1 + count);
                }
            }
            vertices.AddRange(fan);
        }

        #endregion AddTriangleFan

        #region AddTriangleStrip

        /// <summary>
        /// Adds a triangle strip to the draft
        /// </summary>
        /// <remarks>
        /// https://en.wikipedia.org/wiki/Triangle_strip
        /// </remarks>
        public MeshDraft AddTriangleStrip(IList<Vector3> strip)
        {
            Vector3 normal = Vector3.Cross(strip[1] - strip[0], strip[2] - strip[0]).normalized;
            return AddTriangleStrip(strip, normal);
        }

        /// <summary>
        /// Adds a triangle strip to the draft
        /// </summary>
        /// <remarks>
        /// https://en.wikipedia.org/wiki/Triangle_strip
        /// </remarks>
        public MeshDraft AddTriangleStrip(IList<Vector3> strip, Vector3 normal)
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

        /// <summary>
        /// Adds a triangle strip to the draft
        /// </summary>
        /// <remarks>
        /// https://en.wikipedia.org/wiki/Triangle_strip
        /// </remarks>
        public MeshDraft AddTriangleStrip(IList<Vector3> strip, IList<Vector3> normals)
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

        /// <summary>
        /// Adds a triangle strip to the draft
        /// </summary>
        /// <remarks>
        /// https://en.wikipedia.org/wiki/Triangle_strip
        /// </remarks>
        public MeshDraft AddTriangleStrip(IList<Vector3> strip, IList<Vector2> uv)
        {
            this.uv.AddRange(uv);
            return AddTriangleStrip(strip);
        }

        /// <summary>
        /// Adds a triangle strip to the draft
        /// </summary>
        /// <remarks>
        /// https://en.wikipedia.org/wiki/Triangle_strip
        /// </remarks>
        public MeshDraft AddTriangleStrip(IList<Vector3> strip, Vector3 normal, IList<Vector2> uv)
        {
            this.uv.AddRange(uv);
            return AddTriangleStrip(strip, normal);
        }

        /// <summary>
        /// Adds a triangle strip to the draft
        /// </summary>
        /// <remarks>
        /// https://en.wikipedia.org/wiki/Triangle_strip
        /// </remarks>
        public MeshDraft AddTriangleStrip(IList<Vector3> strip, IList<Vector3> normals, IList<Vector2> uv)
        {
            this.uv.AddRange(uv);
            return AddTriangleStrip(strip, normals);
        }

        #endregion AddTriangleStrip

        #region AddBaselessPyramid

        /// <summary>
        /// Adds a baseless pyramid to the draft
        /// </summary>
        public MeshDraft AddBaselessPyramid(Vector3 apex, IList<Vector3> ring, bool generateUV, bool reverseTriangles = false)
        {
            if (generateUV)
            {
                var uv00 = new Vector2(0, 0);
                var uvApex = new Vector2(0.5f, 1);
                var uv10 = new Vector2(1, 0);
                if (reverseTriangles)
                {
                    for (int i = ring.Count - 1; i > 0; i--)
                    {
                        AddTriangle(ring[i - 1], apex, ring[i], true, uv00, uvApex, uv10);
                    }
                    AddTriangle(ring[ring.Count - 1], apex, ring[0], true, uv00, uvApex, uv10);
                }
                else
                {
                    for (var i = 0; i < ring.Count - 1; i++)
                    {
                        AddTriangle(ring[i + 1], apex, ring[i], true, uv00, uvApex, uv10);
                    }
                    AddTriangle(ring[0], apex, ring[ring.Count - 1], true, uv00, uvApex, uv10);
                }
            }
            else
            {
                if (reverseTriangles)
                {
                    for (int i = ring.Count - 1; i > 0; i--)
                    {
                        AddTriangle(ring[i - 1], apex, ring[i], true);
                    }
                    AddTriangle(ring[ring.Count - 1], apex, ring[0], true);
                }
                else
                {
                    for (var i = 0; i < ring.Count - 1; i++)
                    {
                        AddTriangle(ring[i + 1], apex, ring[i], true);
                    }
                    AddTriangle(ring[0], apex, ring[ring.Count - 1], true);
                }
            }
            return this;
        }

        #endregion AddBaselessPyramid

        #region AddFlatTriangleBand

        /// <summary>
        /// Adds a band made from triangles to the draft
        /// </summary>
        public MeshDraft AddFlatTriangleBand(IList<Vector3> lowerRing, IList<Vector3> upperRing, bool generateUV)
        {
            if (lowerRing.Count != upperRing.Count)
            {
                throw new ArgumentException("Array sizes must be equal");
            }
            if (lowerRing.Count < 3)
            {
                throw new ArgumentException("Array sizes must be greater than 2");
            }

            Vector2 uv00 = new Vector2(0, 0);
            Vector2 uvBottomCenter = new Vector2(0.5f, 0);
            Vector2 uv10 = new Vector2(1, 0);
            Vector2 uv01 = new Vector2(0, 1);
            Vector2 uvTopCenter = new Vector2(0.5f, 1);
            Vector2 uv11 = new Vector2(1, 1);

            Vector3 lower0, upper0, lower1, upper1;
            for (int i = 0; i < lowerRing.Count - 1; i++)
            {
                lower0 = lowerRing[i];
                lower1 = lowerRing[i + 1];
                upper0 = upperRing[i];
                upper1 = upperRing[i + 1];
                if (generateUV)
                {
                    AddTriangle(lower1, upper0, lower0, true, uv00, uvTopCenter, uv10);
                    AddTriangle(lower1, upper1, upper0, true, uvBottomCenter, uv01, uv11);
                }
                else
                {
                    AddTriangle(lower1, upper0, lower0, true);
                    AddTriangle(lower1, upper1, upper0, true);
                }
            }

            lower0 = lowerRing[lowerRing.Count - 1];
            lower1 = lowerRing[0];
            upper0 = upperRing[upperRing.Count - 1];
            upper1 = upperRing[0];
            if (generateUV)
            {
                AddTriangle(lower1, upper0, lower0, true, uv00, uvTopCenter, uv10);
                AddTriangle(lower1, upper1, upper0, true, uvBottomCenter, uv01, uv11);
            }
            else
            {
                AddTriangle(lower1, upper0, lower0, true);
                AddTriangle(lower1, upper1, upper0, true);
            }
            return this;
        }

        #endregion AddFlatTriangleBand

        #region AddFlatQuadBand

        /// <summary>
        /// Adds a band made from quads to the draft
        /// </summary>
        public MeshDraft AddFlatQuadBand(IList<Vector3> lowerRing, IList<Vector3> upperRing, bool generateUV)
        {
            if (lowerRing.Count != upperRing.Count)
            {
                throw new ArgumentException("Array sizes must be equal");
            }
            if (lowerRing.Count < 3)
            {
                throw new ArgumentException("Array sizes must be greater than 2");
            }

            Vector2 uv00 = new Vector2(0, 0);
            Vector2 uv10 = new Vector2(1, 0);
            Vector2 uv01 = new Vector2(0, 1);
            Vector2 uv11 = new Vector2(1, 1);

            Vector3 lower0, upper0, lower1, upper1;
            for (int i = 0; i < lowerRing.Count - 1; i++)
            {
                lower0 = lowerRing[i];
                lower1 = lowerRing[i + 1];
                upper0 = upperRing[i];
                upper1 = upperRing[i + 1];
                if (generateUV)
                {
                    AddQuad(lower1, upper1, upper0, lower0, true, uv00, uv01, uv11, uv10);
                }
                else
                {
                    AddQuad(lower1, upper1, upper0, lower0, true);
                }
            }

            lower0 = lowerRing[lowerRing.Count - 1];
            lower1 = lowerRing[0];
            upper0 = upperRing[upperRing.Count - 1];
            upper1 = upperRing[0];
            if (generateUV)
            {
                AddQuad(lower1, upper1, upper0, lower0, true, uv00, uv01, uv11, uv10);
            }
            else
            {
                AddQuad(lower1, upper1, upper0, lower0, true);
            }
            return this;
        }

        #endregion AddFlatQuadBand

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
        /// Reverses the winding order of draft triangles
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
        /// Reverses the direction of draft normals
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
        /// Flips the UV map horizontally in the selected <paramref name="channel"/>
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
        /// Flips the UV map vertically in the selected <paramref name="channel"/>
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
        /// Projects vertices on a sphere with the given <paramref name="radius"/> and <paramref name="center"/>, recalculates normals
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
        /// Creates a new mesh from the data in the draft
        /// </summary>
        /// <param name="calculateBounds"> Calculate the bounding box of the Mesh after setting the triangles. </param>
        /// <param name="autoIndexFormat"> Use 16 bit or 32 bit index buffers based on vertex count. </param>
        public Mesh ToMesh(bool calculateBounds = true, bool autoIndexFormat = true)
        {
            var mesh = new Mesh();
            FillMesh(ref mesh, calculateBounds, autoIndexFormat);
            return mesh;
        }

        /// <summary>
        /// Fills the <paramref name="mesh"/> with the data in the draft
        /// </summary>
        /// <param name="mesh"> Resulting mesh. Cleared before use. </param>
        /// <param name="calculateBounds"> Calculate the bounding box of the Mesh after setting the triangles. </param>
        /// <param name="autoIndexFormat"> Use 16 bit or 32 bit index buffers based on vertex count. </param>
        public void ToMesh(ref Mesh mesh, bool calculateBounds = true, bool autoIndexFormat = true)
        {
            if (mesh == null)
            {
                throw new ArgumentNullException("mesh");
            }
            mesh.Clear(false);
            FillMesh(ref mesh, calculateBounds, autoIndexFormat);
        }

        private void FillMesh(ref Mesh mesh, bool calculateBounds, bool autoIndexFormat)
        {
            if (vertexCount > 65535)
            {
                if (autoIndexFormat)
                {
                    mesh.indexFormat = IndexFormat.UInt32;
                }
                else
                {
                    Debug.LogError("A mesh can't have more than 65535 vertices with 16 bit index buffer. Vertex count: " + vertexCount);
                    mesh.indexFormat = IndexFormat.UInt16;
                }
            }
            else
            {
                mesh.indexFormat = IndexFormat.UInt16;
            }
            mesh.name = name;
            mesh.SetVertices(vertices);
            mesh.SetTriangles(triangles, 0, calculateBounds);
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
