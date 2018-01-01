using System;
using UnityEngine;

namespace ProceduralToolkit
{
    /// <summary>
    /// Collection of generic vector and raster drawing algorithms
    /// </summary>
    public static partial class Draw
    {
        public delegate void DebugDrawLine(Vector3 start, Vector3 end, Color color, float duration, bool depthTest);

        private const int circleSegments = 64;
        private const float circleSegmentAngle = 360f/circleSegments;

        private static readonly Func<float, float, Vector3> pointOnCircleXY;
        private static readonly Func<float, float, Vector3> pointOnCircleXZ;
        private static readonly Func<float, float, Vector3> pointOnCircleYZ;

        static Draw()
        {
            pointOnCircleXY = PTUtils.PointOnCircle3XY;
            pointOnCircleXZ = PTUtils.PointOnCircle3XZ;
            pointOnCircleYZ = PTUtils.PointOnCircle3YZ;
        }

        private static void GetSegmentsAndSegmentAngle(float fromAngle, float toAngle, out int segments,
            out float segmentAngle)
        {
            float range = toAngle - fromAngle;
            if (range > circleSegmentAngle)
            {
                segments = Mathf.FloorToInt(range/circleSegmentAngle);
                segmentAngle = range/segments;
            }
            else
            {
                segments = 1;
                segmentAngle = range;
            }
        }

        #region WireRay

        public static void WireRay(Action<Vector3, Vector3> drawLine, Ray ray)
        {
            drawLine(ray.origin, ray.origin + ray.direction);
        }

        public static void WireRay(DebugDrawLine drawLine, Ray ray, Color color, float duration, bool depthTest)
        {
            drawLine(ray.origin, ray.origin + ray.direction, color, duration, depthTest);
        }

        #endregion WireRay

        #region WireQuad

        public static void WireQuadXY(
            Action<Vector3, Vector3> drawLine,
            Vector3 position,
            Quaternion rotation,
            Vector2 scale)
        {
            WireQuad(drawLine, position, rotation, scale, Vector3.right, Vector3.up);
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
            WireQuad(drawLine, position, rotation, scale, Vector3.right, Vector3.up, color, duration, depthTest);
        }

        public static void WireQuadXZ(
            Action<Vector3, Vector3> drawLine,
            Vector3 position,
            Quaternion rotation,
            Vector2 scale)
        {
            WireQuad(drawLine, position, rotation, scale, Vector3.right, Vector3.forward);
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
            WireQuad(drawLine, position, rotation, scale, Vector3.right, Vector3.forward, color, duration, depthTest);
        }

        public static void WireQuadYZ(
            Action<Vector3, Vector3> drawLine,
            Vector3 position,
            Quaternion rotation,
            Vector2 scale)
        {
            WireQuad(drawLine, position, rotation, scale, Vector3.up, Vector3.forward);
        }

        public static void WireQuadYZ(
            DebugDrawLine drawLine,
            Vector3 position,
            Quaternion rotation,
            Vector2 scale,
            Color color,
            float duration,
            bool depthTest)
        {
            WireQuad(drawLine, position, rotation, scale, Vector3.up, Vector3.forward, color, duration, depthTest);
        }

        public static void WireQuad(
            Action<Vector3, Vector3> drawLine,
            Vector3 position,
            Quaternion rotation,
            Vector2 scale,
            Vector3 planeRight,
            Vector3 planeForward)
        {
            Vector3 right = rotation*planeRight*scale.x;
            Vector3 forward = rotation*planeForward*scale.y;
            Vector3 forwardRight = position + right*0.5f + forward*0.5f;
            Vector3 backRight = forwardRight - forward;
            Vector3 backLeft = backRight - right;
            Vector3 forwardLeft = forwardRight - right;

            drawLine(forwardRight, backRight);
            drawLine(backRight, backLeft);
            drawLine(backLeft, forwardLeft);
            drawLine(forwardLeft, forwardRight);
        }

