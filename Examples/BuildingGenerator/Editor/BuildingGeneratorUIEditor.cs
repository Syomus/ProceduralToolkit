using UnityEditor;
using UnityEngine;

namespace ProceduralToolkit.Examples.UI
{
    [CustomEditor(typeof (BuildingGeneratorUI))]
    public class BuildingGeneratorUIEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.Space();
            if (GUILayout.Button("Generate"))
            {
                ((BuildingGeneratorUI) target).Generate();
            }
        }
    }
}