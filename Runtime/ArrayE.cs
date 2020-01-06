using System;
using System.Collections.Generic;
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
        /// Looped indexer getter, allows out of bounds indices
        /// </summary>
        public static T GetLooped<T>(this IList<T> list, int index)
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
        /// Checks if <paramref name="vector"/> is within array bounds
        /// </summary>
        public static bool IsInBounds<T>(this T[,] array, Vector2Int vector)
        {
            return IsInBounds(array, vector.x, vector.y);
        }

        /// <summary>
        /// Checks if <paramref name="x"/> and <paramref name="y"/> are within array bounds
        /// </summary>
        public static bool IsInBounds<T>(this T[,] array, int x, int y)
        {
            if (array == null) throw new ArgumentNullException(nameof(array));
            return x >= 0 && x < array.GetLength(0) && y >= 0 && y < array.GetLength(1);
        }

        #region FloodVisit4

        /// <summary>
        /// Visits all connected elements with the same value as the start element
        /// </summary>
        /// <remarks>
        /// https://en.wikipedia.org/wiki/Flood_fill
        /// </remarks>
        public static void FloodVisit4<T>(this T[,] array, Vector2Int start, Action<int, int> visit,
            IEqualityComparer<T> comparer = null)
        {
            FloodVisit4(array, start.x, start.y, visit, comparer);
        }

        /// <summary>
        /// Visits all connected elements with the same value as the start element
        /// </summary>
        /// <remarks>
        /// https://en.wikipedia.org/wiki/Flood_fill
        /// </remarks>
        public static void FloodVisit4<T>(this T[,] array, int startX, int startY, Action<int, int> visit,
            IEqualityComparer<T> comparer = null)
        {
            if (array == null) throw new ArgumentNullException(nameof(array));
            if (visit == null) throw new ArgumentNullException(nameof(visit));

            int lengthX = array.GetLength(0);
            int lengthY = array.GetLength(1);

            if (startX < 0 || startX >= lengthX) throw new ArgumentOutOfRangeException(nameof(startX));
            if (startY < 0 || startY >= lengthY) throw new ArgumentOutOfRangeException(nameof(startY));

            if (comparer == null)
            {
                comparer = EqualityComparer<T>.Default;
            }

            bool[,] processed = new bool[lengthX, lengthY];
            T value = array[startX, startY];

            var queue = new Queue<Vector2Int>();
            queue.Enqueue(new Vector2Int(startX, startY));
            processed[startX, startY] = true;

            void process(int x, int y)
            {
                if (!processed[x, y])
                {
                    if (comparer.Equals(array[x, y], value))
                    {
                        queue.Enqueue(new Vector2Int(x, y));
                    }
                    processed[x, y] = true;
                }
            }

            while (queue.Count > 0)
            {
                Vector2Int cell = queue.Dequeue();

                if (cell.x > 0)
                {
                    process(cell.x - 1, cell.y);
                }
                if (cell.x + 1 < lengthX)
                {
                    process(cell.x + 1, cell.y);
                }
                if (cell.y > 0)
                {
                    process(cell.x, cell.y - 1);
                }
                if (cell.y + 1 < lengthY)
                {
                    process(cell.x, cell.y + 1);
                }

                visit(cell.x, cell.y);
            }
        }

        #endregion FloodVisit4

        #region FloodVisit8

        /// <summary>
        /// Visits all connected elements with the same value as the start element
        /// </summary>
        /// <remarks>
        /// https://en.wikipedia.org/wiki/Flood_fill
        /// </remarks>
        public static void FloodVisit8<T>(this T[,] array, Vector2Int start, Action<int, int> visit,
            IEqualityComparer<T> comparer = null)
        {
            FloodVisit4(array, start.x, start.y, visit, comparer);
        }

        /// <summary>
        /// Visits all connected elements with the same value as the start element
        /// </summary>
        /// <remarks>
        /// https://en.wikipedia.org/wiki/Flood_fill
        /// </remarks>
        public static void FloodVisit8<T>(this T[,] array, int startX, int startY, Action<int, int> visit,
            IEqualityComparer<T> comparer = null)
        {
            if (array == null) throw new ArgumentNullException(nameof(array));
            if (visit == null) throw new ArgumentNullException(nameof(visit));

            int lengthX = array.GetLength(0);
            int lengthY = array.GetLength(1);

            if (startX < 0 || startX >= lengthX) throw new ArgumentOutOfRangeException(nameof(startX));
            if (startY < 0 || startY >= lengthY) throw new ArgumentOutOfRangeException(nameof(startY));

            if (comparer == null)
            {
                comparer = EqualityComparer<T>.Default;
            }

            bool[,] processed = new bool[lengthX, lengthY];
            T value = array[startX, startY];

            var queue = new Queue<Vector2Int>();
            queue.Enqueue(new Vector2Int(startX, startY));
            processed[startX, startY] = true;

            void process(int x, int y)
            {
                if (!processed[x, y])
                {
                    if (comparer.Equals(array[x, y], value))
                    {
                        queue.Enqueue(new Vector2Int(x, y));
                    }
                    processed[x, y] = true;
                }
            }

            while (queue.Count > 0)
            {
                Vector2Int cell = queue.Dequeue();

                bool xGreaterThanZero = cell.x > 0;
                bool xLessThanWidth = cell.x + 1 < lengthX;

                bool yGreaterThanZero = cell.y > 0;
                bool yLessThanHeight = cell.y + 1 < lengthY;

                if (yGreaterThanZero)
                {
                    if (xGreaterThanZero) process(cell.x - 1, cell.y - 1);

                    process(cell.x, cell.y - 1);

                    if (xLessThanWidth) process(cell.x + 1, cell.y - 1);
                }

                if (xGreaterThanZero) process(cell.x - 1, cell.y);
                if (xLessThanWidth) process(cell.x + 1, cell.y);

                if (yLessThanHeight)
                {
                    if (xGreaterThanZero) process(cell.x - 1, cell.y + 1);

                    process(cell.x, cell.y + 1);

                    if (xLessThanWidth) process(cell.x + 1, cell.y + 1);
                }

                visit(cell.x, cell.y);
            }
        }

        #endregion FloodVisit8

        #region Visit4

        /// <summary>
        /// Visits four cells orthogonally surrounding the center cell
        /// </summary>
        /// <remarks>
        /// https://en.wikipedia.org/wiki/Von_Neumann_neighborhood
        /// </remarks>
        public static void Visit4<T>(this T[,] array, Vector2Int center, Action<int, int> visit)
        {
            Visit4(array, center.x, center.y, visit);
        }

        /// <summary>
        /// Visits four cells orthogonally surrounding the center cell
        /// </summary>
        /// <remarks>
        /// https://en.wikipedia.org/wiki/Von_Neumann_neighborhood
        /// </remarks>
        public static void Visit4<T>(this T[,] array, int x, int y, Action<int, int> visit)
        {
            if (array == null) throw new ArgumentNullException(nameof(array));
            if (visit == null) throw new ArgumentNullException(nameof(visit));

            if (x > 0)
            {
                visit(x - 1, y);
            }
            if (x + 1 < array.GetLength(0))
            {
                visit(x + 1, y);
            }
            if (y > 0)
            {
                visit(x, y - 1);
            }
            if (y + 1 < array.GetLength(1))
            {
                visit(x, y + 1);
            }
        }

        /// <summary>
        /// Visits four cells orthogonally surrounding the center cell
        /// </summary>
        /// <remarks>
        /// https://en.wikipedia.org/wiki/Von_Neumann_neighborhood
        /// </remarks>
        public static void Visit4Unbounded<T>(this T[,] array, Vector2Int center, Action<int, int> visit)
        {
            Visit4Unbounded(array, center.x, center.y, visit);
        }

        /// <summary>
        /// Visits four cells orthogonally surrounding the center cell
        /// </summary>
        /// <remarks>
        /// https://en.wikipedia.org/wiki/Von_Neumann_neighborhood
        /// </remarks>
        public static void Visit4Unbounded<T>(this T[,] array, int x, int y, Action<int, int> visit)
        {
            if (array == null) throw new ArgumentNullException(nameof(array));
            if (visit == null) throw new ArgumentNullException(nameof(visit));

            visit(x - 1, y);
            visit(x + 1, y);
            visit(x, y - 1);
            visit(x, y + 1);
        }

        #endregion Visit4

        #region Visit8

        /// <summary>
        /// Visits eight cells surrounding the center cell
        /// </summary>
        /// <remarks>
        /// https://en.wikipedia.org/wiki/Moore_neighborhood
        /// </remarks>
        public static void Visit8<T>(this T[,] array, Vector2Int center, Action<int, int> visit)
        {
            Visit8(array, center.x, center.y, visit);
        }

        /// <summary>
        /// Visits eight cells surrounding the center cell
        /// </summary>
        /// <remarks>
        /// https://en.wikipedia.org/wiki/Moore_neighborhood
        /// </remarks>
        public static void Visit8<T>(this T[,] array, int x, int y, Action<int, int> visit)
        {
            if (array == null) throw new ArgumentNullException(nameof(array));
            if (visit == null) throw new ArgumentNullException(nameof(visit));

            bool xGreaterThanZero = x > 0;
            bool xLessThanWidth = x + 1 < array.GetLength(0);

            bool yGreaterThanZero = y > 0;
            bool yLessThanHeight = y + 1 < array.GetLength(1);

            if (yGreaterThanZero)
            {
                if (xGreaterThanZero) visit(x - 1, y - 1);

                visit(x, y - 1);

                if (xLessThanWidth) visit(x + 1, y - 1);
            }

            if (xGreaterThanZero) visit(x - 1, y);
            if (xLessThanWidth) visit(x + 1, y);

            if (yLessThanHeight)
            {
                if (xGreaterThanZero) visit(x - 1, y + 1);

                visit(x, y + 1);

                if (xLessThanWidth) visit(x + 1, y + 1);
            }
        }

        /// <summary>
        /// Visits eight cells surrounding the center cell
        /// </summary>
        /// <remarks>
        /// https://en.wikipedia.org/wiki/Moore_neighborhood
        /// </remarks>
        public static void Visit8Unbounded<T>(this T[,] array, Vector2Int center, Action<int, int> visit)
        {
            Visit8Unbounded(array, center.x, center.y, visit);
        }

        /// <summary>
        /// Visits eight cells surrounding the center cell
        /// </summary>
        /// <remarks>
        /// https://en.wikipedia.org/wiki/Moore_neighborhood
        /// </remarks>
        public static void Visit8Unbounded<T>(this T[,] array, int x, int y, Action<int, int> visit)
        {
            if (array == null) throw new ArgumentNullException(nameof(array));
            if (visit == null) throw new ArgumentNullException(nameof(visit));

            visit(x - 1, y - 1);
            visit(x, y - 1);
            visit(x + 1, y - 1);

            visit(x - 1, y);
            visit(x + 1, y);

            visit(x - 1, y + 1);
            visit(x, y + 1);
            visit(x + 1, y + 1);
        }

        #endregion Visit8
    }
}
