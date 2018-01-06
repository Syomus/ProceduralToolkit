using System;
using UnityEngine;

namespace ProceduralToolkit
{
    /// <summary>
    /// Representation of a circle
    /// </summary>
    [Serializable]
    public struct Circle
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

        public static Circle Lerp(Circle a, Circle b, float t)
        {
            t = Mathf.Clamp01(t);
            return new Circle(a.center + (b.center - a.center)*t, a.radius + (b.radius - a.radius)*t);
        }

        public static Circle LerpUnclamped(Circle a, Circle b, float t)
        {
            return new Circle(a.center + (b.center - a.center)*t, a.radius + (b.radius - a.radius)*t);
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
            return center.GetHashCode() ^ radius.GetHashCode() << 2;
        }

        public override bool Equals(object other)
        {
            if (!(other is Circle)) return false;

            Circle circle = (Circle) other;
            return center.Equals(circle.center) && radius.Equals(circle.radius);
        }

        public override string ToString()
        {
            return string.Format("Color(center: {0}, radius: {1})", center, radius);
        }

        public string ToString(string format)
        {
            return string.Format("Color(center: {0}, radius: {1})", center.ToString(format), radius.ToString(format));
        }
    }
}
