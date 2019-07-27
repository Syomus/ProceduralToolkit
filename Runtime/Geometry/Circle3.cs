using System;
using UnityEngine;

namespace ProceduralToolkit
{
    /// <summary>
    /// Representation of a 3D circle
    /// </summary>
    [Serializable]
    public struct Circle3 : IEquatable<Circle3>, IFormattable
    {
        public Vector3 center;
        public Vector3 normal;
        public float radius;

        public static Circle3 unitXY { get { return new Circle3(Vector3.zero, Vector3.back, 1); } }
        public static Circle3 unitXZ { get { return new Circle3(Vector3.zero, Vector3.up, 1); } }
        public static Circle3 unitYZ { get { return new Circle3(Vector3.zero, Vector3.left, 1); } }

        public Circle3(float radius) : this(Vector3.zero, Vector3.back, radius)
        {
        }

        public Circle3(Vector3 center, float radius) : this(center, Vector3.back, radius)
        {
        }

        public Circle3(Vector3 center, Vector3 normal, float radius)
        {
            this.center = center;
            this.normal = normal;
            this.radius = radius;
        }

        /// <summary>
        /// Linearly interpolates between two circles
        /// </summary>
        public static Circle3 Lerp(Circle3 a, Circle3 b, float t)
        {
            t = Mathf.Clamp01(t);
            return new Circle3(
                center: a.center + (b.center - a.center)*t,
                normal: Vector3.LerpUnclamped(a.normal, b.normal, t),
                radius: a.radius + (b.radius - a.radius)*t);
        }

        /// <summary>
        /// Linearly interpolates between two circles without clamping the interpolant
        /// </summary>
        public static Circle3 LerpUnclamped(Circle3 a, Circle3 b, float t)
        {
            return new Circle3(
                center: a.center + (b.center - a.center)*t,
                normal: Vector3.LerpUnclamped(a.normal, b.normal, t),
                radius: a.radius + (b.radius - a.radius)*t);
        }

        public static explicit operator Sphere(Circle3 circle)
        {
            return new Sphere(circle.center, circle.radius);
        }

        public static explicit operator Circle2(Circle3 circle)
        {
            return new Circle2((Vector2) circle.center, circle.radius);
        }

        public static Circle3 operator +(Circle3 circle, Vector3 vector)
        {
            return new Circle3(circle.center + vector, circle.normal, circle.radius);
        }

        public static Circle3 operator -(Circle3 circle, Vector3 vector)
        {
            return new Circle3(circle.center - vector, circle.normal, circle.radius);
        }

        public static bool operator ==(Circle3 a, Circle3 b)
        {
            return a.center == b.center && a.normal == b.normal && a.radius == b.radius;
        }

        public static bool operator !=(Circle3 a, Circle3 b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            return center.GetHashCode() ^ (normal.GetHashCode() << 2) ^ (radius.GetHashCode() >> 2);
        }

        public override bool Equals(object other)
        {
            return other is Circle3 && Equals((Circle3) other);
        }

        public bool Equals(Circle3 other)
        {
            return center.Equals(other.center) && normal.Equals(other.normal) && radius.Equals(other.radius);
        }

        public override string ToString()
        {
            return string.Format("Circle3(center: {0}, normal: {1}, radius: {2})", center, normal, radius);
        }

        public string ToString(string format)
        {
            return string.Format("Circle3(center: {0}, normal: {1}, radius: {2})",
                center.ToString(format), normal.ToString(format), radius.ToString(format));
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            return string.Format("Circle3(center: {0}, normal: {1}, radius: {2})",
                center.ToString(format, formatProvider), normal.ToString(format, formatProvider), radius.ToString(format, formatProvider));
        }
    }
}
