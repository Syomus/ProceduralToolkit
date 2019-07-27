using UnityEngine;

namespace ProceduralToolkit
{
    /// <summary>
    /// Collection of closest point(s) algorithms
    /// </summary>
    public static partial class Closest
    {
        #region Point-Line

        /// <summary>
        /// Projects the point onto the line
        /// </summary>
        public static Vector2 PointLine(Vector2 point, Line2 line)
        {
            float projectedX;
            return PointLine(point, line.origin, line.direction, out projectedX);
        }

        /// <summary>
        /// Projects the point onto the line
        /// </summary>
        /// <param name="projectedX">Position of the projected point on the line relative to the origin</param>
        public static Vector2 PointLine(Vector2 point, Line2 line, out float projectedX)
        {
            return PointLine(point, line.origin, line.direction, out projectedX);
        }

        /// <summary>
        /// Projects the point onto the line
        /// </summary>
        /// <param name="lineDirection">Normalized direction of the line</param>
        public static Vector2 PointLine(Vector2 point, Vector2 lineOrigin, Vector2 lineDirection)
        {
            float projectedX;
            return PointLine(point, lineOrigin, lineDirection, out projectedX);
        }

        /// <summary>
        /// Projects the point onto the line
        /// </summary>
        /// <param name="lineDirection">Normalized direction of the line</param>
        /// <param name="projectedX">Position of the projected point on the line relative to the origin</param>
        public static Vector2 PointLine(Vector2 point, Vector2 lineOrigin, Vector2 lineDirection, out float projectedX)
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
        public static Vector2 PointRay(Vector2 point, Ray2D ray)
        {
            float projectedX;
            return PointRay(point, ray.origin, ray.direction, out projectedX);
        }

        /// <summary>
        /// Projects the point onto the ray
        /// </summary>
        /// <param name="projectedX">Position of the projected point on the ray relative to the origin</param>
        public static Vector2 PointRay(Vector2 point, Ray2D ray, out float projectedX)
        {
            return PointRay(point, ray.origin, ray.direction, out projectedX);
        }

        /// <summary>
        /// Projects the point onto the ray
        /// </summary>
        /// <param name="rayDirection">Normalized direction of the ray</param>
        public static Vector2 PointRay(Vector2 point, Vector2 rayOrigin, Vector2 rayDirection)
        {
            float projectedX;
            return PointRay(point, rayOrigin, rayDirection, out projectedX);
        }

        /// <summary>
        /// Projects the point onto the ray
        /// </summary>
        /// <param name="rayDirection">Normalized direction of the ray</param>
        /// <param name="projectedX">Position of the projected point on the ray relative to the origin</param>
        public static Vector2 PointRay(Vector2 point, Vector2 rayOrigin, Vector2 rayDirection, out float projectedX)
        {
            float pointProjection = Vector2.Dot(rayDirection, point - rayOrigin);
            if (pointProjection <= 0)
            {
                projectedX = 0;
                return rayOrigin;
            }

            // In theory, sqrMagnitude should be 1, but in practice this division helps with numerical stability
            projectedX = pointProjection/rayDirection.sqrMagnitude;
            return rayOrigin + rayDirection*projectedX;
        }

        #endregion Point-Ray

        #region Point-Segment

        /// <summary>
        /// Projects the point onto the segment
        /// </summary>
        public static Vector2 PointSegment(Vector2 point, Segment2 segment)
        {
            float projectedX;
            return PointSegment(point, segment.a, segment.b, out projectedX);
        }

        /// <summary>
        /// Projects the point onto the segment
        /// </summary>
        /// <param name="projectedX">Normalized position of the projected point on the segment. 
        /// Value of zero means that the projected point coincides with segment.a. 
        /// Value of one means that the projected point coincides with segment.b.</param>
        public static Vector2 PointSegment(Vector2 point, Segment2 segment, out float projectedX)
        {
            return PointSegment(point, segment.a, segment.b, out projectedX);
        }

        /// <summary>
        /// Projects the point onto the segment
        /// </summary>
        public static Vector2 PointSegment(Vector2 point, Vector2 segmentA, Vector2 segmentB)
        {
            float projectedX;
            return PointSegment(point, segmentA, segmentB, out projectedX);
        }

