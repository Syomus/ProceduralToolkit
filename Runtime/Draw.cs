using System;
using UnityEngine;

namespace ProceduralToolkit
{
    /// <summary>
    /// Collection of generic vector and raster drawing algorithms
    /// </summary>
    public static partial class Draw
    {
        public const int circleSegments = 64;
        public const float circleSegmentAngle = 360f/circleSegments;

        public static readonly Func<float, float, Vector3> pointOnCircleXY;
        public static readonly Func<float, float, Vector3> pointOnCircleXZ;
        public static readonly Func<float, float, Vector3> pointOnCircleYZ;

        static Draw()
        {
            pointOnCircleXY = Geometry.PointOnCircle3XY;
            pointOnCircleXZ = Geometry.PointOnCircle3XZ;
            pointOnCircleYZ = Geometry.PointOnCircle3YZ;
        }

        public static void GetSegmentsAndSegmentAngle(float fromAngle, float toAngle, out int segments, out float segmentAngle)
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

        /// <summary>
        /// Draws a line between specified from and to points
        /// </summary>
        public static void Line(Action<Vector3, Vector3> drawLine, Vector3 from, Vector3 to)
        {
            drawLine(from, to);
        }

        /// <summary>
        /// Draws a ray starting at ray.origin to ray.origin + ray.direction
        /// </summary>
        public static void Ray(Action<Vector3, Vector3> drawLine, Ray ray)
        {
            Line(drawLine, ray.origin, ray.origin + ray.direction);
        }

        /// <summary>
        /// Draws a segment
        /// </summary>
        public static void Segment2(Action<Vector3, Vector3> drawLine, Segment2 segment)
        {
            Line(drawLine, (Vector3) segment.a, (Vector3) segment.b);
        }

        /// <summary>
        /// Draws a segment
        /// </summary>
        public static void Segment3(Action<Vector3, Vector3> drawLine, Segment3 segment)
        {
            Line(drawLine, segment.a, segment.b);
        }

        #region WireQuad

        /// <summary>
        /// Draws a wireframe quad with position, rotation and scale
        /// </summary>
        public static void WireQuadXY(Action<Vector3, Vector3> drawLine, Vector3 position, Quaternion rotation, Vector2 scale)
        {
            WireQuad(drawLine, position, rotation, scale, Vector3.right, Vector3.up);
        }

        /// <summary>
        /// Draws a wireframe quad with position, rotation and scale
        /// </summary>
        public static void WireQuadXZ(Action<Vector3, Vector3> drawLine, Vector3 position, Quaternion rotation, Vector2 scale)
        {
            WireQuad(drawLine, position, rotation, scale, Vector3.right, Vector3.forward);
        }

        /// <summary>
        /// Draws a wireframe quad with position, rotation and scale
        /// </summary>
        public static void WireQuadYZ(Action<Vector3, Vector3> drawLine, Vector3 position, Quaternion rotation, Vector2 scale)
        {
            WireQuad(drawLine, position, rotation, scale, Vector3.up, Vector3.forward);
        }

        /// <summary>
        /// Draws a wireframe quad with position, rotation and scale
        /// </summary>
        public static void WireQuad(Action<Vector3, Vector3> drawLine, Vector3 position, Quaternion rotation, Vector2 scale, Vector3 planeRight,
            Vector3 planeForward)
        {
            Vector3 right = rotation*planeRight*scale.x;
            Vector3 forward = rotation*planeForward*scale.y;
            Vector3 forwardRight = position + right*0.5f + forward*0.5f;
            Vector3 backRight = forwardRight - forward;
            Vector3 backLeft = backRight - right;
            Vector3 forwardLeft = forwardRight - right;

            Line(drawLine, forwardRight, backRight);
            Line(drawLine, backRight, backLeft);
            Line(drawLine, backLeft, forwardLeft);
            Line(drawLine, forwardLeft, forwardRight);
        }

        #endregion WireQuad

        /// <summary>
        /// Draws a wireframe cube with position, rotation and scale
        /// </summary>
        public static void WireCube(Action<Vector3, Vector3> drawLine, Vector3 position, Quaternion rotation, Vector3 scale)
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

            Line(drawLine, a1, b1);
            Line(drawLine, b1, c1);
            Line(drawLine, c1, d1);
            Line(drawLine, d1, a1);

            Line(drawLine, a2, b2);
            Line(drawLine, b2, c2);
            Line(drawLine, c2, d2);
            Line(drawLine, d2, a2);

            Line(drawLine, a1, a2);
            Line(drawLine, b1, b2);
            Line(drawLine, c1, c2);
            Line(drawLine, d1, d2);
        }

        #region WireCircle

        /// <summary>
        /// Draws a wireframe circle with position and radius
        /// </summary>
        public static void WireCircleXY(Action<Vector3, Vector3> drawLine, Vector3 position, float radius)
        {
            WireCircle(drawLine, pointOnCircleXY, position, radius);
        }

