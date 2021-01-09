using Unity.Collections;
using UnityEngine;

namespace ProceduralToolkit.MarchingSquares
{
    public struct ColorData : IMarchingSquaresData
    {
        public int dataLengthX => data.LengthX;
        public int dataLengthY => data.LengthY;
        public bool useInterpolation { get; }

        private NativeArray2D<Color> data;
        private readonly float threshold;

        public ColorData(Color[] data, int dataLengthX, float threshold, bool useInterpolation)
        {
            this.data = new NativeArray2D<Color>(data, dataLengthX, data.Length/dataLengthX, Allocator.Persistent);
            this.threshold = threshold;
            this.useInterpolation = useInterpolation;
        }

        public void Dispose()
        {
            data.Dispose();
        }

        private bool TestValue(Color value)
        {
            return value.grayscale > threshold;
        }

        private bool TestAverage(Color a, Color b, Color c, Color d)
        {
            return TestValue((a + b + c + d)/4);
        }

        public bool TestValue(int x, int y)
        {
            return TestValue(data[x, y]);
        }

        public bool TestAverage(int x, int y)
        {
            return TestAverage(data[x, y], data[x, y + 1], data[x + 1, y + 1], data[x + 1, y]);
        }

        public float InverseLerp(int xa, int ya, int xb, int yb)
        {
            return Mathf.InverseLerp(data[xa, ya].grayscale, data[xb, yb].grayscale, threshold);
        }
    }
}
