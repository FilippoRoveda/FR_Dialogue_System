using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DS.Inspectors
{
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
            DrawFiltersArea();
            DrawDialogueGroupArea();
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
        private void DrawDialogueGroupArea()
        {
            DS_InspectorUtilities.DrawHeader("Dialogue Group");
            selectedGroupIndexProperty.intValue = DS_InspectorUtilities.DrawPopup("Dialogue Group", selectedGroupIndexProperty, new string[] { });
            dialogueGroupProperty.DrawPropertyField();
            EditorGUILayout.Space(4);
        }

        private void DrawDialogueArea()
        {
            DS_InspectorUtilities.DrawHeader("Dialogue");
            selectedDialogueIndexProperty.intValue = DS_InspectorUtilities.DrawPopup("Dialogue", selectedDialogueIndexProperty, new string[] { });
            dialogueProperty.DrawPropertyField();
        }

    }
}
