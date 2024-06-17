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

        #region Unity callbacks
        private void OnEnable()
        {
            SetDialogueSerializedProperties();
        }
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawDialogueContainerArea();

            DS_DialogueContainerSO dialogueContainer = dialogueContainerProperty.objectReferenceValue as DS_DialogueContainerSO;

            if (dialogueContainer == null)
            {
                DS_InspectorUtilities.DrawHelpBox("Select a Dialogue Container to see the rest of the Inspector.");
                serializedObject.ApplyModifiedProperties();
                return;
            }

            DrawFiltersArea();

            bool currentStartingDialoguesOnlyFilter = startingDialogueOnlyProperty.boolValue;

            List<string> dialogueNames;
            string commonFolderPath = $"Assets/DialogueSystem/Dialogues/{dialogueContainer.GraphName}";
            string dialogueInfoMessage;

            if (groupedDialoguesProperty.boolValue == true)
            {
                List<string> groupNames = dialogueContainer.GetGroupNames();

                if (groupNames.Count == 0)
                {
                    DS_InspectorUtilities.DrawHelpBox("There are no Dialogue Groups in this Dialogue Container.", MessageType.Warning);
                    EditorGUILayout.Space(4);
                    DS_InspectorUtilities.DrawHelpBox("You need to select a Dialogue to make that component work properly at runtime", MessageType.Warning);
                    serializedObject.ApplyModifiedProperties();
                    return;
                }

                DrawDialogueGroupArea(dialogueContainer, groupNames);
                DS_DialogueGroupSO group = dialogueGroupProperty.objectReferenceValue as DS_DialogueGroupSO;
                dialogueNames = dialogueContainer.GetGroupedDialogueNames(group, currentStartingDialoguesOnlyFilter);
                commonFolderPath += $"/Groups/{group.GroupName}/Dialogues";
                dialogueInfoMessage = "There are no" + (currentStartingDialoguesOnlyFilter ? " Starting" : "") + " Dialogues in this Dialogue Group.";
            }
            else
            {
                dialogueNames = dialogueContainer.GetUngroupedDialogueNames(currentStartingDialoguesOnlyFilter);
                commonFolderPath += "/Global/Dialogues";
                dialogueInfoMessage = "There are no Ungrouped" + (currentStartingDialoguesOnlyFilter ? " Starting" : "") + "Dialogues in this Dialogue Container";
            }

            if (dialogueNames.Count == 0)
            {
                DS_InspectorUtilities.DrawHelpBox(dialogueInfoMessage, MessageType.Warning);
                EditorGUILayout.Space(4);
                DS_InspectorUtilities.DrawHelpBox("You need to select a Dialogue to make that component work properly at runtime", MessageType.Warning);
                serializedObject.ApplyModifiedProperties();
                return;
            }

            DrawDialogueArea(dialogueNames, commonFolderPath);

            serializedObject.ApplyModifiedProperties();
        }
        #endregion


        private void SetDialogueSerializedProperties()
        {
            dialogueContainerProperty = serializedObject.FindProperty("dialogueContainer");
            dialogueGroupProperty = serializedObject.FindProperty("dialogueGroup");
            dialogueProperty = serializedObject.FindProperty("dialogue");

            groupedDialoguesProperty = serializedObject.FindProperty("groupedDialogues");
            startingDialogueOnlyProperty = serializedObject.FindProperty("startingDialoguesOnly");

            selectedGroupIndexProperty = serializedObject.FindProperty("selectedGroupIndex");
            selectedDialogueIndexProperty = serializedObject.FindProperty("selectedDialogueIndex");
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
        private void DrawDialogueArea(List<string> dialogueNames, string commonFolderPath)
        {
            DS_InspectorUtilities.DrawHeader("Dialogue");

            int oldSelectedDialogueIndex = selectedDialogueIndexProperty.intValue;
            DS_DialogueSO oldDialogue = dialogueProperty.objectReferenceValue as DS_DialogueSO;

            string oldDialogueName = oldDialogue == null ? "" : oldDialogue.DialogueName;
            UpdateIndexOnUpdate(dialogueNames, selectedDialogueIndexProperty, oldSelectedDialogueIndex, oldDialogueName, oldDialogue == null);


            selectedDialogueIndexProperty.intValue = DS_InspectorUtilities.DrawPopup("Dialogue", selectedDialogueIndexProperty, dialogueNames.ToArray());
            string selectedDialogueName = dialogueNames[selectedDialogueIndexProperty.intValue];
            DS_DialogueSO selectedDialogue = DS_IOUtilities.LoadAsset<DS_DialogueSO>(commonFolderPath, selectedDialogueName);
            dialogueProperty.objectReferenceValue = selectedDialogue;
            dialogueProperty.DrawPropertyField(false);
        }

    }
}
