using System;
using System.Collections.Generic;

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
        public static T GetLooped<T>(this T[] array, int index)
        {
            while (index < 0)
            {
                index += array.Length;
            }
            if (index >= array.Length)
            {
                index %= array.Length;
            }
            return array[index];
        }

        /// <summary>
        /// Looped indexer setter, allows out of bounds indices
        /// </summary>
        public static void SetLooped<T>(this T[] array, int index, T value)
        {
            while (index < 0)
            {
                index += array.Length;
            }
            if (index >= array.Length)
            {
                index %= array.Length;
            }
            array[index] = value;
        }

        /// <summary>
        /// Looped indexer getter, allows out of bounds indices
        /// </summary>
        public static T GetLooped<T>(this List<T> array, int index)
        {
            while (index < 0)
            {
                index += array.Count;
            }
            if (index >= array.Count)
            {
                index %= array.Count;
            }
            return array[index];
        }

        /// <summary>
        /// Looped indexer setter, allows out of bounds indices
        /// </summary>
        public static void SetLooped<T>(this List<T> array, int index, T value)
        {
            while (index < 0)
            {
                index += array.Count;
            }
            if (index >= array.Count)
            {
                index %= array.Count;
            }
            array[index] = value;
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
            if (array == null) throw new ArgumentNullException("array");
            return x >= 0 && x < array.GetLength(0) && y >= 0 && y < array.GetLength(1);
        }

        #region FloodVisit4

        /// <summary>
        /// Visits all connected elements with the same value as start element
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
        /// Visits all connected elements with the same value as start element
        /// </summary>
        /// <remarks>
        /// https://en.wikipedia.org/wiki/Flood_fill
        /// </remarks>
        public static void FloodVisit4<T>(this T[,] array, int startX, int startY, Action<int, int> visit,
            IEqualityComparer<T> comparer = null)
        {
            if (array == null) throw new ArgumentNullException("array");
            if (visit == null) throw new ArgumentNullException("visit");

            int lengthX = array.GetLength(0);
            int lengthY = array.GetLength(1);

            if (startX < 0 || startX >= lengthX) throw new ArgumentOutOfRangeException("startX");
            if (startY < 0 || startY >= lengthY) throw new ArgumentOutOfRangeException("startY");

            if (comparer == null)
            {
                comparer = EqualityComparer<T>.Default;
            }

            bool[,] processed = new bool[lengthX, lengthY];
            T value = array[startX, startY];

            var queue = new Queue<Vector2Int>();
            queue.Enqueue(new Vector2Int(startX, startY));
            processed[startX, startY] = true;

            Action<int, int> processNeighbours = (x, y) =>
            {
                if (x >= 0 && x < lengthX &&
                    y >= 0 && y < lengthY &&
                    !processed[x, y])
                {
                    if (comparer.Equals(array[x, y], value))
                    {
                        queue.Enqueue(new Vector2Int(x, y));
                    }
                    processed[x, y] = true;
                }
            };

            while (queue.Count > 0)
            {
                Vector2Int cell = queue.Dequeue();
                array.Visit4Unbounded(cell.x, cell.y, processNeighbours);
                visit(cell.x, cell.y);
            }
        }

        /// <summary>
        /// Visits all connected elements with the same value as start element
        /// </summary>
        /// <remarks>
        /// https://en.wikipedia.org/wiki/Flood_fill
        /// </remarks>
        public static void FloodVisit4<T>(this T[,] array, Vector2Int start, Action<int, int, bool> visit,
            IEqualityComparer<T> comparer = null)
        {
            FloodVisit4(array, start.x, start.y, visit, comparer);
        }

        /// <summary>
        /// Visits all connected elements with the same value as start element
        /// </summary>
        /// <remarks>
        /// https://en.wikipedia.org/wiki/Flood_fill
        /// </remarks>
        public static void FloodVisit4<T>(this T[,] array, int startX, int startY, Action<int, int, bool> visit,
            IEqualityComparer<T> comparer = null)
        {
            if (array == null) throw new ArgumentNullException("array");
            if (visit == null) throw new ArgumentNullException("visit");

            int lengthX = array.GetLength(0);
            int lengthY = array.GetLength(1);

            if (startX < 0 || startX >= lengthX) throw new ArgumentOutOfRangeException("startX");
            if (startY < 0 || startY >= lengthY) throw new ArgumentOutOfRangeException("startY");

            if (comparer == null)
            {
                comparer = EqualityComparer<T>.Default;
            }

            bool[,] processed = new bool[lengthX, lengthY];
            T value = array[startX, startY];

            var queue = new Queue<Vector2Int>();
            queue.Enqueue(new Vector2Int(startX, startY));
            processed[startX, startY] = true;

            Vector2Int cell = new Vector2Int();
            bool isBorderCell = false;

            Action<int, int> processNeighbours = (x, y) =>
            {
                if (x >= 0 && x < lengthX &&
                    y >= 0 && y < lengthY &&
                    comparer.Equals(array[x, y], value))
                {
                    if (!processed[x, y])
                    {
                        bool vonNeumannNeighbour = x == cell.x || y == cell.y;
                        if (vonNeumannNeighbour)
                        {
                            queue.Enqueue(new Vector2Int(x, y));
                        }
                        processed[x, y] = true;
                    }
                }
                else
                {
                    isBorderCell = true;
                }
            };

            while (queue.Count > 0)
            {
                cell = queue.Dequeue();
                isBorderCell = false;
                array.Visit8Unbounded(cell.x, cell.y, processNeighbours);
                visit(cell.x, cell.y, isBorderCell);
            }
        }

        #endregion FloodVisit4

        #region Visit4

        /// <summary>
        /// Visits four cells orthogonally surrounding a central cell
        /// </summary>
        /// <remarks>
        /// https://en.wikipedia.org/wiki/Von_Neumann_neighborhood
        /// </remarks>
        public static void Visit4<T>(this T[,] array, Vector2Int center, Action<int, int> visit)
        {
            Visit4(array, center.x, center.y, visit);
        }

        /// <summary>
        /// Visits four cells orthogonally surrounding a central cell
        /// </summary>
        /// <remarks>
        /// https://en.wikipedia.org/wiki/Von_Neumann_neighborhood
        /// </remarks>
        public static void Visit4<T>(this T[,] array, int x, int y, Action<int, int> visit)
        {
            if (array == null) throw new ArgumentNullException("array");
            if (visit == null) throw new ArgumentNullException("visit");

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
        /// Visits four cells orthogonally surrounding a central cell
        /// </summary>
        /// <remarks>
        /// https://en.wikipedia.org/wiki/Von_Neumann_neighborhood
        /// </remarks>
        public static void Visit4Unbounded<T>(this T[,] array, Vector2Int center, Action<int, int> visit)
        {
            Visit4Unbounded(array, center.x, center.y, visit);
        }

        /// <summary>
        /// Visits four cells orthogonally surrounding a central cell
        /// </summary>
        /// <remarks>
        /// https://en.wikipedia.org/wiki/Von_Neumann_neighborhood
        /// </remarks>
        public static void Visit4Unbounded<T>(this T[,] array, int x, int y, Action<int, int> visit)
        {
            if (array == null) throw new ArgumentNullException("array");
            if (visit == null) throw new ArgumentNullException("visit");

            visit(x - 1, y);
            visit(x + 1, y);
            visit(x, y - 1);
            visit(x, y + 1);
        }

        #endregion Visit4

        #region Visit8

        /// <summary>
        /// Visits eight cells surrounding a central cell
        /// </summary>
        /// <remarks>
        /// https://en.wikipedia.org/wiki/Moore_neighborhood
        /// </remarks>
        public static void Visit8<T>(this T[,] array, Vector2Int center, Action<int, int> visit)
        {
            Visit8(array, center.x, center.y, visit);
        }

        /// <summary>
        /// Visits eight cells surrounding a central cell
        /// </summary>
        /// <remarks>
        /// https://en.wikipedia.org/wiki/Moore_neighborhood
        /// </remarks>
        public static void Visit8<T>(this T[,] array, int x, int y, Action<int, int> visit)
        {
            if (array == null) throw new ArgumentNullException("array");
            if (visit == null) throw new ArgumentNullException("visit");

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
        /// Visits eight cells surrounding a central cell
        /// </summary>
        /// <remarks>
        /// https://en.wikipedia.org/wiki/Moore_neighborhood
        /// </remarks>
        public static void Visit8Unbounded<T>(this T[,] array, Vector2Int center, Action<int, int> visit)
        {
            Visit8Unbounded(array, center.x, center.y, visit);
        }

        /// <summary>
        /// Visits eight cells surrounding a central cell
        /// </summary>
        /// <remarks>
        /// https://en.wikipedia.org/wiki/Moore_neighborhood
        /// </remarks>
        public static void Visit8Unbounded<T>(this T[,] array, int x, int y, Action<int, int> visit)
        {
            if (array == null) throw new ArgumentNullException("array");
            if (visit == null) throw new ArgumentNullException("visit");

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