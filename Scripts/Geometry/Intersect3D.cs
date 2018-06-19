using UnityEngine;

namespace ProceduralToolkit
{
    /// <summary>
    /// Collection of intersection algorithms
    /// </summary>
    public static partial class Intersect
    {
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
        public static bool PointSphere(Vector3 point, Vector3 center, float radius)
        {
            return (point - center).sqrMagnitude <= radius*radius;
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
            Vector3 pointA;
            Vector3 pointB;
            return LineSphere(line.origin, line.direction, sphere.center, sphere.radius, out pointA, out pointB);
        }

        /// <summary>
        /// Computes an intersection of the line and the sphere
        /// </summary>
        public static bool LineSphere(Line3 line, Sphere sphere, out Vector3 pointA, out Vector3 pointB)
        {
            return LineSphere(line.origin, line.direction, sphere.center, sphere.radius, out pointA, out pointB);
        }

        /// <summary>
        /// Computes an intersection of the line and the sphere
        /// </summary>
        public static bool LineSphere(Vector3 lineOrigin, Vector3 lineDirection, Vector3 sphereCenter, float sphereRadius)
        {
            Vector3 pointA;
            Vector3 pointB;
            return LineSphere(lineOrigin, lineDirection, sphereCenter, sphereRadius, out pointA, out pointB);
        }

        /// <summary>
        /// Computes an intersection of the line and the sphere
        /// </summary>
        public static bool LineSphere(Vector3 lineOrigin, Vector3 lineDirection, Vector3 sphereCenter, float sphereRadius,
            out Vector3 pointA, out Vector3 pointB)
        {
            Vector3 toCenter = sphereCenter - lineOrigin;
            float toCenterOnLine = Vector3.Dot(toCenter, lineDirection);
            float sqrDistanceToLine = toCenter.sqrMagnitude - toCenterOnLine*toCenterOnLine;

            float sqrRadius = sphereRadius*sphereRadius;
            if (sqrDistanceToLine > sqrRadius)
            {
                pointA = Vector3.zero;
                pointB = Vector3.zero;
                return false;
            }
            float fromClosestPointToIntersection = Mathf.Sqrt(sqrRadius - sqrDistanceToLine);
            float intersectionA = toCenterOnLine - fromClosestPointToIntersection;
            float intersectionB = toCenterOnLine + fromClosestPointToIntersection;

            if (intersectionA > intersectionB)
            {
                PTUtils.Swap(ref intersectionA, ref intersectionB);
            }

            pointA = lineOrigin + intersectionA*lineDirection;
            pointB = lineOrigin + intersectionB*lineDirection;
            return true;
        }

        #endregion Line-Sphere

        #region Ray-Sphere

        /// <summary>
        /// Computes an intersection of the ray and the sphere
        /// </summary>
        public static bool RaySphere(Ray ray, Sphere sphere)
        {
            Vector3 pointA;
            Vector3 pointB;
            return RaySphere(ray.origin, ray.direction, sphere.center, sphere.radius, out pointA, out pointB);
        }

        /// <summary>
        /// Computes an intersection of the ray and the sphere
        /// </summary>
        public static bool RaySphere(Ray ray, Sphere sphere, out Vector3 pointA, out Vector3 pointB)
        {
            return RaySphere(ray.origin, ray.direction, sphere.center, sphere.radius, out pointA, out pointB);
        }

        /// <summary>
        /// Computes an intersection of the ray and the sphere
        /// </summary>
        public static bool RaySphere(Vector3 rayOrigin, Vector3 rayDirection, Vector3 sphereCenter, float sphereRadius)
        {
            Vector3 pointA;
            Vector3 pointB;
            return RaySphere(rayOrigin, rayDirection, sphereCenter, sphereRadius, out pointA, out pointB);
        }

        /// <summary>
        /// Computes an intersection of the ray and the sphere
        /// </summary>
        public static bool RaySphere(Vector3 rayOrigin, Vector3 rayDirection, Vector3 sphereCenter, float sphereRadius, out Vector3 pointA,
            out Vector3 pointB)
        {
            Vector3 toCenter = sphereCenter - rayOrigin;
            float toCenterOnLine = Vector3.Dot(toCenter, rayDirection);
            float sqrDistanceToLine = toCenter.sqrMagnitude - toCenterOnLine*toCenterOnLine;

            float sqrRadius = sphereRadius*sphereRadius;
            if (sqrDistanceToLine > sqrRadius)
            {
                pointA = Vector3.zero;
                pointB = Vector3.zero;
                return false;
            }
            float fromClosestPointToIntersection = Mathf.Sqrt(sqrRadius - sqrDistanceToLine);
            float intersectionA = toCenterOnLine - fromClosestPointToIntersection;
            float intersectionB = toCenterOnLine + fromClosestPointToIntersection;

            if (intersectionA > intersectionB)
            {
                PTUtils.Swap(ref intersectionA, ref intersectionB);
            }

            if (intersectionA < 0)
            {
                intersectionA = intersectionB;
                if (intersectionA < 0)
                {
                    pointA = Vector3.zero;
                    pointB = Vector3.zero;
                    return false;
                }
            }

            pointA = rayOrigin + intersectionA*rayDirection;
            pointB = rayOrigin + intersectionB*rayDirection;
            return true;
        }

        #endregion Ray-Sphere
    }
}
