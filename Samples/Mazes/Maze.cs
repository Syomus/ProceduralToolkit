using System;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

namespace ProceduralToolkit.Samples
{
    /// <summary>
    /// Maze graph representation
    /// </summary>
    public struct Maze : IDisposable
    {
        public int width => vertices.LengthX;
        public int height => vertices.LengthY;
        private NativeArray2D<Directions> vertices;

        public Directions this[Vector2Int position]
        {
            get => vertices[position];
            set => vertices[position] = value;
        }

        public Directions this[int x, int y]
        {
            get => vertices[x, y];
            set => vertices[x, y] = value;
        }

        public Maze(int width, int height)
        {
            vertices = new NativeArray2D<Directions>(width, height, Allocator.Persistent);
        }

        public void Dispose()
        {
            vertices.Dispose();
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
                if (vertices.IsInBounds(b) && IsUnconnected(b))
                {
                    connections.Add(new Connection(a, b));
                }
            }
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
