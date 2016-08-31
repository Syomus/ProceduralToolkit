using System.Collections.Generic;
using UnityEngine;

namespace ProceduralToolkit
{
    /// <summary>
    /// Representation of color in HSV model
    /// </summary>
    public struct ColorHSV
    {
        /// <summary>
        /// Hue component of the color
        /// </summary>
        public float h;

        /// <summary>
        /// Saturation component of the color
        /// </summary>
        public float s;

        /// <summary>
        /// Value component of the color
        /// </summary>
        public float v;

        /// <summary>
        /// Alpha component of the color
        /// </summary>
        public float a;

        /// <summary>
        /// Returns opposite color on the color wheel
        /// </summary>
        public ColorHSV complementary { get { return WithOffsetH(180); } }

        /// <summary>
        /// Constructs a new ColorHSV with given h, s, v, a components
        /// </summary>
        /// <param name="h">Hue component</param>
        /// <param name="s">Saturation component</param>
        /// <param name="v">Value component</param>
        /// <param name="a">Alpha component</param>
        public ColorHSV(float h, float s, float v, float a)
        {
            this.h = h;
            this.s = s;
            this.v = v;
            this.a = a;
        }

        /// <summary>
        /// Constructs a new ColorHSV with given h, s, v components and sets alpha to 1
        /// </summary>
        /// <param name="h">Hue component</param>
        /// <param name="s">Saturation component</param>
        /// <param name="v">Value component</param>
        public ColorHSV(float h, float s, float v)
        {
            this.h = h;
            this.s = s;
            this.v = v;
            a = 1;
        }

        /// <summary>
        /// Constructs a new ColorHSV from a Color
        /// </summary>
        public ColorHSV(Color color)
        {
            Color.RGBToHSV(color, out h, out s, out v);
            a = color.a;
        }

        /// <summary>
        /// Converts ColorHSV to a RGB representation
        /// </summary>
        public Color ToColor()
        {
            var color = Color.HSVToRGB(h, s, v);
            color.a = a;
            return color;
        }

        /// <summary>
        /// Returns new color with hue offset by <paramref name="angle"/> degrees
        /// </summary>
        public ColorHSV WithOffsetH(float angle)
        {
            return WithH(Mathf.Repeat(h + angle/360, 1));
        }

        /// <summary>
        /// Returns new color with modified hue component
        /// </summary>
        public ColorHSV WithH(float h)
        {
            return new ColorHSV(h, s, v, a);
        }

        /// <summary>
        /// Returns new color with modified saturation component
        /// </summary>
        public ColorHSV WithS(float s)
        {
            return new ColorHSV(h, s, v, a);
        }

        /// <summary>
        /// Returns new color with modified value component
        /// </summary>
        public ColorHSV WithV(float v)
        {
            return new ColorHSV(h, s, v, a);
        }

        /// <summary>
        /// Returns new color with modified alpha component
        /// </summary>
        public ColorHSV WithA(float a)
        {
            return new ColorHSV(h, s, v, a);
        }

        /// <summary>
        /// Returns list of this color, <paramref name="count"/> of analogous colors and optionally complementary color
        /// </summary>
        public List<ColorHSV> GetAnalogousPalette(int count = 2, bool withComplementary = false)
        {
            const float analogousAngle = 30;

            var palette = new List<ColorHSV> {this};
            int rightCount = count/2;
            int leftCount = count - rightCount;

            for (int i = 0; i < leftCount; i++)
            {
                palette.Add(WithOffsetH(-(i + 1)*analogousAngle));
            }
            for (int i = 0; i < rightCount; i++)
            {
                palette.Add(WithOffsetH((i + 1)*analogousAngle));
            }
            if (withComplementary)
            {
                palette.Add(complementary);
            }
            return palette;
        }

        /// <summary>
        /// Returns list of this color, two triadic colors and optionally complementary color
        /// </summary>
        public List<ColorHSV> GetTriadicPalette(bool withComplementary = false)
        {
            const float triadicAngle = 120;

            var palette = new List<ColorHSV>
            {
                this,
                WithOffsetH(-triadicAngle),
                WithOffsetH(triadicAngle)
            };
            if (withComplementary)
            {
                palette.Add(complementary);
            }
            return palette;
        }

        /// <summary>
        /// Returns list of this color and three tetradic colors
        /// </summary>
        public List<ColorHSV> GetTetradicPalette()
        {
            const float tetradicAngle = 60;

            var palette = new List<ColorHSV>
            {
                this,
                WithOffsetH(tetradicAngle),
                complementary,
                complementary.WithOffsetH(tetradicAngle)
            };
            return palette;
        }

        /// <summary>
        /// Returns a nicely formatted string for this color
        /// </summary>
        public override string ToString()
        {
            return string.Format("HSVA({0:F3}, {1:F3}, {2:F3}, {3:F3})", h, s, v, a);
        }

        /// <summary>
        /// Linearly interpolates between colors a and b by t.
        /// </summary>
        public static ColorHSV Lerp(ColorHSV a, ColorHSV b, float t)
        {
            t = Mathf.Clamp01(t);
            return LerpUnclamped(a, b, t);
        }

        /// <summary>
        /// Linearly interpolates between colors a and b by t.
        /// </summary>
        public static ColorHSV LerpUnclamped(ColorHSV a, ColorHSV b, float t)
        {
            float deltaH = Mathf.Repeat(b.h - a.h, 1);
            if (deltaH > 0.5f)
            {
                deltaH -= 1;
            }
            return new ColorHSV(
                Mathf.Repeat(a.h + deltaH*t, 1),
                a.s + (b.s - a.s)*t,
                a.v + (b.v - a.v)*t,
                a.a + (b.a - a.a)*t);
        }
    }
}