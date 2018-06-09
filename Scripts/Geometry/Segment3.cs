using System;
using UnityEngine;

namespace ProceduralToolkit
{
    /// <summary>
    /// Representation of a 3D line segment
    /// </summary>
    [Serializable]
    public struct Segment3 : IEquatable<Segment3>, IFormattable
    {
        public Vector3 a;
        public Vector3 b;

        public Segment3(Vector3 a, Vector3 b)
        {
            this.a = a;
            this.b = b;
        }

        /// <summary>
        /// Returns a point on the segment at the given normalized position
        /// </summary>
        public Vector3 GetPoint(float position)
        {
            return Vector3.Lerp(a, b, position);
        }

        public static Segment3 Lerp(Segment3 a, Segment3 b, float t)
        {
            t = Mathf.Clamp01(t);
            return new Segment3(a.a + (b.a - a.a)*t, a.b + (b.b - a.b)*t);
        }

        public static Segment3 LerpUnclamped(Segment3 a, Segment3 b, float t)
        {
            return new Segment3(a.a + (b.a - a.a)*t, a.b + (b.b - a.b)*t);
        }

        public static explicit operator Line3(Segment3 segment)
        {
            return new Line3(segment.a, (segment.b - segment.a).normalized);
        }

        public static Segment3 operator +(Segment3 segment, Vector3 vector)
        {
            return new Segment3(segment.a + vector, segment.b + vector);
        }

        public static Segment3 operator -(Segment3 segment, Vector3 vector)
        {
            return new Segment3(segment.a - vector, segment.b - vector);
        }

        public static bool operator ==(Segment3 a, Segment3 b)
        {
            return a.a == b.a && a.b == b.b;
        }

        public static bool operator !=(Segment3 a, Segment3 b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            return a.GetHashCode() ^ b.GetHashCode() << 2;
        }

        public override bool Equals(object other)
        {
            return other is Segment3 && Equals((Segment3) other);
        }

        public bool Equals(Segment3 other)
        {
            return a.Equals(other.a) && b.Equals(other.b);
        }

        public override string ToString()
        {
            return string.Format("Segment3(a: {0}, b: {1})", a, b);
        }

        public string ToString(string format)
        {
            return string.Format("Segment3(a: {0}, b: {1})", a.ToString(format), b.ToString(format));
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            return string.Format("Segment3(a: {0}, b: {1})", a.ToString(format, formatProvider), b.ToString(format, formatProvider));
        }
    }
}
