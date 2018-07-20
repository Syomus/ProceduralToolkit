using System.Collections.Generic;
using UnityEngine;

namespace ProceduralToolkit.Examples
{
    public class PathOffsetterExample : MonoBehaviour
    {
        public LineRenderer inputRenderer;
        public LineRenderer positiveOutputRenderer;
        public LineRenderer negativeOutputRenderer;

        private void Awake()
        {
            var input = new List<Vector2>(4)
            {
                new Vector2(+0.00000f, +2.00000f),
                new Vector2(+0.58779f, +0.80902f),
                new Vector2(+1.90211f, +0.61803f),
                new Vector2(+0.95106f, -0.30902f),
                new Vector2(+1.17557f, -1.61803f),
                new Vector2(-0.00000f, -1.00000f),
                new Vector2(-1.17557f, -1.61803f),
                new Vector2(-0.95106f, -0.30902f),
                new Vector2(-1.90211f, +0.61803f),
                new Vector2(-0.58779f, +0.80902f),
            };
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
