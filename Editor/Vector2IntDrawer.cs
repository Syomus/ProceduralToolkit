using UnityEditor;
using UnityEngine;

namespace ProceduralToolkit.Editor
{
    [CustomPropertyDrawer(typeof(Vector2Int))]
    public class Vector2IntDrawer : PropertyDrawer
    {
        private static GUIContent[] xyLabels = new GUIContent[]
        {
            new GUIContent("X"),
            new GUIContent("Y"),
        };

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            position = EditorGUI.PrefixLabel(position, label);
            position.xMin -= 1;
            position.xMax -= (position.width - 4)/3 + 2;

            SerializedProperty valuesIterator = property.Copy();
            valuesIterator.NextVisible(true);
            EditorGUI.MultiPropertyField(position, xyLabels, valuesIterator);

            EditorGUI.EndProperty();
        }
    }
}