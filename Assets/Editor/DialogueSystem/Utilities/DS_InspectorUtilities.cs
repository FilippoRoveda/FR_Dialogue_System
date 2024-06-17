using UnityEditor;

namespace DS.Utilities
{
    public static class DS_InspectorUtilities
    {
        public static void DrawHeader(string label)
        {
            EditorGUILayout.LabelField(label, EditorStyles.boldLabel);
        }

        public static void DrawHelpBox(string message, MessageType messageType = MessageType.Info, bool isWide = true)
        {
            EditorGUILayout.HelpBox(message, messageType, isWide);
        }
        public static void DrawPropertyField(this SerializedProperty serializedProperty, bool interactable = true)
        {
            if(interactable == true) EditorGUILayout.PropertyField(serializedProperty);
            else
            {
                EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.PropertyField(serializedProperty);
                EditorGUI.EndDisabledGroup();
            }        
        }

        public static int DrawPopup(string label, SerializedProperty selectedIndexProperty, string[] options)
        {
            return EditorGUILayout.Popup(label,selectedIndexProperty.intValue, options);
        }
    }
}
