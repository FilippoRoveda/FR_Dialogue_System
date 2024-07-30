using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Converter.Editor
{
    using DS.Editor.Data;
    using DS.Editor.Enumerations;
    using DS.Editor.ScriptableObjects;

    using DS.Runtime.Enumerations;
    using DS.Runtime.ScriptableObjects;

    public class Converter
    {
        private IOUtilities BaseIO = new();
        private DataConversion DataConversion = new();

        /// <summary>
        /// Base assets path for all saved dialogues.
        /// </summary>
        public static readonly string commonAssetsPath = "Assets/Data/GeneratedDialogues";
        private string containerFolderPath;

        private Dictionary<string, DialogueGroupSO> createdGroups;
        private Dictionary<string, BaseDialogueSO> createdDialogues;


        private string graphFileName;

        private GraphSO _graphSO;

        public void Initialize(GraphSO graphSO, string graphName)
        {
            graphFileName = graphName;
            _graphSO = graphSO;

            containerFolderPath = commonAssetsPath + "/" + graphName;

            createdGroups = new Dictionary<string, DialogueGroupSO>();
            createdDialogues = new Dictionary<string, BaseDialogueSO>();
        }

        public void ConvertGraph()
        {
            CreateStaticFolders();
            BaseIO.DeleteFolder(containerFolderPath, graphFileName);


            DialogueContainerSO dialogueContainer = BaseIO.CreateAsset<DialogueContainerSO>(containerFolderPath, graphFileName);
            dialogueContainer.Initialize(graphFileName);

            ConvertGroups(dialogueContainer);
            ConvertNodes(dialogueContainer);

            BaseIO.SaveAsset(dialogueContainer);
        }

        private void ConvertNodes(DialogueContainerSO dialogueContainer)
        {
            Dictionary<string, List<string>> groupedNodeNames = new Dictionary<string, List<string>>();
            List<string> ungroupedNodeNames = new List<string>();

            foreach (var nodeData in _graphSO.GetAllNodes())
            {
                switch (nodeData.NodeType)
                {
                    case NodeType.End:
                        SaveNodeToSO<EndDialogueSO>(nodeData, dialogueContainer);
                        break;
                    case NodeType.Event:
                        SaveNodeToSO<EventDialogueSO>(nodeData, dialogueContainer);
                        break;
                    case NodeType.Branch:
                        SaveNodeToSO<BranchDialogueSO>(nodeData, dialogueContainer);
                        break;
                    default:
                        SaveNodeToSO<DialogueSO>(nodeData, dialogueContainer);
                        break;
                }
                if (nodeData.GroupID != null)
                {
                    if (groupedNodeNames.ContainsKey(nodeData.GroupID)) groupedNodeNames[nodeData.GroupID].Add(nodeData.Name);
                    else
                    {
                        groupedNodeNames.Add(nodeData.GroupID, new List<string>());
                        groupedNodeNames[nodeData.GroupID].Add((string)nodeData.Name);
                    }
                }
                else
                {
                    ungroupedNodeNames.Add(nodeData.Name);
                }
            }

            UpdateDialogueChoicesConnection();
            UpdateOldGroupedNodes(groupedNodeNames, _graphSO);
            UpdateOldUngroupedNodes(ungroupedNodeNames, _graphSO);
        }


        private void SaveNodeToSO<T>(BaseNodeData nodeData, DialogueContainerSO dialogueContainer) where T : BaseDialogueSO
        {
            T dialogue;

            if (nodeData.GroupID == null || nodeData.GroupID == string.Empty || nodeData.GroupID == "")
            {
                dialogue = BaseIO.CreateAsset<T>($"{containerFolderPath}/Global/Dialogues", nodeData.Name);
                dialogueContainer.UngroupedDialogues.Add(dialogue);
            }
            else
            {
                dialogue = BaseIO.CreateAsset<T>($"{containerFolderPath}/Groups/{createdGroups[nodeData.GroupID].GroupName}/Dialogues", nodeData.Name);

                if (dialogueContainer.DialogueGroups.ContainsKey(createdGroups[nodeData.GroupID])) dialogueContainer.DialogueGroups[createdGroups[nodeData.GroupID]].Add(dialogue);
                else
                {
                    dialogueContainer.DialogueGroups.Add(createdGroups[nodeData.GroupID], new());
                    dialogueContainer.DialogueGroups[createdGroups[nodeData.GroupID]].Add(dialogue);
                }
            }

            switch (nodeData.NodeType)
            {
                case NodeType.Branch:
                    var branchDialogue = (dialogue as BranchDialogueSO);
                    branchDialogue.Initialize(nodeData.Name, nodeData.NodeID, (DialogueType)nodeData.NodeType);
                    var branchNodeData = (BranchNodeData) nodeData;
                    branchDialogue.Choices = DataConversion.NodeToDialogueChoice(branchNodeData.Choices);
                    branchDialogue.Condtitions = DataConversion.ConditionToDialogueConditionContainer(branchNodeData.Conditions);
                    break;


                case NodeType.End:
                    var endDialogue = (dialogue as EndDialogueSO);
                    endDialogue.Initialize(nodeData.Name, nodeData.NodeID, (DialogueType)nodeData.NodeType, DataConversion.LenguageDataConvert(((EndNodeData)nodeData).Texts));
                    endDialogue.IsRepetable = (((EndNodeData)nodeData).IsDialogueRepetable);
                    break;


                case NodeType.Event:
                    var eventDialogue = (dialogue as EventDialogueSO);
                    var eventNode = (EventNodeData)nodeData;
                    eventDialogue.Initialize(nodeData.Name, nodeData.NodeID, (DialogueType)nodeData.NodeType,
                                             DataConversion.LenguageDataConvert(((EventNodeData)nodeData).Texts),
                                             DataConversion.NodeToDialogueChoice(((EventNodeData)nodeData).Choices));

                    if (((EventNodeData)nodeData).GameEvents == null) break;
                    eventDialogue.SetGameEvents(DataConversion.ConvertEvents(((EventNodeData)nodeData).GameEvents));
                    eventDialogue.SetVariableEvents(DataConversion.VarEventsToDialogueVarEvents(eventNode.VariableEventsContainer));

                    break;

                default:
                    var fullDialoue = (dialogue as DialogueSO);
                    var convertedTexts = DataConversion.LenguageDataConvert(((DialogueNodeData)nodeData).Texts);
                    fullDialoue.Initialize(nodeData.Name, nodeData.NodeID, (DialogueType)nodeData.NodeType, convertedTexts, DataConversion.NodeToDialogueChoice(((DialogueNodeData)nodeData).Choices));
                    break;
            }

            createdDialogues.Add(nodeData.NodeID, dialogue);
            BaseIO.SaveAsset(dialogue);
        }


        private void ConvertGroups(DialogueContainerSO dialogueContainer)
        {
            List<string> groupNames = new List<string>();

            foreach (var groupData in _graphSO.groups)
            {
                SaveGroupToSO(groupData, dialogueContainer);
                groupNames.Add(groupData.Name);
            }
            UpdateOldGroups(groupNames, _graphSO);
        }
        private void CreateStaticFolders()
        {
            BaseIO.CreateFolder("Assets", "Data");
            BaseIO.CreateFolder("Assets/Data", "GeneratedDialogues");
            BaseIO.CreateFolder("Assets/Data/GeneratedDialogues", graphFileName);
            BaseIO.CreateFolder(containerFolderPath, "Global");
            BaseIO.CreateFolder(containerFolderPath, "Groups");
            BaseIO.CreateFolder($"{containerFolderPath}/Global", "Dialogues");

        }
        private void SaveGroupToSO(GroupData group, DialogueContainerSO dialogueContainer)
        {
            string groupName = group.Name;
            BaseIO.CreateFolder($"{containerFolderPath}/Groups", groupName);
            BaseIO.CreateFolder($"{containerFolderPath}/Groups/{groupName}", "Dialogues");

            DialogueGroupSO dialogueGroup = BaseIO.CreateAsset<DialogueGroupSO>($"{containerFolderPath}/Groups/{groupName}", groupName);
            dialogueGroup.Initialize(groupName);

            createdGroups.Add(group.ID, dialogueGroup);

            dialogueContainer.DialogueGroups.Add(dialogueGroup, new List<BaseDialogueSO>());

            BaseIO.SaveAsset(dialogueGroup);
        }
        private void UpdateOldGroups(List<string> currentGroupNames, GraphSO graphData)
        {
            if (graphData.oldGroupsNames != null && graphData.oldGroupsNames.Count != 0)
            {
                List<string> groupsToRemove = graphData.oldGroupsNames.Except(currentGroupNames).ToList();
                foreach (string groupToRemove in groupsToRemove)
                {
                    BaseIO.RemoveFolder($"{containerFolderPath}/Groups/{groupToRemove}");
                }
            }
            graphData.oldGroupsNames = new List<string>(currentGroupNames);
        }
        private void UpdateOldGroupedNodes(Dictionary<string, List<string>> currentGroupedNodeNames, GraphSO graphData)
        {
            if (graphData.oldGroupedNodesNames != null && graphData.oldGroupedNodesNames.Count != 0)
            {
                foreach (KeyValuePair<string, List<string>> oldGroupedNodes in graphData.oldGroupedNodesNames)
                {
                    List<string> nodesToRemove = new List<string>();

                    if (currentGroupedNodeNames.ContainsKey(oldGroupedNodes.Key))
                    {
                        nodesToRemove = oldGroupedNodes.Value.Except(currentGroupedNodeNames[oldGroupedNodes.Key]).ToList();

                        foreach (string nodeToRemove in nodesToRemove)
                        {
                            BaseIO.RemoveAsset($"{containerFolderPath}/Groups/{oldGroupedNodes.Key}/Dialogue", nodeToRemove);
                        }
                    }
                    else
                    {
                        BaseIO.RemoveFolder($"{containerFolderPath}/Groups/{oldGroupedNodes.Key}");
                    }
                }
                graphData.oldGroupedNodesNames = new Dictionary<string, List<string>>(currentGroupedNodeNames);
            }
        }
        private void UpdateOldUngroupedNodes(List<string> currentUngroupedNodeNames, GraphSO graphData)
        {
            if (graphData.oldUngroupedNames != null && graphData.oldUngroupedNames.Count != 0)
            {
                List<string> nodesToRemove = graphData.oldUngroupedNames.Except(currentUngroupedNodeNames).ToList();
                foreach (string nodeToRemove in nodesToRemove)
                {
                    BaseIO.RemoveAsset($"{containerFolderPath}/Global/Dialogues", nodeToRemove);
                }
            }
            graphData.oldUngroupedNames = new List<string>(currentUngroupedNodeNames);
        }

        private void UpdateDialogueChoicesConnection()
        {
            foreach (var node in _graphSO.GetAllNodes())
            {
                BaseDialogueSO dialogue = createdDialogues[node.NodeID];

                if (node.NodeType != NodeType.End && node.NodeType != NodeType.Branch) // Aggiungere caso per branch
                {
                    var dialogueNode = (DialogueNodeData)node;
                    var choicedDialogue = dialogue as DialogueSO;


                    for (int choiceIndex = 0; choiceIndex < dialogueNode.Choices.Count; choiceIndex++)
                    {
                        var choice = dialogueNode.Choices[choiceIndex];

                        if (string.IsNullOrEmpty(choice.NextNodeID)) continue;
                        else
                        {
                            choicedDialogue.Choices[choiceIndex].NextDialogue = createdDialogues[choice.NextNodeID];
                            BaseIO.SaveAsset(dialogue);
                        }
                    }
                }
                else if(node.NodeType == NodeType.Branch)
                {
                    var dialogueNode = (BranchNodeData)node;
                    var choicedDialogue = dialogue as BranchDialogueSO;


                    for (int choiceIndex = 0; choiceIndex < dialogueNode.Choices.Count; choiceIndex++)
                    {
                        var choice = dialogueNode.Choices[choiceIndex];

                        if (string.IsNullOrEmpty(choice.NextNodeID)) continue;
                        else
                        {
                            choicedDialogue.Choices[choiceIndex].NextDialogue = createdDialogues[choice.NextNodeID];
                            BaseIO.SaveAsset(dialogue);
                        }
                    }
                }
                else continue;
            }
        }
    }  
}
