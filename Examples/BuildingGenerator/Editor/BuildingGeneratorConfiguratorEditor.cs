using UnityEngine;
using UnityEditor;

namespace ProceduralToolkit.Examples
{
    [CustomEditor(typeof (BuildingGeneratorConfigurator))]
    public class BuildingGeneratorConfiguratorEditor : UnityEditor.Editor
    {
        private BuildingGeneratorConfigurator generator;

        private void OnEnable()
        {
            generator = (BuildingGeneratorConfigurator) target;
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
                    generator.buildingMeshFilter,
                    generator.platformMeshFilter,
                }, "Generate building");
                generator.Generate(randomizeConfig: false);
            }
            if (GUILayout.Button("Randomize config and generate mesh"))
            {
                Undo.RecordObjects(new Object[]
                {
                    generator,
                    generator.buildingMeshFilter,
                    generator.platformMeshFilter,
                }, "Generate building");
                generator.Generate(randomizeConfig: true);
            }
        }
    }
}