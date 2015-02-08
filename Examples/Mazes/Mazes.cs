using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProceduralToolkit.Examples
{
    public class Maze
    {
        private int width;
        private int height;
        private Directions[,] cells;

        public Maze(int width, int height)
        {
            this.width = width;
            this.height = height;
            cells = new Directions[width, height];
        }

        public Directions this[Cell cell]
        {
            get { return cells[cell.x, cell.y]; }
            set { cells[cell.x, cell.y] = value; }
        }

        public bool Contains(Cell cell)
        {
            return cell.x >= 0 && cell.x < width && cell.y >= 0 && cell.y < height;
        }

        public void AddEdge(Edge edge)
        {
            this[edge.origin] |= edge.origin.direction;
            this[edge.exit] = edge.exit.direction;
        }

        public List<Edge> GetEdges(Cell origin)
        {
            var edges = new List<Edge>();
            if (origin.direction != Directions.Left)
            {
                var edge = new Edge(origin.x, origin.y, Directions.Right, origin.depth);
                if (Contains(edge.exit) && this[edge.exit] == Directions.None)
                {
                    edges.Add(edge);
                }
            }
            if (origin.direction != Directions.Right)
            {
                var edge = new Edge(origin.x, origin.y, Directions.Left, origin.depth);
                if (Contains(edge.exit) && this[edge.exit] == Directions.None)
                {
                    edges.Add(edge);
                }
            }
            if (origin.direction != Directions.Down)
            {
                var edge = new Edge(origin.x, origin.y, Directions.Up, origin.depth);
                if (Contains(edge.exit) && this[edge.exit] == Directions.None)
                {
                    edges.Add(edge);
                }
            }
            if (origin.direction != Directions.Up)
            {
                var edge = new Edge(origin.x, origin.y, Directions.Down, origin.depth);
                if (Contains(edge.exit) && this[edge.exit] == Directions.None)
                {
                    edges.Add(edge);
                }
            }
            return edges;
        }
    }

    public class Edge
    {
        public Cell origin;
        public Cell exit;

        public Edge(int x, int y, Directions direction, int depth)
        {
            origin = new Cell {direction = direction, x = x, y = y, depth = depth};
            exit = new Cell {x = x, y = y, depth = depth + 1};

            switch (origin.direction)
            {
                case Directions.Left:
                    exit.direction = Directions.Right;
                    exit.x -= 1;
                    break;
                case Directions.Right:
                    exit.direction = Directions.Left;
                    exit.x += 1;
                    break;
                case Directions.Down:
                    exit.direction = Directions.Up;
                    exit.y -= 1;
                    break;
                case Directions.Up:
                    exit.direction = Directions.Down;
                    exit.y += 1;
                    break;
            }
        }
    }

    public class Cell
    {
        public Directions direction;
        public int x;
        public int y;
        public int depth;
    }

    public enum MazeAlgorithm
    {
        None,
        RandomTraversal,
        RandomDepthFirstTraversal
    }

    /// <summary>
    /// A maze generators
    /// </summary>
    public class Mazes : MonoBehaviour
    {
        public int mazeWidth = 256;
        public int mazeHeight = 256;
        public int cellSize = 20;
        public int cellWallSize = 1;
        public MazeAlgorithm mazeAlgorithm;
        public bool rainbow;
        public int drawStep = 200;

        private int cellWidth;
        private int cellHeight;
        private Texture2D texture;
        private Maze maze;
        private List<Edge> edges;

        private void Awake()
        {
            Generate();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Generate();
            }
        }

        private void Generate()
        {
            StopAllCoroutines();
            StartCoroutine(GenerateCoroutine());
        }

        private IEnumerator GenerateCoroutine()
        {
            texture = new Texture2D(mazeWidth, mazeHeight, TextureFormat.ARGB32, false, true)
            {
                filterMode = FilterMode.Point
            };
            cellWidth = (mazeWidth - cellWallSize)/(cellSize + cellWallSize);
            cellHeight = (mazeHeight - cellWallSize)/(cellSize + cellWallSize);
            maze = new Maze(cellWidth, cellHeight);

            var origin = new Cell {x = Random.Range(0, cellWidth), y = Random.Range(0, cellHeight)};
            maze[origin] = Directions.None;
            edges = new List<Edge>(maze.GetEdges(origin));

            texture.Clear(Color.black);
            yield return null;

            switch (mazeAlgorithm)
            {
                case MazeAlgorithm.None:
                    if (RandomE.Chance(0.5f))
                    {
                        yield return StartCoroutine(RandomTraversal());
                    }
                    else
                    {
                        yield return StartCoroutine(RandomDepthFirstTraversal());
                    }
                    break;
                case MazeAlgorithm.RandomTraversal:
                    yield return StartCoroutine(RandomTraversal());
                    break;
                case MazeAlgorithm.RandomDepthFirstTraversal:
                    yield return StartCoroutine(RandomDepthFirstTraversal());
                    break;
            }

            texture.Apply();
        }

        private IEnumerator RandomTraversal()
        {
            int count = 0;
            while (edges.Count > 0)
            {
                var passage = edges.PopRandom();

                if (maze[passage.exit] == Directions.None)
                {
                    maze.AddEdge(passage);
                    edges.AddRange(maze.GetEdges(passage.exit));

                    DrawEdge(passage);
                    count++;
                    if (count > drawStep)
                    {
                        count = 0;
                        texture.Apply();
                        yield return null;
                    }
                }
            }
        }

        private IEnumerator RandomDepthFirstTraversal()
        {
            int count = 0;
            while (edges.Count > 0)
            {
                var edge = edges[edges.Count - 1];
                edges.RemoveAt(edges.Count - 1);

                if (maze[edge.exit] == Directions.None)
                {
                    maze.AddEdge(edge);
                    var newEdges = maze.GetEdges(edge.exit);
                    newEdges.Shuffle();
                    edges.AddRange(newEdges);

                    DrawEdge(edge);
                    count++;
                    if (count > drawStep)
                    {
                        count = 0;
                        texture.Apply();
                        yield return null;
                    }
                }
            }
        }

        private void DrawEdge(Edge edge)
        {
            int x, y, width, height;
            if (edge.origin.direction == Directions.Left || edge.origin.direction == Directions.Down)
            {
                x = Translate(edge.exit.x);
                y = Translate(edge.exit.y);
            }
            else
            {
                x = Translate(edge.origin.x);
                y = Translate(edge.origin.y);
            }

            if (edge.origin.direction == Directions.Left || edge.origin.direction == Directions.Right)
            {
                width = cellSize*2 + cellWallSize;
                height = cellSize;
            }
            else
            {
                width = cellSize;
                height = cellSize*2 + cellWallSize;
            }
            var color = rainbow ? ColorE.HSVToRGB(Mathf.Repeat(edge.origin.depth/360f, 1), 1, 1) : Color.white;
            texture.DrawRect(x, y, width, height, color);
        }

        private int Translate(int x)
        {
            return cellWallSize + x*(cellSize + cellWallSize);
        }

        private void OnGUI()
        {
            GUI.DrawTexture(new Rect(50, 50, Screen.width - 100, Screen.height - 100), texture, ScaleMode.ScaleToFit);

            GUI.color = Color.black;
            GUI.Label(new Rect(20, 20, Screen.width, Screen.height), "Click to generate new maze");
        }
    }
}