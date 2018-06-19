using UnityEngine;

namespace ProceduralToolkit
{
    /// <summary>
    /// Collection of intersection algorithms
    /// </summary>
    public static partial class Intersect
    {
        #region Point-Line

        /// <summary>
        /// Tests if the point lies on the line
        /// </summary>
        public static bool PointLine(Vector3 point, Line3 line)
        {
            return PointLine(point, line.origin, line.direction);
        }

        /// <summary>
        /// Tests if the point lies on the line
        /// </summary>
        public static bool PointLine(Vector3 point, Vector3 lineOrigin, Vector3 lineDirection)
        {
            return Distance.PointLine(point, lineOrigin, lineDirection) < Geometry.Epsilon;
        }

        #endregion Point-Line

        #region Point-Ray

        /// <summary>
        /// Tests if the point lies on the ray
        /// </summary>
        public static bool PointRay(Vector3 point, Ray ray)
        {
            return PointRay(point, ray.origin, ray.direction);
        }

        /// <summary>
        /// Tests if the point lies on the ray
        /// </summary>
        public static bool PointRay(Vector3 point, Vector3 rayOrigin, Vector3 rayDirection)
        {
            return Distance.PointRay(point, rayOrigin, rayDirection) < Geometry.Epsilon;
        }

        #endregion Point-Ray

        #region Point-Segment

        /// <summary>
        /// Tests if the point lies on the segment
        /// </summary>
        public static bool PointSegment(Vector3 point, Segment3 segment)
        {
            return PointSegment(point, segment.a, segment.b);
        }

        /// <summary>
        /// Tests if the point lies on the segment
        /// </summary>
        public static bool PointSegment(Vector3 point, Vector3 segmentA, Vector3 segmentB)
        {
            return Distance.PointSegment(point, segmentA, segmentB) < Geometry.Epsilon;
        }

        #endregion Point-Segment

        #region Point-Sphere

        /// <summary>
        /// Tests if the point is inside the sphere
        /// </summary>
        public static bool PointSphere(Vector3 point, Sphere sphere)
        {
            return PointSphere(point, sphere.center, sphere.radius);
        }

        /// <summary>
        /// Tests if the point is inside the sphere
        /// </summary>
        public static bool PointSphere(Vector3 point, Vector3 sphereCenter, float sphereRadius)
        {
            // For points on the sphere's surface magnitude is more stable than sqrMagnitude
            return (point - sphereCenter).magnitude < sphereRadius*sphereRadius + Geometry.Epsilon;
        }

        #endregion Point-Sphere

        #region Line-Line

        /// <summary>
        /// Computes an intersection of the lines
        /// </summary>
        public static bool LineLine(Line3 lineA, Line3 lineB)
        {
            Vector3 intersection;
            return LineLine(lineA.origin, lineA.direction, lineB.origin, lineB.direction, out intersection);
        }

        /// <summary>
        /// Computes an intersection of the lines
        /// </summary>
        public static bool LineLine(Line3 lineA, Line3 lineB, out Vector3 intersection)
        {
            return LineLine(lineA.origin, lineA.direction, lineB.origin, lineB.direction, out intersection);
        }

        /// <summary>
        /// Computes an intersection of the lines
        /// </summary>
        public static bool LineLine(Vector3 originA, Vector3 directionA, Vector3 originB, Vector3 directionB)
        {
            Vector3 intersection;
            return LineLine(originA, directionA, originB, directionB, out intersection);
        }

        /// <summary>
        /// Computes an intersection of the lines
        /// </summary>
        public static bool LineLine(Vector3 originA, Vector3 directionA, Vector3 originB, Vector3 directionB,
            out Vector3 intersection)
        {
            float sqrMagnitudeA = directionA.sqrMagnitude;
            float sqrMagnitudeB = directionB.sqrMagnitude;
            float dotAB = Vector3.Dot(directionA, directionB);

            float denominator = sqrMagnitudeA*sqrMagnitudeB - dotAB*dotAB;
            Vector3 originBToA = originA - originB;
            float a = Vector3.Dot(directionA, originBToA);
            float b = Vector3.Dot(directionB, originBToA);

            Vector3 closestPointA;
            Vector3 closestPointB;
            if (Mathf.Abs(denominator) < Geometry.Epsilon)
            {
                // Parallel
                float distanceB = dotAB > sqrMagnitudeB ? a/dotAB : b/sqrMagnitudeB;

                closestPointA = originA;
                closestPointB = originB + directionB*distanceB;
            }
            else
            {
                // Not parallel
                float distanceA = (sqrMagnitudeA*b - dotAB*a)/denominator;
                float distanceB = (dotAB*b - sqrMagnitudeB*a)/denominator;

                closestPointA = originA + directionA*distanceA;
                closestPointB = originB + directionB*distanceB;
            }

            if ((closestPointB - closestPointA).sqrMagnitude < Geometry.Epsilon)
            {
                intersection = closestPointA;
                return true;
            }
            intersection = Vector3.zero;
            return false;
        }

        #endregion Line-Line

        #region Line-Sphere

        /// <summary>
        /// Computes an intersection of the line and the sphere
        /// </summary>
        public static bool LineSphere(Line3 line, Sphere sphere)
        {
            IntersectionLineSphere intersection;
            return LineSphere(line.origin, line.direction, sphere.center, sphere.radius, out intersection);
        }

