using UnityEngine;

namespace ProceduralToolkit
{
    /// <summary>
    /// Visits eight cells surrounding the center cell
    /// </summary>
    /// <remarks>
    /// https://en.wikipedia.org/wiki/Moore_neighborhood
    /// </remarks>
    public struct Visitor8<TVisitAction>
        where TVisitAction : struct, IVisitAction
    {
        public TVisitAction action;

        public int lengthX;
        public int lengthY;

        public Visitor8(int lengthX, int lengthY, TVisitAction action)
        {
            this.lengthX = lengthX;
            this.lengthY = lengthY;
            this.action = action;
        }

        public void Visit8(Vector2Int center)
        {
            Visit8(center.x, center.y);
        }

        public void Visit8(int x, int y)
        {
            bool xGreaterThanZero = x > 0;
            bool xLessThanWidth = x + 1 < lengthX;

            bool yGreaterThanZero = y > 0;
            bool yLessThanHeight = y + 1 < lengthY;

            if (yGreaterThanZero)
            {
                if (xGreaterThanZero) action.Visit(x - 1, y - 1);

                action.Visit(x, y - 1);

                if (xLessThanWidth) action.Visit(x + 1, y - 1);
            }

            if (xGreaterThanZero) action.Visit(x - 1, y);
            if (xLessThanWidth) action.Visit(x + 1, y);

            if (yLessThanHeight)
            {
                if (xGreaterThanZero) action.Visit(x - 1, y + 1);

                action.Visit(x, y + 1);

                if (xLessThanWidth) action.Visit(x + 1, y + 1);
            }
        }
    }
}
