using System.Text;
using ProceduralToolkit.Examples;
using UnityEditor;
using UnityEngine;

namespace ProceduralToolkit
{
    [CustomPropertyDrawer(typeof(Ruleset))]
    public class RulesetDrawer : PropertyDrawer
    {
        private const float spacing = 5;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            position = EditorGUI.PrefixLabel(position, label);

            Rect bRect = new Rect(position.x, position.y, position.width/2, position.height);
            DrawRule(property, "birthRule", bRect, "B");

            Rect sRect = bRect;
            sRect.x += position.width/2 + spacing;
            sRect.width -= spacing;
            DrawRule(property, "survivalRule", sRect, "S");

            EditorGUI.EndProperty();
        }

        private void DrawRule(SerializedProperty property, string name, Rect position, string label)
        {
            var rule = property.FindPropertyRelative(name);
            var stringBuilder = new StringBuilder();
            for (int i = 0; i < rule.arraySize; i++)
            {
                stringBuilder.Append(rule.GetArrayElementAtIndex(i).intValue);
            }

            float labelWidth = EditorGUIUtility.labelWidth;
            int indentLevel = EditorGUI.indentLevel;
            EditorGUIUtility.labelWidth = 15;
            EditorGUI.indentLevel = 0;
            string ruleString = EditorGUI.TextField(position, label, stringBuilder.ToString());
            EditorGUIUtility.labelWidth = labelWidth;
            EditorGUI.indentLevel = indentLevel;

            var ruleList = Ruleset.ConvertRuleStringToList(ruleString);
            rule.ClearArray();
            for (int i = 0; i < ruleList.Count; i++)
            {
                rule.InsertArrayElementAtIndex(i);
                var element = rule.GetArrayElementAtIndex(i);
                element.intValue = ruleList[i];
            }
        }
    }
}