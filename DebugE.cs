using UnityEngine;

namespace ProceduralToolkit
{
    public static class DebugE
    {
        private const int circleSegments = 64;
        private const float circleSegmentAngle = Mathf.PI*2/circleSegments;

        public static void DrawWireQuadXY(Vector3 position, Quaternion rotation, Vector2 scale)
        {
            DrawWireQuadXY(position, rotation, scale, Color.white);
        }

        public static void DrawWireQuadXY(
            Vector3 position,
            Quaternion rotation,
            Vector2 scale,
            Color color,
            float duration = 0,
            bool depthTest = true)
        {
            Vector3 right = rotation*Vector3.right*scale.x;
            Vector3 up = rotation*Vector3.up*scale.y;
            Vector3 upperRight = position + right*0.5f + up*0.5f;
            Vector3 lowerRight = upperRight - up;
            Vector3 lowerLeft = lowerRight - right;
            Vector3 upperLeft = upperRight - right;

            Debug.DrawLine(upperRight, lowerRight, color, duration, depthTest);
            Debug.DrawLine(lowerRight, lowerLeft, color, duration, depthTest);
            Debug.DrawLine(lowerLeft, upperLeft, color, duration, depthTest);
            Debug.DrawLine(upperLeft, upperRight, color, duration, depthTest);
        }

        public static void DrawWireQuadXZ(Vector3 position, Quaternion rotation, Vector2 scale)
        {
            DrawWireQuadXZ(position, rotation, scale, Color.white);
        }

        public static void DrawWireQuadXZ(
            Vector3 position,
            Quaternion rotation,
            Vector2 scale,
            Color color,
            float duration = 0,
            bool depthTest = true)
        {
            Vector3 right = rotation*Vector3.right*scale.x;
            Vector3 forward = rotation*Vector3.forward*scale.y;
            Vector3 forwardRight = position + right*0.5f + forward*0.5f;
            Vector3 backRight = forwardRight - forward;
            Vector3 backLeft = backRight - right;
            Vector3 forwardLeft = forwardRight - right;

            Debug.DrawLine(forwardRight, backRight, color, duration, depthTest);
            Debug.DrawLine(backRight, backLeft, color, duration, depthTest);
            Debug.DrawLine(backLeft, forwardLeft, color, duration, depthTest);
            Debug.DrawLine(forwardLeft, forwardRight, color, duration, depthTest);
        }

        public static void DrawWireCube(Vector3 position, Quaternion rotation, Vector3 scale)
        {
            DrawWireCube(position, rotation, scale, Color.white);
        }

        public static void DrawWireCube(
            Vector3 position,
            Quaternion rotation,
            Vector3 scale,
            Color color,
            float duration = 0,
            bool depthTest = true)
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

            Debug.DrawLine(a1, b1, color, duration, depthTest);
            Debug.DrawLine(b1, c1, color, duration, depthTest);
            Debug.DrawLine(c1, d1, color, duration, depthTest);
            Debug.DrawLine(d1, a1, color, duration, depthTest);

            Debug.DrawLine(a2, b2, color, duration, depthTest);
            Debug.DrawLine(b2, c2, color, duration, depthTest);
            Debug.DrawLine(c2, d2, color, duration, depthTest);
            Debug.DrawLine(d2, a2, color, duration, depthTest);

            Debug.DrawLine(a1, a2, color, duration, depthTest);
            Debug.DrawLine(b1, b2, color, duration, depthTest);
            Debug.DrawLine(c1, c2, color, duration, depthTest);
            Debug.DrawLine(d1, d2, color, duration, depthTest);
        }

        public static void DrawWireCircleXY(Vector3 position, float radius)
        {
            DrawWireCircleXY(position, radius, Color.white);
        }

        public static void DrawWireCircleXY(
            Vector3 position,
            float radius,
            Color color,
            float duration = 0,
            bool depthTest = true)
        {
            float currentAngle = 0;
            for (var i = 0; i < circleSegments; i++)
            {
                Vector3 a = position + PTUtils.PointOnCircle3XY(radius, currentAngle);
                Vector3 b = position + PTUtils.PointOnCircle3XY(radius, currentAngle - circleSegmentAngle);
                Debug.DrawLine(a, b, color, duration, depthTest);
                currentAngle -= circleSegmentAngle;
            }
        }

        public static void DrawWireCircleXZ(Vector3 position, float radius)
        {
            DrawWireCircleXZ(position, radius, Color.white);
        }

        public static void DrawWireCircleXZ(
            Vector3 position,
            float radius,
            Color color,
            float duration = 0,
            bool depthTest = true)
        {
            float currentAngle = 0;
            for (var i = 0; i < circleSegments; i++)
            {
                Vector3 a = position + PTUtils.PointOnCircle3XZ(radius, currentAngle);
                Vector3 b = position + PTUtils.PointOnCircle3XZ(radius, currentAngle - circleSegmentAngle);
                Debug.DrawLine(a, b, color, duration, depthTest);
                currentAngle -= circleSegmentAngle;
            }
        }
    }
}