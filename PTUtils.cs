using System;
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
        /// Returns list of points on circle in the XY plane
        /// </summary>
        /// <param name="radius">Circle radius</param>
        /// <param name="segments">Number of circle segments</param>
        public static List<Vector2> PointsOnCircle2(float radius, int segments)
        {
            float segmentAngle = Mathf.PI*2/segments;
            float currentAngle = 0f;
            var ring = new List<Vector2>(segments);
            for (var i = 0; i < segments; i++)
            {
                ring.Add(PointOnCircle2(radius, currentAngle));
                currentAngle -= segmentAngle;
            }
            return ring;
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
        /// Returns list of points on circle in the XZ plane
        /// </summary>
        /// <param name="radius">Circle radius</param>
        /// <param name="segments">Number of circle segments</param>
        public static List<Vector3> PointsOnCircle3(float radius, int segments)
        {
            float segmentAngle = Mathf.PI*2/segments;
            float currentAngle = 0f;
            var ring = new List<Vector3>(segments);
            for (var i = 0; i < segments; i++)
            {
                ring.Add(PointOnCircle3(radius, currentAngle));
                currentAngle -= segmentAngle;
            }
            return ring;
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
        /// Returns point on teardrop in geographic coordinate system
        /// </summary>
        /// <param name="radius">Teardrop radius</param>
        /// <param name="longitude">Longitude in radians</param>
        /// <param name="latitude">Latitude in radians</param>
        /// <param name="height">Teardrop height</param>
        public static Vector3 PointOnTeardrop(float radius, float longitude, float latitude, float height)
        {
            return new Vector3(radius*(0.5f*(1f-Mathf.Cos(latitude))*Mathf.Sin(latitude)*Mathf.Cos(longitude)),
                height*Mathf.Cos(latitude),
                radius*(0.5f*(1f-Mathf.Cos(latitude))*Mathf.Sin(latitude)*Mathf.Sin(longitude)));
        }

        /// <summary>
        /// Draws aliased line and calls <paramref name="draw"/> on every point in line
        /// </summary>
        /// <remarks>
        /// https://en.wikipedia.org/wiki/Bresenham%27s_line_algorithm
        /// </remarks>
        public static void DrawLine(Vector2Int v0, Vector2Int v1, Action<int, int> draw)
        {
            DrawLine(v0.x, v0.y, v1.x, v1.y, draw);
        }

        /// <summary>
        /// Draws aliased line and calls <paramref name="draw"/> on every point in line
        /// </summary>
        /// <remarks>
        /// https://en.wikipedia.org/wiki/Bresenham%27s_line_algorithm
        /// </remarks>
        public static void DrawLine(int x0, int y0, int x1, int y1, Action<int, int> draw)
        {
            bool steep = Math.Abs(y1 - y0) > Math.Abs(x1 - x0);
            if (steep)
            {
                Swap(ref x0, ref y0);
                Swap(ref x1, ref y1);
            }
            if (x0 > x1)
            {
                Swap(ref x0, ref x1);
                Swap(ref y0, ref y1);
            }

            int dx = x1 - x0;
            int dy = Math.Abs(y1 - y0);
            int error = dx/2;
            int ystep = (y0 < y1) ? 1 : -1;
            int y = y0;
            for (int x = x0; x <= x1; x++)
            {
                draw(steep ? y : x, steep ? x : y);
                error -= dy;
                if (error < 0)
                {
                    y += ystep;
                    error += dx;
                }
            }
        }

        /// <summary>
        /// Draws aliased circle and calls <paramref name="draw"/> on every point in line
        /// </summary>
        /// <remarks>
        /// A Rasterizing Algorithm for Drawing Curves
        /// http://members.chello.at/easyfilter/bresenham.pdf
        /// </remarks>
        public static void DrawCircle(Vector2Int v0, int radius, Action<int, int> draw)
        {
            DrawCircle(v0.x, v0.y, radius, draw);
        }

        /// <summary>
        /// Draws aliased circle and calls <paramref name="draw"/> on every point in line
        /// </summary>
        /// <remarks>
        /// A Rasterizing Algorithm for Drawing Curves
        /// http://members.chello.at/easyfilter/bresenham.pdf
        /// </remarks>
        public static void DrawCircle(int x0, int y0, int radius, Action<int, int> draw)
        {
            int x = -radius;
            int y = 0;
            int error = 2 - 2*radius; // 2 quadrant ◴
            while (x < 0)
            {
                draw(x0 - x, y0 + y); // 1 quadrant ◷
                draw(x0 - y, y0 - x); // 2 quadrant ◴
                draw(x0 + x, y0 - y); // 3 quadrant ◵
                draw(x0 + y, y0 + x); // 4 quadrant ◶

                int lastError = error;
                if (y >= error)
                {
                    y++;
                    error += 2*y + 1;
                }

                // Second check is needed to avoid weird pixels at diagonals at some radiuses
                // Example radiuses: 4, 11, 134, 373, 4552
                if (x < lastError || y < error)
                {
                    x++;
                    error += 2*x + 1;
                }
            }
        }

        /// <summary>
        /// Draws filled aliased circle and calls <paramref name="draw"/> on every point in line
        /// </summary>
        public static void DrawFilledCircle(Vector2Int v0, int radius, Action<int, int> draw)
        {
            DrawFilledCircle(v0.x, v0.y, radius, draw);
        }

        /// <summary>
        /// Draws filled aliased circle and calls <paramref name="draw"/> on every point in line
        /// </summary>
        public static void DrawFilledCircle(int x0, int y0, int radius, Action<int, int> draw)
        {
            int x = -radius;
            int y = 0;
            int error = 2 - 2*radius; // 2 quadrant ◴
            // lastY must have a different value than y
            int lastY = int.MaxValue;
            while (x < 0)
            {
                // This check prevents overdraw at poles
                if (lastY != y)
                {
                    DrawHorizontalLine(x0 + x, x0 - x, y0 + y, draw); // ◠
                    // This check prevents overdraw at central horizontal
                    if (y != 0)
                    {
                        DrawHorizontalLine(x0 + x, x0 - x, y0 - y, draw); // ◡
                    }
                }
                lastY = y;

                int lastError = error;
                if (y >= error)
                {
                    y++;
                    error += 2*y + 1;
                }

                // Second check is needed to avoid weird pixels at diagonals at some radiuses
                // Example radiuses: 4, 11, 134, 373, 4552
                if (x < lastError || y < error)
                {
                    x++;
                    error += 2*x + 1;
                }
            }
        }

        private static void DrawHorizontalLine(int fromX, int toX, int y, Action<int, int> draw)
        {
            for (int x = fromX; x <= toX; x++)
            {
                draw(x, y);
            }
        }

        /// <summary>
        /// Draws anti-aliased line and calls <paramref name="draw"/> on every point in line
        /// </summary>
        /// <remarks>
        /// https://en.wikipedia.org/wiki/Xiaolin_Wu%27s_line_algorithm
        /// </remarks>
        public static void DrawAALine(Vector2Int v0, Vector2Int v1, Action<int, int, float> draw)
        {
            DrawAALine(v0.x, v0.y, v1.x, v1.y, draw);
        }

        /// <summary>
        /// Draws anti-aliased line and calls <paramref name="draw"/> on every point in line
        /// </summary>
        /// <remarks>
        /// https://en.wikipedia.org/wiki/Xiaolin_Wu%27s_line_algorithm
        /// </remarks>
        public static void DrawAALine(int x0, int y0, int x1, int y1, Action<int, int, float> draw)
        {
            bool steep = Math.Abs(y1 - y0) > Math.Abs(x1 - x0);
            if (steep)
            {
                Swap(ref x0, ref y0);
                Swap(ref x1, ref y1);
            }
            if (x0 > x1)
            {
                Swap(ref x0, ref x1);
                Swap(ref y0, ref y1);
            }

            if (steep)
            {
                draw(y0, x0, 1);
                draw(y1, x1, 1);
            }
            else
            {
                draw(x0, y0, 1);
                draw(x1, y1, 1);
            }
            float dx = x1 - x0;
            float dy = y1 - y0;
            float gradient = dy/dx;
            float y = y0 + gradient;
            for (var x = x0 + 1; x <= x1 - 1; x++)
            {
                if (steep)
                {
                    draw((int) y, x, 1 - (y - (int) y));
                    draw((int) y + 1, x, y - (int) y);
                }
                else
                {
                    draw(x, (int) y, 1 - (y - (int) y));
                    draw(x, (int) y + 1, y - (int) y);
                }
                y += gradient;
            }
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
        /// Returns perp of vector
        /// </summary>
        /// <remarks>
        /// Hill, F. S. Jr. "The Pleasures of 'Perp Dot' Products."
        /// Ch. II.5 in Graphics Gems IV (Ed. P. S. Heckbert). San Diego: Academic Press, pp. 138-148, 1994
        /// </remarks>
        public static Vector2Int Perp(Vector2Int vector)
        {
            return new Vector2Int(-vector.y, vector.x);
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
        /// Returns perp dot product of vectors
        /// </summary>
        /// <remarks>
        /// Hill, F. S. Jr. "The Pleasures of 'Perp Dot' Products."
        /// Ch. II.5 in Graphics Gems IV (Ed. P. S. Heckbert). San Diego: Academic Press, pp. 138-148, 1994
        /// </remarks>
        public static int PerpDot(Vector2Int a, Vector2Int b)
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

        /// <summary>
        /// Knapsack problem solver for items with equal value
        /// </summary>
        /// <typeparam name="T">Item identificator</typeparam>
        /// <param name="set">
        /// Set of items where key <typeparamref name="T"/> is item identificator and value is item weight</param>
        /// <param name="capacity">Maximum weight</param>
        /// <param name="knapsack">Pre-filled knapsack</param>
        /// <returns>
        /// Filled knapsack where values are number of items of type key.
        /// Tends to overload knapsack: fills remainder with one smallest item.</returns>
        /// <remarks>
        /// https://en.wikipedia.org/wiki/Knapsack_problem
        /// </remarks>
        public static Dictionary<T, int> Knapsack<T>(Dictionary<T, float> set, float capacity,
            Dictionary<T, int> knapsack = null)
        {
            var keys = new List<T>(set.Keys);
            // Sort keys by their weights in descending order
            keys.Sort((a, b) => -set[a].CompareTo(set[b]));

            if (knapsack == null)
            {
                knapsack = new Dictionary<T, int>();
                foreach (var key in keys)
                {
                    knapsack[key] = 0;
                }
            }
            return Knapsack(set, keys, capacity, knapsack, 0);
        }

        private static Dictionary<T, int> Knapsack<T>(Dictionary<T, float> set, List<T> keys, float remainder,
            Dictionary<T, int> knapsack, int startIndex)
        {
            T smallestKey = keys[keys.Count - 1];
            if (remainder < set[smallestKey])
            {
                knapsack[smallestKey] = 1;
                return knapsack;
            }
            // Cycle through items and try to put them in knapsack
            for (var i = startIndex; i < keys.Count; i++)
            {
                T key = keys[i];
                float weight = set[key];
                // Larger items won't fit, smaller items will fill as much space as they can
                knapsack[key] += (int) (remainder/weight);
                remainder %= weight;
            }
            if (remainder > 0)
            {
                // Throw out largest item and try again
                for (var i = 0; i < keys.Count; i++)
                {
                    T key = keys[i];
                    if (knapsack[key] != 0)
                    {
                        // Already tried every combination, return as is
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
                knapsack = Knapsack(set, keys, remainder, knapsack, startIndex);
            }
            return knapsack;
        }
    }
}