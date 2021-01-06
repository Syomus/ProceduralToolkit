using System;
using UnityEngine;

namespace ProceduralToolkit
{
    /// <summary>
    /// Collection of drawing methods similar to Debug.DrawLine
    /// </summary>
    public static class DebugE
    {
        #region DrawLine

        /// <summary>
        /// Draws a line between specified from and to points
        /// </summary>
        public static void DrawLine(Vector3 from, Vector3 to)
        {
            Debug.DrawLine(from, to, Color.white);
        }

        /// <summary>
        /// Draws a line between specified from and to points
        /// </summary>
        public static void DrawLine(Vector3 from, Vector3 to, Color color, float duration = 0, bool depthTest = true)
        {
            Debug.DrawLine(from, to, color, duration, depthTest);
        }

        #endregion DrawLine

        #region DrawRay

        /// <summary>
        /// Draws a ray starting at ray.origin to ray.origin + ray.direction
        /// </summary>
        public static void DrawRay(Ray ray)
        {
            DrawLine(ray.origin, ray.origin + ray.direction, Color.white);
        }

        /// <summary>
        /// Draws a ray starting at ray.origin to ray.origin + ray.direction
        /// </summary>
        public static void DrawRay(Ray ray, Color color, float duration = 0, bool depthTest = true)
        {
            DrawLine(ray.origin, ray.origin + ray.direction, color, duration, depthTest);
        }

        #endregion DrawRay

        #region DrawSegment2

        /// <summary>
        /// Draws a segment
        /// </summary>
        public static void DrawSegment2(Segment2 segment)
        {
            DrawLine((Vector3) segment.a, (Vector3) segment.b, Color.white);
        }

        /// <summary>
        /// Draws a segment
        /// </summary>
        public static void DrawSegment2(Segment2 segment, Color color, float duration = 0, bool depthTest = true)
        {
            DrawLine((Vector3) segment.a, (Vector3) segment.b, color, duration, depthTest);
        }

        #endregion DrawSegment2

        #region DrawSegment3

        /// <summary>
        /// Draws a segment
        /// </summary>
        public static void DrawSegment3(Segment3 segment)
        {
            DrawLine(segment.a, segment.b, Color.white);
        }

        /// <summary>
        /// Draws a segment
        /// </summary>
        public static void DrawSegment3(Segment3 segment, Color color, float duration = 0, bool depthTest = true)
        {
            DrawLine(segment.a, segment.b, color, duration, depthTest);
        }

        #endregion DrawSegment3

        #region DrawWireQuad

        /// <summary>
        /// Draws a wireframe quad with position, rotation and scale
        /// </summary>
        public static void DrawWireQuadXY(Vector3 position, Quaternion rotation, Vector2 scale)
        {
            DrawWireQuad(position, rotation, scale, Vector3.right, Vector3.up, Color.white);
        }

        /// <summary>
        /// Draws a wireframe quad with position, rotation and scale
        /// </summary>
        public static void DrawWireQuadXY(Vector3 position, Quaternion rotation, Vector2 scale, Color color, float duration = 0,
            bool depthTest = true)
        {
            DrawWireQuad(position, rotation, scale, Vector3.right, Vector3.up, color, duration, depthTest);
        }

        /// <summary>
        /// Draws a wireframe quad with position, rotation and scale
        /// </summary>
        public static void DrawWireQuadXZ(Vector3 position, Quaternion rotation, Vector2 scale)
        {
            DrawWireQuad(position, rotation, scale, Vector3.right, Vector3.forward, Color.white);
        }

        /// <summary>
        /// Draws a wireframe quad with position, rotation and scale
        /// </summary>
        public static void DrawWireQuadXZ(Vector3 position, Quaternion rotation, Vector2 scale, Color color, float duration = 0,
            bool depthTest = true)
        {
            DrawWireQuad(position, rotation, scale, Vector3.right, Vector3.forward, color, duration, depthTest);
        }

        /// <summary>
        /// Draws a wireframe quad with position, rotation and scale
        /// </summary>
        public static void DrawWireQuadYZ(Vector3 position, Quaternion rotation, Vector2 scale)
        {
            DrawWireQuad(position, rotation, scale, Vector3.up, Vector3.forward, Color.white);
        }

