using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using System.Linq;

namespace DS.Utilities
{
    using Elements;
    using System;
    using Windows;
    using Data.Save;
    using ScriptableObjects;
    using Data;

    public static class DS_IOUtilities
    {
        private static DS_GraphView graphView;
        private static string graphFileName;
        private static string containerFolderPath;

        private static List<DS_Group> groups;
        private static List<DS_Node> nodes;

        public static Dictionary<string, DS_DialogueGroupSO> createdGroupsSO;
        public static Dictionary<string, DS_DialogueSO> createdDialoguesSO;

        private static Dictionary<string, DS_Group> loadedGroups;
        private static Dictionary<string, DS_Node> loadedNodes;


        public static void Initialize(DS_GraphView graphView, string graphName)
        {
            DS_IOUtilities.graphView = graphView;
            graphFileName = graphName;
            containerFolderPath = $"Assets/DialogueSystem/Dialogues/{graphFileName}";

            groups = new List<DS_Group>();
            nodes = new List<DS_Node>();
            createdGroupsSO = new Dictionary<string, DS_DialogueGroupSO>();
            createdDialoguesSO = new Dictionary<string, DS_DialogueSO>();

            loadedGroups = new Dictionary<string, DS_Group>();
            loadedNodes = new Dictionary<string, DS_Node>();
        }

        #region Save methods
        public static void SaveGraph()
        {
            CreateStaticFolders();
            GetElementsFromGraphView();

            DS_GraphSO graphData = CreateAsset<DS_GraphSO>("Assets/Editor/DialogueSystem/Graphs", $"{graphFileName}_Graph");
            graphData.Initialize(graphFileName);

            DS_DialogueContainerSO dialogueContainer = CreateAsset<DS_DialogueContainerSO>(containerFolderPath, graphFileName);
            dialogueContainer.Initialize(graphFileName);

            SaveGroups(graphData, dialogueContainer);
            SaveNodes(graphData, dialogueContainer);

            SaveAsset(graphData);
            SaveAsset(dialogueContainer);
        }

        private static void SaveGroups(DS_GraphSO graphData, DS_DialogueContainerSO dialogueContainer)
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

        private static void SaveGroupInGraphData(DS_Group group, DS_GraphSO graphData)
        {
            DS_Group_SaveData groupData = new DS_Group_SaveData(group);
            graphData.Groups.Add(groupData);
        }

        private static void SaveGroupToSO(DS_Group group, DS_DialogueContainerSO dialogueContainer)
        {
            string groupName = group.title;
            CreateFolders($"{containerFolderPath}/Groups", groupName);
            CreateFolders($"{containerFolderPath}/Groups/{groupName}", "Dialogues");

            DS_DialogueGroupSO dialogueGroup = CreateAsset<DS_DialogueGroupSO>($"{containerFolderPath}/Groups/{groupName}", groupName);
            dialogueGroup.Initialize(groupName);

            createdGroupsSO.Add(group.ID, dialogueGroup);

            dialogueContainer.DialogueGroups.Add(dialogueGroup, new List<DS_DialogueSO>());

            SaveAsset(dialogueGroup);
        }

        private static void UpdateOldGroups(List<string> currentGroupNames, DS_GraphSO graphData)
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
        private static void SaveNodes(DS_GraphSO graphData, DS_DialogueContainerSO dialogueContainer)
        {

            SerializableDictionary<string, List<string>> groupedNodeNames = new SerializableDictionary<string, List<string>>();
            List<string> ungroupedNodeNames = new List<string>();

            foreach(DS_Node node in nodes)
            {
                SaveNodeInGraphData(node, graphData);
                SaveNodeToSO(node, dialogueContainer);

                if (node.Group != null)
                {
                    groupedNodeNames.AddItem(node.Group.title, node.DialogueName);
                }
                else
                {
                    ungroupedNodeNames.Add(node.DialogueName);
                }
            }

            UpdateDialogueChoicesConnection();

            UpdateOldGroupedNodes(groupedNodeNames, graphData);
            UpdateOldUngroupedNodes(ungroupedNodeNames, graphData);
        }


