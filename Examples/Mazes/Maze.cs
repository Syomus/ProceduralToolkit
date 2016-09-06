using System.Collections.Generic;

namespace ProceduralToolkit.Examples
{
    /// <summary>
    /// Maze graph representation
    /// </summary>
    public class Maze
    {
        private int width;
        private int height;
        private Directions[,] cells;

        public Maze(int width, int height)
        {
            this.width = width;
            this.height = height;
            cells = new Directions[width, height];
        }

        public Directions this[Cell cell]
        {
            get { return cells[cell.x, cell.y]; }
            set { cells[cell.x, cell.y] = value; }
        }

        public bool Contains(Cell cell)
        {
            return cell.x >= 0 && cell.x < width && cell.y >= 0 && cell.y < height;
        }

        public void AddEdge(Edge edge)
        {
            this[edge.origin] |= edge.origin.direction;
            this[edge.exit] = edge.exit.direction;
        }

        public List<Edge> GetEdges(Cell origin)
        {
            var edges = new List<Edge>();
            if (origin.direction != Directions.Left)
            {
                var edge = new Edge(origin.x, origin.y, Directions.Right, origin.depth);
                if (Contains(edge.exit) && this[edge.exit] == Directions.None)
                {
                    edges.Add(edge);
                }
            }
            if (origin.direction != Directions.Right)
            {
                var edge = new Edge(origin.x, origin.y, Directions.Left, origin.depth);
                if (Contains(edge.exit) && this[edge.exit] == Directions.None)
                {
                    edges.Add(edge);
                }
            }
            if (origin.direction != Directions.Down)
            {
                var edge = new Edge(origin.x, origin.y, Directions.Up, origin.depth);
                if (Contains(edge.exit) && this[edge.exit] == Directions.None)
                {
                    edges.Add(edge);
                }
            }
            if (origin.direction != Directions.Up)
            {
                var edge = new Edge(origin.x, origin.y, Directions.Down, origin.depth);
                if (Contains(edge.exit) && this[edge.exit] == Directions.None)
                {
                    edges.Add(edge);
                }
            }
            return edges;
        }
    }
}