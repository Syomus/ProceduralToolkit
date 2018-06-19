using UnityEngine;
using System.Collections.Generic;

namespace ProceduralToolkit
{
    /// <summary>
    /// Utility class for computational geometry algorithms
    /// </summary>
    public static class Geometry
    {
        public const float Epsilon = 0.00001f;

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
    }
}
