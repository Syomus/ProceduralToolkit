using System;
using UnityEngine;

namespace ProceduralToolkit
{
    /// <summary>
    /// Representation of a sphere
    /// </summary>
    [Serializable]
    public struct Sphere : IEquatable<Sphere>
    {
        public Vector3 center;
        public float radius;

        public static Sphere unit { get { return new Sphere(Vector3.zero, 1); } }

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
        public Vector3 GetPoint(float horizontalAngle, float verticalAngle)
        {
            return center + PTUtils.PointOnSphere(radius, horizontalAngle, verticalAngle);
        }

        public bool Contains(Vector3 point)
        {
            return Intersect.PointSphere(point, center, radius);
        }

        public static Sphere Lerp(Sphere a, Sphere b, float t)
        {
            t = Mathf.Clamp01(t);
            return new Sphere(a.center + (b.center - a.center)*t, a.radius + (b.radius - a.radius)*t);
        }

        public static Sphere LerpUnclamped(Sphere a, Sphere b, float t)
        {
            return new Sphere(a.center + (b.center - a.center)*t, a.radius + (b.radius - a.radius)*t);
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
            return center.GetHashCode() ^ radius.GetHashCode() << 2;
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
    }
}
