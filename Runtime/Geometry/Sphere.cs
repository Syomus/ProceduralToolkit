using System;
using UnityEngine;

namespace ProceduralToolkit
{
    /// <summary>
    /// Representation of a sphere
    /// </summary>
    [Serializable]
    public struct Sphere : IEquatable<Sphere>, IFormattable
    {
        public Vector3 center;
        public float radius;

        /// <summary>
        /// Returns the area of the sphere
        /// </summary>
        public float area => 4*Mathf.PI*radius*radius;
        /// <summary>
        /// Returns the volume of the sphere
        /// </summary>
        public float volume => 4f/3f*Mathf.PI*radius*radius*radius;

        public static Sphere unit => new Sphere(Vector3.zero, 1);

        public Sphere(float radius)
        {
            center = Vector3.zero;
            this.radius = radius;
        }

        public Sphere(Vector3 center, float radius)
        {
            this.center = center;
            this.radius = radius;
        }

        /// <summary>
        /// Returns a point on the sphere at the given coordinates
        /// </summary>
        /// <param name="horizontalAngle">Horizontal angle in degrees [0, 360]</param>
        /// <param name="verticalAngle">Vertical angle in degrees [-90, 90]</param>
        public Vector3 GetPoint(float horizontalAngle, float verticalAngle)
        {
            return center + Geometry.PointOnSphere(radius, horizontalAngle, verticalAngle);
        }

        /// <summary>
        /// Returns true if the point intersects the sphere
        /// </summary>
        public bool Contains(Vector3 point)
        {
            return Intersect.PointSphere(point, center, radius);
        }

        /// <summary>
        /// Linearly interpolates between two spheres
        /// </summary>
        public static Sphere Lerp(Sphere a, Sphere b, float t)
        {
            t = Mathf.Clamp01(t);
            return new Sphere(a.center + (b.center - a.center)*t, a.radius + (b.radius - a.radius)*t);
        }

        /// <summary>
        /// Linearly interpolates between two spheres without clamping the interpolant
        /// </summary>
        public static Sphere LerpUnclamped(Sphere a, Sphere b, float t)
        {
            return new Sphere(a.center + (b.center - a.center)*t, a.radius + (b.radius - a.radius)*t);
        }

        public static explicit operator Circle2(Sphere sphere)
        {
            return new Circle2((Vector2) sphere.center, sphere.radius);
        }

        public static Sphere operator +(Sphere sphere, Vector3 vector)
        {
            return new Sphere(sphere.center + vector, sphere.radius);
        }

        public static Sphere operator -(Sphere sphere, Vector3 vector)
        {
            return new Sphere(sphere.center - vector, sphere.radius);
        }

        public static bool operator ==(Sphere a, Sphere b)
        {
            return a.center == b.center && a.radius == b.radius;
        }

        public static bool operator !=(Sphere a, Sphere b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            return center.GetHashCode() ^ (radius.GetHashCode() << 2);
        }

        public override bool Equals(object other)
        {
            return other is Sphere && Equals((Sphere) other);
        }

        public bool Equals(Sphere other)
        {
            return center.Equals(other.center) && radius.Equals(other.radius);
        }

        public override string ToString()
        {
            return string.Format("Sphere(center: {0}, radius: {1})", center, radius);
        }

        public string ToString(string format)
        {
            return string.Format("Sphere(center: {0}, radius: {1})", center.ToString(format), radius.ToString(format));
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            return string.Format("Sphere(center: {0}, radius: {1})", center.ToString(format, formatProvider),
                radius.ToString(format, formatProvider));
        }
    }
}