        /// <summary>
        /// Projects the point onto the segment
        /// </summary>
        /// <param name="projectedX">Normalized position of the projected point on the segment. 
        /// Value of zero means that the projected point coincides with <paramref name="segmentA"/>. 
        /// Value of one means that the projected point coincides with <paramref name="segmentB"/>.</param>
        public static Vector2 PointSegment(Vector2 point, Vector2 segmentA, Vector2 segmentB, out float projectedX)
        {
            Vector2 segmentDirection = segmentB - segmentA;
            float sqrSegmentLength = segmentDirection.sqrMagnitude;
            if (sqrSegmentLength < Geometry.Epsilon)
            {
                // The segment is a point
                projectedX = 0;
                return segmentA;
            }

            float pointProjection = Vector2.Dot(segmentDirection, point - segmentA);
            if (pointProjection <= 0)
            {
                projectedX = 0;
                return segmentA;
            }
            if (pointProjection >= sqrSegmentLength)
            {
                projectedX = 1;
                return segmentB;
            }

            projectedX = pointProjection/sqrSegmentLength;
            return segmentA + segmentDirection*projectedX;
        }

        private static Vector2 PointSegment(Vector2 point, Vector2 segmentA, Vector2 segmentB, Vector2 segmentDirection, float segmentLength)
        {
            float pointProjection = Vector2.Dot(segmentDirection, point - segmentA);
            if (pointProjection <= 0)
            {
                return segmentA;
            }
            if (pointProjection >= segmentLength)
            {
                return segmentB;
            }
            return segmentA + segmentDirection*pointProjection;
        }

        #endregion Point-Segment

        #region Point-Circle

        /// <summary>
        /// Projects the point onto the circle
        /// </summary>
        public static Vector2 PointCircle(Vector2 point, Circle2 circle)
        {
            return PointCircle(point, circle.center, circle.radius);
        }

        /// <summary>
        /// Projects the point onto the circle
        /// </summary>
        public static Vector2 PointCircle(Vector2 point, Vector2 circleCenter, float circleRadius)
        {
            return circleCenter + (point - circleCenter).normalized*circleRadius;
        }

        #endregion Point-Circle

        #region Line-Line

        /// <summary>
        /// Finds closest points on the lines
        /// </summary>
        public static void LineLine(Line2 lineA, Line2 lineB, out Vector2 pointA, out Vector2 pointB)
        {
            LineLine(lineA.origin, lineA.direction, lineB.origin, lineB.direction, out pointA, out pointB);
        }

