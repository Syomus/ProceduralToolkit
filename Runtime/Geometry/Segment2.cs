using System;
using UnityEngine;

namespace ProceduralToolkit
{
    /// <summary>
    /// Representation of a 2D line segment
    /// </summary>
    [Serializable]
    public struct Segment2 : IEquatable<Segment2>, IFormattable
    {
        public Vector2 a;
        public Vector2 b;

        /// <summary>
        /// Returns the normalized direction of the segment
        /// </summary>
        public Vector2 direction { get { return (b - a).normalized; } }
        /// <summary>
        /// Returns the length of the segment
        /// </summary>
        public float length { get { return (b - a).magnitude; } }

        public Segment2(Vector2 a, Vector2 b)
        {
            this.a = a;
            this.b = b;
        }

        /// <summary>
        /// Access the a or b component using [0] or [1] respectively
        /// </summary>
        public Vector2 this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0: return a;
                    case 1: return b;
                    default:
                        throw new IndexOutOfRangeException("Invalid Segment2 index!");
                }
            }
            set
            {
                switch (index)
                {
                    case 0:
                        a = value;
                        break;
                    case 1:
                        b = value;
                        break;
                    default:
                        throw new IndexOutOfRangeException("Invalid Segment2 index!");
                }
            }
        }

        /// <summary>
        /// Returns a point on the segment at the given normalized position
        /// </summary>
        public Vector2 GetPoint(float position)
        {
            return Vector2.Lerp(a, b, position);
        }

        /// <summary>
        /// Linearly interpolates between two segments
        /// </summary>
        public static Segment2 Lerp(Segment2 a, Segment2 b, float t)
        {
            t = Mathf.Clamp01(t);
            return new Segment2(a.a + (b.a - a.a)*t, a.b + (b.b - a.b)*t);
        }

        /// <summary>
        /// Linearly interpolates between two segments without clamping the interpolant
        /// </summary>
        public static Segment2 LerpUnclamped(Segment2 a, Segment2 b, float t)
        {
            return new Segment2(a.a + (b.a - a.a)*t, a.b + (b.b - a.b)*t);
        }

        #region Casting operators

        public static explicit operator Line2(Segment2 segment)
        {
            return new Line2(segment.a, (segment.b - segment.a).normalized);
        }

        public static explicit operator Ray2D(Segment2 segment)
        {
            return new Ray2D(segment.a, (segment.b - segment.a).normalized);
        }

        public static explicit operator Segment3(Segment2 segment)
        {
            return new Segment3((Vector3) segment.a, (Vector3) segment.b);
        }

        #endregion Casting operators

        public static Segment2 operator +(Segment2 segment, Vector2 vector)
        {
            return new Segment2(segment.a + vector, segment.b + vector);
        }

        public static Segment2 operator -(Segment2 segment, Vector2 vector)
        {
            return new Segment2(segment.a - vector, segment.b - vector);
        }

        public static bool operator ==(Segment2 a, Segment2 b)
        {
            return a.a == b.a && a.b == b.b;
        }

        public static bool operator !=(Segment2 a, Segment2 b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            return a.GetHashCode() ^ (b.GetHashCode() << 2);
        }

        public override bool Equals(object other)
        {
            return other is Segment2 && Equals((Segment2) other);
        }

        public bool Equals(Segment2 other)
        {
            return a.Equals(other.a) && b.Equals(other.b);
        }

        public override string ToString()
        {
            return string.Format("Segment2(a: {0}, b: {1})", a, b);
        }

        public string ToString(string format)
        {
            return string.Format("Segment2(a: {0}, b: {1})", a.ToString(format), b.ToString(format));
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            return string.Format("Segment2(a: {0}, b: {1})", a.ToString(format, formatProvider), b.ToString(format, formatProvider));
        }
    }
}
