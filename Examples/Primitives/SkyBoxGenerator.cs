using System.Collections.Generic;
using UnityEngine;

namespace ProceduralToolkit.Examples
{
    public class SkyBoxGenerator : MonoBehaviour
    {
        private const float generateInterval = 5;
        private const float lerpSpeed = 1/generateInterval;

        private Material skybox;
        private List<ColorHSV> targetPalette = new List<ColorHSV>();
        private List<ColorHSV> currentPalette = new List<ColorHSV>();
        private float nextGenerateTime;

        private void Awake()
        {
            skybox = new Material(RenderSettings.skybox);
            RenderSettings.skybox = skybox;
            GeneratePalette();
            currentPalette.AddRange(targetPalette);
        }

        private void Update()
        {
            if (Time.time > nextGenerateTime)
            {
                nextGenerateTime = Time.time + generateInterval;
                GeneratePalette();
            }

            LerpSkybox(skybox, currentPalette, targetPalette, 1, 2, 3);
        }

        private void GeneratePalette()
        {
            targetPalette = new ColorHSV(Random.value, 0.5f, 0.75f).GetTriadicPalette();
            targetPalette.Shuffle();
            targetPalette.Add(ColorHSV.Lerp(targetPalette[1], targetPalette[2], 0.5f));
        }

        public static void LerpSkybox(
            Material skybox,
            List<ColorHSV> currentPalette,
            List<ColorHSV> targetPalette,
            int skyColorIndex,
            int horizonColorIndex,
            int groundColorIndex)
        {
            for (int i = 0; i < currentPalette.Count; i++)
            {
                currentPalette[i] = ColorHSV.Lerp(currentPalette[i], targetPalette[i], Time.deltaTime*lerpSpeed);
            }

            skybox.SetColor("_SkyColor", currentPalette[skyColorIndex].ToColor());
            skybox.SetColor("_HorizonColor", currentPalette[horizonColorIndex].ToColor());
            skybox.SetColor("_GroundColor", currentPalette[groundColorIndex].ToColor());
        }
    }
}