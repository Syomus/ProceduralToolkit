using ProceduralToolkit.Samples.UI;
using UnityEngine;

namespace ProceduralToolkit.Samples
{
    /// <summary>
    /// Configurator for Breakout with UI controls
    /// </summary>
    public class BreakoutConfigurator : ConfiguratorBase
    {
        public RectTransform leftPanel;
        [Space]
        public Breakout.Config config = new Breakout.Config();

        private Breakout breakout;

        private void Awake()
        {
            breakout = new Breakout();
            Generate();
            SetupSkyboxAndPalette();

            InstantiateControl<TextControl>(leftPanel).Initialize("Use A/D or Left/Right to move");

            InstantiateControl<SliderControl>(leftPanel)
                .Initialize("Wall width", 1, 20, config.wallWidth, value =>
                {
                    config.wallWidth = value;
                    Generate();
                });

            InstantiateControl<SliderControl>(leftPanel)
                .Initialize("Wall height", 1, 20, config.wallHeight, value =>
                {
                    config.wallHeight = value;
                    Generate();
                });

            InstantiateControl<SliderControl>(leftPanel)
                .Initialize("Wall height offset", 1, 10, config.wallHeightOffset, value =>
                {
                    config.wallHeightOffset = value;
                    Generate();
                });

            InstantiateControl<SliderControl>(leftPanel)
                .Initialize("Paddle width", 1, 10, config.paddleWidth, value =>
                {
                    config.paddleWidth = value;
                    Generate();
                });

            InstantiateControl<SliderControl>(leftPanel)
                .Initialize("Ball size", 0.5f, 3f, config.ballSize, value =>
                {
                    config.ballSize = value;
                    Generate();
                });

            InstantiateControl<SliderControl>(leftPanel)
                .Initialize("Ball velocity", 1, 20, config.ballVelocityMagnitude, value =>
                {
                    config.ballVelocityMagnitude = value;
                    Generate();
                });

            InstantiateControl<ButtonControl>(leftPanel).Initialize("Generate", Generate);
        }

        private void Update()
        {
            breakout.Update();

            UpdateSkybox();
        }

        private void Generate()
        {
            GeneratePalette();

            config.gradient = ColorE.Gradient(GetMainColorHSV(), GetSecondaryColorHSV());

            breakout.Generate(config);
        }
    }
}
