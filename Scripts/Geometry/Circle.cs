using System;
using UnityEngine;

namespace ProceduralToolkit
{
    /// <summary>
    /// Representation of a circle
    /// </summary>
    [Serializable]
    public struct Circle : IEquatable<Circle>, IFormattable
    {
        public Vector2 center;
        public float radius;

        public static Circle unit { get { return new Circle(Vector2.zero, 1); } }

        public Circle(float radius)
        {
            center = Vector2.zero;
            this.radius = radius;
        }

        public Circle(Vector2 center, float radius)
        {
            this.center = center;
            this.radius = radius;
        }

        /// <summary>
        /// Returns a point on the circle at the given <paramref name="angle"/>
        /// </summary>
        public Vector2 GetPoint(float angle)
        {
            return center + PTUtils.PointOnCircle2(radius, angle);
        }

        public bool Contains(Vector2 point)
        {
            return Intersect.PointCircle(point, center, radius);
        }

        /// <summary>
        /// Linearly interpolates between two circles
        /// </summary>
        public static Circle Lerp(Circle a, Circle b, float t)
        {
            t = Mathf.Clamp01(t);
            return new Circle(a.center + (b.center - a.center)*t, a.radius + (b.radius - a.radius)*t);
        }

        /// <summary>
        /// Linearly interpolates between two circles without clamping the interpolant
        /// </summary>
        public static Circle LerpUnclamped(Circle a, Circle b, float t)
        {
            return new Circle(a.center + (b.center - a.center)*t, a.radius + (b.radius - a.radius)*t);
        }

        public static explicit operator Sphere(Circle circle)
        {
            return new Sphere((Vector3) circle.center, circle.radius);
        }

        public static Circle operator +(Circle circle, Vector2 vector)
        {
            return new Circle(circle.center + vector, circle.radius);
        }

        public static Circle operator -(Circle circle, Vector2 vector)
        {
            return new Circle(circle.center - vector, circle.radius);
        }

        public static bool operator ==(Circle a, Circle b)
        {
            return a.center == b.center && a.radius == b.radius;
        }

        public static bool operator !=(Circle a, Circle b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            return center.GetHashCode() ^ (radius.GetHashCode() << 2);
        }

        public override bool Equals(object other)
        {
            return other is Circle && Equals((Circle) other);
        }

        public bool Equals(Circle other)
        {
            return center.Equals(other.center) && radius.Equals(other.radius);
        }

        public override string ToString()
        {
            return string.Format("Circle(center: {0}, radius: {1})", center, radius);
        }

        public string ToString(string format)
        {
            return string.Format("Circle(center: {0}, radius: {1})", center.ToString(format), radius.ToString(format));
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            return string.Format("Circle(center: {0}, radius: {1})", center.ToString(format, formatProvider),
                radius.ToString(format, formatProvider));
        }
    }
}
