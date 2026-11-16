using System.Collections.Generic;
using UnityEngine;

namespace ProceduralToolkit.Samples
{
    /// <summary>
    /// 2D character generator
    /// </summary>
    /// <remarks>
    /// Sprites made by Kenney http://kenney.nl/
    /// </remarks>
    public class CharacterGenerator : MonoBehaviour
    {
        public Character character;
        public TextAsset namesJson;
        public List<Sprite> hairSprites = new List<Sprite>();
        public List<Sprite> bodySprites = new List<Sprite>();
        public List<Sprite> headSprites = new List<Sprite>();
        public List<Sprite> chestSprites = new List<Sprite>();
        public List<Sprite> legsSprites = new List<Sprite>();
        public List<Sprite> feetSprites = new List<Sprite>();
        public List<Sprite> weaponSprites = new List<Sprite>();
        public List<Sprite> shieldSprites = new List<Sprite>();

        private NameGenerator nameGenerator;

        private void Awake()
        {
            nameGenerator = new NameGenerator(namesJson);
        }

        public void Generate()
        {
            character.characterName.text = nameGenerator.fullName;
            character.hairRenderer.sprite = hairSprites.GetRandom();
            character.bodyRenderer.sprite = bodySprites.GetRandom();
            character.headRenderer.sprite = headSprites.GetRandom();
            character.chestRenderer.sprite = chestSprites.GetRandom();
            character.legsRenderer.sprite = legsSprites.GetRandom();
            character.feetRenderer.sprite = feetSprites.GetRandom();

            if (RandomE.Chance(0.3f))
            {
                character.weaponRenderer.enabled = true;
                character.weaponRenderer.sprite = weaponSprites.GetRandom();

                if (RandomE.Chance(0.3f))
                {
                    character.shieldRenderer.enabled = true;
                    character.shieldRenderer.sprite = shieldSprites.GetRandom();
                }
                else
                {
                    character.shieldRenderer.enabled = false;
                }
            }
            else
            {
                character.weaponRenderer.enabled = false;
                character.shieldRenderer.enabled = false;
            }
        }
    }
}
