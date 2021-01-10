using System;

namespace ProceduralToolkit.CellularAutomaton
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
}
