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


    public class GraphIOSystem
    {
        /// <summary>
        /// 
        /// </summary>
        private IOUtilities IOUtilities;
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
        public static readonly string commonEditorPath = "Assets/Editor/Files/Graphs";


        private string graphFileName;
        private string containerFolderPath;

        private List<DS_Group> groups;
        private List<DS_BaseNode> nodes;

        public Dictionary<string, DialogueGroupSO> createdGroupsSO;
        public Dictionary<string, DialogueSO> createdDialoguesSO;

        private Dictionary<string, DS_Group> loadedGroups;
        private Dictionary<string, DS_BaseNode> loadedNodes;

        /// <summary>
        /// Initialize the static IOUtilities class with informations for the current DS_Graph view created, displayed or loaded in the editor window.
        /// </summary>
        /// <param name="graphView"></param>
        /// <param name="graphName"></param>
        public void Initialize(DS_GraphView graphView, string graphName)
        {
            IOUtilities = new IOUtilities();

            this.graphView = graphView;
            graphFileName = graphName;
            containerFolderPath = commonAssetsPath + "/" + graphFileName;

            groups = new List<DS_Group>();
            nodes = new List<DS_BaseNode>();
            createdGroupsSO = new Dictionary<string, DialogueGroupSO>();
            createdDialoguesSO = new Dictionary<string, DialogueSO>();

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

            DialogueContainerSO dialogueContainer = IOUtilities.CreateAsset<DialogueContainerSO>(containerFolderPath, graphFileName);
            dialogueContainer.Initialize(graphFileName);

            SaveGroups(graphData, dialogueContainer);
            SaveNodes(graphData, dialogueContainer);

            IOUtilities.SaveAsset(graphData);
            IOUtilities.SaveAsset(dialogueContainer);
        }

        private void SaveGroups(DS_GraphSO graphData, DialogueContainerSO dialogueContainer)
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
            GroupData groupData = new GroupData(group.ID, group.title, group.GetPosition().position);
            graphData.Groups.Add(groupData);
        }

        private void SaveGroupToSO(DS_Group group, DialogueContainerSO dialogueContainer)
        {
            string groupName = group.title;
            IOUtilities.CreateFolder($"{containerFolderPath}/Groups", groupName);
            IOUtilities.CreateFolder($"{containerFolderPath}/Groups/{groupName}", "Dialogues");

            DialogueGroupSO dialogueGroup = IOUtilities.CreateAsset<DialogueGroupSO>($"{containerFolderPath}/Groups/{groupName}", groupName);
            dialogueGroup.Initialize(groupName);

            createdGroupsSO.Add(group.ID, dialogueGroup);

            dialogueContainer.DialogueGroups.Add(dialogueGroup, new List<DialogueSO>());

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
        private void SaveNodes(DS_GraphSO graphData, DialogueContainerSO dialogueContainer)
        {
            SerializableDictionary<string, List<string>> groupedNodeNames = new SerializableDictionary<string, List<string>>();
            List<string> ungroupedNodeNames = new List<string>();

            foreach(DS_BaseNode node in nodes)
            {
                SaveNodeInGraphData(node, graphData);
                switch (node.DialogueType)
                {
                    case Enums.DialogueType.Event:
                        SaveNodeToSO<EventDialogueSO>(node, dialogueContainer);
                        break;
                    case Enums.DialogueType.End:
                        SaveNodeToSO<EndDialogueSO>(node, dialogueContainer);
                        break;
                    default:
                        SaveNodeToSO<DialogueSO>(node, dialogueContainer);
                        break;
                }
                

                if (node.Group != null)
                {
                    groupedNodeNames.AddItem(node.Group.title, node.DialogueName);
                }
                else
                {
                    //Debug.Log("Adding node to ungrouped dictionary in utilities");
                    ungroupedNodeNames.Add(node.DialogueName);
                }
            }

            UpdateDialogueChoicesConnection();

            UpdateOldGroupedNodes(groupedNodeNames, graphData);
            UpdateOldUngroupedNodes(ungroupedNodeNames, graphData);
        }


        private void SaveNodeInGraphData(DS_BaseNode node, DS_GraphSO graphData)
        {
            switch (node.DialogueType)
            {
                case Enums.DialogueType.Event:
                    EventNodeData eventNodeData = GetEventNodeData((DS_EventNode)node);
                    graphData.EventNodes.Add(eventNodeData);
                    break;
                case Enums.DialogueType.End:
                    EndNodeData endNodeData = GetEndNodeData((DS_EndNode)node);
                    graphData.EndNodes.Add(endNodeData);
                    break;
                default:
                    NodeData nodeData = GetNodeData(node);
                    graphData.Nodes.Add(nodeData);
                    break;
            }
                   
            static NodeData GetNodeData(DS_BaseNode node)
            {
                return new(node.ID, node.DialogueName, node.Choices, node.Texts, node.DialogueType,
                                                       node.Group == null ? null : node.Group.ID, node.GetPosition().position);
            }
            static EventNodeData GetEventNodeData(DS_EventNode node)
            {
                return new(node.ID, node.DialogueName, node.Choices, node.Texts, node.DialogueType,
                                                       node.Group == null ? null : node.Group.ID, node.GetPosition().position, node.DialogueEvents);
            }
            static EndNodeData GetEndNodeData(DS_EndNode node)
            {
                return new(node.ID, node.DialogueName, node.Choices, node.Texts, node.DialogueType,
                                                       node.Group == null ? null : node.Group.ID, node.GetPosition().position, node.IsRepetableDialogue);
            }
        }
        private void SaveNodeToSO<T>(DS_BaseNode node, DialogueContainerSO dialogueContainer) where T :DialogueSO
        {
            T dialogue;

            if(node.Group == null)
            {
                dialogue = IOUtilities.CreateAsset<T>($"{containerFolderPath}/Global/Dialogues", node.DialogueName);
                dialogueContainer.UngroupedDialogues.Add(dialogue);
            }
            else 
            {
                dialogue = IOUtilities.CreateAsset<T>($"{containerFolderPath}/Groups/{node.Group.title}/Dialogues", node.DialogueName);

                dialogueContainer.DialogueGroups.AddItem(createdGroupsSO[node.Group.ID], dialogue);
            }

            dialogue.Initialize(node.DialogueName, node.ID, node.Texts, NodeToDialogueChoice(node.Choices), node.DialogueType, node.IsStartingNode());

            if(node.DialogueType == Enums.DialogueType.Event)
            {
                (dialogue as EventDialogueSO).SaveEvents(((DS_EventNode)node).DialogueEvents);
            }
            else if (node.DialogueType == Enums.DialogueType.End)
            {
                (dialogue as EndDialogueSO).SetRepetableDialogue(((DS_EndNode)node).IsRepetableDialogue);
            }

            createdDialoguesSO.Add(node.ID, dialogue);
            IOUtilities.SaveAsset(dialogue);
        }


        private List<DialogueChoiceData> NodeToDialogueChoice(List<ChoiceData> nodeChoices)
        {
            List<DialogueChoiceData> dialogueChoices = new();
            foreach(ChoiceData choiceData in nodeChoices)
            {
                DialogueChoiceData dialogueChoice = new() { ChoiceTexts = choiceData.ChoiceTexts };
                dialogueChoice.ChoiceID = choiceData.ChoiceID;
                dialogueChoices.Add(dialogueChoice);
            }
            return dialogueChoices;
        }

        private void UpdateDialogueChoicesConnection()
        {
           foreach(DS_BaseNode node in nodes)
            {
                DialogueSO dialogue = createdDialoguesSO[node.ID];

                for(int choiceIndex = 0; choiceIndex < node.Choices.Count; choiceIndex++)
                {
                    ChoiceData choice = node.Choices[choiceIndex];

                    if (string.IsNullOrEmpty(choice.NextNodeID)) continue;

                    dialogue.Choices[choiceIndex].NextDialogue = createdDialoguesSO[choice.NextNodeID];
                    
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
            LoadEventNodes(graphData.EventNodes);
            LoadEndNodes(graphData.EndNodes);
            LoadNodesConnections();
        }




        private void LoadGroups(List<GroupData> groups)
        {
            foreach(GroupData groupData in groups)
            {
                DS_Group group = graphView.CreateGroup(groupData.Name, groupData.Position);
                group.ID = groupData.ID;

                loadedGroups.Add(group.ID, group);
            }
        }
        private void LoadNodes(List<NodeData> nodes)
        {
            foreach (NodeData nodeData in nodes)
            {
                LoadNode(nodeData, true);
            }
        }

        private void LoadEventNodes(List<EventNodeData> eventNodes)
        {
            foreach (EventNodeData evntNodeData in eventNodes)
            {
                var node = (DS_EventNode)LoadNode(evntNodeData, false);
                if (evntNodeData.Events != null && evntNodeData.Events.Count != 0)
                {
                    foreach (var _event in evntNodeData.Events)
                    {
                        node.DialogueEvents.Add(_event);
                    }
                }else node.DialogueEvents = new List<DS_EventSO> { };
                node.Draw();
            }
        }
        private void LoadEndNodes(List<EndNodeData> endNodes)
        {
            foreach (EndNodeData endNodeData in endNodes)
            {
                var node = (DS_EndNode)LoadNode(endNodeData, false);
                node.IsRepetableDialogue = endNodeData.IsDialogueRepetable;
                node.Draw();
            }
        }

        private DS_BaseNode LoadNode(NodeData nodeData, bool draw = false)
        {
            DS_BaseNode node = graphView.CreateNode(nodeData.Name, nodeData.Position, nodeData.DialogueType, false);

            node.ID = nodeData.NodeID;
            List<ChoiceData> clonedChoices = CloneChoices(nodeData.Choices);

            node.Choices = clonedChoices;
            node.Texts = new(LenguageUtilities.UpdateLenguageDataSet(nodeData.Texts));

            if(draw == true )node.Draw();
            graphView.AddElement(node);

            if (string.IsNullOrEmpty(nodeData.GroupID) == false)
            {
                DS_Group group = loadedGroups[nodeData.GroupID];
                node.Group = group;
                group.AddElement(node);

            }
            loadedNodes.Add(node.ID, node);

            return node;
        }
        private void LoadNodesConnections()
        {
            foreach(KeyValuePair<string, DS_BaseNode> loadedNode in loadedNodes)
            {
                foreach(Port choicePort in loadedNode.Value.outputContainer.Children())
                {
                    ChoiceData choiceData = (ChoiceData) choicePort.userData;

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
            IOUtilities.CreateFolder("Assets/Editor/Files", "Graphs");
            IOUtilities.CreateFolder("Assets", "DialogueSystem");
            IOUtilities.CreateFolder("Assets/DialogueSystem", "Dialogues");
            IOUtilities.CreateFolder("Assets/DialogueSystem/Dialogues", graphFileName);
            IOUtilities.CreateFolder(containerFolderPath, "Global");
            IOUtilities.CreateFolder(containerFolderPath, "Groups");
            IOUtilities.CreateFolder($"{containerFolderPath}/Global", "Dialogues");

        }
        #endregion

        #region Utility methods

        public void GetElementsFromGraphView()
        {
            graphView.graphElements.ForEach(FetchGraphElements());
        }
        
        public Action<GraphElement> FetchGraphElements()
        {
            return graphElement =>
            {
                if (graphElement.GetType() == typeof(DS_SingleNode) ||
                    graphElement.GetType() == typeof(DS_MultipleNode) ||
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
     

        public List<ChoiceData> CloneChoices(List<ChoiceData> choiceList)
        {
            List<ChoiceData> choices = new List<ChoiceData>();

            foreach (ChoiceData choice in choiceList)
            {
                ChoiceData choice_SaveData = new ChoiceData(choice);
                choice_SaveData.ChoiceTexts = new(LenguageUtilities.UpdateLenguageDataSet(choice_SaveData.ChoiceTexts));
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
                Logger.Error($"No group with ID:{groupID} is currently loaded in the graph.", Color.red);
                return null;
            }
        }
        public void AddLoadedNode(string nodeID, DS_BaseNode node)
        {
            if(loadedNodes.ContainsKey(nodeID) == true)
            {
                Logger.Error($"Node ID:{nodeID} key in loaded nodes dictionary already exists in the current loaded graph. You can not load the same node more than once.", Color.red);
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
