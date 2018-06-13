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
        public static bool PointLine(Vector2 point, Line2 line)
        {
            return PointLine(point, line.origin, line.direction);
        }

        /// <summary>
        /// Tests if the point lies on the line
        /// </summary>
        /// <param name="side">
        /// -1 if the point is to the left of the line,
        /// 0 if it is on the line,
        /// 1 if it is to the right of the line
        /// </param>
        public static bool PointLine(Vector2 point, Line2 line, out int side)
        {
            return PointLine(point, line.origin, line.direction, out side);
        }

        /// <summary>
        /// Tests if the point lies on the line
        /// </summary>
        public static bool PointLine(Vector2 point, Vector2 lineOrigin, Vector2 lineDirection)
        {
            float perpDot = VectorE.PerpDot(point - lineOrigin, lineDirection);
            return -Geometry.Epsilon < perpDot && perpDot < Geometry.Epsilon;
        }

        /// <summary>
        /// Tests if the point lies on the line
        /// </summary>
        /// <param name="side">
        /// -1 if the point is to the left of the line,
        /// 0 if it is on the line,
        /// 1 if it is to the right of the line
        /// </param>
        public static bool PointLine(Vector2 point, Vector2 lineOrigin, Vector2 lineDirection, out int side)
        {
            float perpDot = VectorE.PerpDot(point - lineOrigin, lineDirection);
            if (perpDot < -Geometry.Epsilon)
            {
                side = -1;
                return false;
            }
            if (perpDot > Geometry.Epsilon)
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
        /// Tests if the point lies on the ray
        /// </summary>
        public static bool PointRay(Vector2 point, Ray2D ray)
        {
            return PointRay(point, ray.origin, ray.direction);
        }

        /// <summary>
        /// Tests if the point lies on the ray
        /// </summary>
        /// <param name="side">
        /// -1 if the point is to the left of the ray,
        /// 0 if it is on the line,
        /// 1 if it is to the right of the ray
        /// </param>
        public static bool PointRay(Vector2 point, Ray2D ray, out int side)
        {
            return PointRay(point, ray.origin, ray.direction, out side);
        }

        /// <summary>
        /// Tests if the point lies on the ray
        /// </summary>
        public static bool PointRay(Vector2 point, Vector2 rayOrigin, Vector2 rayDirection)
        {
            Vector2 toPoint = point - rayOrigin;
            float perpDot = VectorE.PerpDot(toPoint, rayDirection);
            return -Geometry.Epsilon < perpDot && perpDot < Geometry.Epsilon &&
                   Vector2.Dot(rayDirection, toPoint) > -Geometry.Epsilon;
        }

        /// <summary>
        /// Tests if the point lies on the ray
        /// </summary>
        /// <param name="side">
        /// -1 if the point is to the left of the ray,
        /// 0 if it is on the line,
        /// 1 if it is to the right of the ray
        /// </param>
        public static bool PointRay(Vector2 point, Vector2 rayOrigin, Vector2 rayDirection, out int side)
        {
            Vector2 toPoint = point - rayOrigin;
            float perpDot = VectorE.PerpDot(toPoint, rayDirection);
            if (perpDot < -Geometry.Epsilon)
            {
                side = -1;
                return false;
            }
            if (perpDot > Geometry.Epsilon)
            {
                side = 1;
                return false;
            }
            side = 0;
            return Vector2.Dot(rayDirection, toPoint) > -Geometry.Epsilon;
        }

        #endregion Point-Ray

        #region Point-Segment

        /// <summary>
        /// Tests if the point lies on the segment
        /// </summary>
        public static bool PointSegment(Vector2 point, Segment2 segment)
        {
            return PointSegment(point, segment.a, segment.b);
        }

        /// <summary>
        /// Tests if the point lies on the segment
        /// </summary>
        /// <param name="side">
        /// -1 if the point is to the left of the segment,
        /// 0 if it is on the line,
        /// 1 if it is to the right of the segment
        /// </param>
        public static bool PointSegment(Vector2 point, Segment2 segment, out int side)
        {
            return PointSegment(point, segment.a, segment.b, out side);
        }

        /// <summary>
        /// Tests if the point lies on the segment
        /// </summary>
        public static bool PointSegment(Vector2 point, Vector2 segmentA, Vector2 segmentB)
        {
            Vector2 segmentDirection = segmentB - segmentA;
            Vector2 toPoint = point - segmentA;
            float perpDot = VectorE.PerpDot(toPoint, segmentDirection);
            if (-Geometry.Epsilon < perpDot && perpDot < Geometry.Epsilon)
            {
                float dotToPoint = Vector2.Dot(segmentDirection, toPoint);
                return dotToPoint > -Geometry.Epsilon &&
                       dotToPoint < segmentDirection.sqrMagnitude + Geometry.Epsilon;
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
        public static bool PointSegment(Vector2 point, Vector2 segmentA, Vector2 segmentB, out int side)
        {
            Vector2 segmentDirection = segmentB - segmentA;
            Vector2 toPoint = point - segmentA;
            float perpDot = VectorE.PerpDot(toPoint, segmentDirection);
            if (perpDot < -Geometry.Epsilon)
            {
                side = -1;
                return false;
            }
            if (perpDot > Geometry.Epsilon)
            {
                side = 1;
                return false;
            }
            side = 0;
            float dotToPoint = Vector2.Dot(segmentDirection, toPoint);
            return dotToPoint > -Geometry.Epsilon &&
                   dotToPoint < segmentDirection.sqrMagnitude + Geometry.Epsilon;
        }

        #endregion Point-Segment

        #region Point-Circle

        /// <summary>
        /// Tests if the point is inside the circle
        /// </summary>
        public static bool PointCircle(Vector2 point, Circle circle)
        {
            return PointCircle(point, circle.center, circle.radius);
        }

        /// <summary>
        /// Tests if the point is inside the circle
        /// </summary>
        public static bool PointCircle(Vector2 point, Vector2 circleCenter, float circleRadius)
        {
            // For points on the circle's edge magnitude is more stable than sqrMagnitude
            return (point - circleCenter).magnitude < circleRadius + Geometry.Epsilon;
        }

        #endregion Point-Circle

        #region Line-Line

        /// <summary>
        /// Computes an intersection of the lines
        /// </summary>
        public static bool LineLine(Line2 lineA, Line2 lineB)
        {
            return Distance.LineLine(lineA.origin, lineA.direction, lineB.origin, lineB.direction) < Geometry.Epsilon;
        }

        /// <summary>
        /// Computes an intersection of the lines
        /// </summary>
        public static bool LineLine(Line2 lineA, Line2 lineB, out IntersectionLineLine2 intersection)
        {
            return LineLine(lineA.origin, lineA.direction, lineB.origin, lineB.direction, out intersection);
        }

        /// <summary>
        /// Computes an intersection of the lines
        /// </summary>
        public static bool LineLine(Vector2 originA, Vector2 directionA, Vector2 originB, Vector2 directionB)
        {
            return Distance.LineLine(originA, directionA, originB, directionB) < Geometry.Epsilon;
        }

        /// <summary>
        /// Computes an intersection of the lines
        /// </summary>
        public static bool LineLine(Vector2 originA, Vector2 directionA, Vector2 originB, Vector2 directionB,
            out IntersectionLineLine2 intersection)
        {
            Vector2 originBToA = originA - originB;
            float denominator = VectorE.PerpDot(directionA, directionB);
            float perpDotB = VectorE.PerpDot(directionB, originBToA);

            if (Mathf.Abs(denominator) < Geometry.Epsilon)
            {
                // Parallel
                float perpDotA = VectorE.PerpDot(directionA, originBToA);
                if (Mathf.Abs(perpDotA) > Geometry.Epsilon || Mathf.Abs(perpDotB) > Geometry.Epsilon)
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
        public static IntersectionType LineLine(Line2 lineA, Line2 lineB, out float distanceA, out float distanceB)
        {
            return LineLine(lineA.origin, lineA.direction, lineB.origin, lineB.direction, out distanceA, out distanceB);
        }

        /// <summary>
        /// Computes an intersection of the lines
        /// </summary>
        public static IntersectionType LineLine(Vector2 originA, Vector2 directionA, Vector2 originB, Vector2 directionB,
            out float distanceA, out float distanceB)
        {
            Vector2 originBToA = originA - originB;
            float denominator = VectorE.PerpDot(directionA, directionB);
            float perpDotA = VectorE.PerpDot(directionA, originBToA);
            float perpDotB = VectorE.PerpDot(directionB, originBToA);

            if (Mathf.Abs(denominator) < Geometry.Epsilon)
            {
                // Parallel
                if (Mathf.Abs(perpDotA) > Geometry.Epsilon || Mathf.Abs(perpDotB) > Geometry.Epsilon)
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
        public static bool LineRay(Line2 line, Ray2D ray)
        {
            return Distance.LineRay(line.origin, line.direction, ray.origin, ray.direction) < Geometry.Epsilon;
        }

        /// <summary>
        /// Computes an intersection of the line and the ray
        /// </summary>
        public static bool LineRay(Line2 line, Ray2D ray, out IntersectionLineRay2 intersection)
        {
            return LineRay(line.origin, line.direction, ray.origin, ray.direction, out intersection);
        }

        /// <summary>
        /// Computes an intersection of the line and the ray
        /// </summary>
        public static bool LineRay(Vector2 lineOrigin, Vector2 lineDirection, Vector2 rayOrigin, Vector2 rayDirection)
        {
            return Distance.LineRay(lineOrigin, lineDirection, rayOrigin, rayDirection) < Geometry.Epsilon;
        }

        /// <summary>
        /// Computes an intersection of the line and the ray
        /// </summary>
        public static bool LineRay(Vector2 lineOrigin, Vector2 lineDirection, Vector2 rayOrigin, Vector2 rayDirection,
            out IntersectionLineRay2 intersection)
        {
            float lineDistance;
            float rayDistance;
            var intersectionType = LineLine(lineOrigin, lineDirection, rayOrigin, rayDirection, out lineDistance, out rayDistance);
            if (intersectionType == IntersectionType.Line)
            {
                intersection = IntersectionLineRay2.Ray(rayOrigin);
                return true;
            }
            if (intersectionType == IntersectionType.Point && rayDistance > -Geometry.Epsilon)
            {
                intersection = IntersectionLineRay2.Point(rayOrigin + rayDirection*rayDistance);
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
        public static bool LineSegment(Line2 line, Segment2 segment, out IntersectionLineSegment2 intersection)
        {
            return LineSegment(line.origin, line.direction, segment.a, segment.b, out intersection);
        }

        /// <summary>
        /// Computes an intersection of the line and the segment
        /// </summary>
        public static bool LineSegment(Vector2 lineOrigin, Vector2 lineDirection, Vector2 segmentA, Vector2 segmentB,
            out IntersectionLineSegment2 intersection)
        {
            float lineDistance;
            float segmentDistance;
            Vector2 segmentDirection = segmentB - segmentA;
            var intersectionType = LineLine(lineOrigin, lineDirection, segmentA, segmentDirection, out lineDistance, out segmentDistance);
            if (intersectionType == IntersectionType.Line)
            {
                bool segmentIsAPoint = segmentDirection.sqrMagnitude < Geometry.Epsilon;
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
                segmentDistance > -Geometry.Epsilon && segmentDistance < 1 + Geometry.Epsilon)
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
        public static bool LineCircle(Line2 line, Circle circle, out IntersectionLineCircle intersection)
        {
            return LineCircle(line.origin, line.direction, circle.center, circle.radius, out intersection);
        }

        /// <summary>
        /// Computes an intersection of the line and the circle
        /// </summary>
        public static bool LineCircle(Vector2 lineOrigin, Vector2 lineDirection, Vector2 circleCenter, float circleRadius,
            out IntersectionLineCircle intersection)
        {
            Vector2 originToCenter = circleCenter - lineOrigin;
            float centerProjection = Vector2.Dot(lineDirection, originToCenter);
            float sqrDistanceToLine = originToCenter.sqrMagnitude - centerProjection*centerProjection;

            float sqrDistanceToIntersection = circleRadius*circleRadius - sqrDistanceToLine;
            if (sqrDistanceToIntersection < -Geometry.Epsilon)
            {
                intersection = IntersectionLineCircle.None();
                return false;
            }
            if (sqrDistanceToIntersection < Geometry.Epsilon)
            {
                intersection = IntersectionLineCircle.Point(lineOrigin + lineDirection*centerProjection);
                return true;
            }

            float distanceToIntersection = Mathf.Sqrt(sqrDistanceToIntersection);
            float distanceA = centerProjection - distanceToIntersection;
            float distanceB = centerProjection + distanceToIntersection;

            Vector2 pointA = lineOrigin + lineDirection*distanceA;
            Vector2 pointB = lineOrigin + lineDirection*distanceB;
            intersection = IntersectionLineCircle.TwoPoints(pointA, pointB);
            return true;
        }

        #endregion Line-Circle

        #region Ray-Ray

        /// <summary>
        /// Computes an intersection of the rays
        /// </summary>
        public static bool RayRay(Ray2D rayA, Ray2D rayB)
        {
            return Distance.RayRay(rayA.origin, rayA.direction, rayB.origin, rayB.direction) < Geometry.Epsilon;
        }

        /// <summary>
        /// Computes an intersection of the rays
        /// </summary>
        public static bool RayRay(Ray2D rayA, Ray2D rayB, out IntersectionRayRay2 intersection)
        {
            return RayRay(rayA.origin, rayA.direction, rayB.origin, rayB.direction, out intersection);
        }

        /// <summary>
        /// Computes an intersection of the rays
        /// </summary>
        public static bool RayRay(Vector2 originA, Vector2 directionA, Vector2 originB, Vector2 directionB)
        {
            return Distance.RayRay(originA, directionA, originB, directionB) < Geometry.Epsilon;
        }

        /// <summary>
        /// Computes an intersection of the rays
        /// </summary>
        public static bool RayRay(Vector2 originA, Vector2 directionA, Vector2 originB, Vector2 directionB,
            out IntersectionRayRay2 intersection)
        {
            Vector2 originBToA = originA - originB;
            float denominator = VectorE.PerpDot(directionA, directionB);
            float perpDotA = VectorE.PerpDot(directionA, originBToA);
            float perpDotB = VectorE.PerpDot(directionB, originBToA);

            if (Mathf.Abs(denominator) < Geometry.Epsilon)
            {
                // Parallel
                if (Mathf.Abs(perpDotA) > Geometry.Epsilon || Mathf.Abs(perpDotB) > Geometry.Epsilon)
                {
                    // Not collinear
                    intersection = IntersectionRayRay2.None();
                    return false;
                }
                // Collinear

                bool codirected = Vector2.Dot(directionA, directionB) > 0;
                float originBProjection = Vector2.Dot(directionA, originBToA);
                if (codirected)
                {
                    intersection = IntersectionRayRay2.Ray(originBProjection > 0 ? originA : originB, directionA);
                    return true;
                }
                else
                {
                    if (originBProjection > 0)
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
            if (distanceA < -Geometry.Epsilon)
            {
                intersection = IntersectionRayRay2.None();
                return false;
            }

            float distanceB = perpDotA/denominator;
            if (distanceB < -Geometry.Epsilon)
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
        public static bool RaySegment(Ray2D ray, Segment2 segment, out IntersectionRaySegment2 intersection)
        {
            return RaySegment(ray.origin, ray.direction, segment.a, segment.b, out intersection);
        }

        /// <summary>
        /// Computes an intersection of the ray and the segment
        /// </summary>
        public static bool RaySegment(Vector2 rayOrigin, Vector2 rayDirection, Vector2 segmentA, Vector2 segmentB,
            out IntersectionRaySegment2 intersection)
        {
            float rayDistance;
            float segmentDistance;
            Vector2 segmentDirection = segmentB - segmentA;
            var intersectionType = LineLine(rayOrigin, rayDirection, segmentA, segmentDirection, out rayDistance, out segmentDistance);
            if (intersectionType == IntersectionType.Line)
            {
                bool segmentIsAPoint = segmentDirection.sqrMagnitude < Geometry.Epsilon;
                float segmentAProjection = Vector2.Dot(rayDirection, segmentA - rayOrigin);
                if (segmentIsAPoint)
                {
                    if (segmentAProjection > -Geometry.Epsilon)
                    {
                        intersection = IntersectionRaySegment2.Point(segmentA);
                        return true;
                    }
                    intersection = IntersectionRaySegment2.None();
                    return false;
                }

                float segmentBProjection = Vector2.Dot(rayDirection, segmentB - rayOrigin);
                if (segmentAProjection > -Geometry.Epsilon)
                {
                    if (segmentBProjection > -Geometry.Epsilon)
                    {
                        if (segmentBProjection > segmentAProjection)
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
                        if (segmentAProjection > Geometry.Epsilon)
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
                if (segmentBProjection > -Geometry.Epsilon)
                {
                    if (segmentBProjection > Geometry.Epsilon)
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
                rayDistance > -Geometry.Epsilon &&
                segmentDistance > -Geometry.Epsilon && segmentDistance < 1 + Geometry.Epsilon)
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
        public static bool RayCircle(Ray2D ray, Circle circle, out Vector2 pointA, out Vector2 pointB)
        {
            return RayCircle(ray.origin, ray.direction, circle.center, circle.radius, out pointA, out pointB);
        }

        /// <summary>
        /// Computes an intersection of the ray and the circle
        /// </summary>
        public static bool RayCircle(Vector2 origin, Vector2 direction, Vector2 center, float radius, out Vector2 pointA, out Vector2 pointB)
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
        public static bool SegmentSegment(Segment2 segment1, Segment2 segment2, out IntersectionSegmentSegment2 intersection)
        {
            return SegmentSegment(segment1.a, segment1.b, segment2.a, segment2.b, out intersection);
        }

        /// <summary>
        /// Computes an intersection of the segments
        /// </summary>
        public static bool SegmentSegment(Vector2 segment1A, Vector2 segment1B, Vector2 segment2A, Vector2 segment2B,
            out IntersectionSegmentSegment2 intersection)
        {
            Vector2 from2ATo1A = segment1A - segment2A;
            Vector2 direction1 = segment1B - segment1A;
            Vector2 direction2 = segment2B - segment2A;
            float denominator = VectorE.PerpDot(direction1, direction2);
            float perpDot1 = VectorE.PerpDot(direction1, from2ATo1A);
            float perpDot2 = VectorE.PerpDot(direction2, from2ATo1A);

            if (Mathf.Abs(denominator) < Geometry.Epsilon)
            {
                // Parallel
                if (Mathf.Abs(perpDot1) > Geometry.Epsilon || Mathf.Abs(perpDot2) > Geometry.Epsilon)
                {
                    // Not collinear
                    intersection = IntersectionSegmentSegment2.None();
                    return false;
                }
                // Collinear or degenerate

                bool segment1IsAPoint = direction1.sqrMagnitude < Geometry.Epsilon;
                bool segment2IsAPoint = direction2.sqrMagnitude < Geometry.Epsilon;
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
                    float segment2AProjection = Vector2.Dot(direction1, from2ATo1A);
                    if (segment2AProjection > 0)
                    {
                        // 2A------2B
                        //     1A------1B
                        return SegmentSegmentCollinear(segment2A, segment2B, segment1A, segment1B, out intersection);
                    }
                    else
                    {
                        // 1A------1B
                        //     2A------2B
                        return SegmentSegmentCollinear(segment1A, segment1B, segment2A, segment2B, out intersection);
                    }
                }
                else
                {
                    // Contradirected
                    float segment2BProjection = Vector2.Dot(direction1, segment2B - segment1A);
                    if (segment2BProjection > 0)
                    {
                        // 1A------1B
                        //     2B------2A
                        return SegmentSegmentCollinear(segment1A, segment1B, segment2B, segment2A, out intersection);
                    }
                    else
                    {
                        // 2B------2A
                        //     1A------1B
                        return SegmentSegmentCollinear(segment2B, segment2A, segment1A, segment1B, out intersection);
                    }
                }
            }

            // The segments are skew
            float distance1 = perpDot2/denominator;
            if (distance1 < -Geometry.Epsilon || distance1 > 1 + Geometry.Epsilon)
            {
                intersection = IntersectionSegmentSegment2.None();
                return false;
            }

            float distance2 = perpDot1/denominator;
            if (distance2 < -Geometry.Epsilon || distance2 > 1 + Geometry.Epsilon)
            {
                intersection = IntersectionSegmentSegment2.None();
                return false;
            }

            intersection = IntersectionSegmentSegment2.Point(segment1A + direction1*distance1);
            return true;
        }

        private static bool CollinearPointInSegment(Vector2 segmentA, Vector2 segmentB, Vector2 point)
        {
            if (Mathf.Abs(segmentA.x - segmentB.x) < Geometry.Epsilon)
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

        private static bool SegmentSegmentCollinear(Vector2 leftA, Vector2 leftB, Vector2 rightA, Vector2 rightB,
            out IntersectionSegmentSegment2 intersection)
        {
            Vector2 leftDirection = leftB - leftA;
            float rightAProjection = Vector2.Dot(leftDirection, leftB - rightA);
            if (Mathf.Abs(rightAProjection) < Geometry.Epsilon)
            {
                // LB == RA
                // LA------LB
                //         RA------RB
                intersection = IntersectionSegmentSegment2.Point(leftB);
                return true;
            }
            if (rightAProjection > 0)
            {
                // LB > RA
                // LA------LB
                //     RARB
                //     RA--RB
                //     RA------RB
                Vector2 pointB;
                float rightBProjection = Vector2.Dot(leftDirection, rightB - leftA);
                if (rightBProjection > leftDirection.sqrMagnitude)
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
        /// Computes an intersection of the circles
        /// </summary>
        /// <returns>True if the circles intersect or one circle is contained within the other</returns>
        public static bool CircleCircle(Circle circleA, Circle circleB)
        {
            return Distance.CircleCircle(circleA, circleB) < Geometry.Epsilon;
        }

        /// <summary>
        /// Computes an intersection of the circles
        /// </summary>
        /// <returns>True if the circles intersect or one circle is contained within the other</returns>
        public static bool CircleCircle(Circle circleA, Circle circleB, out IntersectionCircleCircle intersection)
        {
            return CircleCircle(circleA.center, circleA.radius, circleB.center, circleB.radius, out intersection);
        }

        /// <summary>
        /// Computes an intersection of the circles
        /// </summary>
        /// <returns>True if the circles intersect or one circle is contained within the other</returns>
        public static bool CircleCircle(Vector2 centerA, float radiusA, Vector2 centerB, float radiusB)
        {
            return Distance.CircleCircle(centerA, radiusA, centerB, radiusB) < Geometry.Epsilon;
        }

        /// <summary>
        /// Computes an intersection of the circles
        /// </summary>
        /// <returns>True if the circles intersect or one circle is contained within the other</returns>
        public static bool CircleCircle(Vector2 centerA, float radiusA, Vector2 centerB, float radiusB,
            out IntersectionCircleCircle intersection)
        {
            Vector2 fromBtoA = centerA - centerB;
            float distanceFromBtoASqr = fromBtoA.sqrMagnitude;
            if (distanceFromBtoASqr < Geometry.Epsilon)
            {
                if (Mathf.Abs(radiusA - radiusB) < Geometry.Epsilon)
                {
                    // Circles are coincident
                    intersection = IntersectionCircleCircle.Circle();
                    return true;
                }
                // One circle is inside the other
                intersection = IntersectionCircleCircle.None();
                return true;
            }

            // For intersections on the circle's edge magnitude is more stable than sqrMagnitude
            float distanceFromBtoA = Mathf.Sqrt(distanceFromBtoASqr);

            float sumOfRadii = radiusA + radiusB;
            if (Mathf.Abs(distanceFromBtoA - sumOfRadii) < Geometry.Epsilon)
            {
                // One intersection outside
                intersection = IntersectionCircleCircle.Point(centerB + fromBtoA*(radiusB/sumOfRadii));
                return true;
            }
            if (distanceFromBtoA > sumOfRadii)
            {
                // No intersections, circles are separate
                intersection = IntersectionCircleCircle.None();
                return false;
            }

            float differenceOfRadii = radiusA - radiusB;
            float differenceOfRadiiAbs = Mathf.Abs(differenceOfRadii);
            if (Mathf.Abs(distanceFromBtoA - differenceOfRadiiAbs) < Geometry.Epsilon)
            {
                // One intersection inside
                intersection = IntersectionCircleCircle.Point(centerB - fromBtoA*(radiusB/differenceOfRadii));
                return true;
            }
            if (distanceFromBtoA < differenceOfRadiiAbs)
            {
                // One circle is contained within the other
                intersection = IntersectionCircleCircle.None();
                return true;
            }

            // Two intersections
            float radiusASqr = radiusA*radiusA;
            float distanceToMiddle = 0.5f*(radiusASqr - radiusB*radiusB)/distanceFromBtoASqr + 0.5f;
            Vector2 middle = centerA - fromBtoA*distanceToMiddle;

            float discriminant = radiusASqr/distanceFromBtoASqr - distanceToMiddle*distanceToMiddle;
            Vector2 offset = fromBtoA.Perp()*Mathf.Sqrt(discriminant);

            intersection = IntersectionCircleCircle.TwoPoints(middle + offset, middle - offset);
            return true;
        }

        #endregion Circle-Circle
    }
}
