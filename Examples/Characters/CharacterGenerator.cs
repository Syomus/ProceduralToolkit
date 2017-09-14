using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ProceduralToolkit.Examples
{
    /// <summary>
    /// 2D character generator
    /// </summary>
    /// <remarks>
    /// Sprites made by Tess Young https://www.patreon.com/chicmonster
    /// </remarks>
    public class CharacterGenerator : MonoBehaviour
    {
        public Text characterName;
        public SpriteRenderer headRenderer;
        public SpriteRenderer hairRenderer;
        public SpriteRenderer eyesRenderer;
        public SpriteRenderer bodyRenderer;
        public SpriteRenderer leftArmRenderer;
        public SpriteRenderer rightArmRenderer;
        public SpriteRenderer leftLegRenderer;
        public SpriteRenderer rightLegRenderer;
        [Space]
        public TextAsset namesJson;
        public List<Sprite> heads = new List<Sprite>();
        public List<Sprite> hairs = new List<Sprite>();
        public List<Sprite> eyes = new List<Sprite>();
        public List<Sprite> bodies = new List<Sprite>();
        public List<Sprite> arms = new List<Sprite>();
        public List<Sprite> legs = new List<Sprite>();

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
            characterName.text = nameGenerator.fullName;
            headRenderer.sprite = heads.GetRandom();
            hairRenderer.sprite = hairs.GetRandom();
            eyesRenderer.sprite = eyes.GetRandom();
            bodyRenderer.sprite = bodies.GetRandom();
            Sprite arm = arms.GetRandom();
            leftArmRenderer.sprite = arm;
            rightArmRenderer.sprite = arm;
            Sprite leg = legs.GetRandom();
            leftLegRenderer.sprite = leg;
            rightLegRenderer.sprite = leg;
        }
    }
}