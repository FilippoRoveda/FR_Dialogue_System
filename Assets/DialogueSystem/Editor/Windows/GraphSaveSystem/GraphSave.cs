using System.Collections.Generic;
using System.Linq;


namespace DS.Editor.Windows.Utilities
{
    using Elements;
    using Editor.Data;
    using Editor.ScriptableObjects;
    using Editor.Utilities;

    public class GraphSave
    {
        private GraphIOSystem _system;
        public GraphSave(GraphIOSystem _system) { this._system = _system; }


        //public void SaveGroups(GraphSO graphData, DialogueContainerSO dialogueContainer)
        public void SaveGroups(GraphSO graphData)
        {
            List<string> groupNames = new List<string>();

            foreach (DS_Group group in _system.groups)
            {
                SaveGroupInGraphData(group, graphData);
                //SaveGroupToSO(group, dialogueContainer);
                groupNames.Add(group.title);
            }

            UpdateOldGroups(groupNames, graphData);
        }
        private void SaveGroupInGraphData(DS_Group group, GraphSO graphData)
        {
            GroupData groupData = new GroupData(group.ID, group.title, group.GetPosition().position);
            graphData.groups.Add(groupData);
        }
        private void UpdateOldGroups(List<string> currentGroupNames, GraphSO graphData)
        {
            if (graphData.oldGroupsNames != null && graphData.oldGroupsNames.Count != 0)
            {
                List<string> groupsToRemove = graphData.oldGroupsNames.Except(currentGroupNames).ToList();
                foreach (string groupToRemove in groupsToRemove)
                {
                    _system.BaseIO.RemoveFolder($"{_system.containerFolderPath}/Groups/{groupToRemove}");
                }
            }
            graphData.oldGroupsNames = new List<string>(currentGroupNames);
        }

        /*//private void SaveGroupToSO(DS_Group group, DialogueContainerSO dialogueContainer)
        //private void SaveGroupToSO(DS_Group group, DialogueContainerSO dialogueContainer)
        //{
        //    string groupName = group.title;
        //    _system.BaseIO.CreateFolder($"{_system.containerFolderPath}/Groups", groupName);
        //    _system.BaseIO.CreateFolder($"{_system.containerFolderPath}/Groups/{groupName}", "Dialogues");

        //    DialogueGroupSO dialogueGroup = _system.BaseIO.CreateAsset<DialogueGroupSO>($"{_system.containerFolderPath}/Groups/{groupName}", groupName);
        //    dialogueGroup.Initialize(groupName);

        //    _system.createdGroupsSO.Add(group.ID, dialogueGroup);

        //    dialogueContainer.DialogueGroups.Add(dialogueGroup, new List<BaseDialogueSO>());

        //    _system.BaseIO.SaveAsset(dialogueGroup);
        //}

        //public void SaveNodes(GraphSO graphData, DialogueContainerSO dialogueContainer)*/

        public void SaveNodes(GraphSO graphData)
        {
            SerializableDictionary<string, List<string>> groupedNodeNames = new SerializableDictionary<string, List<string>>();
            List<string> ungroupedNodeNames = new List<string>();

            foreach(var node in _system.dialogueNodes) 
            {
                Logger.Error("Saving a " +node._nodeType.ToString());

                DialogueNodeData nodeData = new DialogueNodeData(node);
                Logger.Error("Generetad data of type " + nodeData.NodeType.ToString());
                graphData.dialogueNodes.Add(nodeData);

                /*//switch (node.Data.DialogueType)
                //{
                //    case Enums.NodeType.Branch:
                //        SaveNodeToSO<BranchDialogueSO>(node, dialogueContainer);
                //        break;
                //    case Enums.NodeType.Event:
                //        SaveNodeToSO<EventDialogueSO>(node, dialogueContainer);
                //        break;
                //    case Enums.NodeType.End:
                //        SaveNodeToSO<EndDialogueSO>(node, dialogueContainer);
                //        break;
                //    default:
                //        SaveNodeToSO<DialogueSO>(node, dialogueContainer);
                //        break;
                //}*/

                if (node.Group != null)
                {
                    groupedNodeNames.AddItem(node.Group.title, node._nodeName);
                }
                else
                {
                    ungroupedNodeNames.Add(node._nodeName);
                }
            }
            foreach (var node in _system.eventNodes) 
            {
                Logger.Error("Saving a " + node._nodeType.ToString());

                Logger.Message($"{node._nodeType}");
                EventNodeData eventNodeData = new EventNodeData(node);
                Logger.Message($"{eventNodeData.NodeType}");
                graphData.eventNodes.Add(eventNodeData);

                if (node.Group != null)
                {
                    groupedNodeNames.AddItem(node.Group.title, node._nodeName);
                }
                else
                {
                    ungroupedNodeNames.Add(node._nodeName);
                }
            }
            foreach (var node in _system.endNodes) 
            {
                Logger.Error("Saving a " + node._nodeType.ToString());
                Logger.Error(node._position.ToString());
                EndNodeData endNodeData = new EndNodeData(node);
                Logger.Error(endNodeData.Position.ToString());

                graphData.endNodes.Add(endNodeData);
                if (node.Group != null)
                {
                    groupedNodeNames.AddItem(node.Group.title, node._nodeName);
                }
                else
                {
                    ungroupedNodeNames.Add(node._nodeName);
                }
            }
            foreach (var node in _system.branchNodes) { }


            //UpdateDialogueChoicesConnection();
            UpdateOldGroupedNodes(groupedNodeNames, graphData);
            UpdateOldUngroupedNodes(ungroupedNodeNames, graphData);
        }
        
