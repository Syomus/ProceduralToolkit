using System;
using System.Collections.Generic;
using UnityEngine;

namespace ProceduralToolkit
{
    public class CompoundMeshDraft
    {
        public string name = "";

        private readonly List<MeshDraft> meshDrafts = new List<MeshDraft>();

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

        public Mesh ToMeshWithSubMeshes()
        {
            var mesh = new Mesh();
            FillMesh(ref mesh);
            return mesh;
        }

        public void ToMeshWithSubMeshes(ref Mesh mesh)
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
            var finalDraft = new MeshDraft();
#if !UNITY_2017_3_OR_NEWER
            var trianglesCounts = new List<int>();
#endif
            for (int i = 0; i < meshDrafts.Count; i++)
            {
                var draft = meshDrafts[i];
                finalDraft.Add(draft);
#if !UNITY_2017_3_OR_NEWER
                trianglesCounts.Add(draft.triangles.Count);
#endif
            }

            mesh.name = name;
            mesh.SetVertices(finalDraft.vertices);
            mesh.subMeshCount = meshDrafts.Count;
#if UNITY_2017_3_OR_NEWER
            for (int i = 0; i < meshDrafts.Count; i++)
            {
                var draft = meshDrafts[i];
                int baseVertex = i > 0 ? meshDrafts[i - 1].vertexCount : 0;
                mesh.SetTriangles(draft.triangles, i, false, baseVertex);
            }
#else
            for (int submesh = 0; submesh < trianglesCounts.Count; submesh++)
            {
                int start = 0;
                for (int i = 0; i < submesh; i++)
                {
                    start += trianglesCounts[i];
                }
                var triangles = finalDraft.triangles.GetRange(start, trianglesCounts[submesh]);
                mesh.SetTriangles(triangles, submesh, false);
            }
#endif

            mesh.SetNormals(finalDraft.normals);
            mesh.SetTangents(finalDraft.tangents);
            mesh.SetUVs(0, finalDraft.uv);
            mesh.SetUVs(1, finalDraft.uv2);
            mesh.SetUVs(2, finalDraft.uv3);
            mesh.SetUVs(3, finalDraft.uv4);
            mesh.SetColors(finalDraft.colors);
        }
    }
}
