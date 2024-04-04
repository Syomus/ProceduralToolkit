using System;
using UnityEngine;

namespace ProceduralToolkit.Samples
{
    [Serializable]
    public class NameGenerator
    {
        public string[] femaleNames = new string[0];
        public string[] maleNames = new string[0];
        public string[] lastNames = new string[0];

        public string femaleName => femaleNames.GetRandom();
        public string maleName => maleNames.GetRandom();
        public string firstName => RandomE.Chance(0.5f) ? femaleName : maleName;
        public string lastName => lastNames.GetRandom();
        public string fullName => $"{firstName} {lastName}";

        /// <param name="namesJson">JSON file with string arrays for femaleNames, maleNames and lastNames</param>
        public NameGenerator(TextAsset namesJson)
        {
            JsonUtility.FromJsonOverwrite(namesJson.text, this);
        }
    }
}
