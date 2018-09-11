using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
                for (int i = 0; i < meshDrafts.Count; i++)
                {
                    count += meshDrafts[i].vertexCount;
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
            if (draft == null) throw new ArgumentNullException("draft");

            meshDrafts.Add(draft);
            return this;
        }

        public CompoundMeshDraft Add(CompoundMeshDraft compoundDraft)
        {
            if (compoundDraft == null) throw new ArgumentNullException("compoundDraft");

            meshDrafts.AddRange(compoundDraft.meshDrafts);
            return this;
        }

        public void Clear()
        {
            meshDrafts.Clear();
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
            for (int i = 0; i < meshDrafts.Count; i++)
            {
                finalDraft.Add(meshDrafts[i]);
            }
            return finalDraft;
        }

        public Mesh ToMeshWithSubMeshes(bool calculateBounds = true)
        {
            var mesh = new Mesh();
            FillMesh(ref mesh, calculateBounds);
            return mesh;
        }

        public void ToMeshWithSubMeshes(ref Mesh mesh, bool calculateBounds = true)
        {
            if (mesh == null)
            {
                throw new ArgumentNullException("mesh");
            }
            mesh.Clear(false);
            FillMesh(ref mesh, calculateBounds);
        }

        private void FillMesh(ref Mesh mesh, bool calculateBounds)
        {
            int vCount = vertexCount;
            if (vCount > 65000)
            {
                Debug.LogError("A mesh may not have more than 65000 vertices. Vertex count: " + vCount);
            }

            var finalDraft = new MeshDraft();
            for (int i = 0; i < meshDrafts.Count; i++)
            {
                var draft = meshDrafts[i];
                finalDraft.Add(draft);
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
