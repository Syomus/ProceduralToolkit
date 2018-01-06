using System;
using UnityEngine;

namespace ProceduralToolkit
{
    [Serializable]
    public struct Circle
    {
        public Vector2 center;
        public float radius;

        public static Circle unit { get { return new Circle(Vector2.zero, 1); } }

        public Circle(Vector2 center, float radius)
        {
            this.center = center;
            this.radius = radius;
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
