using UnityEngine;

namespace ProceduralToolkit
{
    /// <summary>
    /// Visits four cells orthogonally surrounding the center cell
    /// </summary>
    /// <remarks>
    /// https://en.wikipedia.org/wiki/Von_Neumann_neighborhood
    /// </remarks>
    public struct Visitor4Unbounded<TVisitAction>
        where TVisitAction : struct, IVisitAction
    {
        public TVisitAction action;

        public Visitor4Unbounded(TVisitAction action)
        {
            this.action = action;
        }

        public void Visit4Unbounded(Vector2Int center)
        {
            Visit4Unbounded(center.x, center.y);
        }

        public void Visit4Unbounded(int x, int y)
        {
            action.Visit(x - 1, y);
            action.Visit(x + 1, y);
            action.Visit(x, y - 1);
            action.Visit(x, y + 1);
        }
    }
}
