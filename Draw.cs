using System;
using UnityEngine;

namespace ProceduralToolkit
{
    public static class Draw
    {
        public delegate void DebugDrawLine(Vector3 start, Vector3 end, Color color, float duration, bool depthTest);

        private const int circleSegments = 64;
        private const float circleSegmentAngle = Mathf.PI*2/circleSegments;

        private static readonly Func<float, float, Vector3> pointOnCircleXY;
        private static readonly Func<float, float, Vector3> pointOnCircleXZ;
        private static readonly Func<float, float, Vector3> pointOnCircleYZ;

        static Draw()
        {
            pointOnCircleXY = PTUtils.PointOnCircle3XY;
            pointOnCircleXZ = PTUtils.PointOnCircle3XZ;
            pointOnCircleYZ = PTUtils.PointOnCircle3YZ;
        }

        public static void WireQuadXY(
            Action<Vector3, Vector3> drawLine,
            Vector3 position,
            Quaternion rotation,
            Vector2 scale)
        {
            Vector3 right = rotation*Vector3.right*scale.x;
            Vector3 up = rotation*Vector3.up*scale.y;
            Vector3 upperRight = position + right*0.5f + up*0.5f;
            Vector3 lowerRight = upperRight - up;
            Vector3 lowerLeft = lowerRight - right;
            Vector3 upperLeft = upperRight - right;

            drawLine(upperRight, lowerRight);
            drawLine(lowerRight, lowerLeft);
            drawLine(lowerLeft, upperLeft);
            drawLine(upperLeft, upperRight);
        }

        public static void WireQuadXY(
            DebugDrawLine drawLine,
            Vector3 position,
            Quaternion rotation,
            Vector2 scale,
            Color color,
            float duration,
            bool depthTest)
        {
            Vector3 right = rotation*Vector3.right*scale.x;
            Vector3 up = rotation*Vector3.up*scale.y;
            Vector3 upperRight = position + right*0.5f + up*0.5f;
            Vector3 lowerRight = upperRight - up;
            Vector3 lowerLeft = lowerRight - right;
            Vector3 upperLeft = upperRight - right;

            drawLine(upperRight, lowerRight, color, duration, depthTest);
            drawLine(lowerRight, lowerLeft, color, duration, depthTest);
            drawLine(lowerLeft, upperLeft, color, duration, depthTest);
            drawLine(upperLeft, upperRight, color, duration, depthTest);
        }

        public static void WireQuadXZ(
            Action<Vector3, Vector3> drawLine,
            Vector3 position,
            Quaternion rotation,
            Vector2 scale)
        {
            Vector3 right = rotation*Vector3.right*scale.x;
            Vector3 forward = rotation*Vector3.forward*scale.y;
            Vector3 forwardRight = position + right*0.5f + forward*0.5f;
            Vector3 backRight = forwardRight - forward;
            Vector3 backLeft = backRight - right;
            Vector3 forwardLeft = forwardRight - right;

            drawLine(forwardRight, backRight);
            drawLine(backRight, backLeft);
            drawLine(backLeft, forwardLeft);
            drawLine(forwardLeft, forwardRight);
        }

        public static void WireQuadXZ(
            DebugDrawLine drawLine,
            Vector3 position,
            Quaternion rotation,
            Vector2 scale,
            Color color,
            float duration,
            bool depthTest)
        {
            Vector3 right = rotation*Vector3.right*scale.x;
            Vector3 forward = rotation*Vector3.forward*scale.y;
            Vector3 forwardRight = position + right*0.5f + forward*0.5f;
            Vector3 backRight = forwardRight - forward;
            Vector3 backLeft = backRight - right;
            Vector3 forwardLeft = forwardRight - right;

            drawLine(forwardRight, backRight, color, duration, depthTest);
            drawLine(backRight, backLeft, color, duration, depthTest);
            drawLine(backLeft, forwardLeft, color, duration, depthTest);
            drawLine(forwardLeft, forwardRight, color, duration, depthTest);
        }