        /// <summary>
        /// Draws a wireframe circle with position, rotation and radius
        /// </summary>
        public static void WireCircleXY(Action<Vector3, Vector3> drawLine, Vector3 position, Quaternion rotation, float radius)
        {
            WireCircle(drawLine, pointOnCircleXY, position, rotation, radius);
        }

        /// <summary>
        /// Draws a wireframe circle with position and radius
        /// </summary>
        public static void WireCircleXZ(Action<Vector3, Vector3> drawLine, Vector3 position, float radius)
        {
            WireCircle(drawLine, pointOnCircleXZ, position, radius);
        }

        /// <summary>
        /// Draws a wireframe circle with position, rotation and radius
        /// </summary>
        public static void WireCircleXZ(Action<Vector3, Vector3> drawLine, Vector3 position, Quaternion rotation, float radius)
        {
            WireCircle(drawLine, pointOnCircleXZ, position, rotation, radius);
        }

        /// <summary>
        /// Draws a wireframe circle with position and radius
        /// </summary>
        public static void WireCircleYZ(Action<Vector3, Vector3> drawLine, Vector3 position, float radius)
        {
            WireCircle(drawLine, pointOnCircleYZ, position, radius);
        }

        /// <summary>
        /// Draws a wireframe circle with position, rotation and radius
        /// </summary>
        public static void WireCircleYZ(Action<Vector3, Vector3> drawLine, Vector3 position, Quaternion rotation, float radius)
        {
            WireCircle(drawLine, pointOnCircleYZ, position, rotation, radius);
        }

        #endregion WireCircle

        #region WireCircle Universal

        /// <summary>
        /// Draws a wireframe circle with position and radius
        /// </summary>
        public static void WireCircle(Action<Vector3, Vector3> drawLine, Func<float, float, Vector3> pointOnCircle, Vector3 position, float radius)
        {
            WireArc(drawLine, pointOnCircle, position, radius, 0, circleSegments, circleSegmentAngle);
        }

        /// <summary>
        /// Draws a wireframe circle with position, rotation and radius
        /// </summary>
        public static void WireCircle(Action<Vector3, Vector3> drawLine, Func<float, float, Vector3> pointOnCircle, Vector3 position,
            Quaternion rotation, float radius)
        {
            WireArc(drawLine, pointOnCircle, position, rotation, radius, 0, circleSegments, circleSegmentAngle);
        }

        #endregion WireCircle Universal

        #region WireArc

        /// <summary>
        /// Draws a wireframe circular arc with position and radius
        /// </summary>
        public static void WireArcXY(Action<Vector3, Vector3> drawLine, Vector3 position, float radius, float fromAngle, float toAngle)
        {
            WireArc(drawLine, pointOnCircleXY, position, radius, fromAngle, toAngle);
        }

        /// <summary>
        /// Draws a wireframe circular arc with position, rotation and radius
        /// </summary>
        public static void WireArcXY(Action<Vector3, Vector3> drawLine, Vector3 position, Quaternion rotation, float radius, float fromAngle,
            float toAngle)
        {
            WireArc(drawLine, pointOnCircleXY, position, rotation, radius, fromAngle, toAngle);
        }

        /// <summary>
        /// Draws a wireframe circular arc with position and radius
        /// </summary>
        public static void WireArcXZ(Action<Vector3, Vector3> drawLine, Vector3 position, float radius, float fromAngle, float toAngle)
        {
            WireArc(drawLine, pointOnCircleXZ, position, radius, fromAngle, toAngle);
        }

        /// <summary>
        /// Draws a wireframe circular arc with position, rotation and radius
        /// </summary>
        public static void WireArcXZ(Action<Vector3, Vector3> drawLine, Vector3 position, Quaternion rotation, float radius, float fromAngle,
            float toAngle)
        {
            WireArc(drawLine, pointOnCircleXZ, position, rotation, radius, fromAngle, toAngle);
        }

        /// <summary>
        /// Draws a wireframe circular arc with position and radius
        /// </summary>
        public static void WireArcYZ(Action<Vector3, Vector3> drawLine, Vector3 position, float radius, float fromAngle, float toAngle)
        {
            WireArc(drawLine, pointOnCircleYZ, position, radius, fromAngle, toAngle);
        }

        /// <summary>
        /// Draws a wireframe circular arc with position, rotation and radius
        /// </summary>
        public static void WireArcYZ(Action<Vector3, Vector3> drawLine, Vector3 position, Quaternion rotation, float radius, float fromAngle,
            float toAngle)
        {
            WireArc(drawLine, pointOnCircleYZ, position, rotation, radius, fromAngle, toAngle);
        }

        #endregion WireArc

        #region WireArc Universal

