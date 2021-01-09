using System;
using Unity.Collections;
using UnityEngine.Assertions;
using Random = UnityEngine.Random;

namespace ProceduralToolkit.CellularAutomata
{
    /// <summary>
    /// Generic cellular automaton for two-state rulesets
    /// </summary>
    public class CellularAutomaton : IDisposable
    {
        [Serializable]
        public struct Config
        {
            public int width;
            public int height;
            public Ruleset ruleset;
            public float startNoise;
            public bool aliveBorders;

            public static Config life = new Config
            {
                width = 128,
                height = 128,
                ruleset = Ruleset.life,
                startNoise = 0.25f,
                aliveBorders = false,
            };
        }

        public NativeArray2D<bool> cells;

        private Config config;
        private NativeArray2D<bool> copy;
        private int aliveNeighbours;

        public CellularAutomaton(Config config)
        {
            Assert.IsTrue(config.width > 0);
            Assert.IsTrue(config.height > 0);

            this.config = config;
            cells = new NativeArray2D<bool>(config.width, config.height, Allocator.Persistent);
            copy = new NativeArray2D<bool>(config.width, config.height, Allocator.Persistent);
            aliveNeighbours = 0;

            FillWithNoise();
        }

        public void Dispose()
        {
            cells.Dispose();
            copy.Dispose();
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

                    if (!copy[x, y])
                    {
                        if (config.ruleset.CanSpawn(aliveCells))
                        {
                            cells[x, y] = true;
                        }
                        else
                        {
                            cells[x, y] = false;
                        }
                    }
                    else
                    {
                        if (!config.ruleset.CanSurvive(aliveCells))
                        {
                            cells[x, y] = false;
                        }
                        else
                        {
                            cells[x, y] = true;
                        }
                    }
                }
            }
        }

        public void FillWithNoise()
        {
            FillWithNoise(config.startNoise);
        }

        public void FillWithNoise(float noise)
        {
            for (int x = 0; x < config.width; x++)
            {
                for (int y = 0; y < config.height; y++)
                {
                    cells[x, y] = copy[x, y] = Random.value < noise;
                }
            }
        }

        private void VisitAliveBorders(int neighbourX, int neighbourY)
        {
            if (neighbourX >= 0 && neighbourX < config.width &&
                neighbourY >= 0 && neighbourY < config.height)
            {
                if (copy[neighbourX, neighbourY])
                {
                    aliveNeighbours++;
                }
            }
            else
            {
                aliveNeighbours++;
            }
        }

        private void VisitDeadBorders(int neighbourX, int neighbourY)
        {
            if (copy[neighbourX, neighbourY])
            {
                aliveNeighbours++;
            }
        }

        private int CountAliveNeighbourCells(int x, int y)
        {
            aliveNeighbours = 0;
            if (config.aliveBorders)
            {
                copy.Visit8Unbounded(x, y, VisitAliveBorders);
            }
            else
            {
                copy.Visit8(x, y, VisitDeadBorders);
            }
            return aliveNeighbours;
        }
    }
}
