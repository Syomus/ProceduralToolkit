using System;
using UnityEngine;

namespace ProceduralToolkit
{
    /// <summary>
    /// Collection of generic vector and raster drawing algorithms
    /// </summary>
    public static partial class Draw
    {
        /// <summary>
        /// Draws a line and calls <paramref name="draw"/> on every pixel
        /// </summary>
        /// <remarks>
        /// https://en.wikipedia.org/wiki/Bresenham%27s_line_algorithm
        /// </remarks>
        public static void RasterLine(Vector2Int v0, Vector2Int v1, Action<int, int> draw)
        {
            RasterLine(v0.x, v0.y, v1.x, v1.y, draw);
        }

        /// <summary>
        /// Draws a line and calls <paramref name="draw"/> on every pixel
        /// </summary>
        /// <remarks>
        /// https://en.wikipedia.org/wiki/Bresenham%27s_line_algorithm
        /// </remarks>
        public static void RasterLine(int x0, int y0, int x1, int y1, Action<int, int> draw)
        {
            bool steep = Math.Abs(y1 - y0) > Math.Abs(x1 - x0);
            if (steep)
            {
                PTUtils.Swap(ref x0, ref y0);
                PTUtils.Swap(ref x1, ref y1);
            }
            if (x0 > x1)
            {
                PTUtils.Swap(ref x0, ref x1);
                PTUtils.Swap(ref y0, ref y1);
            }

            int dx = x1 - x0;
            int dy = Math.Abs(y1 - y0);
            int error = dx/2;
            int ystep = (y0 < y1) ? 1 : -1;
            int y = y0;
            for (int x = x0; x <= x1; x++)
            {
                draw(steep ? y : x, steep ? x : y);
                error -= dy;
                if (error < 0)
                {
                    y += ystep;
                    error += dx;
                }
            }
        }

        /// <summary>
        /// Draws an anti-aliased line and calls <paramref name="draw"/> on every pixel
        /// </summary>
        /// <remarks>
        /// https://en.wikipedia.org/wiki/Xiaolin_Wu%27s_line_algorithm
        /// </remarks>
        public static void RasterAALine(Vector2Int v0, Vector2Int v1, Action<int, int, float> draw)
        {
            RasterAALine(v0.x, v0.y, v1.x, v1.y, draw);
        }

        /// <summary>
        /// Draws an anti-aliased line and calls <paramref name="draw"/> on every pixel
        /// </summary>
        /// <remarks>
        /// https://en.wikipedia.org/wiki/Xiaolin_Wu%27s_line_algorithm
        /// </remarks>
        public static void RasterAALine(int x0, int y0, int x1, int y1, Action<int, int, float> draw)
        {
            bool steep = Math.Abs(y1 - y0) > Math.Abs(x1 - x0);
            if (steep)
            {
                PTUtils.Swap(ref x0, ref y0);
                PTUtils.Swap(ref x1, ref y1);
            }
            if (x0 > x1)
            {
                PTUtils.Swap(ref x0, ref x1);
                PTUtils.Swap(ref y0, ref y1);
            }

            if (steep)
            {
                draw(y0, x0, 1);
                draw(y1, x1, 1);
            }
            else
            {
                draw(x0, y0, 1);
                draw(x1, y1, 1);
            }
            float dx = x1 - x0;
            float dy = y1 - y0;
            float gradient = dy/dx;
            float y = y0 + gradient;
            for (var x = x0 + 1; x <= x1 - 1; x++)
            {
                if (steep)
                {
                    draw((int) y, x, 1 - (y - (int) y));
                    draw((int) y + 1, x, y - (int) y);
                }
                else
                {
                    draw(x, (int) y, 1 - (y - (int) y));
                    draw(x, (int) y + 1, y - (int) y);
                }
                y += gradient;
            }
        }

        /// <summary>
        /// Draws a circle and calls <paramref name="draw"/> on every pixel
        /// </summary>
        /// <remarks>
        /// A Rasterizing Algorithm for Drawing Curves
        /// http://members.chello.at/easyfilter/bresenham.pdf
        /// </remarks>
        public static void RasterCircle(Vector2Int v0, int radius, Action<int, int> draw)
        {
            RasterCircle(v0.x, v0.y, radius, draw);
        }

        /// <summary>
        /// Draws a circle and calls <paramref name="draw"/> on every pixel
        /// </summary>
        /// <remarks>
        /// A Rasterizing Algorithm for Drawing Curves
        /// http://members.chello.at/easyfilter/bresenham.pdf
        /// </remarks>
        public static void RasterCircle(int x0, int y0, int radius, Action<int, int> draw)
        {
            if (radius == 0)
            {
                draw(x0, y0);
                return;
            }

            int x = -radius;
            int y = 0;
            int error = 2 - 2*radius; // 2 quadrant ◴
            while (x < 0)
            {
                draw(x0 - x, y0 + y); // 1 quadrant ◷
                draw(x0 - y, y0 - x); // 2 quadrant ◴
                draw(x0 + x, y0 - y); // 3 quadrant ◵
                draw(x0 + y, y0 + x); // 4 quadrant ◶

                int lastError = error;
                if (y >= error)
                {
                    y++;
                    error += 2*y + 1;
                }

                // Second check is needed to avoid weird pixels at diagonals at some radiuses
                // Example radiuses: 4, 11, 134, 373, 4552
                if (x < lastError || y < error)
                {
                    x++;
                    error += 2*x + 1;
                }
            }
        }

        /// <summary>
        /// Draws a filled circle and calls <paramref name="draw"/> on every pixel
        /// </summary>
        public static void RasterFilledCircle(Vector2Int v0, int radius, Action<int, int> draw)
        {
            RasterFilledCircle(v0.x, v0.y, radius, draw);
        }

        /// <summary>
        /// Draws a filled circle and calls <paramref name="draw"/> on every pixel
        /// </summary>
        public static void RasterFilledCircle(int x0, int y0, int radius, Action<int, int> draw)
        {
            if (radius == 0)
            {
                draw(x0, y0);
                return;
            }
            if (radius == 1)
            {
                draw(x0, y0 + 1);
                draw(x0 - 1, y0);
                draw(x0, y0);
                draw(x0 + 1, y0);
                draw(x0, y0 - 1);
                return;
            }

            int x = -radius;
            int y = 0;
            int error = 2 - 2*radius; // 2 quadrant ◴
            // lastY must have a different value than y
            int lastY = int.MaxValue;
            while (x < 0)
            {
                // This check prevents overdraw at poles
                if (lastY != y)
                {
                    DrawHorizontalLine(x0 + x, x0 - x, y0 + y, draw); // ◠
                    // This check prevents overdraw at central horizontal
                    if (y != 0)
                    {
                        DrawHorizontalLine(x0 + x, x0 - x, y0 - y, draw); // ◡
                    }
                }
                lastY = y;

                int lastError = error;
                if (y >= error)
                {
                    y++;
                    error += 2*y + 1;
                }

                // Second check is needed to avoid weird pixels at diagonals at some radiuses
                // Example radiuses: 4, 11, 134, 373, 4552
                if (x < lastError || y < error)
                {
                    x++;
                    error += 2*x + 1;
                }
            }
        }

        private static void DrawHorizontalLine(int fromX, int toX, int y, Action<int, int> draw)
        {
            for (int x = fromX; x <= toX; x++)
            {
                draw(x, y);
            }
        }
    }
}
