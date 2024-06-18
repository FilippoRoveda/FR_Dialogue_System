using UnityEditor;

namespace DS.Utilities
{
    /// <summary>
    /// Inspector Utilities class to facilitate common custom inspector drawing operations.
    /// </summary>
    public static class DS_InspectorUtilities
    {
        /// <summary>
        /// Draw a bold header label field.
        /// </summary>
        /// <param name="label"></param>
        public static void DrawHeader(string label)
        {
            EditorGUILayout.LabelField(label, EditorStyles.boldLabel);
        }

        public static void DrawHelpBox(string message, MessageType messageType = MessageType.Info, bool isWide = true)
        {
            EditorGUILayout.HelpBox(message, messageType, isWide);
        }

        /// <summary>
        /// Draw a preperty field and decide if that will be interactable or not.
        /// </summary>
        /// <param name="serializedProperty"></param>
        /// <param name="interactable"></param>
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
