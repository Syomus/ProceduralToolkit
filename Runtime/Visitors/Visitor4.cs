using UnityEngine;

namespace ProceduralToolkit
{
    /// <summary>
    /// Visits four cells orthogonally surrounding the center cell
    /// </summary>
    /// <remarks>
    /// https://en.wikipedia.org/wiki/Von_Neumann_neighborhood
    /// </remarks>
    public struct Visitor4<TVisitAction>
        where TVisitAction : struct, IVisitAction
    {
        public TVisitAction action;

        public int lengthX;
        public int lengthY;

        public Visitor4(int lengthX, int lengthY, TVisitAction action)
        {
            this.lengthX = lengthX;
            this.lengthY = lengthY;
            this.action = action;
        }

        public void Visit4(Vector2Int center)
        {
            Visit4(center.x, center.y);
        }

        public void Visit4(int x, int y)
        {
            if (x > 0)
            {
                action.Visit(x - 1, y);
            }
            if (x + 1 < lengthX)
            {
                action.Visit(x + 1, y);
            }
            if (y > 0)
            {
                action.Visit(x, y - 1);
            }
            if (y + 1 < lengthY)
            {
                action.Visit(x, y + 1);
            }
        }
    }
}
