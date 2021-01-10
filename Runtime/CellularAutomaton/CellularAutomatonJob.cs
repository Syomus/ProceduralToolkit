using Unity.Collections;
using Unity.Jobs;
using UnityEngine.Assertions;

namespace ProceduralToolkit.CellularAutomaton
{
    /// <summary>
    /// Generic cellular automaton for two-state rulesets
    /// </summary>
    public struct CellularAutomatonJob : IJob
    {
        public Cells cells;
        public Config config;
        public int simulationSteps;

        public CellularAutomatonJob(Cells cells, Config config, int simulationSteps = 1)
        {
            Assert.IsTrue(config.width > 0);
            Assert.IsTrue(config.height > 0);

            this.cells = cells;
            this.config = config;
            this.simulationSteps = simulationSteps;
        }

        public CellularAutomatonJob(Config config, int simulationSteps = 1)
        {
            Assert.IsTrue(config.width > 0);
            Assert.IsTrue(config.height > 0);

            cells = new Cells(config);
            this.config = config;
            this.simulationSteps = simulationSteps;
        }

        public void Execute()
        {
            for (int i = 0; i < simulationSteps; i++)
            {
                cells.Swap();
                for (int x = 0; x < config.width; x++)
                {
                    for (int y = 0; y < config.height; y++)
                    {
                        int aliveCells = CountAliveNeighbourCells(x, y);

                        if (cells.GetPrevious(x, y))
                        {
                            cells[x, y] = config.ruleset.CanSurvive(aliveCells);
                        }
                        else
                        {
                            cells[x, y] = config.ruleset.CanSpawn(aliveCells);
                        }
                    }
                }
            }
        }

        private int CountAliveNeighbourCells(int x, int y)
        {
            if (config.aliveBorders)
            {
                var visitor = new Visitor8Unbounded<CounterUnbounded>(new CounterUnbounded {array = cells.GetPrevious()});
                visitor.Visit8Unbounded(x, y);
                return visitor.action.count;
            }
            else
            {
                var visitor = new Visitor8<Counter>(config.width, config.height, new Counter {array = cells.GetPrevious()});
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
