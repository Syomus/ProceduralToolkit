using System;
using UnityEngine;
using System.Collections.Generic;
using System.Text;
using Random = UnityEngine.Random;

namespace ProceduralToolkit
{
    /// <summary>
    /// Class for generating random data. Contains extensions for arrays and other collections.
    /// </summary>
    public static class RandomE
    {
        /// <summary>
        /// Returns a random 2D rotation
        /// </summary>
        public static float rotation2 { get { return Random.Range(0, 360f); } }

        /// <summary>
        /// Returns a random rotation around X axis
        /// </summary>
        public static Quaternion xRotation { get { return Quaternion.Euler(rotation2, 0, 0); } }

        /// <summary>
        /// Returns a random rotation around Y axis
        /// </summary>
        public static Quaternion yRotation { get { return Quaternion.Euler(0, rotation2, 0); } }

        /// <summary>
        /// Returns a random rotation around Z axis
        /// </summary>
        public static Quaternion zRotation { get { return Quaternion.Euler(0, 0, rotation2); } }

        #region Geometry

        /// <summary>
        /// Returns a random point on a circle with radius 1
        /// </summary>
        public static Vector2 onUnitCircle2 { get { return Geometry.PointOnCircle2(1, rotation2); } }

        /// <summary>
        /// Returns a random point inside a circle with radius 1
        /// </summary>
        public static Vector3 insideUnitCircle3XY
        {
            get { return Geometry.PointOnCircle3XY(Random.value, rotation2); }
        }

        /// <summary>
        /// Returns a random point inside a circle with radius 1
        /// </summary>
        public static Vector3 insideUnitCircle3XZ
        {
            get { return Geometry.PointOnCircle3XZ(Random.value, rotation2); }
        }

        /// <summary>
        /// Returns a random point inside a circle with radius 1
        /// </summary>
        public static Vector3 insideUnitCircle3YZ
        {
            get { return Geometry.PointOnCircle3YZ(Random.value, rotation2); }
        }

        /// <summary>
        /// Returns a random point on a circle with radius 1
        /// </summary>
        public static Vector3 onUnitCircle3XY { get { return Geometry.PointOnCircle3XY(1, rotation2); } }

        /// <summary>
        /// Returns a random point on a circle with radius 1
        /// </summary>
        public static Vector3 onUnitCircle3XZ { get { return Geometry.PointOnCircle3XZ(1, rotation2); } }

        /// <summary>
        /// Returns a random point on a circle with radius 1
        /// </summary>
        public static Vector3 onUnitCircle3YZ { get { return Geometry.PointOnCircle3YZ(1, rotation2); } }

        /// <summary>
        /// Returns a random point inside a unit square
        /// </summary>
        public static Vector2 insideUnitSquare
        {
            get { return Range(new Vector2(-0.5f, -0.5f), new Vector2(0.5f, 0.5f)); }
        }

        /// <summary>
        /// Returns a random point on the perimeter of a unit square
        /// </summary>
        public static Vector2 onUnitSquare { get { return PointOnRect(new Rect(-0.5f, -0.5f, 1, 1)); } }

        /// <summary>
        /// Returns a random point inside a unit cube
        /// </summary>
        public static Vector3 insideUnitCube
        {
            get { return Range(new Vector3(-0.5f, -0.5f, -0.5f), new Vector3(0.5f, 0.5f, 0.5f)); }
        }

        /// <summary>
        /// Returns a random point on a segment
        /// </summary>
        public static Vector2 PointOnSegment(Segment2 segment)
        {
            return PointOnSegment(segment.a, segment.b);
        }

        /// <summary>
        /// Returns a random point on a segment
        /// </summary>
        public static Vector2 PointOnSegment(Vector2 segmentA, Vector2 segmentB)
        {
            return segmentA + (segmentB - segmentA)*Random.value;
        }

        /// <summary>
        /// Returns a random point on a segment
        /// </summary>
        public static Vector3 PointOnSegment(Segment3 segment)
        {
            return PointOnSegment(segment.a, segment.b);
        }

        /// <summary>
        /// Returns a random point on a segment
        /// </summary>
        public static Vector3 PointOnSegment(Vector3 segmentA, Vector3 segmentB)
        {
            return segmentA + (segmentB - segmentA)*Random.value;
        }

        /// <summary>
        /// Returns a random point on a circle
        /// </summary>
        public static Vector2 PointOnCircle(Circle2 circle)
        {
            return PointOnCircle(circle.center, circle.radius);
        }

        /// <summary>
        /// Returns a random point on a circle
        /// </summary>
        public static Vector2 PointOnCircle(Vector2 center, float radius)
        {
            return center + Geometry.PointOnCircle2(radius, rotation2);
        }

        /// <summary>
        /// Returns a random point inside a circle
        /// </summary>
        public static Vector2 PointInCircle(Circle2 circle)
        {
            return PointInCircle(circle.center, circle.radius);
        }