        public static void WireQuad(
            DebugDrawLine drawLine,
            Vector3 position,
            Quaternion rotation,
            Vector2 scale,
            Vector3 planeRight,
            Vector3 planeForward,
            Color color,
            float duration,
            bool depthTest)
        {
            Vector3 right = rotation*planeRight*scale.x;
            Vector3 forward = rotation*planeForward*scale.y;
            Vector3 forwardRight = position + right*0.5f + forward*0.5f;
            Vector3 backRight = forwardRight - forward;
            Vector3 backLeft = backRight - right;
            Vector3 forwardLeft = forwardRight - right;

            drawLine(forwardRight, backRight, color, duration, depthTest);
            drawLine(backRight, backLeft, color, duration, depthTest);
            drawLine(backLeft, forwardLeft, color, duration, depthTest);
            drawLine(forwardLeft, forwardRight, color, duration, depthTest);
        }

        #endregion WireQuad

        #region WireCube

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

        #endregion WireCube

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
            WireCircle(pointOnCircleXZ, drawLine, position, rotation, radius, color, duration, depthTest);
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

        #region WireCircle Universal

        public static void WireCircle(
            Func<float, float, Vector3> pointOnCircle,
            Action<Vector3, Vector3> drawLine,
            Vector3 position,
            float radius)
        {
            WireArc(pointOnCircle, drawLine, position, radius, 0, circleSegments, circleSegmentAngle);
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
            WireArc(pointOnCircle, drawLine, position, radius, 0, circleSegments, circleSegmentAngle, color, duration,
                depthTest);
        }

        public static void WireCircle(
            Func<float, float, Vector3> pointOnCircle,
            Action<Vector3, Vector3> drawLine,
            Vector3 position,
            Quaternion rotation,
            float radius)
        {
            WireArc(pointOnCircle, drawLine, position, rotation, radius, 0, circleSegments, circleSegmentAngle);
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
            WireArc(pointOnCircle, drawLine, position, rotation, radius, 0, circleSegments, circleSegmentAngle, color,
                duration, depthTest);
        }

        #endregion WireCircle Universal

        #region WireArcXY

        public static void WireArcXY(
            Action<Vector3, Vector3> drawLine,
            Vector3 position,
            float radius,
            float fromAngle,
            float toAngle)
        {
            WireArc(pointOnCircleXY, drawLine, position, radius, fromAngle, toAngle);
        }

        public static void WireArcXY(
            DebugDrawLine drawLine,
            Vector3 position,
            float radius,
            float fromAngle,
            float toAngle,
            Color color,
            float duration,
            bool depthTest)
        {
            WireArc(pointOnCircleXY, drawLine, position, radius, fromAngle, toAngle, color, duration, depthTest);
        }

        public static void WireArcXY(
            Action<Vector3, Vector3> drawLine,
            Vector3 position,
            Quaternion rotation,
            float radius,
            float fromAngle,
            float toAngle)
        {
            WireArc(pointOnCircleXY, drawLine, position, rotation, radius, fromAngle, toAngle);
        }

        public static void WireArcXY(
            DebugDrawLine drawLine,
            Vector3 position,
            Quaternion rotation,
            float radius,
            float fromAngle,
            float toAngle,
            Color color,
            float duration,
            bool depthTest)
        {
            WireArc(pointOnCircleXY, drawLine, position, rotation, radius, fromAngle, toAngle, color, duration,
                depthTest);
        }

        #endregion WireCircleXY

        #region WireArcXZ

        public static void WireArcXZ(
            Action<Vector3, Vector3> drawLine,
            Vector3 position,
            float radius,
            float fromAngle,
            float toAngle)
        {
            WireArc(pointOnCircleXZ, drawLine, position, radius, fromAngle, toAngle);
        }

        public static void WireArcXZ(
            DebugDrawLine drawLine,
            Vector3 position,
            float radius,
            float fromAngle,
            float toAngle,
            Color color,
            float duration,
            bool depthTest)
        {
            WireArc(pointOnCircleXZ, drawLine, position, radius, fromAngle, toAngle, color, duration, depthTest);
        }

