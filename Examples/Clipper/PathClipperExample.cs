using System.Collections.Generic;
using UnityEngine;
using ProceduralToolkit.ClipperLib;

namespace ProceduralToolkit.Examples
{
    public class PathClipperExample : MonoBehaviour
    {
        public LineRenderer subjectRenderer;
        public LineRenderer clipRenderer;
        public LineRenderer outputRenderer;

        private void Awake()
        {
            var subject = new List<Vector2>(4)
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
            SetVertices(subjectRenderer, subject);

            var clip = Geometry.PointsOnCircle2(1.5f, 50);
            SetVertices(clipRenderer, clip);

            var output = new List<List<Vector2>>();

            var clipper = new PathClipper();
            clipper.AddPath(subject, PolyType.ptSubject);
            clipper.AddPath(clip, PolyType.ptClip);
            clipper.Clip(ClipType.ctIntersection, ref output);
            SetVertices(outputRenderer, output[0]);
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
