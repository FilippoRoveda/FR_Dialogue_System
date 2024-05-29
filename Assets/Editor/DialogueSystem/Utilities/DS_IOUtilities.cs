using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditor.Experimental.GraphView;


namespace DS.Utilities
{
    using Elements;
    using System;
    using Windows;
    using Data.Save;
    using ScriptableObjects;
    using Data;
    using System.Linq;

    public static class DS_IOUtilities
    {
        private static DS_GraphView graphView;
        private static string graphFileName;
        private static string containerFolderPath;

        private static List<DS_Group> groups;
        private static List<DS_Node> nodes;

        public static Dictionary<string, DS_DialogueGroup_SO> createdGroupsSO;
        public static Dictionary<string, DS_Dialogue_SO> createdDialoguesSO;


        public static void Initialize(DS_GraphView graphView, string graphName)
        {
            DS_IOUtilities.graphView = graphView;
            graphFileName = graphName;
            containerFolderPath = $"Assets/DialogueSystem/Dialogues/{graphFileName}";

            groups = new List<DS_Group>();
            nodes = new List<DS_Node>();
            createdGroupsSO = new Dictionary<string, DS_DialogueGroup_SO>();
            createdDialoguesSO = new Dictionary<string, DS_Dialogue_SO>();
        }

        #region Save methods
        public static void SaveGraph()
        {
            CreateStaticFolders();
            GetElementsFromGraphView();

            DS_Graph_SaveData_SO graphData = CreateAsset<DS_Graph_SaveData_SO>("Assets/Editor/DialogueSystem/Graphs", $"{graphFileName}_Graph");
            graphData.Initialize(graphFileName);

            DS_DialogueContainer_SO dialogueContainer = CreateAsset<DS_DialogueContainer_SO>(containerFolderPath, graphFileName);
            dialogueContainer.Initialize(graphFileName);

            SaveGroups(graphData, dialogueContainer);
            SaveNodes(graphData, dialogueContainer);

            SaveAsset(graphData);
            SaveAsset(dialogueContainer);
        }

        private static void SaveGroups(DS_Graph_SaveData_SO graphData, DS_DialogueContainer_SO dialogueContainer)
        {
            List<string> groupNames = new List<string>();

            foreach (DS_Group group in groups) 
            {
                SaveGroupInGraphData(group, graphData);
                SaveGroupToSO(group, dialogueContainer);
                groupNames.Add(group.title);
            }

            UpdateOldGroups(groupNames, graphData);
        }

        private static void SaveGroupInGraphData(DS_Group group, DS_Graph_SaveData_SO graphData)
        {
            DS_Group_SaveData groupData = new DS_Group_SaveData(group);
            graphData.Groups.Add(groupData);
        }

        private static void SaveGroupToSO(DS_Group group, DS_DialogueContainer_SO dialogueContainer)
        {
            string groupName = group.title;
            CreateFolders($"{containerFolderPath}/Groups", groupName);
            CreateFolders($"{containerFolderPath}/Groups/{groupName}", "Dialogues");

            DS_DialogueGroup_SO dialogueGroup = CreateAsset<DS_DialogueGroup_SO>($"{containerFolderPath}/Groups/{groupName}", groupName);
            dialogueGroup.Initialize(groupName);

            createdGroupsSO.Add(group.ID, dialogueGroup);

            dialogueContainer.DialogueGroups.Add(dialogueGroup, new List<DS_Dialogue_SO>());

            SaveAsset(dialogueGroup);
        }

        private static void UpdateOldGroups(List<string> currentGroupNames, DS_Graph_SaveData_SO graphData)
        {
            if(graphData.OldGroupsNames != null && graphData.OldGroupsNames.Count != 0)
            {
                List<string> groupsToRemove = graphData.OldGroupsNames.Except(currentGroupNames).ToList();
                foreach(string groupToRemove in groupsToRemove)
                {
                    RemoveFolder($"{containerFolderPath}/Groups/{groupToRemove}");
                }
            }
            graphData.OldGroupsNames = new List<string>(currentGroupNames);
        }
        private static void SaveNodes(DS_Graph_SaveData_SO graphData, DS_DialogueContainer_SO dialogueContainer)
        {
            foreach(DS_Node node in nodes)
            {
                SaveNodeInGraphData(node, graphData);
                SaveNodeToSO(node, dialogueContainer);
            }
            UpdateDialogueChoicesConnection();
        }