       /* //private void SaveNodeToSO<T>(BaseNode node, DialogueContainerSO dialogueContainer) where T : BaseDialogueSO
        //{
        //    T dialogue;

        //    if (node.Group == null)
        //    {
        //        dialogue = _system.BaseIO.CreateAsset<T>($"{_system.containerFolderPath}/Global/Dialogues", node.Data.Name);
        //        dialogueContainer.UngroupedDialogues.Add(dialogue);
        //    }
        //    else
        //    {
        //        dialogue = _system.BaseIO.CreateAsset<T>($"{_system.containerFolderPath}/Groups/{node.Group.title}/Dialogues", node.Data.Name);

        //        dialogueContainer.DialogueGroups.AddItem(_system.createdGroupsSO[node.Group.ID], dialogue);
        //    }

        //    switch (node.Data.DialogueType)
        //    {
        //        case Enums.NodeType.Branch:
        //            var branchDialogue = (dialogue as BranchDialogueSO);
        //            branchDialogue.Initialize(node.Data.Name, node.Data.NodeID, node.Data.DialogueType, node.IsStartingNode());
        //            //
        //            break;
        //        case Enums.NodeType.End:
        //            var endDialogue = (dialogue as EndDialogueSO);
        //            endDialogue.Initialize(node.Data.Name, node.Data.NodeID, node.Data.DialogueType, node.IsStartingNode(), ((EndNode)node).Data.Texts);
        //            endDialogue.SetRepetableDialogue(((EndNode)node).Data.IsDialogueRepetable);
        //            break;
        //        case Enums.NodeType.Event:

        //            var eventDialogue = (dialogue as EventDialogueSO);
        //            var eventNode = (EventNode)node;
        //            Editor.Utilities.Logger.Message(eventNode.Data.Choices.Count.ToString());
        //            eventDialogue.Initialize(node.Data.Name, node.Data.NodeID, node.Data.DialogueType, 
        //                                     node.IsStartingNode(), ((EventNode)node).Data.Texts, 
        //                                     NodeToDialogueChoice(((EventNode)node).Data.Choices));

        //            eventDialogue.SaveEvents(((EventNode)node).Data.Events);

        //            break;
        //        default:
        //            var fullDialoue = (dialogue as DialogueSO);
        //            fullDialoue.Initialize(node.Data.Name, node.Data.NodeID, node.Data.DialogueType, node.IsStartingNode(), ((DialogueNode)node).Data.Texts, NodeToDialogueChoice(((DialogueNode)node).Data.Choices));
        //            break;
        //    }

        //    _system.createdDialoguesSO.Add(node.Data.NodeID, dialogue);
        //    _system.BaseIO.SaveAsset(dialogue);
        //}
        //private List<DialogueChoiceData> NodeToDialogueChoice(List<ChoiceData> nodeChoices)
        //{
        //    Editor.Utilities.Logger.Message(nodeChoices[0].ChoiceTexts[0].Data);
        //    List<DialogueChoiceData> dialogueChoices = new();
        //    foreach (ChoiceData choiceData in nodeChoices)
        //    {
        //        DialogueChoiceData dialogueChoice = new() { ChoiceTexts = new(choiceData.ChoiceTexts) };
        //        dialogueChoice.ChoiceID = choiceData.ChoiceID;
        //        dialogueChoices.Add(dialogueChoice);
        //    }
        //    return dialogueChoices;
        //}
        //private void UpdateDialogueChoicesConnection()
        //{
        //    foreach (BaseNode node in _system.nodes)
        //    {
        //        BaseDialogueSO dialogue = _system.createdDialoguesSO[node.Data.NodeID];

        //        if (node.Data.DialogueType != Enums.NodeType.End) // Aggiungere caso per branch
        //        {
        //            var dialogueNode = (DialogueNode)node;
        //            var choicedDialogue = dialogue as DialogueSO;


        //            for (int choiceIndex = 0; choiceIndex < dialogueNode.Data.Choices.Count; choiceIndex++)
        //            {               
        //                var choice = dialogueNode.Data.Choices[choiceIndex];

        //                if (string.IsNullOrEmpty(choice.NextNodeID)) continue;
        //                else
        //                {
        //                    choicedDialogue.Choices[choiceIndex].NextDialogue = _system.createdDialoguesSO[choice.NextNodeID];
        //                    _system.BaseIO.SaveAsset(dialogue);
        //                }
        //            }
        //        }
        //        else continue;
        //    }
        //}*/
        private void UpdateOldGroupedNodes(SerializableDictionary<string, List<string>> currentGroupedNodeNames, GraphSO graphData)
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
                            _system.BaseIO.RemoveAsset($"{_system.containerFolderPath}/Groups/{oldGroupedNodes.Key}/Dialpgue", nodeToRemove);
                        }
                    }
                    else
                    {
                        _system.BaseIO.RemoveFolder($"{_system.containerFolderPath}/Groups/{oldGroupedNodes.Key}");
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
                    _system.BaseIO.RemoveAsset($"{_system.containerFolderPath}/Global/Dialogues", nodeToRemove);
                }
            }
            graphData.oldUngroupedNames = new List<string>(currentUngroupedNodeNames);
        }
    }
}
