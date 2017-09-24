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
        public static Vector2 ToVector2X(this Vector2 vector)
        {
            return new Vector2(vector.x, 0);
        }

        /// <summary>
        /// Returns new vector with zero X component
        /// </summary>
        public static Vector2 ToVector2Y(this Vector2 vector)
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
            return Mathf.Abs(PerpDot(vector, other)) < epsilon;
        }

        /// <summary>
        /// Returns perp of vector
        /// </summary>
        /// <remarks>
        /// Hill, F. S. Jr. "The Pleasures of 'Perp Dot' Products."
        /// Ch. II.5 in Graphics Gems IV (Ed. P. S. Heckbert). San Diego: Academic Press, pp. 138-148, 1994
        /// </remarks>
        public static Vector2 Perp(this Vector2 vector)
        {
            return new Vector2(-vector.y, vector.x);
        }

        /// <summary>
        /// Returns perp dot product of vectors
        /// </summary>
        /// <remarks>
        /// Hill, F. S. Jr. "The Pleasures of 'Perp Dot' Products."
        /// Ch. II.5 in Graphics Gems IV (Ed. P. S. Heckbert). San Diego: Academic Press, pp. 138-148, 1994
        /// </remarks>
        public static float PerpDot(Vector2 a, Vector2 b)
        {
            return a.x*b.y - a.y*b.x;
        }

        /// <summary>
        /// Returns the signed angle in degrees [-180, 180] between from and to
        /// </summary>
        /// <param name="from">The angle extends round from this vector</param>
        /// <param name="to">The angle extends round to this vector</param>
        public static float SignedAngle(Vector2 from, Vector2 to)
        {
            return Mathf.Atan2(PerpDot(to, from), Vector2.Dot(to, from))*Mathf.Rad2Deg;
        }

        /// <summary>
        /// Returns the angle in degrees [0, 360] between from and to
        /// </summary>
        /// <param name="from">The angle extends round from this vector</param>
        /// <param name="to">The angle extends round to this vector</param>
        public static float Angle360(Vector2 from, Vector2 to)
        {
            float angle = SignedAngle(from, to);
            if (angle < 0)
            {
                angle += 360;
            }
            return angle;
        }

        /// <summary>
        /// Calculates the linear parameter t that produces the interpolant value within the range [a, b].
        /// </summary>
        public static Vector2 InverseLerp(Vector2 a, Vector2 b, Vector2 value)
        {
            return new Vector2(
                Mathf.InverseLerp(a.x, b.x, value.x),
                Mathf.InverseLerp(a.y, b.y, value.y));
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

        #region Vector2Int

        /// <summary>
        /// Returns perp of vector
        /// </summary>
        /// <remarks>
        /// Hill, F. S. Jr. "The Pleasures of 'Perp Dot' Products."
        /// Ch. II.5 in Graphics Gems IV (Ed. P. S. Heckbert). San Diego: Academic Press, pp. 138-148, 1994
        /// </remarks>
        public static Vector2Int Perp(this Vector2Int vector)
        {
            return new Vector2Int(-vector.y, vector.x);
        }

        /// <summary>
        /// Returns perp dot product of vectors
        /// </summary>
        /// <remarks>
        /// Hill, F. S. Jr. "The Pleasures of 'Perp Dot' Products."
        /// Ch. II.5 in Graphics Gems IV (Ed. P. S. Heckbert). San Diego: Academic Press, pp. 138-148, 1994
        /// </remarks>
        public static int PerpDot(Vector2Int a, Vector2Int b)
        {
            return a.x*b.y - a.y*b.x;
        }

        #endregion Vector2Int

        #region Vector3

        /// <summary>
        /// Returns new vector with zero Y and Z components
        /// </summary>
        public static Vector3 ToVector3X(this Vector3 vector)
        {
            return new Vector3(vector.x, 0, 0);
        }

        /// <summary>
        /// Returns new vector with zero X and Z components
        /// </summary>
        public static Vector3 ToVector3Y(this Vector3 vector)
        {
            return new Vector3(0, vector.y, 0);
        }

        /// <summary>
        /// Returns new vector with zero X and Y components
        /// </summary>
        public static Vector3 ToVector3Z(this Vector3 vector)
        {
            return new Vector3(0, 0, vector.z);
        }

        /// <summary>
        /// Returns new vector with zero Z component
        /// </summary>
        public static Vector3 ToVector3XY(this Vector3 vector)
        {
            return new Vector3(vector.x, vector.y, 0);
        }

        /// <summary>
        /// Returns new vector with zero Y component
        /// </summary>
        public static Vector3 ToVector3XZ(this Vector3 vector)
        {
            return new Vector3(vector.x, 0, vector.z);
        }

        /// <summary>
        /// Returns new vector with zero X component
        /// </summary>
        public static Vector3 ToVector3YZ(this Vector3 vector)
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
        /// Returns the signed angle in degrees [-180, 180] between from and to
        /// </summary>
        /// <param name="from">The angle extends round from this vector</param>
        /// <param name="to">The angle extends round to this vector</param>
        /// <param name="normal">Up direction of the clockwise axis</param>
        public static float SignedAngle(Vector3 from, Vector3 to, Vector3 normal)
        {
            return Mathf.Atan2(
                       Vector3.Dot(normal, Vector3.Cross(from, to)),
                       Vector3.Dot(from, to))*Mathf.Rad2Deg;
        }

        /// <summary>
        /// Returns the angle in degrees [0, 360] between from and to
        /// </summary>
        /// <param name="from">The angle extends round from this vector</param>
        /// <param name="to">The angle extends round to this vector</param>
        /// <param name="normal">Up direction of the clockwise axis</param>
        public static float Angle360(Vector3 from, Vector3 to, Vector3 normal)
        {
            float angle = SignedAngle(from, to, normal);
            if (angle < 0)
            {
                angle += 360;
            }
            return angle;
        }

        /// <summary>
        /// Calculates the linear parameter t that produces the interpolant value within the range [a, b].
        /// </summary>
        public static Vector3 InverseLerp(Vector3 a, Vector3 b, Vector3 value)
        {
            return new Vector3(
                Mathf.InverseLerp(a.x, b.x, value.x),
                Mathf.InverseLerp(a.y, b.y, value.y),
                Mathf.InverseLerp(a.z, b.z, value.z));
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

        #region Vector4

        /// <summary>
        /// Calculates the linear parameter t that produces the interpolant value within the range [a, b].
        /// </summary>
        public static Vector4 InverseLerp(Vector4 a, Vector4 b, Vector4 value)
        {
            return new Vector4(
                Mathf.InverseLerp(a.x, b.x, value.x),
                Mathf.InverseLerp(a.y, b.y, value.y),
                Mathf.InverseLerp(a.z, b.z, value.z),
                Mathf.InverseLerp(a.w, b.w, value.w));
        }

        #endregion Vector4
    }
}