        public static void WireArcXZ(
            Action<Vector3, Vector3> drawLine,
            Vector3 position,
            Quaternion rotation,
            float radius,
            float fromAngle,
            float toAngle)
        {
            WireArc(pointOnCircleXZ, drawLine, position, rotation, radius, fromAngle, toAngle);
        }

        public static void WireArcXZ(
            DebugDrawLine drawLine,
            Vector3 position,
            Quaternion rotation,
            float radius,
            float fromAngle,
            float toAngle,
            Color color,
            float duration,
            bool depthTest)
        {
            WireArc(pointOnCircleXZ, drawLine, position, rotation, radius, fromAngle, toAngle, color, duration,
                depthTest);
        }

        #endregion WireCircleXZ

        #region WireArcYZ

        public static void WireArcYZ(
            Action<Vector3, Vector3> drawLine,
            Vector3 position,
            float radius,
            float fromAngle,
            float toAngle)
        {
            WireArc(pointOnCircleYZ, drawLine, position, radius, fromAngle, toAngle);
        }

        public static void WireArcYZ(
            DebugDrawLine drawLine,
            Vector3 position,
            float radius,
            float fromAngle,
            float toAngle,
            Color color,
            float duration,
            bool depthTest)
        {
            WireArc(pointOnCircleYZ, drawLine, position, radius, fromAngle, toAngle, color, duration, depthTest);
        }

        public static void WireArcYZ(
            Action<Vector3, Vector3> drawLine,
            Vector3 position,
            Quaternion rotation,
            float radius,
            float fromAngle,
            float toAngle)
        {
            WireArc(pointOnCircleYZ, drawLine, position, rotation, radius, fromAngle, toAngle);
        }

        public static void WireArcYZ(
            DebugDrawLine drawLine,
            Vector3 position,
            Quaternion rotation,
            float radius,
            float fromAngle,
            float toAngle,
            Color color,
            float duration,
            bool depthTest)
        {
            WireArc(pointOnCircleYZ, drawLine, position, rotation, radius, fromAngle, toAngle, color, duration,
                depthTest);
        }

        #endregion WireCircleYZ

        #region WireArc Universal

        public static void WireArc(
            Func<float, float, Vector3> pointOnCircle,
            Action<Vector3, Vector3> drawLine,
            Vector3 position,
            float radius,
            float fromAngle,
            float toAngle)
        {
            int segments;
            float segmentAngle;
            GetSegmentsAndSegmentAngle(fromAngle, toAngle, out segments, out segmentAngle);

            WireArc(pointOnCircle, drawLine, position, radius, fromAngle, segments, segmentAngle);
        }

        public static void WireArc(
            Func<float, float, Vector3> pointOnCircle,
            DebugDrawLine drawLine,
            Vector3 position,
            float radius,
            float fromAngle,
            float toAngle,
            Color color,
            float duration,
            bool depthTest)
        {
            int segments;
            float segmentAngle;
            GetSegmentsAndSegmentAngle(fromAngle, toAngle, out segments, out segmentAngle);

            WireArc(pointOnCircle, drawLine, position, radius, fromAngle, segments, segmentAngle, color, duration,
                depthTest);
        }

        public static void WireArc(
            Func<float, float, Vector3> pointOnCircle,
            Action<Vector3, Vector3> drawLine,
            Vector3 position,
            Quaternion rotation,
            float radius,
            float fromAngle,
            float toAngle)
        {
            int segments;
            float segmentAngle;
            GetSegmentsAndSegmentAngle(fromAngle, toAngle, out segments, out segmentAngle);

            WireArc(pointOnCircle, drawLine, position, rotation, radius, fromAngle, segments, segmentAngle);
        }

