using UnityEditor;
using UnityEngine;

namespace ProceduralToolkit.Samples
{
    [CustomEditor(typeof(LowPolyTerrainExample))]
    public class LowPolyTerrainExampleEditor : Editor
    {
        private LowPolyTerrainExample generator;

        private void OnEnable()
        {
            generator = (LowPolyTerrainExample) target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.Space();
            if (GUILayout.Button("Generate mesh"))
            {
                Undo.RecordObjects(new Object[]
                {
                    generator,
                    generator.terrainMeshFilter,
                }, "Generate terrain");
                generator.Generate(randomizeConfig: false);
            }
            if (GUILayout.Button("Randomize config and generate mesh"))
            {
                Undo.RecordObjects(new Object[]
                {
                    generator,
                    generator.terrainMeshFilter,
                }, "Generate terrain");
                generator.Generate(randomizeConfig: true);
            }
        }
    }
}
