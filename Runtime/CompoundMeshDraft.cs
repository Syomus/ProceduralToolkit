using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace ProceduralToolkit
{
    /// <summary>
    /// Helper class for mesh generation supporting large meshes and submeshes
    /// </summary>
    public class CompoundMeshDraft : IEnumerable<MeshDraft>
    {
        public string name = "";

        public int vertexCount
        {
            get
            {
                int count = 0;
                foreach (var meshDraft in meshDrafts)
                {
                    count += meshDraft.vertexCount;
                }
                return count;
            }
        }

        private readonly List<MeshDraft> meshDrafts = new List<MeshDraft>();

        public IEnumerator<MeshDraft> GetEnumerator()
        {
            return meshDrafts.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public CompoundMeshDraft Add(MeshDraft draft)
        {
            if (draft == null) throw new ArgumentNullException(nameof(draft));

            meshDrafts.Add(draft);
            return this;
        }

        public CompoundMeshDraft Add(CompoundMeshDraft compoundDraft)
        {
            if (compoundDraft == null) throw new ArgumentNullException(nameof(compoundDraft));

            meshDrafts.AddRange(compoundDraft.meshDrafts);
            return this;
        }

        /// <summary>
        /// Clears all vertex data and all triangle indices
        /// </summary>
        public void Clear()
        {
            meshDrafts.Clear();
        }

        /// <summary>
        /// Moves draft vertices by <paramref name="vector"/>
        /// </summary>
        public CompoundMeshDraft Move(Vector3 vector)
        {
            foreach (var meshDraft in meshDrafts)
            {
                meshDraft.Move(vector);
            }
            return this;
        }

        /// <summary>
        /// Rotates draft vertices by <paramref name="rotation"/>
        /// </summary>
        public CompoundMeshDraft Rotate(Quaternion rotation)
        {
            foreach (var meshDraft in meshDrafts)
            {
                meshDraft.Rotate(rotation);
            }
            return this;
        }

        /// <summary>
        /// Scales draft vertices uniformly by <paramref name="scale"/>
        /// </summary>
        public CompoundMeshDraft Scale(float scale)
        {
            foreach (var meshDraft in meshDrafts)
            {
                meshDraft.Scale(scale);
            }
            return this;
        }

        /// <summary>
        /// Scales draft vertices non-uniformly by <paramref name="scale"/>
        /// </summary>
        public CompoundMeshDraft Scale(Vector3 scale)
        {
            foreach (var meshDraft in meshDrafts)
            {
                meshDraft.Scale(scale);
            }
            return this;
        }

        /// <summary>
        /// Paints draft vertices with <paramref name="color"/>
        /// </summary>
        public CompoundMeshDraft Paint(Color color)
        {
            foreach (var meshDraft in meshDrafts)
            {
                meshDraft.Paint(color);
            }
            return this;
        }

        /// <summary>
        /// Flips draft faces
        /// </summary>
        public CompoundMeshDraft FlipFaces()
        {
            foreach (var meshDraft in meshDrafts)
            {
                meshDraft.FlipFaces();
            }
            return this;
        }

        /// <summary>
        /// Reverses the winding order of draft triangles
        /// </summary>
        public CompoundMeshDraft FlipTriangles()
        {
            foreach (var meshDraft in meshDrafts)
            {
                meshDraft.FlipTriangles();
            }
            return this;
        }

        /// <summary>
        /// Reverses the direction of draft normals
        /// </summary>
        public CompoundMeshDraft FlipNormals()
        {
            foreach (var meshDraft in meshDrafts)
            {
                meshDraft.FlipNormals();
            }
            return this;
        }

        /// <summary>
        /// Flips the UV map horizontally in the selected <paramref name="channel"/>
        /// </summary>
        public CompoundMeshDraft FlipUVHorizontally(int channel = 0)
        {
            foreach (var meshDraft in meshDrafts)
            {
                meshDraft.FlipUVHorizontally(channel);
            }
            return this;
        }

        /// <summary>
        /// Flips the UV map vertically in the selected <paramref name="channel"/>
        /// </summary>
        public CompoundMeshDraft FlipUVVertically(int channel = 0)
        {
            foreach (var meshDraft in meshDrafts)
            {
                meshDraft.FlipUVVertically(channel);
            }
            return this;
        }

        /// <summary>
        /// Projects vertices on a sphere with the given <paramref name="radius"/> and <paramref name="center"/>, recalculates normals
        /// </summary>
        public CompoundMeshDraft Spherify(float radius, Vector3 center = default)
        {
            foreach (var meshDraft in meshDrafts)
            {
                meshDraft.Spherify(radius, center);
            }
            return this;
        }

        public void MergeDraftsWithTheSameName()
        {
            for (int i = 0; i < meshDrafts.Count; i++)
            {
                var merged = MergeDraftsWithName(meshDrafts[i].name);
                meshDrafts.Insert(i, merged);
            }
        }

        private MeshDraft MergeDraftsWithName(string draftName)
        {
            var merged = new MeshDraft {name = draftName};
            for (int i = 0; i < meshDrafts.Count; i++)
            {
                var meshDraft = meshDrafts[i];
                if (meshDraft.name == draftName)
                {
                    merged.Add(meshDraft);
                    meshDrafts.RemoveAt(i);
                    i--;
                }
            }
            return merged;
        }

        public void SortDraftsByName()
        {
            meshDrafts.Sort((a, b) => a.name.CompareTo(b.name));
        }

        public MeshDraft ToMeshDraft()
        {
            var finalDraft = new MeshDraft();
            foreach (var meshDraft in meshDrafts)
            {
                finalDraft.Add(meshDraft);
            }
            return finalDraft;
        }

        /// <summary>
        /// Creates a new mesh from the data in the draft
        /// </summary>
        /// <param name="calculateBounds"> Calculate the bounding box of the Mesh after setting the triangles. </param>
        /// <param name="autoIndexFormat"> Use 16 bit or 32 bit index buffers based on vertex count. </param>
        public Mesh ToMeshWithSubMeshes(bool calculateBounds = true, bool autoIndexFormat = true)
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
        public void ToMeshWithSubMeshes(ref Mesh mesh, bool calculateBounds = true, bool autoIndexFormat = true)
        {
            if (mesh == null)
            {
                throw new ArgumentNullException(nameof(mesh));
            }
            mesh.Clear(false);
            FillMesh(ref mesh, calculateBounds, autoIndexFormat);
        }

        private void FillMesh(ref Mesh mesh, bool calculateBounds, bool autoIndexFormat)
        {
            int vCount = vertexCount;
            if (vCount > 65535)
            {
                if (autoIndexFormat)
                {
                    mesh.indexFormat = IndexFormat.UInt32;
                }
                else
                {
                    Debug.LogError("A mesh can't have more than 65535 vertices with 16 bit index buffer. Vertex count: " + vCount);
                    mesh.indexFormat = IndexFormat.UInt16;
                }
            }
            else
            {
                mesh.indexFormat = IndexFormat.UInt16;
            }

            var finalDraft = new MeshDraft();
            foreach (var meshDraft in meshDrafts)
            {
                finalDraft.Add(meshDraft);
            }

            mesh.name = name;
            mesh.SetVertices(finalDraft.vertices);
            mesh.subMeshCount = meshDrafts.Count;

            int baseVertex = 0;
            for (int i = 0; i < meshDrafts.Count; i++)
            {
                var draft = meshDrafts[i];
                mesh.SetTriangles(draft.triangles, i, false, baseVertex);
                baseVertex += draft.vertexCount;
            }
            if (calculateBounds)
            {
                mesh.RecalculateBounds();
            }

            mesh.SetNormals(finalDraft.normals);
            mesh.SetTangents(finalDraft.tangents);
            mesh.SetUVs(0, finalDraft.uv);
            mesh.SetUVs(1, finalDraft.uv2);
            mesh.SetUVs(2, finalDraft.uv3);
            mesh.SetUVs(3, finalDraft.uv4);
            mesh.SetColors(finalDraft.colors);
        }

        public override string ToString()
        {
            return name + " (ProceduralToolkit.CompoundMeshDraft)";
        }
    }
}
