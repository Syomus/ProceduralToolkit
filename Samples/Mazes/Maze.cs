using System.Collections.Generic;
using UnityEngine;

namespace ProceduralToolkit.Samples
{
    /// <summary>
    /// Maze graph representation
    /// </summary>
    public class Maze
    {
        public readonly int width;
        public readonly int height;
        private readonly Directions[,] vertices;

        public Directions this[Vector2Int position]
        {
            get { return vertices[position.x, position.y]; }
            set { vertices[position.x, position.y] = value; }
        }

        public Directions this[int x, int y]
        {
            get { return vertices[x, y]; }
            set { vertices[x, y] = value; }
        }

        public Maze(int width, int height)
        {
            this.width = width;
            this.height = height;
            vertices = new Directions[width, height];
        }

        public List<Edge> GetPossibleConnections(Vertex origin)
        {
            var edges = new List<Edge>();
            if (!origin.connections.HasFlag(Directions.Left))
            {
                var position = origin.position + Vector2Int.left;
                if (IsInBounds(position) && IsUnconnected(position))
                {
                    edges.Add(new Edge(origin, new Vertex(position, Directions.Right, origin.depth + 1)));
                }
            }
            if (!origin.connections.HasFlag(Directions.Right))
            {
                var position = origin.position + Vector2Int.right;
                if (IsInBounds(position) && IsUnconnected(position))
                {
                    edges.Add(new Edge(origin, new Vertex(position, Directions.Left, origin.depth + 1)));
                }
            }
            if (!origin.connections.HasFlag(Directions.Down))
            {
                var position = origin.position + Vector2Int.down;
                if (IsInBounds(position) && IsUnconnected(position))
                {
                    edges.Add(new Edge(origin, new Vertex(position, Directions.Up, origin.depth + 1)));
                }
            }
            if (!origin.connections.HasFlag(Directions.Up))
            {
                var position = origin.position + Vector2Int.up;
                if (IsInBounds(position) && IsUnconnected(position))
                {
                    edges.Add(new Edge(origin, new Vertex(position, Directions.Down, origin.depth + 1)));
                }
            }
            return edges;
        }

        public void AddEdge(Edge edge)
        {
            this[edge.origin.position] |= edge.origin.connections;
            this[edge.exit.position] = edge.exit.connections;
        }

        public bool IsUnconnected(Vector2Int position)
        {
            return this[position] == Directions.None;
        }

        public bool IsInBounds(Vector2Int position)
        {
            return position.x >= 0 && position.x < width &&
                   position.y >= 0 && position.y < height;
        }

        /// <summary>
        /// Maze graph vertex
        /// </summary>
        public struct Vertex
        {
            public readonly Vector2Int position;
            public readonly Directions connections;
            public readonly int depth;

            public Vertex(Vector2Int position, Directions connections, int depth)
            {
                this.position = position;
                this.connections = connections;
                this.depth = depth;
            }
        }

        /// <summary>
        /// Maze graph edge
        /// </summary>
        public struct Edge
        {
            public readonly Vertex origin;
            public readonly Vertex exit;

            public Edge(Vertex origin, Vertex exit)
            {
                this.origin = origin;
                this.exit = exit;
            }
        }
    }
}
