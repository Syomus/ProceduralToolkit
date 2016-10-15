using UnityEngine;

namespace ProceduralToolkit.Examples.UI
{
    public class BreakoutUI : UIBase
    {
        public Camera mainCamera;
        public RectTransform leftPanel;

        private int wallWidth = 9;
        private int wallHeight = 7;
        private int wallHeightOffset = 5;
        private float paddleWidth = 1;
        private float ballSize = 0.5f;
        private float ballVelocityMagnitude = 5;

        private Breakout breakout;

        private void Awake()
        {
            breakout = new Breakout(mainCamera);
            Generate();

            InstantiateControl<TextControl>(leftPanel).Initialize("Use A/D or Left/Right to move");

            InstantiateControl<SliderControl>(leftPanel)
                .Initialize("Wall width", 1, 20, wallWidth, value =>
                {
                    wallWidth = value;
                    Generate();
                });

            InstantiateControl<SliderControl>(leftPanel)
                .Initialize("Wall height", 1, 20, wallHeight, value =>
                {
                    wallHeight = value;
                    Generate();
                });

            InstantiateControl<SliderControl>(leftPanel)
                .Initialize("Wall height offset", 1, 10, wallHeightOffset, value =>
                {
                    wallHeightOffset = value;
                    Generate();
                });

            InstantiateControl<SliderControl>(leftPanel)
                .Initialize("Paddle width", 1, 10, paddleWidth, value =>
                {
                    paddleWidth = value;
                    Generate();
                });

            InstantiateControl<SliderControl>(leftPanel)
                .Initialize("Ball size", 0.5f, 3f, ballSize, value =>
                {
                    ballSize = value;
                    Generate();
                });

            InstantiateControl<SliderControl>(leftPanel)
                .Initialize("Ball velocity", 1, 20, ballVelocityMagnitude, value =>
                {
                    ballVelocityMagnitude = value;
                    Generate();
                });

            InstantiateControl<ButtonControl>(leftPanel).Initialize("Generate", Generate);
        }

        private void Generate()
        {
            breakout.UpdateParameters(wallWidth, wallHeight, wallHeightOffset, paddleWidth, ballSize,
                ballVelocityMagnitude);
            breakout.ResetLevel();
        }

        private void Update()
        {
            breakout.Update();
        }
    }
}