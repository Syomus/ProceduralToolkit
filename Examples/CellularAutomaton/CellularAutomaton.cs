using System;
using Random = UnityEngine.Random;

namespace ProceduralToolkit.Examples
{
    /// <summary>
    /// Generic cellular automaton for two-state rulesets
    /// </summary>
    public class CellularAutomaton
    {
        public CellState[,] cells;

        private int width;
        private int height;
        private Ruleset ruleset;
        private bool aliveBorders;

        private CellState[,] copy;
        private int aliveNeighbours;
        private readonly Action<int, int> visitAliveBorders;
        private readonly Action<int, int> visitDeadBorders;

        public CellularAutomaton(int width, int height, Ruleset ruleset, float startNoise, bool aliveBorders)
        {
            this.width = width;
            this.height = height;
            this.ruleset = ruleset;
            this.aliveBorders = aliveBorders;
            cells = new CellState[width, height];
            copy = new CellState[width, height];

            visitAliveBorders = (int neighbourX, int neighbourY) =>
            {
                if (copy.IsInBounds(neighbourX, neighbourY))
                {
                    if (copy[neighbourX, neighbourY] == CellState.Alive)
                    {
                        aliveNeighbours++;
                    }
                }
                else
                {
                    aliveNeighbours++;
                }
            };
            visitDeadBorders = (int neighbourX, int neighbourY) =>
            {
                if (copy[neighbourX, neighbourY] == CellState.Alive)
                {
                    aliveNeighbours++;
                }
            };

            FillWithNoise(startNoise);
        }

        public void FillWithNoise(float noise)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    cells[x, y] = Random.value < noise ? CellState.Alive : CellState.Dead;
                }
            }
        }

        public void Simulate(int generations)
        {
            for (int i = 0; i < generations; i++)
            {
                Simulate();
            }
        }

        public void Simulate()
        {
            PTUtils.Swap(ref cells, ref copy);
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    int aliveCells = CountAliveNeighbourCells(x, y);

                    if (copy[x, y] == CellState.Dead)
                    {
                        if (ruleset.CanSpawn(aliveCells))
                        {
                            cells[x, y] = CellState.Alive;
                        }
                        else
                        {
                            cells[x, y] = CellState.Dead;
                        }
                    }
                    else
                    {
                        if (!ruleset.CanSurvive(aliveCells))
                        {
                            cells[x, y] = CellState.Dead;
                        }
                        else
                        {
                            cells[x, y] = CellState.Alive;
                        }
                    }
                }
            }
        }

        private int CountAliveNeighbourCells(int x, int y)
        {
            aliveNeighbours = 0;
            if (aliveBorders)
            {
                copy.VisitMooreNeighbours(x, y, false, visitAliveBorders);
            }
            else
            {
                copy.VisitMooreNeighbours(x, y, true, visitDeadBorders);
            }
            return aliveNeighbours;
        }
    }
}