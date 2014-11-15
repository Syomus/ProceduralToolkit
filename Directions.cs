using System;

namespace ProceduralToolkit
{
    [Flags]
    public enum Directions
    {
        None = 0,
        Left = 1,
        Right = 2,
        Down = 4,
        Up = 8,
        Back = 16,
        Forward = 32,
        XAxis = Left | Right,
        YAxis = Down | Up,
        ZAxis = Back | Forward,
        All = Left | Right | Down | Up | Back | Forward
    }
}