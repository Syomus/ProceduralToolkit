using System;

namespace ProceduralToolkit.MarchingSquares
{
    /// <summary>
    /// An interface for working with grids
    /// </summary>
    public interface IMarchingSquaresData : IDisposable
    {
        int dataLengthX { get; }
        int dataLengthY { get; }
        bool useInterpolation { get; }
        bool TestValue(int x, int y);
        bool TestAverage(int x, int y);
        float InverseLerp(int xa, int ya, int xb, int yb);
    }
}