        /// <summary>
        /// Returns a random point inside a circle
        /// </summary>
        public static Vector2 PointInCircle(Vector2 center, float radius)
        {
            return center + Random.insideUnitCircle*radius;
        }

        /// <summary>
        /// Returns a random point on a sphere
        /// </summary>
        public static Vector3 PointOnSphere(Sphere sphere)
        {
            return PointOnSphere(sphere.center, sphere.radius);
        }

        /// <summary>
        /// Returns a random point on a sphere
        /// </summary>
        public static Vector3 PointOnSphere(Vector3 center, float radius)
        {
            return center + Random.onUnitSphere*radius;
        }

        /// <summary>
        /// Returns a random point inside a sphere
        /// </summary>
        public static Vector3 PointInSphere(Sphere sphere)
        {
            return PointInSphere(sphere.center, sphere.radius);
        }

        /// <summary>
        /// Returns a random point inside a sphere
        /// </summary>
        public static Vector3 PointInSphere(Vector3 center, float radius)
        {
            return center + Random.insideUnitSphere*radius;
        }

        /// <summary>
        /// Returns a random point inside a <paramref name="rect"/>
        /// </summary>
        public static Vector2 PointInRect(Rect rect)
        {
            return Range(rect.min, rect.max);
        }

        /// <summary>
        /// Returns a random point on the perimeter of a <paramref name="rect"/>
        /// </summary>
        public static Vector2 PointOnRect(Rect rect)
        {
            float perimeter = 2*rect.width + 2*rect.height;
            float value = Random.value*perimeter;
            if (value < rect.width)
            {
                return rect.min + new Vector2(value, 0);
            }
            value -= rect.width;
            if (value < rect.height)
            {
                return rect.min + new Vector2(rect.width, value);
            }
            value -= rect.height;
            if (value < rect.width)
            {
                return rect.min + new Vector2(value, rect.height);
            }
            return rect.min + new Vector2(0, value - rect.width);
        }

        /// <summary>
        /// Returns a random point inside <paramref name="bounds"/>
        /// </summary>
        public static Vector3 PointInBounds(Bounds bounds)
        {
            return Range(bounds.min, bounds.max);
        }

        #endregion Geometry

        #region Colors

        /// <summary>
        /// Returns a random color between black [inclusive] and white [inclusive]
        /// </summary>
        public static Color color { get { return new Color(Random.value, Random.value, Random.value); } }

        /// <summary>
        /// Returns a color with a random hue and a maximum saturation and value in HSV model
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
        /// Returns a color with a random hue and a given <paramref name="saturation"/> and <paramref name="value"/>
        /// </summary>
        public static ColorHSV ColorHue(float saturation, float value, float alpha = 1)
        {
            return new ColorHSV(Random.value, saturation, value, alpha);
        }

        /// <summary>
        /// Returns a color with a random saturation and given <paramref name="hue"/> and <paramref name="value"/>
        /// </summary>
        public static ColorHSV ColorSaturation(float hue, float value, float alpha = 1)
        {
            return new ColorHSV(hue, Random.value, value, alpha);
        }

        /// <summary>
        /// Returns a color with a random value and given <paramref name="hue"/> and <paramref name="saturation"/>
        /// </summary>
        public static ColorHSV ColorValue(float hue, float saturation, float alpha = 1)
        {
            return new ColorHSV(hue, saturation, Random.value, alpha);
        }

        /// <summary>
        /// Returns an analogous palette based on a color with a random hue
        /// </summary>
        public static List<ColorHSV> AnalogousPalette(float saturation = 1, float value = 1, float alpha = 1, int count = 2,
            bool withComplementary = false)
        {
            return ColorHue(saturation, value, alpha).GetAnalogousPalette(count, withComplementary);
        }

        /// <summary>
        /// Returns a triadic palette based on a color with a random hue
        /// </summary>
        public static List<ColorHSV> TriadicPalette(float saturation = 1, float value = 1, float alpha = 1, bool withComplementary = false)
        {
            return ColorHue(saturation, value, alpha).GetTriadicPalette(withComplementary);
        }

        /// <summary>
        /// Returns a tetradic palette based on a color with a random hue
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
        public static string string8 { get { return PTUtils.Alphanumerics.GetRandom(8); } }

        /// <summary>
        /// Returns a random alphanumeric 16-character string
        /// </summary>
        public static string string16 { get { return PTUtils.Alphanumerics.GetRandom(16); } }

        /// <summary>
        /// Returns a random lowercase letter
        /// </summary>
        public static char lowercaseLetter { get { return PTUtils.LowercaseLetters.GetRandom(); } }

        /// <summary>
        /// Returns a random uppercase letter
        /// </summary>
        public static char uppercaseLetter { get { return PTUtils.UppercaseLetters.GetRandom(); } }

        #endregion Strings

