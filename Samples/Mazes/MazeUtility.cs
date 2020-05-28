using UnityEngine;

namespace ProceduralToolkit.Samples
{
    public static class MazeUtility
    {
        private const int roomSize = 2;
        private const int wallSize = 1;

        public static void DrawConnection(Vector2Int a, Vector2Int b, Texture2D texture, Color color)
        {
            texture.DrawRect(ConnectionToRect(a, b), color);
        }

        public static int GetTextureWidth(int mazeWidth)
        {
            return wallSize + mazeWidth*(roomSize + wallSize);
        }

        public static int GetTextureHeight(int mazeHeight)
        {
            return wallSize + mazeHeight*(roomSize + wallSize);
        }

        private static RectInt ConnectionToRect(Vector2Int a, Vector2Int b)
        {
            var rect = new RectInt
            {
                min = new Vector2Int(
                    x: wallSize + Mathf.Min(a.x, b.x)*(roomSize + wallSize),
                    y: wallSize + Mathf.Min(a.y, b.y)*(roomSize + wallSize))
            };

            if ((b - a).y == 0)
            {
                rect.width = roomSize*2 + wallSize;
                rect.height = roomSize;
            }
            else
            {
                rect.width = roomSize;
                rect.height = roomSize*2 + wallSize;
            }
            return rect;
        }
    }
}
