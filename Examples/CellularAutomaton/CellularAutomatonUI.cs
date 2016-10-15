using System.Collections.Generic;
using ProceduralToolkit.Examples.UI;
using UnityEngine;
using UnityEngine.UI;

namespace ProceduralToolkit.Examples
{
    public class CellularAutomatonUI : UIBase
    {
        public RectTransform leftPanel;
        public ToggleGroup toggleGroup;
        public RawImage image;
        public Image background;

        private const float backgroundSaturation = 0.25f;
        private const float backgroundValue = 0.7f;
        private const float fadeDuration = 0.5f;

        private const float deadCellSaturation = 0.3f;
        private const float deadCellValue = 0.2f;
        private const float aliveCellSaturation = 0.7f;
        private const float aliveCellValue = 0.7f;

        private enum RulesetName
        {
            Life,
            Mazectric,
            Coral,
            WalledCities,
            Coagulations,
            Anneal,
            Majority,
        }

        private const int width = 128;
        private const int height = 128;
        private Color[] pixels = new Color[width*height];
        private Texture2D texture;
        private CellularAutomaton automaton;
        private Color deadColor;
        private Color aliveColor;
        private TextControl header;

        private Ruleset ruleset;
        private float startNoise = 0.25f;
        private bool aliveBorders = false;

        private RulesetName[] rulesetNames = new[]
        {
            RulesetName.Life,
            RulesetName.Mazectric,
            RulesetName.Coral,
            RulesetName.WalledCities,
            RulesetName.Coagulations,
            RulesetName.Anneal,
            RulesetName.Majority,
        };

        private Dictionary<RulesetName, Ruleset> nameToRuleset = new Dictionary<RulesetName, Ruleset>
        {
            {RulesetName.Life, Ruleset.life},
            {RulesetName.Mazectric, Ruleset.mazectric},
            {RulesetName.Coral, Ruleset.coral},
            {RulesetName.WalledCities, Ruleset.walledCities},
            {RulesetName.Coagulations, Ruleset.coagulations},
            {RulesetName.Anneal, Ruleset.anneal},
            {RulesetName.Majority, Ruleset.majority},
        };

        private void Awake()
        {
            texture = new Texture2D(width, height, TextureFormat.ARGB32, false, true)
            {
                filterMode = FilterMode.Point
            };
            texture.Clear(Color.clear);
            texture.Apply();
            image.texture = texture;

            header = InstantiateControl<TextControl>(leftPanel);
            header.transform.SetAsFirstSibling();

            var rulesetName = RulesetName.Life;
            SelectRuleset(rulesetName);

            for (int i = 0; i < rulesetNames.Length; i++)
            {
                RulesetName currentName = rulesetNames[i];
                var toggle = InstantiateControl<ToggleControl>(toggleGroup.transform);
                toggle.Initialize(
                    header: currentName.ToString(),
                    value: currentName == rulesetName,
                    onValueChanged: isOn =>
                    {
                        if (isOn)
                        {
                            SelectRuleset(currentName);
                            Generate();
                        }
                    },
                    toggleGroup: toggleGroup);
            }

            InstantiateControl<SliderControl>(leftPanel).Initialize("Start noise", 0, 1, startNoise, value =>
            {
                startNoise = value;
                Generate();
            });

            InstantiateControl<ToggleControl>(leftPanel).Initialize("Alive borders", aliveBorders, value =>
            {
                aliveBorders = value;
                Generate();
            });

            InstantiateControl<ButtonControl>(leftPanel).Initialize("Generate", Generate);

            Generate();
        }

        private void Update()
        {
            automaton.Simulate();
            Draw();
        }

        private void SelectRuleset(RulesetName rulesetName)
        {
            ruleset = nameToRuleset[rulesetName];

            header.Initialize("Rulestring: " + ruleset);
        }

        private void Generate()
        {
            automaton = new CellularAutomaton(width, height, ruleset, startNoise, aliveBorders);

            float hue = Random.value;
            deadColor = new ColorHSV(hue, deadCellSaturation, deadCellValue).ToColor();
            aliveColor = new ColorHSV(hue, aliveCellSaturation, aliveCellValue).ToColor();

            var backgroundColor = new ColorHSV(hue, backgroundSaturation, backgroundValue).complementary.ToColor();
            background.CrossFadeColor(backgroundColor, fadeDuration, true, false);
        }

        private void Draw()
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (automaton.cells[x, y] == CellState.Alive)
                    {
                        pixels[y*width + x] = aliveColor;
                    }
                    else
                    {
                        pixels[y*width + x] = deadColor;
                    }
                }
            }

            texture.SetPixels(pixels);
            texture.Apply();
        }
    }
}