using System;
using UnityEngine;

namespace ProceduralToolkit
{
    /// <summary>
    /// Representation of a 2D circle
    /// </summary>
    [Serializable]
    public struct Circle2 : IEquatable<Circle2>, IFormattable
    {
        public Vector2 center;
        public float radius;

        public static Circle2 unit { get { return new Circle2(Vector2.zero, 1); } }

        public Circle2(float radius) : this(Vector2.zero, radius)
        {
        }

        public Circle2(Vector2 center, float radius)
        {
            this.center = center;
            this.radius = radius;
        }

        /// <summary>
        /// Returns a point on the circle at the given <paramref name="angle"/>
        /// </summary>
        public Vector2 GetPoint(float angle)
        {
            return center + Geometry.PointOnCircle2(radius, angle);
        }

        public bool Contains(Vector2 point)
        {
            return Intersect.PointCircle(point, center, radius);
        }

        /// <summary>
        /// Linearly interpolates between two circles
        /// </summary>
        public static Circle2 Lerp(Circle2 a, Circle2 b, float t)
        {
            t = Mathf.Clamp01(t);
            return new Circle2(a.center + (b.center - a.center)*t, a.radius + (b.radius - a.radius)*t);
        }

        /// <summary>
        /// Linearly interpolates between two circles without clamping the interpolant
        /// </summary>
        public static Circle2 LerpUnclamped(Circle2 a, Circle2 b, float t)
        {
            return new Circle2(a.center + (b.center - a.center)*t, a.radius + (b.radius - a.radius)*t);
        }

        public static explicit operator Sphere(Circle2 circle)
        {
            return new Sphere((Vector3) circle.center, circle.radius);
        }

        public static explicit operator Circle3(Circle2 circle)
        {
            return new Circle3((Vector3) circle.center, Vector3.back, circle.radius);
        }

        public static Circle2 operator +(Circle2 circle, Vector2 vector)
        {
            return new Circle2(circle.center + vector, circle.radius);
        }

        public static Circle2 operator -(Circle2 circle, Vector2 vector)
        {
            return new Circle2(circle.center - vector, circle.radius);
        }

        public static bool operator ==(Circle2 a, Circle2 b)
        {
            return a.center == b.center && a.radius == b.radius;
        }

        public static bool operator !=(Circle2 a, Circle2 b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            return center.GetHashCode() ^ (radius.GetHashCode() << 2);
        }

        public override bool Equals(object other)
        {
            return other is Circle2 && Equals((Circle2) other);
        }

        public bool Equals(Circle2 other)
        {
            return center.Equals(other.center) && radius.Equals(other.radius);
        }

        public override string ToString()
        {
            return string.Format("Circle2(center: {0}, radius: {1})", center, radius);
        }

        public string ToString(string format)
        {
            return string.Format("Circle2(center: {0}, radius: {1})", center.ToString(format), radius.ToString(format));
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            return string.Format("Circle2(center: {0}, radius: {1})", center.ToString(format, formatProvider),
                radius.ToString(format, formatProvider));
        }
    }
}
