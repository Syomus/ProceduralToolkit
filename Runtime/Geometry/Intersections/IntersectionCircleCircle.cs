using UnityEngine;

namespace ProceduralToolkit
{
    public struct IntersectionCircleCircle
    {
        public IntersectionType type;
        public Vector2 pointA;
        public Vector2 pointB;

        public static IntersectionCircleCircle None()
        {
            return new IntersectionCircleCircle {type = IntersectionType.None};
        }

        public static IntersectionCircleCircle Point(Vector2 point)
        {
            return new IntersectionCircleCircle
            {
                type = IntersectionType.Point,
                pointA = point,
            };
        }

        public static IntersectionCircleCircle TwoPoints(Vector2 pointA, Vector2 pointB)
        {
            return new IntersectionCircleCircle
            {
                type = IntersectionType.TwoPoints,
                pointA = pointA,
                pointB = pointB,
            };
        }

        public static IntersectionCircleCircle Circle()
        {
            return new IntersectionCircleCircle {type = IntersectionType.Circle};
        }
    }
}
