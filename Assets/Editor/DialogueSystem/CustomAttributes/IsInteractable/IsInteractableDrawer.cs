using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(IsInteractable))]
public class ConditionalInteractableDrawer : PropertyDrawer
{

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        IsInteractable conditionalAttribute = (IsInteractable)attribute;
        bool isInteractable = conditionalAttribute.isInteractable;
        bool isInteractableField = CheckCondition(property.serializedObject.targetObject, conditionalAttribute.ConditionFieldName);

        bool previousGUIState = GUI.enabled;

        GUI.enabled = isInteractable || isInteractableField;
        EditorGUI.PropertyField(position, property, label);
        GUI.enabled = previousGUIState;     
    }

    private bool CheckCondition(object target, string conditionFieldName)
    {
        var conditionField = target.GetType().GetField(conditionFieldName, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);
        var conditionProperty = target.GetType().GetProperty(conditionFieldName, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);

        if (conditionField != null && conditionField.FieldType == typeof(bool))
        {
            return (bool)conditionField.GetValue(target);
        }
        else if (conditionProperty != null && conditionProperty.PropertyType == typeof(bool))
        {
            return (bool)conditionProperty.GetValue(target);
        }
        //DS.Utilities.Logger.Warning($"IsInteractableDrawer: No matching field or property found for {conditionFieldName}");
        return false;
    }
}
