using UnityEngine;

namespace ProceduralToolkit
{
    public struct IntersectionSegmentCircle
    {
        public IntersectionType type;
        public Vector2 pointA;
        public Vector2 pointB;

        public static IntersectionSegmentCircle None()
        {
            return new IntersectionSegmentCircle {type = IntersectionType.None};
        }

        public static IntersectionSegmentCircle Point(Vector2 point)
        {
            return new IntersectionSegmentCircle
            {
                type = IntersectionType.Point,
                pointA = point,
            };
        }

        public static IntersectionSegmentCircle TwoPoints(Vector2 pointA, Vector2 pointB)
        {
            return new IntersectionSegmentCircle
            {
                type = IntersectionType.TwoPoints,
                pointA = pointA,
                pointB = pointB,
            };
        }
    }
}