        public static void WireCube(
            Action<Vector3, Vector3> drawLine,
            Vector3 position,
            Quaternion rotation,
            Vector3 scale)
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

            drawLine(a1, b1);
            drawLine(b1, c1);
            drawLine(c1, d1);
            drawLine(d1, a1);

            drawLine(a2, b2);
            drawLine(b2, c2);
            drawLine(c2, d2);
            drawLine(d2, a2);

            drawLine(a1, a2);
            drawLine(b1, b2);
            drawLine(c1, c2);
            drawLine(d1, d2);
        }

        public static void WireCube(
            DebugDrawLine drawLine,
            Vector3 position,
            Quaternion rotation,
            Vector3 scale,
            Color color,
            float duration,
            bool depthTest)
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

            drawLine(a1, b1, color, duration, depthTest);
            drawLine(b1, c1, color, duration, depthTest);
            drawLine(c1, d1, color, duration, depthTest);
            drawLine(d1, a1, color, duration, depthTest);

            drawLine(a2, b2, color, duration, depthTest);
            drawLine(b2, c2, color, duration, depthTest);
            drawLine(c2, d2, color, duration, depthTest);
            drawLine(d2, a2, color, duration, depthTest);

            drawLine(a1, a2, color, duration, depthTest);
            drawLine(b1, b2, color, duration, depthTest);
            drawLine(c1, c2, color, duration, depthTest);
            drawLine(d1, d2, color, duration, depthTest);
        }

        #region WireCircleXY

        public static void WireCircleXY(
            Action<Vector3, Vector3> drawLine,
            Vector3 position,
            float radius)
        {
            WireCircle(pointOnCircleXY, drawLine, position, radius);
        }

        public static void WireCircleXY(
            DebugDrawLine drawLine,
            Vector3 position,
            float radius,
            Color color,
            float duration,
            bool depthTest)
        {
            WireCircle(pointOnCircleXY, drawLine, position, radius, color, duration, depthTest);
        }

        public static void WireCircleXY(
            Action<Vector3, Vector3> drawLine,
            Vector3 position,
            Quaternion rotation,
            float radius)
        {
            WireCircle(pointOnCircleXY, drawLine, position, rotation, radius);
        }

        public static void WireCircleXY(
            DebugDrawLine drawLine,
            Vector3 position,
            Quaternion rotation,
            float radius,
            Color color,
            float duration,
            bool depthTest)
        {
            WireCircle(pointOnCircleXY, drawLine, position, rotation, radius, color, duration, depthTest);
        }

        #endregion WireCircleXY

        #region WireCircleXZ

        public static void WireCircleXZ(
            Action<Vector3, Vector3> drawLine,
            Vector3 position,
            float radius)
        {
            WireCircle(pointOnCircleXZ, drawLine, position, radius);
        }

        public static void WireCircleXZ(
            DebugDrawLine drawLine,
            Vector3 position,
            float radius,
            Color color,
            float duration,
            bool depthTest)
        {
            WireCircle(pointOnCircleXZ, drawLine, position, radius, color, duration, depthTest);
        }

        public static void WireCircleXZ(
            Action<Vector3, Vector3> drawLine,
            Vector3 position,
            Quaternion rotation,
            float radius)
        {
            WireCircle(pointOnCircleXZ, drawLine, position, rotation, radius);
        }

        public static void WireCircleXZ(
            DebugDrawLine drawLine,
            Vector3 position,
            Quaternion rotation,
            float radius,
            Color color,
            float duration,
            bool depthTest)
        {
            WireCircle(pointOnCircleYZ, drawLine, position, rotation, radius, color, duration, depthTest);
        }

        #endregion WireCircleXZ

        #region WireCircleYZ

        public static void WireCircleYZ(
            Action<Vector3, Vector3> drawLine,
            Vector3 position,
            float radius)
        {
            WireCircle(pointOnCircleYZ, drawLine, position, radius);
        }

        public static void WireCircleYZ(
            DebugDrawLine drawLine,
            Vector3 position,
            float radius,
            Color color,
            float duration,
            bool depthTest)
        {
            WireCircle(pointOnCircleYZ, drawLine, position, radius, color, duration, depthTest);
        }

        public static void WireCircleYZ(
            Action<Vector3, Vector3> drawLine,
            Vector3 position,
            Quaternion rotation,
            float radius)
        {
            WireCircle(pointOnCircleYZ, drawLine, position, rotation, radius);
        }

        public static void WireCircleYZ(
            DebugDrawLine drawLine,
            Vector3 position,
            Quaternion rotation,
            float radius,
            Color color,
            float duration,
            bool depthTest)
        {
            WireCircle(pointOnCircleYZ, drawLine, position, rotation, radius, color, duration, depthTest);
        }

        #endregion WireCircleYZ

        #region WireCircle

        public static void WireCircle(
            Func<float, float, Vector3> pointOnCircle,
            Action<Vector3, Vector3> drawLine,
            Vector3 position,
            float radius)
        {
            float currentAngle = 0;
            for (var i = 0; i < circleSegments; i++)
            {
                Vector3 a = position + pointOnCircle(radius, currentAngle);
                Vector3 b = position + pointOnCircle(radius, currentAngle - circleSegmentAngle);
                drawLine(a, b);
                currentAngle -= circleSegmentAngle;
            }
        }

        public static void WireCircle(
            Func<float, float, Vector3> pointOnCircle,
            DebugDrawLine drawLine,
            Vector3 position,
            float radius,
            Color color,
            float duration,
            bool depthTest)
        {
            float currentAngle = 0;
            for (var i = 0; i < circleSegments; i++)
            {
                Vector3 a = position + pointOnCircle(radius, currentAngle);
                Vector3 b = position + pointOnCircle(radius, currentAngle - circleSegmentAngle);
                drawLine(a, b, color, duration, depthTest);
                currentAngle -= circleSegmentAngle;
            }
        }

        public static void WireCircle(
            Func<float, float, Vector3> pointOnCircle,
            Action<Vector3, Vector3> drawLine,
            Vector3 position,
            Quaternion rotation,
            float radius)
        {
            float currentAngle = 0;
            for (var i = 0; i < circleSegments; i++)
            {
                Vector3 a = position + rotation*pointOnCircle(radius, currentAngle);
                Vector3 b = position + rotation*pointOnCircle(radius, currentAngle - circleSegmentAngle);
                drawLine(a, b);
                currentAngle -= circleSegmentAngle;
            }
        }

        public static void WireCircle(
            Func<float, float, Vector3> pointOnCircle,
            DebugDrawLine drawLine,
            Vector3 position,
            Quaternion rotation,
            float radius,
            Color color,
            float duration,
            bool depthTest)
        {
            float currentAngle = 0;
            for (var i = 0; i < circleSegments; i++)
            {
                Vector3 a = position + rotation*pointOnCircle(radius, currentAngle);
                Vector3 b = position + rotation*pointOnCircle(radius, currentAngle - circleSegmentAngle);
                drawLine(a, b, color, duration, depthTest);
                currentAngle -= circleSegmentAngle;
            }
        }

        #endregion WireCircle

        public static void WireSphere(
            Action<Vector3, Vector3> drawLine,
            Vector3 position,
            Quaternion rotation,
            float radius)
        {
            WireCircleXY(drawLine, position, rotation, radius);
            WireCircleXZ(drawLine, position, rotation, radius);
            WireCircleYZ(drawLine, position, rotation, radius);
        }

        public static void WireSphere(
            DebugDrawLine drawLine,
            Vector3 position,
            Quaternion rotation,
            float radius,
            Color color,
            float duration,
            bool depthTest)
        {
            WireCircleXY(drawLine, position, rotation, radius, color, duration, depthTest);
            WireCircleXZ(drawLine, position, rotation, radius, color, duration, depthTest);
            WireCircleYZ(drawLine, position, rotation, radius, color, duration, depthTest);
        }
    }
}