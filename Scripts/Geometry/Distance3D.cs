using UnityEngine;

namespace ProceduralToolkit
{
    /// <summary>
    /// Collection of distance calculation algorithms
    /// </summary>
    public static partial class Distance
    {
        #region Point-Line

        /// <summary>
        /// Returns a distance to the closest point on the line
        /// </summary>
        public static float PointLine(Vector3 point, Line3 line)
        {
            return Vector3.Distance(point, Geometry.ClosestPointOnLine(point, line));
        }

        /// <summary>
        /// Returns a distance to the closest point on the line
        /// </summary>
        public static float PointLine(Vector3 point, Vector3 lineOrigin, Vector3 lineDirection)
        {
            return Vector3.Distance(point, Geometry.ClosestPointOnLine(point, lineOrigin, lineDirection));
        }

        #endregion Point-Line

        #region Point-Ray

        /// <summary>
        /// Returns a distance to the closest point on the ray
        /// </summary>
        public static float PointRay(Vector3 point, Ray ray)
        {
            return Vector3.Distance(point, Geometry.ClosestPointOnRay(point, ray));
        }

        /// <summary>
        /// Returns a distance to the closest point on the ray
        /// </summary>
        public static float PointRay(Vector3 point, Vector3 rayOrigin, Vector3 rayDirection)
        {
            return Vector3.Distance(point, Geometry.ClosestPointOnRay(point, rayOrigin, rayDirection));
        }

        #endregion Point-Ray

        #region Point-Segment

        /// <summary>
        /// Returns a distance to the closest point on the segment
        /// </summary>
        public static float PointSegment(Vector3 point, Segment3 segment)
        {
            return Vector3.Distance(point, Geometry.ClosestPointOnSegment(point, segment));
        }

        /// <summary>
        /// Returns a distance to the closest point on the segment
        /// </summary>
        public static float PointSegment(Vector3 point, Vector3 segmentA, Vector3 segmentB)
        {
            return Vector3.Distance(point, Geometry.ClosestPointOnSegment(point, segmentA, segmentB));
        }

        #endregion Point-Segment

        #region Point-Sphere

        /// <summary>
        /// Returns a distance to the closest point on the sphere
        /// </summary>
        /// <returns>Positive value if the point is outside, negative otherwise</returns>
        public static float PointSphere(Vector3 point, Sphere sphere)
        {
            return PointSphere(point, sphere.center, sphere.radius);
        }

        /// <summary>
        /// Returns a distance to the closest point on the sphere
        /// </summary>
        /// <returns>Positive value if the point is outside, negative otherwise</returns>
        public static float PointSphere(Vector3 point, Vector3 center, float radius)
        {
            return (center - point).magnitude - radius;
        }

        #endregion Point-Sphere
    }
}
