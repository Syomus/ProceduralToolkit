using UnityEditor;
using UnityEngine;

namespace ProceduralToolkit.Samples
{
    [CustomEditor(typeof(ChairGeneratorConfigurator))]
    public class ChairGeneratorConfiguratorEditor : Editor
    {
        private ChairGeneratorConfigurator generator;

        private void OnEnable()
        {
            generator = (ChairGeneratorConfigurator) target;
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
                    generator.chairMeshFilter,
                    generator.platformMeshFilter,
                }, "Generate chair");
                generator.Generate(randomizeConfig: false);
            }
            if (GUILayout.Button("Randomize config and generate mesh"))
            {
                Undo.RecordObjects(new Object[]
                {
                    generator,
                    generator.chairMeshFilter,
                    generator.platformMeshFilter,
                }, "Generate chair");
                generator.Generate(randomizeConfig: true);
            }
        }
    }
}
