using UnityEngine;

namespace ProceduralToolkit
{
    /// <summary>
    /// Collection of basic computational geometry algorithms
    /// </summary>
    public static partial class Geometry
    {
        public const float Epsilon = 0.00001f;

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
        /// Projects the point onto the line
        /// </summary>
        /// <param name="lineDirection">Normalized direction of the line</param>
        public static Vector2 ClosestPointOnLine(Vector2 point, Vector2 lineOrigin, Vector2 lineDirection)
        {
            float projectedX;
            return ClosestPointOnLine(point, lineOrigin, lineDirection, out projectedX);
        }

        /// <summary>
        /// Projects the point onto the line
        /// </summary>
        /// <param name="lineDirection">Normalized direction of the line</param>
        /// <param name="projectedX">Position of the projected point on the line relative to the origin</param>
        public static Vector2 ClosestPointOnLine(Vector2 point, Vector2 lineOrigin, Vector2 lineDirection, out float projectedX)
        {
            // In theory, sqrMagnitude should be 1, but in practice this division helps with numerical stability
            projectedX = Vector2.Dot(lineDirection, point - lineOrigin)/lineDirection.sqrMagnitude;
            return lineOrigin + lineDirection*projectedX;
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
        /// Projects the point onto the ray
        /// </summary>
        /// <param name="projectedX">Position of the projected point on the ray relative to the origin</param>
        public static Vector2 ClosestPointOnRay(Vector2 point, Ray2D ray, out float projectedX)
        {
            return ClosestPointOnRay(point, ray.origin, ray.direction, out projectedX);
        }

        /// <summary>
        /// Projects the point onto the ray
        /// </summary>
        /// <param name="rayDirection">Normalized direction of the ray</param>
        public static Vector2 ClosestPointOnRay(Vector2 point, Vector2 rayOrigin, Vector2 rayDirection)
        {
            float projectedX;
            return ClosestPointOnRay(point, rayOrigin, rayDirection, out projectedX);
        }

        /// <summary>
        /// Projects the point onto the ray
        /// </summary>
        /// <param name="rayDirection">Normalized direction of the ray</param>
        /// <param name="projectedX">Position of the projected point on the ray relative to the origin</param>
        public static Vector2 ClosestPointOnRay(Vector2 point, Vector2 rayOrigin, Vector2 rayDirection, out float projectedX)
        {
            float dotToPoint = Vector2.Dot(rayDirection, point - rayOrigin);
            if (dotToPoint <= 0)
            {
                projectedX = 0;
                return rayOrigin;
            }

            // In theory, sqrMagnitude should be 1, but in practice this division helps with numerical stability
            projectedX = dotToPoint/rayDirection.sqrMagnitude;
            return rayOrigin + rayDirection*projectedX;
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
            Vector2 segmentDirection = segmentB - segmentA;
            float sqrSegmentLength = segmentDirection.sqrMagnitude;
            if (sqrSegmentLength < Epsilon)
            {
                // The segment is a point
                projectedX = 0;
                return segmentA;
            }

            float pointProjection = Vector2.Dot(segmentDirection, point - segmentA);
            if (pointProjection < -Epsilon)
            {
                projectedX = 0;
                return segmentA;
            }
            if (pointProjection > sqrSegmentLength + Epsilon)
            {
                projectedX = 1;
                return segmentB;
            }

            projectedX = pointProjection/sqrSegmentLength;
            return segmentA + segmentDirection*projectedX;
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
        /// Projects the point onto the circle
        /// </summary>
        public static Vector2 ClosestPointOnCircle(Vector2 point, Vector2 circleCenter, float circleRadius)
        {
            return circleCenter + (point - circleCenter).normalized*circleRadius;
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

            if (Mathf.Abs(denominator) < Epsilon)
            {
                float perpDotB = VectorE.PerpDot(rayDirection, rayOriginToLineOrigin);
                // Parallel
                if (Mathf.Abs(perpDotA) > Epsilon || Mathf.Abs(perpDotB) > Epsilon)
                {
                    // Not collinear
                    float rayOriginProjection = Vector2.Dot(lineDirection, rayOriginToLineOrigin);
                    linePoint = lineOrigin - lineDirection*rayOriginProjection;
                    rayPoint = rayOrigin;
                    return;
                }
                // Collinear
                linePoint = rayPoint = rayOrigin;
                return;
            }

            // Not parallel
            float rayDistance = perpDotA/denominator;
            if (rayDistance < -Epsilon)
            {
                // No intersection
                float rayOriginProjection = Vector2.Dot(lineDirection, rayOriginToLineOrigin);
                linePoint = lineOrigin - lineDirection*rayOriginProjection;
                rayPoint = rayOrigin;
                return;
            }
            // Point intersection
            linePoint = rayPoint = rayOrigin + rayDirection*rayDistance;
        }

        #endregion Line-Ray

        #region Line-Segment

        /// <summary>
        /// Finds closest points on the line and the segment
        /// </summary>
        public static void ClosestPointsLineSegment(Line2 line, Segment2 segment, out Vector2 linePoint, out Vector2 segmentPoint)
        {
            ClosestPointsLineSegment(line.origin, line.direction, segment.a, segment.b, out linePoint, out segmentPoint);
        }

        /// <summary>
        /// Finds closest points on the line and the segment
        /// </summary>
        public static void ClosestPointsLineSegment(Vector2 lineOrigin, Vector2 lineDirection, Vector2 segmentA, Vector2 segmentB,
            out Vector2 linePoint, out Vector2 segmentPoint)
        {
            Vector2 segmentDirection = segmentB - segmentA;
            Vector2 segmentAToOrigin = lineOrigin - segmentA;
            float denominator = VectorE.PerpDot(lineDirection, segmentDirection);
            float perpDotA = VectorE.PerpDot(lineDirection, segmentAToOrigin);

            if (Mathf.Abs(denominator) < Epsilon)
            {
                // Parallel
                bool codirected = Vector2.Dot(lineDirection, segmentDirection) > 0;
                // Normalized direction gives more stable results 
                float perpDotB = VectorE.PerpDot(segmentDirection.normalized, segmentAToOrigin);
                if (Mathf.Abs(perpDotA) > Epsilon || Mathf.Abs(perpDotB) > Epsilon)
                {
                    // Not collinear
                    if (codirected)
                    {
                        float segmentAProjection = Vector2.Dot(lineDirection, segmentAToOrigin);
                        linePoint = lineOrigin - lineDirection*segmentAProjection;
                        segmentPoint = segmentA;
                    }
                    else
                    {
                        float segmentBProjection = Vector2.Dot(lineDirection, lineOrigin - segmentB);
                        linePoint = lineOrigin - lineDirection*segmentBProjection;
                        segmentPoint = segmentB;
                    }
                    return;
                }

                // Collinear
                if (codirected)
                {
                    linePoint = segmentPoint = segmentA;
                }
                else
                {
                    linePoint = segmentPoint = segmentB;
                }
                return;
            }

            // Not parallel
            float segmentDistance = perpDotA/denominator;
            if (segmentDistance < -Epsilon || segmentDistance > 1 + Epsilon)
            {
                // No intersection
                segmentPoint = segmentA + segmentDirection*Mathf.Clamp01(segmentDistance);
                float segmentPointProjection = Vector2.Dot(lineDirection, segmentPoint - lineOrigin);
                linePoint = lineOrigin + lineDirection*segmentPointProjection;
                return;
            }
            // Point intersection
            linePoint = segmentPoint = segmentA + segmentDirection*segmentDistance;
        }

        #endregion Line-Segment

        #region Line-Circle

        /// <summary>
        /// Finds closest points on the line and the circle
        /// </summary>
        public static void ClosestPointsLineCircle(Line2 line, Circle circle, out Vector2 linePoint, out Vector2 circlePoint)
        {
            ClosestPointsLineCircle(line.origin, line.direction, circle.center, circle.radius, out linePoint, out circlePoint);
        }

        /// <summary>
        /// Finds closest points on the line and the circle
        /// </summary>
        public static void ClosestPointsLineCircle(Vector2 lineOrigin, Vector2 lineDirection, Vector2 circleCenter, float circleRadius,
            out Vector2 linePoint, out Vector2 circlePoint)
        {
            Vector2 originToCenter = circleCenter - lineOrigin;
            float centerProjection = Vector2.Dot(lineDirection, originToCenter);
            float sqrDistanceToLine = originToCenter.sqrMagnitude - centerProjection*centerProjection;

            float sqrDistanceToIntersection = circleRadius*circleRadius - sqrDistanceToLine;
            if (sqrDistanceToIntersection < -Epsilon)
            {
                // No intersection
                linePoint = lineOrigin + lineDirection*centerProjection;
                circlePoint = circleCenter + (linePoint - circleCenter).normalized*circleRadius;
                return;
            }
            if (sqrDistanceToIntersection < Epsilon)
            {
                // Point intersection
                linePoint = circlePoint = lineOrigin + lineDirection*centerProjection;
                return;
            }

            // Two points intersection
            float distanceToIntersection = Mathf.Sqrt(sqrDistanceToIntersection);
            float distanceA = centerProjection - distanceToIntersection;
            linePoint = circlePoint = lineOrigin + lineDirection*distanceA;
        }

        #endregion Line-Circle

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
                float originBProjection = Vector2.Dot(directionA, originBToA);

                if (Mathf.Abs(perpDotA) > Epsilon || Mathf.Abs(perpDotB) > Epsilon)
                {
                    // Not collinear
                    if (codirected)
                    {
                        if (originBProjection > 0)
                        {
                            // Projection of originA is on rayB
                            pointA = originA;
                            pointB = originB + directionA*originBProjection;
                            return;
                        }
                        else
                        {
                            pointA = originA - directionA*originBProjection;
                            pointB = originB;
                            return;
                        }
                    }
                    else
                    {
                        if (originBProjection > 0)
                        {
                            pointA = originA;
                            pointB = originB;
                            return;
                        }
                        else
                        {
                            // Projection of originA is on rayB
                            pointA = originA;
                            pointB = originB + directionA*originBProjection;
                            return;
                        }
                    }
                }
                // Collinear

                if (codirected)
                {
                    // Ray intersection
                    if (originBProjection > 0)
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
                    if (originBProjection > 0)
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

        #region Ray-Segment

        /// <summary>
        /// Finds closest points on the ray and the segment
        /// </summary>
        public static void ClosestPointsRaySegment(Ray2D ray, Segment2 segment, out Vector2 rayPoint, out Vector2 segmentPoint)
        {
            ClosestPointsRaySegment(ray.origin, ray.direction, segment.a, segment.b, out rayPoint, out segmentPoint);
        }

        /// <summary>
        /// Finds closest points on the ray and the segment
        /// </summary>
        public static void ClosestPointsRaySegment(Vector2 rayOrigin, Vector2 rayDirection, Vector2 segmentA, Vector2 segmentB,
            out Vector2 rayPoint, out Vector2 segmentPoint)
        {
            Vector2 segmentDirection = segmentB - segmentA;
            Vector2 segmentAToRayOrigin = rayOrigin - segmentA;
            float denominator = VectorE.PerpDot(rayDirection, segmentDirection);
            float perpDotA = VectorE.PerpDot(rayDirection, segmentAToRayOrigin);
            float perpDotB = VectorE.PerpDot(segmentDirection, segmentAToRayOrigin);

            if (Mathf.Abs(denominator) < Epsilon)
            {
                // Parallel
                float segmentAProjection = -Vector2.Dot(rayDirection, segmentAToRayOrigin);
                Vector2 rayOriginToSegmentB = segmentB - rayOrigin;
                float segmentBProjection = Vector2.Dot(rayDirection, rayOriginToSegmentB);
                if (Mathf.Abs(perpDotA) > Epsilon || Mathf.Abs(perpDotB) > Epsilon)
                {
                    // Not collinear
                    if (segmentAProjection > -Epsilon && segmentBProjection > -Epsilon)
                    {
                        if (segmentAProjection < segmentBProjection)
                        {
                            rayPoint = rayOrigin + rayDirection*segmentAProjection;
                            segmentPoint = segmentA;
                            return;
                        }
                        else
                        {
                            rayPoint = rayOrigin + rayDirection*segmentBProjection;
                            segmentPoint = segmentB;
                            return;
                        }
                    }
                    if (segmentAProjection > -Epsilon || segmentBProjection > -Epsilon)
                    {
                        rayPoint = rayOrigin;
                        float sqrSegmentLength = segmentDirection.sqrMagnitude;
                        if (sqrSegmentLength > Epsilon)
                        {
                            float rayOriginProjection = Vector2.Dot(segmentDirection, segmentAToRayOrigin)/sqrSegmentLength;
                            segmentPoint = segmentA + segmentDirection*rayOriginProjection;
                        }
                        else
                        {
                            segmentPoint = segmentA;
                        }
                        return;
                    }
                    rayPoint = rayOrigin;
                    segmentPoint = segmentAProjection > segmentBProjection ? segmentA : segmentB;
                    return;
                }

                // Collinear
                if (segmentAProjection > -Epsilon && segmentBProjection > -Epsilon)
                {
                    // Point or segment intersection
                    rayPoint = segmentPoint = segmentAProjection < segmentBProjection ? segmentA : segmentB;
                    return;
                }
                if (segmentAProjection > -Epsilon || segmentBProjection > -Epsilon)
                {
                    // Point or segment intersection
                    rayPoint = segmentPoint = rayOrigin;
                    return;
                }
                rayPoint = rayOrigin;
                segmentPoint = segmentAProjection > segmentBProjection ? segmentA : segmentB;
                return;
            }

            // Not parallel
            float rayDistance = perpDotB/denominator;
            float segmentDistance = perpDotA/denominator;
            if (rayDistance < -Epsilon ||
                segmentDistance < -Epsilon || segmentDistance > 1 + Epsilon)
            {
                // No intersection
                segmentPoint = segmentA + segmentDirection*Mathf.Clamp01(segmentDistance);
                float segmentPointProjection = Vector2.Dot(rayDirection, segmentPoint - rayOrigin);
                if (segmentPointProjection <= 0)
                {
                    rayPoint = rayOrigin;
                }
                else
                {
                    rayPoint = rayOrigin + rayDirection*segmentPointProjection;
                }
                return;
            }
            // Point intersection
            rayPoint = segmentPoint = rayOrigin + rayDirection*rayDistance;
        }

        #endregion Ray-Segment

        #region Ray-Circle

        /// <summary>
        /// Finds closest points on the ray and the circle
        /// </summary>
        public static void ClosestPointsRayCircle(Ray2D ray, Circle circle, out Vector2 rayPoint, out Vector2 circlePoint)
        {
            ClosestPointsRayCircle(ray.origin, ray.direction, circle.center, circle.radius, out rayPoint, out circlePoint);
        }

        /// <summary>
        /// Finds closest points on the ray and the circle
        /// </summary>
        public static void ClosestPointsRayCircle(Vector2 rayOrigin, Vector2 rayDirection, Vector2 circleCenter, float circleRadius,
            out Vector2 rayPoint, out Vector2 circlePoint)
        {
            Vector2 originToCenter = circleCenter - rayOrigin;
            float centerProjection = Vector2.Dot(rayDirection, originToCenter);
            if (centerProjection + circleRadius < -Epsilon)
            {
                // No intersection
                rayPoint = rayOrigin;
                circlePoint = circleCenter - originToCenter.normalized*circleRadius;
                return;
            }

            float sqrDistanceToLine = originToCenter.sqrMagnitude - centerProjection*centerProjection;
            float sqrDistanceToIntersection = circleRadius*circleRadius - sqrDistanceToLine;
            if (sqrDistanceToIntersection < -Epsilon)
            {
                // No intersection
                if (centerProjection < -Epsilon)
                {
                    rayPoint = rayOrigin;
                    circlePoint = circleCenter - originToCenter.normalized*circleRadius;
                    return;
                }
                rayPoint = rayOrigin + rayDirection*centerProjection;
                circlePoint = circleCenter + (rayPoint - circleCenter).normalized*circleRadius;
                return;
            }
            if (sqrDistanceToIntersection < Epsilon)
            {
                if (centerProjection < -Epsilon)
                {
                    // No intersection
                    rayPoint = rayOrigin;
                    circlePoint = circleCenter - originToCenter.normalized*circleRadius;
                    return;
                }
                // Point intersection
                rayPoint = circlePoint = rayOrigin + rayDirection*centerProjection;
                return;
            }

            // Line intersection
            float distanceToIntersection = Mathf.Sqrt(sqrDistanceToIntersection);
            float distanceA = centerProjection - distanceToIntersection;

            if (distanceA < -Epsilon)
            {
                float distanceB = centerProjection + distanceToIntersection;
                if (distanceB < -Epsilon)
                {
                    // No intersection
                    rayPoint = rayOrigin;
                    circlePoint = circleCenter - originToCenter.normalized*circleRadius;
                    return;
                }

                // Point intersection
                rayPoint = circlePoint = rayOrigin + rayDirection*distanceB;
                return;
            }

            // Two points intersection
            rayPoint = circlePoint = rayOrigin + rayDirection*distanceA;
        }

        #endregion Ray-Circle

        #region Segment-Circle

        /// <summary>
        /// Finds closest points on the segment and the circle
        /// </summary>
        public static void ClosestPointsSegmentCircle(Segment2 segment, Circle circle, out Vector2 segmentPoint, out Vector2 circlePoint)
        {
            ClosestPointsSegmentCircle(segment.a, segment.b, circle.center, circle.radius, out segmentPoint, out circlePoint);
        }

        /// <summary>
        /// Finds closest points on the segment and the circle
        /// </summary>
        public static void ClosestPointsSegmentCircle(Vector2 segmentA, Vector2 segmentB, Vector2 circleCenter, float circleRadius,
            out Vector2 segmentPoint, out Vector2 circlePoint)
        {
            Vector2 segmentAToCenter = circleCenter - segmentA;
            Vector2 fromAtoB = segmentB - segmentA;
            float segmentLength = fromAtoB.magnitude;
            Vector2 segmentDirection = fromAtoB.normalized;
            float centerProjection = Vector2.Dot(segmentDirection, segmentAToCenter);
            if (centerProjection + circleRadius < -Epsilon ||
                centerProjection - circleRadius > segmentLength + Epsilon)
            {
                // No intersection
                if (centerProjection < 0)
                {
                    segmentPoint = segmentA;
                    circlePoint = circleCenter - segmentAToCenter.normalized*circleRadius;
                    return;
                }
                segmentPoint = segmentB;
                circlePoint = circleCenter - (circleCenter - segmentB).normalized*circleRadius;
                return;
            }

            float sqrDistanceToLine = segmentAToCenter.sqrMagnitude - centerProjection*centerProjection;
            float sqrDistanceToIntersection = circleRadius*circleRadius - sqrDistanceToLine;
            if (sqrDistanceToIntersection < -Epsilon)
            {
                // No intersection
                if (centerProjection < -Epsilon)
                {
                    segmentPoint = segmentA;
                    circlePoint = circleCenter - segmentAToCenter.normalized*circleRadius;
                    return;
                }
                if (centerProjection > segmentLength + Epsilon)
                {
                    segmentPoint = segmentB;
                    circlePoint = circleCenter - (circleCenter - segmentB).normalized*circleRadius;
                    return;
                }
                segmentPoint = segmentA + segmentDirection*centerProjection;
                circlePoint = circleCenter + (segmentPoint - circleCenter).normalized*circleRadius;
                return;
            }

            if (sqrDistanceToIntersection < Epsilon)
            {
                if (centerProjection < -Epsilon)
                {
                    // No intersection
                    segmentPoint = segmentA;
                    circlePoint = circleCenter - segmentAToCenter.normalized*circleRadius;
                    return;
                }
                if (centerProjection > segmentLength + Epsilon)
                {
                    // No intersection
                    segmentPoint = segmentB;
                    circlePoint = circleCenter - (circleCenter - segmentB).normalized*circleRadius;
                    return;
                }
                // Point intersection
                segmentPoint = circlePoint = segmentA + segmentDirection*centerProjection;
                return;
            }

            // Line intersection
            float distanceToIntersection = Mathf.Sqrt(sqrDistanceToIntersection);
            float distanceA = centerProjection - distanceToIntersection;
            float distanceB = centerProjection + distanceToIntersection;

            bool pointAIsAfterSegmentA = distanceA > -Epsilon;
            bool pointBIsBeforeSegmentB = distanceB < segmentLength + Epsilon;

            if (pointAIsAfterSegmentA && pointBIsBeforeSegmentB)
            {
                segmentPoint = circlePoint = segmentA + segmentDirection*distanceA;
                return;
            }
            if (!pointAIsAfterSegmentA && !pointBIsBeforeSegmentB)
            {
                // The segment is inside, but no intersection
                if (distanceA > -(distanceB - segmentLength))
                {
                    segmentPoint = segmentA;
                    circlePoint = segmentA + segmentDirection*distanceA;
                    return;
                }
                segmentPoint = segmentB;
                circlePoint = segmentA + segmentDirection*distanceB;
                return;
            }

            bool pointAIsBeforeSegmentB = distanceA < segmentLength + Epsilon;
            if (pointAIsAfterSegmentA && pointAIsBeforeSegmentB)
            {
                // Point A intersection
                segmentPoint = circlePoint = segmentA + segmentDirection*distanceA;
                return;
            }
            bool pointBIsAfterSegmentA = distanceB > -Epsilon;
            if (pointBIsAfterSegmentA && pointBIsBeforeSegmentB)
            {
                // Point B intersection
                segmentPoint = circlePoint = segmentA + segmentDirection*distanceB;
                return;
            }

            // No intersection
            if (centerProjection < 0)
            {
                segmentPoint = segmentA;
                circlePoint = circleCenter - segmentAToCenter.normalized*circleRadius;
                return;
            }
            segmentPoint = segmentB;
            circlePoint = circleCenter - (circleCenter - segmentB).normalized*circleRadius;
        }

        #endregion Segment-Circle

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
            Vector2 fromBtoA = (centerA - centerB).normalized;
            pointA = centerA - fromBtoA*radiusA;
            pointB = centerB + fromBtoA*radiusB;
        }

        #endregion Circle-Circle
    }
}
