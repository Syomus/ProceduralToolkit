using System;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

namespace ProceduralToolkit
{
    /// <summary>
    /// Visits all connected elements with the same value as the start element
    /// </summary>
    /// <remarks>
    /// https://en.wikipedia.org/wiki/Flood_fill
    /// </remarks>
    public struct FloodVisitor8<T, TVisitAction>
        where T : struct
        where TVisitAction : struct, IVisitAction
    {
        public TVisitAction action;

        public NativeArray2D<T> array;

        public FloodVisitor8(NativeArray2D<T> array, TVisitAction action)
        {
            this.array = array;
            this.action = action;
        }

        public void FloodVisit8(Vector2Int start, IEqualityComparer<T> comparer = null)
        {
            FloodVisit8(start.x, start.y, comparer);
        }

        public void FloodVisit8(int startX, int startY, IEqualityComparer<T> comparer = null)
        {
            if (startX < 0 || startX >= array.LengthX) throw new ArgumentOutOfRangeException(nameof(startX));
            if (startY < 0 || startY >= array.LengthY) throw new ArgumentOutOfRangeException(nameof(startY));

            if (comparer == null)
            {
                comparer = EqualityComparer<T>.Default;
            }

            var processed = new NativeArray2D<bool>(array.LengthX, array.LengthY, Allocator.Temp);
            T value = array[startX, startY];

            var queue = new Queue<Vector2Int>();
            queue.Enqueue(new Vector2Int(startX, startY));
            processed[startX, startY] = true;

            void process(ref NativeArray2D<T> array, int x, int y)
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
                bool xLessThanWidth = cell.x + 1 < array.LengthX;

                bool yGreaterThanZero = cell.y > 0;
                bool yLessThanHeight = cell.y + 1 < array.LengthY;

                if (yGreaterThanZero)
                {
                    if (xGreaterThanZero) process(ref array, cell.x - 1, cell.y - 1);

                    process(ref array, cell.x, cell.y - 1);

                    if (xLessThanWidth) process(ref array, cell.x + 1, cell.y - 1);
                }

                if (xGreaterThanZero) process(ref array, cell.x - 1, cell.y);
                if (xLessThanWidth) process(ref array, cell.x + 1, cell.y);

                if (yLessThanHeight)
                {
                    if (xGreaterThanZero) process(ref array, cell.x - 1, cell.y + 1);

                    process(ref array, cell.x, cell.y + 1);

                    if (xLessThanWidth) process(ref array, cell.x + 1, cell.y + 1);
                }

                action.Visit(cell.x, cell.y);
            }
        }
    }
}
