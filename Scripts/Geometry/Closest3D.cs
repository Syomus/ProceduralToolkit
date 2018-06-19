using UnityEngine;

namespace ProceduralToolkit
{
    /// <summary>
    /// Collection of closest point(s) algorithms
    /// </summary>
    public static partial class Closest
    {
        #region Point-Line

        /// <summary>
        /// Projects the point onto the line
        /// </summary>
        public static Vector3 PointLine(Vector3 point, Line3 line)
        {
            float projectedX;
            return PointLine(point, line.origin, line.direction, out projectedX);
        }

        /// <summary>
        /// Projects the point onto the line
        /// </summary>
        /// <param name="projectedX">Position of the projected point on the line relative to the origin</param>
        public static Vector3 PointLine(Vector3 point, Line3 line, out float projectedX)
        {
            return PointLine(point, line.origin, line.direction, out projectedX);
        }

        /// <summary>
        /// Projects the point onto the line
        /// </summary>
        /// <param name="lineDirection">Normalized direction of the line</param>
        public static Vector3 PointLine(Vector3 point, Vector3 lineOrigin, Vector3 lineDirection)
        {
            float projectedX;
            return PointLine(point, lineOrigin, lineDirection, out projectedX);
        }

        /// <summary>
        /// Projects the point onto the line
        /// </summary>
        /// <param name="lineDirection">Normalized direction of the line</param>
        /// <param name="projectedX">Position of the projected point on the line relative to the origin</param>
        public static Vector3 PointLine(Vector3 point, Vector3 lineOrigin, Vector3 lineDirection, out float projectedX)
        {
            // In theory, sqrMagnitude should be 1, but in practice this division helps with numerical stability
            projectedX = Vector3.Dot(lineDirection, point - lineOrigin)/lineDirection.sqrMagnitude;
            return lineOrigin + lineDirection*projectedX;
        }

        #endregion Point-Line

        #region Point-Ray

        /// <summary>
        /// Projects the point onto the ray
        /// </summary>
        public static Vector3 PointRay(Vector3 point, Ray ray)
        {
            float projectedX;
            return PointRay(point, ray.origin, ray.direction, out projectedX);
        }

        /// <summary>
        /// Projects the point onto the ray
        /// </summary>
        /// <param name="projectedX">Position of the projected point on the ray relative to the origin</param>
        public static Vector3 PointRay(Vector3 point, Ray ray, out float projectedX)
        {
            return PointRay(point, ray.origin, ray.direction, out projectedX);
        }

        /// <summary>
        /// Projects the point onto the ray
        /// </summary>
        /// <param name="rayDirection">Normalized direction of the ray</param>
        public static Vector3 PointRay(Vector3 point, Vector3 rayOrigin, Vector3 rayDirection)
        {
            float projectedX;
            return PointRay(point, rayOrigin, rayDirection, out projectedX);
        }

        /// <summary>
        /// Projects the point onto the ray
        /// </summary>
        /// <param name="rayDirection">Normalized direction of the ray</param>
        /// <param name="projectedX">Position of the projected point on the ray relative to the origin</param>
        public static Vector3 PointRay(Vector3 point, Vector3 rayOrigin, Vector3 rayDirection, out float projectedX)
        {
            Vector3 toPoint = point - rayOrigin;
            float pointProjection = Vector3.Dot(rayDirection, toPoint);
            if (pointProjection <= 0)
            {
                projectedX = 0;
                return rayOrigin;
            }

            // In theory, sqrMagnitude should be 1, but in practice this division helps with numerical stability
            projectedX = pointProjection/rayDirection.sqrMagnitude;
            return rayOrigin + rayDirection*projectedX;
        }

        #endregion Point-Ray

        #region Point-Segment

        /// <summary>
        /// Projects the point onto the segment
        /// </summary>
        public static Vector3 PointSegment(Vector3 point, Segment3 segment)
        {
            float projectedX;
            return PointSegment(point, segment.a, segment.b, out projectedX);
        }

        /// <summary>
        /// Projects the point onto the segment
        /// </summary>
        /// <param name="projectedX">Normalized position of the projected point on the segment. 
        /// Value of zero means that the projected point coincides with segment.a. 
        /// Value of one means that the projected point coincides with segment.b.</param>
        public static Vector3 PointSegment(Vector3 point, Segment3 segment, out float projectedX)
        {
            return PointSegment(point, segment.a, segment.b, out projectedX);
        }

        /// <summary>
        /// Projects the point onto the segment
        /// </summary>
        public static Vector3 PointSegment(Vector3 point, Vector3 segmentA, Vector3 segmentB)
        {
            float projectedX;
            return PointSegment(point, segmentA, segmentB, out projectedX);
        }

        /// <summary>
        /// Projects the point onto the segment
        /// </summary>
        /// <param name="projectedX">Normalized position of the projected point on the segment. 
        /// Value of zero means that the projected point coincides with <paramref name="segmentA"/>. 
        /// Value of one means that the projected point coincides with <paramref name="segmentB"/>.</param>
        public static Vector3 PointSegment(Vector3 point, Vector3 segmentA, Vector3 segmentB, out float projectedX)
        {
            Vector3 segmentDirection = segmentB - segmentA;
            float sqrSegmentLength = segmentDirection.sqrMagnitude;
            if (sqrSegmentLength < Geometry.Epsilon)
            {
                // The segment is a point
                projectedX = 0;
                return segmentA;
            }

            float pointProjection = Vector3.Dot(segmentDirection, point - segmentA);
            if (pointProjection <= 0)
            {
                projectedX = 0;
                return segmentA;
            }
            if (pointProjection >= sqrSegmentLength)
            {
                projectedX = 1;
                return segmentB;
            }

            projectedX = pointProjection/sqrSegmentLength;
            return segmentA + segmentDirection*projectedX;
        }

        #endregion Point-Segment

        #region Point-Sphere

        /// <summary>
        /// Projects the point onto the sphere
        /// </summary>
        public static Vector3 PointSphere(Vector3 point, Sphere sphere)
        {
            return PointSphere(point, sphere.center, sphere.radius);
        }

        /// <summary>
        /// Projects the point onto the sphere
        /// </summary>
        public static Vector3 PointSphere(Vector3 point, Vector3 sphereCenter, float sphereRadius)
        {
            return sphereCenter + (point - sphereCenter).normalized*sphereRadius;
        }

        #endregion Point-Sphere
    }
}
