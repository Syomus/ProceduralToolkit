using ProceduralToolkit.FastNoiseLib;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ProceduralToolkit
{
    /// <summary>
    /// Various useful methods and constants
    /// </summary>
    public static class PTUtils
    {
        /// <summary>
        /// Lowercase letters from a to z
        /// </summary>
        public const string LowercaseLetters = "abcdefghijklmnopqrstuvwxyz";
        /// <summary>
        /// Uppercase letters from A to Z
        /// </summary>
        public const string UppercaseLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        /// <summary>
        /// Digits from 0 to 9
        /// </summary>
        public const string Digits = "0123456789";
        /// <summary>
        /// The concatenation of the strings <see cref="LowercaseLetters"/> and <see cref="UppercaseLetters"/>
        /// </summary>
        public const string Letters = LowercaseLetters + UppercaseLetters;
        /// <summary>
        /// The concatenation of the strings <see cref="Letters"/> and <see cref="Digits"/>
        /// </summary>
        public const string Alphanumerics = Letters + Digits;

        /// <summary>
        /// Square root of 0.5
        /// </summary>
        public const float Sqrt05 = 0.7071067811865475244f;
        /// <summary>
        /// Square root of 2
        /// </summary>
        public const float Sqrt2 = 1.4142135623730950488f;
        /// <summary>
        /// Square root of 5
        /// </summary>
        public const float Sqrt5 = 2.2360679774997896964f;
        /// <summary>
        /// Golden angle in radians
        /// </summary>
        public const float GoldenAngle = Mathf.PI*(3 - Sqrt5);

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

        public static string ToString(this Vector3 vector, string format, IFormatProvider formatProvider)
        {
            return string.Format("({0}, {1}, {2})", vector.x.ToString(format, formatProvider), vector.y.ToString(format, formatProvider),
                vector.z.ToString(format, formatProvider));
        }

        public static string ToString(this Quaternion quaternion, string format, IFormatProvider formatProvider)
        {
            return string.Format("({0}, {1}, {2}, {3})", quaternion.x.ToString(format, formatProvider), quaternion.y.ToString(format, formatProvider),
                quaternion.z.ToString(format, formatProvider), quaternion.w.ToString(format, formatProvider));
        }

        public static void ApplyProperties(this Renderer renderer, RendererProperties properties)
        {
            renderer.lightProbeUsage = properties.lightProbeUsage;
            renderer.lightProbeProxyVolumeOverride = properties.lightProbeProxyVolumeOverride;
            renderer.reflectionProbeUsage = properties.reflectionProbeUsage;
            renderer.probeAnchor = properties.probeAnchor;
            renderer.shadowCastingMode = properties.shadowCastingMode;
            renderer.receiveShadows = properties.receiveShadows;
            renderer.motionVectorGenerationMode = properties.motionVectorGenerationMode;
        }

        public static MeshRenderer CreateMeshRenderer(string name, out MeshFilter meshFilter)
        {
            var gameObject = new GameObject(name);
            meshFilter = gameObject.AddComponent<MeshFilter>();
            var meshRenderer = gameObject.AddComponent<MeshRenderer>();
            return meshRenderer;
        }

        public static Texture2D CreateTexture(int width, int height, Color clearColor)
        {
            var texture = new Texture2D(width, height, TextureFormat.ARGB32, false, true)
            {
                filterMode = FilterMode.Point
            };
            texture.Clear(clearColor);
            texture.Apply();
            return texture;
        }

        /// <summary>
        /// Returns a noise value between 0.0 and 1.0
        /// </summary>
        public static float GetNoise01(this FastNoise noise, float x, float y)
        {
            return Mathf.Clamp01(noise.GetNoise(x, y)*0.5f + 0.5f);
        }
    }
}
