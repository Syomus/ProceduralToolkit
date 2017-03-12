using UnityEditor;
using UnityEngine;

namespace ProceduralToolkit.Examples
{
    [CustomEditor(typeof (BuildingGeneratorConfigurator))]
    public class BuildingGeneratorConfiguratorEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.Space();
            if (GUILayout.Button("Generate"))
            {
                ((BuildingGeneratorConfigurator) target).Generate();
            }
        }
    }
}