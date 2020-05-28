using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using Random = UnityEngine.Random;

namespace ProceduralToolkit.Samples
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
            public int width = 32;
            public int height = 32;
            public Algorithm algorithm = Algorithm.RandomTraversal;
        }

        private readonly Config config;

        public MazeGenerator(Config config)
        {
            Assert.IsTrue(config.width > 0);
            Assert.IsTrue(config.height > 0);

            this.config = config;
        }

        public Maze Generate()
        {
            var maze = new Maze(config.width, config.height);

            var position = new Vector2Int(Random.Range(0, config.width), Random.Range(0, config.height));
            List<Maze.Connection> connections = maze.GetPossibleConnections(position);

            while (connections.Count > 0)
            {
                switch (config.algorithm)
                {
                    case Algorithm.RandomTraversal:
                        RandomTraversal(maze, connections);
                        break;
                    case Algorithm.RandomDepthFirstTraversal:
                        RandomDepthFirstTraversal(maze, connections);
                        break;
                    default:
                        RandomTraversal(maze, connections);
                        break;
                }
            }
            return maze;
        }

        private void RandomTraversal(Maze maze, List<Maze.Connection> connections)
        {
            Maze.Connection connection = connections.PopRandom();

            if (maze.IsUnconnected(connection.b))
            {
                maze.AddConnection(connection);
                connections.AddRange(maze.GetPossibleConnections(connection.b));
            }
        }

        private void RandomDepthFirstTraversal(Maze maze, List<Maze.Connection> connections)
        {
            Maze.Connection connection = connections[connections.Count - 1];
            connections.RemoveAt(connections.Count - 1);

            if (maze.IsUnconnected(connection.b))
            {
                maze.AddConnection(connection);
                List<Maze.Connection> newConnections = maze.GetPossibleConnections(connection.b);
                newConnections.Shuffle();
                connections.AddRange(newConnections);
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

        public static RectInt ConnectionToRect(Vector2Int a, Vector2Int b, int wallSize, int roomSize)
        {
            var rect = new RectInt
            {
                min = new Vector2Int(
                    x: wallSize + Mathf.Min(a.x, b.x)*(roomSize + wallSize),
                    y: wallSize + Mathf.Min(a.y, b.y)*(roomSize + wallSize))
            };

            if ((b - a).y == 0)
            {
                rect.width = roomSize*2 + wallSize;
                rect.height = roomSize;
            }
            else
            {
                rect.width = roomSize;
                rect.height = roomSize*2 + wallSize;
            }
            return rect;
        }
    }
}