        /// <summary>
        /// Computes an intersection of the line and the sphere
        /// </summary>
        public static bool LineSphere(Line3 line, Sphere sphere, out IntersectionLineSphere intersection)
        {
            return LineSphere(line.origin, line.direction, sphere.center, sphere.radius, out intersection);
        }

        /// <summary>
        /// Computes an intersection of the line and the sphere
        /// </summary>
        public static bool LineSphere(Vector3 lineOrigin, Vector3 lineDirection, Vector3 sphereCenter, float sphereRadius)
        {
            IntersectionLineSphere intersection;
            return LineSphere(lineOrigin, lineDirection, sphereCenter, sphereRadius, out intersection);
        }

        /// <summary>
        /// Computes an intersection of the line and the sphere
        /// </summary>
        public static bool LineSphere(Vector3 lineOrigin, Vector3 lineDirection, Vector3 sphereCenter, float sphereRadius,
            out IntersectionLineSphere intersection)
        {
            Vector3 originToCenter = sphereCenter - lineOrigin;
            float centerProjection = Vector3.Dot(lineDirection, originToCenter);
            float sqrDistanceToLine = originToCenter.sqrMagnitude - centerProjection*centerProjection;

            float sqrDistanceToIntersection = sphereRadius*sphereRadius - sqrDistanceToLine;
            if (sqrDistanceToIntersection < -Geometry.Epsilon)
            {
                intersection = IntersectionLineSphere.None();
                return false;
            }
            if (sqrDistanceToIntersection < Geometry.Epsilon)
            {
                intersection = IntersectionLineSphere.Point(lineOrigin + lineDirection*centerProjection);
                return true;
            }

            float distanceToIntersection = Mathf.Sqrt(sqrDistanceToIntersection);
            float distanceA = centerProjection - distanceToIntersection;
            float distanceB = centerProjection + distanceToIntersection;

            Vector3 pointA = lineOrigin + lineDirection*distanceA;
            Vector3 pointB = lineOrigin + lineDirection*distanceB;
            intersection = IntersectionLineSphere.TwoPoints(pointA, pointB);
            return true;
        }

        #endregion Line-Sphere

        #region Ray-Sphere

        /// <summary>
        /// Computes an intersection of the ray and the sphere
        /// </summary>
        public static bool RaySphere(Ray ray, Sphere sphere)
        {
            IntersectionRaySphere intersection;
            return RaySphere(ray.origin, ray.direction, sphere.center, sphere.radius, out intersection);
        }

        /// <summary>
        /// Computes an intersection of the ray and the sphere
        /// </summary>
        public static bool RaySphere(Ray ray, Sphere sphere, out IntersectionRaySphere intersection)
        {
            return RaySphere(ray.origin, ray.direction, sphere.center, sphere.radius, out intersection);
        }

        /// <summary>
        /// Computes an intersection of the ray and the sphere
        /// </summary>
        public static bool RaySphere(Vector3 rayOrigin, Vector3 rayDirection, Vector3 sphereCenter, float sphereRadius)
        {
            IntersectionRaySphere intersection;
            return RaySphere(rayOrigin, rayDirection, sphereCenter, sphereRadius, out intersection);
        }

        /// <summary>
        /// Computes an intersection of the ray and the sphere
        /// </summary>
        public static bool RaySphere(Vector3 rayOrigin, Vector3 rayDirection, Vector3 sphereCenter, float sphereRadius,
            out IntersectionRaySphere intersection)
        {
            Vector3 originToCenter = sphereCenter - rayOrigin;
            float centerProjection = Vector3.Dot(rayDirection, originToCenter);
            if (centerProjection + sphereRadius < -Geometry.Epsilon)
            {
                intersection = IntersectionRaySphere.None();
                return false;
            }

            float sqrDistanceToLine = originToCenter.sqrMagnitude - centerProjection*centerProjection;
            float sqrDistanceToIntersection = sphereRadius*sphereRadius - sqrDistanceToLine;
            if (sqrDistanceToIntersection < -Geometry.Epsilon)
            {
                intersection = IntersectionRaySphere.None();
                return false;
            }
            if (sqrDistanceToIntersection < Geometry.Epsilon)
            {
                if (centerProjection < -Geometry.Epsilon)
                {
                    intersection = IntersectionRaySphere.None();
                    return false;
                }
                intersection = IntersectionRaySphere.Point(rayOrigin + rayDirection*centerProjection);
                return true;
            }

            // Line intersection
            float distanceToIntersection = Mathf.Sqrt(sqrDistanceToIntersection);
            float distanceA = centerProjection - distanceToIntersection;
            float distanceB = centerProjection + distanceToIntersection;

            if (distanceA < -Geometry.Epsilon)
            {
                if (distanceB < -Geometry.Epsilon)
                {
                    intersection = IntersectionRaySphere.None();
                    return false;
                }
                intersection = IntersectionRaySphere.Point(rayOrigin + rayDirection*distanceB);
                return true;
            }

            Vector3 pointA = rayOrigin + rayDirection*distanceA;
            Vector3 pointB = rayOrigin + rayDirection*distanceB;
            intersection = IntersectionRaySphere.TwoPoints(pointA, pointB);
            return true;
        }

        #endregion Ray-Sphere
    }
}
