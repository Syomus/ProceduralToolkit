using UnityEngine;

namespace ProceduralToolkit
{
    /// <summary>
    /// Collection of drawing methods similar to Debug.DrawLine
    /// </summary>
    public static class DebugE
    {
        private static readonly Draw.DebugDrawLine drawLine;
        private static readonly Color white = Color.white;

        static DebugE()
        {
            drawLine = Debug.DrawLine;
        }

        #region DrawRay

        /// <summary>
        /// Draws a ray starting at ray.origin to ray.origin + ray.direction
        /// </summary>
        public static void DrawRay(Ray ray)
        {
            DrawRay(ray, white);
        }

        /// <summary>
        /// Draws a ray starting at ray.origin to ray.origin + ray.direction
        /// </summary>
        public static void DrawRay(Ray ray, Color color, float duration = 0, bool depthTest = true)
        {
            Draw.Ray(drawLine, ray, color, duration, depthTest);
        }

        #endregion DrawRay

        #region DrawSegment2

        /// <summary>
        /// Draws a segment
        /// </summary>
        public static void DrawSegment2(Segment2 segment)
        {
            DrawSegment2(segment, white);
        }

        /// <summary>
        /// Draws a segment
        /// </summary>
        public static void DrawSegment2(Segment2 segment, Color color, float duration = 0, bool depthTest = true)
        {
            Draw.Segment2(drawLine, segment, color, duration, depthTest);
        }

        #endregion DrawSegment2

        #region DrawSegment3

        /// <summary>
        /// Draws a segment
        /// </summary>
        public static void DrawSegment3(Segment3 segment)
        {
            DrawSegment3(segment, white);
        }

        /// <summary>
        /// Draws a segment
        /// </summary>
        public static void DrawSegment3(Segment3 segment, Color color, float duration = 0, bool depthTest = true)
        {
            Draw.Segment3(drawLine, segment, color, duration, depthTest);
        }

        #endregion DrawSegment3

        #region DrawWireQuad

        /// <summary>
        /// Draws a wireframe quad with position, rotation and scale
        /// </summary>
        public static void DrawWireQuadXY(Vector3 position, Quaternion rotation, Vector2 scale)
        {
            DrawWireQuadXY(position, rotation, scale, white);
        }

        /// <summary>
        /// Draws a wireframe quad with position, rotation and scale
        /// </summary>
        public static void DrawWireQuadXY(Vector3 position, Quaternion rotation, Vector2 scale, Color color, float duration = 0,
            bool depthTest = true)
        {
            Draw.WireQuadXY(drawLine, position, rotation, scale, color, duration, depthTest);
        }

        /// <summary>
        /// Draws a wireframe quad with position, rotation and scale
        /// </summary>
        public static void DrawWireQuadXZ(Vector3 position, Quaternion rotation, Vector2 scale)
        {
            DrawWireQuadXZ(position, rotation, scale, white);
        }

        /// <summary>
        /// Draws a wireframe quad with position, rotation and scale
        /// </summary>
        public static void DrawWireQuadXZ(Vector3 position, Quaternion rotation, Vector2 scale, Color color, float duration = 0,
            bool depthTest = true)
        {
            Draw.WireQuadXZ(drawLine, position, rotation, scale, color, duration, depthTest);
        }

        /// <summary>
        /// Draws a wireframe quad with position, rotation and scale
        /// </summary>
        public static void DrawWireQuadYZ(Vector3 position, Quaternion rotation, Vector2 scale)
        {
            DrawWireQuadYZ(position, rotation, scale, white);
        }

        /// <summary>
        /// Draws a wireframe quad with position, rotation and scale
        /// </summary>
        public static void DrawWireQuadYZ(Vector3 position, Quaternion rotation, Vector2 scale, Color color, float duration = 0,
            bool depthTest = true)
        {
            Draw.WireQuadYZ(drawLine, position, rotation, scale, color, duration, depthTest);
        }

        #endregion DrawWireQuad

        #region DrawWireCube

        /// <summary>
        /// Draws a wireframe cube with position, rotation and scale
        /// </summary>
        public static void DrawWireCube(Vector3 position, Quaternion rotation, Vector3 scale)
        {
            DrawWireCube(position, rotation, scale, white);
        }

        /// <summary>
        /// Draws a wireframe cube with position, rotation and scale
        /// </summary>
        public static void DrawWireCube(Vector3 position, Quaternion rotation, Vector3 scale, Color color, float duration = 0, bool depthTest = true)
        {
            Draw.WireCube(drawLine, position, rotation, scale, color, duration, depthTest);
        }

        #endregion DrawWireCube

        #region DrawWireCircle

        /// <summary>
        /// Draws a wireframe circle with position and radius
        /// </summary>
        public static void DrawWireCircleXY(Vector3 position, float radius)
        {
            DrawWireCircleXY(position, radius, white);
        }

