using System;
using System.Collections.Generic;
using UnityEngine.Assertions;
using Random = UnityEngine.Random;

namespace ProceduralToolkit
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

        private bool[,] _cells;
        public bool[,] cells
        {
            get => _cells;
            private set => _cells = value;
        }

        private Config config;
        private readonly Action<int, int> visitAliveBorders;
        private readonly Action<int, int> visitDeadBorders;
        private bool[,] copy;
        private int aliveNeighbours;

        public CellularAutomaton(Config config)
        {
            SetConfig(config);

            visitAliveBorders = (int neighbourX, int neighbourY) =>
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
            };
            visitDeadBorders = (int neighbourX, int neighbourY) =>
            {
                if (copy[neighbourX, neighbourY])
                {
                    aliveNeighbours++;
                }
            };

            FillWithNoise();
        }

        public void SetConfig(Config config)
        {
            Assert.IsTrue(config.width > 0);
            Assert.IsTrue(config.height > 0);

            this.config = config;
            if (cells == null ||
                config.width != cells.GetLength(0) ||
                config.height != cells.GetLength(1))
            {
                cells = new bool[config.width, config.height];
                copy = new bool[config.width, config.height];
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
            PTUtils.Swap(ref _cells, ref copy);
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
                    cells[x, y] = Random.value < noise;
                }
            }
        }

        private int CountAliveNeighbourCells(int x, int y)
        {
            aliveNeighbours = 0;
            if (config.aliveBorders)
            {
                copy.Visit8Unbounded(x, y, visitAliveBorders);
            }
            else
            {
                copy.Visit8(x, y, visitDeadBorders);
            }
            return aliveNeighbours;
        }

        /// <summary>
        /// Cellular automaton ruleset representation
        /// </summary>
        [Serializable]
        public struct Ruleset
        {
            #region Common rulesets

            public static Ruleset life => new Ruleset("3", "23");
            public static Ruleset highlife => new Ruleset("36", "23");
            public static Ruleset lifeWithoutDeath => new Ruleset("3", "012345678");
            public static Ruleset thirtyFour => new Ruleset("34", "34");
            public static Ruleset inverseLife => new Ruleset("0123478", "34678");
            public static Ruleset pseudoLife => new Ruleset("357", "238");
            public static Ruleset longLife => new Ruleset("345", "5");
            public static Ruleset dotLife => new Ruleset("3", "023");
            public static Ruleset dryLife => new Ruleset("37", "23");
            public static Ruleset seeds => new Ruleset("2");
            public static Ruleset serviettes => new Ruleset("234");
            public static Ruleset gnarl => new Ruleset("1", "1");
            public static Ruleset liveFreeOrDie => new Ruleset("2", "0");
            public static Ruleset dayAndNight => new Ruleset("3678", "34678");
            public static Ruleset replicator => new Ruleset("1357", "1357");
            public static Ruleset twoXTwo => new Ruleset("36", "125");
            public static Ruleset move => new Ruleset("368", "245");
            public static Ruleset maze => new Ruleset("3", "12345");
            public static Ruleset mazectric => new Ruleset("3", "1234");
            public static Ruleset amoeba => new Ruleset("357", "1358");
            public static Ruleset diamoeba => new Ruleset("35678", "5678");
            public static Ruleset coral => new Ruleset("3", "45678");
            public static Ruleset anneal => new Ruleset("4678", "35678");
            public static Ruleset majority => new Ruleset("5678", "45678");
            public static Ruleset walledCities => new Ruleset("45678", "2345");
            public static Ruleset stains => new Ruleset("3678", "235678");
            public static Ruleset coagulations => new Ruleset("378", "235678");
            public static Ruleset assimilation => new Ruleset("345", "4567");

            #endregion Common rulesets

            public byte[] birthRule;
            public byte[] survivalRule;

            public Ruleset(byte[] birthRule, byte[] survivalRule)
            {
                this.birthRule = new byte[birthRule.Length];
                for (int i = 0; i < birthRule.Length; i++)
                {
                    this.birthRule[i] = birthRule[i];
                }
                this.survivalRule = new byte[survivalRule.Length];
                for (int i = 0; i < survivalRule.Length; i++)
                {
                    this.survivalRule[i] = survivalRule[i];
                }
            }

            public Ruleset(List<byte> birthRule, List<byte> survivalRule)
            {
                this.birthRule = birthRule.ToArray();
                this.survivalRule = survivalRule.ToArray();
            }

            public Ruleset(string birthRule = null, string survivalRule = null)
            {
                this.birthRule = ConvertRuleStringToList(birthRule).ToArray();
                this.survivalRule = ConvertRuleStringToList(survivalRule).ToArray();
            }

            public bool CanSpawn(int aliveCells)
            {
                foreach (byte number in birthRule)
                {
                    if (number == aliveCells) return true;
                }
                return false;
            }

            public bool CanSurvive(int aliveCells)
            {
                foreach (byte number in survivalRule)
                {
                    if (number == aliveCells) return true;
                }
                return false;
            }

            public static List<byte> ConvertRuleStringToList(string rule)
            {
                var list = new List<byte>();
                if (!string.IsNullOrEmpty(rule))
                {
                    foreach (char c in rule)
                    {
                        if (char.IsDigit(c))
                        {
                            byte digit = (byte) char.GetNumericValue(c);
                            if (!list.Contains(digit))
                            {
                                list.Add(digit);
                            }
                        }
                    }
                    list.Sort();
                }
                return list;
            }

            public override string ToString()
            {
                string b = "";
                foreach (var digit in birthRule)
                {
                    b += digit;
                }
                string s = "";
                foreach (var digit in survivalRule)
                {
                    s += digit;
                }
                return string.Format("B{0}/S{1}", b, s);
            }
        }
    }
}
