using UnityEngine;

namespace ProceduralToolkit
{
    public struct IntersectionLineSphere
    {
        public IntersectionType type;
        public Vector3 pointA;
        public Vector3 pointB;

        public static IntersectionLineSphere None()
        {
            return new IntersectionLineSphere {type = IntersectionType.None};
        }

        public static IntersectionLineSphere Point(Vector3 point)
        {
            return new IntersectionLineSphere
            {
                type = IntersectionType.Point,
                pointA = point,
            };
        }

        public static IntersectionLineSphere TwoPoints(Vector3 pointA, Vector3 pointB)
        {
            return new IntersectionLineSphere
            {
                type = IntersectionType.TwoPoints,
                pointA = pointA,
                pointB = pointB,
            };
        }
    }
}
