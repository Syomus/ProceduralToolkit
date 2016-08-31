using UnityEngine;
using System.Collections.Generic;
using System.Text;

namespace ProceduralToolkit
{
    /// <summary>
    /// Class for generating random data. Contains extensions for arrays and other classes.
    /// </summary>
    public static class RandomE
    {
        #region Vectors

        /// <summary>
        /// Returns a random point on a circle with radius 1
        /// </summary>
        public static Vector2 onUnitCircle2 { get { return PTUtils.PointOnCircle2(1, Random.Range(0, 360f)); } }

        /// <summary>
        /// Returns a random point on a circle with radius 1
        /// </summary>
        public static Vector2 onUnitCircle3XY { get { return PTUtils.PointOnCircle3XY(1, Random.Range(0, 360f)); } }

        /// <summary>
        /// Returns a random point on a circle with radius 1
        /// </summary>
        public static Vector2 onUnitCircle3XZ { get { return PTUtils.PointOnCircle3XZ(1, Random.Range(0, 360f)); } }

        /// <summary>
        /// Returns a random point on a circle with radius 1
        /// </summary>
        public static Vector2 onUnitCircle3YZ { get { return PTUtils.PointOnCircle3YZ(1, Random.Range(0, 360f)); } }

        /// <summary>
        /// Returns a random point inside a square with side lengths 1
        /// </summary>
        public static Vector2 insideUnitSquare { get { return new Vector2(Random.value, Random.value); } }

        /// <summary>
        /// Returns a random point on a perimeter of square with side lengths 1
        /// </summary>
        public static Vector2 onUnitSquare { get { return PointOnSquare(1, 1); } }

        /// <summary>
        /// Returns a random point inside a cube with side lengths 1
        /// </summary>
        public static Vector3 insideUnitCube { get { return new Vector3(Random.value, Random.value, Random.value); } }

        #endregion Vectors

        #region Colors

        /// <summary>
        /// Returns a random color between black [inclusive] and white [inclusive]
        /// </summary>
        public static Color color { get { return new Color(Random.value, Random.value, Random.value); } }

        /// <summary>
        /// Returns a color with random hue and maximum saturation and value in HSV model
        /// </summary>
        public static ColorHSV colorHSV { get { return new ColorHSV(Random.value, 1, 1); } }

        /// <summary>
        /// Returns a gradient between two random colors
        /// </summary>
        public static Gradient gradient { get { return ColorE.Gradient(color, color); } }

        /// <summary>
        /// Returns a gradient between two random HSV colors
        /// </summary>
        public static Gradient gradientHSV { get { return ColorE.Gradient(colorHSV, colorHSV); } }

        /// <summary>
        /// Returns a color with random hue and given <paramref name="saturation"/> and <paramref name="value"/>
        /// </summary>
        public static ColorHSV ColorHue(float saturation, float value, float alpha = 1)
        {
            return new ColorHSV(Random.value, saturation, value, alpha);
        }

        /// <summary>
        /// Returns a color with random saturation and given <paramref name="hue"/> and <paramref name="value"/>
        /// </summary>
        public static ColorHSV ColorSaturation(float hue, float value, float alpha = 1)
        {
            return new ColorHSV(hue, Random.value, value, alpha);
        }

        /// <summary>
        /// Returns a color with random value and given <paramref name="hue"/> and <paramref name="saturation"/>
        /// </summary>
        public static ColorHSV ColorValue(float hue, float saturation, float alpha = 1)
        {
            return new ColorHSV(hue, saturation, Random.value, alpha);
        }

        /// <summary>
        /// Returns a analogous palette based on a color with random hue
        /// </summary>
        public static List<ColorHSV> AnalogousPalette(
            float saturation = 1,
            float value = 1,
            float alpha = 1,
            int count = 2,
            bool withComplementary = false)
        {
            return ColorHue(saturation, value, alpha).GetAnalogousPalette(count, withComplementary);
        }

        /// <summary>
        /// Returns a triadic palette based on a color with random hue
        /// </summary>
        public static List<ColorHSV> TriadicPalette(
            float saturation = 1,
            float value = 1,
            float alpha = 1,
            bool withComplementary = false)
        {
            return ColorHue(saturation, value, alpha).GetTriadicPalette(withComplementary);
        }

        /// <summary>
        /// Returns a tetradic palette based on a color with random hue
        /// </summary>
        public static List<ColorHSV> TetradicPalette(float saturation = 1, float value = 1, float alpha = 1)
        {
            return ColorHue(saturation, value, alpha).GetTetradicPalette();
        }

