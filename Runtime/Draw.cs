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
        public static void Line(Vector3 from, Vector3 to, Action<Vector3, Vector3> drawLine)
        {
            drawLine(from, to);
        }

        /// <summary>
        /// Draws a ray starting at ray.origin to ray.origin + ray.direction
        /// </summary>
        public static void Ray(Ray ray, Action<Vector3, Vector3> drawLine)
        {
            Line(ray.origin, ray.origin + ray.direction, drawLine);
        }

        /// <summary>
        /// Draws a segment
        /// </summary>
        public static void Segment2(Segment2 segment, Action<Vector3, Vector3> drawLine)
        {
            Line((Vector3) segment.a, (Vector3) segment.b, drawLine);
        }

        /// <summary>
        /// Draws a segment
        /// </summary>
        public static void Segment3(Segment3 segment, Action<Vector3, Vector3> drawLine)
        {
            Line(segment.a, segment.b, drawLine);
        }

        #region WireQuad

        /// <summary>
        /// Draws a wireframe quad with position, rotation and scale
        /// </summary>
        public static void WireQuadXY(Vector3 position, Quaternion rotation, Vector2 scale, Action<Vector3, Vector3> drawLine)
        {
            WireQuad(position, rotation, scale, Vector3.right, Vector3.up, drawLine);
        }

        /// <summary>
        /// Draws a wireframe quad with position, rotation and scale
        /// </summary>
        public static void WireQuadXZ(Vector3 position, Quaternion rotation, Vector2 scale, Action<Vector3, Vector3> drawLine)
        {
            WireQuad(position, rotation, scale, Vector3.right, Vector3.forward, drawLine);
        }

        /// <summary>
        /// Draws a wireframe quad with position, rotation and scale
        /// </summary>
        public static void WireQuadYZ(Vector3 position, Quaternion rotation, Vector2 scale, Action<Vector3, Vector3> drawLine)
        {
            WireQuad(position, rotation, scale, Vector3.up, Vector3.forward, drawLine);
        }

        /// <summary>
        /// Draws a wireframe quad with position, rotation and scale
        /// </summary>
        public static void WireQuad(Vector3 position, Quaternion rotation, Vector2 scale, Vector3 planeRight, Vector3 planeForward,
            Action<Vector3, Vector3> drawLine)
        {
            Vector3 right = rotation*planeRight*scale.x;
            Vector3 forward = rotation*planeForward*scale.y;
            Vector3 forwardRight = position + right*0.5f + forward*0.5f;
            Vector3 backRight = forwardRight - forward;
            Vector3 backLeft = backRight - right;
            Vector3 forwardLeft = forwardRight - right;

            Line(forwardRight, backRight, drawLine);
            Line(backRight, backLeft, drawLine);
            Line(backLeft, forwardLeft, drawLine);
            Line(forwardLeft, forwardRight, drawLine);
        }

        #endregion WireQuad

        /// <summary>
        /// Draws a wireframe cube with position, rotation and scale
        /// </summary>
        public static void WireCube(Vector3 position, Quaternion rotation, Vector3 scale, Action<Vector3, Vector3> drawLine)
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

            Line(a1, b1, drawLine);
            Line(b1, c1, drawLine);
            Line(c1, d1, drawLine);
            Line(d1, a1, drawLine);

            Line(a2, b2, drawLine);
            Line(b2, c2, drawLine);
            Line(c2, d2, drawLine);
            Line(d2, a2, drawLine);

            Line(a1, a2, drawLine);
            Line(b1, b2, drawLine);
            Line(c1, c2, drawLine);
            Line(d1, d2, drawLine);
        }

        #region WireCircle

        /// <summary>
        /// Draws a wireframe circle with position and radius
        /// </summary>
        public static void WireCircleXY(Vector3 position, float radius, Action<Vector3, Vector3> drawLine)
        {
            WireCircle(pointOnCircleXY, position, radius, drawLine);
        }

        /// <summary>
        /// Draws a wireframe circle with position, rotation and radius
        /// </summary>
        public static void WireCircleXY(Vector3 position, Quaternion rotation, float radius, Action<Vector3, Vector3> drawLine)
        {
            WireCircle(pointOnCircleXY, position, rotation, radius, drawLine);
        }

        /// <summary>
        /// Draws a wireframe circle with position and radius
        /// </summary>
        public static void WireCircleXZ(Vector3 position, float radius, Action<Vector3, Vector3> drawLine)
        {
            WireCircle(pointOnCircleXZ, position, radius, drawLine);
        }

        /// <summary>
        /// Draws a wireframe circle with position, rotation and radius
        /// </summary>
        public static void WireCircleXZ(Vector3 position, Quaternion rotation, float radius, Action<Vector3, Vector3> drawLine)
        {
            WireCircle(pointOnCircleXZ, position, rotation, radius, drawLine);
        }

        /// <summary>
        /// Draws a wireframe circle with position and radius
        /// </summary>
        public static void WireCircleYZ(Vector3 position, float radius, Action<Vector3, Vector3> drawLine)
        {
            WireCircle(pointOnCircleYZ, position, radius, drawLine);
        }

        /// <summary>
        /// Draws a wireframe circle with position, rotation and radius
        /// </summary>
        public static void WireCircleYZ(Vector3 position, Quaternion rotation, float radius, Action<Vector3, Vector3> drawLine)
        {
            WireCircle(pointOnCircleYZ, position, rotation, radius, drawLine);
        }

        #endregion WireCircle

        #region WireCircle Universal

        /// <summary>
        /// Draws a wireframe circle with position and radius
        /// </summary>
        public static void WireCircle(Func<float, float, Vector3> pointOnCircle, Vector3 position, float radius, Action<Vector3, Vector3> drawLine)
        {
            WireArc(pointOnCircle, position, radius, 0, circleSegments, circleSegmentAngle, drawLine);
        }

        /// <summary>
        /// Draws a wireframe circle with position, rotation and radius
        /// </summary>
        public static void WireCircle(Func<float, float, Vector3> pointOnCircle, Vector3 position, Quaternion rotation, float radius,
            Action<Vector3, Vector3> drawLine)
        {
            WireArc(pointOnCircle, position, rotation, radius, 0, circleSegments, circleSegmentAngle, drawLine);
        }

        #endregion WireCircle Universal

        #region WireArc

        /// <summary>
        /// Draws a wireframe circular arc with position and radius
        /// </summary>
        public static void WireArcXY(Vector3 position, float radius, float fromAngle, float toAngle, Action<Vector3, Vector3> drawLine)
        {
            WireArc(pointOnCircleXY, position, radius, fromAngle, toAngle, drawLine);
        }

        /// <summary>
        /// Draws a wireframe circular arc with position, rotation and radius
        /// </summary>
        public static void WireArcXY(Vector3 position, Quaternion rotation, float radius, float fromAngle, float toAngle,
            Action<Vector3, Vector3> drawLine)
        {
            WireArc(pointOnCircleXY, position, rotation, radius, fromAngle, toAngle, drawLine);
        }

        /// <summary>
        /// Draws a wireframe circular arc with position and radius
        /// </summary>
        public static void WireArcXZ(Vector3 position, float radius, float fromAngle, float toAngle, Action<Vector3, Vector3> drawLine)
        {
            WireArc(pointOnCircleXZ, position, radius, fromAngle, toAngle, drawLine);
        }

        /// <summary>
        /// Draws a wireframe circular arc with position, rotation and radius
        /// </summary>
        public static void WireArcXZ(Vector3 position, Quaternion rotation, float radius, float fromAngle, float toAngle,
            Action<Vector3, Vector3> drawLine)
        {
            WireArc(pointOnCircleXZ, position, rotation, radius, fromAngle, toAngle, drawLine);
        }

        /// <summary>
        /// Draws a wireframe circular arc with position and radius
        /// </summary>
        public static void WireArcYZ(Vector3 position, float radius, float fromAngle, float toAngle, Action<Vector3, Vector3> drawLine)
        {
            WireArc(pointOnCircleYZ, position, radius, fromAngle, toAngle, drawLine);
        }

        /// <summary>
        /// Draws a wireframe circular arc with position, rotation and radius
        /// </summary>
        public static void WireArcYZ(Vector3 position, Quaternion rotation, float radius, float fromAngle, float toAngle,
            Action<Vector3, Vector3> drawLine)
        {
            WireArc(pointOnCircleYZ, position, rotation, radius, fromAngle, toAngle, drawLine);
        }

        #endregion WireArc

        #region WireArc Universal

        /// <summary>
        /// Draws a wireframe circular arc with position and radius
        /// </summary>
        public static void WireArc(Func<float, float, Vector3> pointOnCircle, Vector3 position, float radius, float fromAngle, float toAngle,
            Action<Vector3, Vector3> drawLine)
        {
            GetSegmentsAndSegmentAngle(fromAngle, toAngle, out int segments, out float segmentAngle);
            WireArc(pointOnCircle, position, radius, fromAngle, segments, segmentAngle, drawLine);
        }

        /// <summary>
        /// Draws a wireframe circular arc with position, rotation and radius
        /// </summary>
        public static void WireArc(Func<float, float, Vector3> pointOnCircle, Vector3 position, Quaternion rotation, float radius, float fromAngle,
            float toAngle, Action<Vector3, Vector3> drawLine)
        {
            GetSegmentsAndSegmentAngle(fromAngle, toAngle, out int segments, out float segmentAngle);
            WireArc(pointOnCircle, position, rotation, radius, fromAngle, segments, segmentAngle, drawLine);
        }

        /// <summary>
        /// Draws a wireframe circular arc with position and radius
        /// </summary>
        public static void WireArc(Func<float, float, Vector3> pointOnCircle, Vector3 position, float radius, float fromAngle, int segments,
            float segmentAngle, Action<Vector3, Vector3> drawLine)
        {
            float currentAngle = fromAngle;
            for (var i = 0; i < segments; i++)
            {
                Vector3 a = position + pointOnCircle(radius, currentAngle);
                currentAngle += segmentAngle;
                Vector3 b = position + pointOnCircle(radius, currentAngle);
                Line(a, b, drawLine);
            }
        }

        /// <summary>
        /// Draws a wireframe circular arc with position, rotation and radius
        /// </summary>
        public static void WireArc(Func<float, float, Vector3> pointOnCircle, Vector3 position, Quaternion rotation, float radius, float fromAngle,
            int segments, float segmentAngle, Action<Vector3, Vector3> drawLine)
        {
            float currentAngle = fromAngle;
            for (var i = 0; i < segments; i++)
            {
                Vector3 a = position + rotation*pointOnCircle(radius, currentAngle);
                currentAngle += segmentAngle;
                Vector3 b = position + rotation*pointOnCircle(radius, currentAngle);
                Line(a, b, drawLine);
            }
        }

        #endregion WireArc Universal

        /// <summary>
        /// Draws a wireframe sphere with position, rotation and radius
        /// </summary>
        public static void WireSphere(Vector3 position, Quaternion rotation, float radius, Action<Vector3, Vector3> drawLine)
        {
            WireCircleXY(position, rotation, radius, drawLine);
            WireCircleXZ(position, rotation, radius, drawLine);
            WireCircleYZ(position, rotation, radius, drawLine);
        }

        /// <summary>
        /// Draws a wireframe hemisphere with position, rotation and radius
        /// </summary>
        public static void WireHemisphere(Vector3 position, Quaternion rotation, float radius, Action<Vector3, Vector3> drawLine)
        {
            WireArcXY(position, rotation, radius, -90, 90, drawLine);
            WireCircleXZ(position, rotation, radius, drawLine);
            WireArcYZ(position, rotation, radius, 0, 180, drawLine);
        }

        /// <summary>
        /// Draws a wireframe cone with position and rotation
        /// </summary>
        public static void WireCone(Vector3 position, Quaternion rotation, float apexRadius, float angle, float length,
            Action<Vector3, Vector3> drawLine)
        {
            Vector3 baseCenter = position + rotation*Vector3.up*length;
            float baseRadius = Mathf.Tan(angle*Mathf.Deg2Rad)*length + apexRadius;
            WireCircleXZ(baseCenter, rotation, baseRadius, drawLine);

            Vector3 a2 = baseCenter + rotation*Geometry.PointOnCircle3XZ(baseRadius, 0);
            Vector3 b2 = baseCenter + rotation*Geometry.PointOnCircle3XZ(baseRadius, 90);
            Vector3 c2 = baseCenter + rotation*Geometry.PointOnCircle3XZ(baseRadius, 180);
            Vector3 d2 = baseCenter + rotation*Geometry.PointOnCircle3XZ(baseRadius, 270);

            if (apexRadius == 0)
            {
                Line(position, a2, drawLine);
                Line(position, b2, drawLine);
                Line(position, c2, drawLine);
                Line(position, d2, drawLine);
            }
            else
            {
                WireCircleXZ(position, rotation, apexRadius, drawLine);

                Vector3 a1 = position + rotation*Geometry.PointOnCircle3XZ(apexRadius, 0);
                Vector3 b1 = position + rotation*Geometry.PointOnCircle3XZ(apexRadius, 90);
                Vector3 c1 = position + rotation*Geometry.PointOnCircle3XZ(apexRadius, 180);
                Vector3 d1 = position + rotation*Geometry.PointOnCircle3XZ(apexRadius, 270);

                Line(a1, a2, drawLine);
                Line(b1, b2, drawLine);
                Line(c1, c2, drawLine);
                Line(d1, d2, drawLine);
            }
        }
    }
}