        /// <summary>
        /// Draws a wireframe quad with position, rotation and scale
        /// </summary>
        public static void DrawWireQuadYZ(Vector3 position, Quaternion rotation, Vector2 scale, Color color, float duration = 0,
            bool depthTest = true)
        {
            DrawWireQuad(position, rotation, scale, Vector3.up, Vector3.forward, color, duration, depthTest);
        }

        /// <summary>
        /// Draws a wireframe quad with position, rotation and scale
        /// </summary>
        public static void DrawWireQuad(Vector3 position, Quaternion rotation, Vector2 scale, Vector3 planeRight, Vector3 planeForward)
        {
            Vector3 right = rotation*planeRight*scale.x;
            Vector3 forward = rotation*planeForward*scale.y;
            Vector3 forwardRight = position + right*0.5f + forward*0.5f;
            Vector3 backRight = forwardRight - forward;
            Vector3 backLeft = backRight - right;
            Vector3 forwardLeft = forwardRight - right;

            DrawLine(forwardRight, backRight, Color.white);
            DrawLine(backRight, backLeft, Color.white);
            DrawLine(backLeft, forwardLeft, Color.white);
            DrawLine(forwardLeft, forwardRight, Color.white);
        }

        /// <summary>
        /// Draws a wireframe quad with position, rotation and scale
        /// </summary>
        public static void DrawWireQuad(Vector3 position, Quaternion rotation, Vector2 scale, Vector3 planeRight, Vector3 planeForward, Color color,
            float duration = 0, bool depthTest = true)
        {
            Vector3 right = rotation*planeRight*scale.x;
            Vector3 forward = rotation*planeForward*scale.y;
            Vector3 forwardRight = position + right*0.5f + forward*0.5f;
            Vector3 backRight = forwardRight - forward;
            Vector3 backLeft = backRight - right;
            Vector3 forwardLeft = forwardRight - right;

            DrawLine(forwardRight, backRight, color, duration, depthTest);
            DrawLine(backRight, backLeft, color, duration, depthTest);
            DrawLine(backLeft, forwardLeft, color, duration, depthTest);
            DrawLine(forwardLeft, forwardRight, color, duration, depthTest);
        }

        #endregion DrawWireQuad

        #region DrawWireCube

        /// <summary>
        /// Draws a wireframe cube with position, rotation and scale
        /// </summary>
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

            DrawLine(a1, b1, Color.white);
            DrawLine(b1, c1, Color.white);
            DrawLine(c1, d1, Color.white);
            DrawLine(d1, a1, Color.white);

            DrawLine(a2, b2, Color.white);
            DrawLine(b2, c2, Color.white);
            DrawLine(c2, d2, Color.white);
            DrawLine(d2, a2, Color.white);

            DrawLine(a1, a2, Color.white);
            DrawLine(b1, b2, Color.white);
            DrawLine(c1, c2, Color.white);
            DrawLine(d1, d2, Color.white);
        }

        /// <summary>
        /// Draws a wireframe cube with position, rotation and scale
        /// </summary>
        public static void DrawWireCube(Vector3 position, Quaternion rotation, Vector3 scale, Color color, float duration = 0, bool depthTest = true)
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

            DrawLine(a1, b1, color, duration, depthTest);
            DrawLine(b1, c1, color, duration, depthTest);
            DrawLine(c1, d1, color, duration, depthTest);
            DrawLine(d1, a1, color, duration, depthTest);

            DrawLine(a2, b2, color, duration, depthTest);
            DrawLine(b2, c2, color, duration, depthTest);
            DrawLine(c2, d2, color, duration, depthTest);
            DrawLine(d2, a2, color, duration, depthTest);

