using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
            None,
            RandomTraversal,
            RandomDepthFirstTraversal,
            RandomBreadthFirstTraversal,
        }

        private Maze maze;
        private List<Edge> edges;
        private int pauseStep;

        public MazeGenerator(int mazeWidth, int mazeHeight, int cellSize, int wallSize, int pauseStep = 200)
        {
            int cellWidth = (mazeWidth - wallSize)/(cellSize + wallSize);
            int cellHeight = (mazeHeight - wallSize)/(cellSize + wallSize);
            maze = new Maze(cellWidth, cellHeight);

            var origin = new Cell
            {
                x = Random.Range(0, cellWidth),
                y = Random.Range(0, cellHeight)
            };
            maze[origin] = Directions.None;
            edges = new List<Edge>(maze.GetEdges(origin));

            this.pauseStep = pauseStep;
        }

        public IEnumerator RandomTraversal(Action<Edge> onDrawEdge, Action onPause)
        {
            int count = 0;
            while (edges.Count > 0)
            {
                Edge passage = edges.PopRandom();

                if (maze[passage.exit] == Directions.None)
                {
                    maze.AddEdge(passage);
                    edges.AddRange(maze.GetEdges(passage.exit));

                    onDrawEdge(passage);

                    // Pause generation to show current results
                    count++;
                    if (count > pauseStep)
                    {
                        count = 0;
                        onPause();
                        yield return null;
                    }
                }
            }
        }

        public IEnumerator RandomDepthFirstTraversal(Action<Edge> onDrawEdge, Action onPause)
        {
            int count = 0;
            while (edges.Count > 0)
            {
                Edge edge = edges[edges.Count - 1];
                edges.RemoveAt(edges.Count - 1);

                if (maze[edge.exit] == Directions.None)
                {
                    maze.AddEdge(edge);
                    List<Edge> newEdges = maze.GetEdges(edge.exit);
                    newEdges.Shuffle();
                    edges.AddRange(newEdges);

                    onDrawEdge(edge);

                    // Pause generation to show current results
                    count++;
                    if (count > pauseStep)
                    {
                        count = 0;
                        onPause();
                        yield return null;
                    }
                }
            }
        }

        public IEnumerator RandomBreadthFirstTraversal(Action<Edge> onDrawEdge, Action onPause)
        {
            int count = 0;
            while (edges.Count > 0)
            {
                Edge edge = edges[0];
                edges.RemoveAt(0);

                if (maze[edge.exit] == Directions.None)
                {
                    maze.AddEdge(edge);
                    List<Edge> newEdges = maze.GetEdges(edge.exit);
                    newEdges.Shuffle();
                    edges.AddRange(newEdges);

                    onDrawEdge(edge);

                    // Pause generation to show current results
                    count++;
                    if (count > pauseStep)
                    {
                        count = 0;
                        onPause();
                        yield return null;
                    }
                }
            }
        }
    }
}