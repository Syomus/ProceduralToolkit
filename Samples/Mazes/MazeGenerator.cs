using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using Random = UnityEngine.Random;

namespace ProceduralToolkit.Examples
{
    /// <summary>
    /// A maze generator
    /// </summary>
    public class MazeGenerator
    {
        public enum Algorithm
        {
            RandomTraversal,
            RandomDepthFirstTraversal,
        }

        [Serializable]
        public class Config
        {
            public int width = 100;
            public int height = 100;
            public Algorithm algorithm = Algorithm.RandomTraversal;
            public Action<Maze.Edge> drawEdge;
        }

        private readonly Maze maze;
        private readonly List<Maze.Edge> edges;
        private readonly Config config;

        public MazeGenerator(Config config)
        {
            Assert.IsTrue(config.width > 0);
            Assert.IsTrue(config.height > 0);

            this.config = config;
            maze = new Maze(config.width, config.height);

            var originPosition = new Vector2Int(Random.Range(0, config.width), Random.Range(0, config.height));
            edges = maze.GetPossibleConnections(new Maze.Vertex(originPosition, Directions.None, 0));
        }

        public bool Generate(int steps = 0)
        {
            bool changed = false;
            for (int i = 0; edges.Count > 0 && (steps == 0 || i < steps); i++)
            {
                switch (config.algorithm)
                {
                    case Algorithm.RandomTraversal:
                        RandomTraversal();
                        break;
                    case Algorithm.RandomDepthFirstTraversal:
                        RandomDepthFirstTraversal();
                        break;
                    default:
                        RandomTraversal();
                        break;
                }
                changed = true;
            }
            return changed;
        }

        private void RandomTraversal()
        {
            Maze.Edge edge = edges.PopRandom();

            if (maze.IsUnconnected(edge.exit.position))
            {
                maze.AddEdge(edge);
                edges.AddRange(maze.GetPossibleConnections(edge.exit));

                if (config.drawEdge != null)
                {
                    config.drawEdge(edge);
                }
            }
        }

        private void RandomDepthFirstTraversal()
        {
            Maze.Edge edge = edges[edges.Count - 1];
            edges.RemoveAt(edges.Count - 1);

            if (maze.IsUnconnected(edge.exit.position))
            {
                maze.AddEdge(edge);
                List<Maze.Edge> newEdges = maze.GetPossibleConnections(edge.exit);
                newEdges.Shuffle();
                edges.AddRange(newEdges);

                if (config.drawEdge != null)
                {
                    config.drawEdge(edge);
                }
            }
        }

        public static int GetMapWidth(int mazeWidth, int wallSize, int roomSize)
        {
            return wallSize + mazeWidth*(roomSize + wallSize);
        }

        public static int GetMapHeight(int mazeHeight, int wallSize, int roomSize)
        {
            return wallSize + mazeHeight*(roomSize + wallSize);
        }

        public static void EdgeToRect(Maze.Edge edge, int wallSize, int roomSize,
            out Vector2Int position, out int width, out int height)
        {
            position = new Vector2Int(
                x: wallSize + Mathf.Min(edge.origin.position.x, edge.exit.position.x)*(roomSize + wallSize),
                y: wallSize + Mathf.Min(edge.origin.position.y, edge.exit.position.y)*(roomSize + wallSize));

            if ((edge.exit.position - edge.origin.position).y == 0)
            {
                width = roomSize*2 + wallSize;
                height = roomSize;
            }
            else
            {
                width = roomSize;
                height = roomSize*2 + wallSize;
            }
        }
    }
}
