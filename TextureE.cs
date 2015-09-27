using System;
using UnityEngine;

namespace ProceduralToolkit
{
    public static class TextureE
    {
        /// <summary>
        /// Draws line on texture using Bresenham's or Wu's algorithm
        /// </summary>
        public static void DrawLine(this Texture2D texture, int x0, int y0, int x1, int y1, Color color, bool AA = false)
        {
            if (AA)
            {
                Action<int, int, float> draw =
                    (x, y, t) => texture.SetPixel(x, y, Color.Lerp(texture.GetPixel(x, y), color, t));
                PTUtils.WuLine(x0, y0, x1, y1, draw);
            }
            else
            {
                Action<int, int> draw = (x, y) => texture.SetPixel(x, y, color);
                PTUtils.BresenhamLine(x0, y0, x1, y1, draw);
            }
        }

        /// <summary>
        /// Draws circle on texture using Bresenham's algorithm
        /// </summary>
        public static void DrawCircle(this Texture2D texture, int x, int y, int radius, Color color)
        {
            Action<int, int> draw = (_x, _y) => texture.SetPixel(_x, _y, color);
            PTUtils.BresenhamCircle(x, y, radius, draw);
        }

        /// <summary>
        /// Draws filled rectangle on texture
        /// </summary>
        public static void DrawRect(this Texture2D texture, int x, int y, int blockWidth, int blockHeight, Color color)
        {
            var colors = new Color[blockWidth*blockHeight];
            for (int _y = 0; _y < blockHeight; _y++)
            {
                for (int _x = 0; _x < blockWidth; _x++)
                {
                    colors[_x + _y*blockWidth] = color;
                }
            }
            texture.SetPixels(x, y, blockWidth, blockHeight, colors);
        }

        /// <summary>
        /// Fills texture with gradient
        /// </summary>
        public static void DrawGradient(this Texture2D texture, Gradient gradient, Directions progressionDirection)
        {
            texture.DrawGradientRect(0, 0, texture.width, texture.height, gradient, progressionDirection);
        }

        /// <summary>
        /// Draws gradient rectangle on texture
        /// </summary>t
        public static void DrawGradientRect(this Texture2D texture, int x, int y, int blockWidth, int blockHeight,
            Gradient gradient, Directions progressionDirection)
        {
            Func<int, int, Color> getColor;
            switch (progressionDirection)
            {
                case Directions.Left:
                    getColor = (_x, _y) => gradient.Evaluate(1 - (float) _x/(float) blockWidth);
                    break;
                case Directions.Right:
                    getColor = (_x, _y) => gradient.Evaluate((float) _x/(float) blockWidth);
                    break;
                case Directions.Down:
                    getColor = (_x, _y) => gradient.Evaluate(1 - (float) _y/(float) blockHeight);
                    break;
                case Directions.Up:
                    getColor = (_x, _y) => gradient.Evaluate((float) _y/(float) blockHeight);
                    break;
                default:
                    Debug.LogError("Not supported direction: " + progressionDirection);
                    return;
            }

            var colors = new Color[blockWidth*blockHeight];
            for (int _y = 0; _y < blockHeight; _y++)
            {
                for (int _x = 0; _x < blockWidth; _x++)
                {
                    colors[_x + _y*blockWidth] = getColor(_x, _y);
                }
            }
            texture.SetPixels(x, y, blockWidth, blockHeight, colors);
        }

        /// <summary>
        /// Fills texture with white color
        /// </summary>
        public static void Clear(this Texture2D texture)
        {
            Clear(texture, Color.white);
        }

        /// <summary>
        /// Fills texture with specified color
        /// </summary>
        public static void Clear(this Texture2D texture, Color color)
        {
            var pixels = texture.GetPixels();
            for (var i = 0; i < pixels.Length; ++i)
            {
                pixels[i] = color;
            }
            texture.SetPixels(pixels);
        }
    }
}