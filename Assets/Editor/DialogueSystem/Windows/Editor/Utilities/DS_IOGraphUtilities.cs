using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using System.Linq;
using System;

namespace DS.Editor.Windows.Utilities
{
    using Elements;

    using Runtime.Data;
    using Editor.Data;

    using Editor.ScriptableObjects;
    using Runtime.ScriptableObjects;

    using Editor.Utilities;
    using Runtime.Utilities;


    public class DS_IOGraphUtilities
    {
        /// <summary>
        /// 
        /// </summary>
        private DS_IOUtilities IOUtilities;
        /// <summary>
        /// Reference to the current displayed graph in the Dialogue System Graph View Editor Window.
        /// </summary>
        private DS_GraphView graphView;
        /// <summary>
        /// Base assets path for all saved dialogues graphs.
        /// </summary>
        public static readonly string commonAssetsPath = "Assets/DialogueSystem/Dialogues";
        /// <summary>
        /// Base editor assets path for all saved diallgues graphs.
        /// </summary>
        public static readonly string commonEditorPath = "Assets/Editor/DialogueSystem/Graphs";


        private string graphFileName;
        private string containerFolderPath;

        private List<DS_Group> groups;
        private List<DS_BaseNode> nodes;

        public Dictionary<string, DS_DialogueGroupSO> createdGroupsSO;
        public Dictionary<string, DS_DialogueSO> createdDialoguesSO;

        private Dictionary<string, DS_Group> loadedGroups;
        private Dictionary<string, DS_BaseNode> loadedNodes;

        /// <summary>
        /// Initialize the static IOUtilities class with informations for the current DS_Graph view created, displayed or loaded in the editor window.
        /// </summary>
        /// <param name="graphView"></param>
        /// <param name="graphName"></param>
        public void Initialize(DS_GraphView graphView, string graphName)
        {
            IOUtilities = new DS_IOUtilities();

            this.graphView = graphView;
            graphFileName = graphName;
            containerFolderPath = commonAssetsPath + "/" + graphFileName;

            groups = new List<DS_Group>();
            nodes = new List<DS_BaseNode>();
            createdGroupsSO = new Dictionary<string, DS_DialogueGroupSO>();
            createdDialoguesSO = new Dictionary<string, DS_DialogueSO>();

            loadedGroups = new Dictionary<string, DS_Group>();
            loadedNodes = new Dictionary<string, DS_BaseNode>();
        }

        #region Save methods
        /// <summary>
        /// Save the current displayed DS_GraphView.
        /// </summary>
        public void SaveGraph()
        {
            CreateStaticFolders();
            GetElementsFromGraphView();

            DS_GraphSO graphData = IOUtilities.CreateAsset<DS_GraphSO>(commonEditorPath, $"/{graphFileName}_Graph");
            graphData.Initialize(graphFileName);

            DS_DialogueContainerSO dialogueContainer = IOUtilities.CreateAsset<DS_DialogueContainerSO>(containerFolderPath, graphFileName);
            dialogueContainer.Initialize(graphFileName);

            SaveGroups(graphData, dialogueContainer);
            SaveNodes(graphData, dialogueContainer);

            IOUtilities.SaveAsset(graphData);
            IOUtilities.SaveAsset(dialogueContainer);
        }

        private void SaveGroups(DS_GraphSO graphData, DS_DialogueContainerSO dialogueContainer)
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

        private void SaveGroupInGraphData(DS_Group group, DS_GraphSO graphData)
        {
            DS_Group_SaveData groupData = new DS_Group_SaveData(group.ID, group.title, group.GetPosition().position);
            graphData.Groups.Add(groupData);
        }

        private void SaveGroupToSO(DS_Group group, DS_DialogueContainerSO dialogueContainer)
        {
            string groupName = group.title;
            CreateFolders($"{containerFolderPath}/Groups", groupName);
            CreateFolders($"{containerFolderPath}/Groups/{groupName}", "Dialogues");

            DS_DialogueGroupSO dialogueGroup = IOUtilities.CreateAsset<DS_DialogueGroupSO>($"{containerFolderPath}/Groups/{groupName}", groupName);
            dialogueGroup.Initialize(groupName);

            createdGroupsSO.Add(group.ID, dialogueGroup);

            dialogueContainer.DialogueGroups.Add(dialogueGroup, new List<DS_DialogueSO>());

            IOUtilities.SaveAsset(dialogueGroup);
        }

