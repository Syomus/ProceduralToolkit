using System.Collections.Generic;
using UnityEngine;

namespace ProceduralToolkit
{
    public static class RandomE
    {
        public static Color color
        {
            get { return new Color(Random.value, Random.value, Random.value); }
        }

        public static Color colorHSV
        {
            get { return ColorE.HSVToRGB(Random.value, 1, 1); }
        }

        public static Gradient gradient
        {
            get { return ColorE.Gradient(color, color); }
        }

        public static Gradient gradientHSV
        {
            get { return ColorE.Gradient(colorHSV, colorHSV); }
        }

        public static string string8
        {
            get { return Datasets.alphanumerics.Choice(8); }
        }

        public static string string16
        {
            get { return Datasets.alphanumerics.Choice(16); }
        }

        public static T Choice<T>(this List<T> items)
        {
            if (items.Count == 0)
            {
                Debug.LogError("RandomE.Choice: items.Count == 0");
                return default(T);
            }
            return items[Random.Range(0, items.Count)];
        }

        public static T Choice<T>(this T[] items)
        {
            if (items.Length == 0)
            {
                Debug.LogError("RandomE.Choice: items.Length == 0");
                return default(T);
            }
            return items[Random.Range(0, items.Length)];
        }

        public static T Choice<T>(T item1, T item2, params T[] items)
        {
            return new List<T>(items) {item1, item2}.Choice();
        }

        public static T Choice<T>(this List<T> list, List<float> weights)
        {
            if (list.Count == 0)
            {
                Debug.LogError("RandomE.Choice: empty array");
                return default(T);
            }
            if (weights.Count == 0)
            {
                Debug.LogError("RandomE.Choice: empty weights");
                return default(T);
            }
            if (list.Count != weights.Count)
            {
                Debug.LogError("RandomE.Choice: array sizes must be equal");
                return list.Choice();
            }

            int index = 0;

            if (weights.Count > 1)
            {
                var cumulative = new List<float>(weights);
                for (int i = 1; i < cumulative.Count; i++)
                {
                    cumulative[i] += cumulative[i - 1];
                }

                float random = Random.Range(0, cumulative[cumulative.Count - 1]);
                index = cumulative.FindIndex(a => a >= random);
                if (index == -1)
                {
                    return list.Choice();
                }
            }
            return list[index];
        }

        public static string Choice(this string chars, int length)
        {
            var randomString = new System.Text.StringBuilder();
            for (int i = 0; i < length; i++)
            {
                randomString.Append(chars[Random.Range(0, chars.Length)]);
            }
            return randomString.ToString();
        }

        /// <summary>
        /// Fisher–Yates shuffle
        /// </summary>
        public static void Shuffle<T>(this T[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                int j = Random.Range(i, array.Length);
                T tmp = array[j];
                array[j] = array[i];
                array[i] = tmp;
            }
        }

        /// <summary>
        /// Fisher–Yates shuffle
        /// </summary>
        public static void Shuffle<T>(this List<T> array)
        {
            for (int i = 0; i < array.Count; i++)
            {
                int j = Random.Range(i, array.Count);
                T tmp = array[j];
                array[j] = array[i];
                array[i] = tmp;
            }
        }

        public static bool Chance(float percent)
        {
            return Random.value < percent;
        }
    }
}