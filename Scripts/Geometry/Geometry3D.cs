using UnityEngine;

namespace ProceduralToolkit
{
    /// <summary>
    /// Collection of basic computational geometry algorithms (distance, intersection, closest point, etc.)
    /// </summary>
    public static partial class Geometry
    {
        #region Point-Line

        /// <summary>
        /// Returns a distance to the closest point on the line
        /// </summary>
        public static float DistanceToLine(Vector3 point, Line3 line)
        {
            return Vector3.Distance(point, ClosestPointOnLine(point, line));
        }

        /// <summary>
        /// Returns a distance to the closest point on the line defined by <paramref name="origin"/> and <paramref name="direction"/>
        /// </summary>
        /// <param name="direction">Normalized direction of the line</param>
        public static float DistanceToLine(Vector3 point, Vector3 origin, Vector3 direction)
        {
            return Vector3.Distance(point, ClosestPointOnLine(point, origin, direction));
        }

        /// <summary>
        /// Projects the point onto the line
        /// </summary>
        public static Vector3 ClosestPointOnLine(Vector3 point, Line3 line)
        {
            float projectedX;
            return ClosestPointOnLine(point, line.origin, line.direction, out projectedX);
        }

        /// <summary>
        /// Projects the point onto the line
        /// </summary>
        /// <param name="projectedX">Position of the projected point on the line relative to the origin</param>
        public static Vector3 ClosestPointOnLine(Vector3 point, Line3 line, out float projectedX)
        {
            return ClosestPointOnLine(point, line.origin, line.direction, out projectedX);
        }

        /// <summary>
        /// Projects the point onto the line defined by <paramref name="origin"/> and <paramref name="direction"/>
        /// </summary>
        /// <param name="direction">Normalized direction of the line</param>
        public static Vector3 ClosestPointOnLine(Vector3 point, Vector3 origin, Vector3 direction)
        {
            float projectedX;
            return ClosestPointOnLine(point, origin, direction, out projectedX);
        }

        /// <summary>
        /// Projects the point onto the line defined by <paramref name="origin"/> and <paramref name="direction"/>
        /// </summary>
        /// <param name="direction">Normalized direction of the line</param>
        /// <param name="projectedX">Position of the projected point on the line relative to the origin</param>
        public static Vector3 ClosestPointOnLine(Vector3 point, Vector3 origin, Vector3 direction, out float projectedX)
        {
            Vector3 toPoint = point - origin;

            float dotDirection = Vector3.Dot(direction, direction);
            if (dotDirection < Epsilon)
            {
                Debug.LogError("Invalid line definition. origin: " + origin + " direction: " + direction);
                projectedX = 0;
                return origin;
            }

            projectedX = Vector3.Dot(toPoint, direction)/dotDirection;
            return origin + direction*projectedX;
        }

        #endregion Point-Line

        #region Point-Ray

        /// <summary>
        /// Returns a distance to the closest point on the ray
        /// </summary>
        public static float DistanceToRay(Vector3 point, Ray ray)
        {
            return Vector3.Distance(point, ClosestPointOnRay(point, ray));
        }

        /// <summary>
        /// Returns a distance to the closest point on the ray defined by <paramref name="origin"/> and <paramref name="direction"/>
        /// </summary>
        /// <param name="direction">Normalized direction of the ray</param>
        public static float DistanceToRay(Vector3 point, Vector3 origin, Vector3 direction)
        {
            return Vector3.Distance(point, ClosestPointOnRay(point, origin, direction));
        }

        /// <summary>
        /// Projects the point onto the ray
        /// </summary>
        public static Vector3 ClosestPointOnRay(Vector3 point, Ray ray)
        {
            float projectedX;
            return ClosestPointOnRay(point, ray.origin, ray.direction, out projectedX);
        }

        /// <summary>
        /// Projects the point onto the ray defined by <paramref name="origin"/> and <paramref name="direction"/>
        /// </summary>
        /// <param name="direction">Normalized direction of the ray</param>
        public static Vector3 ClosestPointOnRay(Vector3 point, Vector3 origin, Vector3 direction)
        {
            float projectedX;
            return ClosestPointOnRay(point, origin, direction, out projectedX);
        }

