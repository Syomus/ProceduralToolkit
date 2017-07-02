using System.Collections.Generic;

namespace ProceduralToolkit.Examples
{
    public enum CellState
    {
        Dead = 0,
        Alive = 1
    }

    public class CellStateComparer : IEqualityComparer<CellState>
    {
        public bool Equals(CellState x, CellState y)
        {
            return x == y;
        }

        public int GetHashCode(CellState obj)
        {
            return obj.GetHashCode();
        }
    }
}