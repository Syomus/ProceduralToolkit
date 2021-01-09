using System;
using System.Collections.Generic;

namespace ProceduralToolkit.CellularAutomata
{
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
