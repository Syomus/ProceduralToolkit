using Unity.Collections;
using UnityEngine;

namespace ProceduralToolkit.MarchingSquares
{
    public struct ColorData : IMarchingSquaresData
    {
        public int dataLengthX { get; }
        public int dataLengthY { get; }
        public bool useInterpolation { get; }

        private NativeArray<Color> data;
        private readonly float threshold;

        public ColorData(Color[] data, int dataLengthX, float threshold, bool useInterpolation)
        {
            this.data = new NativeArray<Color>(data, Allocator.Persistent);
            this.dataLengthX = dataLengthX;
            dataLengthY = data.Length/dataLengthX;
            this.threshold = threshold;
            this.useInterpolation = useInterpolation;
        }

        public void Dispose()
        {
            data.Dispose();
        }

        private Color GetValue(int x, int y)
        {
            return data.GetXY(x, y, dataLengthX);
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
            return TestValue(GetValue(x, y));
        }

        public bool TestAverage(int x, int y)
        {
            return TestAverage(GetValue(x, y), GetValue(x, y + 1), GetValue(x + 1, y + 1), GetValue(x + 1, y));
        }

        public float InverseLerp(int xa, int ya, int xb, int yb)
        {
            return Mathf.InverseLerp(GetValue(xa, ya).grayscale, GetValue(xb, yb).grayscale, threshold);
        }
    }
}
