using UnityEngine;

namespace ProceduralToolkit
{
    public struct IntersectionRayRay2
    {
        public IntersectionType type;
        public Vector2 pointA;
        public Vector2 pointB;

        public static IntersectionRayRay2 None()
        {
            return new IntersectionRayRay2 {type = IntersectionType.None};
        }

        public static IntersectionRayRay2 Point(Vector2 point)
        {
            return new IntersectionRayRay2
            {
                type = IntersectionType.Point,
                pointA = point,
            };
        }

        public static IntersectionRayRay2 Ray(Vector2 origin, Vector2 direction)
        {
            return new IntersectionRayRay2
            {
                type = IntersectionType.Ray,
                pointA = origin,
                pointB = direction,
            };
        }

        public static IntersectionRayRay2 Segment(Vector2 pointA, Vector2 pointB)
        {
            return new IntersectionRayRay2
            {
                type = IntersectionType.Segment,
                pointA = pointA,
                pointB = pointB,
            };
        }
    }
}
