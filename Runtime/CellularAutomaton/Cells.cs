using System;
using Unity.Collections;
using Random = UnityEngine.Random;

namespace ProceduralToolkit.CellularAutomaton
{
    public struct Cells : IDisposable
    {
        public NativeArray2D<bool> cells;
        public NativeArray2D<bool> copy;

        public Cells(Config config)
        {
            cells = new NativeArray2D<bool>(config.width, config.height, Allocator.Persistent);
            copy = new NativeArray2D<bool>(config.width, config.height, Allocator.Persistent);

            FillWithNoise(config.startNoise);
        }

        public void Dispose()
        {
            if (cells.IsCreated)
            {
                cells.Dispose();
            }
            if (copy.IsCreated)
            {
                copy.Dispose();
            }
        }

        public void FillWithNoise(float noise)
        {
            for (int y = 0; y < cells.LengthY; y++)
            {
                for (int x = 0; x < cells.LengthX; x++)
                {
                    cells[x, y] = copy[x, y] = Random.value < noise;
                }
            }
        }
    }
}
