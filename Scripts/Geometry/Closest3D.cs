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
        public static Vector3 PointLine(Vector3 point, Line3 line)
        {
            float projectedX;
            return PointLine(point, line.origin, line.direction, out projectedX);
        }

        /// <summary>
        /// Projects the point onto the line
        /// </summary>
        /// <param name="projectedX">Position of the projected point on the line relative to the origin</param>
        public static Vector3 PointLine(Vector3 point, Line3 line, out float projectedX)
        {
            return PointLine(point, line.origin, line.direction, out projectedX);
        }

        /// <summary>
        /// Projects the point onto the line
        /// </summary>
        /// <param name="lineDirection">Normalized direction of the line</param>
        public static Vector3 PointLine(Vector3 point, Vector3 lineOrigin, Vector3 lineDirection)
        {
            float projectedX;
            return PointLine(point, lineOrigin, lineDirection, out projectedX);
        }

        /// <summary>
        /// Projects the point onto the line
        /// </summary>
        /// <param name="lineDirection">Normalized direction of the line</param>
        /// <param name="projectedX">Position of the projected point on the line relative to the origin</param>
        public static Vector3 PointLine(Vector3 point, Vector3 lineOrigin, Vector3 lineDirection, out float projectedX)
        {
            // In theory, sqrMagnitude should be 1, but in practice this division helps with numerical stability
            projectedX = Vector3.Dot(lineDirection, point - lineOrigin)/lineDirection.sqrMagnitude;
            return lineOrigin + lineDirection*projectedX;
        }

        #endregion Point-Line

        #region Point-Ray

        /// <summary>
        /// Projects the point onto the ray
        /// </summary>
        public static Vector3 PointRay(Vector3 point, Ray ray)
        {
            float projectedX;
            return PointRay(point, ray.origin, ray.direction, out projectedX);
        }

        /// <summary>
        /// Projects the point onto the ray
        /// </summary>
        /// <param name="projectedX">Position of the projected point on the ray relative to the origin</param>
        public static Vector3 PointRay(Vector3 point, Ray ray, out float projectedX)
        {
            return PointRay(point, ray.origin, ray.direction, out projectedX);
        }

        /// <summary>
        /// Projects the point onto the ray
        /// </summary>
        /// <param name="rayDirection">Normalized direction of the ray</param>
        public static Vector3 PointRay(Vector3 point, Vector3 rayOrigin, Vector3 rayDirection)
        {
            float projectedX;
            return PointRay(point, rayOrigin, rayDirection, out projectedX);
        }

        /// <summary>
        /// Projects the point onto the ray
        /// </summary>
        /// <param name="rayDirection">Normalized direction of the ray</param>
        /// <param name="projectedX">Position of the projected point on the ray relative to the origin</param>
        public static Vector3 PointRay(Vector3 point, Vector3 rayOrigin, Vector3 rayDirection, out float projectedX)
        {
            Vector3 toPoint = point - rayOrigin;
            float pointProjection = Vector3.Dot(rayDirection, toPoint);
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
        public static Vector3 PointSegment(Vector3 point, Segment3 segment)
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
        public static Vector3 PointSegment(Vector3 point, Segment3 segment, out float projectedX)
        {
            return PointSegment(point, segment.a, segment.b, out projectedX);
        }

        /// <summary>
        /// Projects the point onto the segment
        /// </summary>
        public static Vector3 PointSegment(Vector3 point, Vector3 segmentA, Vector3 segmentB)
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
        public static Vector3 PointSegment(Vector3 point, Vector3 segmentA, Vector3 segmentB, out float projectedX)
        {
            Vector3 segmentDirection = segmentB - segmentA;
            float sqrSegmentLength = segmentDirection.sqrMagnitude;
            if (sqrSegmentLength < Geometry.Epsilon)
            {
                // The segment is a point
                projectedX = 0;
                return segmentA;
            }

            float pointProjection = Vector3.Dot(segmentDirection, point - segmentA);
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

        #endregion Point-Segment

        #region Point-Sphere

        /// <summary>
        /// Projects the point onto the sphere
        /// </summary>
        public static Vector3 PointSphere(Vector3 point, Sphere sphere)
        {
            return PointSphere(point, sphere.center, sphere.radius);
        }

        /// <summary>
        /// Projects the point onto the sphere
        /// </summary>
        public static Vector3 PointSphere(Vector3 point, Vector3 sphereCenter, float sphereRadius)
        {
            return sphereCenter + (point - sphereCenter).normalized*sphereRadius;
        }

        #endregion Point-Sphere

        #region Line-Sphere

        /// <summary>
        /// Finds closest points on the line and the sphere
        /// </summary>
        public static void LineSphere(Line3 line, Sphere sphere, out Vector3 linePoint, out Vector3 spherePoint)
        {
            LineSphere(line.origin, line.direction, sphere.center, sphere.radius, out linePoint, out spherePoint);
        }

        /// <summary>
        /// Finds closest points on the line and the sphere
        /// </summary>
        public static void LineSphere(Vector3 lineOrigin, Vector3 lineDirection, Vector3 sphereCenter, float sphereRadius,
            out Vector3 linePoint, out Vector3 spherePoint)
        {
            Vector3 originToCenter = sphereCenter - lineOrigin;
            float centerProjection = Vector3.Dot(lineDirection, originToCenter);
            float sqrDistanceToLine = originToCenter.sqrMagnitude - centerProjection*centerProjection;
            float sqrDistanceToIntersection = sphereRadius*sphereRadius - sqrDistanceToLine;
            if (sqrDistanceToIntersection < -Geometry.Epsilon)
            {
                // No intersection
                linePoint = lineOrigin + lineDirection*centerProjection;
                spherePoint = sphereCenter + (linePoint - sphereCenter).normalized*sphereRadius;
                return;
            }
            if (sqrDistanceToIntersection < Geometry.Epsilon)
            {
                // Point intersection
                linePoint = spherePoint = lineOrigin + lineDirection*centerProjection;
                return;
            }

            // Two points intersection
            float distanceToIntersection = Mathf.Sqrt(sqrDistanceToIntersection);
            float distanceA = centerProjection - distanceToIntersection;
            linePoint = spherePoint = lineOrigin + lineDirection*distanceA;
        }

        #endregion Line-Sphere

        #region Ray-Sphere

        /// <summary>
        /// Finds closest points on the ray and the sphere
        /// </summary>
        public static void RaySphere(Ray ray, Sphere sphere, out Vector3 rayPoint, out Vector3 spherePoint)
        {
            RaySphere(ray.origin, ray.direction, sphere.center, sphere.radius, out rayPoint, out spherePoint);
        }

        /// <summary>
        /// Finds closest points on the ray and the sphere
        /// </summary>
        public static void RaySphere(Vector3 rayOrigin, Vector3 rayDirection, Vector3 sphereCenter, float sphereRadius,
            out Vector3 rayPoint, out Vector3 spherePoint)
        {
            Vector3 originToCenter = sphereCenter - rayOrigin;
            float centerProjection = Vector3.Dot(rayDirection, originToCenter);
            if (centerProjection + sphereRadius < -Geometry.Epsilon)
            {
                // No intersection
                rayPoint = rayOrigin;
                spherePoint = sphereCenter - originToCenter.normalized*sphereRadius;
                return;
            }

            float sqrDistanceToLine = originToCenter.sqrMagnitude - centerProjection*centerProjection;
            float sqrDistanceToIntersection = sphereRadius*sphereRadius - sqrDistanceToLine;
            if (sqrDistanceToIntersection < -Geometry.Epsilon)
            {
                // No intersection
                if (centerProjection < -Geometry.Epsilon)
                {
                    rayPoint = rayOrigin;
                    spherePoint = sphereCenter - originToCenter.normalized*sphereRadius;
                    return;
                }
                rayPoint = rayOrigin + rayDirection*centerProjection;
                spherePoint = sphereCenter + (rayPoint - sphereCenter).normalized*sphereRadius;
                return;
            }
            if (sqrDistanceToIntersection < Geometry.Epsilon)
            {
                if (centerProjection < -Geometry.Epsilon)
                {
                    // No intersection
                    rayPoint = rayOrigin;
                    spherePoint = sphereCenter - originToCenter.normalized*sphereRadius;
                    return;
                }
                // Point intersection
                rayPoint = spherePoint = rayOrigin + rayDirection*centerProjection;
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
                    spherePoint = sphereCenter - originToCenter.normalized*sphereRadius;
                    return;
                }

                // Point intersection
                rayPoint = spherePoint = rayOrigin + rayDirection*distanceB;
                return;
            }

            // Two points intersection
            rayPoint = spherePoint = rayOrigin + rayDirection*distanceA;
        }

        #endregion Ray-Sphere

        #region Segment-Sphere

        /// <summary>
        /// Finds closest points on the segment and the sphere
        /// </summary>
        public static void SegmentSphere(Segment3 segment, Sphere sphere, out Vector3 segmentPoint, out Vector3 spherePoint)
        {
            SegmentSphere(segment.a, segment.b, sphere.center, sphere.radius, out segmentPoint, out spherePoint);
        }

        /// <summary>
        /// Finds closest points on the segment and the sphere
        /// </summary>
        public static void SegmentSphere(Vector3 segmentA, Vector3 segmentB, Vector3 sphereCenter, float sphereRadius,
            out Vector3 segmentPoint, out Vector3 spherePoint)
        {
            Vector3 segmentAToCenter = sphereCenter - segmentA;
            Vector3 fromAtoB = segmentB - segmentA;
            float segmentLength = fromAtoB.magnitude;
            if (segmentLength < Geometry.Epsilon)
            {
                segmentPoint = segmentA;
                float distanceToPoint = segmentAToCenter.magnitude;
                if (distanceToPoint < sphereRadius + Geometry.Epsilon)
                {
                    if (distanceToPoint > sphereRadius - Geometry.Epsilon)
                    {
                        spherePoint = segmentPoint;
                        return;
                    }
                    if (distanceToPoint < Geometry.Epsilon)
                    {
                        spherePoint = segmentPoint;
                        return;
                    }
                }
                Vector3 toPoint = -segmentAToCenter/distanceToPoint;
                spherePoint = sphereCenter + toPoint*sphereRadius;
                return;
            }

            Vector3 segmentDirection = fromAtoB.normalized;
            float centerProjection = Vector3.Dot(segmentDirection, segmentAToCenter);
            if (centerProjection + sphereRadius < -Geometry.Epsilon ||
                centerProjection - sphereRadius > segmentLength + Geometry.Epsilon)
            {
                // No intersection
                if (centerProjection < 0)
                {
                    segmentPoint = segmentA;
                    spherePoint = sphereCenter - segmentAToCenter.normalized*sphereRadius;
                    return;
                }
                segmentPoint = segmentB;
                spherePoint = sphereCenter - (sphereCenter - segmentB).normalized*sphereRadius;
                return;
            }

            float sqrDistanceToLine = segmentAToCenter.sqrMagnitude - centerProjection*centerProjection;
            float sqrDistanceToIntersection = sphereRadius*sphereRadius - sqrDistanceToLine;
            if (sqrDistanceToIntersection < -Geometry.Epsilon)
            {
                // No intersection
                if (centerProjection < -Geometry.Epsilon)
                {
                    segmentPoint = segmentA;
                    spherePoint = sphereCenter - segmentAToCenter.normalized*sphereRadius;
                    return;
                }
                if (centerProjection > segmentLength + Geometry.Epsilon)
                {
                    segmentPoint = segmentB;
                    spherePoint = sphereCenter - (sphereCenter - segmentB).normalized*sphereRadius;
                    return;
                }
                segmentPoint = segmentA + segmentDirection*centerProjection;
                spherePoint = sphereCenter + (segmentPoint - sphereCenter).normalized*sphereRadius;
                return;
            }

            if (sqrDistanceToIntersection < Geometry.Epsilon)
            {
                if (centerProjection < -Geometry.Epsilon)
                {
                    // No intersection
                    segmentPoint = segmentA;
                    spherePoint = sphereCenter - segmentAToCenter.normalized*sphereRadius;
                    return;
                }
                if (centerProjection > segmentLength + Geometry.Epsilon)
                {
                    // No intersection
                    segmentPoint = segmentB;
                    spherePoint = sphereCenter - (sphereCenter - segmentB).normalized*sphereRadius;
                    return;
                }
                // Point intersection
                segmentPoint = spherePoint = segmentA + segmentDirection*centerProjection;
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
                segmentPoint = spherePoint = segmentA + segmentDirection*distanceA;
                return;
            }
            if (!pointAIsAfterSegmentA && !pointBIsBeforeSegmentB)
            {
                // The segment is inside, but no intersection
                if (distanceA > -(distanceB - segmentLength))
                {
                    segmentPoint = segmentA;
                    spherePoint = segmentA + segmentDirection*distanceA;
                    return;
                }
                segmentPoint = segmentB;
                spherePoint = segmentA + segmentDirection*distanceB;
                return;
            }

            bool pointAIsBeforeSegmentB = distanceA < segmentLength + Geometry.Epsilon;
            if (pointAIsAfterSegmentA && pointAIsBeforeSegmentB)
            {
                // Point A intersection
                segmentPoint = spherePoint = segmentA + segmentDirection*distanceA;
                return;
            }
            bool pointBIsAfterSegmentA = distanceB > -Geometry.Epsilon;
            if (pointBIsAfterSegmentA && pointBIsBeforeSegmentB)
            {
                // Point B intersection
                segmentPoint = spherePoint = segmentA + segmentDirection*distanceB;
                return;
            }

            // No intersection
            if (centerProjection < 0)
            {
                segmentPoint = segmentA;
                spherePoint = sphereCenter - segmentAToCenter.normalized*sphereRadius;
                return;
            }
            segmentPoint = segmentB;
            spherePoint = sphereCenter - (sphereCenter - segmentB).normalized*sphereRadius;
        }

        #endregion Segment-Sphere

        #region Sphere-Sphere

        /// <summary>
        /// Finds closest points on the spheres
        /// </summary>
        public static void SphereSphere(Sphere sphereA, Sphere sphereB, out Vector3 pointA, out Vector3 pointB)
        {
            SphereSphere(sphereA.center, sphereA.radius, sphereB.center, sphereB.radius, out pointA, out pointB);
        }

        /// <summary>
        /// Finds closest points on the spheres
        /// </summary>
        public static void SphereSphere(Vector3 centerA, float radiusA, Vector3 centerB, float radiusB,
            out Vector3 pointA, out Vector3 pointB)
        {
            Vector3 fromBtoA = (centerA - centerB).normalized;
            pointA = centerA - fromBtoA*radiusA;
            pointB = centerB + fromBtoA*radiusB;
        }

        #endregion Sphere-Sphere
    }
}
