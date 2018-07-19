using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace ProceduralToolkit
{
    /// <summary>
    /// Utility class for computational geometry algorithms
    /// </summary>
    public static class Geometry
    {
        public const float Epsilon = 0.00001f;

        #region Point samplers

        /// <summary>
        /// Returns a point on a circle in the XY plane
        /// </summary>
        /// <param name="radius">Circle radius</param>
        /// <param name="angle">Angle in degrees</param>
        public static Vector2 PointOnCircle2(float radius, float angle)
        {
            float angleInRadians = angle*Mathf.Deg2Rad;
            return new Vector2(radius*Mathf.Sin(angleInRadians), radius*Mathf.Cos(angleInRadians));
        }

        /// <summary>
        /// Returns a list of points on a circle in the XY plane
        /// </summary>
        /// <param name="radius">Circle radius</param>
        /// <param name="segments">Number of circle segments</param>
        public static List<Vector2> PointsOnCircle2(float radius, int segments)
        {
            float segmentAngle = 360f/segments;
            float currentAngle = 0;
            var ring = new List<Vector2>(segments);
            for (var i = 0; i < segments; i++)
            {
                ring.Add(PointOnCircle2(radius, currentAngle));
                currentAngle += segmentAngle;
            }
            return ring;
        }

        /// <summary>
        /// Returns a point on a circle in the XY plane
        /// </summary>
        /// <param name="radius">Circle radius</param>
        /// <param name="angle">Angle in degrees</param>
        public static Vector3 PointOnCircle3XY(float radius, float angle)
        {
            float angleInRadians = angle*Mathf.Deg2Rad;
            return new Vector3(radius*Mathf.Sin(angleInRadians), radius*Mathf.Cos(angleInRadians), 0);
        }

        /// <summary>
        /// Returns a list of points on a circle in the XY plane
        /// </summary>
        /// <param name="radius">Circle radius</param>
        /// <param name="segments">Number of circle segments</param>
        public static List<Vector3> PointsOnCircle3XY(float radius, int segments)
        {
            float segmentAngle = 360f/segments;
            float currentAngle = 0;
            var ring = new List<Vector3>(segments);
            for (var i = 0; i < segments; i++)
            {
                ring.Add(PointOnCircle3XY(radius, currentAngle));
                currentAngle += segmentAngle;
            }
            return ring;
        }

        /// <summary>
        /// Returns a point on a circle in the XZ plane
        /// </summary>
        /// <param name="radius">Circle radius</param>
        /// <param name="angle">Angle in degrees</param>
        public static Vector3 PointOnCircle3XZ(float radius, float angle)
        {
            float angleInRadians = angle*Mathf.Deg2Rad;
            return new Vector3(radius*Mathf.Sin(angleInRadians), 0, radius*Mathf.Cos(angleInRadians));
        }

        /// <summary>
        /// Returns a list of points on a circle in the XZ plane
        /// </summary>
        /// <param name="radius">Circle radius</param>
        /// <param name="segments">Number of circle segments</param>
        public static List<Vector3> PointsOnCircle3XZ(float radius, int segments)
        {
            float segmentAngle = 360f/segments;
            float currentAngle = 0;
            var ring = new List<Vector3>(segments);
            for (var i = 0; i < segments; i++)
            {
                ring.Add(PointOnCircle3XZ(radius, currentAngle));
                currentAngle += segmentAngle;
            }
            return ring;
        }

        /// <summary>
        /// Returns a point on a circle in the YZ plane
        /// </summary>
        /// <param name="radius">Circle radius</param>
        /// <param name="angle">Angle in degrees</param>
        public static Vector3 PointOnCircle3YZ(float radius, float angle)
        {
            float angleInRadians = angle*Mathf.Deg2Rad;
            return new Vector3(0, radius*Mathf.Sin(angleInRadians), radius*Mathf.Cos(angleInRadians));
        }

        /// <summary>
        /// Returns a list of points on a circle in the YZ plane
        /// </summary>
        /// <param name="radius">Circle radius</param>
        /// <param name="segments">Number of circle segments</param>
        public static List<Vector3> PointsOnCircle3YZ(float radius, int segments)
        {
            float segmentAngle = 360f/segments;
            float currentAngle = 0;
            var ring = new List<Vector3>(segments);
            for (var i = 0; i < segments; i++)
            {
                ring.Add(PointOnCircle3YZ(radius, currentAngle));
                currentAngle += segmentAngle;
            }
            return ring;
        }

        /// <summary>
        /// Returns a point on a sphere in geographic coordinate system
        /// </summary>
        /// <param name="radius">Sphere radius</param>
        /// <param name="horizontalAngle">Horizontal angle in degrees [0, 360]</param>
        /// <param name="verticalAngle">Vertical angle in degrees [-90, 90]</param>
        public static Vector3 PointOnSphere(float radius, float horizontalAngle, float verticalAngle)
        {
            return PointOnSpheroid(radius, radius, horizontalAngle, verticalAngle);
        }

        /// <summary>
        /// Returns a point on a spheroid in geographic coordinate system
        /// </summary>
        /// <param name="radius">Spheroid radius</param>
        /// <param name="height">Spheroid height</param>
        /// <param name="horizontalAngle">Horizontal angle in degrees [0, 360]</param>
        /// <param name="verticalAngle">Vertical angle in degrees [-90, 90]</param>
        public static Vector3 PointOnSpheroid(float radius, float height, float horizontalAngle, float verticalAngle)
        {
            float horizontalRadians = horizontalAngle*Mathf.Deg2Rad;
            float verticalRadians = verticalAngle*Mathf.Deg2Rad;
            float cosVertical = Mathf.Cos(verticalRadians);

            return new Vector3(
                x: radius*Mathf.Sin(horizontalRadians)*cosVertical,
                y: height*Mathf.Sin(verticalRadians),
                z: radius*Mathf.Cos(horizontalRadians)*cosVertical);
        }

        /// <summary>
        /// Returns a point on a teardrop surface in geographic coordinate system
        /// </summary>
        /// <param name="radius">Teardrop radius</param>
        /// <param name="height">Teardrop height</param>
        /// <param name="horizontalAngle">Horizontal angle in degrees [0, 360]</param>
        /// <param name="verticalAngle">Vertical angle in degrees [-90, 90]</param>
        public static Vector3 PointOnTeardrop(float radius, float height, float horizontalAngle, float verticalAngle)
        {
            float horizontalRadians = horizontalAngle*Mathf.Deg2Rad;
            float verticalRadians = verticalAngle*Mathf.Deg2Rad;
            float sinVertical = Mathf.Sin(verticalRadians);
            float teardrop = (1 - sinVertical)*Mathf.Cos(verticalRadians)/2;

            return new Vector3(
                x: radius*Mathf.Sin(horizontalRadians)*teardrop,
                y: height*sinVertical,
                z: radius*Mathf.Cos(horizontalRadians)*teardrop);
        }

        #endregion Point samplers

        /// <summary>
        /// Returns the bisector of an angle. Assumes clockwise order of the polygon.
        /// </summary>
        /// <param name="previous">Previous vertex</param>
        /// <param name="current">Current vertex</param>
        /// <param name="next">Next vertex</param>
        /// <param name="degrees">Value of the angle in degrees. Always positive.</param>
        public static Vector2 GetAngleBisector(Vector2 previous, Vector2 current, Vector2 next, out float degrees)
        {
            Vector2 toPrevious = (previous - current).normalized;
            Vector2 toNext = (next - current).normalized;

            degrees = VectorE.Angle360(toNext, toPrevious);
            Assert.IsFalse(float.IsNaN(degrees));
            return toNext.RotateCW(degrees/2);
        }

        /// <summary>
        /// Creates a new offset polygon from the input polygon. Assumes clockwise order of the polygon.
        /// Does not handle intersections.
        /// </summary>
        /// <param name="polygon">Vertices of the polygon in clockwise order.</param>
        /// <param name="distance">Offset distance. Positive values offset outside, negative inside.</param>
        public static List<Vector2> OffsetPolygon(IList<Vector2> polygon, float distance)
        {
            var newPolygon = new List<Vector2>(polygon.Count);
            for (int i = 0; i < polygon.Count; i++)
            {
                Vector2 previous = polygon.GetLooped(i - 1);
                Vector2 current = polygon[i];
                Vector2 next = polygon.GetLooped(i + 1);

                float angle;
                Vector2 bisector = GetAngleBisector(previous, current, next, out angle);
                float angleOffset = GetAngleOffset(distance, angle);
                newPolygon.Add(current - bisector*angleOffset);
            }
            return newPolygon;
        }

        /// <summary>
        /// Offsets the input polygon. Assumes clockwise order of the polygon.
        /// Does not handle intersections.
        /// </summary>
        /// <param name="polygon">Vertices of the polygon in clockwise order.</param>
        /// <param name="distance">Offset distance. Positive values offset outside, negative inside.</param>
        public static void OffsetPolygon(ref List<Vector2> polygon, float distance)
        {
            var offsets = new Vector2[polygon.Count];
            for (int i = 0; i < polygon.Count; i++)
            {
                Vector2 previous = polygon.GetLooped(i - 1);
                Vector2 current = polygon[i];
                Vector2 next = polygon.GetLooped(i + 1);

                float angle;
                Vector2 bisector = GetAngleBisector(previous, current, next, out angle);
                float angleOffset = GetAngleOffset(distance, angle);
                offsets[i] = -bisector*angleOffset;
            }

            for (int i = 0; i < polygon.Count; i++)
            {
                polygon[i] += offsets[i];
            }
        }

        /// <summary>
        /// Offsets the input polygon. Assumes clockwise order of the polygon.
        /// Does not handle intersections.
        /// </summary>
        /// <param name="polygon">Vertices of the polygon in clockwise order.</param>
        /// <param name="distance">Offset distance. Positive values offset outside, negative inside.</param>
        public static void OffsetPolygon(ref Vector2[] polygon, float distance)
        {
            var offsets = new Vector2[polygon.Length];
            for (int i = 0; i < polygon.Length; i++)
            {
                Vector2 previous = polygon.GetLooped(i - 1);
                Vector2 current = polygon[i];
                Vector2 next = polygon.GetLooped(i + 1);

                float angle;
                Vector2 bisector = GetAngleBisector(previous, current, next, out angle);
                float angleOffset = GetAngleOffset(distance, angle);
                offsets[i] = -bisector*angleOffset;
            }

            for (int i = 0; i < polygon.Length; i++)
            {
                polygon[i] += offsets[i];
            }
        }

        public static float GetAngleOffset(float edgeOffset, float angle)
        {
            return edgeOffset/GetAngleBisectorSin(angle);
        }

        public static float GetAngleBisectorSin(float angle)
        {
            return Mathf.Sin(angle*Mathf.Deg2Rad/2);
        }
    }
}
