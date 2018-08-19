using UnityEngine;

namespace ProceduralToolkit
{
    public struct IntersectionSphereSphere
    {
        public IntersectionType type;
        public Vector3 point;
        public Vector3 normal;
        public float radius;

        public static IntersectionSphereSphere None()
        {
            return new IntersectionSphereSphere {type = IntersectionType.None};
        }

        public static IntersectionSphereSphere Point(Vector3 point)
        {
            return new IntersectionSphereSphere
            {
                type = IntersectionType.Point,
                point = point,
            };
        }

        public static IntersectionSphereSphere Circle(Vector3 center, Vector3 normal, float radius)
        {
            return new IntersectionSphereSphere
            {
                type = IntersectionType.Circle,
                point = center,
                normal = normal,
                radius = radius,
            };
        }

        public static IntersectionSphereSphere Sphere(Vector3 center, float radius)
        {
            return new IntersectionSphereSphere
            {
                type = IntersectionType.Sphere,
                point = center,
                radius = radius,
            };
        }
    }
}
