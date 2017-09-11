using System.Text;
using UnityEditor;
using UnityEngine;

namespace ProceduralToolkit.Editor
{
    [CustomPropertyDrawer(typeof(CellularAutomaton.Ruleset))]
    public class RulesetDrawer : PropertyDrawer
    {
        private const float labelWidth = 13;
        private const float spacing = 1;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            position = EditorGUI.PrefixLabel(position, label);

            Rect bRect = new Rect(position.x - spacing, position.y, position.width/2, position.height);
            DrawRule(property, "birthRule", bRect, "B");

            Rect sRect = bRect;
            sRect.x += bRect.width + spacing;
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

            float oldLabelWidth = EditorGUIUtility.labelWidth;
            int oldIndentLevel = EditorGUI.indentLevel;
            EditorGUIUtility.labelWidth = labelWidth;
            EditorGUI.indentLevel = 0;
            string ruleString = EditorGUI.TextField(position, label, stringBuilder.ToString());
            EditorGUIUtility.labelWidth = oldLabelWidth;
            EditorGUI.indentLevel = oldIndentLevel;

            var ruleList = CellularAutomaton.Ruleset.ConvertRuleStringToList(ruleString);
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