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
        /// <summary>
        /// A tiny floating point value used in comparisons
        /// </summary>
        public const float Epsilon = 0.00001f;

        #region Point samplers 2D

        /// <summary>
        /// Returns a point on a segment at the given normalized position
        /// </summary>
        /// <param name="segmentA">Start of the segment</param>
        /// <param name="segmentB">End of the segment</param>
        /// <param name="position">Normalized position</param>
        public static Vector2 PointOnSegment2(Vector2 segmentA, Vector2 segmentB, float position)
        {
            return Vector2.Lerp(segmentA, segmentB, position);
        }

        /// <summary>
        /// Returns a list of evenly distributed points on a segment
        /// </summary>
        /// <param name="segmentA">Start of the segment</param>
        /// <param name="segmentB">End of the segment</param>
        /// <param name="count">Number of points</param>
        public static List<Vector2> PointsOnSegment2(Vector2 segmentA, Vector2 segmentB, int count)
        {
            var points = new List<Vector2>(count);
            if (count <= 0)
            {
                return points;
            }
            if (count == 1)
            {
                points.Add(segmentA);
                return points;
            }
            for (int i = 0; i < count; i++)
            {
                points.Add(PointOnSegment2(segmentA, segmentB, i/(float) (count - 1)));
            }
            return points;
        }

        #region PointOnCircle2

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
        /// Returns a point on a circle in the XY plane
        /// </summary>
        /// <param name="center">Center of the circle</param>
        /// <param name="radius">Circle radius</param>
        /// <param name="angle">Angle in degrees</param>
        public static Vector2 PointOnCircle2(Vector2 center, float radius, float angle)
        {
            return center + PointOnCircle2(radius, angle);
        }

        #endregion PointOnCircle2

        #region PointsOnCircle2

        /// <summary>
        /// Returns a list of evenly distributed points on a circle in the XY plane
        /// </summary>
        /// <param name="radius">Circle radius</param>
        /// <param name="count">Number of points</param>
        public static List<Vector2> PointsOnCircle2(float radius, int count)
        {
            float segmentAngle = 360f/count;
            float currentAngle = 0;
            var points = new List<Vector2>(count);
            for (var i = 0; i < count; i++)
            {
                points.Add(PointOnCircle2(radius, currentAngle));
                currentAngle += segmentAngle;
            }
            return points;
        }

        /// <summary>
        /// Returns a list of evenly distributed points on a circle in the XY plane
        /// </summary>
        /// <param name="center">Center of the circle</param>
        /// <param name="radius">Circle radius</param>
        /// <param name="count">Number of points</param>
        public static List<Vector2> PointsOnCircle2(Vector2 center, float radius, int count)
        {
            float segmentAngle = 360f/count;
            float currentAngle = 0;
            var points = new List<Vector2>(count);
            for (var i = 0; i < count; i++)
            {
                points.Add(PointOnCircle2(center, radius, currentAngle));
                currentAngle += segmentAngle;
            }
            return points;
        }

        #endregion PointsOnCircle2

        #region PointsInCircle2

        /// <summary>
        /// Returns a list of evenly distributed points inside a circle in the XY plane
        /// </summary>
        /// <param name="radius">Circle radius</param>
        /// <param name="count">Number of points</param>
        public static List<Vector2> PointsInCircle2(float radius, int count)
        {
            float currentAngle = 0;
            var points = new List<Vector2>(count);
            for (int i = 0; i < count; i++)
            {
                // The 0.5 offset improves the position of the first point
                float r = Mathf.Sqrt((i + 0.5f)/count);
                points.Add(new Vector2(radius*Mathf.Sin(currentAngle)*r, radius*Mathf.Cos(currentAngle)*r));
                currentAngle += PTUtils.GoldenAngle;
            }
            return points;
        }

        /// <summary>
        /// Returns a list of evenly distributed points inside a circle in the XY plane
        /// </summary>
        /// <param name="center">Center of the circle</param>
        /// <param name="radius">Circle radius</param>
        /// <param name="count">Number of points</param>
        public static List<Vector2> PointsInCircle2(Vector2 center, float radius, int count)
        {
            float currentAngle = 0;
            var points = new List<Vector2>(count);
            for (int i = 0; i < count; i++)
            {
                // The 0.5 offset improves the position of the first point
                float r = Mathf.Sqrt((i + 0.5f)/count);
                points.Add(center + new Vector2(radius*Mathf.Sin(currentAngle)*r, radius*Mathf.Cos(currentAngle)*r));
                currentAngle += PTUtils.GoldenAngle;
            }
            return points;
        }

        #endregion PointsInCircle2

        #endregion Point samplers 2D

        #region Point samplers 3D

        /// <summary>
        /// Returns a point on a segment at the given normalized position
        /// </summary>
        /// <param name="segmentA">Start of the segment</param>
        /// <param name="segmentB">End of the segment</param>
        /// <param name="position">Normalized position</param>
        public static Vector3 PointOnSegment3(Vector3 segmentA, Vector3 segmentB, float position)
        {
            return Vector3.Lerp(segmentA, segmentB, position);
        }

        /// <summary>
        /// Returns a list of evenly distributed points on a segment
        /// </summary>
        /// <param name="segmentA">Start of the segment</param>
        /// <param name="segmentB">End of the segment</param>
        /// <param name="count">Number of points</param>
        public static List<Vector3> PointsOnSegment3(Vector3 segmentA, Vector3 segmentB, int count)
        {
            var points = new List<Vector3>(count);
            if (count <= 0)
            {
                return points;
            }
            if (count == 1)
            {
                points.Add(segmentA);
                return points;
            }
            for (int i = 0; i < count; i++)
            {
                points.Add(PointOnSegment3(segmentA, segmentB, i/(float) (count - 1)));
            }
            return points;
        }

        #region PointOnCircle3

        /// <summary>
        /// Returns a point on a circle in the XY plane
        /// </summary>
        /// <param name="radius">Circle radius</param>
        /// <param name="angle">Angle in degrees</param>
        public static Vector3 PointOnCircle3XY(float radius, float angle)
        {
            return PointOnCircle3(0, 1, radius, angle);
        }

        /// <summary>
        /// Returns a point on a circle in the XY plane
        /// </summary>
        /// <param name="center">Center of the circle</param>
        /// <param name="radius">Circle radius</param>
        /// <param name="angle">Angle in degrees</param>
        public static Vector3 PointOnCircle3XY(Vector3 center, float radius, float angle)
        {
            return PointOnCircle3(0, 1, center, radius, angle);
        }

        /// <summary>
        /// Returns a point on a circle in the XZ plane
        /// </summary>
        /// <param name="radius">Circle radius</param>
        /// <param name="angle">Angle in degrees</param>
        public static Vector3 PointOnCircle3XZ(float radius, float angle)
        {
            return PointOnCircle3(0, 2, radius, angle);
        }

        /// <summary>
        /// Returns a point on a circle in the XZ plane
        /// </summary>
        /// <param name="center">Center of the circle</param>
        /// <param name="radius">Circle radius</param>
        /// <param name="angle">Angle in degrees</param>
        public static Vector3 PointOnCircle3XZ(Vector3 center, float radius, float angle)
        {
            return PointOnCircle3(0, 2, center, radius, angle);
        }

        /// <summary>
        /// Returns a point on a circle in the YZ plane
        /// </summary>
        /// <param name="radius">Circle radius</param>
        /// <param name="angle">Angle in degrees</param>
        public static Vector3 PointOnCircle3YZ(float radius, float angle)
        {
            return PointOnCircle3(1, 2, radius, angle);
        }

        /// <summary>
        /// Returns a point on a circle in the YZ plane
        /// </summary>
        /// <param name="center">Center of the circle</param>
        /// <param name="radius">Circle radius</param>
        /// <param name="angle">Angle in degrees</param>
        public static Vector3 PointOnCircle3YZ(Vector3 center, float radius, float angle)
        {
            return PointOnCircle3(1, 2, center, radius, angle);
        }

        private static Vector3 PointOnCircle3(int xIndex, int yIndex, float radius, float angle)
        {
            float angleInRadians = angle*Mathf.Deg2Rad;
            var point = new Vector3();
            point[xIndex] = radius*Mathf.Sin(angleInRadians);
            point[yIndex] = radius*Mathf.Cos(angleInRadians);
            return point;
        }

        private static Vector3 PointOnCircle3(int xIndex, int yIndex, Vector3 center, float radius, float angle)
        {
            return center + PointOnCircle3(xIndex, yIndex, radius, angle);
        }

        #endregion PointOnCircle3

        #region PointsOnCircle3

        /// <summary>
        /// Returns a list of evenly distributed points on a circle in the XY plane
        /// </summary>
        /// <param name="radius">Circle radius</param>
        /// <param name="count">Number of points</param>
        public static List<Vector3> PointsOnCircle3XY(float radius, int count)
        {
            return PointsOnCircle3(0, 1, radius, count);
        }

        /// <summary>
        /// Returns a list of evenly distributed points on a circle in the XY plane
        /// </summary>
        /// <param name="center">Center of the circle</param>
        /// <param name="radius">Circle radius</param>
        /// <param name="count">Number of points</param>
        public static List<Vector3> PointsOnCircle3XY(Vector3 center, float radius, int count)
        {
            return PointsOnCircle3(0, 1, center, radius, count);
        }

        /// <summary>
        /// Returns a list of evenly distributed points on a circle in the XZ plane
        /// </summary>
        /// <param name="radius">Circle radius</param>
        /// <param name="count">Number of points</param>
        public static List<Vector3> PointsOnCircle3XZ(float radius, int count)
        {
            return PointsOnCircle3(0, 2, radius, count);
        }

        /// <summary>
        /// Returns a list of evenly distributed points on a circle in the XZ plane
        /// </summary>
        /// <param name="center">Center of the circle</param>
        /// <param name="radius">Circle radius</param>
        /// <param name="count">Number of points</param>
        public static List<Vector3> PointsOnCircle3XZ(Vector3 center, float radius, int count)
        {
            return PointsOnCircle3(0, 2, center, radius, count);
        }

        /// <summary>
        /// Returns a list of evenly distributed points on a circle in the YZ plane
        /// </summary>
        /// <param name="radius">Circle radius</param>
        /// <param name="count">Number of points</param>
        public static List<Vector3> PointsOnCircle3YZ(float radius, int count)
        {
            return PointsOnCircle3(1, 2, radius, count);
        }

        /// <summary>
        /// Returns a list of evenly distributed points on a circle in the YZ plane
        /// </summary>
        /// <param name="center">Center of the circle</param>
        /// <param name="radius">Circle radius</param>
        /// <param name="count">Number of points</param>
        public static List<Vector3> PointsOnCircle3YZ(Vector3 center, float radius, int count)
        {
            return PointsOnCircle3(1, 2, center, radius, count);
        }

        private static List<Vector3> PointsOnCircle3(int xIndex, int yIndex, float radius, int count)
        {
            float segmentAngle = 360f/count;
            float currentAngle = 0;
            var points = new List<Vector3>(count);
            for (var i = 0; i < count; i++)
            {
                points.Add(PointOnCircle3(xIndex, yIndex, radius, currentAngle));
                currentAngle += segmentAngle;
            }
            return points;
        }

        private static List<Vector3> PointsOnCircle3(int xIndex, int yIndex, Vector3 center, float radius, int count)
        {
            float segmentAngle = 360f/count;
            float currentAngle = 0;
            var points = new List<Vector3>(count);
            for (var i = 0; i < count; i++)
            {
                points.Add(PointOnCircle3(xIndex, yIndex, center, radius, currentAngle));
                currentAngle += segmentAngle;
            }
            return points;
        }

        #endregion PointsOnCircle3

        #region PointsInCircle3

        /// <summary>
        /// Returns a list of evenly distributed points inside a circle in the XY plane
        /// </summary>
        /// <param name="radius">Circle radius</param>
        /// <param name="count">Number of points</param>
        public static List<Vector3> PointsInCircle3XY(float radius, int count)
        {
            return PointsInCircle3(0, 1, radius, count);
        }

        /// <summary>
        /// Returns a list of evenly distributed points inside a circle in the XY plane
        /// </summary>
        /// <param name="radius">Circle radius</param>
        /// <param name="count">Number of points</param>
        public static List<Vector3> PointsInCircle3XY(Vector3 center, float radius, int count)
        {
            return PointsInCircle3(0, 1, center, radius, count);
        }

        /// <summary>
        /// Returns a list of evenly distributed points inside a circle in the XZ plane
        /// </summary>
        /// <param name="radius">Circle radius</param>
        /// <param name="count">Number of points</param>
        public static List<Vector3> PointsInCircle3XZ(float radius, int count)
        {
            return PointsInCircle3(0, 2, radius, count);
        }

        /// <summary>
        /// Returns a list of evenly distributed points inside a circle in the XZ plane
        /// </summary>
        /// <param name="radius">Circle radius</param>
        /// <param name="count">Number of points</param>
        public static List<Vector3> PointsInCircle3XZ(Vector3 center, float radius, int count)
        {
            return PointsInCircle3(0, 2, center, radius, count);
        }

        /// <summary>
        /// Returns a list of evenly distributed points inside a circle in the YZ plane
        /// </summary>
        /// <param name="radius">Circle radius</param>
        /// <param name="count">Number of points</param>
        public static List<Vector3> PointsInCircle3YZ(float radius, int count)
        {
            return PointsInCircle3(1, 2, radius, count);
        }

        /// <summary>
        /// Returns a list of evenly distributed points inside a circle in the YZ plane
        /// </summary>
        /// <param name="radius">Circle radius</param>
        /// <param name="count">Number of points</param>
        public static List<Vector3> PointsInCircle3YZ(Vector3 center, float radius, int count)
        {
            return PointsInCircle3(1, 2, center, radius, count);
        }

        private static List<Vector3> PointsInCircle3(int xIndex, int yIndex, float radius, int count)
        {
            float currentAngle = 0;
            var points = new List<Vector3>(count);
            for (int i = 0; i < count; i++)
            {
                // The 0.5 offset improves the position of the first point
                float r = Mathf.Sqrt((i + 0.5f)/count);
                var point = new Vector3();
                point[xIndex] = radius*Mathf.Sin(currentAngle)*r;
                point[yIndex] = radius*Mathf.Cos(currentAngle)*r;
                points.Add(point);
                currentAngle += PTUtils.GoldenAngle;
            }
            return points;
        }

        private static List<Vector3> PointsInCircle3(int xIndex, int yIndex, Vector3 center, float radius, int count)
        {
            float currentAngle = 0;
            var points = new List<Vector3>(count);
            for (int i = 0; i < count; i++)
            {
                // The 0.5 offset improves the position of the first point
                float r = Mathf.Sqrt((i + 0.5f)/count);
                var point = new Vector3();
                point[xIndex] = radius*Mathf.Sin(currentAngle)*r;
                point[yIndex] = radius*Mathf.Cos(currentAngle)*r;
                points.Add(center + point);
                currentAngle += PTUtils.GoldenAngle;
            }
            return points;
        }

        #endregion PointsInCircle3

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

        /// <summary>
        /// Returns a list of evenly distributed points on a sphere
        /// </summary>
        /// <param name="radius">Sphere radius</param>
        /// <param name="count">Number of points</param>
        public static List<Vector3> PointsOnSphere(float radius, int count)
        {
            var points = new List<Vector3>(count);
            float deltaY = -2f/count;
            float y = 1 + deltaY/2;
            float currentAngle = 0;
            for (int i = 0; i < count; i++)
            {
                float r = Mathf.Sqrt(1 - y*y);
                points.Add(new Vector3(
                    x: radius*Mathf.Sin(currentAngle)*r,
                    y: radius*y,
                    z: radius*Mathf.Cos(currentAngle)*r));
                y += deltaY;
                currentAngle += PTUtils.GoldenAngle;
            }
            return points;
        }

        #endregion Point samplers 3D

        /// <summary>
        /// Returns a list of points representing a polygon in the XY plane
        /// </summary>
        /// <param name="radius">Radius of the circle passing through the vertices</param>
        /// <param name="vertices">Number of polygon vertices</param>
        public static List<Vector2> Polygon2(int vertices, float radius)
        {
            return PointsOnCircle2(radius, vertices);
        }

        /// <summary>
        /// Returns a list of points representing a star polygon in the XY plane
        /// </summary>
        /// <param name="innerRadius">Radius of the circle passing through the outer vertices</param>
        /// <param name="outerRadius">Radius of the circle passing through the inner vertices</param>
        /// <param name="vertices">Number of polygon vertices</param>
        public static List<Vector2> StarPolygon2(int vertices, float innerRadius, float outerRadius)
        {
            float segmentAngle = 360f/vertices;
            float halfSegmentAngle = segmentAngle/2;
            float currentAngle = 0;
            var polygon = new List<Vector2>(vertices);
            for (var i = 0; i < vertices; i++)
            {
                polygon.Add(PointOnCircle2(outerRadius, currentAngle));
                polygon.Add(PointOnCircle2(innerRadius, currentAngle + halfSegmentAngle));
                currentAngle += segmentAngle;
            }
            return polygon;
        }

        /// <summary>
        /// Returns the value of an angle. Assumes clockwise order of the polygon.
        /// </summary>
        /// <param name="previous">Previous vertex</param>
        /// <param name="current">Current vertex</param>
        /// <param name="next">Next vertex</param>
        public static float GetAngle(Vector2 previous, Vector2 current, Vector2 next)
        {
            Vector2 toPrevious = (previous - current).normalized;
            Vector2 toNext = (next - current).normalized;
            return VectorE.Angle360(toNext, toPrevious);
        }

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

                Vector2 bisector = GetAngleBisector(previous, current, next, out float angle);
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

                Vector2 bisector = GetAngleBisector(previous, current, next, out float angle);
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

                Vector2 bisector = GetAngleBisector(previous, current, next, out float angle);
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

        /// <summary>
        /// Calculates a bounding rect for a set of vertices.
        /// </summary>
        public static Rect GetRect(IList<Vector2> vertices)
        {
            Vector2 min = vertices[0];
            Vector2 max = vertices[0];
            for (var i = 1; i < vertices.Count; i++)
            {
                var vertex = vertices[i];
                min = Vector2.Min(min, vertex);
                max = Vector2.Max(max, vertex);
            }
            return Rect.MinMaxRect(min.x, min.y, max.x, max.y);
        }

        /// <summary>
        /// Calculates a circumradius for a rectangle.
        /// </summary>
        public static float GetCircumradius(Rect rect)
        {
            return GetCircumradius(rect.width, rect.height);
        }

        /// <summary>
        /// Calculates a circumradius for a rectangle.
        /// </summary>
        public static float GetCircumradius(float width, float height)
        {
            return Mathf.Sqrt(width/2*width/2 + height/2*height/2);
        }
    }
}
