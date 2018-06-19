using UnityEngine;

namespace ProceduralToolkit
{
    public struct IntersectionRayCircle
    {
        public IntersectionType type;
        public Vector2 pointA;
        public Vector2 pointB;

        public static IntersectionRayCircle None()
        {
            return new IntersectionRayCircle {type = IntersectionType.None};
        }

        public static IntersectionRayCircle Point(Vector2 point)
        {
            return new IntersectionRayCircle
            {
                type = IntersectionType.Point,
                pointA = point,
            };
        }

        public static IntersectionRayCircle TwoPoints(Vector2 pointA, Vector2 pointB)
        {
            return new IntersectionRayCircle
            {
                type = IntersectionType.TwoPoints,
                pointA = pointA,
                pointB = pointB,
            };
        }
    }
}