        private void UpdateOldGroups(List<string> currentGroupNames, DS_GraphSO graphData)
        {
            if(graphData.OldGroupsNames != null && graphData.OldGroupsNames.Count != 0)
            {
                List<string> groupsToRemove = graphData.OldGroupsNames.Except(currentGroupNames).ToList();
                foreach(string groupToRemove in groupsToRemove)
                {
                    IOUtilities.RemoveFolder($"{containerFolderPath}/Groups/{groupToRemove}");
                }
            }
            graphData.OldGroupsNames = new List<string>(currentGroupNames);
        }
        private void SaveNodes(DS_GraphSO graphData, DS_DialogueContainerSO dialogueContainer)
        {
            SerializableDictionary<string, List<string>> groupedNodeNames = new SerializableDictionary<string, List<string>>();
            List<string> ungroupedNodeNames = new List<string>();

            foreach(DS_BaseNode node in nodes)
            {
                SaveNodeInGraphData(node, graphData);
                SaveNodeToSO(node, dialogueContainer);

                if (node.Group != null)
                {
                    groupedNodeNames.AddItem(node.Group.title, node.DialogueName);
                }
                else
                {
                    Debug.Log("Adding node to ungrouped dictionary in utilities");
                    ungroupedNodeNames.Add(node.DialogueName);
                }
            }

            UpdateDialogueChoicesConnection();

            UpdateOldGroupedNodes(groupedNodeNames, graphData);
            UpdateOldUngroupedNodes(ungroupedNodeNames, graphData);
        }


        private void SaveNodeInGraphData(DS_BaseNode node, DS_GraphSO graphData)
        {
            var events = new List<DS_DialogueEventSO>();
            if (node is DS_EventNode eventNode)
            {
                events = eventNode.DialogueEvents;
            } else events = null;

            DS_Node_SaveData nodeData = new(node.ID, node.DialogueName, node.Choices, node.Texts, node.DialogueType,
                                            events ?? null, node.Group == null? null : node.Group.ID, node.GetPosition().position);
            graphData.Nodes.Add(nodeData);
        }
        private void SaveNodeToSO(DS_BaseNode node, DS_DialogueContainerSO dialogueContainer)
        {
            DS_DialogueSO dialogue;
            
            if(node.Group == null)
            {
                dialogue = IOUtilities.CreateAsset<DS_DialogueSO>($"{containerFolderPath}/Global/Dialogues", node.DialogueName);
                dialogueContainer.UngroupedDialogues.Add(dialogue);
            }
            else 
            {
                dialogue = IOUtilities.CreateAsset<DS_DialogueSO>($"{containerFolderPath}/Groups/{node.Group.title}/Dialogues", node.DialogueName);

                dialogueContainer.DialogueGroups.AddItem(createdGroupsSO[node.Group.ID], dialogue);
            }

            dialogue.Initialize(node.DialogueName, node.Texts, NodeToDialogueChoice(node.Choices), node.DialogueType, node.IsStartingNode());
            if(node.DialogueType == Enumerations.DS_DialogueType.Event)
            {
                dialogue.SaveEvents(((DS_EventNode)node).DialogueEvents);
            }

            createdDialoguesSO.Add(node.ID, dialogue);
            IOUtilities.SaveAsset(dialogue);
        }


        private List<DS_DialogueChoiceData> NodeToDialogueChoice(List<DS_NodeChoiceData> nodeChoices)
        {
            List<DS_DialogueChoiceData> dialogueChoices = new();
            foreach(DS_NodeChoiceData choiceData in nodeChoices)
            {
                DS_DialogueChoiceData dialogueChoice = new() { ChoiceTexts = choiceData.ChoiceTexts };
                dialogueChoices.Add(dialogueChoice);
            }
            return dialogueChoices;
        }

        private void UpdateDialogueChoicesConnection()
        {
           foreach(DS_BaseNode node in nodes)
            {
                DS_DialogueSO dialogue = createdDialoguesSO[node.ID];

                for(int choiceIndex = 0; choiceIndex < node.Choices.Count; choiceIndex++)
                {
                    DS_NodeChoiceData choice = node.Choices[choiceIndex];

                    if (string.IsNullOrEmpty(choice.NextNodeID)) continue;

                    dialogue.Choices[choiceIndex].NextDialogue = createdDialoguesSO[choice.NextNodeID];
                    Debug.Log($"Saving some edge linked to node called ");
                    IOUtilities.SaveAsset(dialogue);
                }
            }
        }

