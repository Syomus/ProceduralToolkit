using UnityEngine;

namespace ProceduralToolkit
{
    public static class DebugE
    {
        private static readonly Draw.DebugDrawLine drawLine;

        static DebugE()
        {
            drawLine = Debug.DrawLine;
        }

        public static void DrawRay(Ray ray)
        {
            DrawRay(ray, Color.white);
        }

        public static void DrawRay(
            Ray ray,
            Color color,
            float duration = 0,
            bool depthTest = true)
        {
            Draw.WireRay(drawLine, ray, color, duration, depthTest);
        }

        public static void DrawWireQuadXY(Vector3 position, Quaternion rotation, Vector2 scale)
        {
            DrawWireQuadXY(position, rotation, scale, Color.white);
        }

        public static void DrawWireQuadXY(
            Vector3 position,
            Quaternion rotation,
            Vector2 scale,
            Color color,
            float duration = 0,
            bool depthTest = true)
        {
            Draw.WireQuadXY(drawLine, position, rotation, scale, color, duration, depthTest);
        }

        public static void DrawWireQuadXZ(Vector3 position, Quaternion rotation, Vector2 scale)
        {
            DrawWireQuadXZ(position, rotation, scale, Color.white);
        }

        public static void DrawWireQuadXZ(
            Vector3 position,
            Quaternion rotation,
            Vector2 scale,
            Color color,
            float duration = 0,
            bool depthTest = true)
        {
            Draw.WireQuadXZ(drawLine, position, rotation, scale, color, duration, depthTest);
        }

        public static void DrawWireQuadYZ(Vector3 position, Quaternion rotation, Vector2 scale)
        {
            DrawWireQuadYZ(position, rotation, scale, Color.white);
        }

        public static void DrawWireQuadYZ(
            Vector3 position,
            Quaternion rotation,
            Vector2 scale,
            Color color,
            float duration = 0,
            bool depthTest = true)
        {
            Draw.WireQuadYZ(drawLine, position, rotation, scale, color, duration, depthTest);
        }

        public static void DrawWireCube(Vector3 position, Quaternion rotation, Vector3 scale)
        {
            DrawWireCube(position, rotation, scale, Color.white);
        }

        public static void DrawWireCube(
            Vector3 position,
            Quaternion rotation,
            Vector3 scale,
            Color color,
            float duration = 0,
            bool depthTest = true)
        {
            Draw.WireCube(drawLine, position, rotation, scale, color, duration, depthTest);
        }

        #region DrawWireCircle

        public static void DrawWireCircleXY(Vector3 position, float radius)
        {
            DrawWireCircleXY(position, radius, Color.white);
        }

        public static void DrawWireCircleXY(
            Vector3 position,
            float radius,
            Color color,
            float duration = 0,
            bool depthTest = true)
        {
            Draw.WireCircleXY(drawLine, position, radius, color, duration, depthTest);
        }

        public static void DrawWireCircleXY(Vector3 position, Quaternion rotation, float radius)
        {
            DrawWireCircleXY(position, rotation, radius, Color.white);
        }

        public static void DrawWireCircleXY(
            Vector3 position,
            Quaternion rotation,
            float radius,
            Color color,
            float duration = 0,
            bool depthTest = true)
        {
            Draw.WireCircleXY(drawLine, position, rotation, radius, color, duration, depthTest);
        }

        public static void DrawWireCircleXZ(Vector3 position, float radius)
        {
            DrawWireCircleXZ(position, radius, Color.white);
        }

        public static void DrawWireCircleXZ(
            Vector3 position,
            float radius,
            Color color,
            float duration = 0,
            bool depthTest = true)
        {
            Draw.WireCircleXZ(drawLine, position, radius, color, duration, depthTest);
        }

        public static void DrawWireCircleXZ(Vector3 position, Quaternion rotation, float radius)
        {
            DrawWireCircleXZ(position, rotation, radius, Color.white);
        }

        public static void DrawWireCircleXZ(
            Vector3 position,
            Quaternion rotation,
            float radius,
            Color color,
            float duration = 0,
            bool depthTest = true)
        {
            Draw.WireCircleXZ(drawLine, position, rotation, radius, color, duration, depthTest);
        }

        public static void DrawWireCircleYZ(Vector3 position, float radius)
        {
            DrawWireCircleYZ(position, radius, Color.white);
        }

        public static void DrawWireCircleYZ(
            Vector3 position,
            float radius,
            Color color,
            float duration = 0,
            bool depthTest = true)
        {
            Draw.WireCircleYZ(drawLine, position, radius, color, duration, depthTest);
        }

        public static void DrawWireCircleYZ(Vector3 position, Quaternion rotation, float radius)
        {
            DrawWireCircleYZ(position, rotation, radius, Color.white);
        }

        public static void DrawWireCircleYZ(
            Vector3 position,
            Quaternion rotation,
            float radius,
            Color color,
            float duration = 0,
            bool depthTest = true)
        {
            Draw.WireCircleYZ(drawLine, position, rotation, radius, color, duration, depthTest);
        }

