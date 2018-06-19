using UnityEngine;

namespace ProceduralToolkit
{
    public struct IntersectionRaySegment2
    {
        public IntersectionType type;
        public Vector2 pointA;
        public Vector2 pointB;

        public static IntersectionRaySegment2 None()
        {
            return new IntersectionRaySegment2 {type = IntersectionType.None};
        }

        public static IntersectionRaySegment2 Point(Vector2 pointA)
        {
            return new IntersectionRaySegment2
            {
                type = IntersectionType.Point,
                pointA = pointA,
            };
        }

        public static IntersectionRaySegment2 Segment(Vector2 pointA, Vector2 pointB)
        {
            return new IntersectionRaySegment2
            {
                type = IntersectionType.Segment,
                pointA = pointA,
                pointB = pointB,
            };
        }
    }
}
