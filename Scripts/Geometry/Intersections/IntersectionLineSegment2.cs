using UnityEngine;

namespace ProceduralToolkit
{
    public struct IntersectionLineSegment2
    {
        public IntersectionType type;
        public Vector2 pointA;
        public Vector2 pointB;

        public static IntersectionLineSegment2 None()
        {
            return new IntersectionLineSegment2 {type = IntersectionType.None};
        }

        public static IntersectionLineSegment2 Point(Vector2 pointA)
        {
            return new IntersectionLineSegment2
            {
                type = IntersectionType.Point,
                pointA = pointA,
            };
        }

        public static IntersectionLineSegment2 Segment(Vector2 pointA, Vector2 pointB)
        {
            return new IntersectionLineSegment2
            {
                type = IntersectionType.Segment,
                pointA = pointA,
                pointB = pointB,
            };
        }
    }
}
