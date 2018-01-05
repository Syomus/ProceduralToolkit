using UnityEngine;

namespace ProceduralToolkit
{
    /// <summary>
    /// Collection of basic computational geometry algorithms (distance, intersection, closest point, etc.)
    /// </summary>
    public static class Geometry
    {
        public const float Epsilon = 0.00001f;

        #region 2D

        #region Point-Line

        /// <summary>
        /// Returns a distance to the closest point on the line
        /// </summary>
        public static float DistanceToLine(Vector2 point, Ray2D line)
        {
            return Vector2.Distance(point, ClosestPointOnLine(point, line));
        }

        /// <summary>
        /// Returns a distance to the closest point on the line defined by <paramref name="origin"/> and <paramref name="direction"/>
        /// </summary>
        /// <param name="direction">Normalized direction of the line</param>
        public static float DistanceToLine(Vector2 point, Vector2 origin, Vector2 direction)
        {
            return Vector2.Distance(point, ClosestPointOnLine(point, origin, direction));
        }

        /// <summary>
        /// Projects the point onto the line
        /// </summary>
        public static Vector2 ClosestPointOnLine(Vector2 point, Ray2D line)
        {
            float projectedX;
            return ClosestPointOnLine(point, line.origin, line.direction, out projectedX);
        }

        /// <summary>
        /// Projects the point onto the line defined by <paramref name="origin"/> and <paramref name="direction"/>
        /// </summary>
        /// <param name="direction">Normalized direction of the line</param>
        public static Vector2 ClosestPointOnLine(Vector2 point, Vector2 origin, Vector2 direction)
        {
            float projectedX;
            return ClosestPointOnLine(point, origin, direction, out projectedX);
        }

        /// <summary>
        /// Projects the point onto the line defined by <paramref name="origin"/> and <paramref name="direction"/>
        /// </summary>
        /// <param name="direction">Normalized direction of the line</param>
        /// <param name="projectedX">Position of the projected point on the line relative to the origin</param>
        public static Vector2 ClosestPointOnLine(Vector2 point, Vector2 origin, Vector2 direction, out float projectedX)
        {
            Vector2 toPoint = point - origin;

            float dotDirection = Vector2.Dot(direction, direction);
            if (dotDirection < Epsilon)
            {
                Debug.LogError("Invalid line definition. origin: " + origin + " direction: " + direction);
                projectedX = 0;
                return origin;
            }

            projectedX = Vector2.Dot(toPoint, direction)/dotDirection;
            return origin + direction*projectedX;
        }

        #endregion Point-Line

        #region Point-Ray

        /// <summary>
        /// Returns a distance to the closest point on the ray
        /// </summary>
        public static float DistanceToRay(Vector2 point, Ray2D ray)
        {
            return Vector2.Distance(point, ClosestPointOnRay(point, ray));
        }

        /// <summary>
        /// Returns a distance to the closest point on the ray defined by <paramref name="origin"/> and <paramref name="direction"/>
        /// </summary>
        /// <param name="direction">Normalized direction of the ray</param>
        public static float DistanceToRay(Vector2 point, Vector2 origin, Vector2 direction)
        {
            return Vector2.Distance(point, ClosestPointOnRay(point, origin, direction));
        }

        /// <summary>
        /// Projects the point onto the ray
        /// </summary>
        public static Vector2 ClosestPointOnRay(Vector2 point, Ray2D ray)
        {
            float projectedX;
            return ClosestPointOnRay(point, ray.origin, ray.direction, out projectedX);
        }

        /// <summary>
        /// Projects the point onto the ray defined by <paramref name="origin"/> and <paramref name="direction"/>
        /// </summary>
        /// <param name="direction">Normalized direction of the ray</param>
        public static Vector2 ClosestPointOnRay(Vector2 point, Vector2 origin, Vector2 direction)
        {
            float projectedX;
            return ClosestPointOnRay(point, origin, direction, out projectedX);
        }

        /// <summary>
        /// Projects the point onto the ray defined by <paramref name="origin"/> and <paramref name="direction"/>
        /// </summary>
        /// <param name="direction">Normalized direction of the ray</param>
        /// <param name="projectedX">Position of the projected point on the ray relative to the origin</param>
        public static Vector2 ClosestPointOnRay(Vector2 point, Vector2 origin, Vector2 direction, out float projectedX)
        {
            Vector2 toPoint = point - origin;

            float dotDirection = Vector2.Dot(direction, direction);
            if (dotDirection < Epsilon)
            {
                Debug.LogError("Invalid ray definition. origin: " + origin + " direction: " + direction);
                projectedX = 0;
                return origin;
            }

            float dotToPoint = Vector2.Dot(toPoint, direction);
            if (dotToPoint <= 0)
            {
                projectedX = 0;
                return origin;
            }

            projectedX = dotToPoint/dotDirection;
            return origin + direction*projectedX;
        }

        #endregion Point-Ray

        #region Point-Segement

        /// <summary>
        /// Returns a distance to the closest point on the line segment defined by <paramref name="segmentA"/> and <paramref name="segmentB"/>
        /// </summary>
        public static float DistanceToSegment(Vector2 point, Vector2 segmentA, Vector2 segmentB)
        {
            return Vector2.Distance(point, ClosestPointOnSegment(point, segmentA, segmentB));
        }