        /// <summary>
        /// Returns a random element
        /// </summary>
        public static T GetRandom<T>(this IList<T> list)
        {
            if (list == null)
            {
                throw new ArgumentNullException("list");
            }
            if (list.Count == 0)
            {
                throw new ArgumentException("Empty list");
            }
            return list[Random.Range(0, list.Count)];
        }

        /// <summary>
        /// Returns a random element
        /// </summary>
        public static T GetRandom<T>(T item1, T item2, params T[] items)
        {
            int index = Random.Range(0, items.Length + 2);
            if (index == 0)
            {
                return item1;
            }
            if (index == 1)
            {
                return item2;
            }
            return items[index - 2];
        }

        /// <summary>
        /// Returns a random value from the dictionary
        /// </summary>
        public static TValue GetRandom<TKey, TValue>(this Dictionary<TKey, TValue> dictionary)
        {
            if (dictionary == null)
            {
                throw new ArgumentNullException("dictionary");
            }
            var keys = dictionary.Keys;
            if (keys.Count == 0)
            {
                throw new ArgumentException("Empty dictionary");
            }
            return dictionary[new List<TKey>(keys).GetRandom()];
        }

        /// <summary>
        /// Returns a random element with the chances of rolling based on <paramref name="weights"/>
        /// </summary>
        /// <param name="weights">Positive floats representing chances</param>
        public static T GetRandom<T>(this IList<T> list, IList<float> weights)
        {
            if (list == null)
            {
                throw new ArgumentNullException("list");
            }
            if (list.Count == 0)
            {
                throw new ArgumentException("Empty list");
            }
            if (weights == null)
            {
                throw new ArgumentNullException("weights");
            }
            if (weights.Count == 0)
            {
                throw new ArgumentException("Empty weights");
            }
            if (list.Count != weights.Count)
            {
                throw new ArgumentException("Array sizes must be equal");
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
                throw new ArgumentException("Weights must be positive");
            }
            return list[index];
        }

        /// <summary>
        /// Returns a random character from the string
        /// </summary>
        public static char GetRandom(this string chars)
        {
            if (string.IsNullOrEmpty(chars))
            {
                throw new ArgumentException("Empty string");
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
                throw new ArgumentException("Empty string");
            }
            var randomString = new StringBuilder(length);
            for (int i = 0; i < length; i++)
            {
                randomString.Append(chars[Random.Range(0, chars.Length)]);
            }
            return randomString.ToString();
        }

        /// <summary>
        /// Returns a random element and removes it from the list
        /// </summary>
        public static T PopRandom<T>(this List<T> list)
        {
            if (list == null)
            {
                throw new ArgumentNullException("list");
            }
            if (list.Count == 0)
            {
                throw new ArgumentException("Empty list");
            }
            var index = Random.Range(0, list.Count);
            var item = list[index];
            list.RemoveAt(index);
            return item;
        }

        /// <summary>
        /// Fisher–Yates shuffle
        /// </summary>
        /// <remarks>
        /// https://en.wikipedia.org/wiki/Fisher–Yates_shuffle
        /// </remarks>
        public static void Shuffle<T>(this IList<T> list)
        {
            if (list == null)
            {
                throw new ArgumentNullException("list");
            }
            for (int i = 0; i < list.Count; i++)
            {
                int j = Random.Range(i, list.Count);
                T tmp = list[j];
                list[j] = list[i];
                list[i] = tmp;
            }
        }

        /// <summary>
        /// Returns true with <paramref name="percent"/> probability
        /// </summary>
        /// <param name="percent">between 0.0 [inclusive] and 1.0 [inclusive]</param>
        public static bool Chance(float percent)
        {
            if (percent == 0) return false;
            if (percent == 1) return true;
            return Random.value < percent;
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
            return new Vector4(Random.Range(min.x, max.x), Random.Range(min.y, max.y), Random.Range(min.z, max.z), Random.Range(min.w, max.w));
        }

        /// <summary>
        /// Returns a random float number between and <paramref name="min"/> [inclusive] and <paramref name="max"/> [inclusive].
        /// Ensures that there will be only specified amount of variants.
        /// </summary>
        public static float Range(float min, float max, int variants)
        {
            if (variants < 2)
            {
                throw new ArgumentException("Variants must be greater than one");
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
            return new Vector3(Range(min.x, max.x, variants), Range(min.y, max.y, variants), Range(min.z, max.z, variants));
        }

        /// <summary>
        /// Returns a random vector between and <paramref name="min"/> [inclusive] and <paramref name="max"/> [inclusive].
        /// Ensures that there will be only specified amount of variants.
        /// </summary>
        public static Vector4 Range(Vector4 min, Vector4 max, int variants)
        {
            return new Vector4(Range(min.x, max.x, variants), Range(min.y, max.y, variants), Range(min.z, max.z, variants),
                Range(min.w, max.w, variants));
        }
    }
}
