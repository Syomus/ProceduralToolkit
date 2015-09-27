using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace ProceduralToolkit.Examples
{
    /// <summary>
    /// A simple terrain based on Perlin noise and coloured according to height
    /// </summary>
    public class TerrainMesh : MonoBehaviour
    {
        public MeshFilter meshFilter;

        public Slider xSizeSlider;
        public Text xSizeText;

        public Slider zSizeSlider;
        public Text zSizeText;

        public Slider xSegmentsSlider;
        public Text xSegmentsText;

        public Slider zSegmentsSlider;
        public Text zSegmentsText;

        public Slider noiseScaleSlider;
        public Text noiseScaleText;

        public Button generateButton;

        private int xSize = 10;
        private int zSize = 10;
        private int xSegments = 100;
        private int zSegments = 100;
        private int noiseScale = 10;

        private void Awake()
        {
            SetupSlider(xSizeSlider, xSizeText, xSize, value => xSize = value);
            SetupSlider(zSizeSlider, zSizeText, zSize, value => zSize = value);
            SetupSlider(xSegmentsSlider, xSegmentsText, xSegments, value => xSegments = value);
            SetupSlider(zSegmentsSlider, zSegmentsText, zSegments, value => zSegments = value);
            SetupSlider(noiseScaleSlider, noiseScaleText, noiseScale, value => noiseScale = value);

            generateButton.onClick.AddListener(Generate);

            Generate();
        }

        private void SetupSlider(Slider slider, Text text, float defaultValue, Action<int> onValueChanged)
        {
            slider.value = defaultValue;
            slider.onValueChanged.AddListener(value =>
            {
                int intValue = Mathf.FloorToInt(value);
                onValueChanged(intValue);
                text.text = intValue.ToString();
            });
            text.text = defaultValue.ToString();
        }

        private void Generate()
        {
            var mesh = TerrainDraft().ToMesh();
            meshFilter.mesh = mesh;
        }

        private MeshDraft TerrainDraft()
        {
            var draft = MeshDraft.Plane(xSize, zSize, xSegments, zSegments);
            draft.Move(Vector3.left*xSize/2 + Vector3.back*zSize/2);

            var gradient = RandomE.gradientHSV;

            var noiseOffset = new Vector2(Random.Range(0f, 100f), Random.Range(0f, 100f));
            draft.colors.Clear();
            for (int i = 0; i < draft.vertices.Count; i++)
            {
                // Calculate noise value
                Vector3 vertex = draft.vertices[i];
                float x = noiseScale*vertex.x/xSize + noiseOffset.x;
                float y = noiseScale*vertex.z/zSize + noiseOffset.y;
                float noise = Mathf.PerlinNoise(x, y);
                draft.vertices[i] = new Vector3(vertex.x, noise, vertex.z);

                draft.colors.Add(gradient.Evaluate(noise));
            }
            return draft;
        }
    }
}