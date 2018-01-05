using UnityEngine;

namespace ProceduralToolkit
{
    /// <summary>
    /// Color extensions
    /// </summary>
    public static class ColorE
    {
        #region HTML colors from http://www.w3.org/TR/REC-html40/types.html#h-6.5

        public static Color32 black32 { get { return new Color32(0, 0, 0, 255); } }
        public static Color32 silver32 { get { return new Color32(192, 192, 192, 255); } }
        public static Color32 gray32 { get { return new Color32(128, 128, 128, 255); } }
        public static Color32 white32 { get { return new Color32(255, 255, 255, 255); } }
        public static Color32 maroon32 { get { return new Color32(128, 0, 0, 255); } }
        public static Color32 red32 { get { return new Color32(255, 0, 0, 255); } }
        public static Color32 purple32 { get { return new Color32(128, 0, 128, 255); } }
        public static Color32 fuchsia32 { get { return new Color32(255, 0, 255, 255); } }
        public static Color32 green32 { get { return new Color32(0, 128, 0, 255); } }
        public static Color32 lime32 { get { return new Color32(0, 255, 0, 255); } }
        public static Color32 olive32 { get { return new Color32(128, 128, 0, 255); } }
        public static Color32 yellow32 { get { return new Color32(255, 255, 0, 255); } }
        public static Color32 navy32 { get { return new Color32(0, 0, 128, 255); } }
        public static Color32 blue32 { get { return new Color32(0, 0, 255, 255); } }
        public static Color32 teal32 { get { return new Color32(0, 128, 128, 255); } }
        public static Color32 aqua32 { get { return new Color32(0, 255, 255, 255); } }

        public static Color black { get { return black32; } }
        public static Color silver { get { return silver32; } }
        public static Color gray { get { return gray32; } }
        public static Color white { get { return white32; } }
        public static Color maroon { get { return maroon32; } }
        public static Color red { get { return red32; } }
        public static Color purple { get { return purple32; } }
        public static Color fuchsia { get { return fuchsia32; } }
        public static Color green { get { return green32; } }
        public static Color lime { get { return lime32; } }
        public static Color olive { get { return olive32; } }
        public static Color yellow { get { return yellow32; } }
        public static Color navy { get { return navy32; } }
        public static Color blue { get { return blue32; } }
        public static Color teal { get { return teal32; } }
        public static Color aqua { get { return aqua32; } }

        #endregion Colors

        /// <summary>
        /// Returns an inverted color with the same alpha
        /// </summary>
        public static Color Inverted(this Color color)
        {
            var result = Color.white - color;
            result.a = color.a;
            return result;
        }

        /// <summary>
        /// Creates a gradient between two colors
        /// </summary>
        public static Gradient Gradient(Color from, Color to)
        {
            var g = new Gradient();
            g.SetKeys(new[] {new GradientColorKey(from, 0), new GradientColorKey(to, 1)},
                new[] {new GradientAlphaKey(from.a, 0), new GradientAlphaKey(to.a, 1)});
            return g;
        }

        /// <summary>
        /// Creates a gradient between two colors
        /// </summary>
        public static Gradient Gradient(ColorHSV from, ColorHSV to)
        {
            var g = new Gradient();
            g.SetKeys(new[] {new GradientColorKey(from.ToColor(), 0), new GradientColorKey(to.ToColor(), 1)},
                new[] {new GradientAlphaKey(from.a, 0), new GradientAlphaKey(to.a, 1)});
            return g;
        }

        /// <summary>
        /// Returns a new color with the modified red component
        /// </summary>
        public static Color WithR(this Color color, float r)
        {
            return new Color(r, color.g, color.b, color.a);
        }

        /// <summary>
        /// Returns anew color with the modified green component
        /// </summary>
        public static Color WithG(this Color color, float g)
        {
            return new Color(color.r, g, color.b, color.a);
        }

        /// <summary>
        /// Returns a new color with the modified blue component
        /// </summary>
        public static Color WithB(this Color color, float b)
        {
            return new Color(color.r, color.g, b, color.a);
        }

        /// <summary>
        /// Returns a new color with the modified alpha component
        /// </summary>
        public static Color WithA(this Color color, float a)
        {
            return new Color(color.r, color.g, color.b, a);
        }

        /// <summary>
        /// Returns the color as a hexadecimal string in the format "RRGGBB"
        /// </summary>
        public static string ToHtmlStringRGB(this Color color)
        {
            return ColorUtility.ToHtmlStringRGB(color);
        }

        /// <summary>
        /// Returns the color as a hexadecimal string in the format "RRGGBBAA"
        /// </summary>
        public static string ToHtmlStringRGBA(this Color color)
        {
            return ColorUtility.ToHtmlStringRGBA(color);
        }
    }
}
