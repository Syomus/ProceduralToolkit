using UnityEngine;

namespace ProceduralToolkit
{
    public static class GizmosE
    {
        private const int circleSegments = 64;
        private const float circleSegmentAngle = Mathf.PI*2/circleSegments;

        public static void DrawWireQuadXY(Vector3 position, Quaternion rotation, Vector2 scale)
        {
            Vector3 right = rotation*Vector3.right*scale.x;
            Vector3 up = rotation*Vector3.up*scale.y;
            Vector3 upperRight = position + right*0.5f + up*0.5f;
            Vector3 lowerRight = upperRight - up;
            Vector3 lowerLeft = lowerRight - right;
            Vector3 upperLeft = upperRight - right;

            Gizmos.DrawLine(upperRight, lowerRight);
            Gizmos.DrawLine(lowerRight, lowerLeft);
            Gizmos.DrawLine(lowerLeft, upperLeft);
            Gizmos.DrawLine(upperLeft, upperRight);
        }

        public static void DrawWireQuadXZ(Vector3 position, Quaternion rotation, Vector2 scale)
        {
            Vector3 right = rotation*Vector3.right*scale.x;
            Vector3 forward = rotation*Vector3.forward*scale.y;
            Vector3 forwardRight = position + right*0.5f + forward*0.5f;
            Vector3 backRight = forwardRight - forward;
            Vector3 backLeft = backRight - right;
            Vector3 forwardLeft = forwardRight - right;

            Gizmos.DrawLine(forwardRight, backRight);
            Gizmos.DrawLine(backRight, backLeft);
            Gizmos.DrawLine(backLeft, forwardLeft);
            Gizmos.DrawLine(forwardLeft, forwardRight);
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

            Gizmos.DrawLine(a1, b1);
            Gizmos.DrawLine(b1, c1);
            Gizmos.DrawLine(c1, d1);
            Gizmos.DrawLine(d1, a1);

            Gizmos.DrawLine(a2, b2);
            Gizmos.DrawLine(b2, c2);
            Gizmos.DrawLine(c2, d2);
            Gizmos.DrawLine(d2, a2);

            Gizmos.DrawLine(a1, a2);
            Gizmos.DrawLine(b1, b2);
            Gizmos.DrawLine(c1, c2);
            Gizmos.DrawLine(d1, d2);
        }

        public static void DrawWireCircleXY(Vector3 position, float radius)
        {
            float currentAngle = 0;
            for (var i = 0; i < circleSegments; i++)
            {
                Vector3 a = position + PTUtils.PointOnCircle3XY(radius, currentAngle);
                Vector3 b = position + PTUtils.PointOnCircle3XY(radius, currentAngle - circleSegmentAngle);
                Gizmos.DrawLine(a, b);
                currentAngle -= circleSegmentAngle;
            }
        }

        public static void DrawWireCircleXY(Vector3 position, Quaternion rotation, float radius)
        {
            float currentAngle = 0;
            for (var i = 0; i < circleSegments; i++)
            {
                Vector3 a = position + rotation*PTUtils.PointOnCircle3XY(radius, currentAngle);
                Vector3 b = position + rotation*PTUtils.PointOnCircle3XY(radius, currentAngle - circleSegmentAngle);
                Gizmos.DrawLine(a, b);
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
                Gizmos.DrawLine(a, b);
                currentAngle -= circleSegmentAngle;
            }
        }

        public static void DrawWireCircleXZ(Vector3 position, Quaternion rotation, float radius)
        {
            float currentAngle = 0;
            for (var i = 0; i < circleSegments; i++)
            {
                Vector3 a = position + rotation*PTUtils.PointOnCircle3XZ(radius, currentAngle);
                Vector3 b = position + rotation*PTUtils.PointOnCircle3XZ(radius, currentAngle - circleSegmentAngle);
                Gizmos.DrawLine(a, b);
                currentAngle -= circleSegmentAngle;
            }
        }

        public static void DrawWireCircleYZ(Vector3 position, float radius)
        {
            float currentAngle = 0;
            for (var i = 0; i < circleSegments; i++)
            {
                Vector3 a = position + PTUtils.PointOnCircle3YZ(radius, currentAngle);
                Vector3 b = position + PTUtils.PointOnCircle3YZ(radius, currentAngle - circleSegmentAngle);
                Gizmos.DrawLine(a, b);
                currentAngle -= circleSegmentAngle;
            }
        }

        public static void DrawWireCircleYZ(Vector3 position, Quaternion rotation, float radius)
        {
            float currentAngle = 0;
            for (var i = 0; i < circleSegments; i++)
            {
                Vector3 a = position + rotation*PTUtils.PointOnCircle3YZ(radius, currentAngle);
                Vector3 b = position + rotation*PTUtils.PointOnCircle3YZ(radius, currentAngle - circleSegmentAngle);
                Gizmos.DrawLine(a, b);
                currentAngle -= circleSegmentAngle;
            }
        }

        public static void DrawWireSphere(Vector3 position, Quaternion rotation, float radius)
        {
            DrawWireCircleXY(position, rotation, radius);
            DrawWireCircleXZ(position, rotation, radius);
            DrawWireCircleYZ(position, rotation, radius);
        }
    }
}