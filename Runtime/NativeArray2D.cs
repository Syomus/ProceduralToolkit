using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace ProceduralToolkit
{
    public struct NativeArray2D<T> : IDisposable, IEnumerable<T>, IEquatable<NativeArray2D<T>>
        where T : struct
    {
        public int Length { get; }
        public int LengthX { get; }
        public int LengthY { get; }

        public T this[int index]
        {
            get => array[index];
            set => array[index] = value;
        }

        public T this[int x, int y]
        {
            get => array.GetXY(x, y, LengthX);
            set => array.SetXY(x, y, LengthX, value);
        }

        public T this[Vector2Int position]
        {
            get => this[position.x, position.y];
            set => this[position.x, position.y] = value;
        }

        private NativeArray<T> array;

        public NativeArray2D(int lengthX, int lengthY, Allocator allocator, NativeArrayOptions options = NativeArrayOptions.ClearMemory)
        {
            LengthX = lengthX;
            LengthY = lengthY;
            Length = LengthX*LengthY;
            array = new NativeArray<T>(Length, allocator, options);
        }

        public NativeArray2D(T[,] array, Allocator allocator)
        {
            LengthX = array.GetLength(0);
            LengthY = array.GetLength(1);
            Length = LengthX*LengthY;
            this.array = new NativeArray<T>(Length, allocator, NativeArrayOptions.UninitializedMemory);
            for (int y = 0; y < LengthY; y++)
            {
                for (int x = 0; x < LengthX; x++)
                {
                    this[x, y] = array[x, y];
                }
            }
        }

        public NativeArray2D(T[] array, int lengthX, int lengthY, Allocator allocator)
        {
            LengthX = lengthX;
            LengthY = lengthY;
            Length = LengthX*LengthY;
            this.array = new NativeArray<T>(array, allocator);
        }

        public NativeArray2D(NativeArray<T> array, int lengthX, int lengthY, Allocator allocator)
        {
            LengthX = lengthX;
            LengthY = lengthY;
            Length = LengthX*LengthY;
            this.array = new NativeArray<T>(array, allocator);
        }

        /// <summary>
        /// Checks if <paramref name="position"/> is within array bounds
        /// </summary>
        public bool IsInBounds(Vector2Int position)
        {
            return IsInBounds(position.x, position.y);
        }

        /// <summary>
        /// Checks if <paramref name="x"/> and <paramref name="y"/> are within array bounds
        /// </summary>
        public bool IsInBounds(int x, int y)
        {
            return x >= 0 && x < LengthX && y >= 0 && y < LengthY;
        }

        #region FloodVisit4

        /// <summary>
        /// Visits all connected elements with the same value as the start element
        /// </summary>
        /// <remarks>
        /// https://en.wikipedia.org/wiki/Flood_fill
        /// </remarks>
        public void FloodVisit4(Vector2Int start, Action<int, int> visit, IEqualityComparer<T> comparer = null)
        {
            FloodVisit4(start.x, start.y, visit, comparer);
        }

        /// <summary>
        /// Visits all connected elements with the same value as the start element
        /// </summary>
        /// <remarks>
        /// https://en.wikipedia.org/wiki/Flood_fill
        /// </remarks>
        public void FloodVisit4(int startX, int startY, Action<int, int> visit, IEqualityComparer<T> comparer = null)
        {
            if (visit == null) throw new ArgumentNullException(nameof(visit));

            int lengthX = LengthX;
            int lengthY = LengthY;

            if (startX < 0 || startX >= lengthX) throw new ArgumentOutOfRangeException(nameof(startX));
            if (startY < 0 || startY >= lengthY) throw new ArgumentOutOfRangeException(nameof(startY));

            if (comparer == null)
            {
                comparer = EqualityComparer<T>.Default;
            }

            bool[,] processed = new bool[lengthX, lengthY];
            T value = this[startX, startY];

            var queue = new Queue<Vector2Int>();
            queue.Enqueue(new Vector2Int(startX, startY));
            processed[startX, startY] = true;

            while (queue.Count > 0)
            {
                Vector2Int cell = queue.Dequeue();

                if (cell.x > 0)
                {
                    Process(processed, cell.x - 1, cell.y, comparer, value, queue);
                }
                if (cell.x + 1 < lengthX)
                {
                    Process(processed, cell.x + 1, cell.y, comparer, value, queue);
                }
                if (cell.y > 0)
                {
                    Process(processed, cell.x, cell.y - 1, comparer, value, queue);
                }
                if (cell.y + 1 < lengthY)
                {
                    Process(processed, cell.x, cell.y + 1, comparer, value, queue);
                }

                visit(cell.x, cell.y);
            }
        }

        private void Process(bool[,] processed, int x, int y, IEqualityComparer<T> comparer, T value, Queue<Vector2Int> queue)
        {
            if (!processed[x, y])
            {
                if (comparer.Equals(this[x, y], value))
                {
                    queue.Enqueue(new Vector2Int(x, y));
                }
                processed[x, y] = true;
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
        public void FloodVisit8(Vector2Int start, Action<int, int> visit, IEqualityComparer<T> comparer = null)
        {
            FloodVisit8(start.x, start.y, visit, comparer);
        }

        /// <summary>
        /// Visits all connected elements with the same value as the start element
        /// </summary>
        /// <remarks>
        /// https://en.wikipedia.org/wiki/Flood_fill
        /// </remarks>
        public void FloodVisit8(int startX, int startY, Action<int, int> visit, IEqualityComparer<T> comparer = null)
        {
            if (visit == null) throw new ArgumentNullException(nameof(visit));

            if (startX < 0 || startX >= LengthX) throw new ArgumentOutOfRangeException(nameof(startX));
            if (startY < 0 || startY >= LengthY) throw new ArgumentOutOfRangeException(nameof(startY));

            if (comparer == null)
            {
                comparer = EqualityComparer<T>.Default;
            }

            bool[,] processed = new bool[LengthX, LengthY];
            T value = this[startX, startY];

            var queue = new Queue<Vector2Int>();
            queue.Enqueue(new Vector2Int(startX, startY));
            processed[startX, startY] = true;

            while (queue.Count > 0)
            {
                Vector2Int cell = queue.Dequeue();

                bool xGreaterThanZero = cell.x > 0;
                bool xLessThanWidth = cell.x + 1 < LengthX;

                bool yGreaterThanZero = cell.y > 0;
                bool yLessThanHeight = cell.y + 1 < LengthY;

                if (yGreaterThanZero)
                {
                    if (xGreaterThanZero) Process(processed, cell.x - 1, cell.y - 1, comparer, value, queue);

                    Process(processed, cell.x, cell.y - 1, comparer, value, queue);

                    if (xLessThanWidth) Process(processed, cell.x + 1, cell.y - 1, comparer, value, queue);
                }

                if (xGreaterThanZero) Process(processed, cell.x - 1, cell.y, comparer, value, queue);
                if (xLessThanWidth) Process(processed, cell.x + 1, cell.y, comparer, value, queue);

                if (yLessThanHeight)
                {
                    if (xGreaterThanZero) Process(processed, cell.x - 1, cell.y + 1, comparer, value, queue);

                    Process(processed, cell.x, cell.y + 1, comparer, value, queue);

                    if (xLessThanWidth) Process(processed, cell.x + 1, cell.y + 1, comparer, value, queue);
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
        public void Visit4(Vector2Int center, Action<int, int> visit)
        {
            Visit4(center.x, center.y, visit);
        }

        /// <summary>
        /// Visits four cells orthogonally surrounding the center cell
        /// </summary>
        /// <remarks>
        /// https://en.wikipedia.org/wiki/Von_Neumann_neighborhood
        /// </remarks>
        public void Visit4(int x, int y, Action<int, int> visit)
        {
            if (visit == null) throw new ArgumentNullException(nameof(visit));

            if (x > 0)
            {
                visit(x - 1, y);
            }
            if (x + 1 < LengthX)
            {
                visit(x + 1, y);
            }
            if (y > 0)
            {
                visit(x, y - 1);
            }
            if (y + 1 < LengthY)
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
        public void Visit4Unbounded(Vector2Int center, Action<int, int> visit)
        {
            Visit4Unbounded(center.x, center.y, visit);
        }

        /// <summary>
        /// Visits four cells orthogonally surrounding the center cell
        /// </summary>
        /// <remarks>
        /// https://en.wikipedia.org/wiki/Von_Neumann_neighborhood
        /// </remarks>
        public void Visit4Unbounded(int x, int y, Action<int, int> visit)
        {
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
        public void Visit8(Vector2Int center, Action<int, int> visit)
        {
            Visit8(center.x, center.y, visit);
        }

        /// <summary>
        /// Visits eight cells surrounding the center cell
        /// </summary>
        /// <remarks>
        /// https://en.wikipedia.org/wiki/Moore_neighborhood
        /// </remarks>
        public void Visit8(int x, int y, Action<int, int> visit)
        {
            if (visit == null) throw new ArgumentNullException(nameof(visit));

            bool xGreaterThanZero = x > 0;
            bool xLessThanWidth = x + 1 < LengthX;

            bool yGreaterThanZero = y > 0;
            bool yLessThanHeight = y + 1 < LengthY;

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
        public void Visit8Unbounded(Vector2Int center, Action<int, int> visit)
        {
            Visit8Unbounded(center.x, center.y, visit);
        }

        /// <summary>
        /// Visits eight cells surrounding the center cell
        /// </summary>
        /// <remarks>
        /// https://en.wikipedia.org/wiki/Moore_neighborhood
        /// </remarks>
        public void Visit8Unbounded(int x, int y, Action<int, int> visit)
        {
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

        public void Dispose() => array.Dispose();
        public void Dispose(JobHandle inputDeps) => array.Dispose(inputDeps);
        public IEnumerator<T> GetEnumerator() => array.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => array.GetEnumerator();

        public bool Equals(NativeArray2D<T> other)
        {
            return array.Equals(other.array);
        }

        public override bool Equals(object obj)
        {
            return obj != null && obj is NativeArray2D<T> other && Equals(other);
        }

        public override int GetHashCode() => array.GetHashCode();
    }
}
