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
        public const string lowercase = "abcdefghijklmnopqrstuvwxyz";

        /// <summary>
        /// Uppercase letters from A to Z
        /// </summary>
        public const string uppercase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        /// <summary>
        /// Digits from 0 to 9
        /// </summary>
        public const string digits = "0123456789";

        /// <summary>
        /// The concatenation of the strings <see cref="lowercase"/> and <see cref="uppercase"/>
        /// </summary>
        public const string letters = lowercase + uppercase;

        /// <summary>
        /// The concatenation of the strings <see cref="letters"/> and <see cref="digits"/>
        /// </summary>
        public const string alphanumerics = letters + digits;

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
