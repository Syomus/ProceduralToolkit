using System;
using System.Collections;
using System.Runtime.CompilerServices;

namespace ProceduralToolkit.CellularAutomaton
{
    /// <summary>
    /// Cellular automaton ruleset representation
    /// </summary>
    [Serializable]
    public struct Ruleset
    {
        #region Common rulesets

        public static Ruleset life { [MethodImpl(MethodImplOptions.AggressiveInlining)] get => new("3", "23"); }
        public static Ruleset highlife { [MethodImpl(MethodImplOptions.AggressiveInlining)] get => new("36", "23"); }
        public static Ruleset lifeWithoutDeath { [MethodImpl(MethodImplOptions.AggressiveInlining)] get => new("3", "012345678"); }
        public static Ruleset thirtyFour { [MethodImpl(MethodImplOptions.AggressiveInlining)] get => new("34", "34"); }
        public static Ruleset inverseLife { [MethodImpl(MethodImplOptions.AggressiveInlining)] get => new("0123478", "34678"); }
        public static Ruleset pseudoLife { [MethodImpl(MethodImplOptions.AggressiveInlining)] get => new("357", "238"); }
        public static Ruleset longLife { [MethodImpl(MethodImplOptions.AggressiveInlining)] get => new("345", "5"); }
        public static Ruleset dotLife { [MethodImpl(MethodImplOptions.AggressiveInlining)] get => new("3", "023"); }
        public static Ruleset dryLife { [MethodImpl(MethodImplOptions.AggressiveInlining)] get => new("37", "23"); }
        public static Ruleset seeds { [MethodImpl(MethodImplOptions.AggressiveInlining)] get => new("2"); }
        public static Ruleset serviettes { [MethodImpl(MethodImplOptions.AggressiveInlining)] get => new("234"); }
        public static Ruleset gnarl { [MethodImpl(MethodImplOptions.AggressiveInlining)] get => new("1", "1"); }
        public static Ruleset liveFreeOrDie { [MethodImpl(MethodImplOptions.AggressiveInlining)] get => new("2", "0"); }
        public static Ruleset dayAndNight { [MethodImpl(MethodImplOptions.AggressiveInlining)] get => new("3678", "34678"); }
        public static Ruleset replicator { [MethodImpl(MethodImplOptions.AggressiveInlining)] get => new("1357", "1357"); }
        public static Ruleset twoXTwo { [MethodImpl(MethodImplOptions.AggressiveInlining)] get => new("36", "125"); }
        public static Ruleset move { [MethodImpl(MethodImplOptions.AggressiveInlining)] get => new("368", "245"); }
        public static Ruleset maze { [MethodImpl(MethodImplOptions.AggressiveInlining)] get => new("3", "12345"); }
        public static Ruleset mazectric { [MethodImpl(MethodImplOptions.AggressiveInlining)] get => new("3", "1234"); }
        public static Ruleset amoeba { [MethodImpl(MethodImplOptions.AggressiveInlining)] get => new("357", "1358"); }
        public static Ruleset diamoeba { [MethodImpl(MethodImplOptions.AggressiveInlining)] get => new("35678", "5678"); }
        public static Ruleset coral { [MethodImpl(MethodImplOptions.AggressiveInlining)] get => new("3", "45678"); }
        public static Ruleset anneal { [MethodImpl(MethodImplOptions.AggressiveInlining)] get => new("4678", "35678"); }
        public static Ruleset majority { [MethodImpl(MethodImplOptions.AggressiveInlining)] get => new("5678", "45678"); }
        public static Ruleset walledCities { [MethodImpl(MethodImplOptions.AggressiveInlining)] get => new("45678", "2345"); }
        public static Ruleset stains { [MethodImpl(MethodImplOptions.AggressiveInlining)] get => new("3678", "235678"); }
        public static Ruleset coagulations { [MethodImpl(MethodImplOptions.AggressiveInlining)] get => new("378", "235678"); }
        public static Ruleset assimilation { [MethodImpl(MethodImplOptions.AggressiveInlining)] get => new("345", "4567"); }

        #endregion Common rulesets

        /// <summary>
        /// Rule integer
        /// 
        /// Life: B3/S23
        /// S8 S7 S6 S5 S4 S3 S2 S1 S0  B8 B7 B6 B5 B4 B3 B2 B1 B0
        ///  0  0  0  0  0  1  1  0  0   0  0  0  0  0  1  0  0  0 - 000001100 000001000
        /// 
        /// B0 = 2 ^ 0 = 1
        /// B1 = 2 ^ 1 = 2
        /// B2 = 2 ^ 2 = 4
        /// B3 = 2 ^ 3 = 8
        /// B4 = 2 ^ 4 = 16
        /// B5 = 2 ^ 5 = 32
        /// B6 = 2 ^ 6 = 64
        /// B7 = 2 ^ 7 = 128
        /// B8 = 2 ^ 8 = 256
        /// 
        /// S0 = 2 ^ 9 = 512
        /// S1 = 2 ^ 10 = 1024
        /// S2 = 2 ^ 11 = 2048
        /// S3 = 2 ^ 12 = 4096
        /// S4 = 2 ^ 13 = 8192
        /// S5 = 2 ^ 14 = 16384
        /// S6 = 2 ^ 15 = 32768
        /// S7 = 2 ^ 16 = 65536
        /// S8 = 2 ^ 17 = 131072
        /// </summary>
        public int rule;

        private const int survivalOffset = 9;

        /// <param name="birthRuleString">Birth rule numbers, for example: "3"</param>
        /// <param name="survivalRuleString">Survival rule numbers, for example: "23"</param>
        public Ruleset(string birthRuleString = null, string survivalRuleString = null)
        {
            rule = ConvertRuleString(birthRuleString, survivalRuleString);
        }

        public bool CanSpawn(int aliveCells)
        {
            int pow = 1 << aliveCells;
            return (rule & pow) == pow;
        }

        public bool CanSurvive(int aliveCells)
        {
            int pow = 1 << (aliveCells + survivalOffset);
            return (rule & pow) == pow;
        }

        public static int ConvertRuleString(string birthRuleString, string survivalRuleString)
        {
            int rule = 0;
            if (!string.IsNullOrEmpty(birthRuleString))
            {
                foreach (char c in birthRuleString)
                {
                    if (!char.IsDigit(c)) continue;

                    byte digit = (byte) char.GetNumericValue(c);
                    int pow = 1 << digit;
                    rule |= pow;
                }
            }
            if (!string.IsNullOrEmpty(survivalRuleString))
            {
                foreach (char c in survivalRuleString)
                {
                    if (!char.IsDigit(c)) continue;

                    byte digit = (byte) char.GetNumericValue(c);
                    int pow = 1 << (digit + survivalOffset);
                    rule |= pow;
                }
            }
            return rule;
        }

        public override string ToString()
        {
            return $"B{GetBirthRuleString(rule)}/S{GetSurvivalRuleString(rule)}";
        }

        public static string GetBirthRuleString(int rule)
        {
            string b = "";
            var bitArray = new BitArray(new[] {rule});
            for (int i = 0; i < survivalOffset; i++)
            {
                if (bitArray[i])
                {
                    b += i;
                }
            }
            return b;
        }

        public static string GetSurvivalRuleString(int rule)
        {
            string s = "";
            var bitArray = new BitArray(new[] {rule});
            for (int i = survivalOffset; i < bitArray.Length; i++)
            {
                if (bitArray[i])
                {
                    s += i - survivalOffset;
                }
            }
            return s;
        }
    }
}
