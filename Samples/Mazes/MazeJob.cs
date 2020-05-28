using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using Unity.Jobs;
using MRandom = Unity.Mathematics.Random;

namespace ProceduralToolkit.Samples
{
    /// <summary>
    /// A maze generator
    /// </summary>
    public struct MazeJob : IJob
    {
        public Maze maze;

        private readonly Config config;
        private MRandom random;

        public MazeJob(Config config, ref MRandom random)
        {
            Assert.IsTrue(config.width > 0);
            Assert.IsTrue(config.height > 0);

            maze = new Maze(config.width, config.height);
            this.config = config;
            this.random = random;
        }

        public void Execute()
        {
            var position = new Vector2Int(random.NextInt(0, config.width), random.NextInt(0, config.height));
            List<Maze.Connection> connections = maze.GetPossibleConnections(position);

            while (connections.Count > 0)
            {
                switch (config.algorithm)
                {
                    case Algorithm.RandomTraversal:
                        RandomTraversal(maze, connections, ref random);
                        break;
                    case Algorithm.RandomDepthFirstTraversal:
                        RandomDepthFirstTraversal(maze, connections, ref random);
                        break;
                    default:
                        RandomTraversal(maze, connections, ref random);
                        break;
                }
            }
        }

        private static void RandomTraversal(Maze maze, List<Maze.Connection> connections, ref MRandom random)
        {
            Maze.Connection connection = connections.PopRandom(ref random);

            if (maze.IsUnconnected(connection.b))
            {
                maze.AddConnection(connection);
                connections.AddRange(maze.GetPossibleConnections(connection.b));
            }
        }

        private static void RandomDepthFirstTraversal(Maze maze, List<Maze.Connection> connections, ref MRandom random)
        {
            Maze.Connection connection = connections[connections.Count - 1];
            connections.RemoveAt(connections.Count - 1);

            if (maze.IsUnconnected(connection.b))
            {
                maze.AddConnection(connection);
                List<Maze.Connection> newConnections = maze.GetPossibleConnections(connection.b);
                newConnections.Shuffle(ref random);
                connections.AddRange(newConnections);
            }
        }

        public enum Algorithm
        {
            RandomTraversal,
            RandomDepthFirstTraversal,
        }

        [Serializable]
        public struct Config
        {
            public int width;
            public int height;
            public Algorithm algorithm;
        }
    }
}
