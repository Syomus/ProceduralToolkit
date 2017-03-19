using System;
using UnityEngine;

namespace ProceduralToolkit
{
    /// <summary>
    /// Texture extensions
    /// </summary>
    public static class TextureE
    {
        /// <summary>
        /// Draws line on texture
        /// </summary>
        public static void DrawLine(this Texture2D texture, Vector2Int v0, Vector2Int v1, Color color)
        {
            if (texture == null)
            {
                throw new ArgumentNullException("texture");
            }
            Draw.RasterLine(v0, v1, (x, y) => texture.SetPixel(x, y, color));
        }

        /// <summary>
        /// Draws line on texture
        /// </summary>
        public static void DrawLine(this Texture2D texture, int x0, int y0, int x1, int y1, Color color)
        {
            if (texture == null)
            {
                throw new ArgumentNullException("texture");
            }
            Draw.RasterLine(x0, y0, x1, y1, (x, y) => texture.SetPixel(x, y, color));
        }

        /// <summary>
        /// Draws anti-aliased line on texture
        /// </summary>
        public static void DrawAALine(this Texture2D texture, Vector2Int v0, Vector2Int v1, Color color)
        {
            if (texture == null)
            {
                throw new ArgumentNullException("texture");
            }
            Draw.RasterAALine(v0, v1,
                (x, y, t) => texture.SetPixel(x, y, Color.Lerp(texture.GetPixel(x, y), color, t)));
        }

        /// <summary>
        /// Draws anti-aliased line on texture
        /// </summary>
        public static void DrawAALine(this Texture2D texture, int x0, int y0, int x1, int y1, Color color)
        {
            if (texture == null)
            {
                throw new ArgumentNullException("texture");
            }
            Draw.RasterAALine(x0, y0, x1, y1,
                (x, y, t) => texture.SetPixel(x, y, Color.Lerp(texture.GetPixel(x, y), color, t)));
        }

        /// <summary>
        /// Draws circle on texture
        /// </summary>
        public static void DrawCircle(this Texture2D texture, Vector2Int center, int radius, Color color)
        {
            if (texture == null)
            {
                throw new ArgumentNullException("texture");
            }
            Draw.RasterCircle(center, radius, (x, y) => texture.SetPixel(x, y, color));
        }

        /// <summary>
        /// Draws circle on texture
        /// </summary>
        public static void DrawCircle(this Texture2D texture, int centerX, int centerY, int radius, Color color)
        {
            if (texture == null)
            {
                throw new ArgumentNullException("texture");
            }
            Draw.RasterCircle(centerX, centerY, radius, (x, y) => texture.SetPixel(x, y, color));
        }

        /// <summary>
        /// Draws filled circle on texture using Bresenham's algorithm
        /// </summary>
        public static void DrawFilledCircle(this Texture2D texture, Vector2Int center, int radius, Color color)
        {
            if (texture == null)
            {
                throw new ArgumentNullException("texture");
            }
            Draw.RasterFilledCircle(center, radius, (x, y) => texture.SetPixel(x, y, color));
        }

        /// <summary>
        /// Draws filled circle on texture using Bresenham's algorithm
        /// </summary>
        public static void DrawFilledCircle(this Texture2D texture, int centerX, int centerY, int radius, Color color)
        {
            if (texture == null)
            {
                throw new ArgumentNullException("texture");
            }
            Draw.RasterFilledCircle(centerX, centerY, radius, (x, y) => texture.SetPixel(x, y, color));
        }

        /// <summary>
        /// Draws filled rectangle on texture
        /// </summary>
        public static void DrawRect(this Texture2D texture, int x, int y, int blockWidth, int blockHeight, Color color)
        {
            if (texture == null)
            {
                throw new ArgumentNullException("texture");
            }
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
            if (texture == null)
            {
                throw new ArgumentNullException("texture");
            }
            texture.DrawGradientRect(0, 0, texture.width, texture.height, gradient, progressionDirection);
        }

        /// <summary>
        /// Draws gradient rectangle on texture
        /// </summary>t
        public static void DrawGradientRect(this Texture2D texture, int x, int y, int blockWidth, int blockHeight,
            Gradient gradient, Directions progressionDirection)
        {
            if (texture == null)
            {
                throw new ArgumentNullException("texture");
            }
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
                    throw new ArgumentException("Not supported direction: " + progressionDirection,
                        "progressionDirection");
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
            if (texture == null)
            {
                throw new ArgumentNullException("texture");
            }
            var pixels = new Color[texture.width*texture.height];
            for (var i = 0; i < pixels.Length; i++)
            {
                pixels[i] = Color.white;
            }
            texture.SetPixels(pixels);
        }

        /// <summary>
        /// Fills texture with specified color
        /// </summary>
        public static void Clear(this Texture2D texture, Color color)
        {
            if (texture == null)
            {
                throw new ArgumentNullException("texture");
            }
            var pixels = new Color[texture.width*texture.height];
            for (var i = 0; i < pixels.Length; i++)
            {
                pixels[i] = color;
            }
            texture.SetPixels(pixels);
        }

        /// <summary>
        /// Fills texture with specified color
        /// </summary>
        public static void Clear(this Texture2D texture, Color color, ref Color[] pixels)
        {
            if (texture == null)
            {
                throw new ArgumentNullException("texture");
            }
            if (pixels == null)
            {
                throw new ArgumentNullException("pixels");
            }
            for (var i = 0; i < pixels.Length; i++)
            {
                pixels[i] = color;
            }
            texture.SetPixels(pixels);
        }
    }
}