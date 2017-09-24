using UnityEngine;

namespace ProceduralToolkit
{
    public static class Geometry
    {
        public const float Epsilon = 0.00001f;

        #region Vector2

        /// <summary>
        /// Returns the distance to the closest point on the line
        /// </summary>
        public static float DistanceToLine(Vector2 point, Ray2D line)
        {
            return Vector2.Distance(point, ClosestPointOnLine(point, line));
        }

        /// <summary>
        /// Returns the distance to the closest point on the line defined by <paramref name="lineA"/> and <paramref name="lineB"/>
        /// </summary>
        public static float DistanceToLine(Vector2 point, Vector2 lineA, Vector2 lineB)
        {
            return Vector2.Distance(point, ClosestPointOnLine(point, lineA, lineB));
        }

        /// <summary>
        /// Projects the point onto the line
        /// </summary>
        public static Vector2 ClosestPointOnLine(Vector2 point, Ray2D line)
        {
            float projectedX;
            return ClosestPointOnLine(point, line.origin, line.origin + line.direction, out projectedX);
        }

        /// <summary>
        /// Projects the point onto the line defined by <paramref name="lineA"/> and <paramref name="lineB"/>
        /// </summary>
        public static Vector2 ClosestPointOnLine(Vector2 point, Vector2 lineA, Vector2 lineB)
        {
            float projectedX;
            return ClosestPointOnLine(point, lineA, lineB, out projectedX);
        }

        /// <summary>
        /// Projects the point onto the line defined by <paramref name="lineA"/> and <paramref name="lineB"/>
        /// </summary>
        /// <param name="projectedX">Normalized position of the projected point on the line segment. 
        /// Value of zero means that projected point coincides with <paramref name="lineA"/>. 
        /// Value of one means that projected point coincides with <paramref name="lineB"/>.</param>
        public static Vector2 ClosestPointOnLine(Vector2 point, Vector2 lineA, Vector2 lineB, out float projectedX)
        {
            Vector2 direction = lineB - lineA;
            Vector2 toPoint = point - lineA;

            float dotDirection = Vector2.Dot(direction, direction);
            if (dotDirection < Epsilon)
            {
                Debug.LogError("Invalid line definition. lineA: " + lineA + " lineB: " + lineB);
                projectedX = 0;
                return lineA;
            }

            projectedX = Vector2.Dot(toPoint, direction)/dotDirection;
            return lineA + direction*projectedX;
        }

        /// <summary>
        /// Returns the distance to the closest point on the line segment defined by <paramref name="segmentA"/> and <paramref name="segmentB"/>
        /// </summary>
        public static float DistanceToSegment(Vector2 point, Vector2 segmentA, Vector2 segmentB)
        {
            return Vector2.Distance(point, ClosestPointOnSegment(point, segmentA, segmentB));
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

        #endregion Vector2

        #region Vector3

        /// <summary>
        /// Returns the distance to the closest point on the line
        /// </summary>
        public static float DistanceToLine(Vector3 point, Ray line)
        {
            return Vector3.Distance(point, ClosestPointOnLine(point, line));
        }

        /// <summary>
        /// Returns the distance to the closest point on the line defined by <paramref name="lineA"/> and <paramref name="lineB"/>
        /// </summary>
        public static float DistanceToLine(Vector3 point, Vector3 lineA, Vector3 lineB)
        {
            return Vector3.Distance(point, ClosestPointOnLine(point, lineA, lineB));
        }

        /// <summary>
        /// Projects the point onto the line
        /// </summary>
        public static Vector3 ClosestPointOnLine(Vector3 point, Ray line)
        {
            float projectedX;
            return ClosestPointOnLine(point, line.origin, line.origin + line.direction, out projectedX);
        }

        /// <summary>
        /// Projects the point onto a line defined by <paramref name="lineA"/> and <paramref name="lineB"/>
        /// </summary>
        public static Vector3 ClosestPointOnLine(Vector3 point, Vector3 lineA, Vector3 lineB)
        {
            float projectedX;
            return ClosestPointOnLine(point, lineA, lineB, out projectedX);
        }

        /// <summary>
        /// Projects the point onto the line defined by <paramref name="lineA"/> and <paramref name="lineB"/>
        /// </summary>
        /// <param name="projectedX">Normalized position of the projected point on the line segment. 
        /// Value of zero means that projected point coincides with <paramref name="lineA"/>. 
        /// Value of one means that projected point coincides with <paramref name="lineB"/>.</param>
        public static Vector3 ClosestPointOnLine(Vector3 point, Vector3 lineA, Vector3 lineB, out float projectedX)
        {
            Vector3 direction = lineB - lineA;
            Vector3 toPoint = point - lineA;

            float dotDirection = Vector3.Dot(direction, direction);
            if (dotDirection < Epsilon)
            {
                Debug.LogError("Invalid line definition. lineA: " + lineA + " lineB: " + lineB);
                projectedX = 0;
                return lineA;
            }

            projectedX = Vector3.Dot(toPoint, direction)/dotDirection;
            return lineA + direction*projectedX;
        }

        /// <summary>
        /// Returns the distance to the closest point on the line segment defined by <paramref name="segmentA"/> and <paramref name="segmentB"/>
        /// </summary>
        public static float DistanceToSegment(Vector3 point, Vector3 segmentA, Vector3 segmentB)
        {
            return Vector3.Distance(point, ClosestPointOnSegment(point, segmentA, segmentB));
        }

        /// <summary>
        /// Projects the point onto the line segment defined by <paramref name="segmentA"/> and <paramref name="segmentB"/>
        /// </summary>
        public static Vector3 ClosestPointOnSegment(Vector3 point, Vector3 segmentA, Vector3 segmentB)
        {
            float projectedX;
            return ClosestPointOnSegment(point, segmentA, segmentB, out projectedX);
        }

        /// <summary>
        /// Projects the point onto the line segment defined by <paramref name="segmentA"/> and <paramref name="segmentB"/>
        /// </summary>
        /// <param name="projectedX">Normalized position of the projected point on the line segment. 
        /// Value of zero means that projected point coincides with <paramref name="segmentA"/>. 
        /// Value of one means that projected point coincides with <paramref name="segmentB"/>.</param>
        public static Vector3 ClosestPointOnSegment(Vector3 point, Vector3 segmentA, Vector3 segmentB, out float projectedX)
        {
            Vector3 direction = segmentB - segmentA;
            Vector3 toPoint = point - segmentA;

            float dotDirection = Vector3.Dot(direction, direction);
            if (dotDirection < Epsilon)
            {
                Debug.LogError("Invalid segment definition. segmentA: " + segmentA + " segmentB: " + segmentB);
                projectedX = 0;
                return segmentA;
            }

            float dotToPoint = Vector3.Dot(toPoint, direction);
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

        #endregion Vector3
    }
}