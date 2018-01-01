using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ProceduralToolkit.Examples
{
    /// <summary>
    /// 2D character generator
    /// </summary>
    /// <remarks>
    /// Sprites made by Kenney http://kenney.nl/
    /// </remarks>
    public class CharacterGenerator : MonoBehaviour
    {
        public bool constantSeed = false;
        public Text characterName;
        public SpriteRenderer hairRenderer;
        public SpriteRenderer bodyRenderer;
        public SpriteRenderer headRenderer;
        public SpriteRenderer chestRenderer;
        public SpriteRenderer legsRenderer;
        public SpriteRenderer feetRenderer;
        public SpriteRenderer weaponRenderer;
        public SpriteRenderer shieldRenderer;
        [Space]
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

            Generate();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Generate();
            }
        }

        private void Generate()
        {
            if (constantSeed)
            {
                Random.InitState(0);
            }

            characterName.text = nameGenerator.fullName;
            hairRenderer.sprite = hairSprites.GetRandom();
            bodyRenderer.sprite = bodySprites.GetRandom();
            headRenderer.sprite = headSprites.GetRandom();
            chestRenderer.sprite = chestSprites.GetRandom();
            legsRenderer.sprite = legsSprites.GetRandom();
            feetRenderer.sprite = feetSprites.GetRandom();
            if (RandomE.Chance(0.3f))
            {
                weaponRenderer.enabled = true;
                weaponRenderer.sprite = weaponSprites.GetRandom();

                if (RandomE.Chance(0.3f))
                {
                    shieldRenderer.enabled = true;
                    shieldRenderer.sprite = shieldSprites.GetRandom();
                }
                else
                {
                    shieldRenderer.enabled = false;
                }
            }
            else
            {
                weaponRenderer.enabled = false;
                shieldRenderer.enabled = false;
            }
        }
    }
}
