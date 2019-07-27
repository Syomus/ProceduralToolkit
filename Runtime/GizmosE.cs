using System;
using UnityEngine;

namespace ProceduralToolkit
{
    /// <summary>
    /// Collection of drawing methods similar to Gizmos
    /// </summary>
    public static class GizmosE
    {
        private static readonly Action<Vector3, Vector3> drawLine;

        static GizmosE()
        {
            drawLine = Gizmos.DrawLine;
        }

        #region DrawWireQuad

        /// <summary>
        /// Draws a wireframe quad with position, rotation and scale
        /// </summary>
        public static void DrawWireQuadXY(Vector3 position, Quaternion rotation, Vector2 scale)
        {
            Draw.WireQuadXY(drawLine, position, rotation, scale);
        }

        /// <summary>
        /// Draws a wireframe quad with position, rotation and scale
        /// </summary>
        public static void DrawWireQuadXZ(Vector3 position, Quaternion rotation, Vector2 scale)
        {
            Draw.WireQuadXZ(drawLine, position, rotation, scale);
        }

        /// <summary>
        /// Draws a wireframe quad with position, rotation and scale
        /// </summary>
        public static void DrawWireQuadYZ(Vector3 position, Quaternion rotation, Vector2 scale)
        {
            Draw.WireQuadYZ(drawLine, position, rotation, scale);
        }

        #endregion DrawWireQuad

        /// <summary>
        /// Draws a wireframe cube with position, rotation and scale
        /// </summary>
        public static void DrawWireCube(Vector3 position, Quaternion rotation, Vector3 scale)
        {
            Draw.WireCube(drawLine, position, rotation, scale);
        }

        #region DrawWireCircle

        /// <summary>
        /// Draws a wireframe circle with position and radius
        /// </summary>
        public static void DrawWireCircleXY(Vector3 position, float radius)
        {
            Draw.WireCircleXY(drawLine, position, radius);
        }

        /// <summary>
        /// Draws a wireframe circle with position, rotation and radius
        /// </summary>
        public static void DrawWireCircleXY(Vector3 position, Quaternion rotation, float radius)
        {
            Draw.WireCircleXY(drawLine, position, rotation, radius);
        }

        /// <summary>
        /// Draws a wireframe circle with position and radius
        /// </summary>
        public static void DrawWireCircleXZ(Vector3 position, float radius)
        {
            Draw.WireCircleXZ(drawLine, position, radius);
        }

        /// <summary>
        /// Draws a wireframe circle with position, rotation and radius
        /// </summary>
        public static void DrawWireCircleXZ(Vector3 position, Quaternion rotation, float radius)
        {
            Draw.WireCircleXZ(drawLine, position, rotation, radius);
        }

        /// <summary>
        /// Draws a wireframe circle with position and radius
        /// </summary>
        public static void DrawWireCircleYZ(Vector3 position, float radius)
        {
            Draw.WireCircleYZ(drawLine, position, radius);
        }

        /// <summary>
        /// Draws a wireframe circle with position, rotation and radius
        /// </summary>
        public static void DrawWireCircleYZ(Vector3 position, Quaternion rotation, float radius)
        {
            Draw.WireCircleYZ(drawLine, position, rotation, radius);
        }

        #endregion DrawWireCircle

        #region DrawWireArc

        /// <summary>
        /// Draws a wireframe circular arc with position and radius
        /// </summary>
        public static void DrawWireArcXY(Vector3 position, float radius, float fromAngle, float toAngle)
        {
            Draw.WireArcXY(drawLine, position, radius, fromAngle, toAngle);
        }

        /// <summary>
        /// Draws a wireframe circular arc with position, rotation and radius
        /// </summary>
        public static void DrawWireArcXY(Vector3 position, Quaternion rotation, float radius, float fromAngle, float toAngle)
        {
            Draw.WireArcXY(drawLine, position, rotation, radius, fromAngle, toAngle);
        }

        /// <summary>
        /// Draws a wireframe circular arc with position and radius
        /// </summary>
        public static void DrawWireArcXZ(Vector3 position, float radius, float fromAngle, float toAngle)
        {
            Draw.WireArcXZ(drawLine, position, radius, fromAngle, toAngle);
        }

        /// <summary>
        /// Draws a wireframe circular arc with position, rotation and radius
        /// </summary>
        public static void DrawWireArcXZ(Vector3 position, Quaternion rotation, float radius, float fromAngle, float toAngle)
        {
            Draw.WireArcXZ(drawLine, position, rotation, radius, fromAngle, toAngle);
        }

        /// <summary>
        /// Draws a wireframe circular arc with position and radius
        /// </summary>
        public static void DrawWireArcYZ(Vector3 position, float radius, float fromAngle, float toAngle)
        {
            Draw.WireArcYZ(drawLine, position, radius, fromAngle, toAngle);
        }

        /// <summary>
        /// Draws a wireframe circular arc with position, rotation and radius
        /// </summary>
        public static void DrawWireArcYZ(Vector3 position, Quaternion rotation, float radius, float fromAngle, float toAngle)
        {
            Draw.WireArcYZ(drawLine, position, rotation, radius, fromAngle, toAngle);
        }

        #endregion DrawWireArc

        /// <summary>
        /// Draws a wireframe sphere with position, rotation and radius
        /// </summary>
        public static void DrawWireSphere(Vector3 position, Quaternion rotation, float radius)
        {
            Draw.WireSphere(drawLine, position, rotation, radius);
        }

        /// <summary>
        /// Draws a wireframe hemisphere with position, rotation and radius
        /// </summary>
        public static void DrawWireHemisphere(Vector3 position, Quaternion rotation, float radius)
        {
            Draw.WireHemisphere(drawLine, position, rotation, radius);
        }

        /// <summary>
        /// Draws a wireframe cone with position and rotation
        /// </summary>
        public static void DrawWireCone(Vector3 position, Quaternion rotation, float apexRadius, float angle, float length)
        {
            Draw.WireCone(drawLine, position, rotation, apexRadius, angle, length);
        }
    }
}
