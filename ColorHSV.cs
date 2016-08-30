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
        /// Returns new color with modified hue component
        /// </summary>
        public ColorHSV WithH(float h)
        {
            return new ColorHSV(h, s, v, a);
        }

        /// <summary>
        /// Returns new color with modified saturation component
        /// </summary>
        public ColorHSV WithS(float g)
        {
            return new ColorHSV(h, s, v, a);
        }

        /// <summary>
        /// Returns new color with modified value component
        /// </summary>
        public ColorHSV WithV(float b)
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
        /// Returns a nicely formatted string for this color
        /// </summary>
        public override string ToString()
        {
            return string.Format("HSVA({0:F3}, {1:F3}, {2:F3}, {3:F3})", h, s, v, a);
        }
    }
}