        /// <summary>
        /// Draws a wireframe circle with position and radius
        /// </summary>
        public static void DrawWireCircleXY(Vector3 position, float radius, Color color, float duration = 0, bool depthTest = true)
        {
            Draw.WireCircleXY(drawLine, position, radius, color, duration, depthTest);
        }

        /// <summary>
        /// Draws a wireframe circle with position, rotation and radius
        /// </summary>
        public static void DrawWireCircleXY(Vector3 position, Quaternion rotation, float radius)
        {
            DrawWireCircleXY(position, rotation, radius, white);
        }

        /// <summary>
        /// Draws a wireframe circle with position, rotation and radius
        /// </summary>
        public static void DrawWireCircleXY(Vector3 position, Quaternion rotation, float radius, Color color, float duration = 0,
            bool depthTest = true)
        {
            Draw.WireCircleXY(drawLine, position, rotation, radius, color, duration, depthTest);
        }

        /// <summary>
        /// Draws a wireframe circle with position and radius
        /// </summary>
        public static void DrawWireCircleXZ(Vector3 position, float radius)
        {
            DrawWireCircleXZ(position, radius, white);
        }

        /// <summary>
        /// Draws a wireframe circle with position and radius
        /// </summary>
        public static void DrawWireCircleXZ(Vector3 position, float radius, Color color, float duration = 0, bool depthTest = true)
        {
            Draw.WireCircleXZ(drawLine, position, radius, color, duration, depthTest);
        }

        /// <summary>
        /// Draws a wireframe circle with position, rotation and radius
        /// </summary>
        public static void DrawWireCircleXZ(Vector3 position, Quaternion rotation, float radius)
        {
            DrawWireCircleXZ(position, rotation, radius, white);
        }

        /// <summary>
        /// Draws a wireframe circle with position, rotation and radius
        /// </summary>
        public static void DrawWireCircleXZ(Vector3 position, Quaternion rotation, float radius, Color color, float duration = 0,
            bool depthTest = true)
        {
            Draw.WireCircleXZ(drawLine, position, rotation, radius, color, duration, depthTest);
        }

        /// <summary>
        /// Draws a wireframe circle with position and radius
        /// </summary>
        public static void DrawWireCircleYZ(Vector3 position, float radius)
        {
            DrawWireCircleYZ(position, radius, white);
        }

        /// <summary>
        /// Draws a wireframe circle with position and radius
        /// </summary>
        public static void DrawWireCircleYZ(Vector3 position, float radius, Color color, float duration = 0, bool depthTest = true)
        {
            Draw.WireCircleYZ(drawLine, position, radius, color, duration, depthTest);
        }

        /// <summary>
        /// Draws a wireframe circle with position, rotation and radius
        /// </summary>
        public static void DrawWireCircleYZ(Vector3 position, Quaternion rotation, float radius)
        {
            DrawWireCircleYZ(position, rotation, radius, white);
        }

        /// <summary>
        /// Draws a wireframe circle with position, rotation and radius
        /// </summary>
        public static void DrawWireCircleYZ(Vector3 position, Quaternion rotation, float radius, Color color, float duration = 0,
            bool depthTest = true)
        {
            Draw.WireCircleYZ(drawLine, position, rotation, radius, color, duration, depthTest);
        }

        #endregion DrawWireCircle

        #region DrawWireArc

        /// <summary>
        /// Draws a wireframe circular arc with position and radius
        /// </summary>
        public static void DrawWireArcXY(Vector3 position, float radius, float fromAngle, float toAngle)
        {
            DrawWireArcXY(position, radius, fromAngle, toAngle, white);
        }

        /// <summary>
        /// Draws a wireframe circular arc with position and radius
        /// </summary>
        public static void DrawWireArcXY(Vector3 position, float radius, float fromAngle, float toAngle, Color color, float duration = 0,
            bool depthTest = true)
        {
            Draw.WireArcXY(drawLine, position, radius, fromAngle, toAngle, color, duration, depthTest);
        }

        /// <summary>
        /// Draws a wireframe circular arc with position, rotation and radius
        /// </summary>
        public static void DrawWireArcXY(Vector3 position, Quaternion rotation, float radius, float fromAngle, float toAngle)
        {
            DrawWireArcXY(position, rotation, radius, fromAngle, toAngle, white);
        }

        /// <summary>
        /// Draws a wireframe circular arc with position, rotation and radius
        /// </summary>
        public static void DrawWireArcXY(Vector3 position, Quaternion rotation, float radius, float fromAngle, float toAngle, Color color,
            float duration = 0, bool depthTest = true)
        {
            Draw.WireArcXY(drawLine, position, rotation, radius, fromAngle, toAngle, color, duration, depthTest);
        }

