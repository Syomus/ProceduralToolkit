using System;
using UnityEngine;

namespace ProceduralToolkit
{
    public static class TextureE
    {
        public static Texture2D whitePixel
        {
            get { return Pixel(Color.white); }
        }

        public static Texture2D blackPixel
        {
            get { return Pixel(Color.black); }
        }

        public static Texture2D checker
        {
            get { return Checker(Color.white, Color.black); }
        }

        public static Texture2D Pixel(Color color)
        {
            var texture = new Texture2D(1, 1);
            texture.SetPixel(0, 0, color);
            texture.Apply();
            return texture;
        }

        public static Texture2D Gradient(Color top, Color bottom)
        {
            var texture = new Texture2D(2, 2) {wrapMode = TextureWrapMode.Clamp};
            texture.SetPixels(new[] {bottom, bottom, top, top});
            texture.Apply();
            return texture;
        }

        public static Texture2D Checker(Color first, Color second)
        {
            var texture = new Texture2D(2, 2) {filterMode = FilterMode.Point};
            texture.SetPixels(new[] {second, first, first, second});
            texture.Apply();
            return texture;
        }

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

        public static void DrawRect(this Texture2D texture, int x, int y, int blockWidth, int blockHeight, Color color)
        {
            var colors = new Color[blockWidth*blockHeight];
            for (int i = 0; i < blockHeight; i++)
            {
                for (int j = 0; j < blockWidth; j++)
                {
                    colors[j + i*blockWidth] = color;
                }
            }
            texture.SetPixels(x, y, blockWidth, blockHeight, colors);
        }

        public static void Clear(this Texture2D texture)
        {
            Clear(texture, Color.white);
        }

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