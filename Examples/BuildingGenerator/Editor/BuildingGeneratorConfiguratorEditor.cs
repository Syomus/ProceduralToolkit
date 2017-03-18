using UnityEngine;
using UnityEditor;

namespace ProceduralToolkit.Examples
{
    [CustomEditor(typeof (BuildingGeneratorConfigurator))]
    public class BuildingGeneratorConfiguratorEditor : UnityEditor.Editor
    {
        private BuildingGeneratorConfigurator generator;

        private void OnEnable()
        {
            generator = (BuildingGeneratorConfigurator) target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Generate", EditorStyles.miniButton))
            {
                Undo.RecordObjects(new Object[]
                {
                    generator,
                    generator.buildingMeshFilter,
                    generator.platformMeshFilter,
                }, "Generate building");
                generator.Generate();
            }
        }
    }
}