using System.Collections.Generic;
using UnityEngine;

namespace ProceduralToolkit.Samples
{
    public class PathOffsetterExample : MonoBehaviour
    {
        public LineRenderer inputRenderer;
        public LineRenderer positiveOutputRenderer;
        public LineRenderer negativeOutputRenderer;

        private void Awake()
        {
            var input = Geometry.StarPolygon2(5, 1, 2);
            SetVertices(inputRenderer, input);

            var output = new List<List<Vector2>>();

            var offsetter = new PathOffsetter();
            offsetter.AddPath(input);
            offsetter.Offset(ref output, 0.5);
            SetVertices(positiveOutputRenderer, output[0]);

            offsetter.Offset(ref output, -0.5);
            SetVertices(negativeOutputRenderer, output[0]);
        }

        private void SetVertices(LineRenderer lineRenderer, List<Vector2> vertices)
        {
            lineRenderer.positionCount = vertices.Count;
            for (int i = 0; i < vertices.Count; i++)
            {
                lineRenderer.SetPosition(i, vertices[i]);
            }
        }
    }
}
