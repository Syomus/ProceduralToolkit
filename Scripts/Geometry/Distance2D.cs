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
        public static float PointLine(Vector2 point, Line2 line)
        {
            return Vector2.Distance(point, Geometry.ClosestPointOnLine(point, line));
        }

        /// <summary>
        /// Returns a distance to the closest point on the line
        /// </summary>
        public static float PointLine(Vector2 point, Vector2 lineOrigin, Vector2 lineDirection)
        {
            return Vector2.Distance(point, Geometry.ClosestPointOnLine(point, lineOrigin, lineDirection));
        }

        #endregion Point-Line

        #region Point-Ray

        /// <summary>
        /// Returns a distance to the closest point on the ray
        /// </summary>
        public static float PointRay(Vector2 point, Ray2D ray)
        {
            return Vector2.Distance(point, Geometry.ClosestPointOnRay(point, ray));
        }

        /// <summary>
        /// Returns a distance to the closest point on the ray
        /// </summary>
        /// <param name="rayDirection">Normalized direction of the ray</param>
        public static float PointRay(Vector2 point, Vector2 rayOrigin, Vector2 rayDirection)
        {
            return Vector2.Distance(point, Geometry.ClosestPointOnRay(point, rayOrigin, rayDirection));
        }

        #endregion Point-Ray

        #region Point-Segment

        /// <summary>
        /// Returns a distance to the closest point on the line segment
        /// </summary>
        public static float PointSegment(Vector2 point, Segment2 segment)
        {
            return Vector2.Distance(point, Geometry.ClosestPointOnSegment(point, segment));
        }

        /// <summary>
        /// Returns a distance to the closest point on the line segment
        /// </summary>
        public static float PointSegment(Vector2 point, Vector2 segmentA, Vector2 segmentB)
        {
            return Vector2.Distance(point, Geometry.ClosestPointOnSegment(point, segmentA, segmentB));
        }

        #endregion Point-Segment

        #region Point-Circle

        /// <summary>
        /// Returns a distance to the closest point on the circle
        /// </summary>
        /// <returns>Positive value if the point is outside, negative otherwise</returns>
        public static float PointCircle(Vector2 point, Circle circle)
        {
            return PointCircle(point, circle.center, circle.radius);
        }

        /// <summary>
        /// Returns a distance to the closest point on the circle
        /// </summary>
        /// <returns>Positive value if the point is outside, negative otherwise</returns>
        public static float PointCircle(Vector2 point, Vector2 circleCenter, float circleRadius)
        {
            return (circleCenter - point).magnitude - circleRadius;
        }

        #endregion Point-Circle

        #region Line-Line

        /// <summary>
        /// Returns the distance between the closest points on the lines
        /// </summary>
        public static float LineLine(Line2 lineA, Line2 lineB)
        {
            return LineLine(lineA.origin, lineA.direction, lineB.origin, lineB.direction);
        }

        /// <summary>
        /// Returns the distance between the closest points on the lines
        /// </summary>
        public static float LineLine(Vector2 originA, Vector2 directionA, Vector2 originB, Vector2 directionB)
        {
            if (Mathf.Abs(VectorE.PerpDot(directionA, directionB)) < Geometry.Epsilon)
            {
                // Parallel
                Vector2 originBToA = originA - originB;
                if (Mathf.Abs(VectorE.PerpDot(directionA, originBToA)) > Geometry.Epsilon ||
                    Mathf.Abs(VectorE.PerpDot(directionB, originBToA)) > Geometry.Epsilon)
                {
                    // Not collinear
                    float originBProjection = Vector2.Dot(directionA, originBToA);
                    float distanceSqr = originBToA.sqrMagnitude - originBProjection*originBProjection;
                    // distanceSqr can be negative
                    return distanceSqr <= 0 ? 0 : Mathf.Sqrt(distanceSqr);
                }

                // Collinear
                return 0;
            }

            // Not parallel
            return 0;
        }

        #endregion Line-Line

        #region Line-Ray

        /// <summary>
        /// Returns the distance between the closest points on the line and the ray
        /// </summary>
        public static float LineRay(Line2 line, Ray2D ray)
        {
            return LineRay(line.origin, line.direction, ray.origin, ray.direction);
        }

        /// <summary>
        /// Returns the distance between the closest points on the line and the ray
        /// </summary>
        public static float LineRay(Vector2 lineOrigin, Vector2 lineDirection, Vector2 rayOrigin, Vector2 rayDirection)
        {
            Vector2 rayOriginToLineOrigin = lineOrigin - rayOrigin;
            float denominator = VectorE.PerpDot(lineDirection, rayDirection);
            float perpDotA = VectorE.PerpDot(lineDirection, rayOriginToLineOrigin);

            if (Mathf.Abs(denominator) < Geometry.Epsilon)
            {
                float perpDotB = VectorE.PerpDot(rayDirection, rayOriginToLineOrigin);
                // Parallel
                if (Mathf.Abs(perpDotA) > Geometry.Epsilon || Mathf.Abs(perpDotB) > Geometry.Epsilon)
                {
                    // Not collinear
                    float rayOriginProjection = Vector2.Dot(lineDirection, rayOriginToLineOrigin);
                    float distanceSqr = rayOriginToLineOrigin.sqrMagnitude - rayOriginProjection*rayOriginProjection;
                    // distanceSqr can be negative
                    return distanceSqr <= 0 ? 0 : Mathf.Sqrt(distanceSqr);
                }
                // Collinear
                return 0;
            }

            // Not parallel
            float rayDistance = perpDotA/denominator;
            if (rayDistance < -Geometry.Epsilon)
            {
                // No intersection
                float rayOriginProjection = Vector2.Dot(lineDirection, rayOriginToLineOrigin);
                Vector2 linePoint = lineOrigin - lineDirection*rayOriginProjection;
                return Vector2.Distance(rayOrigin, linePoint);
            }
            // Point intersection
            return 0;
        }

        #endregion Line-Ray

        #region Ray-Ray

        /// <summary>
        /// Returns the distance between the closest points on the rays
        /// </summary>
        public static float RayRay(Ray2D rayA, Ray2D rayB)
        {
            return RayRay(rayA.origin, rayA.direction, rayB.origin, rayB.direction);
        }

        /// <summary>
        /// Returns the distance between the closest points on the rays
        /// </summary>
        public static float RayRay(Vector2 originA, Vector2 directionA, Vector2 originB, Vector2 directionB)
        {
            Vector2 originBToA = originA - originB;
            float denominator = VectorE.PerpDot(directionA, directionB);
            float perpDotA = VectorE.PerpDot(directionA, originBToA);
            float perpDotB = VectorE.PerpDot(directionB, originBToA);

            if (Mathf.Abs(denominator) < Geometry.Epsilon)
            {
                // Parallel
                bool codirected = Vector2.Dot(directionA, directionB) > 0;
                float originBProjection = Vector2.Dot(directionA, originBToA);
                if (Mathf.Abs(perpDotA) > Geometry.Epsilon || Mathf.Abs(perpDotB) > Geometry.Epsilon)
                {
                    // Not collinear
                    if (!codirected && originBProjection > 0)
                    {
                        return Vector2.Distance(originA, originB);
                    }
                    float distanceSqr = originBToA.sqrMagnitude - originBProjection*originBProjection;
                    // distanceSqr can be negative
                    return distanceSqr <= 0 ? 0 : Mathf.Sqrt(distanceSqr);
                }
                // Collinear

                if (codirected)
                {
                    // Ray intersection
                    return 0;
                }
                else
                {
                    if (originBProjection > 0)
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
            bool intersectionNotOnA = distanceA < -Geometry.Epsilon;
            bool intersectionNotOnB = distanceB < -Geometry.Epsilon;
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

        #endregion Ray-Ray

        #region Circle-Circle

        /// <summary>
        /// Returns the distance between the closest points on the circles
        /// </summary>
        /// <returns>
        /// Positive value if the circles do not intersect, negative otherwise.
        /// Negative value can be interpreted as depth of penetration.
        /// </returns>
        public static float CircleCircle(Circle circleA, Circle circleB)
        {
            return CircleCircle(circleA.center, circleA.radius, circleB.center, circleB.radius);
        }

        /// <summary>
        /// Returns the distance between the closest points on the circles
        /// </summary>
        /// <returns>
        /// Positive value if the circles do not intersect, negative otherwise.
        /// Negative value can be interpreted as depth of penetration.
        /// </returns>
        public static float CircleCircle(Vector2 centerA, float radiusA, Vector2 centerB, float radiusB)
        {
            return Vector2.Distance(centerA, centerB) - radiusA - radiusB;
        }

        #endregion Circle-Circle
    }
}
