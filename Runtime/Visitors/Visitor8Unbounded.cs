using UnityEngine;

namespace ProceduralToolkit
{
    /// <summary>
    /// Visits eight cells surrounding the center cell
    /// </summary>
    /// <remarks>
    /// https://en.wikipedia.org/wiki/Moore_neighborhood
    /// </remarks>
    public struct Visitor8Unbounded<TVisitAction>
        where TVisitAction : struct, IVisitAction
    {
        public TVisitAction action;

        public Visitor8Unbounded(TVisitAction action)
        {
            this.action = action;
        }

        public void Visit8Unbounded(Vector2Int center)
        {
            Visit8Unbounded(center.x, center.y);
        }

        public void Visit8Unbounded(int x, int y)
        {
            action.Visit(x - 1, y - 1);
            action.Visit(x, y - 1);
            action.Visit(x + 1, y - 1);

            action.Visit(x - 1, y);
            action.Visit(x + 1, y);

            action.Visit(x - 1, y + 1);
            action.Visit(x, y + 1);
            action.Visit(x + 1, y + 1);
        }
    }
}
