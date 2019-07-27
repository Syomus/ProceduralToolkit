using UnityEngine;

namespace ProceduralToolkit
{
    public struct IntersectionSegmentSphere
    {
        public IntersectionType type;
        public Vector3 pointA;
        public Vector3 pointB;

        public static IntersectionSegmentSphere None()
        {
            return new IntersectionSegmentSphere {type = IntersectionType.None};
        }

        public static IntersectionSegmentSphere Point(Vector3 point)
        {
            return new IntersectionSegmentSphere
            {
                type = IntersectionType.Point,
                pointA = point,
            };
        }

        public static IntersectionSegmentSphere TwoPoints(Vector3 pointA, Vector3 pointB)
        {
            return new IntersectionSegmentSphere
            {
                type = IntersectionType.TwoPoints,
                pointA = pointA,
                pointB = pointB,
            };
        }
    }
}
