using System;
using UnityEngine;

namespace ProceduralToolkit
{
    [Serializable]
    public struct Line2
    {
        public Vector2 origin;
        public Vector2 direction;

        public static Line2 xAxis { get { return new Line2(Vector2.zero, Vector2.right); } }
        public static Line2 yAxis { get { return new Line2(Vector2.zero, Vector2.up); } }

        public Line2(Vector2 origin, Vector2 direction)
        {
            this.origin = origin;
            this.direction = direction;
        }

        public override string ToString()
        {
            return string.Format("Line2(origin: {0}, direction: {1})", origin, direction);
        }

        public string ToString(string format)
        {
            return string.Format("Line2(origin: {0}, direction: {1})", origin.ToString(format), direction.ToString(format));
        }
    }
}
