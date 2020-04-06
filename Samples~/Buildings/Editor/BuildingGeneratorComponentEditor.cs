using UnityEngine;
using UnityEditor;

namespace ProceduralToolkit.Samples.Buildings
{
    [CustomEditor(typeof(BuildingGeneratorComponent))]
    public class BuildingGeneratorComponentEditor : Editor
    {
        private BuildingGeneratorComponent generator;

        private void OnEnable()
        {
            generator = (BuildingGeneratorComponent) target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.Space();
            if (GUILayout.Button("Generate building"))
            {
                var transform = generator.Generate();
                Undo.RegisterCreatedObjectUndo(transform.gameObject, "Generate building");
            }
        }
    }
}
