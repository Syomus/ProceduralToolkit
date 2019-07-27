using UnityEditor;
using UnityEngine;

namespace ProceduralToolkit.Examples
{
    [CustomEditor(typeof(LowPolyTerrainGeneratorConfigurator))]
    public class LowPolyTerrainGeneratorConfiguratorEditor : UnityEditor.Editor
    {
        private LowPolyTerrainGeneratorConfigurator generator;

        private void OnEnable()
        {
            generator = (LowPolyTerrainGeneratorConfigurator) target;
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
