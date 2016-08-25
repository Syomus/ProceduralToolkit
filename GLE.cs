using UnityEngine;
using UnityEngine.Rendering;

namespace ProceduralToolkit
{
    public static class GLE
    {
        public static readonly Material wireMaterial;

        private const int circleSegments = 64;
        private const float circleSegmentAngle = Mathf.PI*2/circleSegments;

        static GLE()
        {
            var shader = Shader.Find("Hidden/Internal-Colored");
            wireMaterial = new Material(shader) {hideFlags = HideFlags.HideAndDontSave};
            wireMaterial.SetInt("_SrcBlend", (int) BlendMode.SrcAlpha);
            wireMaterial.SetInt("_DstBlend", (int) BlendMode.OneMinusSrcAlpha);
            wireMaterial.SetInt("_Cull", (int) CullMode.Off);
            wireMaterial.SetInt("_ZWrite", 0);
        }

        public static void DrawLine(Vector3 from, Vector3 to)
        {
            GL.Vertex(from);
            GL.Vertex(to);
        }

        public static void DrawWireQuadXY(Vector3 position, Quaternion rotation, Vector2 scale)
        {
            Vector3 right = rotation*Vector3.right*scale.x;
            Vector3 up = rotation*Vector3.up*scale.y;
            Vector3 upperRight = position + right*0.5f + up*0.5f;
            Vector3 lowerRight = upperRight - up;
            Vector3 lowerLeft = lowerRight - right;
            Vector3 upperLeft = upperRight - right;

            DrawLine(upperRight, lowerRight);
            DrawLine(lowerRight, lowerLeft);
            DrawLine(lowerLeft, upperLeft);
            DrawLine(upperLeft, upperRight);
        }

        public static void DrawWireQuadXZ(Vector3 position, Quaternion rotation, Vector2 scale)
        {
            Vector3 right = rotation*Vector3.right*scale.x;
            Vector3 forward = rotation*Vector3.forward*scale.y;
            Vector3 forwardRight = position + right*0.5f + forward*0.5f;
            Vector3 backRight = forwardRight - forward;
            Vector3 backLeft = backRight - right;
            Vector3 forwardLeft = forwardRight - right;

            DrawLine(forwardRight, backRight);
            DrawLine(backRight, backLeft);
            DrawLine(backLeft, forwardLeft);
            DrawLine(forwardLeft, forwardRight);
        }

        public static void DrawWireCube(Vector3 position, Quaternion rotation, Vector3 scale)
        {
            Vector3 right = rotation*Vector3.right*scale.x;
            Vector3 up = rotation*Vector3.up*scale.y;
            Vector3 forward = rotation*Vector3.forward*scale.z;

            Vector3 a1 = position + right*0.5f + up*0.5f + forward*0.5f;
            Vector3 b1 = a1 - up;
            Vector3 c1 = b1 - right;
            Vector3 d1 = a1 - right;

            Vector3 a2 = a1 - forward;
            Vector3 b2 = b1 - forward;
            Vector3 c2 = c1 - forward;
            Vector3 d2 = d1 - forward;

            DrawLine(a1, b1);
            DrawLine(b1, c1);
            DrawLine(c1, d1);
            DrawLine(d1, a1);

            DrawLine(a2, b2);
            DrawLine(b2, c2);
            DrawLine(c2, d2);
            DrawLine(d2, a2);

            DrawLine(a1, a2);
            DrawLine(b1, b2);
            DrawLine(c1, c2);
            DrawLine(d1, d2);
        }

        public static void DrawWireCircleXY(Vector3 position, float radius)
        {
            float currentAngle = 0;
            for (var i = 0; i < circleSegments; i++)
            {
                Vector3 a = position + PTUtils.PointOnCircle3XY(radius, currentAngle);
                Vector3 b = position + PTUtils.PointOnCircle3XY(radius, currentAngle - circleSegmentAngle);
                DrawLine(a, b);
                currentAngle -= circleSegmentAngle;
            }
        }

        public static void DrawWireCircleXZ(Vector3 position, float radius)
        {
            float currentAngle = 0;
            for (var i = 0; i < circleSegments; i++)
            {
                Vector3 a = position + PTUtils.PointOnCircle3XZ(radius, currentAngle);
                Vector3 b = position + PTUtils.PointOnCircle3XZ(radius, currentAngle - circleSegmentAngle);
                DrawLine(a, b);
                currentAngle -= circleSegmentAngle;
            }
        }
    }
}