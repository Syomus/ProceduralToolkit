using UnityEngine;

namespace ProceduralToolkit
{
    public struct IntersectionLineCircle
    {
        public IntersectionType type;
        public Vector2 pointA;
        public Vector2 pointB;

        public static IntersectionLineCircle None()
        {
            return new IntersectionLineCircle {type = IntersectionType.None};
        }

        public static IntersectionLineCircle Point(Vector2 point)
        {
            return new IntersectionLineCircle
            {
                type = IntersectionType.Point,
                pointA = point,
            };
        }

        public static IntersectionLineCircle TwoPoints(Vector2 pointA, Vector2 pointB)
        {
            return new IntersectionLineCircle
            {
                type = IntersectionType.TwoPoints,
                pointA = pointA,
                pointB = pointB,
            };
        }
    }
}
