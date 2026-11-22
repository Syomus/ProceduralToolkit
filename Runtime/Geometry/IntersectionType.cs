namespace ProceduralToolkit
{
    /// <summary>
    /// Intersection types returned by intersection algorithms
    /// </summary>
    public enum IntersectionType : byte
    {
        None = 0,
        Point,
        TwoPoints,
        Line,
        Ray,
        Segment,
        Circle,
        Sphere,
    }
}
