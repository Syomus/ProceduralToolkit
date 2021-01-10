using System;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

namespace ProceduralToolkit
{
    /// <summary>
    /// Array extensions
    /// </summary>
    public static class ArrayE
    {
        /// <summary>
        /// Gets the next or the first node in the <see cref="LinkedList{T}"/>
        /// </summary>
        public static LinkedListNode<T> NextOrFirst<T>(this LinkedListNode<T> current)
        {
            return current.Next ?? current.List.First;
        }

        /// <summary>
        /// Gets the previous or the last node in the <see cref="LinkedList{T}"/>
        /// </summary>
        public static LinkedListNode<T> PreviousOrLast<T>(this LinkedListNode<T> current)
        {
            return current.Previous ?? current.List.Last;
        }

        /// <summary>
        /// Two-dimensional indexer getter
        /// </summary>
        public static T GetXY<T>(this IReadOnlyList<T> list, Vector2Int position, int width)
        {
            return list[position.y*width + position.x];
        }

        /// <summary>
        /// Two-dimensional indexer getter
        /// </summary>
        public static T GetXY<T>(this IReadOnlyList<T> list, int x, int y, int width)
        {
            return list[y*width + x];
        }

        /// <summary>
        /// Two-dimensional indexer getter
        /// </summary>
        public static T GetXY<T>(this NativeArray<T> list, Vector2Int position, int width) where T : struct
        {
            return list[position.y*width + position.x];
        }

        /// <summary>
        /// Two-dimensional indexer getter
        /// </summary>
        public static T GetXY<T>(this NativeArray<T> list, int x, int y, int width) where T : struct
        {
            return list[y*width + x];
        }

        /// <summary>
        /// Two-dimensional indexer setter, ignores IList.IsReadOnly
        /// </summary>
        public static void SetXY<T>(this IList<T> list, Vector2Int position, int width, T value)
        {
            list[position.y*width + position.x] = value;
        }

        /// <summary>
        /// Two-dimensional indexer setter, ignores IList.IsReadOnly
        /// </summary>
        public static void SetXY<T>(this IList<T> list, int x, int y, int width, T value)
        {
            list[y*width + x] = value;
        }

        /// <summary>
        /// Two-dimensional indexer setter
        /// </summary>
        public static void SetXY<T>(this NativeArray<T> list, Vector2Int position, int width, T value) where T : struct
        {
            list[position.y*width + position.x] = value;
        }

        /// <summary>
        /// Two-dimensional indexer setter
        /// </summary>
        public static void SetXY<T>(this NativeArray<T> list, int x, int y, int width, T value) where T : struct
        {
            list[y*width + x] = value;
        }

        /// <summary>
        /// Looped indexer getter, allows out of bounds indices
        /// </summary>
        public static T GetLooped<T>(this IReadOnlyList<T> list, int index)
        {
            while (index < 0)
            {
                index += list.Count;
            }
            if (index >= list.Count)
            {
                index %= list.Count;
            }
            return list[index];
        }

        /// <summary>
        /// Looped indexer setter, allows out of bounds indices, ignores IList.IsReadOnly
        /// </summary>
        public static void SetLooped<T>(this IList<T> list, int index, T value)
        {
            while (index < 0)
            {
                index += list.Count;
            }
            if (index >= list.Count)
            {
                index %= list.Count;
            }
            list[index] = value;
        }

        /// <summary>
        /// Checks if <paramref name="position"/> is within array bounds
        /// </summary>
        public static bool IsInBounds<T>(this T[,] array, Vector2Int position)
        {
            return IsInBounds(array, position.x, position.y);
        }

        /// <summary>
        /// Checks if <paramref name="x"/> and <paramref name="y"/> are within array bounds
        /// </summary>
        public static bool IsInBounds<T>(this T[,] array, int x, int y)
        {
            if (array == null) throw new ArgumentNullException(nameof(array));
            return x >= 0 && x < array.GetLength(0) && y >= 0 && y < array.GetLength(1);
        }
    }
}
