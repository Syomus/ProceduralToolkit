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
            if (index < 0)
            {
                index = index%array.Length + array.Length;
            }
            else if (index >= array.Length)
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
            if (index < 0)
            {
                index = index%array.Length + array.Length;
            }
            else if (index >= array.Length)
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
            if (index < 0)
            {
                index = index%array.Count + array.Count;
            }
            else if (index >= array.Count)
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
            if (index < 0)
            {
                index = index%array.Count + array.Count;
            }
            else if (index >= array.Count)
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
            if (array == null)
            {
                throw new ArgumentNullException("array");
            }
            return x >= 0 && x < array.GetLength(0) && y >= 0 && y < array.GetLength(1);
        }

        /// <summary>
        /// Visits all connected elements with the same value as start element
        /// </summary>
        /// <remarks>
        /// https://en.wikipedia.org/wiki/Flood_fill
        /// </remarks>
        public static void FloodVisit<T>(this T[,] array, Vector2Int start, Action<int, int> visit,
            IEqualityComparer<T> comparer = null)
        {
            FloodVisit(array, start.x, start.y, visit, comparer);
        }

        /// <summary>
        /// Visits all connected elements with the same value as start element
        /// </summary>
        /// <remarks>
        /// https://en.wikipedia.org/wiki/Flood_fill
        /// </remarks>
        public static void FloodVisit<T>(this T[,] array, int startX, int startY, Action<int, int> visit,
            IEqualityComparer<T> comparer = null)
        {
            if (array == null)
            {
                throw new ArgumentNullException("array");
            }
            if (visit == null)
            {
                throw new ArgumentNullException("visit");
            }
            int lengthX = array.GetLength(0);
            if (startX < 0 || startX >= lengthX)
            {
                throw new ArgumentOutOfRangeException("startX");
            }
            int lengthY = array.GetLength(1);
            if (startY < 0 || startY >= lengthY)
            {
                throw new ArgumentOutOfRangeException("startY");
            }

            if (comparer == null)
            {
                comparer = EqualityComparer<T>.Default;
            }

            bool[,] processed = new bool[lengthX, lengthY];
            T value = array[startX, startY];

            var queue = new Queue<Vector2Int>();
            queue.Enqueue(new Vector2Int(startX, startY));
            processed[startX, startY] = true;

            while (queue.Count > 0)
            {
                Vector2Int cell = queue.Dequeue();

                array.VonNeumannVisit(cell.x, cell.y, true, (x, y) =>
                {
                    if (comparer.Equals(array[x, y], value) && !processed[x, y])
                    {
                        queue.Enqueue(new Vector2Int(x, y));
                        processed[x, y] = true;
                    }
                });

                visit(cell.x, cell.y);
            }
        }

        /// <summary>
        /// Visits all connected elements with the same value as start element
        /// </summary>
        /// <remarks>
        /// https://en.wikipedia.org/wiki/Flood_fill
        /// </remarks>
        public static void FloodVisit<T>(this T[,] array, Vector2Int start, Action<int, int, bool> visit,
            IEqualityComparer<T> comparer = null)
        {
            FloodVisit(array, start.x, start.y, visit, comparer);
        }

        /// <summary>
        /// Visits all connected elements with the same value as start element
        /// </summary>
        /// <remarks>
        /// https://en.wikipedia.org/wiki/Flood_fill
        /// </remarks>
        public static void FloodVisit<T>(this T[,] array, int startX, int startY, Action<int, int, bool> visit,
            IEqualityComparer<T> comparer = null)
        {
            if (array == null)
            {
                throw new ArgumentNullException("array");
            }
            if (visit == null)
            {
                throw new ArgumentNullException("visit");
            }
            int lengthX = array.GetLength(0);
            if (startX < 0 || startX >= lengthX)
            {
                throw new ArgumentOutOfRangeException("startX");
            }
            int lengthY = array.GetLength(1);
            if (startY < 0 || startY >= lengthY)
            {
                throw new ArgumentOutOfRangeException("startY");
            }

            if (comparer == null)
            {
                comparer = EqualityComparer<T>.Default;
            }

            bool[,] processed = new bool[lengthX, lengthY];
            T value = array[startX, startY];

            var queue = new Queue<Vector2Int>();
            queue.Enqueue(new Vector2Int(startX, startY));
            processed[startX, startY] = true;

            while (queue.Count > 0)
            {
                Vector2Int cell = queue.Dequeue();

                bool isBorderCell = false;
                array.MooreVisit(cell.x, cell.y, false, (x, y) =>
                {
                    if (x >= 0 && x < lengthX &&
                        y >= 0 && y < lengthY)
                    {
                        if (comparer.Equals(array[x, y], value))
                        {
                            bool vonNeumannNeighbour = (x == cell.x || y == cell.y);
                            if (vonNeumannNeighbour && !processed[x, y])
                            {
                                queue.Enqueue(new Vector2Int(x, y));
                                processed[x, y] = true;
                            }
                        }
                        else
                        {
                            isBorderCell = true;
                        }
                    }
                    else
                    {
                        isBorderCell = true;
                    }
                });

                visit(cell.x, cell.y, isBorderCell);
            }
        }

        /// <summary>
        /// Visits four cells orthogonally surrounding a central cell
        /// </summary>
        /// <remarks>
        /// https://en.wikipedia.org/wiki/Von_Neumann_neighborhood
        /// </remarks>
        public static void VonNeumannVisit<T>(this T[,] array, Vector2Int center, bool checkArrayBounds,
            Action<int, int> visit)
        {
            VonNeumannVisit(array, center.x, center.y, checkArrayBounds, visit);
        }

        /// <summary>
        /// Visits four cells orthogonally surrounding a central cell
        /// </summary>
        /// <remarks>
        /// https://en.wikipedia.org/wiki/Von_Neumann_neighborhood
        /// </remarks>
        public static void VonNeumannVisit<T>(this T[,] array, int x, int y, bool checkArrayBounds,
            Action<int, int> visit)
        {
            if (array == null)
            {
                throw new ArgumentNullException("array");
            }
            if (visit == null)
            {
                throw new ArgumentNullException("visit");
            }

            if (checkArrayBounds)
            {
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
            else
            {
                visit(x - 1, y);
                visit(x + 1, y);
                visit(x, y - 1);
                visit(x, y + 1);
            }
        }

        /// <summary>
        /// Visits eight cells surrounding a central cell
        /// </summary>
        /// <remarks>
        /// https://en.wikipedia.org/wiki/Moore_neighborhood
        /// </remarks>
        public static void MooreVisit<T>(this T[,] array, Vector2Int center, bool checkArrayBounds,
            Action<int, int> visit)
        {
            MooreVisit(array, center.x, center.y, checkArrayBounds, visit);
        }

        /// <summary>
        /// Visits eight cells surrounding a central cell
        /// </summary>
        /// <remarks>
        /// https://en.wikipedia.org/wiki/Moore_neighborhood
        /// </remarks>
        public static void MooreVisit<T>(this T[,] array, int x, int y, bool checkArrayBounds, Action<int, int> visit)
        {
            if (array == null)
            {
                throw new ArgumentNullException("array");
            }
            if (visit == null)
            {
                throw new ArgumentNullException("visit");
            }

            if (checkArrayBounds)
            {
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
            else
            {
                visit(x - 1, y - 1);
                visit(x, y - 1);
                visit(x + 1, y - 1);

                visit(x - 1, y);
                visit(x + 1, y);

                visit(x - 1, y + 1);
                visit(x, y + 1);
                visit(x + 1, y + 1);
            }
        }
    }
}