        #endregion Colors

        #region Strings

        /// <summary>
        /// Returns a random alphanumeric 8-character string
        /// </summary>
        public static string string8 { get { return Datasets.alphanumerics.GetRandom(8); } }

        /// <summary>
        /// Returns a random alphanumeric 16-character string
        /// </summary>
        public static string string16 { get { return Datasets.alphanumerics.GetRandom(16); } }

        /// <summary>
        /// Returns a random lowercase letter
        /// </summary>
        public static char lowercaseLetter { get { return Datasets.lowercase.GetRandom(); } }

        /// <summary>
        /// Returns a random uppercase letter
        /// </summary>
        public static char uppercaseLetter { get { return Datasets.uppercase.GetRandom(); } }

        /// <summary>
        /// Returns a random first name
        /// </summary>
        public static string name { get { return Chance(0.5f) ? femaleName : maleName; } }

        /// <summary>
        /// Returns a random female name
        /// </summary>
        public static string femaleName { get { return Datasets.femaleNames.GetRandom(); } }

        /// <summary>
        /// Returns a random male name
        /// </summary>
        public static string maleName { get { return Datasets.maleNames.GetRandom(); } }

        /// <summary>
        /// Returns a random last name
        /// </summary>
        public static string lastName { get { return Datasets.lastNames.GetRandom(); } }

        /// <summary>
        /// Returns a random full name in format "[First name] [Last name]"
        /// </summary>
        public static string fullName { get { return string.Format("{0} {1}", name, lastName); } }

        #endregion Strings

        /// <summary>
        /// Returns a random element
        /// </summary>
        public static T GetRandom<T>(this List<T> items)
        {
            if (items.Count == 0)
            {
                Debug.LogError("Empty array");
                return default(T);
            }
            return items[Random.Range(0, items.Count)];
        }

        /// <summary>
        /// Returns a random element
        /// </summary>
        public static T GetRandom<T>(this T[] items)
        {
            if (items.Length == 0)
            {
                Debug.LogError("Empty array");
                return default(T);
            }
            return items[Random.Range(0, items.Length)];
        }

        /// <summary>
        /// Returns a random element
        /// </summary>
        public static T GetRandom<T>(T item1, T item2, params T[] items)
        {
            return new List<T>(items) {item1, item2}.GetRandom();
        }

        /// <summary>
        /// Returns a random value from dictionary
        /// </summary>
        public static TValue GetRandom<TKey, TValue>(this Dictionary<TKey, TValue> dictionary)
        {
            var keys = dictionary.Keys;
            if (keys.Count == 0)
            {
                Debug.LogError("Empty dictionary");
                return default(TValue);
            }
            return dictionary[new List<TKey>(keys).GetRandom()];
        }

        /// <summary>
        /// Returns a random element with chances for roll of each element based on <paramref name="weights"/>
        /// </summary>
        /// <param name="weights">Positive floats representing chances</param>
        public static T GetRandom<T>(this List<T> list, List<float> weights)
        {
            if (list.Count == 0)
            {
                Debug.LogError("Empty array");
                return default(T);
            }
            if (weights.Count == 0)
            {
                Debug.LogError("Empty weights");
                return default(T);
            }
            if (list.Count != weights.Count)
            {
                Debug.LogError("Array sizes must be equal");
                return list.GetRandom();
            }

            if (list.Count == 1)
            {
                return list[0];
            }

            var cumulative = new List<float>(weights);
            for (int i = 1; i < cumulative.Count; i++)
            {
                cumulative[i] += cumulative[i - 1];
            }

            float random = Random.Range(0, cumulative[cumulative.Count - 1]);
            int index = cumulative.FindIndex(a => a >= random);
            if (index == -1)
            {
                Debug.LogError("Invalid weights");
                return list.GetRandom();
            }
            return list[index];
        }

        /// <summary>
        /// Returns a random character from string
        /// </summary>
        public static char GetRandom(this string chars)
        {
            if (string.IsNullOrEmpty(chars))
            {
                Debug.LogError("Empty string");
                return default(char);
            }
            return chars[Random.Range(0, chars.Length)];
        }

        /// <summary>
        /// Returns a random string consisting of characters from that string
        /// </summary>
        public static string GetRandom(this string chars, int length)
        {
            if (string.IsNullOrEmpty(chars))
            {
                Debug.LogError("Empty string");
                return default(string);
            }
            var randomString = new StringBuilder(length);
            for (int i = 0; i < length; i++)
            {
                randomString.Append(chars[Random.Range(0, chars.Length)]);
            }
            return randomString.ToString();
        }