        /// <summary>
        /// Draws a wireframe circular arc with position and radius
        /// </summary>
        public static void DrawWireArcXZ(Vector3 position, float radius, float fromAngle, float toAngle)
        {
            DrawWireArcXZ(position, radius, fromAngle, toAngle, white);
        }

        /// <summary>
        /// Draws a wireframe circular arc with position and radius
        /// </summary>
        public static void DrawWireArcXZ(Vector3 position, float radius, float fromAngle, float toAngle, Color color, float duration = 0,
            bool depthTest = true)
        {
            Draw.WireArcXZ(drawLine, position, radius, fromAngle, toAngle, color, duration, depthTest);
        }

        /// <summary>
        /// Draws a wireframe circular arc with position, rotation and radius
        /// </summary>
        public static void DrawWireArcXZ(Vector3 position, Quaternion rotation, float radius, float fromAngle, float toAngle)
        {
            DrawWireArcXZ(position, rotation, radius, fromAngle, toAngle, white);
        }

        /// <summary>
        /// Draws a wireframe circular arc with position, rotation and radius
        /// </summary>
        public static void DrawWireArcXZ(Vector3 position, Quaternion rotation, float radius, float fromAngle, float toAngle, Color color,
            float duration = 0, bool depthTest = true)
        {
            Draw.WireArcXZ(drawLine, position, rotation, radius, fromAngle, toAngle, color, duration, depthTest);
        }

        /// <summary>
        /// Draws a wireframe circular arc with position and radius
        /// </summary>
        public static void DrawWireArcYZ(Vector3 position, float radius, float fromAngle, float toAngle)
        {
            DrawWireArcYZ(position, radius, fromAngle, toAngle, white);
        }

        /// <summary>
        /// Draws a wireframe circular arc with position and radius
        /// </summary>
        public static void DrawWireArcYZ(Vector3 position, float radius, float fromAngle, float toAngle, Color color, float duration = 0,
            bool depthTest = true)
        {
            Draw.WireArcYZ(drawLine, position, radius, fromAngle, toAngle, color, duration, depthTest);
        }

        /// <summary>
        /// Draws a wireframe circular arc with position, rotation and radius
        /// </summary>
        public static void DrawWireArcYZ(Vector3 position, Quaternion rotation, float radius, float fromAngle, float toAngle)
        {
            DrawWireArcYZ(position, rotation, radius, fromAngle, toAngle, white);
        }

        /// <summary>
        /// Draws a wireframe circular arc with position, rotation and radius
        /// </summary>
        public static void DrawWireArcYZ(Vector3 position, Quaternion rotation, float radius, float fromAngle, float toAngle, Color color,
            float duration = 0, bool depthTest = true)
        {
            Draw.WireArcYZ(drawLine, position, rotation, radius, fromAngle, toAngle, color, duration, depthTest);
        }

        #endregion DrawWireArc

        #region DrawWireSphere

        /// <summary>
        /// Draws a wireframe sphere with position, rotation and radius
        /// </summary>
        public static void DrawWireSphere(Vector3 position, Quaternion rotation, float radius)
        {
            DrawWireSphere(position, rotation, radius, white);
        }

        /// <summary>
        /// Draws a wireframe sphere with position, rotation and radius
        /// </summary>
        public static void DrawWireSphere(Vector3 position, Quaternion rotation, float radius, Color color, float duration = 0, bool depthTest = true)
        {
            Draw.WireSphere(drawLine, position, rotation, radius, color, duration, depthTest);
        }

        #endregion DrawWireSphere

        #region DrawWireHemisphere

        /// <summary>
        /// Draws a wireframe hemisphere with position, rotation and radius
        /// </summary>
        public static void DrawWireHemisphere(Vector3 position, Quaternion rotation, float radius)
        {
            DrawWireHemisphere(position, rotation, radius, white);
        }

        /// <summary>
        /// Draws a wireframe hemisphere with position, rotation and radius
        /// </summary>
        public static void DrawWireHemisphere(Vector3 position, Quaternion rotation, float radius, Color color, float duration = 0,
            bool depthTest = true)
        {
            Draw.WireHemisphere(drawLine, position, rotation, radius, color, duration, depthTest);
        }

        #endregion DrawWireHemisphere

        #region DrawWireCone

        /// <summary>
        /// Draws a wireframe cone with position and rotation
        /// </summary>
        public static void DrawWireCone(Vector3 position, Quaternion rotation, float apexRadius, float angle, float length)
        {
            DrawWireCone(position, rotation, apexRadius, angle, length, white);
        }

        /// <summary>
        /// Draws a wireframe cone with position and rotation
        /// </summary>
        public static void DrawWireCone(Vector3 position, Quaternion rotation, float apexRadius, float angle, float length, Color color,
            float duration = 0, bool depthTest = true)
        {
            Draw.WireCone(drawLine, position, rotation, apexRadius, angle, length, color, duration, depthTest);
        }

        #endregion DrawWireCone
    }
}
