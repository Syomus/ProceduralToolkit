using UnityEditor;
using UnityEngine;

namespace ProceduralToolkit.Examples
{
    [CustomEditor(typeof (LowPolyTerrainGeneratorConfigurator))]
    public class LowPolyTerrainGeneratorConfiguratorEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.Space();
            if (GUILayout.Button("Generate"))
            {
                ((LowPolyTerrainGeneratorConfigurator) target).Generate();
            }
        }
    }
}