        /// <summary>
        /// Returns a random element and removes it from list
        /// </summary>
        public static T PopRandom<T>(this List<T> items)
        {
            if (items.Count == 0)
            {
                Debug.LogError("Empty array");
                return default(T);
            }
            var index = Random.Range(0, items.Count);
            var item = items[index];
            items.RemoveAt(index);
            return item;
        }

        /// <summary>
        /// Fisher–Yates shuffle
        /// </summary>
        /// <remarks>
        /// https://en.wikipedia.org/wiki/Fisher–Yates_shuffle
        /// </remarks>
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
        /// <remarks>
        /// https://en.wikipedia.org/wiki/Fisher–Yates_shuffle
        /// </remarks>
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

        /// <summary>
        /// Returns true with probability from <paramref name="percent"/>
        /// </summary>
        /// <param name="percent">between 0.0 [inclusive] and 1.0 [inclusive]</param>
        public static bool Chance(float percent)
        {
            return Random.value < percent;
        }

        /// <summary>
        /// Returns a random point on a perimeter of square
        /// </summary>
        public static Vector2 PointOnSquare(float a, float b)
        {
            float value = Random.value*(2*a + 2*b);
            if (value < a)
            {
                return new Vector2(value, 0);
            }
            value -= a;
            if (value < b)
            {
                return new Vector2(a, value);
            }
            value -= b;
            if (value < a)
            {
                return new Vector2(value, b);
            }
            return new Vector2(0, value - a);
        }

        /// <summary>
        /// Returns a random vector between <paramref name="min"/> [inclusive] and <paramref name="max"/> [inclusive]
        /// </summary>
        public static Vector2 Range(Vector2 min, Vector2 max)
        {
            return new Vector2(Random.Range(min.x, max.x), Random.Range(min.y, max.y));
        }

        /// <summary>
        /// Returns a random vector between <paramref name="min"/> [inclusive] and <paramref name="max"/> [inclusive]
        /// </summary>
        public static Vector3 Range(Vector3 min, Vector3 max)
        {
            return new Vector3(Random.Range(min.x, max.x), Random.Range(min.y, max.y), Random.Range(min.z, max.z));
        }

        /// <summary>
        /// Returns a random vector between <paramref name="min"/> [inclusive] and <paramref name="max"/> [inclusive]
        /// </summary>
        public static Vector4 Range(Vector4 min, Vector4 max)
        {
            return new Vector4(Random.Range(min.x, max.x), Random.Range(min.y, max.y), Random.Range(min.z, max.z),
                Random.Range(min.w, max.w));
        }

        /// <summary>
        /// Returns a random float number between and <paramref name="min"/> [inclusive] and <paramref name="max"/> [inclusive].
        /// Ensures that there will be only specified amount of variants.
        /// </summary>
        public static float Range(float min, float max, int variants)
        {
            if (variants < 2)
            {
                Debug.LogError("Variants must be greater than one");
                variants = 2;
            }
            return Mathf.Lerp(min, max, Random.Range(0, variants)/(variants - 1f));
        }

        /// <summary>
        /// Returns a random vector between and <paramref name="min"/> [inclusive] and <paramref name="max"/> [inclusive].
        /// Ensures that there will be only specified amount of variants.
        /// </summary>
        public static Vector2 Range(Vector2 min, Vector2 max, int variants)
        {
            return new Vector2(Range(min.x, max.x, variants), Range(min.y, max.y, variants));
        }

        /// <summary>
        /// Returns a random vector between and <paramref name="min"/> [inclusive] and <paramref name="max"/> [inclusive].
        /// Ensures that there will be only specified amount of variants.
        /// </summary>
        public static Vector3 Range(Vector3 min, Vector3 max, int variants)
        {
            return new Vector3(Range(min.x, max.x, variants), Range(min.y, max.y, variants),
                Range(min.z, max.z, variants));
        }

        /// <summary>
        /// Returns a random vector between and <paramref name="min"/> [inclusive] and <paramref name="max"/> [inclusive].
        /// Ensures that there will be only specified amount of variants.
        /// </summary>
        public static Vector4 Range(Vector4 min, Vector4 max, int variants)
        {
            return new Vector4(Range(min.x, max.x, variants), Range(min.y, max.y, variants),
                Range(min.z, max.z, variants), Range(min.w, max.w, variants));
        }
    }
}