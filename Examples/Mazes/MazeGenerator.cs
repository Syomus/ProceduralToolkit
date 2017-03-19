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
            public int mazeWidth = 256;
            public int mazeHeight = 256;
            public int cellSize = 2;
            public int wallSize = 1;
            public Algorithm algorithm = Algorithm.RandomTraversal;
            public Action<Edge> drawEdge = edge => { };
        }

        private readonly Maze maze;
        private readonly List<Edge> edges;
        private readonly Config config;

        public MazeGenerator(Config config)
        {
            Assert.IsTrue(config.mazeWidth > 0);
            Assert.IsTrue(config.mazeHeight > 0);
            Assert.IsTrue(config.cellSize > 0);
            Assert.IsTrue(config.wallSize >= 0);
            Assert.IsNotNull(config.drawEdge);

            this.config = config;
            int widthInCells = (config.mazeWidth - config.wallSize)/(config.cellSize + config.wallSize);
            int heightInCells = (config.mazeHeight - config.wallSize)/(config.cellSize + config.wallSize);
            maze = new Maze(widthInCells, heightInCells);

            var origin = new Cell
            {
                x = Random.Range(0, widthInCells),
                y = Random.Range(0, heightInCells)
            };
            maze[origin] = Directions.None;
            edges = new List<Edge>(maze.GetEdges(origin));
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
            Edge passage = edges.PopRandom();

            if (maze[passage.exit] == Directions.None)
            {
                maze.AddEdge(passage);
                edges.AddRange(maze.GetEdges(passage.exit));

                config.drawEdge(passage);
            }
        }

        private void RandomDepthFirstTraversal()
        {
            Edge edge = edges[edges.Count - 1];
            edges.RemoveAt(edges.Count - 1);

            if (maze[edge.exit] == Directions.None)
            {
                maze.AddEdge(edge);
                List<Edge> newEdges = maze.GetEdges(edge.exit);
                newEdges.Shuffle();
                edges.AddRange(newEdges);

                config.drawEdge(edge);
            }
        }

        private void RandomBreadthFirstTraversal()
        {
            Edge edge = edges[0];
            edges.RemoveAt(0);

            if (maze[edge.exit] == Directions.None)
            {
                maze.AddEdge(edge);
                List<Edge> newEdges = maze.GetEdges(edge.exit);
                newEdges.Shuffle();
                edges.AddRange(newEdges);

                config.drawEdge(edge);
            }
        }
    }
}