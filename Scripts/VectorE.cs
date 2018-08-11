using System;
using UnityEngine;

namespace ProceduralToolkit
{
    /// <summary>
    /// Vector extensions
    /// </summary>
    public static class VectorE
    {
        #region Vector2

        /// <summary>
        /// Returns a new vector with zero Y component
        /// </summary>
        public static Vector2 ToVector2X(this Vector2 vector)
        {
            return new Vector2(vector.x, 0);
        }

        /// <summary>
        /// Returns a new vector with zero X component
        /// </summary>
        public static Vector2 ToVector2Y(this Vector2 vector)
        {
            return new Vector2(0, vector.y);
        }

        /// <summary>
        /// Projects the vector onto the three dimensional XY plane
        /// </summary>
        public static Vector3 ToVector3XY(this Vector2 vector)
        {
            return new Vector3(vector.x, vector.y, 0);
        }

        /// <summary>
        /// Projects the vector onto the three dimensional XZ plane
        /// </summary>
        public static Vector3 ToVector3XZ(this Vector2 vector)
        {
            return new Vector3(vector.x, 0, vector.y);
        }

        /// <summary>
        /// Projects the vector onto the three dimensional YZ plane
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
            return Mathf.Abs(PerpDot(vector, other)) < Geometry.Epsilon;
        }

        /// <summary>
        /// Returns a new vector rotated counterclockwise by 90°
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
        /// Returns a perp dot product of vectors
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
        /// Returns a signed clockwise angle in degrees [-180, 180] between from and to
        /// </summary>
        /// <param name="from">The angle extends round from this vector</param>
        /// <param name="to">The angle extends round to this vector</param>
        public static float SignedAngle(Vector2 from, Vector2 to)
        {
            return Mathf.Atan2(PerpDot(to, from), Vector2.Dot(to, from))*Mathf.Rad2Deg;
        }

        /// <summary>
        /// Returns a clockwise angle in degrees [0, 360] between from and to
        /// </summary>
        /// <param name="from">The angle extends round from this vector</param>
        /// <param name="to">The angle extends round to this vector</param>
        public static float Angle360(Vector2 from, Vector2 to)
        {
            float angle = SignedAngle(from, to);
            while (angle < 0)
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
        /// Returns a new vector rotated clockwise by the specified angle
        /// </summary>
        public static Vector2 RotateCW(this Vector2 vector, float degrees)
        {
            float radians = degrees*Mathf.Deg2Rad;
            float sin = Mathf.Sin(radians);
            float cos = Mathf.Cos(radians);
            return new Vector2(
                vector.x*cos + vector.y*sin,
                vector.y*cos - vector.x*sin);
        }

        /// <summary>
        /// Returns a new vector rotated counterclockwise by the specified angle
        /// </summary>
        public static Vector2 RotateCCW(this Vector2 vector, float degrees)
        {
            float radians = degrees*Mathf.Deg2Rad;
            float sin = Mathf.Sin(radians);
            float cos = Mathf.Cos(radians);
            return new Vector2(
                vector.x*cos - vector.y*sin,
                vector.y*cos + vector.x*sin);
        }

        /// <summary>
        /// Returns a new vector rotated clockwise by 45°
        /// </summary>
        public static Vector2 RotateCW45(this Vector2 vector)
        {
            return new Vector2((vector.x + vector.y)*PTUtils.Sqrt05, (vector.y - vector.x)*PTUtils.Sqrt05);
        }

        /// <summary>
        /// Returns a new vector rotated counterclockwise by 45°
        /// </summary>
        public static Vector2 RotateCCW45(this Vector2 vector)
        {
            return new Vector2((vector.x - vector.y)*PTUtils.Sqrt05, (vector.y + vector.x)*PTUtils.Sqrt05);
        }

        /// <summary>
        /// Returns a new vector rotated clockwise by 90°
        /// </summary>
        public static Vector2 RotateCW90(this Vector2 vector)
        {
            return new Vector2(vector.y, -vector.x);
        }

        /// <summary>
        /// Returns a new vector rotated counterclockwise by 90°
        /// </summary>
        public static Vector2 RotateCCW90(this Vector2 vector)
        {
            return new Vector2(-vector.y, vector.x);
        }

        public static string ToString(this Vector2 vector, string format, IFormatProvider formatProvider)
        {
            return string.Format("({0}, {1})", vector.x.ToString(format, formatProvider), vector.y.ToString(format, formatProvider));
        }

        #endregion Vector2

        #region Vector2Int

        /// <summary>
        /// Returns a perp of vector
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
        /// Returns a perp dot product of vectors
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
        /// Returns a new vector with zero Y and Z components
        /// </summary>
        public static Vector3 ToVector3X(this Vector3 vector)
        {
            return new Vector3(vector.x, 0, 0);
        }

        /// <summary>
        /// Returns a new vector with zero X and Z components
        /// </summary>
        public static Vector3 ToVector3Y(this Vector3 vector)
        {
            return new Vector3(0, vector.y, 0);
        }

        /// <summary>
        /// Returns a new vector with zero X and Y components
        /// </summary>
        public static Vector3 ToVector3Z(this Vector3 vector)
        {
            return new Vector3(0, 0, vector.z);
        }

        /// <summary>
        /// Returns a new vector with zero Z component
        /// </summary>
        public static Vector3 ToVector3XY(this Vector3 vector)
        {
            return new Vector3(vector.x, vector.y, 0);
        }

        /// <summary>
        /// Returns a new vector with zero Y component
        /// </summary>
        public static Vector3 ToVector3XZ(this Vector3 vector)
        {
            return new Vector3(vector.x, 0, vector.z);
        }

        /// <summary>
        /// Returns a new vector with zero X component
        /// </summary>
        public static Vector3 ToVector3YZ(this Vector3 vector)
        {
            return new Vector3(0, vector.y, vector.z);
        }

        /// <summary>
        /// Returns a new Vector2 made from X and Y components of this vector
        /// </summary>
        public static Vector2 ToVector2XY(this Vector3 vector)
        {
            return new Vector2(vector.x, vector.y);
        }

        /// <summary>
        /// Returns a new Vector2 made from X and Z components of this vector
        /// </summary>
        public static Vector2 ToVector2XZ(this Vector3 vector)
        {
            return new Vector2(vector.x, vector.z);
        }

        /// <summary>
        /// Returns a new Vector2 made from Y and Z components of this vector
        /// </summary>
        public static Vector2 ToVector2YZ(this Vector3 vector)
        {
            return new Vector2(vector.y, vector.z);
        }

        /// <summary>
        /// Returns an angle in degrees [0, 360] between from and to
        /// </summary>
        /// <param name="from">The angle extends round from this vector</param>
        /// <param name="to">The angle extends round to this vector</param>
        /// <param name="normal">Up direction of the clockwise axis</param>
        public static float Angle360(Vector3 from, Vector3 to, Vector3 normal)
        {
            float angle = Vector3.SignedAngle(from, to, normal);
            while (angle < 0)
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
