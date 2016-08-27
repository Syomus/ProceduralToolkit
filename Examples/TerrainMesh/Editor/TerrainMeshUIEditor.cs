using UnityEditor;
using UnityEngine;

namespace ProceduralToolkit.Examples.UI
{
    [CustomEditor(typeof (TerrainMeshUI))]
    public class TerrainMeshUIEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.Space();
            if (GUILayout.Button("Generate"))
            {
                ((TerrainMeshUI) target).Generate();
            }
        }
    }
}