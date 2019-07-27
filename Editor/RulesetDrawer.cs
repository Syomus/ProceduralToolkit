using System.Text;
using UnityEditor;
using UnityEngine;

namespace ProceduralToolkit.Editor
{
    /// <summary>
    /// PropertyDrawer for CellularAutomaton.Ruleset
    /// </summary>
    [CustomPropertyDrawer(typeof(CellularAutomaton.Ruleset))]
    public class RulesetDrawer : PropertyDrawer
    {
        private const float labelWidth = 13;
        private const float labelSpacing = 1;
        private const float dropdownWidth = 13;
        private const float dropdownSpacing = 2;
        private const string birthRuleName = "birthRule";
        private const string survivalRuleName = "survivalRule";

        private readonly GUIStyle dropdownStyle = (GUIStyle) "StaticDropdown";
        private readonly GUIContent[] options = new GUIContent[]
        {
            new GUIContent("Life"),
            new GUIContent("Highlife"),
            new GUIContent("LifeWithoutDeath"),
            new GUIContent("ThirtyFour"),
            new GUIContent("InverseLife"),
            new GUIContent("PseudoLife"),
            new GUIContent("LongLife"),
            new GUIContent("DotLife"),
            new GUIContent("DryLife"),
            new GUIContent("Seeds"),
            new GUIContent("Serviettes"),
            new GUIContent("Gnarl"),
            new GUIContent("LiveFreeOrDie"),
            new GUIContent("DayAndNight"),
            new GUIContent("Replicator"),
            new GUIContent("TwoXTwo"),
            new GUIContent("Move"),
            new GUIContent("Maze"),
            new GUIContent("Mazectric"),
            new GUIContent("Amoeba"),
            new GUIContent("Diamoeba"),
            new GUIContent("Coral"),
            new GUIContent("Anneal"),
            new GUIContent("Majority"),
            new GUIContent("WalledCities"),
            new GUIContent("Stains"),
            new GUIContent("Coagulations"),
            new GUIContent("Assimilation"),
        };
        private readonly CellularAutomaton.Ruleset[] rulesets = new CellularAutomaton.Ruleset[]
        {
            CellularAutomaton.Ruleset.life,
            CellularAutomaton.Ruleset.highlife,
            CellularAutomaton.Ruleset.lifeWithoutDeath,
            CellularAutomaton.Ruleset.thirtyFour,
            CellularAutomaton.Ruleset.inverseLife,
            CellularAutomaton.Ruleset.pseudoLife,
            CellularAutomaton.Ruleset.longLife,
            CellularAutomaton.Ruleset.dotLife,
            CellularAutomaton.Ruleset.dryLife,
            CellularAutomaton.Ruleset.seeds,
            CellularAutomaton.Ruleset.serviettes,
            CellularAutomaton.Ruleset.gnarl,
            CellularAutomaton.Ruleset.liveFreeOrDie,
            CellularAutomaton.Ruleset.dayAndNight,
            CellularAutomaton.Ruleset.replicator,
            CellularAutomaton.Ruleset.twoXTwo,
            CellularAutomaton.Ruleset.move,
            CellularAutomaton.Ruleset.maze,
            CellularAutomaton.Ruleset.mazectric,
            CellularAutomaton.Ruleset.amoeba,
            CellularAutomaton.Ruleset.diamoeba,
            CellularAutomaton.Ruleset.coral,
            CellularAutomaton.Ruleset.anneal,
            CellularAutomaton.Ruleset.majority,
            CellularAutomaton.Ruleset.walledCities,
            CellularAutomaton.Ruleset.stains,
            CellularAutomaton.Ruleset.coagulations,
            CellularAutomaton.Ruleset.assimilation,
        };

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var birthRule = property.FindPropertyRelative(birthRuleName);
            var survivalRule = property.FindPropertyRelative(survivalRuleName);

            position = EditorGUI.PrefixLabel(position, label);

            float ruleWidth = (position.width - dropdownWidth)/2;
            Rect ruleRect = new Rect(position.x - labelSpacing, position.y, ruleWidth, position.height);
            DrawRule(birthRule, ruleRect, "B");

            ruleRect.x += ruleWidth + labelSpacing;
            DrawRule(survivalRule, ruleRect, "S");

            int oldIndentLevel = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            {
                Rect dropdownRect = new Rect(ruleRect.xMax + dropdownSpacing, position.y, dropdownWidth, position.height);
                int selected = EditorGUI.Popup(dropdownRect, -1, options, dropdownStyle);
                if (selected >= 0)
                {
                    SelectRuleset(birthRule, survivalRule, selected);
                }
            }
            EditorGUI.indentLevel = oldIndentLevel;

            EditorGUI.EndProperty();
        }

        private void SelectRuleset(SerializedProperty birthRule, SerializedProperty survivalRule, int selected)
        {
            var ruleset = rulesets[selected];

            birthRule.ClearArray();
            for (int i = 0; i < ruleset.birthRule.Length; i++)
            {
                birthRule.InsertArrayElementAtIndex(i);
                var element = birthRule.GetArrayElementAtIndex(i);
                element.intValue = ruleset.birthRule[i];
            }

            survivalRule.ClearArray();
            for (int i = 0; i < ruleset.survivalRule.Length; i++)
            {
                survivalRule.InsertArrayElementAtIndex(i);
                var element = survivalRule.GetArrayElementAtIndex(i);
                element.intValue = ruleset.survivalRule[i];
            }
        }

        private void DrawRule(SerializedProperty rule, Rect position, string label)
        {
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
