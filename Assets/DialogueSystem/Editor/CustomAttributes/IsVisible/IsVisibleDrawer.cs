using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(IsVisible))]
public class ConditionalVisibleDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        IsVisible conditionalAttribute = (IsVisible)attribute;
        object target = property.serializedObject.targetObject;

        return conditionalAttribute.isVisible == true || CheckCondition(target, conditionalAttribute.ConditionFieldName) ? EditorGUI.GetPropertyHeight(property, label, true) : 0;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        IsVisible conditionalAttribute = (IsVisible)attribute;
        object target = property.serializedObject.targetObject;

        if (conditionalAttribute.isVisible == true || CheckCondition(target, conditionalAttribute.ConditionFieldName))
        {
            EditorGUI.PropertyField(position, property, label, true);
        }
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
        return false;
    }
}
