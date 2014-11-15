using System.Collections.Generic;
using UnityEngine;

namespace ProceduralToolkit
{
    /// <summary>
    /// Useful utility methods
    /// </summary>
    public static class PTUtils
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

        public static Dictionary<T, int> Knapsack<T>(Dictionary<T, float> set, float remainder,
            Dictionary<T, int> knapsack = null, int startIndex = 0)
        {
            var keys = new List<T>(set.Keys);
            keys.Sort((a, b) => -set[a].CompareTo(set[b]));
            if (knapsack == null)
            {
                knapsack = new Dictionary<T, int>();
                foreach (var key in keys)
                {
                    knapsack[key] = 0;
                }
            }

            var smallestKey = keys[keys.Count - 1];
            if (remainder < set[smallestKey])
            {
                knapsack[smallestKey] = 1;
                return knapsack;
            }
            for (var i = startIndex; i < keys.Count; i++)
            {
                var key = keys[i];
                var value = set[key];
                knapsack[key] += (int) (remainder/value);
                remainder %= value;
            }
            if (remainder > 0)
            {
                for (var i = 0; i < keys.Count; i++)
                {
                    var key = keys[i];
                    if (knapsack[key] != 0)
                    {
                        if (key.Equals(smallestKey))
                        {
                            return knapsack;
                        }
                        knapsack[key]--;
                        remainder += set[key];
                        startIndex = i + 1;
                        break;
                    }
                }
                knapsack = Knapsack(set, remainder, knapsack, startIndex);
            }
            return knapsack;
        }
    }
}