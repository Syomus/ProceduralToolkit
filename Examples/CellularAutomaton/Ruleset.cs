using System;
using System.Collections.Generic;
using UnityEngine;

namespace ProceduralToolkit.Examples
{
    /// <summary>
    /// Cellular automaton ruleset representation
    /// </summary>
    [Serializable]
    public struct Ruleset
    {
        #region Common rulesets

        public static Ruleset life { get { return new Ruleset("3", "23"); } }
        public static Ruleset highlife { get { return new Ruleset("36", "23"); } }
        public static Ruleset lifeWithoutDeath { get { return new Ruleset("3", "012345678"); } }
        public static Ruleset thirtyFour { get { return new Ruleset("34", "34"); } }
        public static Ruleset inverseLife { get { return new Ruleset("0123478", "34678"); } }
        public static Ruleset pseudoLife { get { return new Ruleset("357", "238"); } }
        public static Ruleset longLife { get { return new Ruleset("345", "5"); } }
        public static Ruleset dotLife { get { return new Ruleset("3", "023"); } }
        public static Ruleset dryLife { get { return new Ruleset("37", "23"); } }
        public static Ruleset seeds { get { return new Ruleset("2"); } }
        public static Ruleset serviettes { get { return new Ruleset("234"); } }
        public static Ruleset gnarl { get { return new Ruleset("1", "1"); } }
        public static Ruleset liveFreeOrDie { get { return new Ruleset("2", "0"); } }
        public static Ruleset dayAndNight { get { return new Ruleset("3678", "34678"); } }
        public static Ruleset replicator { get { return new Ruleset("1357", "1357"); } }
        public static Ruleset twoXTwo { get { return new Ruleset("36", "125"); } }
        public static Ruleset move { get { return new Ruleset("368", "245"); } }
        public static Ruleset maze { get { return new Ruleset("3", "12345"); } }
        public static Ruleset mazectric { get { return new Ruleset("3", "1234"); } }
        public static Ruleset amoeba { get { return new Ruleset("357", "1358"); } }
        public static Ruleset diamoeba { get { return new Ruleset("35678", "5678"); } }
        public static Ruleset coral { get { return new Ruleset("3", "45678"); } }
        public static Ruleset anneal { get { return new Ruleset("4678", "35678"); } }
        public static Ruleset majority { get { return new Ruleset("5678", "45678"); } }
        public static Ruleset walledCities { get { return new Ruleset("45678", "2345"); } }
        public static Ruleset stains { get { return new Ruleset("3678", "235678"); } }
        public static Ruleset coagulations { get { return new Ruleset("378", "235678"); } }
        public static Ruleset assimilation { get { return new Ruleset("345", "4567"); } }

        #endregion Common rulesets

        [SerializeField]
        private byte[] birthRule;
        [SerializeField]
        private byte[] survivalRule;

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
            for (int i = 0; i < birthRule.Length; i++)
            {
                if (birthRule[i] == aliveCells) return true;
            }
            return false;
        }

        public bool CanSurvive(int aliveCells)
        {
            for (int i = 0; i < survivalRule.Length; i++)
            {
                if (survivalRule[i] == aliveCells) return true;
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