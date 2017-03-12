using UnityEditor;
using UnityEngine;

namespace ProceduralToolkit.Examples
{
    [CustomEditor(typeof (ChairGeneratorConfigurator))]
    public class ChairGeneratorConfiguratorEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.Space();
            if (GUILayout.Button("Generate"))
            {
                ((ChairGeneratorConfigurator) target).Generate();
            }
        }
    }
}