        private static void SaveNodeInGraphData(DS_Node node, DS_GraphSO graphData)
        {
            //List<DS_ChoiceData> choices = CloneChoices(node.Choices);

            DS_Node_SaveData nodeData = new DS_Node_SaveData(node);
            graphData.Nodes.Add(nodeData);
        }


        private static void SaveNodeToSO(DS_Node node, DS_DialogueContainerSO dialogueContainer)
        {
            DS_DialogueSO dialogue;
            
            if(node.Group == null)
            {
                dialogue = CreateAsset<DS_DialogueSO>($"{containerFolderPath}/Global/Dialogues", node.DialogueName);
                dialogueContainer.UngroupedDialogues.Add(dialogue);
            }
            else 
            {
                dialogue = CreateAsset<DS_DialogueSO>($"{containerFolderPath}/Groups/{node.Group.title}/Dialogues", node.DialogueName);

                dialogueContainer.DialogueGroups.AddItem(createdGroupsSO[node.Group.ID], dialogue);
            }

            dialogue.Initialize(node.DialogueName, node.Text, ChoicesDataToDialogueChoices(node.Choices), node.DialogueType, node.IsStartingNode());


            createdDialoguesSO.Add(node.ID, dialogue);
            SaveAsset(dialogue);
        }

        private static List<Data.DS_ChoiceData> ChoicesDataToDialogueChoices(List<Data.Save.DS_Choice_SaveData> nodeChoices)
        {
            List<Data.DS_ChoiceData> dialogueChoices = new();
            foreach(Data.Save.DS_Choice_SaveData choiceData in nodeChoices)
            {
                Data.DS_ChoiceData dialogueChoice = new() { LabelText = choiceData.ChoiceName };
                dialogueChoices.Add(dialogueChoice);
            }
            return dialogueChoices;
        }

        private static void UpdateDialogueChoicesConnection()
        {
           foreach(DS_Node node in nodes)
            {
                DS_DialogueSO dialogue = createdDialoguesSO[node.ID];

                for(int choiceIndex = 0; choiceIndex < node.Choices.Count; choiceIndex++)
                {
                    Data.Save.DS_Choice_SaveData choice = node.Choices[choiceIndex];

                    if (string.IsNullOrEmpty(choice.NodeID)) continue;

                    dialogue.Choices[choiceIndex].NextDialogue = createdDialoguesSO[choice.NodeID];

                    SaveAsset(dialogue);
                }
            }
        }

        private static void UpdateOldGroupedNodes(SerializableDictionary<string, List<string>> currentGroupedNodeNames, DS_GraphSO graphData)
        {
            if (graphData.OldGroupedNodesNames != null && graphData.OldGroupedNodesNames.Count != 0)
            {
                foreach(KeyValuePair<string, List<string>> oldGroupedNodes in graphData.OldGroupedNodesNames)
                {
                    List<string> nodesToRemove = new List<string>();

                    if(currentGroupedNodeNames.ContainsKey(oldGroupedNodes.Key))
                    {
                        nodesToRemove = oldGroupedNodes.Value.Except(currentGroupedNodeNames[oldGroupedNodes.Key]).ToList();

                        foreach (string nodeToRemove in nodesToRemove)
                        {
                            RemoveAsset($"{containerFolderPath}/Groups/{oldGroupedNodes.Key}/Dialpgue", nodeToRemove);
                        }
                    }
                    else
                    {
                        RemoveFolder($"{containerFolderPath}/Groups/{oldGroupedNodes.Key}");
                    }
                }
                graphData.OldGroupedNodesNames = new SerializableDictionary<string, List<string>>(currentGroupedNodeNames);
            }
        }
        private static void UpdateOldUngroupedNodes(List<string> currentUngroupedNodeNames, DS_GraphSO graphData)
        {
            if(graphData.OldUngroupedNodesNames != null && graphData.OldUngroupedNodesNames.Count != 0)
            {
                List<string> nodesToRemove = graphData.OldUngroupedNodesNames.Except(currentUngroupedNodeNames).ToList();

                foreach(string nodeToRemove in nodesToRemove)
                {
                    RemoveAsset($"{containerFolderPath}/Global/Dialogues", nodeToRemove);
                }
            }
            graphData.OldUngroupedNodesNames = new List<string>(currentUngroupedNodeNames);
        }

