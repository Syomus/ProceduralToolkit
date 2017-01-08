namespace ProceduralToolkit.Examples
{
    /// <summary>
    /// Maze graph edge
    /// </summary>
    public class Edge
    {
        public Cell origin;
        public Cell exit;

        public Edge(int x, int y, Directions direction, int depth)
        {
            origin = new Cell {direction = direction, x = x, y = y, depth = depth};
            exit = new Cell {x = x, y = y, depth = depth + 1};

            if (origin.direction == Directions.Left)
            {
                exit.direction = Directions.Right;
                exit.x--;
            }
            else if (origin.direction == Directions.Right)
            {
                exit.direction = Directions.Left;
                exit.x++;
            }
            else if (origin.direction == Directions.Down)
            {
                exit.direction = Directions.Up;
                exit.y--;
            }
            else if (origin.direction == Directions.Up)
            {
                exit.direction = Directions.Down;
                exit.y++;
            }
        }
    }
}