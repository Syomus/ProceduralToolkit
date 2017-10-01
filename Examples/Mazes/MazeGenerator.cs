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
            RandomBreadthFirstTraversal,
        }

        [Serializable]
        public class Config
        {
            public int width = 100;
            public int height = 100;
            public Algorithm algorithm = Algorithm.RandomTraversal;
            public Action<Maze.Edge> drawEdge = edge => { };
        }

        private readonly Maze maze;
        private readonly List<Maze.Edge> edges;
        private readonly Config config;

        public MazeGenerator(Config config)
        {
            Assert.IsTrue(config.width > 0);
            Assert.IsTrue(config.height > 0);
            Assert.IsNotNull(config.drawEdge);

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
                    case Algorithm.RandomBreadthFirstTraversal:
                        RandomBreadthFirstTraversal();
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

                config.drawEdge(edge);
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

                config.drawEdge(edge);
            }
        }

        private void RandomBreadthFirstTraversal()
        {
            Maze.Edge edge = edges[0];
            edges.RemoveAt(0);

            if (maze.IsUnconnected(edge.exit.position))
            {
                maze.AddEdge(edge);
                List<Maze.Edge> newEdges = maze.GetPossibleConnections(edge.exit);
                newEdges.Shuffle();
                edges.AddRange(newEdges);

                config.drawEdge(edge);
            }
        }
    }
}