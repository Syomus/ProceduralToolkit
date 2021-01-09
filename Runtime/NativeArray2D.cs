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
