using UnityEditor;
using UnityEngine;

namespace ProceduralToolkit.Examples
{
    [CustomEditor(typeof (ChairGeneratorConfigurator))]
    public class ChairGeneratorConfiguratorEditor : UnityEditor.Editor
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
            if (GUILayout.Button("Generate"))
            {
                Undo.RecordObjects(new Object[]
                {
                    generator,
                    generator.chairMeshFilter,
                    generator.platformMeshFilter,
                }, "Generate chair");
                generator.Generate();
            }
        }
    }
}