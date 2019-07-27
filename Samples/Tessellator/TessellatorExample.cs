using System.Collections.Generic;
using UnityEngine;

namespace ProceduralToolkit.Examples
{
    public class TessellatorExample : MonoBehaviour
    {
        public LineRenderer lineRenderer;
        public MeshFilter meshFilter;

        private List<Vector3> vertices = new List<Vector3>
        {
            new Vector3(0, 3),
            new Vector3(1, 0),
            new Vector3(-1.6f, 1.9f),
            new Vector3(1.6f, 1.9f),
            new Vector3(-1, 0),
        };

        private void Awake()
        {
            lineRenderer.positionCount = vertices.Count;
            for (int i = 0; i < vertices.Count; i++)
            {
                lineRenderer.SetPosition(i, vertices[i]);
            }

            var tessellator = new Tessellator();
            tessellator.AddContour(vertices);
            tessellator.Tessellate(normal: Vector3.back);

            meshFilter.mesh = tessellator.ToMesh();
        }
    }
}
