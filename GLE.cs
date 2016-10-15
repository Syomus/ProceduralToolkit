﻿using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace ProceduralToolkit
{
    public static class GLE
    {
        public static readonly Material wireMaterial;

        private static readonly Action<Vector3, Vector3> drawLine;

        static GLE()
        {
            var shader = Shader.Find("Hidden/Internal-Colored");
            wireMaterial = new Material(shader) {hideFlags = HideFlags.HideAndDontSave};
            wireMaterial.SetInt("_SrcBlend", (int) BlendMode.SrcAlpha);
            wireMaterial.SetInt("_DstBlend", (int) BlendMode.OneMinusSrcAlpha);
            wireMaterial.SetInt("_Cull", (int) CullMode.Off);
            wireMaterial.SetInt("_ZWrite", 0);

            drawLine = DrawLine;
        }

        public static void DrawLine(Vector3 from, Vector3 to)
        {
            GL.Vertex(from);
            GL.Vertex(to);
        }

        public static void DrawRay(Ray ray)
        {
            Draw.WireRay(drawLine, ray);
        }

        public static void DrawWireQuadXY(Vector3 position, Quaternion rotation, Vector2 scale)
        {
            Draw.WireQuadXY(drawLine, position, rotation, scale);
        }

        public static void DrawWireQuadXZ(Vector3 position, Quaternion rotation, Vector2 scale)
        {
            Draw.WireQuadXZ(drawLine, position, rotation, scale);
        }

        public static void DrawWireQuadYZ(Vector3 position, Quaternion rotation, Vector2 scale)
        {
            Draw.WireQuadYZ(drawLine, position, rotation, scale);
        }

        public static void DrawWireCube(Vector3 position, Quaternion rotation, Vector3 scale)
        {
            Draw.WireCube(drawLine, position, rotation, scale);
        }

        #region DrawWireCircle

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

        #endregion DrawWireCircle

        #region DrawWireArc

        public static void DrawWireArcXY(Vector3 position, float radius, float fromAngle, float toAngle)
        {
            Draw.WireArcXY(drawLine, position, radius, fromAngle, toAngle);
        }

        public static void DrawWireArcXY(Vector3 position, Quaternion rotation, float radius, float fromAngle,
            float toAngle)
        {
            Draw.WireArcXY(drawLine, position, rotation, radius, fromAngle, toAngle);
        }

        public static void DrawWireArcXZ(Vector3 position, float radius, float fromAngle, float toAngle)
        {
            Draw.WireArcXZ(drawLine, position, radius, fromAngle, toAngle);
        }

        public static void DrawWireArcXZ(Vector3 position, Quaternion rotation, float radius, float fromAngle,
            float toAngle)
        {
            Draw.WireArcXZ(drawLine, position, rotation, radius, fromAngle, toAngle);
        }

        public static void DrawWireArcYZ(Vector3 position, float radius, float fromAngle, float toAngle)
        {
            Draw.WireArcYZ(drawLine, position, radius, fromAngle, toAngle);
        }

        public static void DrawWireArcYZ(Vector3 position, Quaternion rotation, float radius, float fromAngle,
            float toAngle)
        {
            Draw.WireArcYZ(drawLine, position, rotation, radius, fromAngle, toAngle);
        }

        #endregion DrawWireArc

        public static void DrawWireSphere(Vector3 position, Quaternion rotation, float radius)
        {
            Draw.WireSphere(drawLine, position, rotation, radius);
        }

        public static void DrawWireHemisphere(Vector3 position, Quaternion rotation, float radius)
        {
            Draw.WireHemisphere(drawLine, position, rotation, radius);
        }

        public static void DrawWireCone(Vector3 position, Quaternion rotation, float radius, float angle, float length)
        {
            Draw.WireCone(drawLine, position, rotation, radius, angle, length);
        }
    }
}