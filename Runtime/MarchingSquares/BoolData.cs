using Unity.Collections;

namespace ProceduralToolkit.MarchingSquares
{
    public struct BoolData : IMarchingSquaresData
    {
        public int dataLengthX => data.LengthX;
        public int dataLengthY => data.LengthY;
        public bool useInterpolation => false;

        private NativeArray2D<bool> data;

        public BoolData(bool[,] data)
        {
            this.data = new NativeArray2D<bool>(data, Allocator.Persistent);
        }

        public void Dispose()
        {
            data.Dispose();
        }

        public bool TestValue(int x, int y)
        {
            return data[x, y];
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
