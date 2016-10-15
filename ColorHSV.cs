﻿using System;
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

        public float this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                        return h;
                    case 1:
                        return s;
                    case 2:
                        return v;
                    case 3:
                        return a;
                    default:
                        throw new IndexOutOfRangeException("Invalid ColorHSV index!");
                }
            }
            set
            {
                switch (index)
                {
                    case 0:
                        h = value;
                        break;
                    case 1:
                        s = value;
                        break;
                    case 2:
                        v = value;
                        break;
                    case 3:
                        a = value;
                        break;
                    default:
                        throw new IndexOutOfRangeException("Invalid ColorHSV index!");
                }
            }
        }

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

        public static explicit operator Vector4(ColorHSV c)
        {
            return new Vector4(c.h, c.s, c.v, c.a);
        }

        public static bool operator ==(ColorHSV lhs, ColorHSV rhs)
        {
            return (Vector4) lhs == (Vector4) rhs;
        }

        public static bool operator !=(ColorHSV lhs, ColorHSV rhs)
        {
            return !(lhs == rhs);
        }

        /// <summary>
        /// Returns a nicely formatted string for this color
        /// </summary>
        public override string ToString()
        {
            return string.Format("HSVA({0:F3}, {1:F3}, {2:F3}, {3:F3})", h, s, v, a);
        }

        public override int GetHashCode()
        {
            return ((Vector4) this).GetHashCode();
        }

        public override bool Equals(object other)
        {
            if (!(other is ColorHSV))
            {
                return false;
            }
            ColorHSV color = (ColorHSV) other;
            if (h.Equals(color.h) && s.Equals(color.s) && v.Equals(color.v))
            {
                return a.Equals(color.a);
            }
            return false;
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
        /// Returns new color with modified saturation and value components
        /// </summary>
        public ColorHSV WithSV(float s, float v)
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