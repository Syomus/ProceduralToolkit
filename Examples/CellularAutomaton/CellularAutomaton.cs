using System;
using UnityEngine.Assertions;
using Random = UnityEngine.Random;

namespace ProceduralToolkit.Examples
{
    /// <summary>
    /// Generic cellular automaton for two-state rulesets
    /// </summary>
    public class CellularAutomaton
    {
        [Serializable]
        public class Config
        {
            public int width = 128;
            public int height = 128;
            public Ruleset ruleset = Ruleset.life;
            public float startNoise = 0.25f;
            public bool aliveBorders = false;
        }

        public CellState[,] cells;

        private readonly Config config;
        private readonly Action<int, int> visitAliveBorders;
        private readonly Action<int, int> visitDeadBorders;
        private CellState[,] copy;
        private int aliveNeighbours;

        public CellularAutomaton(Config config)
        {
            Assert.IsTrue(config.width > 0);
            Assert.IsTrue(config.height > 0);

            this.config = config;
            cells = new CellState[config.width, config.height];
            copy = new CellState[config.width, config.height];

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

            FillWithNoise(config.startNoise);
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
            for (int x = 0; x < config.width; x++)
            {
                for (int y = 0; y < config.height; y++)
                {
                    int aliveCells = CountAliveNeighbourCells(x, y);

                    if (copy[x, y] == CellState.Dead)
                    {
                        if (config.ruleset.CanSpawn(aliveCells))
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
                        if (!config.ruleset.CanSurvive(aliveCells))
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

        private void FillWithNoise(float noise)
        {
            for (int x = 0; x < config.width; x++)
            {
                for (int y = 0; y < config.height; y++)
                {
                    cells[x, y] = Random.value < noise ? CellState.Alive : CellState.Dead;
                }
            }
        }

        private int CountAliveNeighbourCells(int x, int y)
        {
            aliveNeighbours = 0;
            if (config.aliveBorders)
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