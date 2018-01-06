using System;
using UnityEngine;

namespace ProceduralToolkit
{
    /// <summary>
    /// Representation of a 2D line segment
    /// </summary>
    [Serializable]
    public struct Segment2
    {
        public Vector2 a;
        public Vector2 b;

        public Segment2(Vector2 a, Vector2 b)
        {
            this.a = a;
            this.b = b;
        }

        public static Segment2 Lerp(Segment2 a, Segment2 b, float t)
        {
            t = Mathf.Clamp01(t);
            return new Segment2(a.a + (b.a - a.a)*t, a.b + (b.b - a.b)*t);
        }

        public static Segment2 LerpUnclamped(Segment2 a, Segment2 b, float t)
        {
            return new Segment2(a.a + (b.a - a.a)*t, a.b + (b.b - a.b)*t);
        }

        public static explicit operator Line2(Segment2 segment)
        {
            return new Line2(segment.a, (segment.b - segment.a).normalized);
        }

        public static Segment2 operator +(Segment2 segment, Vector2 vector)
        {
            return new Segment2(segment.a + vector, segment.b + vector);
        }

        public static Segment2 operator -(Segment2 segment, Vector2 vector)
        {
            return new Segment2(segment.a - vector, segment.b - vector);
        }

        public override string ToString()
        {
            return string.Format("Segment2(a: {0}, b: {1})", a, b);
        }

        public string ToString(string format)
        {
            return string.Format("Segment2(a: {0}, b: {1})", a.ToString(format), b.ToString(format));
        }
    }
}