        private static void SaveNodeInGraphData(DS_Node node, DS_Graph_SaveData_SO graphData)
        {
            DS_Node_SaveData nodeData = new DS_Node_SaveData();
            graphData.Nodes.Add(nodeData);
        }

        private static void SaveNodeToSO(DS_Node node, DS_DialogueContainer_SO dialogueContainer)
        {
            DS_Dialogue_SO dialogue;
            
            if(node.Group == null)
            {
                dialogue = CreateAsset<DS_Dialogue_SO>($"{containerFolderPath}/Groups/Global/Dialogues", node.DialogueName);
                dialogueContainer.UngroupedDialogues.Add(dialogue);
            }
            else 
            {
                dialogue = CreateAsset<DS_Dialogue_SO>($"{containerFolderPath}/Groups/{node.Group.title}/Dialogues", node.DialogueName);

                dialogueContainer.DialogueGroups.AddItem(createdGroupsSO[node.Group.ID], dialogue);
            }

            dialogue.Initialize(node.DialogueName, node.Text, ChoicesDataToDialogueChoices(node.Choices), node.DialogueType, node.IsStartingNode());


            createdDialoguesSO.Add(node.ID, dialogue);
            SaveAsset(dialogue);
        }

        private static List<DS_DialogueChoiceData> ChoicesDataToDialogueChoices(List<DS_ChoiceData> nodeChoices)
        {
            List<DS_DialogueChoiceData> dialogueChoices = new();
            foreach(DS_ChoiceData choiceData in nodeChoices)
            {
                DS_DialogueChoiceData dialogueChoice = new() { Text = choiceData.ChoiceName };
                dialogueChoices.Add(dialogueChoice);
            }
            return dialogueChoices;
        }

        private static void UpdateDialogueChoicesConnection()
        {
           foreach(DS_Node node in nodes)
            {
                DS_Dialogue_SO dialogue = createdDialoguesSO[node.ID];

                for(int choiceIndex = 0; choiceIndex < node.Choices.Count; choiceIndex++)
                {
                    DS_ChoiceData choice = node.Choices[choiceIndex];

                    if (string.IsNullOrEmpty(choice.NodeID)) continue;

                    dialogue.Choices[choiceIndex].NextDialogue = createdDialoguesSO[choice.NodeID];

                    SaveAsset(dialogue);
                }
            }
        }

        #endregion

        #region Creation methods
        private static void CreateStaticFolders()
        {
            CreateFolders("Assets/Editor/DialogueSystem", "Graphs");
            CreateFolders("Assets", "DialogueSystem");
            CreateFolders("Assets/DialogueSystem", "Dialogues");
            CreateFolders("Assets/DialogueSystem/Dialogues", graphFileName);
            CreateFolders(containerFolderPath, "Global");
            CreateFolders(containerFolderPath, "Groups");
            CreateFolders($"{containerFolderPath}/Global", "Dialogues");

        }
        #endregion

        #region Utility methods
        private static void CreateFolders(string path, string folderName)
        {
            if (AssetDatabase.IsValidFolder($"{path}/{folderName}") == true) return;
            else AssetDatabase.CreateFolder(path, folderName);
            
        }
        private static void GetElementsFromGraphView()
        {
            graphView.graphElements.ForEach(FetchGraphElements());
        }
        private static Action<GraphElement> FetchGraphElements()
        {
            return graphElement =>
            {
                if (graphElement.GetType() == typeof(DS_Node))
                {
                    nodes.Add((DS_Node)graphElement);
                }
                else if (graphElement.GetType() == typeof(DS_Group))
                {
                    groups.Add((DS_Group)graphElement);
                }
            };
        }


        private static T CreateAsset<T>(string path, string assetName) where T : ScriptableObject
        {
            string fullPath = $"{path}/{assetName}.asset";
            T asset = AssetDatabase.LoadAssetAtPath<T>(fullPath);

            if (asset == null)
            {
                asset = ScriptableObject.CreateInstance<T>();
                AssetDatabase.CreateAsset(asset, fullPath);
            }
            return asset;
        }

        private static void SaveAsset(UnityEngine.Object asset)
        {
            EditorUtility.SetDirty(asset);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        private static void RemoveFolder(string folderPath)
        {
            FileUtil.DeleteFileOrDirectory($"{folderPath}.meta");
            FileUtil.DeleteFileOrDirectory($"{folderPath}/");
        }

        #endregion
    }
}
