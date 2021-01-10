using System;
using Unity.Collections;
using UnityEngine.Assertions;
using Random = UnityEngine.Random;

namespace ProceduralToolkit.CellularAutomata
{
    /// <summary>
    /// Generic cellular automaton for two-state rulesets
    /// </summary>
    public struct CellularAutomaton : IDisposable
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

        public CellularAutomaton(Config config)
        {
            Assert.IsTrue(config.width > 0);
            Assert.IsTrue(config.height > 0);

            this.config = config;
            cells = new NativeArray2D<bool>(config.width, config.height, Allocator.Persistent);
            copy = new NativeArray2D<bool>(config.width, config.height, Allocator.Persistent);

            FillWithNoise();
        }

        public void Dispose()
        {
            if (cells.IsCreated)
            {
                cells.Dispose();
            }
            if (copy.IsCreated)
            {
                copy.Dispose();
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

        private int CountAliveNeighbourCells(int x, int y)
        {
            if (config.aliveBorders)
            {
                var visitor = new Visitor8Unbounded<CounterUnbounded>(new CounterUnbounded {array = copy});
                visitor.Visit8Unbounded(x, y);
                return visitor.action.count;
            }
            else
            {
                var visitor = new Visitor8<Counter>(copy.LengthX, copy.LengthY, new Counter {array = copy});
                visitor.Visit8(x, y);
                return visitor.action.count;
            }
        }

        private struct Counter : IVisitAction
        {
            [ReadOnly]
            public NativeArray2D<bool> array;
            public int count;

            public void Visit(int x, int y)
            {
                if (array[x, y])
                {
                    count++;
                }
            }
        }

        private struct CounterUnbounded : IVisitAction
        {
            [ReadOnly]
            public NativeArray2D<bool> array;
            public int count;

            public void Visit(int x, int y)
            {
                if (x >= 0 && x < array.LengthX &&
                    y >= 0 && y < array.LengthY)
                {
                    if (array[x, y])
                    {
                        count++;
                    }
                }
                else
                {
                    count++;
                }
            }
        }
    }
}
