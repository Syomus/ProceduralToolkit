using System;
using UnityEngine;

namespace ProceduralToolkit
{
    public static class GizmosE
    {
        private static readonly Action<Vector3, Vector3> drawLine;

        static GizmosE()
        {
            drawLine = Gizmos.DrawLine;
        }

        public static void DrawWireQuadXY(Vector3 position, Quaternion rotation, Vector2 scale)
        {
            Draw.WireQuadXY(drawLine, position, rotation, scale);
        }

        public static void DrawWireQuadXZ(Vector3 position, Quaternion rotation, Vector2 scale)
        {
            Draw.WireQuadXZ(drawLine, position, rotation, scale);
        }

        public static void DrawWireCube(Vector3 position, Quaternion rotation, Vector3 scale)
        {
            Draw.WireCube(drawLine, position, rotation, scale);
        }

        public static void DrawWireCircleXY(Vector3 position, float radius)
        {
            Draw.WireCircleXY(drawLine, position, radius);
        }

        public static void DrawWireCircleXY(Vector3 position, Quaternion rotation, float radius)
        {
            Draw.WireCircleXY(drawLine, position, rotation, radius);
        }

        public static void DrawWireCircleXZ(Vector3 position, float radius)
        {
            Draw.WireCircleXZ(drawLine, position, radius);
        }

        public static void DrawWireCircleXZ(Vector3 position, Quaternion rotation, float radius)
        {
            Draw.WireCircleXZ(drawLine, position, rotation, radius);
        }

        public static void DrawWireCircleYZ(Vector3 position, float radius)
        {
            Draw.WireCircleYZ(drawLine, position, radius);
        }

        public static void DrawWireCircleYZ(Vector3 position, Quaternion rotation, float radius)
        {
            Draw.WireCircleYZ(drawLine, position, rotation, radius);
        }

        public static void DrawWireSphere(Vector3 position, Quaternion rotation, float radius)
        {
            Draw.WireSphere(drawLine, position, rotation, radius);
        }
    }
}