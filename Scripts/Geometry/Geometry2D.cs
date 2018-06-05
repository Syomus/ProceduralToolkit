using UnityEngine;

namespace ProceduralToolkit
{
    /// <summary>
    /// Collection of basic computational geometry algorithms
    /// </summary>
    public static partial class Geometry
    {
        public const float Epsilon = 0.00001f;
        public const float EpsilonSqr = Epsilon*Epsilon;

        #region Point-Line

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

        #endregion Point-Line

        #region Point-Ray

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

        #region Point-Segment

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

        #endregion Point-Segment

        #region Point-Circle

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

        #endregion Point-Circle

        #region Line-Line

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

        #endregion Line-Line

        #region Line-Ray

        /// <summary>
        /// Finds closest points on the line and the ray
        /// </summary>
        public static void ClosestPointsLineRay(Line2 line, Ray2D ray, out Vector2 linePoint, out Vector2 rayPoint)
        {
            ClosestPointsLineRay(line.origin, line.direction, ray.origin, ray.direction, out linePoint, out rayPoint);
        }

        /// <summary>
        /// Finds closest points on the line and the ray
        /// </summary>
        public static void ClosestPointsLineRay(Vector2 lineOrigin, Vector2 lineDirection, Vector2 rayOrigin, Vector2 rayDirection,
            out Vector2 linePoint, out Vector2 rayPoint)
        {
            Vector2 rayOriginToLineOrigin = lineOrigin - rayOrigin;
            float denominator = VectorE.PerpDot(lineDirection, rayDirection);
            float perpDotA = VectorE.PerpDot(lineDirection, rayOriginToLineOrigin);
            float perpDotB = VectorE.PerpDot(rayDirection, rayOriginToLineOrigin);

            if (Mathf.Abs(denominator) < Epsilon)
            {
                // Parallel
                if (Mathf.Abs(perpDotA) > Epsilon || Mathf.Abs(perpDotB) > Epsilon)
                {
                    // Not collinear
                    float dotA = Vector2.Dot(lineDirection, rayOriginToLineOrigin);
                    linePoint = lineOrigin - dotA*lineDirection;
                    rayPoint = rayOrigin;
                    return;
                }
                // Collinear
                linePoint = rayPoint = rayOrigin;
                return;
            }

            // Not parallel
            float lineDistance = perpDotB/denominator;
            float rayDistance = perpDotA/denominator;
            if (rayDistance < -Epsilon)
            {
                // No intersection
                linePoint = lineOrigin + lineDirection*lineDistance;
                rayPoint = rayOrigin;
                return;
            }
            // Point intersection
            linePoint = rayPoint = lineOrigin + lineDirection*lineDistance;
        }

        #endregion Line-Ray

        #region Ray-Ray

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

        #endregion Ray-Ray

        #region Circle-Circle

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

        #endregion Circle-Circle
    }
}
