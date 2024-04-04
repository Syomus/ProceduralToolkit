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
            var connections = new List<Maze.Connection>();
            maze.GetPossibleConnections(position, connections);

            if (config.algorithm == Algorithm.RandomTraversal)
            {
                RandomTraversal(connections);
            }
            else if (config.algorithm == Algorithm.RandomDepthFirstTraversal)
            {
                RandomDepthFirstTraversal(connections);
            }
            else
            {
                RandomTraversal(connections);
            }
        }

        private void RandomTraversal(List<Maze.Connection> connections)
        {
            while (connections.Count > 0)
            {
                Maze.Connection connection = connections.PopRandom(ref random);

                if (maze.IsUnconnected(connection.b))
                {
                    maze.AddConnection(connection);
                    maze.GetPossibleConnections(connection.b, connections);
                }
            }
        }

        private void RandomDepthFirstTraversal(List<Maze.Connection> connections)
        {
            var possibleConnections = new List<Maze.Connection>();

            while (connections.Count > 0)
            {
                Maze.Connection connection = connections[connections.Count - 1];
                connections.RemoveAt(connections.Count - 1);

                if (maze.IsUnconnected(connection.b))
                {
                    maze.AddConnection(connection);
                    possibleConnections.Clear();
                    maze.GetPossibleConnections(connection.b, possibleConnections);
                    possibleConnections.Shuffle(ref random);
                    connections.AddRange(possibleConnections);
                }
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
