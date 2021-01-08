using Unity.Collections;

namespace ProceduralToolkit.MarchingSquares
{
    public struct BoolData : IMarchingSquaresData
    {
        public int dataLengthX { get; }
        public int dataLengthY { get; }
        public bool useInterpolation => false;

        private NativeArray<bool> data;

        public BoolData(bool[,] data)
        {
            dataLengthX = data.GetLength(0);
            dataLengthY = data.GetLength(1);
            this.data = new NativeArray<bool>(dataLengthX*dataLengthY, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
            for (int y = 0; y < dataLengthY; y++)
            {
                for (int x = 0; x < dataLengthX; x++)
                {
                    this.data.SetXY(x, y, dataLengthX, data[x, y]);
                }
            }
        }

        public void Dispose()
        {
            data.Dispose();
        }

        private bool GetValue(int x, int y)
        {
            return data.GetXY(x, y, dataLengthX);
        }

        public bool TestValue(int x, int y)
        {
            return GetValue(x, y);
        }

        public bool TestAverage(int x, int y)
        {
            return false;
        }

        public float InverseLerp(int xa, int ya, int xb, int yb)
        {
            throw new System.NotImplementedException();
        }
    }
}
