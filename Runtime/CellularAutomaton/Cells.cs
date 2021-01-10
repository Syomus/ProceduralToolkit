using System;
using Unity.Collections;
using Random = UnityEngine.Random;

namespace ProceduralToolkit.CellularAutomaton
{
    public struct Cells : IDisposable
    {
        public bool this[int x, int y]
        {
            get => swapped[0] ? array1[x, y] : array0[x, y];
            set
            {
                if (swapped[0])
                {
                    array1[x, y] = value;
                }
                else
                {
                    array0[x, y] = value;
                }
            }
        }

        private NativeArray2D<bool> array0;
        private NativeArray2D<bool> array1;
        private NativeArray<bool> swapped;

        public Cells(Config config)
        {
            array0 = new NativeArray2D<bool>(config.width, config.height, Allocator.Persistent);
            array1 = new NativeArray2D<bool>(config.width, config.height, Allocator.Persistent);
            swapped = new NativeArray<bool>(1, Allocator.Persistent);

            FillWithNoise(config.startNoise);
        }

        public void Dispose()
        {
            array0.Dispose();
            array1.Dispose();
            swapped.Dispose();
        }

        public void FillWithNoise(float noise)
        {
            for (int y = 0; y < array0.LengthY; y++)
            {
                for (int x = 0; x < array0.LengthX; x++)
                {
                    array0[x, y] = array1[x, y] = Random.value < noise;
                }
            }
        }

        public void Swap()
        {
            swapped[0] = !swapped[0];
        }

        public NativeArray2D<bool> GetCurrent()
        {
            return swapped[0] ? array1 : array0;
        }

        public NativeArray2D<bool> GetPrevious()
        {
            return swapped[0] ? array0 : array1;
        }

        public bool GetPrevious(int x, int y)
        {
            return swapped[0] ? array0[x, y] : array1[x, y];
        }
    }
}
