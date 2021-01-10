using System;

namespace ProceduralToolkit
{
    public struct VisitAction : IVisitAction
    {
        public Action<int, int> action;

        public VisitAction(Action<int, int> action)
        {
            this.action = action;
        }

        public void Visit(int x, int y)
        {
            action(x, y);
        }

        public static implicit operator VisitAction(Action<int, int> action) => new VisitAction(action);
        public static implicit operator Action<int, int>(VisitAction action) => action.action;
    }
}