        #endregion DrawWireCicle

        #region DrawWireArc

        public static void DrawWireArcXY(Vector3 position, float radius, float fromAngle, float toAngle)
        {
            DrawWireArcXY(position, radius, fromAngle, toAngle, Color.white);
        }

        public static void DrawWireArcXY(
            Vector3 position,
            float radius,
            float fromAngle,
            float toAngle,
            Color color,
            float duration = 0,
            bool depthTest = true)
        {
            Draw.WireArcXY(drawLine, position, radius, fromAngle, toAngle, color, duration, depthTest);
        }

        public static void DrawWireArcXY(Vector3 position, Quaternion rotation, float radius, float fromAngle,
            float toAngle)
        {
            DrawWireArcXY(position, rotation, radius, fromAngle, toAngle, Color.white);
        }

        public static void DrawWireArcXY(
            Vector3 position,
            Quaternion rotation,
            float radius,
            float fromAngle,
            float toAngle,
            Color color,
            float duration = 0,
            bool depthTest = true)
        {
            Draw.WireArcXY(drawLine, position, rotation, radius, fromAngle, toAngle, color, duration, depthTest);
        }

        public static void DrawWireArcXZ(Vector3 position, float radius, float fromAngle, float toAngle)
        {
            DrawWireArcXZ(position, radius, fromAngle, toAngle, Color.white);
        }

        public static void DrawWireArcXZ(
            Vector3 position,
            float radius,
            float fromAngle,
            float toAngle,
            Color color,
            float duration = 0,
            bool depthTest = true)
        {
            Draw.WireArcXZ(drawLine, position, radius, fromAngle, toAngle, color, duration, depthTest);
        }

        public static void DrawWireArcXZ(Vector3 position, Quaternion rotation, float radius, float fromAngle,
            float toAngle)
        {
            DrawWireArcXZ(position, rotation, radius, fromAngle, toAngle, Color.white);
        }

        public static void DrawWireArcXZ(
            Vector3 position,
            Quaternion rotation,
            float radius,
            float fromAngle,
            float toAngle,
            Color color,
            float duration = 0,
            bool depthTest = true)
        {
            Draw.WireArcXZ(drawLine, position, rotation, radius, fromAngle, toAngle, color, duration, depthTest);
        }

        public static void DrawWireArcYZ(Vector3 position, float radius, float fromAngle, float toAngle)
        {
            DrawWireArcYZ(position, radius, fromAngle, toAngle, Color.white);
        }

        public static void DrawWireArcYZ(
            Vector3 position,
            float radius,
            float fromAngle,
            float toAngle,
            Color color,
            float duration = 0,
            bool depthTest = true)
        {
            Draw.WireArcYZ(drawLine, position, radius, fromAngle, toAngle, color, duration, depthTest);
        }

        public static void DrawWireArcYZ(Vector3 position, Quaternion rotation, float radius, float fromAngle,
            float toAngle)
        {
            DrawWireArcYZ(position, rotation, radius, fromAngle, toAngle, Color.white);
        }

        public static void DrawWireArcYZ(
            Vector3 position,
            Quaternion rotation,
            float radius,
            float fromAngle,
            float toAngle,
            Color color,
            float duration = 0,
            bool depthTest = true)
        {
            Draw.WireArcYZ(drawLine, position, rotation, radius, fromAngle, toAngle, color, duration, depthTest);
        }

        #endregion DrawWireArc

        public static void DrawWireSphere(Vector3 position, Quaternion rotation, float radius)
        {
            DrawWireSphere(position, rotation, radius, Color.white);
        }

        public static void DrawWireSphere(
            Vector3 position,
            Quaternion rotation,
            float radius,
            Color color,
            float duration = 0,
            bool depthTest = true)
        {
            Draw.WireSphere(drawLine, position, rotation, radius, color, duration, depthTest);
        }

        public static void DrawWireHemisphere(Vector3 position, Quaternion rotation, float radius)
        {
            DrawWireHemisphere(position, rotation, radius, Color.white);
        }

        public static void DrawWireHemisphere(
            Vector3 position,
            Quaternion rotation,
            float radius,
            Color color,
            float duration = 0,
            bool depthTest = true)
        {
            Draw.WireHemisphere(drawLine, position, rotation, radius, color, duration, depthTest);
        }

        public static void DrawWireCone(
            Vector3 position,
            Quaternion rotation,
            float apexRadius,
            float angle,
            float length)
        {
            DrawWireCone(position, rotation, apexRadius, angle, length, Color.white);
        }

        public static void DrawWireCone(
            Vector3 position,
            Quaternion rotation,
            float apexRadius,
            float angle,
            float length,
            Color color,
            float duration = 0,
            bool depthTest = true)
        {
            Draw.WireCone(drawLine, position, rotation, apexRadius, angle, length, color, duration, depthTest);
        }
    }
}