            DrawLine(a1, a2, color, duration, depthTest);
            DrawLine(b1, b2, color, duration, depthTest);
            DrawLine(c1, c2, color, duration, depthTest);
            DrawLine(d1, d2, color, duration, depthTest);
        }

        #endregion DrawWireCube

        #region DrawWireCircle

        /// <summary>
        /// Draws a wireframe circle with position and radius
        /// </summary>
        public static void DrawWireCircleXY(Vector3 position, float radius)
        {
            DrawWireCircle(Draw.pointOnCircleXY, position, radius, Color.white);
        }

        /// <summary>
        /// Draws a wireframe circle with position and radius
        /// </summary>
        public static void DrawWireCircleXY(Vector3 position, float radius, Color color, float duration = 0, bool depthTest = true)
        {
            DrawWireCircle(Draw.pointOnCircleXY, position, radius, color, duration, depthTest);
        }

        /// <summary>
        /// Draws a wireframe circle with position, rotation and radius
        /// </summary>
        public static void DrawWireCircleXY(Vector3 position, Quaternion rotation, float radius)
        {
            DrawWireCircle(Draw.pointOnCircleXY, position, rotation, radius, Color.white);
        }

        /// <summary>
        /// Draws a wireframe circle with position, rotation and radius
        /// </summary>
        public static void DrawWireCircleXY(Vector3 position, Quaternion rotation, float radius, Color color, float duration = 0,
            bool depthTest = true)
        {
            DrawWireCircle(Draw.pointOnCircleXY, position, rotation, radius, color, duration, depthTest);
        }

        /// <summary>
        /// Draws a wireframe circle with position and radius
        /// </summary>
        public static void DrawWireCircleXZ(Vector3 position, float radius)
        {
            DrawWireCircle(Draw.pointOnCircleXZ, position, radius, Color.white);
        }

        /// <summary>
        /// Draws a wireframe circle with position and radius
        /// </summary>
        public static void DrawWireCircleXZ(Vector3 position, float radius, Color color, float duration = 0, bool depthTest = true)
        {
            DrawWireCircle(Draw.pointOnCircleXZ, position, radius, color, duration, depthTest);
        }

        /// <summary>
        /// Draws a wireframe circle with position, rotation and radius
        /// </summary>
        public static void DrawWireCircleXZ(Vector3 position, Quaternion rotation, float radius)
        {
            DrawWireCircle(Draw.pointOnCircleXZ, position, rotation, radius, Color.white);
        }

        /// <summary>
        /// Draws a wireframe circle with position, rotation and radius
        /// </summary>
        public static void DrawWireCircleXZ(Vector3 position, Quaternion rotation, float radius, Color color, float duration = 0,
            bool depthTest = true)
        {
            DrawWireCircle(Draw.pointOnCircleXZ, position, rotation, radius, color, duration, depthTest);
        }

        /// <summary>
        /// Draws a wireframe circle with position and radius
        /// </summary>
        public static void DrawWireCircleYZ(Vector3 position, float radius)
        {
            DrawWireCircle(Draw.pointOnCircleYZ, position, radius, Color.white);
        }

        /// <summary>
        /// Draws a wireframe circle with position and radius
        /// </summary>
        public static void DrawWireCircleYZ(Vector3 position, float radius, Color color, float duration = 0, bool depthTest = true)
        {
            DrawWireCircle(Draw.pointOnCircleYZ, position, radius, color, duration, depthTest);
        }

        /// <summary>
        /// Draws a wireframe circle with position, rotation and radius
        /// </summary>
        public static void DrawWireCircleYZ(Vector3 position, Quaternion rotation, float radius)
        {
            DrawWireCircle(Draw.pointOnCircleYZ, position, rotation, radius, Color.white);
        }

        /// <summary>
        /// Draws a wireframe circle with position, rotation and radius
        /// </summary>
        public static void DrawWireCircleYZ(Vector3 position, Quaternion rotation, float radius, Color color, float duration = 0,
            bool depthTest = true)
        {
            DrawWireCircle(Draw.pointOnCircleYZ, position, rotation, radius, color, duration, depthTest);
        }

        #endregion DrawWireCircle

        #region DrawWireCircle Universal

        /// <summary>
        /// Draws a wireframe circle with position and radius
        /// </summary>
        public static void DrawWireCircle(Func<float, float, Vector3> pointOnCircle, Vector3 position, float radius)
        {
            DrawWireArc(pointOnCircle, position, radius, 0, Draw.circleSegments, Draw.circleSegmentAngle, Color.white);
        }

        /// <summary>
        /// Draws a wireframe circle with position and radius
        /// </summary>
        public static void DrawWireCircle(Func<float, float, Vector3> pointOnCircle, Vector3 position, float radius, Color color, float duration = 0,
            bool depthTest = true)
        {
            DrawWireArc(pointOnCircle, position, radius, 0, Draw.circleSegments, Draw.circleSegmentAngle, color, duration, depthTest);
        }

        /// <summary>
        /// Draws a wireframe circle with position, rotation and radius
        /// </summary>
        public static void DrawWireCircle(Func<float, float, Vector3> pointOnCircle, Vector3 position, Quaternion rotation, float radius)
        {
            DrawWireArc(pointOnCircle, position, rotation, radius, 0, Draw.circleSegments, Draw.circleSegmentAngle, Color.white);
        }

        /// <summary>
        /// Draws a wireframe circle with position, rotation and radius
        /// </summary>
        public static void DrawWireCircle(Func<float, float, Vector3> pointOnCircle, Vector3 position, Quaternion rotation, float radius, Color color,
            float duration = 0, bool depthTest = true)
        {
            DrawWireArc(pointOnCircle, position, rotation, radius, 0, Draw.circleSegments, Draw.circleSegmentAngle, color, duration, depthTest);
        }

        #endregion DrawWireCircle Universal

        #region DrawWireArc

        /// <summary>
        /// Draws a wireframe circular arc with position and radius
        /// </summary>
        public static void DrawWireArcXY(Vector3 position, float radius, float fromAngle, float toAngle)
        {
            DrawWireArc(Draw.pointOnCircleXY, position, radius, fromAngle, toAngle, Color.white);
        }

        /// <summary>
        /// Draws a wireframe circular arc with position and radius
        /// </summary>
        public static void DrawWireArcXY(Vector3 position, float radius, float fromAngle, float toAngle, Color color, float duration = 0,
            bool depthTest = true)
        {
            DrawWireArc(Draw.pointOnCircleXY, position, radius, fromAngle, toAngle, color, duration, depthTest);
        }

        /// <summary>
        /// Draws a wireframe circular arc with position, rotation and radius
        /// </summary>
        public static void DrawWireArcXY(Vector3 position, Quaternion rotation, float radius, float fromAngle, float toAngle)
        {
            DrawWireArc(Draw.pointOnCircleXY, position, rotation, radius, fromAngle, toAngle, Color.white);
        }

        /// <summary>
        /// Draws a wireframe circular arc with position, rotation and radius
        /// </summary>
        public static void DrawWireArcXY(Vector3 position, Quaternion rotation, float radius, float fromAngle, float toAngle, Color color,
            float duration = 0, bool depthTest = true)
        {
            DrawWireArc(Draw.pointOnCircleXY, position, rotation, radius, fromAngle, toAngle, color, duration, depthTest);
        }

        /// <summary>
        /// Draws a wireframe circular arc with position and radius
        /// </summary>
        public static void DrawWireArcXZ(Vector3 position, float radius, float fromAngle, float toAngle)
        {
            DrawWireArc(Draw.pointOnCircleXZ, position, radius, fromAngle, toAngle, Color.white);
        }

        /// <summary>
        /// Draws a wireframe circular arc with position and radius
        /// </summary>
        public static void DrawWireArcXZ(Vector3 position, float radius, float fromAngle, float toAngle, Color color, float duration = 0,
            bool depthTest = true)
        {
            DrawWireArc(Draw.pointOnCircleXZ, position, radius, fromAngle, toAngle, color, duration, depthTest);
        }

        /// <summary>
        /// Draws a wireframe circular arc with position, rotation and radius
        /// </summary>
        public static void DrawWireArcXZ(Vector3 position, Quaternion rotation, float radius, float fromAngle, float toAngle)
        {
            DrawWireArc(Draw.pointOnCircleXZ, position, rotation, radius, fromAngle, toAngle, Color.white);
        }

        /// <summary>
        /// Draws a wireframe circular arc with position, rotation and radius
        /// </summary>
        public static void DrawWireArcXZ(Vector3 position, Quaternion rotation, float radius, float fromAngle, float toAngle, Color color,
            float duration = 0, bool depthTest = true)
        {
            DrawWireArc(Draw.pointOnCircleXZ, position, rotation, radius, fromAngle, toAngle, color, duration, depthTest);
        }

        /// <summary>
        /// Draws a wireframe circular arc with position and radius
        /// </summary>
        public static void DrawWireArcYZ(Vector3 position, float radius, float fromAngle, float toAngle)
        {
            DrawWireArc(Draw.pointOnCircleYZ, position, radius, fromAngle, toAngle, Color.white);
        }

        /// <summary>
        /// Draws a wireframe circular arc with position and radius
        /// </summary>
        public static void DrawWireArcYZ(Vector3 position, float radius, float fromAngle, float toAngle, Color color, float duration = 0,
            bool depthTest = true)
        {
            DrawWireArc(Draw.pointOnCircleYZ, position, radius, fromAngle, toAngle, color, duration, depthTest);
        }

        /// <summary>
        /// Draws a wireframe circular arc with position, rotation and radius
        /// </summary>
        public static void DrawWireArcYZ(Vector3 position, Quaternion rotation, float radius, float fromAngle, float toAngle)
        {
            DrawWireArc(Draw.pointOnCircleYZ, position, rotation, radius, fromAngle, toAngle, Color.white);
        }

        /// <summary>
        /// Draws a wireframe circular arc with position, rotation and radius
        /// </summary>
        public static void DrawWireArcYZ(Vector3 position, Quaternion rotation, float radius, float fromAngle, float toAngle, Color color,
            float duration = 0, bool depthTest = true)
        {
            DrawWireArc(Draw.pointOnCircleYZ, position, rotation, radius, fromAngle, toAngle, color, duration, depthTest);
        }

        #endregion DrawWireArc

        #region DrawWireArc Universal

        /// <summary>
        /// Draws a wireframe circular arc with position and radius
        /// </summary>
        public static void DrawWireArc(Func<float, float, Vector3> pointOnCircle, Vector3 position, float radius, float fromAngle, float toAngle)
        {
            Draw.GetSegmentsAndSegmentAngle(fromAngle, toAngle, out int segments, out float segmentAngle);
            DrawWireArc(pointOnCircle, position, radius, fromAngle, segments, segmentAngle, Color.white);
        }

        /// <summary>
        /// Draws a wireframe circular arc with position and radius
        /// </summary>
        public static void DrawWireArc(Func<float, float, Vector3> pointOnCircle, Vector3 position, float radius, float fromAngle, float toAngle,
            Color color, float duration = 0, bool depthTest = true)
        {
            Draw.GetSegmentsAndSegmentAngle(fromAngle, toAngle, out int segments, out float segmentAngle);
            DrawWireArc(pointOnCircle, position, radius, fromAngle, segments, segmentAngle, color, duration, depthTest);
        }

        /// <summary>
        /// Draws a wireframe circular arc with position, rotation and radius
        /// </summary>
        public static void DrawWireArc(Func<float, float, Vector3> pointOnCircle, Vector3 position, Quaternion rotation, float radius,
            float fromAngle, float toAngle)
        {
            Draw.GetSegmentsAndSegmentAngle(fromAngle, toAngle, out int segments, out float segmentAngle);
            DrawWireArc(pointOnCircle, position, rotation, radius, fromAngle, segments, segmentAngle, Color.white);
        }

        /// <summary>
        /// Draws a wireframe circular arc with position, rotation and radius
        /// </summary>
        public static void DrawWireArc(Func<float, float, Vector3> pointOnCircle, Vector3 position, Quaternion rotation, float radius,
            float fromAngle, float toAngle, Color color, float duration = 0, bool depthTest = true)
        {
            Draw.GetSegmentsAndSegmentAngle(fromAngle, toAngle, out int segments, out float segmentAngle);
            DrawWireArc(pointOnCircle, position, rotation, radius, fromAngle, segments, segmentAngle, color, duration, depthTest);
        }

        /// <summary>
        /// Draws a wireframe circular arc with position and radius
        /// </summary>
        public static void DrawWireArc(Func<float, float, Vector3> pointOnCircle, Vector3 position, float radius, float fromAngle, int segments,
            float segmentAngle)
        {
            float currentAngle = fromAngle;
            for (var i = 0; i < segments; i++)
            {
                Vector3 a = position + pointOnCircle(radius, currentAngle);
                currentAngle += segmentAngle;
                Vector3 b = position + pointOnCircle(radius, currentAngle);
                DrawLine(a, b, Color.white);
            }
        }

        /// <summary>
        /// Draws a wireframe circular arc with position and radius
        /// </summary>
        public static void DrawWireArc(Func<float, float, Vector3> pointOnCircle, Vector3 position, float radius, float fromAngle, int segments,
            float segmentAngle, Color color, float duration = 0, bool depthTest = true)
        {
            float currentAngle = fromAngle;
            for (var i = 0; i < segments; i++)
            {
                Vector3 a = position + pointOnCircle(radius, currentAngle);
                currentAngle += segmentAngle;
                Vector3 b = position + pointOnCircle(radius, currentAngle);
                DrawLine(a, b, color, duration, depthTest);
            }
        }

        /// <summary>
        /// Draws a wireframe circular arc with position, rotation and radius
        /// </summary>
        public static void DrawWireArc(Func<float, float, Vector3> pointOnCircle, Vector3 position, Quaternion rotation, float radius,
            float fromAngle, int segments, float segmentAngle)
        {
            float currentAngle = fromAngle;
            for (var i = 0; i < segments; i++)
            {
                Vector3 a = position + rotation*pointOnCircle(radius, currentAngle);
                currentAngle += segmentAngle;
                Vector3 b = position + rotation*pointOnCircle(radius, currentAngle);
                DrawLine(a, b, Color.white);
            }
        }

        /// <summary>
        /// Draws a wireframe circular arc with position, rotation and radius
        /// </summary>
        public static void DrawWireArc(Func<float, float, Vector3> pointOnCircle, Vector3 position, Quaternion rotation, float radius,
            float fromAngle, int segments, float segmentAngle, Color color, float duration = 0, bool depthTest = true)
        {
            float currentAngle = fromAngle;
            for (var i = 0; i < segments; i++)
            {
                Vector3 a = position + rotation*pointOnCircle(radius, currentAngle);
                currentAngle += segmentAngle;
                Vector3 b = position + rotation*pointOnCircle(radius, currentAngle);
                DrawLine(a, b, color, duration, depthTest);
            }
        }

        #endregion DrawWireArc Universal

        #region DrawWireSphere

        /// <summary>
        /// Draws a wireframe sphere with position, rotation and radius
        /// </summary>
        public static void DrawWireSphere(Vector3 position, Quaternion rotation, float radius)
        {
            DrawWireCircleXY(position, rotation, radius, Color.white);
            DrawWireCircleXZ(position, rotation, radius, Color.white);
            DrawWireCircleYZ(position, rotation, radius, Color.white);
        }

        /// <summary>
        /// Draws a wireframe sphere with position, rotation and radius
        /// </summary>
        public static void DrawWireSphere(Vector3 position, Quaternion rotation, float radius, Color color, float duration = 0, bool depthTest = true)
        {
            DrawWireCircleXY(position, rotation, radius, color, duration, depthTest);
            DrawWireCircleXZ(position, rotation, radius, color, duration, depthTest);
            DrawWireCircleYZ(position, rotation, radius, color, duration, depthTest);
        }

        #endregion DrawWireSphere

        #region DrawWireHemisphere

        /// <summary>
        /// Draws a wireframe hemisphere with position, rotation and radius
        /// </summary>
        public static void DrawWireHemisphere(Vector3 position, Quaternion rotation, float radius)
        {
            DrawWireArcXY(position, rotation, radius, -90, 90, Color.white);
            DrawWireCircleXZ(position, rotation, radius, Color.white);
            DrawWireArcYZ(position, rotation, radius, 0, 180, Color.white);
        }

        /// <summary>
        /// Draws a wireframe hemisphere with position, rotation and radius
        /// </summary>
        public static void DrawWireHemisphere(Vector3 position, Quaternion rotation, float radius, Color color, float duration = 0,
            bool depthTest = true)
        {
            DrawWireArcXY(position, rotation, radius, -90, 90, color, duration, depthTest);
            DrawWireCircleXZ(position, rotation, radius, color, duration, depthTest);
            DrawWireArcYZ(position, rotation, radius, 0, 180, color, duration, depthTest);
        }

        #endregion DrawWireHemisphere

        #region DrawWireCone

        /// <summary>
        /// Draws a wireframe cone with position and rotation
        /// </summary>
        public static void DrawWireCone(Vector3 position, Quaternion rotation, float apexRadius, float angle, float length)
        {
            Vector3 baseCenter = position + rotation*Vector3.up*length;
            float baseRadius = Mathf.Tan(angle*Mathf.Deg2Rad)*length + apexRadius;
            DrawWireCircleXZ(baseCenter, rotation, baseRadius, Color.white);

            Vector3 a2 = baseCenter + rotation*Geometry.PointOnCircle3XZ(baseRadius, 0);
            Vector3 b2 = baseCenter + rotation*Geometry.PointOnCircle3XZ(baseRadius, 90);
            Vector3 c2 = baseCenter + rotation*Geometry.PointOnCircle3XZ(baseRadius, 180);
            Vector3 d2 = baseCenter + rotation*Geometry.PointOnCircle3XZ(baseRadius, 270);

            if (apexRadius == 0)
            {
                DrawLine(position, a2, Color.white);
                DrawLine(position, b2, Color.white);
                DrawLine(position, c2, Color.white);
                DrawLine(position, d2, Color.white);
            }
            else
            {
                DrawWireCircleXZ(position, rotation, apexRadius, Color.white);

                Vector3 a1 = position + rotation*Geometry.PointOnCircle3XZ(apexRadius, 0);
                Vector3 b1 = position + rotation*Geometry.PointOnCircle3XZ(apexRadius, 90);
                Vector3 c1 = position + rotation*Geometry.PointOnCircle3XZ(apexRadius, 180);
                Vector3 d1 = position + rotation*Geometry.PointOnCircle3XZ(apexRadius, 270);

                DrawLine(a1, a2, Color.white);
                DrawLine(b1, b2, Color.white);
                DrawLine(c1, c2, Color.white);
                DrawLine(d1, d2, Color.white);
            }
        }

        /// <summary>
        /// Draws a wireframe cone with position and rotation
        /// </summary>
        public static void DrawWireCone(Vector3 position, Quaternion rotation, float apexRadius, float angle, float length, Color color,
            float duration = 0, bool depthTest = true)
        {
            Vector3 baseCenter = position + rotation*Vector3.up*length;
            float baseRadius = Mathf.Tan(angle*Mathf.Deg2Rad)*length + apexRadius;
            DrawWireCircleXZ(baseCenter, rotation, baseRadius, color, duration, depthTest);

            Vector3 a2 = baseCenter + rotation*Geometry.PointOnCircle3XZ(baseRadius, 0);
            Vector3 b2 = baseCenter + rotation*Geometry.PointOnCircle3XZ(baseRadius, 90);
            Vector3 c2 = baseCenter + rotation*Geometry.PointOnCircle3XZ(baseRadius, 180);
            Vector3 d2 = baseCenter + rotation*Geometry.PointOnCircle3XZ(baseRadius, 270);

            if (apexRadius == 0)
            {
                DrawLine(position, a2, color, duration, depthTest);
                DrawLine(position, b2, color, duration, depthTest);
                DrawLine(position, c2, color, duration, depthTest);
                DrawLine(position, d2, color, duration, depthTest);
            }
            else
            {
                DrawWireCircleXZ(position, rotation, apexRadius, color, duration, depthTest);

                Vector3 a1 = position + rotation*Geometry.PointOnCircle3XZ(apexRadius, 0);
                Vector3 b1 = position + rotation*Geometry.PointOnCircle3XZ(apexRadius, 90);
                Vector3 c1 = position + rotation*Geometry.PointOnCircle3XZ(apexRadius, 180);
                Vector3 d1 = position + rotation*Geometry.PointOnCircle3XZ(apexRadius, 270);

                DrawLine(a1, a2, color, duration, depthTest);
                DrawLine(b1, b2, color, duration, depthTest);
                DrawLine(c1, c2, color, duration, depthTest);
                DrawLine(d1, d2, color, duration, depthTest);
            }
        }

        #endregion DrawWireCone
    }
}
