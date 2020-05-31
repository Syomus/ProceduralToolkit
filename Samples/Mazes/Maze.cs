using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

namespace ProceduralToolkit.Samples
{
    /// <summary>
    /// Maze graph representation
    /// </summary>
    public struct Maze
    {
        public readonly int width;
        public readonly int height;
        public NativeArray<Directions> vertices;

        public Directions this[Vector2Int position]
        {
            get => vertices.GetXY(position, width);
            set => vertices.SetXY(position, width, value);
        }

        public Directions this[int x, int y]
        {
            get => vertices.GetXY(x, y, width);
            set => vertices.SetXY(x, y, width, value);
        }

        public Maze(int width, int height)
        {
            this.width = width;
            this.height = height;
            vertices = new NativeArray<Directions>(width*height, Allocator.Persistent);
        }

        public void GetPossibleConnections(Vector2Int position, List<Connection> connections)
        {
            TestDirection(position, Vector2Int.left, Directions.Left, connections);
            TestDirection(position, Vector2Int.right, Directions.Right, connections);
            TestDirection(position, Vector2Int.down, Directions.Down, connections);
            TestDirection(position, Vector2Int.up, Directions.Up, connections);
        }

        public void AddConnection(Connection connection)
        {
            Vector2Int delta = connection.b - connection.a;
            if (delta == Vector2Int.left)
            {
                this[connection.a] = this[connection.a].AddFlag(Directions.Left);
                this[connection.b] = Directions.Right;
            }
            else if (delta == Vector2Int.right)
            {
                this[connection.a] = this[connection.a].AddFlag(Directions.Right);
                this[connection.b] = Directions.Left;
            }
            else if (delta == Vector2Int.down)
            {
                this[connection.a] = this[connection.a].AddFlag(Directions.Down);
                this[connection.b] = Directions.Up;
            }
            else if (delta == Vector2Int.up)
            {
                this[connection.a] = this[connection.a].AddFlag(Directions.Up);
                this[connection.b] = Directions.Down;
            }
        }

        public bool IsUnconnected(Vector2Int position)
        {
            return this[position] == Directions.None;
        }

        private void TestDirection(Vector2Int a, Vector2Int offset, Directions direction, List<Connection> connections)
        {
            if (!this[a].HasFlag(direction))
            {
                var b = a + offset;
                if (IsInBounds(b) && IsUnconnected(b))
                {
                    connections.Add(new Connection(a, b));
                }
            }
        }

        private bool IsInBounds(Vector2Int position)
        {
            return position.x >= 0 && position.x < width &&
                   position.y >= 0 && position.y < height;
        }

        /// <summary>
        /// Maze graph connection
        /// </summary>
        public struct Connection
        {
            public readonly Vector2Int a;
            public readonly Vector2Int b;

            public Connection(Vector2Int a, Vector2Int b)
            {
                this.a = a;
                this.b = b;
            }
        }
    }
}
