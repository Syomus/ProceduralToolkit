using UnityEngine;

namespace ProceduralToolkit
{
    public struct IntersectionRaySphere
    {
        public IntersectionType type;
        public Vector3 pointA;
        public Vector3 pointB;

        public static IntersectionRaySphere None()
        {
            return new IntersectionRaySphere {type = IntersectionType.None};
        }

        public static IntersectionRaySphere Point(Vector3 point)
        {
            return new IntersectionRaySphere
            {
                type = IntersectionType.Point,
                pointA = point,
            };
        }

        public static IntersectionRaySphere TwoPoints(Vector3 pointA, Vector3 pointB)
        {
            return new IntersectionRaySphere
            {
                type = IntersectionType.TwoPoints,
                pointA = pointA,
                pointB = pointB,
            };
        }
    }
}
