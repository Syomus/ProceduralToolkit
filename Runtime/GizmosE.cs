using System;
using UnityEngine;

namespace ProceduralToolkit
{
    /// <summary>
    /// Collection of drawing methods similar to Gizmos
    /// </summary>
    public static class GizmosE
    {
        /// <summary>
        /// Draws a line between specified from and to points
        /// </summary>
        public static void DrawLine(Vector3 from, Vector3 to)
        {
            Gizmos.DrawLine(from, to);
        }

        /// <summary>
        /// Draws a ray starting at ray.origin to ray.origin + ray.direction
        /// </summary>
        public static void DrawRay(Ray ray)
        {
            DrawLine(ray.origin, ray.origin + ray.direction);
        }

        /// <summary>
        /// Draws a segment
        /// </summary>
        public static void DrawSegment2(Segment2 segment)
        {
            DrawLine((Vector3) segment.a, (Vector3) segment.b);
        }

        /// <summary>
        /// Draws a segment
        /// </summary>
        public static void DrawSegment3(Segment3 segment)
        {
            DrawLine(segment.a, segment.b);
        }

        #region DrawWireQuad

        /// <summary>
        /// Draws a wireframe quad with position, rotation and scale
        /// </summary>
        public static void DrawWireQuadXY(Vector3 position, Quaternion rotation, Vector2 scale)
        {
            DrawWireQuad(position, rotation, scale, Vector3.right, Vector3.up);
        }

        /// <summary>
        /// Draws a wireframe quad with position, rotation and scale
        /// </summary>
        public static void DrawWireQuadXZ(Vector3 position, Quaternion rotation, Vector2 scale)
        {
            DrawWireQuad(position, rotation, scale, Vector3.right, Vector3.forward);
        }

        /// <summary>
        /// Draws a wireframe quad with position, rotation and scale
        /// </summary>
        public static void DrawWireQuadYZ(Vector3 position, Quaternion rotation, Vector2 scale)
        {
            DrawWireQuad(position, rotation, scale, Vector3.up, Vector3.forward);
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

            DrawLine(forwardRight, backRight);
            DrawLine(backRight, backLeft);
            DrawLine(backLeft, forwardLeft);
            DrawLine(forwardLeft, forwardRight);
        }

        #endregion DrawWireQuad

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

        #region DrawWireCircle

        /// <summary>
        /// Draws a wireframe circle with position and radius
        /// </summary>
        public static void DrawWireCircleXY(Vector3 position, float radius)
        {
            DrawWireCircle(Draw.pointOnCircleXY, position, radius);
        }

        /// <summary>
        /// Draws a wireframe circle with position, rotation and radius
        /// </summary>
        public static void DrawWireCircleXY(Vector3 position, Quaternion rotation, float radius)
        {
            DrawWireCircle(Draw.pointOnCircleXY, position, rotation, radius);
        }

        /// <summary>
        /// Draws a wireframe circle with position and radius
        /// </summary>
        public static void DrawWireCircleXZ(Vector3 position, float radius)
        {
            DrawWireCircle(Draw.pointOnCircleXZ, position, radius);
        }

        /// <summary>
        /// Draws a wireframe circle with position, rotation and radius
        /// </summary>
        public static void DrawWireCircleXZ(Vector3 position, Quaternion rotation, float radius)
        {
            DrawWireCircle(Draw.pointOnCircleXZ, position, rotation, radius);
        }

        /// <summary>
        /// Draws a wireframe circle with position and radius
        /// </summary>
        public static void DrawWireCircleYZ(Vector3 position, float radius)
        {
            DrawWireCircle(Draw.pointOnCircleYZ, position, radius);
        }

        /// <summary>
        /// Draws a wireframe circle with position, rotation and radius
        /// </summary>
        public static void DrawWireCircleYZ(Vector3 position, Quaternion rotation, float radius)
        {
            DrawWireCircle(Draw.pointOnCircleYZ, position, rotation, radius);
        }

        #endregion DrawWireCircle

        #region DrawWireCircle Universal

        /// <summary>
        /// Draws a wireframe circle with position and radius
        /// </summary>
        public static void DrawWireCircle(Func<float, float, Vector3> pointOnCircle, Vector3 position, float radius)
        {
            DrawWireArc(pointOnCircle, position, radius, 0, Draw.circleSegments, Draw.circleSegmentAngle);
        }

        /// <summary>
        /// Draws a wireframe circle with position, rotation and radius
        /// </summary>
        public static void DrawWireCircle(Func<float, float, Vector3> pointOnCircle, Vector3 position, Quaternion rotation, float radius)
        {
            DrawWireArc(pointOnCircle, position, rotation, radius, 0, Draw.circleSegments, Draw.circleSegmentAngle);
        }

        #endregion DrawWireCircle Universal

        #region DrawWireArc

        /// <summary>
        /// Draws a wireframe circular arc with position and radius
        /// </summary>
        public static void DrawWireArcXY(Vector3 position, float radius, float fromAngle, float toAngle)
        {
            DrawWireArc(Draw.pointOnCircleXY, position, radius, fromAngle, toAngle);
        }

