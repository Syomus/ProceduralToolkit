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
                return Vector2.Distance(linePoint, rayOrigin);
            }
            // Point intersection
            return 0;
        }

        #endregion Line-Ray

        #region Line-Segment

        /// <summary>
        /// Returns the distance between the closest points on the line and the segment
        /// </summary>
        public static float LineSegment(Line2 line, Segment2 segment)
        {
            return LineSegment(line.origin, line.direction, segment.a, segment.b);
        }

        /// <summary>
        /// Returns the distance between the closest points on the line and the segment
        /// </summary>
        public static float LineSegment(Vector2 lineOrigin, Vector2 lineDirection, Vector2 segmentA, Vector2 segmentB)
        {
            Vector2 segmentDirection = segmentB - segmentA;
            Vector2 segmentAToLineOrigin = lineOrigin - segmentA;
            float denominator = VectorE.PerpDot(lineDirection, segmentDirection);
            float perpDotA = VectorE.PerpDot(lineDirection, segmentAToLineOrigin);

            if (Mathf.Abs(denominator) < Geometry.Epsilon)
            {
                // Parallel
                float perpDotB = VectorE.PerpDot(segmentDirection, segmentAToLineOrigin);
                if (Mathf.Abs(perpDotA) > Geometry.Epsilon || Mathf.Abs(perpDotB) > Geometry.Epsilon)
                {
                    // Not collinear
                    float segmentAProjection = Vector2.Dot(lineDirection, segmentAToLineOrigin);
                    float distanceSqr = segmentAToLineOrigin.sqrMagnitude - segmentAProjection*segmentAProjection;
                    // distanceSqr can be negative
                    return distanceSqr <= 0 ? 0 : Mathf.Sqrt(distanceSqr);
                }
                // Collinear
                return 0;
            }

            // Not parallel
            float segmentDistance = perpDotA/denominator;
            if (segmentDistance < -Geometry.Epsilon || segmentDistance > 1 + Geometry.Epsilon)
            {
                // No intersection
                Vector2 segmentPoint = segmentA + segmentDirection*Mathf.Clamp01(segmentDistance);
                float segmentPointProjection = Vector2.Dot(lineDirection, lineOrigin - segmentPoint);
                Vector2 linePoint = lineOrigin - lineDirection*segmentPointProjection;
                return Vector2.Distance(linePoint, segmentPoint);
            }
            // Point intersection
            return 0;
        }

        #endregion Line-Segment

        #region Line-Circle

        /// <summary>
        /// Returns the distance between the closest points on the line and the circle
        /// </summary>
        public static float LineCircle(Line2 line, Circle circle)
        {
            return LineCircle(line.origin, line.direction, circle.center, circle.radius);
        }

        /// <summary>
        /// Returns the distance between the closest points on the line and the circle
        /// </summary>
        public static float LineCircle(Vector2 lineOrigin, Vector2 lineDirection, Vector2 circleCenter, float circleRadius)
        {
            Vector2 originToCenter = circleCenter - lineOrigin;
            float centerProjection = Vector2.Dot(lineDirection, originToCenter);
            float sqrDistanceToLine = originToCenter.sqrMagnitude - centerProjection*centerProjection;

            float sqrDistanceToIntersection = circleRadius*circleRadius - sqrDistanceToLine;
            if (sqrDistanceToIntersection < -Geometry.Epsilon)
            {
                // No intersection
                return Mathf.Sqrt(sqrDistanceToLine) - circleRadius;
            }
            return 0;
        }

        #endregion Line-Circle

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

        #region Ray-Segment

        /// <summary>
        /// Returns the distance between the closest points on the ray and the segment
        /// </summary>
        public static float RaySegment(Ray2D ray, Segment2 segment)
        {
            return RaySegment(ray.origin, ray.direction, segment.a, segment.b);
        }

        /// <summary>
        /// Returns the distance between the closest points on the ray and the segment
        /// </summary>
        public static float RaySegment(Vector2 rayOrigin, Vector2 rayDirection, Vector2 segmentA, Vector2 segmentB)
        {
            Vector2 segmentDirection = segmentB - segmentA;
            Vector2 segmentAToRayOrigin = rayOrigin - segmentA;
            float denominator = VectorE.PerpDot(rayDirection, segmentDirection);
            float perpDotA = VectorE.PerpDot(rayDirection, segmentAToRayOrigin);
            float perpDotB = VectorE.PerpDot(segmentDirection, segmentAToRayOrigin);

            if (Mathf.Abs(denominator) < Geometry.Epsilon)
            {
                // Parallel
                float segmentAProjection = -Vector2.Dot(rayDirection, segmentAToRayOrigin);
                Vector2 rayOriginToSegmentB = segmentB - rayOrigin;
                float segmentBProjection = Vector2.Dot(rayDirection, rayOriginToSegmentB);
                if (Mathf.Abs(perpDotA) > Geometry.Epsilon || Mathf.Abs(perpDotB) > Geometry.Epsilon)
                {
                    // Not collinear
                    if (segmentAProjection > -Geometry.Epsilon)
                    {
                        float distanceSqr = segmentAToRayOrigin.sqrMagnitude - segmentAProjection*segmentAProjection;
                        // distanceSqr can be negative
                        return distanceSqr <= 0 ? 0 : Mathf.Sqrt(distanceSqr);
                    }
                    if (segmentBProjection > -Geometry.Epsilon)
                    {
                        float distanceSqr = rayOriginToSegmentB.sqrMagnitude - segmentBProjection*segmentBProjection;
                        // distanceSqr can be negative
                        return distanceSqr <= 0 ? 0 : Mathf.Sqrt(distanceSqr);
                    }

                    if (segmentAProjection > segmentBProjection)
                    {
                        return Vector2.Distance(rayOrigin, segmentA);
                    }
                    return Vector2.Distance(rayOrigin, segmentB);
                }
                // Collinear
                if (segmentAProjection > -Geometry.Epsilon || segmentBProjection > -Geometry.Epsilon)
                {
                    // Point or segment intersection
                    return 0;
                }
                return segmentAProjection > segmentBProjection ? -segmentAProjection : -segmentBProjection;
            }

            // Not parallel
            float rayDistance = perpDotB/denominator;
            float segmentDistance = perpDotA/denominator;
            if (rayDistance < -Geometry.Epsilon ||
                segmentDistance < -Geometry.Epsilon || segmentDistance > 1 + Geometry.Epsilon)
            {
                // No intersection
                Vector2 segmentPoint = segmentA + segmentDirection*Mathf.Clamp01(segmentDistance);
                float segmentPointProjection = Vector2.Dot(rayDirection, segmentPoint - rayOrigin);
                Vector2 rayPoint;
                if (segmentPointProjection <= 0)
                {
                    rayPoint = rayOrigin;
                }
                else
                {
                    rayPoint = rayOrigin + rayDirection*segmentPointProjection;
                }
                return Vector2.Distance(rayPoint, segmentPoint);
            }
            // Point intersection
            return 0;
        }

        #endregion Ray-Segment

        #region Ray-Circle

        /// <summary>
        /// Returns the distance between the closest points on the ray and the circle
        /// </summary>
        public static float RayCircle(Ray2D ray, Circle circle)
        {
            return RayCircle(ray.origin, ray.direction, circle.center, circle.radius);
        }

        /// <summary>
        /// Returns the distance between the closest points on the ray and the circle
        /// </summary>
        public static float RayCircle(Vector2 rayOrigin, Vector2 rayDirection, Vector2 circleCenter, float circleRadius)
        {
            Vector2 originToCenter = circleCenter - rayOrigin;
            float centerProjection = Vector2.Dot(rayDirection, originToCenter);
            if (centerProjection + circleRadius < -Geometry.Epsilon)
            {
                // No intersection
                return Mathf.Sqrt(originToCenter.sqrMagnitude) - circleRadius;
            }

            float sqrDistanceToOrigin = originToCenter.sqrMagnitude;
            float sqrDistanceToLine = sqrDistanceToOrigin - centerProjection*centerProjection;
            float sqrDistanceToIntersection = circleRadius*circleRadius - sqrDistanceToLine;
            if (sqrDistanceToIntersection < -Geometry.Epsilon)
            {
                // No intersection
                if (centerProjection < -Geometry.Epsilon)
                {
                    return Mathf.Sqrt(sqrDistanceToOrigin) - circleRadius;
                }
                return Mathf.Sqrt(sqrDistanceToLine) - circleRadius;
            }
            if (sqrDistanceToIntersection < Geometry.Epsilon)
            {
                if (centerProjection < -Geometry.Epsilon)
                {
                    // No intersection
                    return Mathf.Sqrt(sqrDistanceToOrigin) - circleRadius;
                }
                // Point intersection
                return 0;
            }

            // Line intersection
            float distanceToIntersection = Mathf.Sqrt(sqrDistanceToIntersection);
            float distanceA = centerProjection - distanceToIntersection;
            float distanceB = centerProjection + distanceToIntersection;

            if (distanceA < -Geometry.Epsilon)
            {
                if (distanceB < -Geometry.Epsilon)
                {
                    // No intersection
                    return Mathf.Sqrt(sqrDistanceToOrigin) - circleRadius;
                }

                // Point intersection;
                return 0;
            }

            // Two points intersection;
            return 0;
        }

        #endregion Ray-Circle

        #region Segment-Circle

        /// <summary>
        /// Returns the distance between the closest points on the segment and the circle
        /// </summary>
        public static float SegmentCircle(Segment2 segment, Circle circle)
        {
            return SegmentCircle(segment.a, segment.b, circle.center, circle.radius);
        }

        /// <summary>
        /// Returns the distance between the closest points on the segment and the circle
        /// </summary>
        public static float SegmentCircle(Vector2 segmentA, Vector2 segmentB, Vector2 circleCenter, float circleRadius)
        {
            Vector2 segmentAToCenter = circleCenter - segmentA;
            Vector2 fromAtoB = segmentB - segmentA;
            float segmentLength = fromAtoB.magnitude;
            Vector2 segmentDirection = fromAtoB.normalized;
            float centerProjection = Vector2.Dot(segmentDirection, segmentAToCenter);
            if (centerProjection + circleRadius < -Geometry.Epsilon ||
                centerProjection - circleRadius > segmentLength + Geometry.Epsilon)
            {
                // No intersection
                if (centerProjection < 0)
                {
                    return Mathf.Sqrt(segmentAToCenter.sqrMagnitude) - circleRadius;
                }
                return (circleCenter - segmentB).magnitude - circleRadius;
            }

            float sqrDistanceToA = segmentAToCenter.sqrMagnitude;
            float sqrDistanceToLine = sqrDistanceToA - centerProjection*centerProjection;
            float sqrDistanceToIntersection = circleRadius*circleRadius - sqrDistanceToLine;
            if (sqrDistanceToIntersection < -Geometry.Epsilon)
            {
                // No intersection
                if (centerProjection < -Geometry.Epsilon)
                {
                    return Mathf.Sqrt(sqrDistanceToA) - circleRadius;
                }
                if (centerProjection > segmentLength + Geometry.Epsilon)
                {
                    return (circleCenter - segmentB).magnitude - circleRadius;
                }
                return Mathf.Sqrt(sqrDistanceToLine) - circleRadius;
            }

            if (sqrDistanceToIntersection < Geometry.Epsilon)
            {
                if (centerProjection < -Geometry.Epsilon)
                {
                    // No intersection
                    return Mathf.Sqrt(sqrDistanceToA) - circleRadius;
                }
                if (centerProjection > segmentLength + Geometry.Epsilon)
                {
                    // No intersection
                    return (circleCenter - segmentB).magnitude - circleRadius;
                }
                // Point intersection
                return 0;
            }

            // Line intersection
            float distanceToIntersection = Mathf.Sqrt(sqrDistanceToIntersection);
            float distanceA = centerProjection - distanceToIntersection;
            float distanceB = centerProjection + distanceToIntersection;

            bool pointAIsAfterSegmentA = distanceA > -Geometry.Epsilon;
            bool pointBIsBeforeSegmentB = distanceB < segmentLength + Geometry.Epsilon;

            if (pointAIsAfterSegmentA && pointBIsBeforeSegmentB)
            {
                // Two points intersection
                return 0;
            }
            if (!pointAIsAfterSegmentA && !pointBIsBeforeSegmentB)
            {
                // The segment is inside, but no intersection
                distanceB = -(distanceB - segmentLength);
                return distanceA > distanceB ? distanceA : distanceB;
            }

            bool pointAIsBeforeSegmentB = distanceA < segmentLength + Geometry.Epsilon;
            if (pointAIsAfterSegmentA && pointAIsBeforeSegmentB)
            {
                // Point A intersection
                return 0;
            }
            bool pointBIsAfterSegmentA = distanceB > -Geometry.Epsilon;
            if (pointBIsAfterSegmentA && pointBIsBeforeSegmentB)
            {
                // Point B intersection
                return 0;
            }

            // No intersection
            if (centerProjection < 0)
            {
                return Mathf.Sqrt(sqrDistanceToA) - circleRadius;
            }
            return (circleCenter - segmentB).magnitude - circleRadius;
        }

        #endregion Segment-Circle

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
