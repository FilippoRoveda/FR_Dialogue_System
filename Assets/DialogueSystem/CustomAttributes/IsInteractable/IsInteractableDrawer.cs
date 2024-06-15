using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(IsInteractable))]
public class ConditionalInteractableDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        IsInteractable conditionalAttribute = (IsInteractable)attribute;
        bool isInteractable = conditionalAttribute.isInteractable;

        bool previousGUIState = GUI.enabled;

        GUI.enabled = isInteractable;
        EditorGUI.PropertyField(position, property, label);
        GUI.enabled = previousGUIState;
    }
}