        public static void WireArc(
            Func<float, float, Vector3> pointOnCircle,
            DebugDrawLine drawLine,
            Vector3 position,
            Quaternion rotation,
            float radius,
            float fromAngle,
            float toAngle,
            Color color,
            float duration,
            bool depthTest)
        {
            int segments;
            float segmentAngle;
            GetSegmentsAndSegmentAngle(fromAngle, toAngle, out segments, out segmentAngle);

            WireArc(pointOnCircle, drawLine, position, rotation, radius, fromAngle, segments, segmentAngle, color,
                duration, depthTest);
        }

        public static void WireArc(
            Func<float, float, Vector3> pointOnCircle,
            Action<Vector3, Vector3> drawLine,
            Vector3 position,
            float radius,
            float fromAngle,
            int segments,
            float segmentAngle)
        {
            float currentAngle = fromAngle;
            for (var i = 0; i < segments; i++)
            {
                Vector3 a = position + pointOnCircle(radius, currentAngle);
                currentAngle += segmentAngle;
                Vector3 b = position + pointOnCircle(radius, currentAngle);
                drawLine(a, b);
            }
        }

        public static void WireArc(
            Func<float, float, Vector3> pointOnCircle,
            DebugDrawLine drawLine,
            Vector3 position,
            float radius,
            float fromAngle,
            int segments,
            float segmentAngle,
            Color color,
            float duration,
            bool depthTest)
        {
            float currentAngle = fromAngle;
            for (var i = 0; i < segments; i++)
            {
                Vector3 a = position + pointOnCircle(radius, currentAngle);
                currentAngle += segmentAngle;
                Vector3 b = position + pointOnCircle(radius, currentAngle);
                drawLine(a, b, color, duration, depthTest);
            }
        }

        public static void WireArc(
            Func<float, float, Vector3> pointOnCircle,
            Action<Vector3, Vector3> drawLine,
            Vector3 position,
            Quaternion rotation,
            float radius,
            float fromAngle,
            int segments,
            float segmentAngle)
        {
            float currentAngle = fromAngle;
            for (var i = 0; i < segments; i++)
            {
                Vector3 a = position + rotation*pointOnCircle(radius, currentAngle);
                currentAngle += segmentAngle;
                Vector3 b = position + rotation*pointOnCircle(radius, currentAngle);
                drawLine(a, b);
            }
        }

        public static void WireArc(
            Func<float, float, Vector3> pointOnCircle,
            DebugDrawLine drawLine,
            Vector3 position,
            Quaternion rotation,
            float radius,
            float fromAngle,
            int segments,
            float segmentAngle,
            Color color,
            float duration,
            bool depthTest)
        {
            float currentAngle = fromAngle;
            for (var i = 0; i < segments; i++)
            {
                Vector3 a = position + rotation*pointOnCircle(radius, currentAngle);
                currentAngle += segmentAngle;
                Vector3 b = position + rotation*pointOnCircle(radius, currentAngle);
                drawLine(a, b, color, duration, depthTest);
            }
        }

        #endregion WireArc Universal

        #region WireSphere

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

        #endregion WireSphere

        #region WireHemisphere

        public static void WireHemisphere(
            Action<Vector3, Vector3> drawLine,
            Vector3 position,
            Quaternion rotation,
            float radius)
        {
            WireArcXY(drawLine, position, rotation, radius, -90, 90);
            WireCircleXZ(drawLine, position, rotation, radius);
            WireArcYZ(drawLine, position, rotation, radius, 0, 180);
        }

        public static void WireHemisphere(
            DebugDrawLine drawLine,
            Vector3 position,
            Quaternion rotation,
            float radius,
            Color color,
            float duration,
            bool depthTest)
        {
            WireArcXY(drawLine, position, rotation, radius, -90, 90, color, duration, depthTest);
            WireCircleXZ(drawLine, position, rotation, radius, color, duration, depthTest);
            WireArcYZ(drawLine, position, rotation, radius, 0, 180, color, duration, depthTest);
        }

