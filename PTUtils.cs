using UnityEngine;

namespace ProceduralToolkit
{
    /// <summary>
    /// Useful utility methods
    /// </summary>
    public class PTUtils
    {
        /// <summary>
        /// Returns point on circle in the XY plane
        /// </summary>
        /// <param name="radius">Circle radius</param>
        /// <param name="angle">Angle in radians</param>
        public static Vector2 PointOnCircle2(float radius, float angle)
        {
            return new Vector2(radius*Mathf.Sin(angle), radius*Mathf.Cos(angle));
        }

        /// <summary>
        /// Returns point on circle in the XZ plane
        /// </summary>
        /// <param name="radius">Circle radius</param>
        /// <param name="angle">Angle in radians</param>
        public static Vector3 PointOnCircle3(float radius, float angle)
        {
            return new Vector3(radius*Mathf.Sin(angle), 0, radius*Mathf.Cos(angle));
        }

        /// <summary>
        /// Returns point on sphere in geographic coordinate system
        /// </summary>
        /// <param name="radius">Sphere radius</param>
        /// <param name="longitude">Longitude in radians</param>
        /// <param name="latitude">Latitude in radians</param>
        public static Vector3 PointOnSphere(float radius, float longitude, float latitude)
        {
            return new Vector3(radius*Mathf.Sin(longitude)*Mathf.Cos(latitude),
                radius*Mathf.Sin(latitude),
                radius*Mathf.Cos(longitude)*Mathf.Cos(latitude));
        }

        /// <summary>
        /// Returns perp of vector
        /// </summary>
        /// <remarks>
        /// Hill, F. S. Jr. "The Pleasures of 'Perp Dot' Products."
        /// Ch. II.5 in Graphics Gems IV (Ed. P. S. Heckbert). San Diego: Academic Press, pp. 138-148, 1994
        /// </remarks>
        public static Vector2 Perp(Vector2 vector)
        {
            return new Vector2(-vector.y, vector.x);
        }

        /// <summary>
        /// Returns perp dot product of vectors
        /// </summary>
        /// <remarks>
        /// Hill, F. S. Jr. "The Pleasures of 'Perp Dot' Products."
        /// Ch. II.5 in Graphics Gems IV (Ed. P. S. Heckbert). San Diego: Academic Press, pp. 138-148, 1994
        /// </remarks>
        public static float PerpDot(Vector2 a, Vector2 b)
        {
            return a.x*b.y - a.y*b.x;
        }

        /// <summary>
        /// Swaps values of <paramref name="left"/> and <paramref name="right"/>
        /// </summary>
        public static void Swap<T>(ref T left, ref T right)
        {
            T temp = left;
            left = right;
            right = temp;
        }
    }
}