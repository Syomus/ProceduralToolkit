using System;
using UnityEngine;

namespace ProceduralToolkit
{
    /// <summary>
    /// Representation of a 3D line
    /// </summary>
    [Serializable]
    public struct Line3 : IEquatable<Line3>, IFormattable
    {
        public Vector3 origin;
        public Vector3 direction;

        public static Line3 xAxis => new Line3(Vector3.zero, Vector3.right);
        public static Line3 yAxis => new Line3(Vector3.zero, Vector3.up);
        public static Line3 zAxis => new Line3(Vector3.zero, Vector3.forward);

        public Line3(Ray ray)
        {
            origin = ray.origin;
            direction = ray.direction;
        }

        public Line3(Vector3 origin, Vector3 direction)
        {
            this.origin = origin;
            this.direction = direction;
        }

        /// <summary>
        /// Returns a point at <paramref name="distance"/> units from origin along the line
        /// </summary>
        public Vector3 GetPoint(float distance)
        {
            return origin + direction*distance;
        }

        /// <summary>
        /// Linearly interpolates between two lines
        /// </summary>
        public static Line3 Lerp(Line3 a, Line3 b, float t)
        {
            t = Mathf.Clamp01(t);
            return new Line3(a.origin + (b.origin - a.origin)*t, a.direction + (b.direction - a.direction)*t);
        }

        /// <summary>
        /// Linearly interpolates between two lines without clamping the interpolant
        /// </summary>
        public static Line3 LerpUnclamped(Line3 a, Line3 b, float t)
        {
            return new Line3(a.origin + (b.origin - a.origin)*t, a.direction + (b.direction - a.direction)*t);
        }

        #region Casting operators

        public static explicit operator Line3(Ray ray)
        {
            return new Line3(ray);
        }

        public static explicit operator Ray(Line3 line)
        {
            return new Ray(line.origin, line.direction);
        }

        public static explicit operator Ray2D(Line3 line)
        {
            return new Ray2D((Vector2) line.origin, (Vector2) line.direction);
        }

        public static explicit operator Line2(Line3 line)
        {
            return new Line2((Vector2) line.origin, (Vector2) line.direction);
        }

        #endregion Casting operators

        public static Line3 operator +(Line3 line, Vector3 vector)
        {
            return new Line3(line.origin + vector, line.direction);
        }

        public static Line3 operator -(Line3 line, Vector3 vector)
        {
            return new Line3(line.origin - vector, line.direction);
        }

        public static bool operator ==(Line3 a, Line3 b)
        {
            return a.origin == b.origin && a.direction == b.direction;
        }

        public static bool operator !=(Line3 a, Line3 b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            return origin.GetHashCode() ^ (direction.GetHashCode() << 2);
        }

        public override bool Equals(object other)
        {
            return other is Line3 && Equals((Line3) other);
        }

        public bool Equals(Line3 other)
        {
            return origin.Equals(other.origin) && direction.Equals(other.direction);
        }

        public override string ToString()
        {
            return string.Format("Line3(origin: {0}, direction: {1})", origin, direction);
        }

        public string ToString(string format)
        {
            return string.Format("Line3(origin: {0}, direction: {1})", origin.ToString(format), direction.ToString(format));
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            return string.Format("Line3(origin: {0}, direction: {1})", origin.ToString(format, formatProvider),
                direction.ToString(format, formatProvider));
        }
    }
}