        /// <summary>
        /// Draws a wireframe circular arc with position, rotation and radius
        /// </summary>
        public static void DrawWireArcXY(Vector3 position, Quaternion rotation, float radius, float fromAngle, float toAngle)
        {
            DrawWireArc(Draw.pointOnCircleXY, position, rotation, radius, fromAngle, toAngle);
        }

        /// <summary>
        /// Draws a wireframe circular arc with position and radius
        /// </summary>
        public static void DrawWireArcXZ(Vector3 position, float radius, float fromAngle, float toAngle)
        {
            DrawWireArc(Draw.pointOnCircleXZ, position, radius, fromAngle, toAngle);
        }

        /// <summary>
        /// Draws a wireframe circular arc with position, rotation and radius
        /// </summary>
        public static void DrawWireArcXZ(Vector3 position, Quaternion rotation, float radius, float fromAngle, float toAngle)
        {
            DrawWireArc(Draw.pointOnCircleXZ, position, rotation, radius, fromAngle, toAngle);
        }

        /// <summary>
        /// Draws a wireframe circular arc with position and radius
        /// </summary>
        public static void DrawWireArcYZ(Vector3 position, float radius, float fromAngle, float toAngle)
        {
            DrawWireArc(Draw.pointOnCircleYZ, position, radius, fromAngle, toAngle);
        }

        /// <summary>
        /// Draws a wireframe circular arc with position, rotation and radius
        /// </summary>
        public static void DrawWireArcYZ(Vector3 position, Quaternion rotation, float radius, float fromAngle, float toAngle)
        {
            DrawWireArc(Draw.pointOnCircleYZ, position, rotation, radius, fromAngle, toAngle);
        }

        #endregion DrawWireArc

        #region DrawWireArc Universal

        /// <summary>
        /// Draws a wireframe circular arc with position and radius
        /// </summary>
        public static void DrawWireArc(Func<float, float, Vector3> pointOnCircle, Vector3 position, float radius, float fromAngle, float toAngle)
        {
            Draw.GetSegmentsAndSegmentAngle(fromAngle, toAngle, out int segments, out float segmentAngle);
            DrawWireArc(pointOnCircle, position, radius, fromAngle, segments, segmentAngle);
        }

        /// <summary>
        /// Draws a wireframe circular arc with position, rotation and radius
        /// </summary>
        public static void DrawWireArc(Func<float, float, Vector3> pointOnCircle, Vector3 position, Quaternion rotation, float radius,
            float fromAngle, float toAngle)
        {
            Draw.GetSegmentsAndSegmentAngle(fromAngle, toAngle, out int segments, out float segmentAngle);
            DrawWireArc(pointOnCircle, position, rotation, radius, fromAngle, segments, segmentAngle);
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
                DrawLine(a, b);
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
                DrawLine(a, b);
            }
        }

        #endregion DrawWireArc Universal

        /// <summary>
        /// Draws a wireframe sphere with position, rotation and radius
        /// </summary>
        public static void DrawWireSphere(Vector3 position, Quaternion rotation, float radius)
        {
            DrawWireCircleXY(position, rotation, radius);
            DrawWireCircleXZ(position, rotation, radius);
            DrawWireCircleYZ(position, rotation, radius);
        }

        /// <summary>
        /// Draws a wireframe hemisphere with position, rotation and radius
        /// </summary>
        public static void DrawWireHemisphere(Vector3 position, Quaternion rotation, float radius)
        {
            DrawWireArcXY(position, rotation, radius, -90, 90);
            DrawWireCircleXZ(position, rotation, radius);
            DrawWireArcYZ(position, rotation, radius, 0, 180);
        }

        /// <summary>
        /// Draws a wireframe cone with position and rotation
        /// </summary>
        public static void DrawWireCone(Vector3 position, Quaternion rotation, float apexRadius, float angle, float length)
        {
            Vector3 baseCenter = position + rotation*Vector3.up*length;
            float baseRadius = Mathf.Tan(angle*Mathf.Deg2Rad)*length + apexRadius;
            DrawWireCircleXZ(baseCenter, rotation, baseRadius);

            Vector3 a2 = baseCenter + rotation*Geometry.PointOnCircle3XZ(baseRadius, 0);
            Vector3 b2 = baseCenter + rotation*Geometry.PointOnCircle3XZ(baseRadius, 90);
            Vector3 c2 = baseCenter + rotation*Geometry.PointOnCircle3XZ(baseRadius, 180);
            Vector3 d2 = baseCenter + rotation*Geometry.PointOnCircle3XZ(baseRadius, 270);

            if (apexRadius == 0)
            {
                DrawLine(position, a2);
                DrawLine(position, b2);
                DrawLine(position, c2);
                DrawLine(position, d2);
            }
            else
            {
                DrawWireCircleXZ(position, rotation, apexRadius);

                Vector3 a1 = position + rotation*Geometry.PointOnCircle3XZ(apexRadius, 0);
                Vector3 b1 = position + rotation*Geometry.PointOnCircle3XZ(apexRadius, 90);
                Vector3 c1 = position + rotation*Geometry.PointOnCircle3XZ(apexRadius, 180);
                Vector3 d1 = position + rotation*Geometry.PointOnCircle3XZ(apexRadius, 270);

                DrawLine(a1, a2);
                DrawLine(b1, b2);
                DrawLine(c1, c2);
                DrawLine(d1, d2);
            }
        }
    }
}
