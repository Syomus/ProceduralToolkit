namespace ProceduralToolkit
{
    /// <summary>
    /// Polygon winding order, can be calculated by Geometry.GetOrientation
    /// </summary>
    public enum Orientation
    {
        CounterClockwise = -1,
        NonOrientable = 0,
        Clockwise = 1,
    }
}
