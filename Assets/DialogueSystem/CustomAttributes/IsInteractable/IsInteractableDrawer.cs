using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(IsInteractable))]
public class ConditionalInteractableDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        IsInteractable conditionalAttribute = (IsInteractable)attribute;
        bool isInteractable = conditionalAttribute.isInteractable;

        // Salva lo stato di GUI.enabled
        bool previousGUIState = GUI.enabled;

        // Imposta l'interazione del campo in base alla condizione
        GUI.enabled = isInteractable;

        // Disegna il campo come normalmente farebbe Unity
        EditorGUI.PropertyField(position, property, label);

        // Ripristina lo stato di GUI.enabled
        GUI.enabled = previousGUIState;
    }
}
