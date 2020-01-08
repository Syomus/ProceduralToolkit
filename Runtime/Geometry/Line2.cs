using System;
using UnityEngine;

namespace ProceduralToolkit
{
    /// <summary>
    /// Representation of a 2D line
    /// </summary>
    [Serializable]
    public struct Line2 : IEquatable<Line2>, IFormattable
    {
        public Vector2 origin;
        public Vector2 direction;

        public static Line2 xAxis => new Line2(Vector2.zero, Vector2.right);
        public static Line2 yAxis => new Line2(Vector2.zero, Vector2.up);

        public Line2(Ray2D ray)
        {
            origin = ray.origin;
            direction = ray.direction;
        }

        public Line2(Vector2 origin, Vector2 direction)
        {
            this.origin = origin;
            this.direction = direction;
        }

        /// <summary>
        /// Returns a point at <paramref name="distance"/> units from origin along the line
        /// </summary>
        public Vector2 GetPoint(float distance)
        {
            return origin + direction*distance;
        }

        /// <summary>
        /// Linearly interpolates between two lines
        /// </summary>
        public static Line2 Lerp(Line2 a, Line2 b, float t)
        {
            t = Mathf.Clamp01(t);
            return new Line2(a.origin + (b.origin - a.origin)*t, a.direction + (b.direction - a.direction)*t);
        }

        /// <summary>
        /// Linearly interpolates between two lines without clamping the interpolant
        /// </summary>
        public static Line2 LerpUnclamped(Line2 a, Line2 b, float t)
        {
            return new Line2(a.origin + (b.origin - a.origin)*t, a.direction + (b.direction - a.direction)*t);
        }

        #region Casting operators

        public static explicit operator Line2(Ray2D ray)
        {
            return new Line2(ray);
        }

        public static explicit operator Ray2D(Line2 line)
        {
            return new Ray2D(line.origin, line.direction);
        }

        public static explicit operator Ray(Line2 line)
        {
            return new Ray((Vector3) line.origin, (Vector3) line.direction);
        }

        public static explicit operator Line3(Line2 line)
        {
            return new Line3((Vector3) line.origin, (Vector3) line.direction);
        }

        #endregion Casting operators

        public static Line2 operator +(Line2 line, Vector2 vector)
        {
            return new Line2(line.origin + vector, line.direction);
        }

        public static Line2 operator -(Line2 line, Vector2 vector)
        {
            return new Line2(line.origin - vector, line.direction);
        }

        public static bool operator ==(Line2 a, Line2 b)
        {
            return a.origin == b.origin && a.direction == b.direction;
        }

        public static bool operator !=(Line2 a, Line2 b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            return origin.GetHashCode() ^ (direction.GetHashCode() << 2);
        }

        public override bool Equals(object other)
        {
            return other is Line2 && Equals((Line2) other);
        }

        public bool Equals(Line2 other)
        {
            return origin.Equals(other.origin) && direction.Equals(other.direction);
        }

        public override string ToString()
        {
            return string.Format("Line2(origin: {0}, direction: {1})", origin, direction);
        }

        public string ToString(string format)
        {
            return string.Format("Line2(origin: {0}, direction: {1})", origin.ToString(format), direction.ToString(format));
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            return string.Format("Line2(origin: {0}, direction: {1})", origin.ToString(format, formatProvider),
                direction.ToString(format, formatProvider));
        }
    }
}