        /// <summary>
        /// Finds closest points on the lines
        /// </summary>
        public static void LineLine(Vector2 originA, Vector2 directionA, Vector2 originB, Vector2 directionB,
            out Vector2 pointA, out Vector2 pointB)
        {
            Vector2 originBToA = originA - originB;
            float denominator = VectorE.PerpDot(directionA, directionB);
            float perpDotB = VectorE.PerpDot(directionB, originBToA);

            if (Mathf.Abs(denominator) < Geometry.Epsilon)
            {
                // Parallel
                if (Mathf.Abs(perpDotB) > Geometry.Epsilon ||
                    Mathf.Abs(VectorE.PerpDot(directionA, originBToA)) > Geometry.Epsilon)
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
        public static void LineRay(Line2 line, Ray2D ray, out Vector2 linePoint, out Vector2 rayPoint)
        {
            LineRay(line.origin, line.direction, ray.origin, ray.direction, out linePoint, out rayPoint);
        }

        /// <summary>
        /// Finds closest points on the line and the ray
        /// </summary>
        public static void LineRay(Vector2 lineOrigin, Vector2 lineDirection, Vector2 rayOrigin, Vector2 rayDirection,
            out Vector2 linePoint, out Vector2 rayPoint)
        {
            Vector2 rayOriginToLineOrigin = lineOrigin - rayOrigin;
            float denominator = VectorE.PerpDot(lineDirection, rayDirection);
            float perpDotA = VectorE.PerpDot(lineDirection, rayOriginToLineOrigin);

            if (Mathf.Abs(denominator) < Geometry.Epsilon)
            {
                // Parallel
                float perpDotB = VectorE.PerpDot(rayDirection, rayOriginToLineOrigin);
                if (Mathf.Abs(perpDotA) > Geometry.Epsilon || Mathf.Abs(perpDotB) > Geometry.Epsilon)
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
            if (rayDistance < -Geometry.Epsilon)
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
        public static void LineSegment(Line2 line, Segment2 segment, out Vector2 linePoint, out Vector2 segmentPoint)
        {
            LineSegment(line.origin, line.direction, segment.a, segment.b, out linePoint, out segmentPoint);
        }

        /// <summary>
        /// Finds closest points on the line and the segment
        /// </summary>
        public static void LineSegment(Vector2 lineOrigin, Vector2 lineDirection, Vector2 segmentA, Vector2 segmentB,
            out Vector2 linePoint, out Vector2 segmentPoint)
        {
            Vector2 segmentDirection = segmentB - segmentA;
            Vector2 segmentAToOrigin = lineOrigin - segmentA;
            float denominator = VectorE.PerpDot(lineDirection, segmentDirection);
            float perpDotA = VectorE.PerpDot(lineDirection, segmentAToOrigin);

            if (Mathf.Abs(denominator) < Geometry.Epsilon)
            {
                // Parallel
                bool codirected = Vector2.Dot(lineDirection, segmentDirection) > 0;
                // Normalized direction gives more stable results 
                float perpDotB = VectorE.PerpDot(segmentDirection.normalized, segmentAToOrigin);
                if (Mathf.Abs(perpDotA) > Geometry.Epsilon || Mathf.Abs(perpDotB) > Geometry.Epsilon)
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
            if (segmentDistance < -Geometry.Epsilon || segmentDistance > 1 + Geometry.Epsilon)
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
        public static void LineCircle(Line2 line, Circle2 circle, out Vector2 linePoint, out Vector2 circlePoint)
        {
            LineCircle(line.origin, line.direction, circle.center, circle.radius, out linePoint, out circlePoint);
        }

        /// <summary>
        /// Finds closest points on the line and the circle
        /// </summary>
        public static void LineCircle(Vector2 lineOrigin, Vector2 lineDirection, Vector2 circleCenter, float circleRadius,
            out Vector2 linePoint, out Vector2 circlePoint)
        {
            Vector2 originToCenter = circleCenter - lineOrigin;
            float centerProjection = Vector2.Dot(lineDirection, originToCenter);
            float sqrDistanceToLine = originToCenter.sqrMagnitude - centerProjection*centerProjection;
            float sqrDistanceToIntersection = circleRadius*circleRadius - sqrDistanceToLine;
            if (sqrDistanceToIntersection < -Geometry.Epsilon)
            {
                // No intersection
                linePoint = lineOrigin + lineDirection*centerProjection;
                circlePoint = circleCenter + (linePoint - circleCenter).normalized*circleRadius;
                return;
            }
            if (sqrDistanceToIntersection < Geometry.Epsilon)
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
        public static void RayRay(Ray2D rayA, Ray2D rayB, out Vector2 pointA, out Vector2 pointB)
        {
            RayRay(rayA.origin, rayA.direction, rayB.origin, rayB.direction, out pointA, out pointB);
        }

        /// <summary>
        /// Finds closest points on the rays
        /// </summary>
        public static void RayRay(Vector2 originA, Vector2 directionA, Vector2 originB, Vector2 directionB,
            out Vector2 pointA, out Vector2 pointB)
        {
            Vector2 originBToA = originA - originB;
            float denominator = VectorE.PerpDot(directionA, directionB);
            float perpDotA = VectorE.PerpDot(directionA, originBToA);
            float perpDotB = VectorE.PerpDot(directionB, originBToA);
            bool codirected = Vector2.Dot(directionA, directionB) > 0;

            if (Mathf.Abs(denominator) < Geometry.Epsilon)
            {
                // Parallel
                float originBProjection = Vector2.Dot(directionA, originBToA);
                if (Mathf.Abs(perpDotA) > Geometry.Epsilon || Mathf.Abs(perpDotB) > Geometry.Epsilon)
                {
                    // Not collinear
                    if (codirected)
                    {
                        if (originBProjection > -Geometry.Epsilon)
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
                    if (originBProjection > -Geometry.Epsilon)
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

            // Not parallel
            float distanceA = perpDotB/denominator;
            float distanceB = perpDotA/denominator;
            if (distanceA < -Geometry.Epsilon || distanceB < -Geometry.Epsilon)
            {
                // No intersection
                if (codirected)
                {
                    float originAProjection = Vector2.Dot(directionB, originBToA);
                    if (originAProjection > -Geometry.Epsilon)
                    {
                        pointA = originA;
                        pointB = originB + directionB*originAProjection;
                        return;
                    }
                    float originBProjection = -Vector2.Dot(directionA, originBToA);
                    if (originBProjection > -Geometry.Epsilon)
                    {
                        pointA = originA + directionA*originBProjection;
                        pointB = originB;
                        return;
                    }
                    pointA = originA;
                    pointB = originB;
                    return;
                }
                else
                {
                    if (distanceA > -Geometry.Epsilon)
                    {
                        float originBProjection = -Vector2.Dot(directionA, originBToA);
                        if (originBProjection > -Geometry.Epsilon)
                        {
                            pointA = originA + directionA*originBProjection;
                            pointB = originB;
                            return;
                        }
                    }
                    else if (distanceB > -Geometry.Epsilon)
                    {
                        float originAProjection = Vector2.Dot(directionB, originBToA);
                        if (originAProjection > -Geometry.Epsilon)
                        {
                            pointA = originA;
                            pointB = originB + directionB*originAProjection;
                            return;
                        }
                    }
                    pointA = originA;
                    pointB = originB;
                    return;
                }
            }
            // Point intersection
            pointA = pointB = originA + directionA*distanceA;
        }

        #endregion Ray-Ray

        #region Ray-Segment

        /// <summary>
        /// Finds closest points on the ray and the segment
        /// </summary>
        public static void RaySegment(Ray2D ray, Segment2 segment, out Vector2 rayPoint, out Vector2 segmentPoint)
        {
            RaySegment(ray.origin, ray.direction, segment.a, segment.b, out rayPoint, out segmentPoint);
        }

        /// <summary>
        /// Finds closest points on the ray and the segment
        /// </summary>
        public static void RaySegment(Vector2 rayOrigin, Vector2 rayDirection, Vector2 segmentA, Vector2 segmentB,
            out Vector2 rayPoint, out Vector2 segmentPoint)
        {
            Vector2 segmentDirection = segmentB - segmentA;
            Vector2 segmentAToOrigin = rayOrigin - segmentA;
            float denominator = VectorE.PerpDot(rayDirection, segmentDirection);
            float perpDotA = VectorE.PerpDot(rayDirection, segmentAToOrigin);
            // Normalized direction gives more stable results 
            float perpDotB = VectorE.PerpDot(segmentDirection.normalized, segmentAToOrigin);

            if (Mathf.Abs(denominator) < Geometry.Epsilon)
            {
                // Parallel
                float segmentAProjection = -Vector2.Dot(rayDirection, segmentAToOrigin);
                Vector2 rayOriginToSegmentB = segmentB - rayOrigin;
                float segmentBProjection = Vector2.Dot(rayDirection, rayOriginToSegmentB);
                if (Mathf.Abs(perpDotA) > Geometry.Epsilon || Mathf.Abs(perpDotB) > Geometry.Epsilon)
                {
                    // Not collinear
                    if (segmentAProjection > -Geometry.Epsilon && segmentBProjection > -Geometry.Epsilon)
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
                    if (segmentAProjection > -Geometry.Epsilon || segmentBProjection > -Geometry.Epsilon)
                    {
                        rayPoint = rayOrigin;
                        float sqrSegmentLength = segmentDirection.sqrMagnitude;
                        if (sqrSegmentLength > Geometry.Epsilon)
                        {
                            float rayOriginProjection = Vector2.Dot(segmentDirection, segmentAToOrigin)/sqrSegmentLength;
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
                if (segmentAProjection > -Geometry.Epsilon && segmentBProjection > -Geometry.Epsilon)
                {
                    // Segment intersection
                    rayPoint = segmentPoint = segmentAProjection < segmentBProjection ? segmentA : segmentB;
                    return;
                }
                if (segmentAProjection > -Geometry.Epsilon || segmentBProjection > -Geometry.Epsilon)
                {
                    // Point or segment intersection
                    rayPoint = segmentPoint = rayOrigin;
                    return;
                }
                // No intersection
                rayPoint = rayOrigin;
                segmentPoint = segmentAProjection > segmentBProjection ? segmentA : segmentB;
                return;
            }

            // Not parallel
            float rayDistance = perpDotB/denominator;
            float segmentDistance = perpDotA/denominator;
            if (rayDistance < -Geometry.Epsilon ||
                segmentDistance < -Geometry.Epsilon || segmentDistance > 1 + Geometry.Epsilon)
            {
                // No intersection
                bool codirected = Vector2.Dot(rayDirection, segmentDirection) > 0;
                Vector2 segmentBToOrigin;
                if (!codirected)
                {
                    PTUtils.Swap(ref segmentA, ref segmentB);
                    segmentDirection = -segmentDirection;
                    segmentBToOrigin = segmentAToOrigin;
                    segmentAToOrigin = rayOrigin - segmentA;
                    segmentDistance = 1 - segmentDistance;
                }
                else
                {
                    segmentBToOrigin = rayOrigin - segmentB;
                }

                float segmentAProjection = -Vector2.Dot(rayDirection, segmentAToOrigin);
                float segmentBProjection = -Vector2.Dot(rayDirection, segmentBToOrigin);
                bool segmentAOnRay = segmentAProjection > -Geometry.Epsilon;
                bool segmentBOnRay = segmentBProjection > -Geometry.Epsilon;
                if (segmentAOnRay && segmentBOnRay)
                {
                    if (segmentDistance < 0)
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
                else if (!segmentAOnRay && segmentBOnRay)
                {
                    if (segmentDistance < 0)
                    {
                        rayPoint = rayOrigin;
                        segmentPoint = segmentA;
                        return;
                    }
                    else if (segmentDistance > 1 + Geometry.Epsilon)
                    {
                        rayPoint = rayOrigin + rayDirection*segmentBProjection;
                        segmentPoint = segmentB;
                        return;
                    }
                    else
                    {
                        rayPoint = rayOrigin;
                        float originProjection = Vector2.Dot(segmentDirection, segmentAToOrigin);
                        segmentPoint = segmentA + segmentDirection*originProjection/segmentDirection.sqrMagnitude;
                        return;
                    }
                }
                else
                {
                    // Not on ray
                    rayPoint = rayOrigin;
                    float originProjection = Vector2.Dot(segmentDirection, segmentAToOrigin);
                    float sqrSegmentLength = segmentDirection.sqrMagnitude;
                    if (originProjection < 0)
                    {
                        segmentPoint = segmentA;
                        return;
                    }
                    else if (originProjection > sqrSegmentLength)
                    {
                        segmentPoint = segmentB;
                        return;
                    }
                    else
                    {
                        segmentPoint = segmentA + segmentDirection*originProjection/sqrSegmentLength;
                        return;
                    }
                }
            }
            // Point intersection
            rayPoint = segmentPoint = segmentA + segmentDirection*segmentDistance;
        }

        #endregion Ray-Segment

        #region Ray-Circle

        /// <summary>
        /// Finds closest points on the ray and the circle
        /// </summary>
        public static void RayCircle(Ray2D ray, Circle2 circle, out Vector2 rayPoint, out Vector2 circlePoint)
        {
            RayCircle(ray.origin, ray.direction, circle.center, circle.radius, out rayPoint, out circlePoint);
        }

        /// <summary>
        /// Finds closest points on the ray and the circle
        /// </summary>
        public static void RayCircle(Vector2 rayOrigin, Vector2 rayDirection, Vector2 circleCenter, float circleRadius,
            out Vector2 rayPoint, out Vector2 circlePoint)
        {
            Vector2 originToCenter = circleCenter - rayOrigin;
            float centerProjection = Vector2.Dot(rayDirection, originToCenter);
            if (centerProjection + circleRadius < -Geometry.Epsilon)
            {
                // No intersection
                rayPoint = rayOrigin;
                circlePoint = circleCenter - originToCenter.normalized*circleRadius;
                return;
            }

            float sqrDistanceToLine = originToCenter.sqrMagnitude - centerProjection*centerProjection;
            float sqrDistanceToIntersection = circleRadius*circleRadius - sqrDistanceToLine;
            if (sqrDistanceToIntersection < -Geometry.Epsilon)
            {
                // No intersection
                if (centerProjection < -Geometry.Epsilon)
                {
                    rayPoint = rayOrigin;
                    circlePoint = circleCenter - originToCenter.normalized*circleRadius;
                    return;
                }
                rayPoint = rayOrigin + rayDirection*centerProjection;
                circlePoint = circleCenter + (rayPoint - circleCenter).normalized*circleRadius;
                return;
            }
            if (sqrDistanceToIntersection < Geometry.Epsilon)
            {
                if (centerProjection < -Geometry.Epsilon)
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

            if (distanceA < -Geometry.Epsilon)
            {
                float distanceB = centerProjection + distanceToIntersection;
                if (distanceB < -Geometry.Epsilon)
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

        #region Segment-Segment

        /// <summary>
        /// Finds closest points on the segments
        /// </summary>
        public static void SegmentSegment(Segment2 segment1, Segment2 segment2, out Vector2 segment1Point, out Vector2 segment2Point)
        {
            SegmentSegment(segment1.a, segment1.b, segment2.a, segment2.b, out segment1Point, out segment2Point);
        }

        /// <summary>
        /// Finds closest points on the segments
        /// </summary>
        public static void SegmentSegment(Vector2 segment1A, Vector2 segment1B, Vector2 segment2A, Vector2 segment2B,
            out Vector2 segment1Point, out Vector2 segment2Point)
        {
            Vector2 from2ATo1A = segment1A - segment2A;
            Vector2 direction1 = segment1B - segment1A;
            Vector2 direction2 = segment2B - segment2A;
            float segment1Length = direction1.magnitude;
            float segment2Length = direction2.magnitude;

            bool segment1IsAPoint = segment1Length < Geometry.Epsilon;
            bool segment2IsAPoint = segment2Length < Geometry.Epsilon;
            if (segment1IsAPoint && segment2IsAPoint)
            {
                if (segment1A == segment2A)
                {
                    segment1Point = segment2Point = segment1A;
                    return;
                }
                segment1Point = segment1A;
                segment2Point = segment2A;
                return;
            }
            if (segment1IsAPoint)
            {
                direction2.Normalize();
                segment1Point = segment1A;
                segment2Point = PointSegment(segment1A, segment2A, segment2B, direction2, segment2Length);
                return;
            }
            if (segment2IsAPoint)
            {
                direction1.Normalize();
                segment1Point = PointSegment(segment2A, segment1A, segment1B, direction1, segment1Length);
                segment2Point = segment2A;
                return;
            }

            direction1.Normalize();
            direction2.Normalize();
            float denominator = VectorE.PerpDot(direction1, direction2);
            float perpDot1 = VectorE.PerpDot(direction1, from2ATo1A);
            float perpDot2 = VectorE.PerpDot(direction2, from2ATo1A);

            if (Mathf.Abs(denominator) < Geometry.Epsilon)
            {
                // Parallel
                bool codirected = Vector2.Dot(direction1, direction2) > 0;
                if (Mathf.Abs(perpDot1) > Geometry.Epsilon || Mathf.Abs(perpDot2) > Geometry.Epsilon)
                {
                    // Not collinear
                    Vector2 from1ATo2B;
                    if (!codirected)
                    {
                        PTUtils.Swap(ref segment2A, ref segment2B);
                        direction2 = -direction2;
                        from1ATo2B = -from2ATo1A;
                        from2ATo1A = segment1A - segment2A;
                    }
                    else
                    {
                        from1ATo2B = segment2B - segment1A;
                    }
                    float segment2AProjection = -Vector2.Dot(direction1, from2ATo1A);
                    float segment2BProjection = Vector2.Dot(direction1, from1ATo2B);

                    bool segment2AIsAfter1A = segment2AProjection > -Geometry.Epsilon;
                    bool segment2BIsAfter1A = segment2BProjection > -Geometry.Epsilon;
                    if (!segment2AIsAfter1A && !segment2BIsAfter1A)
                    {
                        //           1A------1B
                        // 2A------2B
                        segment1Point = segment1A;
                        segment2Point = segment2B;
                        return;
                    }
                    bool segment2AIsBefore1B = segment2AProjection < segment1Length + Geometry.Epsilon;
                    bool segment2BIsBefore1B = segment2BProjection < segment1Length + Geometry.Epsilon;
                    if (!segment2AIsBefore1B && !segment2BIsBefore1B)
                    {
                        // 1A------1B
                        //           2A------2B
                        segment1Point = segment1B;
                        segment2Point = segment2A;
                        return;
                    }

                    if (segment2AIsAfter1A && segment2BIsBefore1B)
                    {
                        // 1A------1B
                        //   2A--2B
                        segment1Point = segment1A + direction1*segment2AProjection;
                        segment2Point = segment2A;
                        return;
                    }

                    if (segment2AIsAfter1A) // && segment2AIsBefore1B && !segment2BIsBefore1B)
                    {
                        // 1A------1B
                        //     2A------2B
                        segment1Point = segment1A + direction1*segment2AProjection;
                        segment2Point = segment2A;
                        return;
                    }
                    else
                    {
                        //   1A------1B
                        // 2A----2B
                        // 2A----------2B
                        segment1Point = segment1A;
                        float segment1AProjection = Vector2.Dot(direction2, from2ATo1A);
                        segment2Point = segment2A + direction2*segment1AProjection;
                        return;
                    }
                }
                // Collinear

                if (codirected)
                {
                    // Codirected
                    float segment2AProjection = -Vector2.Dot(direction1, from2ATo1A);
                    if (segment2AProjection > -Geometry.Epsilon)
                    {
                        // 1A------1B
                        //     2A------2B
                        SegmentSegmentCollinear(segment1A, segment1B, segment2A, out segment1Point, out segment2Point);
                        return;
                    }
                    else
                    {
                        //     1A------1B
                        // 2A------2B
                        SegmentSegmentCollinear(segment2A, segment2B, segment1A, out segment2Point, out segment1Point);
                        return;
                    }
                }
                else
                {
                    // Contradirected
                    float segment2BProjection = Vector2.Dot(direction1, segment2B - segment1A);
                    if (segment2BProjection > -Geometry.Epsilon)
                    {
                        // 1A------1B
                        //     2B------2A
                        SegmentSegmentCollinear(segment1A, segment1B, segment2B, out segment1Point, out segment2Point);
                        return;
                    }
                    else
                    {
                        //     1A------1B
                        // 2B------2A
                        SegmentSegmentCollinear(segment2B, segment2A, segment1A, out segment2Point, out segment1Point);
                        return;
                    }
                }
            }

            // Not parallel
            float distance1 = perpDot2/denominator;
            float distance2 = perpDot1/denominator;
            if (distance1 < -Geometry.Epsilon || distance1 > segment1Length + Geometry.Epsilon ||
                distance2 < -Geometry.Epsilon || distance2 > segment2Length + Geometry.Epsilon)
            {
                // No intersection
                bool codirected = Vector2.Dot(direction1, direction2) > 0;
                Vector2 from1ATo2B;
                if (!codirected)
                {
                    PTUtils.Swap(ref segment2A, ref segment2B);
                    direction2 = -direction2;
                    from1ATo2B = -from2ATo1A;
                    from2ATo1A = segment1A - segment2A;
                    distance2 = segment2Length - distance2;
                }
                else
                {
                    from1ATo2B = segment2B - segment1A;
                }

                float segment2AProjection = -Vector2.Dot(direction1, from2ATo1A);
                float segment2BProjection = Vector2.Dot(direction1, from1ATo2B);

                bool segment2AIsAfter1A = segment2AProjection > -Geometry.Epsilon;
                bool segment2BIsBefore1B = segment2BProjection < segment1Length + Geometry.Epsilon;
                bool segment2AOnSegment1 = segment2AIsAfter1A && segment2AProjection < segment1Length + Geometry.Epsilon;
                bool segment2BOnSegment1 = segment2BProjection > -Geometry.Epsilon && segment2BIsBefore1B;
                if (segment2AOnSegment1 && segment2BOnSegment1)
                {
                    if (distance2 < -Geometry.Epsilon)
                    {
                        segment1Point = segment1A + direction1*segment2AProjection;
                        segment2Point = segment2A;
                    }
                    else
                    {
                        segment1Point = segment1A + direction1*segment2BProjection;
                        segment2Point = segment2B;
                    }
                }
                else if (!segment2AOnSegment1 && !segment2BOnSegment1)
                {
                    if (!segment2AIsAfter1A && !segment2BIsBefore1B)
                    {
                        segment1Point = distance1 < -Geometry.Epsilon ? segment1A : segment1B;
                    }
                    else
                    {
                        // Not on segment
                        segment1Point = segment2AIsAfter1A ? segment1B : segment1A;
                    }
                    float segment1PointProjection = Vector2.Dot(direction2, segment1Point - segment2A);
                    segment1PointProjection = Mathf.Clamp(segment1PointProjection, 0, segment2Length);
                    segment2Point = segment2A + direction2*segment1PointProjection;
                }
                else if (segment2AOnSegment1)
                {
                    if (distance2 < -Geometry.Epsilon)
                    {
                        segment1Point = segment1A + direction1*segment2AProjection;
                        segment2Point = segment2A;
                    }
                    else
                    {
                        segment1Point = segment1B;
                        float segment1PointProjection = Vector2.Dot(direction2, segment1Point - segment2A);
                        segment1PointProjection = Mathf.Clamp(segment1PointProjection, 0, segment2Length);
                        segment2Point = segment2A + direction2*segment1PointProjection;
                    }
                }
                else
                {
                    if (distance2 > segment2Length + Geometry.Epsilon)
                    {
                        segment1Point = segment1A + direction1*segment2BProjection;
                        segment2Point = segment2B;
                    }
                    else
                    {
                        segment1Point = segment1A;
                        float segment1PointProjection = Vector2.Dot(direction2, segment1Point - segment2A);
                        segment1PointProjection = Mathf.Clamp(segment1PointProjection, 0, segment2Length);
                        segment2Point = segment2A + direction2*segment1PointProjection;
                    }
                }
                return;
            }

            // Point intersection
            segment1Point = segment2Point = segment1A + direction1*distance1;
        }

        private static void SegmentSegmentCollinear(Vector2 leftA, Vector2 leftB, Vector2 rightA, out Vector2 leftPoint, out Vector2 rightPoint)
        {
            Vector2 leftDirection = leftB - leftA;
            float rightAProjection = Vector2.Dot(leftDirection.normalized, rightA - leftB);
            if (Mathf.Abs(rightAProjection) < Geometry.Epsilon)
            {
                // LB == RA
                // LA------LB
                //         RA------RB

                // Point intersection
                leftPoint = rightPoint = leftB;
                return;
            }
            if (rightAProjection < 0)
            {
                // LB > RA
                // LA------LB
                //     RARB
                //     RA--RB
                //     RA------RB

                // Segment intersection
                leftPoint = rightPoint = rightA;
                return;
            }
            // LB < RA
            // LA------LB
            //             RA------RB

            // No intersection
            leftPoint = leftB;
            rightPoint = rightA;
        }

        #endregion Segment-Segment

        #region Segment-Circle

        /// <summary>
        /// Finds closest points on the segment and the circle
        /// </summary>
        public static void SegmentCircle(Segment2 segment, Circle2 circle, out Vector2 segmentPoint, out Vector2 circlePoint)
        {
            SegmentCircle(segment.a, segment.b, circle.center, circle.radius, out segmentPoint, out circlePoint);
        }

        /// <summary>
        /// Finds closest points on the segment and the circle
        /// </summary>
        public static void SegmentCircle(Vector2 segmentA, Vector2 segmentB, Vector2 circleCenter, float circleRadius,
            out Vector2 segmentPoint, out Vector2 circlePoint)
        {
            Vector2 segmentAToCenter = circleCenter - segmentA;
            Vector2 fromAtoB = segmentB - segmentA;
            float segmentLength = fromAtoB.magnitude;
            if (segmentLength < Geometry.Epsilon)
            {
                segmentPoint = segmentA;
                float distanceToPoint = segmentAToCenter.magnitude;
                if (distanceToPoint < circleRadius + Geometry.Epsilon)
                {
                    if (distanceToPoint > circleRadius - Geometry.Epsilon)
                    {
                        circlePoint = segmentPoint;
                        return;
                    }
                    if (distanceToPoint < Geometry.Epsilon)
                    {
                        circlePoint = segmentPoint;
                        return;
                    }
                }
                Vector2 toPoint = -segmentAToCenter/distanceToPoint;
                circlePoint = circleCenter + toPoint*circleRadius;
                return;
            }

            Vector2 segmentDirection = fromAtoB.normalized;
            float centerProjection = Vector2.Dot(segmentDirection, segmentAToCenter);
            if (centerProjection + circleRadius < -Geometry.Epsilon ||
                centerProjection - circleRadius > segmentLength + Geometry.Epsilon)
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
            if (sqrDistanceToIntersection < -Geometry.Epsilon)
            {
                // No intersection
                if (centerProjection < -Geometry.Epsilon)
                {
                    segmentPoint = segmentA;
                    circlePoint = circleCenter - segmentAToCenter.normalized*circleRadius;
                    return;
                }
                if (centerProjection > segmentLength + Geometry.Epsilon)
                {
                    segmentPoint = segmentB;
                    circlePoint = circleCenter - (circleCenter - segmentB).normalized*circleRadius;
                    return;
                }
                segmentPoint = segmentA + segmentDirection*centerProjection;
                circlePoint = circleCenter + (segmentPoint - circleCenter).normalized*circleRadius;
                return;
            }

            if (sqrDistanceToIntersection < Geometry.Epsilon)
            {
                if (centerProjection < -Geometry.Epsilon)
                {
                    // No intersection
                    segmentPoint = segmentA;
                    circlePoint = circleCenter - segmentAToCenter.normalized*circleRadius;
                    return;
                }
                if (centerProjection > segmentLength + Geometry.Epsilon)
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

            bool pointAIsAfterSegmentA = distanceA > -Geometry.Epsilon;
            bool pointBIsBeforeSegmentB = distanceB < segmentLength + Geometry.Epsilon;

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

            bool pointAIsBeforeSegmentB = distanceA < segmentLength + Geometry.Epsilon;
            if (pointAIsAfterSegmentA && pointAIsBeforeSegmentB)
            {
                // Point A intersection
                segmentPoint = circlePoint = segmentA + segmentDirection*distanceA;
                return;
            }
            bool pointBIsAfterSegmentA = distanceB > -Geometry.Epsilon;
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
        public static void CircleCircle(Circle2 circleA, Circle2 circleB, out Vector2 pointA, out Vector2 pointB)
        {
            CircleCircle(circleA.center, circleA.radius, circleB.center, circleB.radius, out pointA, out pointB);
        }

        /// <summary>
        /// Finds closest points on the circles
        /// </summary>
        public static void CircleCircle(Vector2 centerA, float radiusA, Vector2 centerB, float radiusB,
            out Vector2 pointA, out Vector2 pointB)
        {
            Vector2 fromBtoA = (centerA - centerB).normalized;
            pointA = centerA - fromBtoA*radiusA;
            pointB = centerB + fromBtoA*radiusB;
        }

        #endregion Circle-Circle
    }
}
