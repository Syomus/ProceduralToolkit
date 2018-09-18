using System.Collections.Generic;
using ProceduralToolkit.ClipperLib;
using UnityEngine;

namespace ProceduralToolkit
{
    /// <summary>
    /// Utility class for conversion of Clipper data from and to Unity format
    /// </summary>
    public class ClipperUtility
    {
        public const float ClipperScale = 100000;

        public static List<IntPoint> ToIntPath(IList<Vector2> path)
        {
            var intPath = new List<IntPoint>(path.Count);
            foreach (var vertex in path)
            {
                intPath.Add(ToIntPoint(vertex));
            }
            return intPath;
        }

        public static List<IntPoint> ToIntPath(List<Vector2> path)
        {
            return path.ConvertAll(ToIntPoint);
        }

        public static List<Vector2> ToVector2Path(List<IntPoint> intPath)
        {
            return intPath.ConvertAll(ToVector2);
        }

        public static List<List<IntPoint>> ToIntPaths(List<List<Vector2>> paths)
        {
            return paths.ConvertAll(ToIntPath);
        }

        public static List<List<Vector2>> ToVector2Paths(List<List<IntPoint>> intPaths)
        {
            return intPaths.ConvertAll(ToVector2Path);
        }

        public static void ToVector2Paths(List<List<IntPoint>> intPaths, ref List<List<Vector2>> paths)
        {
            paths.Clear();
            foreach (List<IntPoint> intPath in intPaths)
            {
                paths.Add(ToVector2Path(intPath));
            }
        }

        public static IntPoint ToIntPoint(Vector2 vector2)
        {
            return new IntPoint(vector2.x*ClipperScale, vector2.y*ClipperScale);
        }

        public static Vector2 ToVector2(IntPoint intPoint)
        {
            return new Vector2(intPoint.X/ClipperScale, intPoint.Y/ClipperScale);
        }
    }
}
