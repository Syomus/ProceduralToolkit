using UnityEngine;

namespace ProceduralToolkit
{
    /// <summary>
    /// Collection of basic computational geometry algorithms (distance, intersection, closest point, etc.)
    /// </summary>
    public static partial class Geometry
    {
        public const float Epsilon = 0.00001f;
        public const float EpsilonSqr = Epsilon*Epsilon;

        #region Point-Line

        /// <summary>
        /// Returns a distance to the closest point on the line
        /// </summary>
        public static float DistanceToLine(Vector2 point, Line2 line)
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
        public static Vector2 ClosestPointOnLine(Vector2 point, Line2 line)
        {
            float projectedX;
            return ClosestPointOnLine(point, line.origin, line.direction, out projectedX);
        }

        /// <summary>
        /// Projects the point onto the line
        /// </summary>
        /// <param name="projectedX">Position of the projected point on the line relative to the origin</param>
        public static Vector2 ClosestPointOnLine(Vector2 point, Line2 line, out float projectedX)
        {
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

        /// <summary>
        /// Tests if the point lies on the line
        /// </summary>
        public static bool IntersectPointLine(Vector2 point, Line2 line)
        {
            return IntersectPointLine(point, line.origin, line.direction);
        }

        /// <summary>
        /// Tests if the point lies on the line
        /// </summary>
        /// <param name="side">
        /// -1 if the point is to the left of the line,
        /// 0 if it is on the line,
        /// 1 if it is to the right of the line
        /// </param>
        public static bool IntersectPointLine(Vector2 point, Line2 line, out int side)
        {
            return IntersectPointLine(point, line.origin, line.direction, out side);
        }

        /// <summary>
        /// Tests if the point lies on the line
        /// </summary>
        public static bool IntersectPointLine(Vector2 point, Vector2 origin, Vector2 direction)
        {
            float perpDot = VectorE.PerpDot(point - origin, direction);
            return -Epsilon < perpDot && perpDot < Epsilon;
        }

        /// <summary>
        /// Tests if the point lies on the line
        /// </summary>
        /// <param name="side">
        /// -1 if the point is to the left of the line,
        /// 0 if it is on the line,
        /// 1 if it is to the right of the line
        /// </param>
        public static bool IntersectPointLine(Vector2 point, Vector2 origin, Vector2 direction, out int side)
        {
            float perpDot = VectorE.PerpDot(point - origin, direction);
            if (perpDot < -Epsilon)
            {
                side = -1;
                return false;
            }
            if (perpDot > Epsilon)
            {
                side = 1;
                return false;
            }
            side = 0;
            return true;
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

        /// <summary>
        /// Tests if the point lies on the ray
        /// </summary>
        public static bool IntersectPointRay(Vector2 point, Ray2D ray)
        {
            return IntersectPointRay(point, ray.origin, ray.direction);
        }

        /// <summary>
        /// Tests if the point lies on the ray
        /// </summary>
        /// <param name="side">
        /// -1 if the point is to the left of the ray,
        /// 0 if it is on the line,
        /// 1 if it is to the right of the ray
        /// </param>
        public static bool IntersectPointRay(Vector2 point, Ray2D ray, out int side)
        {
            return IntersectPointRay(point, ray.origin, ray.direction, out side);
        }

        /// <summary>
        /// Tests if the point lies on the ray
        /// </summary>
        public static bool IntersectPointRay(Vector2 point, Vector2 origin, Vector2 direction)
        {
            Vector2 toPoint = point - origin;
            float perpDot = VectorE.PerpDot(toPoint, direction);
            return -Epsilon < perpDot && perpDot < Epsilon &&
                   Vector2.Dot(toPoint, direction) > -Epsilon;
        }

        /// <summary>
        /// Tests if the point lies on the ray
        /// </summary>
        /// <param name="side">
        /// -1 if the point is to the left of the ray,
        /// 0 if it is on the line,
        /// 1 if it is to the right of the ray
        /// </param>
        public static bool IntersectPointRay(Vector2 point, Vector2 origin, Vector2 direction, out int side)
        {
            Vector2 toPoint = point - origin;
            float perpDot = VectorE.PerpDot(toPoint, direction);
            if (perpDot < -Epsilon)
            {
                side = -1;
                return false;
            }
            if (perpDot > Epsilon)
            {
                side = 1;
                return false;
            }
            side = 0;
            return Vector2.Dot(toPoint, direction) > -Epsilon;
        }

        #endregion Point-Ray

        #region Point-Segment

        /// <summary>
        /// Returns a distance to the closest point on the line segment
        /// </summary>
        public static float DistanceToSegment(Vector2 point, Segment2 segment)
        {
            return Vector2.Distance(point, ClosestPointOnSegment(point, segment));
        }

        /// <summary>
        /// Returns a distance to the closest point on the line segment defined by <paramref name="segmentA"/> and <paramref name="segmentB"/>
        /// </summary>
        public static float DistanceToSegment(Vector2 point, Vector2 segmentA, Vector2 segmentB)
        {
            return Vector2.Distance(point, ClosestPointOnSegment(point, segmentA, segmentB));
        }

        /// <summary>
        /// Projects the point onto the line segment
        /// </summary>
        public static Vector2 ClosestPointOnSegment(Vector2 point, Segment2 segment)
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
        public static Vector2 ClosestPointOnSegment(Vector2 point, Segment2 segment, out float projectedX)
        {
            return ClosestPointOnSegment(point, segment.a, segment.b, out projectedX);
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

        /// <summary>
        /// Tests if the point lies on the segment
        /// </summary>
        public static bool IntersectPointSegment(Vector2 point, Segment2 segment)
        {
            return IntersectPointSegment(point, segment.a, segment.b);
        }

        /// <summary>
        /// Tests if the point lies on the segment
        /// </summary>
        /// <param name="side">
        /// -1 if the point is to the left of the segment,
        /// 0 if it is on the line,
        /// 1 if it is to the right of the segment
        /// </param>
        public static bool IntersectPointSegment(Vector2 point, Segment2 segment, out int side)
        {
            return IntersectPointSegment(point, segment.a, segment.b, out side);
        }

        /// <summary>
        /// Tests if the point lies on the segment
        /// </summary>
        public static bool IntersectPointSegment(Vector2 point, Vector2 segmentA, Vector2 segmentB)
        {
            Vector2 direction = segmentB - segmentA;
            Vector2 toPoint = point - segmentA;
            float perpDot = VectorE.PerpDot(toPoint, direction);
            if (-Epsilon < perpDot && perpDot < Epsilon)
            {
                float dotToPoint = Vector2.Dot(toPoint, direction);
                return dotToPoint > -Epsilon &&
                       dotToPoint < Vector2.Dot(direction, direction) + Epsilon;
            }
            return false;
        }

        /// <summary>
        /// Tests if the point lies on the segment
        /// </summary>
        /// <param name="side">
        /// -1 if the point is to the left of the segment,
        /// 0 if it is on the line,
        /// 1 if it is to the right of the segment
        /// </param>
        public static bool IntersectPointSegment(Vector2 point, Vector2 segmentA, Vector2 segmentB, out int side)
        {
            Vector2 direction = segmentB - segmentA;
            Vector2 toPoint = point - segmentA;
            float perpDot = VectorE.PerpDot(toPoint, direction);
            if (perpDot < -Epsilon)
            {
                side = -1;
                return false;
            }
            if (perpDot > Epsilon)
            {
                side = 1;
                return false;
            }
            side = 0;
            float dotToPoint = Vector2.Dot(toPoint, direction);
            return dotToPoint > -Epsilon &&
                   dotToPoint < Vector2.Dot(direction, direction) + Epsilon;
        }

        #endregion Point-Segment

        #region Point-Circle

        /// <summary>
        /// Returns a distance to the closest point on the circle
        /// </summary>
        /// <returns>Positive value if the point is outside, negative otherwise</returns>
        public static float DistanceToCircle(Vector2 point, Circle circle)
        {
            return DistanceToCircle(point, circle.center, circle.radius);
        }

        /// <summary>
        /// Returns a distance to the closest point on the circle defined by <paramref name="center"/> and <paramref name="radius"/>
        /// </summary>
        /// <returns>Positive value if the point is outside, negative otherwise</returns>
        public static float DistanceToCircle(Vector2 point, Vector2 center, float radius)
        {
            return (center - point).magnitude - radius;
        }

        /// <summary>
        /// Projects the point onto the circle
        /// </summary>
        public static Vector2 ClosestPointOnCircle(Vector2 point, Circle circle)
        {
            return ClosestPointOnCircle(point, circle.center, circle.radius);
        }

        /// <summary>
        /// Projects the point onto the circle defined by <paramref name="center"/> and <paramref name="radius"/>
        /// </summary>
        public static Vector2 ClosestPointOnCircle(Vector2 point, Vector2 center, float radius)
        {
            return center + (point - center).normalized*radius;
        }

        /// <summary>
        /// Tests if the point is inside the circle
        /// </summary>
        public static bool IntersectPointCircle(Vector2 point, Circle circle)
        {
            return IntersectPointCircle(point, circle.center, circle.radius);
        }

        /// <summary>
        /// Tests if the point is inside the circle
        /// </summary>
        public static bool IntersectPointCircle(Vector2 point, Vector2 center, float radius)
        {
            return (point - center).sqrMagnitude <= radius*radius;
        }

        #endregion Point-Circle

        #region Line-Line

        /// <summary>
        /// Returns the distance between the closest points on the lines
        /// </summary>
        public static float DistanceToLine(Line2 lineA, Line2 lineB)
        {
            return DistanceToLine(lineA.origin, lineA.direction, lineB.origin, lineB.direction);
        }

        /// <summary>
        /// Returns the distance between the closest points on the lines defined by origin and direction
        /// </summary>
        public static float DistanceToLine(Vector2 originA, Vector2 directionA, Vector2 originB, Vector2 directionB)
        {
            if (Mathf.Abs(VectorE.PerpDot(directionA, directionB)) < Epsilon)
            {
                // Parallel
                Vector2 originBToA = originA - originB;
                if (Mathf.Abs(VectorE.PerpDot(directionA, originBToA)) > Epsilon ||
                    Mathf.Abs(VectorE.PerpDot(directionB, originBToA)) > Epsilon)
                {
                    // Not collinear
                    float dotA = Vector2.Dot(directionA, originBToA);
                    float distanceSqr = originBToA.sqrMagnitude - dotA*dotA;
                    return distanceSqr < 0 ? 0 : Mathf.Sqrt(distanceSqr);
                }

                // Collinear
                return 0;
            }

            // Not parallel
            return 0;
        }

        /// <summary>
        /// Finds closest points on the lines
        /// </summary>
        public static void ClosestPointsOnLines(Line2 lineA, Line2 lineB, out Vector2 pointA, out Vector2 pointB)
        {
            ClosestPointsOnLines(lineA.origin, lineA.direction, lineB.origin, lineB.direction, out pointA, out pointB);
        }

        /// <summary>
        /// Finds closest points on the lines
        /// </summary>
        public static void ClosestPointsOnLines(Vector2 originA, Vector2 directionA, Vector2 originB, Vector2 directionB,
            out Vector2 pointA, out Vector2 pointB)
        {
            Vector2 originBToA = originA - originB;
            float denominator = VectorE.PerpDot(directionA, directionB);
            float perpDotB = VectorE.PerpDot(directionB, originBToA);

            if (Mathf.Abs(denominator) < Epsilon)
            {
                // Parallel
                if (Mathf.Abs(perpDotB) > Epsilon ||
                    Mathf.Abs(VectorE.PerpDot(directionA, originBToA)) > Epsilon)
                {
                    // Not collinear
                    pointA = originA;
                    pointB = originB + directionB*Vector2.Dot(directionB, originBToA);
                    return;
                }

                // Collinear
                pointA = pointB = originA;
                return;
            }

            // Not parallel
            pointA = pointB = originA + directionA*(perpDotB/denominator);
        }

        /// <summary>
        /// Computes an intersection of the lines
        /// </summary>
        public static bool IntersectLineLine(Line2 lineA, Line2 lineB, out IntersectionLineLine2 intersection)
        {
            return IntersectLineLine(lineA.origin, lineA.direction, lineB.origin, lineB.direction, out intersection);
        }

        /// <summary>
        /// Computes an intersection of the lines
        /// </summary>
        public static bool IntersectLineLine(Vector2 originA, Vector2 directionA, Vector2 originB, Vector2 directionB,
            out IntersectionLineLine2 intersection)
        {
            Vector2 originBToA = originA - originB;
            float denominator = VectorE.PerpDot(directionA, directionB);
            float perpDotB = VectorE.PerpDot(directionB, originBToA);

            if (Mathf.Abs(denominator) < Epsilon)
            {
                // Parallel
                float perpDotA = VectorE.PerpDot(directionA, originBToA);
                if (Mathf.Abs(perpDotA) > Epsilon || Mathf.Abs(perpDotB) > Epsilon)
                {
                    // Not collinear
                    intersection = IntersectionLineLine2.None();
                    return false;
                }
                // Collinear
                intersection = IntersectionLineLine2.Line(originA);
                return true;
            }

            // Not parallel
            intersection = IntersectionLineLine2.Point(originA + directionA*(perpDotB/denominator));
            return true;
        }

        /// <summary>
        /// Computes an intersection of the lines
        /// </summary>
        public static IntersectionType IntersectLineLine(Line2 lineA, Line2 lineB, out float distanceA, out float distanceB)
        {
            return IntersectLineLine(lineA.origin, lineA.direction, lineB.origin, lineB.direction, out distanceA, out distanceB);
        }

        /// <summary>
        /// Computes an intersection of the lines
        /// </summary>
        public static IntersectionType IntersectLineLine(Vector2 originA, Vector2 directionA, Vector2 originB, Vector2 directionB,
            out float distanceA, out float distanceB)
        {
            Vector2 originBToA = originA - originB;
            float denominator = VectorE.PerpDot(directionA, directionB);
            float perpDotA = VectorE.PerpDot(directionA, originBToA);
            float perpDotB = VectorE.PerpDot(directionB, originBToA);

            if (Mathf.Abs(denominator) < Epsilon)
            {
                // Parallel
                if (Mathf.Abs(perpDotA) > Epsilon || Mathf.Abs(perpDotB) > Epsilon)
                {
                    // Not collinear
                    distanceA = 0;
                    distanceB = 0;
                    return IntersectionType.None;
                }
                // Collinear
                distanceA = 0;
                distanceB = 0;
                return IntersectionType.Line;
            }

            // Not parallel
            distanceA = perpDotB/denominator;
            distanceB = perpDotA/denominator;
            return IntersectionType.Point;
        }

        #endregion Line-Line

        #region Line-Ray

        /// <summary>
        /// Computes an intersection of the line and the ray
        /// </summary>
        public static bool IntersectLineRay(Line2 line, Ray2D ray, out IntersectionLineRay2 intersection)
        {
            return IntersectLineRay(line.origin, line.direction, ray.origin, ray.direction, out intersection);
        }

        /// <summary>
        /// Computes an intersection of the line and the ray
        /// </summary>
        public static bool IntersectLineRay(Vector2 lineOrigin, Vector2 lineDirection, Vector2 rayOrigin, Vector2 rayDirection,
            out IntersectionLineRay2 intersection)
        {
            float lineDistance;
            float rayDistance;
            var intersectionType = IntersectLineLine(lineOrigin, lineDirection, rayOrigin, rayDirection, out lineDistance, out rayDistance);
            if (intersectionType == IntersectionType.Line)
            {
                intersection = IntersectionLineRay2.Ray(rayOrigin);
                return true;
            }
            if (intersectionType == IntersectionType.Point && rayDistance > -Epsilon)
            {
                intersection = IntersectionLineRay2.Point(lineOrigin + lineDirection*lineDistance);
                return true;
            }

            intersection = IntersectionLineRay2.None();
            return false;
        }

        #endregion Line-Ray

        #region Line-Segment

        /// <summary>
        /// Computes an intersection of the line and the segment
        /// </summary>
        public static bool IntersectLineSegment(Line2 line, Segment2 segment, out IntersectionLineSegment2 intersection)
        {
            return IntersectLineSegment(line.origin, line.direction, segment.a, segment.b, out intersection);
        }

        /// <summary>
        /// Computes an intersection of the line and the segment
        /// </summary>
        public static bool IntersectLineSegment(Vector2 lineOrigin, Vector2 lineDirection, Vector2 segmentA, Vector2 segmentB,
            out IntersectionLineSegment2 intersection)
        {
            float lineDistance;
            float segmentDistance;
            Vector2 segmentDirection = segmentB - segmentA;
            var intersectionType = IntersectLineLine(lineOrigin, lineDirection, segmentA, segmentDirection, out lineDistance, out segmentDistance);
            if (intersectionType == IntersectionType.Line)
            {
                bool segmentIsAPoint = segmentDirection.sqrMagnitude < Epsilon;
                if (segmentIsAPoint)
                {
                    intersection = IntersectionLineSegment2.Point(segmentA);
                    return true;
                }

                bool codirected = Vector2.Dot(lineDirection, segmentB - segmentA) > 0;
                if (codirected)
                {
                    intersection = IntersectionLineSegment2.Segment(segmentA, segmentB);
                }
                else
                {
                    intersection = IntersectionLineSegment2.Segment(segmentB, segmentA);
                }
                return true;
            }
            if (intersectionType == IntersectionType.Point &&
                segmentDistance > -Epsilon && segmentDistance < 1 + Epsilon)
            {
                intersection = IntersectionLineSegment2.Point(lineOrigin + lineDirection*lineDistance);
                return true;
            }

            intersection = IntersectionLineSegment2.None();
            return false;
        }

        #endregion Line-Segment

        #region Line-Circle

        /// <summary>
        /// Computes an intersection of the line and the circle
        /// </summary>
        public static bool IntersectLineCircle(Line2 line, Circle circle, out Vector2 pointA, out Vector2 pointB)
        {
            return IntersectLineCircle(line.origin, line.direction, circle.center, circle.radius, out pointA, out pointB);
        }

        /// <summary>
        /// Computes an intersection of the line and the circle
        /// </summary>
        public static bool IntersectLineCircle(Vector2 origin, Vector2 direction, Vector2 center, float radius, out Vector2 pointA,
            out Vector2 pointB)
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

        #region Ray-Ray

        /// <summary>
        /// Returns the distance between the closest points on the rays
        /// </summary>
        public static float DistanceToRay(Ray2D rayA, Ray2D rayB)
        {
            return DistanceToRay(rayA.origin, rayA.direction, rayB.origin, rayB.direction);
        }

        /// <summary>
        /// Returns the distance between the closest points on the rays
        /// </summary>
        public static float DistanceToRay(Vector2 originA, Vector2 directionA, Vector2 originB, Vector2 directionB)
        {
            Vector2 originBToA = originA - originB;
            float denominator = VectorE.PerpDot(directionA, directionB);
            float perpDotA = VectorE.PerpDot(directionA, originBToA);
            float perpDotB = VectorE.PerpDot(directionB, originBToA);

            if (Mathf.Abs(denominator) < Epsilon)
            {
                // Parallel

                bool codirected = Vector2.Dot(directionA, directionB) > 0;
                float dotA = Vector2.Dot(directionA, originBToA);
                if (Mathf.Abs(perpDotA) > Epsilon || Mathf.Abs(perpDotB) > Epsilon)
                {
                    // Not collinear
                    if (!codirected && dotA > 0)
                    {
                        return Vector2.Distance(originA, originB);
                    }
                    float distanceSqr = originBToA.sqrMagnitude - dotA*dotA;
                    return distanceSqr < 0 ? 0 : Mathf.Sqrt(distanceSqr);
                }
                // Collinear

                if (codirected)
                {
                    // Ray intersection
                    return 0;
                }
                else
                {
                    if (dotA > 0)
                    {
                        // No intersection
                        return Vector2.Distance(originA, originB);
                    }
                    else
                    {
                        // Segment intersection
                        return 0;
                    }
                }
            }

            // The rays are skew and may intersect in a point
            float distanceA = perpDotB/denominator;
            float distanceB = perpDotA/denominator;
            bool intersectionNotOnA = distanceA < -Epsilon;
            bool intersectionNotOnB = distanceB < -Epsilon;
            if (intersectionNotOnA && intersectionNotOnB)
            {
                // No intersection
                return Vector2.Distance(originA, originB);
            }
            if (intersectionNotOnA)
            {
                // No intersection
                return Vector2.Distance(originA, originB + directionB*distanceB);
            }
            if (intersectionNotOnB)
            {
                // No intersection
                return Vector2.Distance(originB, originA + directionA*distanceA);
            }
            // Point intersection
            return 0;
        }

        /// <summary>
        /// Finds closest points on the rays
        /// </summary>
        public static void ClosestPointsOnRays(Ray2D rayA, Ray2D rayB, out Vector2 pointA, out Vector2 pointB)
        {
            ClosestPointsOnRays(rayA.origin, rayA.direction, rayB.origin, rayB.direction, out pointA, out pointB);
        }

        /// <summary>
        /// Finds closest points on the rays
        /// </summary>
        public static void ClosestPointsOnRays(Vector2 originA, Vector2 directionA, Vector2 originB, Vector2 directionB,
            out Vector2 pointA, out Vector2 pointB)
        {
            Vector2 originBToA = originA - originB;
            float denominator = VectorE.PerpDot(directionA, directionB);
            float perpDotA = VectorE.PerpDot(directionA, originBToA);
            float perpDotB = VectorE.PerpDot(directionB, originBToA);

            if (Mathf.Abs(denominator) < Epsilon)
            {
                // Parallel
                bool codirected = Vector2.Dot(directionA, directionB) > 0;
                float dotA = Vector2.Dot(directionA, originBToA);

                if (Mathf.Abs(perpDotA) > Epsilon || Mathf.Abs(perpDotB) > Epsilon)
                {
                    // Not collinear
                    if (codirected)
                    {
                        if (dotA > 0)
                        {
                            // Projection of originA is on rayB
                            pointA = originA;
                            pointB = originB + dotA*directionA;
                            return;
                        }
                        else
                        {
                            pointA = originA - dotA*directionA;
                            pointB = originB;
                            return;
                        }
                    }
                    else
                    {
                        if (dotA > 0)
                        {
                            pointA = originA;
                            pointB = originB;
                            return;
                        }
                        else
                        {
                            // Projection of originA is on rayB
                            pointA = originA;
                            pointB = originB + dotA*directionA;
                            return;
                        }
                    }
                }
                // Collinear

                if (codirected)
                {
                    // Ray intersection
                    if (dotA > 0)
                    {
                        // Projection of originA is on rayB
                        pointA = pointB = originA;
                        return;
                    }
                    else
                    {
                        pointA = pointB = originB;
                        return;
                    }
                }
                else
                {
                    if (dotA > 0)
                    {
                        // No intersection
                        pointA = originA;
                        pointB = originB;
                        return;
                    }
                    else
                    {
                        // Segment intersection
                        pointA = pointB = originA;
                        return;
                    }
                }
            }

            // The rays are skew and may intersect in a point
            float distanceA = perpDotB/denominator;
            float distanceB = perpDotA/denominator;
            bool intersectionNotOnA = distanceA < -Epsilon;
            bool intersectionNotOnB = distanceB < -Epsilon;
            if (intersectionNotOnA && intersectionNotOnB)
            {
                // No intersection
                pointA = originA;
                pointB = originB;
                return;
            }
            if (intersectionNotOnA)
            {
                // No intersection
                pointA = originA;
                pointB = originB + directionB*distanceB;
                return;
            }
            if (intersectionNotOnB)
            {
                // No intersection
                pointA = originA + directionA*distanceA;
                pointB = originB;
                return;
            }
            // Point intersection
            pointA = pointB = originA + directionA*distanceA;
        }

        /// <summary>
        /// Computes an intersection of the rays
        /// </summary>
        public static bool IntersectRayRay(Ray2D rayA, Ray2D rayB, out IntersectionRayRay2 intersection)
        {
            return IntersectRayRay(rayA.origin, rayA.direction, rayB.origin, rayB.direction, out intersection);
        }

        /// <summary>
        /// Computes an intersection of the rays
        /// </summary>
        public static bool IntersectRayRay(Vector2 originA, Vector2 directionA, Vector2 originB, Vector2 directionB,
            out IntersectionRayRay2 intersection)
        {
            Vector2 originBToA = originA - originB;
            float denominator = VectorE.PerpDot(directionA, directionB);
            float perpDotA = VectorE.PerpDot(directionA, originBToA);
            float perpDotB = VectorE.PerpDot(directionB, originBToA);

            if (Mathf.Abs(denominator) < Epsilon)
            {
                // Parallel
                if (Mathf.Abs(perpDotA) > Epsilon || Mathf.Abs(perpDotB) > Epsilon)
                {
                    // Not collinear
                    intersection = IntersectionRayRay2.None();
                    return false;
                }
                // Collinear

                bool codirected = Vector2.Dot(directionA, directionB) > 0;
                float dotA = Vector2.Dot(directionA, originBToA);
                if (codirected)
                {
                    intersection = IntersectionRayRay2.Ray(dotA > 0 ? originA : originB, directionA);
                    return true;
                }
                else
                {
                    if (dotA > 0)
                    {
                        intersection = IntersectionRayRay2.None();
                        return false;
                    }
                    else
                    {
                        intersection = IntersectionRayRay2.Segment(originA, originB);
                        return true;
                    }
                }
            }

            // The rays are skew and may intersect in a point
            float distanceA = perpDotB/denominator;
            if (distanceA < -Epsilon)
            {
                intersection = IntersectionRayRay2.None();
                return false;
            }

            float distanceB = perpDotA/denominator;
            if (distanceB < -Epsilon)
            {
                intersection = IntersectionRayRay2.None();
                return false;
            }

            intersection = IntersectionRayRay2.Point(originA + directionA*distanceA);
            return true;
        }

        #endregion Ray-Ray

        #region Ray-Segment

        /// <summary>
        /// Computes an intersection of the ray and the segment
        /// </summary>
        public static bool IntersectRaySegment(Ray2D ray, Segment2 segment, out IntersectionRaySegment2 intersection)
        {
            return IntersectRaySegment(ray.origin, ray.direction, segment.a, segment.b, out intersection);
        }

        /// <summary>
        /// Computes an intersection of the ray and the segment
        /// </summary>
        public static bool IntersectRaySegment(Vector2 rayOrigin, Vector2 rayDirection, Vector2 segmentA, Vector2 segmentB,
            out IntersectionRaySegment2 intersection)
        {
            float rayDistance;
            float segmentDistance;
            Vector2 segmentDirection = segmentB - segmentA;
            var intersectionType = IntersectLineLine(rayOrigin, rayDirection, segmentA, segmentDirection, out rayDistance, out segmentDistance);
            if (intersectionType == IntersectionType.Line)
            {
                bool segmentIsAPoint = segmentDirection.sqrMagnitude < Epsilon;
                float projectionFromOriginToA = Vector2.Dot(rayDirection, segmentA - rayOrigin);
                if (segmentIsAPoint)
                {
                    if (projectionFromOriginToA > -Epsilon)
                    {
                        intersection = IntersectionRaySegment2.Point(segmentA);
                        return true;
                    }
                    intersection = IntersectionRaySegment2.None();
                    return false;
                }

                float projectionFromOriginToB = Vector2.Dot(rayDirection, segmentB - rayOrigin);
                if (projectionFromOriginToA > -Epsilon)
                {
                    if (projectionFromOriginToB > -Epsilon)
                    {
                        if (projectionFromOriginToB > projectionFromOriginToA)
                        {
                            intersection = IntersectionRaySegment2.Segment(segmentA, segmentB);
                        }
                        else
                        {
                            intersection = IntersectionRaySegment2.Segment(segmentB, segmentA);
                        }
                    }
                    else
                    {
                        if (projectionFromOriginToA > Epsilon)
                        {
                            intersection = IntersectionRaySegment2.Segment(rayOrigin, segmentA);
                        }
                        else
                        {
                            intersection = IntersectionRaySegment2.Point(rayOrigin);
                        }
                    }
                    return true;
                }
                if (projectionFromOriginToB > -Epsilon)
                {
                    if (projectionFromOriginToB > Epsilon)
                    {
                        intersection = IntersectionRaySegment2.Segment(rayOrigin, segmentB);
                    }
                    else
                    {
                        intersection = IntersectionRaySegment2.Point(rayOrigin);
                    }
                    return true;
                }
                intersection = IntersectionRaySegment2.None();
                return false;
            }
            if (intersectionType == IntersectionType.Point &&
                rayDistance > -Epsilon &&
                segmentDistance > -Epsilon && segmentDistance < 1 + Epsilon)
            {
                intersection = IntersectionRaySegment2.Point(rayOrigin + rayDirection*rayDistance);
                return true;
            }

            intersection = IntersectionRaySegment2.None();
            return false;
        }

        #endregion Ray-Segment

        #region Ray-Circle

        /// <summary>
        /// Computes an intersection of the ray and the circle
        /// </summary>
        public static bool IntersectRayCircle(Ray2D ray, Circle circle, out Vector2 pointA, out Vector2 pointB)
        {
            return IntersectRayCircle(ray.origin, ray.direction, circle.center, circle.radius, out pointA, out pointB);
        }

        /// <summary>
        /// Computes an intersection of the ray and the circle
        /// </summary>
        public static bool IntersectRayCircle(Vector2 origin, Vector2 direction, Vector2 center, float radius, out Vector2 pointA, out Vector2 pointB)
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

        #region Segment-Segment

        /// <summary>
        /// Computes an intersection of the segments
        /// </summary>
        public static bool IntersectSegmentSegment(Segment2 segment1, Segment2 segment2, out IntersectionSegmentSegment2 intersection)
        {
            return IntersectSegmentSegment(segment1.a, segment1.b, segment2.a, segment2.b, out intersection);
        }

        /// <summary>
        /// Computes an intersection of the segments
        /// </summary>
        public static bool IntersectSegmentSegment(Vector2 segment1A, Vector2 segment1B, Vector2 segment2A, Vector2 segment2B,
            out IntersectionSegmentSegment2 intersection)
        {
            Vector2 from2ATo1A = segment1A - segment2A;
            Vector2 direction1 = segment1B - segment1A;
            Vector2 direction2 = segment2B - segment2A;
            float denominator = VectorE.PerpDot(direction1, direction2);
            float perpDot1 = VectorE.PerpDot(direction1, from2ATo1A);
            float perpDot2 = VectorE.PerpDot(direction2, from2ATo1A);

            if (Mathf.Abs(denominator) < Epsilon)
            {
                // Parallel
                if (Mathf.Abs(perpDot1) > Epsilon || Mathf.Abs(perpDot2) > Epsilon)
                {
                    // Not collinear
                    intersection = IntersectionSegmentSegment2.None();
                    return false;
                }
                // Collinear or degenerate

                bool segment1IsAPoint = direction1.sqrMagnitude < Epsilon;
                bool segment2IsAPoint = direction2.sqrMagnitude < Epsilon;
                if (segment1IsAPoint && segment2IsAPoint)
                {
                    if (segment1A == segment2A)
                    {
                        intersection = IntersectionSegmentSegment2.Point(segment1A);
                        return true;
                    }
                    intersection = IntersectionSegmentSegment2.None();
                    return false;
                }
                if (segment1IsAPoint)
                {
                    if (CollinearPointInSegment(segment2A, segment2B, point: segment1A))
                    {
                        intersection = IntersectionSegmentSegment2.Point(segment1A);
                        return true;
                    }
                    intersection = IntersectionSegmentSegment2.None();
                    return false;
                }
                if (segment2IsAPoint)
                {
                    if (CollinearPointInSegment(segment1A, segment1B, point: segment2A))
                    {
                        intersection = IntersectionSegmentSegment2.Point(segment2A);
                        return true;
                    }
                    intersection = IntersectionSegmentSegment2.None();
                    return false;
                }

                bool codirected = Vector2.Dot(direction1, direction2) > 0;
                if (codirected)
                {
                    // Codirected
                    float projectionFrom2ATo1A = Vector2.Dot(direction1, from2ATo1A);
                    if (projectionFrom2ATo1A > 0)
                    {
                        // 2A------2B
                        //     1A------1B
                        return IntersectSegmentSegmentCollinear(segment2A, segment2B, segment1A, segment1B, out intersection);
                    }
                    else
                    {
                        // 1A------1B
                        //     2A------2B
                        return IntersectSegmentSegmentCollinear(segment1A, segment1B, segment2A, segment2B, out intersection);
                    }
                }
                else
                {
                    // Contradirected
                    float projectionFrom1ATo2B = Vector2.Dot(direction1, segment2B - segment1A);
                    if (projectionFrom1ATo2B > 0)
                    {
                        // 1A------1B
                        //     2B------2A
                        return IntersectSegmentSegmentCollinear(segment1A, segment1B, segment2B, segment2A, out intersection);
                    }
                    else
                    {
                        // 2B------2A
                        //     1A------1B
                        return IntersectSegmentSegmentCollinear(segment2B, segment2A, segment1A, segment1B, out intersection);
                    }
                }
            }

            // The segments are skew
            float distance1 = perpDot2/denominator;
            if (distance1 < -Epsilon || distance1 > 1 + Epsilon)
            {
                intersection = IntersectionSegmentSegment2.None();
                return false;
            }

            float distance2 = perpDot1/denominator;
            if (distance2 < -Epsilon || distance2 > 1 + Epsilon)
            {
                intersection = IntersectionSegmentSegment2.None();
                return false;
            }

            intersection = IntersectionSegmentSegment2.Point(segment1A + direction1*distance1);
            return true;
        }

        private static bool CollinearPointInSegment(Vector2 segmentA, Vector2 segmentB, Vector2 point)
        {
            if (Mathf.Abs(segmentA.x - segmentB.x) < Epsilon)
            {
                // Segment is vertical
                if (segmentA.y <= point.y && point.y <= segmentB.y)
                {
                    return true;
                }
                if (segmentA.y >= point.y && point.y >= segmentB.y)
                {
                    return true;
                }
            }
            else
            {
                // Segment is not vertical
                if (segmentA.x <= point.x && point.x <= segmentB.x)
                {
                    return true;
                }
                if (segmentA.x >= point.x && point.x >= segmentB.x)
                {
                    return true;
                }
            }
            return false;
        }

        private static bool IntersectSegmentSegmentCollinear(Vector2 leftA, Vector2 leftB, Vector2 rightA, Vector2 rightB,
            out IntersectionSegmentSegment2 intersection)
        {
            Vector2 leftDirection = leftB - leftA;
            float projectionRA = Vector2.Dot(leftDirection, leftB - rightA);
            if (Mathf.Abs(projectionRA) < Epsilon)
            {
                // LB == RA
                // LA------LB
                //         RA------RB
                intersection = IntersectionSegmentSegment2.Point(leftB);
                return true;
            }
            if (projectionRA > 0)
            {
                // LB > RA
                // LA------LB
                //     RARB
                //     RA--RB
                //     RA------RB
                Vector2 pointB;
                float projectionRB = Vector2.Dot(leftDirection, rightB - leftA);
                if (projectionRB > leftDirection.sqrMagnitude)
                {
                    pointB = leftB;
                }
                else
                {
                    pointB = rightB;
                }
                intersection = IntersectionSegmentSegment2.Segment(rightA, pointB);
                return true;
            }
            // LB < RA
            // LA------LB
            //             RA------RB
            intersection = IntersectionSegmentSegment2.None();
            return false;
        }

        #endregion Segment-Segment

        #region Circle-Circle

        /// <summary>
        /// Returns the distance between the closest points on the circles
        /// </summary>
        /// <returns>
        /// Positive value if the circles do not intersect, negative otherwise.
        /// Negative value can be interpreted as depth of penetration.
        /// </returns>
        public static float DistanceToCircle(Circle circleA, Circle circleB)
        {
            return DistanceToCircle(circleA.center, circleA.radius, circleB.center, circleB.radius);
        }

        /// <summary>
        /// Returns the distance between the closest points on the circles
        /// </summary>
        /// <returns>
        /// Positive value if the circles do not intersect, negative otherwise.
        /// Negative value can be interpreted as depth of penetration.
        /// </returns>
        public static float DistanceToCircle(Vector2 centerA, float radiusA, Vector2 centerB, float radiusB)
        {
            return Vector2.Distance(centerA, centerB) - radiusA - radiusB;
        }

        /// <summary>
        /// Finds closest points on the circles
        /// </summary>
        public static void ClosestPointsOnCircles(Circle circleA, Circle circleB, out Vector2 pointA, out Vector2 pointB)
        {
            ClosestPointsOnCircles(circleA.center, circleA.radius, circleB.center, circleB.radius, out pointA, out pointB);
        }

        /// <summary>
        /// Finds closest points on the circles
        /// </summary>
        public static void ClosestPointsOnCircles(Vector2 centerA, float radiusA, Vector2 centerB, float radiusB,
            out Vector2 pointA, out Vector2 pointB)
        {
            Vector2 aToB = (centerB - centerA).normalized;
            pointA = centerA + aToB*radiusA;
            pointB = centerB - aToB*radiusB;
        }

        /// <summary>
        /// Computes an intersection of the circles
        /// </summary>
        /// <returns>True if the circles intersect or one circle is contained within the other</returns>
        public static bool IntersectCircleCircle(Circle circleA, Circle circleB, out IntersectionCircleCircle intersection)
        {
            return IntersectCircleCircle(circleA.center, circleA.radius, circleB.center, circleB.radius, out intersection);
        }

        /// <summary>
        /// Computes an intersection of the circles
        /// </summary>
        /// <returns>True if the circles intersect or one circle is contained within the other</returns>
        public static bool IntersectCircleCircle(Vector2 centerA, float radiusA, Vector2 centerB, float radiusB,
            out IntersectionCircleCircle intersection)
        {
            Vector2 fromBtoA = centerA - centerB;
            float distanceFromBtoASqr = fromBtoA.sqrMagnitude;
            if (distanceFromBtoASqr < EpsilonSqr)
            {
                if (Mathf.Abs(radiusA - radiusB) < Epsilon)
                {
                    // Circles are coincident
                    intersection = IntersectionCircleCircle.Circle();
                    return true;
                }
                // One circle is inside the other
                intersection = IntersectionCircleCircle.None();
                return true;
            }

            float sumOfRadii = radiusA + radiusB;
            float sumOfRadiiSqr = sumOfRadii*sumOfRadii;
            if (distanceFromBtoASqr > sumOfRadiiSqr)
            {
                // No intersections, circles are separate
                intersection = IntersectionCircleCircle.None();
                return false;
            }
            if (Mathf.Abs(distanceFromBtoASqr - sumOfRadiiSqr) < EpsilonSqr)
            {
                // One intersection outside
                intersection = IntersectionCircleCircle.Point(centerB + fromBtoA*(radiusB/sumOfRadii));
                return true;
            }

            float differenceOfRadii = radiusA - radiusB;
            float differenceOfRadiiSqr = differenceOfRadii*differenceOfRadii;
            if (distanceFromBtoASqr < differenceOfRadiiSqr)
            {
                // One circle is contained within the other
                intersection = IntersectionCircleCircle.None();
                return true;
            }
            if (Mathf.Abs(distanceFromBtoASqr - differenceOfRadiiSqr) < EpsilonSqr)
            {
                // One intersection inside
                intersection = IntersectionCircleCircle.Point(centerB - fromBtoA*(radiusB/differenceOfRadii));
                return true;
            }

            // Two intersections
            float radiusASqr = radiusA*radiusA;
            float distanceToMiddle = 0.5f*(radiusASqr - radiusB*radiusB)/distanceFromBtoASqr + 0.5f;
            Vector2 middle = centerA - fromBtoA*distanceToMiddle;

            float discriminant = radiusASqr/distanceFromBtoASqr - distanceToMiddle*distanceToMiddle;
            if (discriminant < 0)
            {
                discriminant = 0;
            }
            Vector2 offset = fromBtoA.Perp()*Mathf.Sqrt(discriminant);

            intersection = IntersectionCircleCircle.TwoPoints(middle + offset, middle - offset);
            return true;
        }

        #endregion Circle-Circle
    }
}