        #endregion WireHemisphere

        #region WireCone

        public static void WireCone(
            Action<Vector3, Vector3> drawLine,
            Vector3 position,
            Quaternion rotation,
            float apexRadius,
            float angle,
            float length)
        {
            Vector3 baseCenter = position + rotation*Vector3.up*length;
            float baseRadius = Mathf.Tan(angle*Mathf.Deg2Rad)*length + apexRadius;
            WireCircleXZ(drawLine, baseCenter, rotation, baseRadius);

            Vector3 a2 = baseCenter + rotation*PTUtils.PointOnCircle3XZ(baseRadius, 0);
            Vector3 b2 = baseCenter + rotation*PTUtils.PointOnCircle3XZ(baseRadius, 90);
            Vector3 c2 = baseCenter + rotation*PTUtils.PointOnCircle3XZ(baseRadius, 180);
            Vector3 d2 = baseCenter + rotation*PTUtils.PointOnCircle3XZ(baseRadius, 270);

            if (apexRadius == 0)
            {
                drawLine(position, a2);
                drawLine(position, b2);
                drawLine(position, c2);
                drawLine(position, d2);
            }
            else
            {
                WireCircleXZ(drawLine, position, rotation, apexRadius);

                Vector3 a1 = position + rotation*PTUtils.PointOnCircle3XZ(apexRadius, 0);
                Vector3 b1 = position + rotation*PTUtils.PointOnCircle3XZ(apexRadius, 90);
                Vector3 c1 = position + rotation*PTUtils.PointOnCircle3XZ(apexRadius, 180);
                Vector3 d1 = position + rotation*PTUtils.PointOnCircle3XZ(apexRadius, 270);

                drawLine(a1, a2);
                drawLine(b1, b2);
                drawLine(c1, c2);
                drawLine(d1, d2);
            }
        }

        public static void WireCone(
            DebugDrawLine drawLine,
            Vector3 position,
            Quaternion rotation,
            float apexRadius,
            float angle,
            float length,
            Color color,
            float duration,
            bool depthTest)
        {
            Vector3 upperCenter = position + rotation*Vector3.up*length;
            float upperRadius = Mathf.Tan(angle*Mathf.Deg2Rad)*length + apexRadius;
            WireCircleXZ(drawLine, upperCenter, rotation, upperRadius, color, duration, depthTest);

            Vector3 a2 = upperCenter + rotation*PTUtils.PointOnCircle3XZ(upperRadius, 0);
            Vector3 b2 = upperCenter + rotation*PTUtils.PointOnCircle3XZ(upperRadius, 90);
            Vector3 c2 = upperCenter + rotation*PTUtils.PointOnCircle3XZ(upperRadius, 180);
            Vector3 d2 = upperCenter + rotation*PTUtils.PointOnCircle3XZ(upperRadius, 270);

            if (apexRadius == 0)
            {
                drawLine(position, a2, color, duration, depthTest);
                drawLine(position, b2, color, duration, depthTest);
                drawLine(position, c2, color, duration, depthTest);
                drawLine(position, d2, color, duration, depthTest);
            }
            else
            {
                WireCircleXZ(drawLine, position, rotation, apexRadius, color, duration, depthTest);

                Vector3 a1 = position + rotation*PTUtils.PointOnCircle3XZ(apexRadius, 0);
                Vector3 b1 = position + rotation*PTUtils.PointOnCircle3XZ(apexRadius, 90);
                Vector3 c1 = position + rotation*PTUtils.PointOnCircle3XZ(apexRadius, 180);
                Vector3 d1 = position + rotation*PTUtils.PointOnCircle3XZ(apexRadius, 270);

                drawLine(a1, a2, color, duration, depthTest);
                drawLine(b1, b2, color, duration, depthTest);
                drawLine(c1, c2, color, duration, depthTest);
                drawLine(d1, d2, color, duration, depthTest);
            }
        }

        #endregion WireCone
    }
}