        private void UpdateOldGroupedNodes(SerializableDictionary<string, List<string>> currentGroupedNodeNames, DS_GraphSO graphData)
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
                            IOUtilities.RemoveAsset($"{containerFolderPath}/Groups/{oldGroupedNodes.Key}/Dialpgue", nodeToRemove);
                        }
                    }
                    else
                    {
                        IOUtilities.RemoveFolder($"{containerFolderPath}/Groups/{oldGroupedNodes.Key}");
                    }
                }
                graphData.OldGroupedNodesNames = new SerializableDictionary<string, List<string>>(currentGroupedNodeNames);
            }
        }
        private void UpdateOldUngroupedNodes(List<string> currentUngroupedNodeNames, DS_GraphSO graphData)
        {
            if(graphData.OldUngroupedNodesNames != null && graphData.OldUngroupedNodesNames.Count != 0)
            {
                List<string> nodesToRemove = graphData.OldUngroupedNodesNames.Except(currentUngroupedNodeNames).ToList();

                foreach(string nodeToRemove in nodesToRemove)
                {
                    IOUtilities.RemoveAsset($"{containerFolderPath}/Global/Dialogues", nodeToRemove);
                }
            }
            graphData.OldUngroupedNodesNames = new List<string>(currentUngroupedNodeNames);
        }

        #endregion

        #region Load methods
        public void LoadGraph()
        {
            DS_GraphSO graphData = IOUtilities.LoadAsset<DS_GraphSO>(commonEditorPath, graphFileName);
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
            graphView.EditorWindow.UpdateFilename(graphData.GraphName);

            LoadGroups(graphData.Groups);
            LoadNodes(graphData.Nodes);
            LoadNodesConnections();
        }
        private void LoadGroups(List<DS_Group_SaveData> groups)
        {
            foreach(DS_Group_SaveData groupData in groups)
            {
                DS_Group group = graphView.CreateGroup(groupData.Name, groupData.Position);
                group.ID = groupData.ID;

                loadedGroups.Add(group.ID, group);
            }
        }
        private void LoadNodes(List<DS_Node_SaveData> nodes)
        {
            foreach (DS_Node_SaveData nodeData in nodes)
            {
                DS_BaseNode node = graphView.CreateNode(nodeData.Name, nodeData.Position, nodeData.DialogueType, false);

                node.ID = nodeData.NodeID;
                List<DS_NodeChoiceData> clonedChoices = CloneChoices(nodeData.Choices);
                node.Choices = clonedChoices;
                node.Texts = nodeData.Texts;

                if(node.DialogueType == Enumerations.DS_DialogueType.Event)
                {
                    DS_EventNode eventNode = (DS_EventNode)node;
                    if (nodeData.Events != null && nodeData.Events.Count > 0)
                    {
                        foreach (var _event in nodeData.Events)
                        {
                            eventNode.DialogueEvents.Add(_event);
                        }
                    }
                }

                node.Draw();
                graphView.AddElement(node);

                if (string.IsNullOrEmpty(nodeData.GroupID) == false)
                {
                    DS_Group group = loadedGroups[nodeData.GroupID];
                    node.Group = group;
                    group.AddElement(node);
                    
                }
                loadedNodes.Add(node.ID, node);
            }
        }

        private void LoadNodesConnections()
        {
            foreach(KeyValuePair<string, DS_BaseNode> loadedNode in loadedNodes)
            {
                foreach(Port choicePort in loadedNode.Value.outputContainer.Children())
                {
                    DS_NodeChoiceData choiceData = (DS_NodeChoiceData) choicePort.userData;

                    if(string.IsNullOrEmpty(choiceData.NextNodeID) == false)
                    {
                        DS_BaseNode linkedNode = loadedNodes[choiceData.NextNodeID];
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
        /// <summary>
        /// Create every needed directory, both assets and editor side, in which save the graph elements.
        /// </summary>
        private void CreateStaticFolders()
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
        public void CreateFolders(string path, string folderName)
        {
            if (AssetDatabase.IsValidFolder($"{path}/{folderName}") == true) return;
            else AssetDatabase.CreateFolder(path, folderName);
            
        }
        public void GetElementsFromGraphView()
        {
            graphView.graphElements.ForEach(FetchGraphElements());
        }
        
        public Action<GraphElement> FetchGraphElements()
        {
            return graphElement =>
            {
                if (graphElement.GetType() == typeof(DS_SingleChoiceNode) ||
                    graphElement.GetType() == typeof(DS_MultipleChoiceNode) ||
                    graphElement.GetType() == typeof(DS_StartNode) ||
                    graphElement.GetType() == typeof(DS_EndNode) ||
                    graphElement.GetType() == typeof(DS_EventNode))
                {
                    nodes.Add((DS_BaseNode)graphElement);
                }
                else if (graphElement.GetType() == typeof(DS_Group))
                {
                    groups.Add((DS_Group)graphElement);
                }
            };
        }
     

        public List<DS_NodeChoiceData> CloneChoices(List<DS_NodeChoiceData> choiceList)
        {
            List<DS_NodeChoiceData> choices = new List<DS_NodeChoiceData>();
            if (choiceList == null) { Debug.Log("!!"); }
            foreach (DS_NodeChoiceData choice in choiceList)
            {
                DS_NodeChoiceData choice_SaveData = new DS_NodeChoiceData(choice);
                choices.Add(choice_SaveData);
            }

            return choices;
        }

        public DS_Group GetLoadedGroup(string groupID)
        {
            if(loadedGroups.ContainsKey(groupID))
            {
                return loadedGroups[groupID];
            }
            else
            {
                DS_Logger.Error($"No group with ID:{groupID} is currently loaded in the graph.", Color.red);
                return null;
            }
        }
        public void AddLoadedNode(string nodeID, DS_BaseNode node)
        {
            if(loadedNodes.ContainsKey(nodeID) == true)
            {
                DS_Logger.Error($"Node ID:{nodeID} key in loaded nodes dictionary already exists in the current loaded graph. You can not load the same node more than once.", Color.red);
                return;
            }
            else
            {
                loadedNodes.Add(node.ID, node);
            }
        }
        #endregion
    }
}