        /// <summary>
        /// Projects the point onto the line segment defined by <paramref name="segmentA"/> and <paramref name="segmentB"/>
        /// </summary>
        public static Vector2 ClosestPointOnSegment(Vector2 point, Vector2 segmentA, Vector2 segmentB)
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
        public static Vector2 ClosestPointOnSegment(Vector2 point, Vector2 segmentA, Vector2 segmentB, out float projectedX)
        {
            Vector2 direction = segmentB - segmentA;
            Vector2 toPoint = point - segmentA;

            float dotDirection = Vector2.Dot(direction, direction);
            if (dotDirection < Epsilon)
            {
                Debug.LogError("Invalid segment definition. segmentA: " + segmentA + " segmentB: " + segmentB);
                projectedX = 0;
                return segmentA;
            }

            float dotToPoint = Vector2.Dot(toPoint, direction);
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

        #endregion Point-Segement

        #region Line-Line

        /// <summary>
        /// Computes an intersection of the lines
        /// </summary>
        public static bool IntersectLineLine(Ray2D lineA, Ray2D lineB, out Vector2 intersection)
        {
            return IntersectLineLine(lineA.origin, lineA.direction, lineB.origin, lineB.direction, out intersection);
        }

        /// <summary>
        /// Computes an intersection of the lines
        /// </summary>
        public static bool IntersectLineLine(Vector2 originA, Vector2 directionA, Vector2 originB, Vector2 directionB,
            out Vector2 intersection)
        {
            float denominator = VectorE.PerpDot(directionA, directionB);
            Vector2 originBToA = originA - originB;
            float a = VectorE.PerpDot(directionA, originBToA);
            float b = VectorE.PerpDot(directionB, originBToA);

            if (Mathf.Abs(denominator) < Epsilon)
            {
                // Parallel
                if (Mathf.Abs(a) > Epsilon || Mathf.Abs(b) > Epsilon)
                {
                    // Not collinear
                    intersection = Vector2.zero;
                    return false;
                }
                // Collinear
                intersection = originA;
                return true;
            }

            float distanceA = b/denominator;
            intersection = originA + distanceA*directionA;
            return true;
        }

        #endregion Line-Line

        #region Line-Circle

        /// <summary>
        /// Computes an intersection of the line and the circle
        /// </summary>
        public static bool IntersectLineCircle(Ray2D line, Vector2 center, float radius,
            out Vector2 pointA, out Vector2 pointB)
        {
            return IntersectLineCircle(line.origin, line.direction, center, radius, out pointA, out pointB);
        }

        /// <summary>
        /// Computes an intersection of the line and the circle
        /// </summary>
        public static bool IntersectLineCircle(Vector2 origin, Vector2 direction, Vector2 center, float radius,
            out Vector2 pointA, out Vector2 pointB)
        {
            Vector2 toCenter = center - origin;
            float toCenterOnLine = Vector2.Dot(toCenter, direction);
            float sqrDistanceToLine = toCenter.sqrMagnitude - toCenterOnLine*toCenterOnLine;

            float sqrRadius = radius*radius;
            if (sqrDistanceToLine > sqrRadius)
            {
                pointA = Vector2.zero;
                pointB = Vector2.zero;
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

        #endregion Line-Circle

        #region Ray-Circle

        /// <summary>
        /// Computes an intersection of the ray and the circle
        /// </summary>
        public static bool IntersectRayCircle(Ray2D ray, Vector2 center, float radius,
            out Vector2 pointA, out Vector2 pointB)
        {
            return IntersectRayCircle(ray.origin, ray.direction, center, radius, out pointA, out pointB);
        }

        /// <summary>
        /// Computes an intersection of the ray and the circle
        /// </summary>
        public static bool IntersectRayCircle(Vector2 origin, Vector2 direction, Vector2 center, float radius,
            out Vector2 pointA, out Vector2 pointB)
        {
            Vector2 toCenter = center - origin;
            float toCenterOnLine = Vector2.Dot(toCenter, direction);
            float sqrDistanceToLine = toCenter.sqrMagnitude - toCenterOnLine*toCenterOnLine;

            float sqrRadius = radius*radius;
            if (sqrDistanceToLine > sqrRadius)
            {
                pointA = Vector2.zero;
                pointB = Vector2.zero;
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
                    pointA = Vector2.zero;
                    pointB = Vector2.zero;
                    return false;
                }
            }

            pointA = origin + intersectionA*direction;
            pointB = origin + intersectionB*direction;
            return true;
        }

        #endregion Ray-Circle

        #endregion 2D

        #region 3D

        #region Point-Line

        /// <summary>
        /// Returns a distance to the closest point on the line
        /// </summary>
        public static float DistanceToLine(Vector3 point, Ray line)
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
        public static Vector3 ClosestPointOnLine(Vector3 point, Ray line)
        {
            float projectedX;
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

        #region Point-Segement

        /// <summary>
        /// Returns a distance to the closest point on the line segment defined by <paramref name="segmentA"/> and <paramref name="segmentB"/>
        /// </summary>
        public static float DistanceToSegment(Vector3 point, Vector3 segmentA, Vector3 segmentB)
        {
            return Vector3.Distance(point, ClosestPointOnSegment(point, segmentA, segmentB));
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

        #endregion Point-Segement

        #region Line-Line

        /// <summary>
        /// Computes an intersection of the lines
        /// </summary>
        public static bool IntersectLineLine(Ray lineA, Ray lineB, out Vector3 intersection)
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
        public static bool IntersectLineSphere(Ray line, Vector3 center, float radius,
            out Vector3 pointA, out Vector3 pointB)
        {
            return IntersectLineSphere(line.origin, line.direction, center, radius, out pointA, out pointB);
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
        public static bool IntersectRaySphere(Ray ray, Vector3 center, float radius,
            out Vector3 pointA, out Vector3 pointB)
        {
            return IntersectRaySphere(ray.origin, ray.direction, center, radius, out pointA, out pointB);
        }

        /// <summary>
        /// Computes an intersection of the ray and the sphere
        /// </summary>
        public static bool IntersectRaySphere(Vector3 origin, Vector3 direction, Vector3 center, float radius,
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

        #endregion 3D
    }
}
