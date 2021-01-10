using ProceduralToolkit.CellularAutomaton;
using UnityEditor;
using UnityEngine;

namespace ProceduralToolkit.Editor
{
    /// <summary>
    /// PropertyDrawer for CellularAutomaton.Ruleset
    /// </summary>
    [CustomPropertyDrawer(typeof(Ruleset))]
    public class RulesetDrawer : PropertyDrawer
    {
        private const float labelWidth = 13;
        private const float labelSpacing = 1;
        private const float dropdownWidth = 13;
        private const float dropdownSpacing = 2;
        private const string ruleName = "rule";

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
        private readonly Ruleset[] rulesets = new Ruleset[]
        {
            Ruleset.life,
            Ruleset.highlife,
            Ruleset.lifeWithoutDeath,
            Ruleset.thirtyFour,
            Ruleset.inverseLife,
            Ruleset.pseudoLife,
            Ruleset.longLife,
            Ruleset.dotLife,
            Ruleset.dryLife,
            Ruleset.seeds,
            Ruleset.serviettes,
            Ruleset.gnarl,
            Ruleset.liveFreeOrDie,
            Ruleset.dayAndNight,
            Ruleset.replicator,
            Ruleset.twoXTwo,
            Ruleset.move,
            Ruleset.maze,
            Ruleset.mazectric,
            Ruleset.amoeba,
            Ruleset.diamoeba,
            Ruleset.coral,
            Ruleset.anneal,
            Ruleset.majority,
            Ruleset.walledCities,
            Ruleset.stains,
            Ruleset.coagulations,
            Ruleset.assimilation,
        };

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var ruleProperty = property.FindPropertyRelative(ruleName);

            position = EditorGUI.PrefixLabel(position, label);

            position = DrawRule(ruleProperty, position);

            DrawDropdown(ruleProperty, position);

            EditorGUI.EndProperty();
        }

        private Rect DrawRule(SerializedProperty ruleProperty, Rect position)
        {
            string birthRuleString = Ruleset.GetBirthRuleString(ruleProperty.intValue);
            string survivalRuleString = Ruleset.GetSurvivalRuleString(ruleProperty.intValue);

            float ruleWidth = (position.width - dropdownWidth)/2;
            var ruleRect = new Rect(position.x - labelSpacing, position.y, ruleWidth, position.height);

            float oldLabelWidth = EditorGUIUtility.labelWidth;
            int oldIndentLevel = EditorGUI.indentLevel;
            EditorGUIUtility.labelWidth = labelWidth;
            EditorGUI.indentLevel = 0;
            {
                birthRuleString = EditorGUI.TextField(ruleRect, "B", birthRuleString);

                ruleRect.x += ruleWidth + labelSpacing;

                survivalRuleString = EditorGUI.TextField(ruleRect, "S", survivalRuleString);
            }
            EditorGUIUtility.labelWidth = oldLabelWidth;
            EditorGUI.indentLevel = oldIndentLevel;

            ruleProperty.intValue = Ruleset.ConvertRuleString(birthRuleString, survivalRuleString);

            return ruleRect;
        }

        private void DrawDropdown(SerializedProperty ruleProperty, Rect position)
        {
            int oldIndentLevel = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            {
                Rect dropdownRect = new Rect(position.xMax + dropdownSpacing, position.y, dropdownWidth, position.height);
                int selected = EditorGUI.Popup(dropdownRect, -1, options, dropdownStyle);
                if (selected >= 0)
                {
                    ruleProperty.intValue = rulesets[selected].rule;
                }
            }
            EditorGUI.indentLevel = oldIndentLevel;
        }
    }
}
