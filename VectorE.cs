using UnityEngine;

namespace ProceduralToolkit
{
    /// <summary>
    /// Vector extensions
    /// </summary>
    public static class VectorE
    {
        private const float epsilon = 0.00001f;

        #region Vector2

        /// <summary>
        /// Returns new vector with zero Y component
        /// </summary>
        public static Vector2 OnlyX(this Vector2 vector)
        {
            return new Vector2(vector.x, 0);
        }

        /// <summary>
        /// Returns new vector with zero X component
        /// </summary>
        public static Vector2 OnlyY(this Vector2 vector)
        {
            return new Vector2(0, vector.y);
        }

        /// <summary>
        /// Projects vector onto three dimensional XY plane
        /// </summary>
        public static Vector3 ToVector3XY(this Vector2 vector)
        {
            return new Vector3(vector.x, vector.y, 0);
        }

        /// <summary>
        /// Projects vector onto three dimensional XZ plane
        /// </summary>
        public static Vector3 ToVector3XZ(this Vector2 vector)
        {
            return new Vector3(vector.x, 0, vector.y);
        }

        /// <summary>
        /// Projects vector onto three dimensional YZ plane
        /// </summary>
        public static Vector3 ToVector3YZ(this Vector2 vector)
        {
            return new Vector3(0, vector.x, vector.y);
        }

        /// <summary>
        /// Returns true if vectors lie on the same line, false otherwise
        /// </summary>
        public static bool IsCollinear(this Vector2 vector, Vector2 other)
        {
            return Mathf.Abs(PTUtils.PerpDot(vector, other)) < epsilon;
        }

        /// <summary>
        /// Returns new vector rotated clockwise by specified angle
        /// </summary>
        public static Vector2 RotateCW(this Vector2 vector, float degrees)
        {
            float radians = degrees*Mathf.Deg2Rad;
            float sin = Mathf.Sin(radians);
            float cos = Mathf.Cos(radians);
            return new Vector2(
                vector.x*cos + vector.y*sin,
                -vector.x*sin + vector.y*cos);
        }

        /// <summary>
        /// Returns the distance to the closest point on a line defined by <paramref name="ray"/>
        /// </summary>
        public static float DistanceToLine(this Vector2 point, Ray ray)
        {
            return Vector2.Distance(point, ProjectOnLine(point, ray));
        }

        /// <summary>
        /// Returns the distance to the closest point on a line defined by <paramref name="lineA"/> and <paramref name="lineB"/>
        /// </summary>
        public static float DistanceToLine(this Vector2 point, Vector2 lineA, Vector2 lineB)
        {
            return Vector2.Distance(point, ProjectOnLine(point, lineA, lineB));
        }

        /// <summary>
        /// Projects a <paramref name="point"/> onto a line defined by <paramref name="ray"/>
        /// </summary>
        public static Vector2 ProjectOnLine(this Vector2 point, Ray2D ray)
        {
            float projectedX;
            return ProjectOnLine(point, ray.origin, ray.origin + ray.direction, out projectedX);
        }

        /// <summary>
        /// Projects a <paramref name="point"/> onto a line defined by <paramref name="lineA"/> and <paramref name="lineB"/>
        /// </summary>
        public static Vector2 ProjectOnLine(this Vector2 point, Vector2 lineA, Vector2 lineB)
        {
            float projectedX;
            return ProjectOnLine(point, lineA, lineB, out projectedX);
        }

        /// <summary>
        /// Projects a <paramref name="point"/> onto a line defined by <paramref name="lineA"/> and <paramref name="lineB"/>
        /// </summary>
        /// <param name="projectedX">Normalized position of projected point on a line segment. 
        /// Value of zero means that projected point coincides with <paramref name="lineA"/>. 
        /// Value of one means that projected point coincides with <paramref name="lineB"/>.</param>
        public static Vector2 ProjectOnLine(this Vector2 point, Vector2 lineA, Vector2 lineB, out float projectedX)
        {
            Vector2 direction = lineB - lineA;
            Vector2 toPoint = point - lineA;

            float dotDirection = Vector2.Dot(direction, direction);
            if ((double) dotDirection < (double) Mathf.Epsilon)
            {
                Debug.LogError("Invalid line definition. lineA: " + lineA + " lineB: " + lineB);
                projectedX = 0;
                return lineA;
            }

            projectedX = Vector2.Dot(toPoint, direction)/dotDirection;
            return lineA + direction*projectedX;
        }