        /// <summary>
        /// Projects the point onto the ray defined by <paramref name="origin"/> and <paramref name="direction"/>
        /// </summary>
        /// <param name="direction">Normalized direction of the ray</param>
        /// <param name="projectedX">Position of the projected point on the ray relative to the origin</param>
        public static Vector3 ClosestPointOnRay(Vector3 point, Vector3 origin, Vector3 direction, out float projectedX)
        {
            Vector3 toPoint = point - origin;

            float dotDirection = Vector3.Dot(direction, direction);
            if (dotDirection < Epsilon)
            {
                Debug.LogError("Invalid ray definition. origin: " + origin + " direction: " + direction);
                projectedX = 0;
                return origin;
            }

            float dotToPoint = Vector3.Dot(toPoint, direction);
            if (dotToPoint <= 0)
            {
                projectedX = 0;
                return origin;
            }

            projectedX = dotToPoint/dotDirection;
            return origin + direction*projectedX;
        }

        #endregion Point-Ray

        #region Point-Segment

        /// <summary>
        /// Returns a distance to the closest point on the line segment
        /// </summary>
        public static float DistanceToSegment(Vector3 point, Segment3 segment)
        {
            return Vector3.Distance(point, ClosestPointOnSegment(point, segment));
        }

        /// <summary>
        /// Returns a distance to the closest point on the line segment defined by <paramref name="segmentA"/> and <paramref name="segmentB"/>
        /// </summary>
        public static float DistanceToSegment(Vector3 point, Vector3 segmentA, Vector3 segmentB)
        {
            return Vector3.Distance(point, ClosestPointOnSegment(point, segmentA, segmentB));
        }

        /// <summary>
        /// Projects the point onto the line segment
        /// </summary>
        public static Vector3 ClosestPointOnSegment(Vector3 point, Segment3 segment)
        {
            float projectedX;
            return ClosestPointOnSegment(point, segment.a, segment.b, out projectedX);
        }

        /// <summary>
        /// Projects the point onto the line segment
        /// </summary>
        /// <param name="projectedX">Normalized position of the projected point on the line segment. 
        /// Value of zero means that the projected point coincides with segment.a. 
        /// Value of one means that the projected point coincides with segment.b.</param>
        public static Vector3 ClosestPointOnSegment(Vector3 point, Segment3 segment, out float projectedX)
        {
            return ClosestPointOnSegment(point, segment.a, segment.b, out projectedX);
        }

        /// <summary>
        /// Projects the point onto the line segment defined by <paramref name="segmentA"/> and <paramref name="segmentB"/>
        /// </summary>
        public static Vector3 ClosestPointOnSegment(Vector3 point, Vector3 segmentA, Vector3 segmentB)
        {
            float projectedX;
            return ClosestPointOnSegment(point, segmentA, segmentB, out projectedX);
        }

        /// <summary>
        /// Projects the point onto the line segment defined by <paramref name="segmentA"/> and <paramref name="segmentB"/>
        /// </summary>
        /// <param name="projectedX">Normalized position of the projected point on the line segment. 
        /// Value of zero means that the projected point coincides with <paramref name="segmentA"/>. 
        /// Value of one means that the projected point coincides with <paramref name="segmentB"/>.</param>
        public static Vector3 ClosestPointOnSegment(Vector3 point, Vector3 segmentA, Vector3 segmentB, out float projectedX)
        {
            Vector3 direction = segmentB - segmentA;
            Vector3 toPoint = point - segmentA;

            float dotDirection = Vector3.Dot(direction, direction);
            if (dotDirection < Epsilon)
            {
                Debug.LogError("Invalid segment definition. segmentA: " + segmentA + " segmentB: " + segmentB);
                projectedX = 0;
                return segmentA;
            }

            float dotToPoint = Vector3.Dot(toPoint, direction);
            if (dotToPoint <= 0)
            {
                projectedX = 0;
                return segmentA;
            }

            if (dotToPoint >= dotDirection)
            {
                projectedX = 1;
                return segmentB;
            }

            projectedX = dotToPoint/dotDirection;
            return segmentA + direction*projectedX;
        }

        #endregion Point-Segment

        #region Point-Sphere

        /// <summary>
        /// Returns a distance to the closest point on the sphere
        /// </summary>
        /// <returns>Positive value if the point is outside, negative otherwise</returns>
        public static float DistanceToSphere(Vector3 point, Sphere sphere)
        {
            return DistanceToSphere(point, sphere.center, sphere.radius);
        }

        /// <summary>
        /// Returns a distance to the closest point on the sphere defined by <paramref name="center"/> and <paramref name="radius"/>
        /// </summary>
        /// <returns>Positive value if the point is outside, negative otherwise</returns>
        public static float DistanceToSphere(Vector3 point, Vector3 center, float radius)
        {
            return (center - point).magnitude - radius;
        }

        /// <summary>
        /// Projects the point onto the sphere
        /// </summary>
        public static Vector3 ClosestPointOnSphere(Vector3 point, Sphere sphere)
        {
            return ClosestPointOnSphere(point, sphere.center, sphere.radius);
        }

