using UnityEditor;
using System.Collections.Generic;


namespace DS.Inspectors
{ 
    using ScriptableObjects;
    using Utilities;

    [CustomEditor(typeof(DS_Dialogue))]
    public class DS_Inspector : Editor
    {
        //Dialogue SO
        private SerializedProperty dialogueContainerProperty;
        private SerializedProperty dialogueGroupProperty;
        private SerializedProperty dialogueProperty;
        //Filters
        private SerializedProperty groupedDialoguesProperty;
        private SerializedProperty startingDialogueOnlyProperty;
        //Indexes
        private SerializedProperty selectedGroupIndexProperty;
        private SerializedProperty selectedDialogueIndexProperty;

        private void OnEnable()
        {
            dialogueContainerProperty = serializedObject.FindProperty("dialogueContainer");
            dialogueGroupProperty = serializedObject.FindProperty("dialogueGroup");
            dialogueProperty = serializedObject.FindProperty("dialogue");

            groupedDialoguesProperty = serializedObject.FindProperty("groupedDialogues");
            startingDialogueOnlyProperty = serializedObject.FindProperty("startingDialoguesOnly");

            selectedGroupIndexProperty = serializedObject.FindProperty("selectedGroupIndex");
            selectedDialogueIndexProperty = serializedObject.FindProperty("selectedDialogueIndex");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawDialogueContainerArea();

            DS_DialogueContainerSO dialogueContainer = dialogueContainerProperty.objectReferenceValue as DS_DialogueContainerSO;

            if(dialogueContainer == null)
            {
                DS_InspectorUtilities.DrawHelpBox("Select a Dialogue Container to see the rest of the Inspector.");
                serializedObject.ApplyModifiedProperties();
                return;
            }

            DrawFiltersArea();

            List<string> dialogueNames;
            string commonFolderPath = $"Assets/DialogueSystem/Dialogues/{dialogueContainer.GraphName}";
            string dialogueInfoMessage;

            if(groupedDialoguesProperty.boolValue == true)
            {
                List<string> groupNames = dialogueContainer.GetGroupNames();

                if(groupNames.Count == 0)
                {
                    DS_InspectorUtilities.DrawHelpBox("There are no Dialogue Groups in this Dialogue Container.", MessageType.Warning);
                    serializedObject.ApplyModifiedProperties();
                    return;
                }

                DrawDialogueGroupArea(dialogueContainer, groupNames);
            }

            DrawDialogueArea();

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawDialogueContainerArea()
        {
            DS_InspectorUtilities.DrawHeader("Dialogue conatiner");
            dialogueContainerProperty.DrawPropertyField();
            EditorGUILayout.Space(4);
        }
        private void DrawFiltersArea()
        {
            DS_InspectorUtilities.DrawHeader("Filters");
            groupedDialoguesProperty.DrawPropertyField();
            startingDialogueOnlyProperty.DrawPropertyField();
            EditorGUILayout.Space(4);
        }
        private void DrawDialogueGroupArea(DS_DialogueContainerSO dialogueContainer, List<string> groupNames)
        {
            DS_InspectorUtilities.DrawHeader("Dialogue Group");

            int oldSelectedGroupIndex = selectedGroupIndexProperty.intValue;
            DS_DialogueGroupSO oldGroup = dialogueGroupProperty.objectReferenceValue as DS_DialogueGroupSO;

            string oldGroupName = oldGroup == null ? "" : oldGroup.GroupName;

            UpdateIndexOnUpdate(groupNames, selectedGroupIndexProperty, oldSelectedGroupIndex, oldGroupName, oldGroup == null);

            selectedGroupIndexProperty.intValue = DS_InspectorUtilities.DrawPopup("Dialogue Group", selectedGroupIndexProperty, groupNames.ToArray());
            string selectedGroupName = groupNames[selectedGroupIndexProperty.intValue];
            DS_DialogueGroupSO selectedGroup = DS_IOUtilities.LoadAsset<DS_DialogueGroupSO>($"Assets/DialogueSystem/Dialogues/{dialogueContainer.GraphName}/Groups/{selectedGroupName}", selectedGroupName);
            dialogueGroupProperty.objectReferenceValue = selectedGroup;
            dialogueGroupProperty.DrawPropertyField(false);
            EditorGUILayout.Space(4);
        }

        private void UpdateIndexOnUpdate(List<string> optionName, SerializedProperty indexProperty, int oldSelectedPropertyIndex, string oldPropertyName, bool isOldPropertyNull)
        {
            if (isOldPropertyNull == true)
            {
                indexProperty.intValue = 0;
                return;
            }

            if (oldSelectedPropertyIndex > optionName.Count - 1 || oldPropertyName != optionName[oldSelectedPropertyIndex])
            {
                if (optionName.Contains(oldPropertyName))
                {
                    indexProperty.intValue = optionName.IndexOf(oldPropertyName);
                }
                else
                {
                    indexProperty.intValue = 0;
                }
            }
        }

        private void DrawDialogueArea()
        {
            DS_InspectorUtilities.DrawHeader("Dialogue");
            selectedDialogueIndexProperty.intValue = DS_InspectorUtilities.DrawPopup("Dialogue", selectedDialogueIndexProperty, new string[] { });
            dialogueProperty.DrawPropertyField();
        }

    }
}
