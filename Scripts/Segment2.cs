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
