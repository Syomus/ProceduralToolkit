﻿using UnityEngine;

namespace ProceduralToolkit
{
    public static class VectorE
    {
        private const float epsilon = 0.00001f;

        #region Vector2

        public static Vector2 OnlyX(this Vector2 vector)
        {
            return new Vector2(vector.x, 0);
        }

        public static Vector2 OnlyY(this Vector2 vector)
        {
            return new Vector2(0, vector.y);
        }

        public static Vector3 ToVector3XY(this Vector2 vector)
        {
            return new Vector3(vector.x, vector.y, 0);
        }

        public static Vector3 ToVector3XZ(this Vector2 vector)
        {
            return new Vector3(vector.x, 0, vector.y);
        }

        public static Vector3 ToVector3YZ(this Vector2 vector)
        {
            return new Vector3(0, vector.x, vector.y);
        }

        /// <summary>
        /// Returns true if vectors lie on the same line, false otherwise
        /// </summary>
        public static bool IsCollinear(this Vector2 vector, Vector2 other)
        {
            return Mathf.Abs(PTUtils.PerpDot(vector, other)) < epsilon;
        }

        public static Vector2 RotateCW(this Vector2 vector, float degrees)
        {
            float radians = degrees*Mathf.Deg2Rad;
            float sin = Mathf.Sin(radians);
            float cos = Mathf.Cos(radians);
            return new Vector2(
                vector.x*cos + vector.y*sin,
                -vector.x*sin + vector.y*cos);
        }

        #endregion Vector2

        #region Vector3

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

        #endregion Vector3
    }
}