        #endregion

        #region Load methods
        public static void LoadGraph()
        {
            DS_GraphSO graphData = LoadAsset<DS_GraphSO>("Assets/Editor/DialogueSystem/Graphs", graphFileName);
            if(graphData == null)
            {
                EditorUtility.DisplayDialog(
                    "Could not load the file.",
                    "The file at the following path could not be found:\n\n" +
                    $"Assets/Editor/DialogueSystem/Graphs/{graphFileName}.",
                    "Ok"
                    );
                return;
            }
            DS_EditorWindow.UpdateFilename(graphData.GraphName);

            LoadGroups(graphData.Groups);
            LoadNodes(graphData.Nodes);
            LoadNodesConnections();
        }

        private static void LoadGroups(List<DS_Group_SaveData> groups)
        {
            foreach(DS_Group_SaveData groupData in groups)
            {
                DS_Group group = graphView.CreateGroup(groupData.Name, groupData.Position);
                group.ID = groupData.ID;

                loadedGroups.Add(group.ID, group);
            }
        }

        private static void LoadNodes(List<DS_Node_SaveData> nodes)
        {
            foreach(DS_Node_SaveData nodeData in nodes)
            {         
                DS_Node node = graphView.CreateNode(nodeData.Name ,nodeData.Position, nodeData.DialogueType, false);

                node.ID = nodeData.NodeID;
                List<Data.Save.DS_Choice_SaveData> clonedChoices = CloneChoices(nodeData.Choices);
                node.Choices = clonedChoices;
                node.Text = nodeData.Text;
                
                node.Draw();
                graphView.AddElement(node);
                
                if(string.IsNullOrEmpty(nodeData.GroupID) == false)
                {
                    DS_Group group = loadedGroups[nodeData.GroupID];
                    node.Group = group;
                    group.AddElement(node);
                    loadedNodes.Add(node.ID, node);
                }
            }
        }

        private static void LoadNodesConnections()
        {
            foreach(KeyValuePair<string, DS_Node> loadedNode in loadedNodes)
            {
                foreach(Port choicePort in loadedNode.Value.outputContainer.Children())
                {
                    Data.Save.DS_Choice_SaveData choiceData = (Data.Save.DS_Choice_SaveData) choicePort.userData;

                    if(string.IsNullOrEmpty(choiceData.NodeID) == false)
                    {
                        DS_Node linkedNode = loadedNodes[choiceData.NodeID];
                        Port linkedNodeInputPort = (Port) linkedNode.inputContainer.Children().First();
                        Edge edge = choicePort.ConnectTo(linkedNodeInputPort);
                        graphView.AddElement(edge);
                        loadedNode.Value.RefreshPorts();
                    }
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
                if (graphElement.GetType() == typeof(DS_SingleChoiceNode) || graphElement.GetType() == typeof(DS_MultipleChoiceNode))
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
            T asset = LoadAsset<T>(path, assetName);

            if (asset == null)
            {
                asset = ScriptableObject.CreateInstance<T>();
                AssetDatabase.CreateAsset(asset, fullPath);
            }
            return asset;
        }

        private static T LoadAsset<T>(string path, string assetName) where T : ScriptableObject
        {
            string fullPath = $"{path}/{assetName}.asset";
            return AssetDatabase.LoadAssetAtPath<T>(fullPath);
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

        private static void RemoveAsset(string path, string assetName)
        {
            AssetDatabase.DeleteAsset($"{path}/{assetName}.asset");
        }

        private static List<Data.Save.DS_Choice_SaveData> CloneChoices(List<Data.Save.DS_Choice_SaveData> choiceList)
        {
            List<Data.Save.DS_Choice_SaveData> choices = new List<Data.Save.DS_Choice_SaveData>();

            foreach (Data.Save.DS_Choice_SaveData choice in choiceList)
            {
                Data.Save.DS_Choice_SaveData choice_SaveData = new Data.Save.DS_Choice_SaveData(choice);
                choices.Add(choice_SaveData);
            }

            return choices;
        }
        #endregion
    }
}