        /// <summary>
        /// Draws a wireframe circular arc with position and radius
        /// </summary>
        public static void WireArc(Action<Vector3, Vector3> drawLine, Func<float, float, Vector3> pointOnCircle, Vector3 position, float radius,
            float fromAngle, float toAngle)
        {
            GetSegmentsAndSegmentAngle(fromAngle, toAngle, out int segments, out float segmentAngle);
            WireArc(drawLine, pointOnCircle, position, radius, fromAngle, segments, segmentAngle);
        }

        /// <summary>
        /// Draws a wireframe circular arc with position, rotation and radius
        /// </summary>
        public static void WireArc(Action<Vector3, Vector3> drawLine, Func<float, float, Vector3> pointOnCircle, Vector3 position,
            Quaternion rotation, float radius, float fromAngle, float toAngle)
        {
            GetSegmentsAndSegmentAngle(fromAngle, toAngle, out int segments, out float segmentAngle);
            WireArc(drawLine, pointOnCircle, position, rotation, radius, fromAngle, segments, segmentAngle);
        }

        /// <summary>
        /// Draws a wireframe circular arc with position and radius
        /// </summary>
        public static void WireArc(Action<Vector3, Vector3> drawLine, Func<float, float, Vector3> pointOnCircle, Vector3 position, float radius,
            float fromAngle, int segments, float segmentAngle)
        {
            float currentAngle = fromAngle;
            for (var i = 0; i < segments; i++)
            {
                Vector3 a = position + pointOnCircle(radius, currentAngle);
                currentAngle += segmentAngle;
                Vector3 b = position + pointOnCircle(radius, currentAngle);
                Line(drawLine, a, b);
            }
        }

        /// <summary>
        /// Draws a wireframe circular arc with position, rotation and radius
        /// </summary>
        public static void WireArc(Action<Vector3, Vector3> drawLine, Func<float, float, Vector3> pointOnCircle, Vector3 position,
            Quaternion rotation, float radius, float fromAngle, int segments, float segmentAngle)
        {
            float currentAngle = fromAngle;
            for (var i = 0; i < segments; i++)
            {
                Vector3 a = position + rotation*pointOnCircle(radius, currentAngle);
                currentAngle += segmentAngle;
                Vector3 b = position + rotation*pointOnCircle(radius, currentAngle);
                Line(drawLine, a, b);
            }
        }

        #endregion WireArc Universal

        /// <summary>
        /// Draws a wireframe sphere with position, rotation and radius
        /// </summary>
        public static void WireSphere(Action<Vector3, Vector3> drawLine, Vector3 position, Quaternion rotation, float radius)
        {
            WireCircleXY(drawLine, position, rotation, radius);
            WireCircleXZ(drawLine, position, rotation, radius);
            WireCircleYZ(drawLine, position, rotation, radius);
        }

        /// <summary>
        /// Draws a wireframe hemisphere with position, rotation and radius
        /// </summary>
        public static void WireHemisphere(Action<Vector3, Vector3> drawLine, Vector3 position, Quaternion rotation, float radius)
        {
            WireArcXY(drawLine, position, rotation, radius, -90, 90);
            WireCircleXZ(drawLine, position, rotation, radius);
            WireArcYZ(drawLine, position, rotation, radius, 0, 180);
        }

        /// <summary>
        /// Draws a wireframe cone with position and rotation
        /// </summary>
        public static void WireCone(Action<Vector3, Vector3> drawLine, Vector3 position, Quaternion rotation, float apexRadius, float angle,
            float length)
        {
            Vector3 baseCenter = position + rotation*Vector3.up*length;
            float baseRadius = Mathf.Tan(angle*Mathf.Deg2Rad)*length + apexRadius;
            WireCircleXZ(drawLine, baseCenter, rotation, baseRadius);

            Vector3 a2 = baseCenter + rotation*Geometry.PointOnCircle3XZ(baseRadius, 0);
            Vector3 b2 = baseCenter + rotation*Geometry.PointOnCircle3XZ(baseRadius, 90);
            Vector3 c2 = baseCenter + rotation*Geometry.PointOnCircle3XZ(baseRadius, 180);
            Vector3 d2 = baseCenter + rotation*Geometry.PointOnCircle3XZ(baseRadius, 270);

            if (apexRadius == 0)
            {
                Line(drawLine, position, a2);
                Line(drawLine, position, b2);
                Line(drawLine, position, c2);
                Line(drawLine, position, d2);
            }
            else
            {
                WireCircleXZ(drawLine, position, rotation, apexRadius);

                Vector3 a1 = position + rotation*Geometry.PointOnCircle3XZ(apexRadius, 0);
                Vector3 b1 = position + rotation*Geometry.PointOnCircle3XZ(apexRadius, 90);
                Vector3 c1 = position + rotation*Geometry.PointOnCircle3XZ(apexRadius, 180);
                Vector3 d1 = position + rotation*Geometry.PointOnCircle3XZ(apexRadius, 270);

                Line(drawLine, a1, a2);
                Line(drawLine, b1, b2);
                Line(drawLine, c1, c2);
                Line(drawLine, d1, d2);
            }
        }
    }
}
