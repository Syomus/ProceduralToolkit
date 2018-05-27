using UnityEngine;

namespace ProceduralToolkit
{
    public struct IntersectionSegmentSegment2
    {
        public IntersectionType type;
        public Vector2 pointA;
        public Vector2 pointB;

        public static IntersectionSegmentSegment2 None()
        {
            return new IntersectionSegmentSegment2 {type = IntersectionType.None};
        }

        public static IntersectionSegmentSegment2 Point(Vector2 point)
        {
            return new IntersectionSegmentSegment2
            {
                type = IntersectionType.Point,
                pointA = point,
            };
        }

        public static IntersectionSegmentSegment2 Segment(Vector2 pointA, Vector2 pointB)
        {
            return new IntersectionSegmentSegment2
            {
                type = IntersectionType.Segment,
                pointA = pointA,
                pointB = pointB,
            };
        }
    }
}