        /// <summary>
        /// Projects the point onto the sphere defined by <paramref name="center"/> and <paramref name="radius"/>
        /// </summary>
        public static Vector3 ClosestPointOnSphere(Vector3 point, Vector3 center, float radius)
        {
            return (point - center).normalized*radius;
        }

        /// <summary>
        /// Tests if the point is inside the sphere
        /// </summary>
        public static bool IntersectPointSphere(Vector3 point, Sphere sphere)
        {
            return IntersectPointSphere(point, sphere.center, sphere.radius);
        }

        /// <summary>
        /// Tests if the point is inside the sphere
        /// </summary>
        public static bool IntersectPointSphere(Vector3 point, Vector3 center, float radius)
        {
            return (point - center).sqrMagnitude < radius*radius;
        }

        #endregion Point-Sphere

        #region Line-Line

        /// <summary>
        /// Computes an intersection of the lines
        /// </summary>
        public static bool IntersectLineLine(Line3 lineA, Line3 lineB, out Vector3 intersection)
        {
            return IntersectLineLine(lineA.origin, lineA.direction, lineB.origin, lineB.direction, out intersection);
        }

        /// <summary>
        /// Computes an intersection of the lines
        /// </summary>
        public static bool IntersectLineLine(Vector3 originA, Vector3 directionA, Vector3 originB, Vector3 directionB,
            out Vector3 intersection)
        {
            float dotAA = Vector3.Dot(directionA, directionA);
            float dotBB = Vector3.Dot(directionB, directionB);
            float dotAB = Vector3.Dot(directionA, directionB);

            float denominator = dotAA*dotBB - dotAB*dotAB;
            Vector3 originBToA = originA - originB;
            float a = Vector3.Dot(directionA, originBToA);
            float b = Vector3.Dot(directionB, originBToA);

            Vector3 closestPointA;
            Vector3 closestPointB;
            if (Mathf.Abs(denominator) < Epsilon)
            {
                // Parallel
                float distanceB = dotAB > dotBB ? a/dotAB : b/dotBB;

                closestPointA = originA;
                closestPointB = originB + distanceB*directionB;
            }
            else
            {
                // Not parallel
                float distanceA = (dotAA*b - dotAB*a)/denominator;
                float distanceB = (dotAB*b - dotBB*a)/denominator;

                closestPointA = originA + distanceA*directionA;
                closestPointB = originB + distanceB*directionB;
            }

            if ((closestPointB - closestPointA).sqrMagnitude < Epsilon)
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
        public static bool IntersectLineSphere(Line3 line, Sphere sphere, out Vector3 pointA, out Vector3 pointB)
        {
            return IntersectLineSphere(line.origin, line.direction, sphere.center, sphere.radius, out pointA, out pointB);
        }

        /// <summary>
        /// Computes an intersection of the line and the sphere
        /// </summary>
        public static bool IntersectLineSphere(Vector3 origin, Vector3 direction, Vector3 center, float radius,
            out Vector3 pointA, out Vector3 pointB)
        {
            Vector3 toCenter = center - origin;
            float toCenterOnLine = Vector3.Dot(toCenter, direction);
            float sqrDistanceToLine = toCenter.sqrMagnitude - toCenterOnLine*toCenterOnLine;

            float sqrRadius = radius*radius;
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

            pointA = origin + intersectionA*direction;
            pointB = origin + intersectionB*direction;
            return true;
        }

        #endregion Line-Sphere

        #region Ray-Sphere

        /// <summary>
        /// Computes an intersection of the ray and the sphere
        /// </summary>
        public static bool IntersectRaySphere(Ray ray, Sphere sphere, out Vector3 pointA, out Vector3 pointB)
        {
            return IntersectRaySphere(ray.origin, ray.direction, sphere.center, sphere.radius, out pointA, out pointB);
        }

        /// <summary>
        /// Computes an intersection of the ray and the sphere
        /// </summary>
        public static bool IntersectRaySphere(Vector3 origin, Vector3 direction, Vector3 center, float radius, out Vector3 pointA, out Vector3 pointB)
        {
            Vector3 toCenter = center - origin;
            float toCenterOnLine = Vector3.Dot(toCenter, direction);
            float sqrDistanceToLine = toCenter.sqrMagnitude - toCenterOnLine*toCenterOnLine;

            float sqrRadius = radius*radius;
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

            pointA = origin + intersectionA*direction;
            pointB = origin + intersectionB*direction;
            return true;
        }

        #endregion Ray-Sphere
    }
}
