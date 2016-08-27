using UnityEditor;
using UnityEngine;

namespace ProceduralToolkit.Examples.UI
{
    [CustomEditor(typeof (LowPolyTerrainGeneratorUI))]
    public class LowPolyTerrainGeneratorUIEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.Space();
            if (GUILayout.Button("Generate"))
            {
                ((LowPolyTerrainGeneratorUI) target).Generate();
            }
        }
    }
}