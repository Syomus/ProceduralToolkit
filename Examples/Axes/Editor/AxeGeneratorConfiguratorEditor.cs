using UnityEditor;
using UnityEngine;

namespace ProceduralToolkit.Examples.Axe
{
    [CustomEditor(typeof (AxeGeneratorConfigurator))]
    public class AxeGeneratorConfiguratorEditor : UnityEditor.Editor
    {
        private AxeGeneratorConfigurator generator;

        private void OnEnable()
        {
            generator = (AxeGeneratorConfigurator) target;
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
                    generator.meshFilter,
                    generator.platformMeshFilter,
                }, "Generate axe");
                generator.Generate(randomizeConfig: false);
            }
            if (GUILayout.Button("Randomize config and generate mesh"))
            {
                Undo.RecordObjects(new Object[]
                {
                    generator,
                    generator.meshFilter,
                    generator.platformMeshFilter,
                }, "Generate axe");
                generator.Generate(randomizeConfig: true);
            }
        }
    }
}