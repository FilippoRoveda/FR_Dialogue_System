using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(IsVisible))]
public class ConditionalVisibleDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        IsVisible conditionalAttribute = (IsVisible)attribute;
        return conditionalAttribute.isVisible ? EditorGUI.GetPropertyHeight(property, label, true) : 0;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        IsVisible conditionalAttribute = (IsVisible)attribute;

        if (conditionalAttribute.isVisible)
        {
            EditorGUI.PropertyField(position, property, label, true);
        }
    }
}
