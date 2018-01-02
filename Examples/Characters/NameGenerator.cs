using System;
using UnityEngine;

namespace ProceduralToolkit.Examples
{
    [Serializable]
    public class NameGenerator
    {
        public string[] femaleNames = new string[0];
        public string[] maleNames = new string[0];
        public string[] lastNames = new string[0];

        public string femaleName { get { return femaleNames.GetRandom(); } }
        public string maleName { get { return maleNames.GetRandom(); } }
        public string firstName { get { return RandomE.Chance(0.5f) ? femaleName : maleName; } }
        public string lastName { get { return lastNames.GetRandom(); } }
        public string fullName { get { return string.Format("{0} {1}", firstName, lastName); } }

        /// <param name="namesJson">JSON file with string arrays for femaleNames, maleNames and lastNames</param>
        public NameGenerator(TextAsset namesJson)
        {
            JsonUtility.FromJsonOverwrite(namesJson.text, this);
        }
    }
}
