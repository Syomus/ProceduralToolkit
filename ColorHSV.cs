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
            float x, max, left, right;
            if (color.r > color.g && color.r > color.b)
            {
                x = 0;
                max = color.r;
                left = color.g;
                right = color.b;
            }
            else if (color.g > color.b)
            {
                x = 2;
                max = color.g;
                left = color.b;
                right = color.r;
            }
            else
            {
                x = 4;
                max = color.b;
                left = color.r;
                right = color.g;
            }

            if (max != 0f)
            {
                float min = right < left ? right : left;
                float chroma = max - min;
                if (chroma != 0f)
                {
                    h = x + (left - right)/chroma;
                    s = chroma/max;
                }
                else
                {
                    h = x + left - right;
                    s = 0f;
                }
                h /= 6;
                if (h < 0)
                {
                    h++;
                }
            }
            else
            {
                h = 0f;
                s = 0f;
            }
            v = max;
            a = color.a;
        }

        /// <summary>
        /// Converts ColorHSV to a RGB representation
        /// </summary>
        public Color ToColor()
        {
            if (s == 0f)
            {
                return new Color(v, v, v);
            }
            if (v == 0f)
            {
                return Color.black;
            }

            float position = h*6f;
            int sector = Mathf.FloorToInt(position);
            float fractional = position - sector;
            float p = v*(1 - s);
            float q = v*(1 - s*fractional);
            float t = v*(1 - s*(1 - fractional));

            switch (sector)
            {
                case -1:
                    return new Color(v, p, q);
                case 0:
                    return new Color(v, t, p);
                case 1:
                    return new Color(q, v, p);
                case 2:
                    return new Color(p, v, t);
                case 3:
                    return new Color(p, q, v);
                case 4:
                    return new Color(t, p, v);
                case 5:
                    return new Color(v, p, q);
                case 6:
                    return new Color(v, t, p);
            }
            return Color.black;
        }
    }
}