        /// <summary>
        /// Returns the distance to the closest point on a line segment defined by <paramref name="segmentA"/> and <paramref name="segmentB"/>
        /// </summary>
        public static float DistanceToSegment(this Vector2 point, Vector2 segmentA, Vector2 segmentB)
        {
            return Vector2.Distance(point, ProjectOnSegment(point, segmentA, segmentB));
        }

        /// <summary>
        /// Projects a <paramref name="point"/> onto a line segment defined by <paramref name="segmentA"/> and <paramref name="segmentB"/>
        /// </summary>
        public static Vector2 ProjectOnSegment(this Vector2 point, Vector2 segmentA, Vector2 segmentB)
        {
            float projectedX;
            return ProjectOnSegment(point, segmentA, segmentB, out projectedX);
        }

        /// <summary>
        /// Projects a <paramref name="point"/> onto a line segment defined by <paramref name="segmentA"/> and <paramref name="segmentB"/>
        /// </summary>
        /// <param name="projectedX">Normalized position of projected point on a line segment. 
        /// Value of zero means that projected point coincides with <paramref name="segmentA"/>. 
        /// Value of one means that projected point coincides with <paramref name="segmentB"/>.</param>
        public static Vector2 ProjectOnSegment(this Vector2 point, Vector2 segmentA, Vector2 segmentB,
            out float projectedX)
        {
            Vector2 direction = segmentB - segmentA;
            Vector2 toPoint = point - segmentA;

            float dotDirection = Vector2.Dot(direction, direction);
            if ((double) dotDirection < (double) Mathf.Epsilon)
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

            if (dotDirection <= dotToPoint)
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
        /// Returns new vector with zero Y and Z components
        /// </summary>
        public static Vector3 OnlyX(this Vector3 vector)
        {
            return new Vector3(vector.x, 0, 0);
        }

        /// <summary>
        /// Returns new vector with zero X and Z components
        /// </summary>
        public static Vector3 OnlyY(this Vector3 vector)
        {
            return new Vector3(0, vector.y, 0);
        }

        /// <summary>
        /// Returns new vector with zero X and Y components
        /// </summary>
        public static Vector3 OnlyZ(this Vector3 vector)
        {
            return new Vector3(0, 0, vector.z);
        }

        /// <summary>
        /// Returns new vector with zero Z component
        /// </summary>
        public static Vector3 OnlyXY(this Vector3 vector)
        {
            return new Vector3(vector.x, vector.y, 0);
        }

        /// <summary>
        /// Returns new vector with zero Y component
        /// </summary>
        public static Vector3 OnlyXZ(this Vector3 vector)
        {
            return new Vector3(vector.x, 0, vector.z);
        }

        /// <summary>
        /// Returns new vector with zero X component
        /// </summary>
        public static Vector3 OnlyYZ(this Vector3 vector)
        {
            return new Vector3(0, vector.y, vector.z);
        }

        /// <summary>
        /// Returns new Vector2 made from X and Y components of this vector
        /// </summary>
        public static Vector2 ToVector2XY(this Vector3 vector)
        {
            return new Vector2(vector.x, vector.y);
        }

        /// <summary>
        /// Returns new Vector2 made from X and Z components of this vector
        /// </summary>
        public static Vector2 ToVector2XZ(this Vector3 vector)
        {
            return new Vector2(vector.x, vector.z);
        }

        /// <summary>
        /// Returns new Vector2 made from Y and Z components of this vector
        /// </summary>
        public static Vector2 ToVector2YZ(this Vector3 vector)
        {
            return new Vector2(vector.y, vector.z);
        }

        /// <summary>
        /// Returns the distance to the closest point on a line defined by <paramref name="ray"/>
        /// </summary>
        public static float DistanceToLine(this Vector3 point, Ray ray)
        {
            return Vector3.Distance(point, ProjectOnLine(point, ray));
        }

        /// <summary>
        /// Returns the distance to the closest point on a line defined by <paramref name="lineA"/> and <paramref name="lineB"/>
        /// </summary>
        public static float DistanceToLine(this Vector3 point, Vector3 lineA, Vector3 lineB)
        {
            return Vector3.Distance(point, ProjectOnLine(point, lineA, lineB));
        }

        /// <summary>
        /// Projects a <paramref name="point"/> onto a line defined by <paramref name="ray"/>
        /// </summary>
        public static Vector3 ProjectOnLine(this Vector3 point, Ray ray)
        {
            float projectedX;
            return ProjectOnLine(point, ray.origin, ray.origin + ray.direction, out projectedX);
        }

        /// <summary>
        /// Projects a <paramref name="point"/> onto a line defined by <paramref name="lineA"/> and <paramref name="lineB"/>
        /// </summary>
        public static Vector3 ProjectOnLine(this Vector3 point, Vector3 lineA, Vector3 lineB)
        {
            float projectedX;
            return ProjectOnLine(point, lineA, lineB, out projectedX);
        }

        /// <summary>
        /// Projects a <paramref name="point"/> onto a line defined by <paramref name="lineA"/> and <paramref name="lineB"/>
        /// </summary>
        /// <param name="projectedX">Normalized position of projected point on a line segment. 
        /// Value of zero means that projected point coincides with <paramref name="lineA"/>. 
        /// Value of one means that projected point coincides with <paramref name="lineB"/>.</param>
        public static Vector3 ProjectOnLine(this Vector3 point, Vector3 lineA, Vector3 lineB, out float projectedX)
        {
            Vector3 direction = lineB - lineA;
            Vector3 toPoint = point - lineA;

            float dotDirection = Vector3.Dot(direction, direction);
            if ((double) dotDirection < (double) Mathf.Epsilon)
            {
                Debug.LogError("Invalid line definition. lineA: " + lineA + " lineB: " + lineB);
                projectedX = 0;
                return lineA;
            }

            projectedX = Vector3.Dot(toPoint, direction)/dotDirection;
            return lineA + direction*projectedX;
        }

        /// <summary>
        /// Returns the distance to the closest point on a line segment defined by <paramref name="segmentA"/> and <paramref name="segmentB"/>
        /// </summary>
        public static float DistanceToSegment(this Vector3 point, Vector3 segmentA, Vector3 segmentB)
        {
            return Vector3.Distance(point, ProjectOnSegment(point, segmentA, segmentB));
        }

        /// <summary>
        /// Projects a <paramref name="point"/> onto a line segment defined by <paramref name="segmentA"/> and <paramref name="segmentB"/>
        /// </summary>
        public static Vector3 ProjectOnSegment(this Vector3 point, Vector3 segmentA, Vector3 segmentB)
        {
            float projectedX;
            return ProjectOnSegment(point, segmentA, segmentB, out projectedX);
        }

        /// <summary>
        /// Projects a <paramref name="point"/> onto a line segment defined by <paramref name="segmentA"/> and <paramref name="segmentB"/>
        /// </summary>
        /// <param name="projectedX">Normalized position of projected point on a line segment. 
        /// Value of zero means that projected point coincides with <paramref name="segmentA"/>. 
        /// Value of one means that projected point coincides with <paramref name="segmentB"/>.</param>
        public static Vector3 ProjectOnSegment(this Vector3 point, Vector3 segmentA, Vector3 segmentB,
            out float projectedX)
        {
            Vector3 direction = segmentB - segmentA;
            Vector3 toPoint = point - segmentA;

            float dotDirection = Vector3.Dot(direction, direction);
            if ((double) dotDirection < (double) Mathf.Epsilon)
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

            if (dotDirection <= dotToPoint)
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