using UnityEngine;

namespace ProceduralToolkit
{
    public static class ColorE
    {
        #region Colors

        // HTML colors from http://www.w3.org/TR/REC-html40/types.html#h-6.5

        public static readonly Color32 black32 = new Color32(0, 0, 0, 255);
        public static readonly Color32 silver32 = new Color32(192, 192, 192, 255);
        public static readonly Color32 gray32 = new Color32(128, 128, 128, 255);
        public static readonly Color32 white32 = new Color32(255, 255, 255, 255);
        public static readonly Color32 maroon32 = new Color32(128, 0, 0, 255);
        public static readonly Color32 red32 = new Color32(255, 0, 0, 255);
        public static readonly Color32 purple32 = new Color32(128, 0, 128, 255);
        public static readonly Color32 fuchsia32 = new Color32(255, 0, 255, 255);
        public static readonly Color32 green32 = new Color32(0, 128, 0, 255);
        public static readonly Color32 lime32 = new Color32(0, 255, 0, 255);
        public static readonly Color32 olive32 = new Color32(128, 128, 0, 255);
        public static readonly Color32 yellow32 = new Color32(255, 255, 0, 255);
        public static readonly Color32 navy32 = new Color32(0, 0, 128, 255);
        public static readonly Color32 blue32 = new Color32(0, 0, 255, 255);
        public static readonly Color32 teal32 = new Color32(0, 128, 128, 255);
        public static readonly Color32 aqua32 = new Color32(0, 255, 255, 255);

        public static readonly Color black = black32;
        public static readonly Color silver = silver32;
        public static readonly Color gray = gray32;
        public static readonly Color white = white32;
        public static readonly Color maroon = maroon32;
        public static readonly Color red = red32;
        public static readonly Color purple = purple32;
        public static readonly Color fuchsia = fuchsia32;
        public static readonly Color green = green32;
        public static readonly Color lime = lime32;
        public static readonly Color olive = olive32;
        public static readonly Color yellow = yellow32;
        public static readonly Color navy = navy32;
        public static readonly Color blue = blue32;
        public static readonly Color teal = teal32;
        public static readonly Color aqua = aqua32;

        #endregion Colors

        /// <summary>
        /// Returns inverted color with the same alpha
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
        /// Returns new color with modified red component
        /// </summary>
        public static Color WithR(this Color color, float r)
        {
            return new Color(r, color.g, color.b, color.a);
        }

        /// <summary>
        /// Returns new color with modified green component
        /// </summary>
        public static Color WithG(this Color color, float g)
        {
            return new Color(color.r, g, color.b, color.a);
        }

        /// <summary>
        /// Returns new color with modified blue component
        /// </summary>
        public static Color WithB(this Color color, float b)
        {
            return new Color(color.r, color.g, b, color.a);
        }

        /// <summary>
        /// Returns new color with modified alpha component
        /// </summary>
        public static Color WithA(this Color color, float a)
        {
            return new Color(color.r, color.g, color.b, a);
        }
    }
}