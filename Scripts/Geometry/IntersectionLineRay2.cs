using UnityEngine;

namespace ProceduralToolkit
{
    public struct IntersectionLineRay2
    {
        public IntersectionType type;
        public Vector2 point;

        public static IntersectionLineRay2 None()
        {
            return new IntersectionLineRay2 {type = IntersectionType.None};
        }

        public static IntersectionLineRay2 Point(Vector2 point)
        {
            return new IntersectionLineRay2
            {
                type = IntersectionType.Point,
                point = point,
            };
        }

        public static IntersectionLineRay2 Ray(Vector2 point)
        {
            return new IntersectionLineRay2
            {
                type = IntersectionType.Ray,
                point = point,
            };
        }
    }
}
