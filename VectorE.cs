using UnityEngine;

namespace ProceduralToolkit
{
    public static class VectorE
    {
        public static Vector2 OnlyX(this Vector2 vector)
        {
            return new Vector2(vector.x, 0);
        }

        public static Vector2 OnlyY(this Vector2 vector)
        {
            return new Vector2(0, vector.y);
        }

        public static Vector3 OnlyX(this Vector3 vector)
        {
            return new Vector3(vector.x, 0, 0);
        }

        public static Vector3 OnlyY(this Vector3 vector)
        {
            return new Vector3(0, vector.y, 0);
        }

        public static Vector3 OnlyZ(this Vector3 vector)
        {
            return new Vector3(0, 0, vector.z);
        }

        public static Vector3 OnlyXY(this Vector3 vector)
        {
            return new Vector3(vector.x, vector.y, 0);
        }

        public static Vector3 OnlyXZ(this Vector3 vector)
        {
            return new Vector3(vector.x, 0, vector.z);
        }

        public static Vector3 OnlyYZ(this Vector3 vector)
        {
            return new Vector3(0, vector.y, vector.z);
        }
    }
}