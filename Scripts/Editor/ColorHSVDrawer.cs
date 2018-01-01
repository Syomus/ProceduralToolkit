using UnityEditor;
using UnityEngine;

namespace ProceduralToolkit.Editor
{
    /// <summary>
    /// PropertyDrawer for ColorHSV
    /// </summary>
    [CustomPropertyDrawer(typeof(ColorHSV))]
    public class ColorHSVDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var colorHsv = new ColorHSV();

            SerializedProperty valuesIterator = property.Copy();
            valuesIterator.NextVisible(true);
            for (int i = 0; i < 4; i++)
            {
                colorHsv[i] = valuesIterator.floatValue;
                valuesIterator.NextVisible(false);
            }

            EditorGUI.BeginChangeCheck();
            var color = EditorGUI.ColorField(position, label, colorHsv.ToColor());
            if (EditorGUI.EndChangeCheck())
            {
                colorHsv = new ColorHSV(color);
                valuesIterator = property.Copy();
                valuesIterator.NextVisible(true);
                for (int i = 0; i < 4; i++)
                {
                    valuesIterator.floatValue = colorHsv[i];
                    valuesIterator.NextVisible(false);
                }
            }

            EditorGUI.EndProperty();
        }
    }
}
