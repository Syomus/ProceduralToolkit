using System.Collections.Generic;
using ProceduralToolkit.CellularAutomaton;
using ProceduralToolkit.Samples.UI;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.UI;

namespace ProceduralToolkit.Samples
{
    /// <summary>
    /// A demonstration of CellularAutomaton from the main library, draws the automaton simulation on a texture.
    /// Note that some of the rulesets need noise value different from the default setting.
    /// </summary>
    public class CellularAutomatonExample : ConfiguratorBase
    {
        public RectTransform leftPanel;
        public ToggleGroup toggleGroup;
        public RawImage image;
        [Space]
        public Config config = Config.life;

        private Color[] pixels;
        private Texture2D texture;
        private Cells cells;
        private Color deadColor;
        private Color aliveColor;

        #region Controls

        private TextControl header;
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

        #endregion Controls

        private void Awake()
        {
            pixels = new Color[config.width*config.height];
            texture = PTUtils.CreateTexture(config.width, config.height, Color.clear);
            image.texture = texture;

            cells = new Cells(config);

            #region Controls

            header = InstantiateControl<TextControl>(leftPanel);
            header.transform.SetAsFirstSibling();

            var currentRulesetName = RulesetName.Life;
            SelectRuleset(currentRulesetName);

            InstantiateToggle(RulesetName.Life, currentRulesetName);
            InstantiateToggle(RulesetName.Mazectric, currentRulesetName);
            InstantiateToggle(RulesetName.Coral, currentRulesetName);
            InstantiateToggle(RulesetName.WalledCities, currentRulesetName);
            InstantiateToggle(RulesetName.Coagulations, currentRulesetName);
            InstantiateToggle(RulesetName.Anneal, currentRulesetName);
            InstantiateToggle(RulesetName.Majority, currentRulesetName);

            InstantiateControl<SliderControl>(leftPanel).Initialize("Start noise", 0, 1, config.startNoise, value =>
            {
                config.startNoise = value;
                Generate();
            });

            InstantiateControl<ToggleControl>(leftPanel).Initialize("Alive borders", config.aliveBorders, value =>
            {
                config.aliveBorders = value;
                Generate();
            });

            InstantiateControl<ButtonControl>(leftPanel).Initialize("Generate", Generate);

            #endregion Controls

            Generate();
            SetupSkyboxAndPalette();
        }

        private void Update()
        {
            var job = new CellularAutomatonJob(cells, config);
            var handle = job.Schedule();
            handle.Complete();
            cells = job.cells;

            for (int x = 0; x < config.width; x++)
            {
                for (int y = 0; y < config.height; y++)
                {
                    pixels.SetXY(x, y, config.width, cells[x, y] ? aliveColor : deadColor);
                }
            }
            texture.SetPixels(pixels);
            texture.Apply();

            UpdateSkybox();
        }

        private void OnDestroy()
        {
            cells.Dispose();
        }

        private void Generate()
        {
            cells.FillWithNoise(config.startNoise);

            GeneratePalette();

            deadColor = GetMainColorHSV().WithSV(0.3f, 0.2f).ToColor();
            aliveColor = GetMainColor();
        }

        #region Controls

        private void SelectRuleset(RulesetName rulesetName)
        {
            config.ruleset = nameToRuleset[rulesetName];
            header.Initialize("Rulestring: " + config.ruleset);
        }

        private void InstantiateToggle(RulesetName rulesetName, RulesetName selectedRulesetName)
        {
            var toggle = InstantiateControl<ToggleControl>(toggleGroup.transform);
            toggle.Initialize(
                header: rulesetName.ToString(),
                value: rulesetName == selectedRulesetName,
                onValueChanged: isOn =>
                {
                    if (isOn)
                    {
                        SelectRuleset(rulesetName);
                        Generate();
                    }
                },
                toggleGroup: toggleGroup);
        }

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

        #endregion Controls
    }
}
