using System.Collections.Generic;
using System.Linq;

using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace DS.Editor.Windows.Utilities
{
    using Editor.Elements;
    using Editor.Data;
    using Editor.Utilities;

    public class GraphLoad
    {
        private readonly GraphIOSystem _system;
        public GraphLoad(GraphIOSystem _system) { this._system = _system; }


        public void LoadGroups(List<GroupData> groups)
        {
            foreach (GroupData groupData in groups)
            {
                DS_Group group = _system.graphView.CreateGroup(groupData.Name, groupData.Position);
                group.ID = groupData.ID;

                _system.loadedGroups.Add(group.ID, group);
            }
        }

        public void LoadDialogueNodes(List<DialogueNodeData> nodes)
        {
            foreach (DialogueNodeData nodeData in nodes)
            {
                if (nodeData.NodeType == Enumerations.NodeType.Single) 
                {
                    var dialogueNode = _system.graphView.CreateNode<SingleNode, DialogueNodeData>(nodeData);
                    dialogueNode.Initialize(nodeData, _system.graphView);

                    dialogueNode._texts = new(LenguageUtilities.UpdateLenguageDataSet(nodeData.Texts));
                    dialogueNode.Draw();
                    _system.graphView.AddElement(dialogueNode);
                    if (string.IsNullOrEmpty(nodeData.GroupID) == false)
                    {
                        DS_Group group = _system.loadedGroups[nodeData.GroupID];
                        dialogueNode.Group = group;
                        group.AddElement(dialogueNode);

                    }
                    _system.loadedDialogueNodes.Add(dialogueNode._nodeID, dialogueNode);
                }
                else if (nodeData.NodeType == Enumerations.NodeType.Multiple) 
                {
                    var dialogueNode = _system.graphView.CreateNode<MultipleNode, DialogueNodeData>(nodeData);
                    dialogueNode.Initialize(nodeData, _system.graphView);
                    dialogueNode._texts = new(LenguageUtilities.UpdateLenguageDataSet(nodeData.Texts));
                    dialogueNode.Draw();
                    _system.graphView.AddElement(dialogueNode);
                    if (string.IsNullOrEmpty(nodeData.GroupID) == false)
                    {
                        DS_Group group = _system.loadedGroups[nodeData.GroupID];
                        dialogueNode.Group = group;
                        group.AddElement(dialogueNode);

                    }
                    _system.loadedDialogueNodes.Add(dialogueNode._nodeID, dialogueNode);
                }
                else if(nodeData.NodeType == Enumerations.NodeType.Start)
                {
                    Logger.Message($"Loading a nodedata of type {nodeData.NodeType}");
                    var dialogueNode = _system.graphView.CreateNode<StartNode, DialogueNodeData>(nodeData);
                    dialogueNode.Initialize(nodeData, _system.graphView);

                    dialogueNode._texts = new(LenguageUtilities.UpdateLenguageDataSet(nodeData.Texts));
                    dialogueNode.Draw();
                    _system.graphView.AddElement(dialogueNode);
                    if (string.IsNullOrEmpty(nodeData.GroupID) == false)
                    {
                        DS_Group group = _system.loadedGroups[nodeData.GroupID];
                        dialogueNode.Group = group;
                        group.AddElement(dialogueNode);

                    }
                    _system.loadedDialogueNodes.Add(dialogueNode._nodeID, dialogueNode);
                }           
            }
        }
        public void LoadBranchNodes(List<BranchNodeData> branchNodes)
        {
            foreach (BranchNodeData branchNodeData in branchNodes)
            {
                BranchNode branchNode = _system.graphView.CreateNode<BranchNode, BranchNodeData>(branchNodeData);
                branchNode.Initialize(branchNodeData, _system.graphView);
               
                branchNode.Draw();
                _system.graphView.AddElement(branchNode);
                if (string.IsNullOrEmpty(branchNodeData.GroupID) == false)
                {
                    DS_Group group = _system.loadedGroups[branchNodeData.GroupID];
                    branchNode.Group = group;
                    group.AddElement(branchNode);

                }
                _system.loadedBranchNodes.Add(branchNode._nodeID, branchNode);
            }
        }

        public void LoadEventNodes(List<EventNodeData> eventNodes)
        {
            foreach (EventNodeData evntNodeData in eventNodes)
            {
                EventNode eventNode = _system.graphView.CreateNode<EventNode, EventNodeData>(evntNodeData);
                eventNode.Initialize(evntNodeData, _system.graphView);

                eventNode.Draw();
                _system.graphView.AddElement(eventNode);
                if (string.IsNullOrEmpty(evntNodeData.GroupID) == false)
                {
                    DS_Group group = _system.loadedGroups[evntNodeData.GroupID];
                    eventNode.Group = group;
                    group.AddElement(eventNode);

                }
                _system.loadedEventNodes.Add(eventNode._nodeID, eventNode);
            }
        }
        public void LoadEndNodes(List<EndNodeData> endNodes)
        {
            foreach (EndNodeData endNodeData in endNodes)
            {
                EndNode endNode = _system.graphView.CreateNode<EndNode, EndNodeData>(endNodeData);
                endNode.Initialize(endNodeData, _system.graphView);

                _system.graphView.AddElement(endNode);
                endNode.Draw();
                if (string.IsNullOrEmpty(endNodeData.GroupID) == false)
                {
                    DS_Group group = _system.loadedGroups[endNodeData.GroupID];
                    endNode.Group = group;
                    group.AddElement(endNode);

                }
                _system.loadedEndNodes.Add(endNode._nodeID, endNode);
            }
        }

        public void LoadNodesConnections()
        {
            foreach (KeyValuePair<string, DialogueNode> loadedNode in _system.loadedDialogueNodes)
            {
                foreach (Box box in loadedNode.Value.outputContainer.Children().Cast<Box>())
                {
                    var choicePort = box.Children().ToList().Find(x => x is Port) as Port;
                    ChoiceData choiceData = (ChoiceData)choicePort.userData;

                    if (string.IsNullOrEmpty(choiceData.NextNodeID) == false)
                    {
                        Port linkedPort = FindLinkedPort(choiceData.NextNodeID);
                        Edge edge = choicePort.ConnectTo(linkedPort);
                        _system.graphView.AddElement(edge);
                        loadedNode.Value.RefreshPorts();
                    }
                }
            }
            foreach (KeyValuePair<string, EventNode> loadedNode in _system.loadedEventNodes)
            {
                foreach (Box box in loadedNode.Value.outputContainer.Children().Cast<Box>())
                {
                    var choicePort = box.Children().First(x => x is Port) as Port;
                    ChoiceData choiceData = (ChoiceData)choicePort.userData;

                    if (string.IsNullOrEmpty(choiceData.NextNodeID) == false)
                    {
                        Port linkedPort = FindLinkedPort(choiceData.NextNodeID);
                        Edge edge = choicePort.ConnectTo(linkedPort);
                        _system.graphView.AddElement(edge);
                        loadedNode.Value.RefreshPorts();
                    }
                }
            }
            foreach (KeyValuePair<string, BranchNode> loadedNode in _system.loadedBranchNodes)
            {
                foreach (Box box in loadedNode.Value.outputContainer.Children().Cast<Box>())
                {
                    var choicePort = box.Children().First(x => x is Port) as Port;
                    ChoiceData choiceData = (ChoiceData)choicePort.userData;

                    if (string.IsNullOrEmpty(choiceData.NextNodeID) == false)
                    {
                        Port linkedPort = FindLinkedPort(choiceData.NextNodeID);
                        Edge edge = choicePort.ConnectTo(linkedPort);
                        _system.graphView.AddElement(edge);
                        loadedNode.Value.RefreshPorts();
                    }
                }
            }
        }

       private Port FindLinkedPort(string linkedNodeID)
        {
            Port linkedNodeInputPort = null;
            if(_system.loadedDialogueNodes.ContainsKey(linkedNodeID) == true)
            {
                DialogueNode linkedNode = _system.loadedDialogueNodes[linkedNodeID];
                linkedNodeInputPort = (Port)linkedNode.inputContainer.Children().First();
            }
            else if (_system.loadedEventNodes.ContainsKey(linkedNodeID) == true)
            {
                EventNode linkedNode = _system.loadedEventNodes[linkedNodeID];
                linkedNodeInputPort = (Port)linkedNode.inputContainer.Children().First();
            }
            else if(_system.loadedEndNodes.ContainsKey(linkedNodeID) == true)
            {
                EndNode linkedNode = _system.loadedEndNodes[linkedNodeID];
                linkedNodeInputPort = (Port)linkedNode.inputContainer.Children().First();
            }
            else if (_system.loadedBranchNodes.ContainsKey(linkedNodeID) == true)
            {
                BranchNode linkedNode = _system.loadedBranchNodes[linkedNodeID];
                linkedNodeInputPort = (Port)linkedNode.inputContainer.Children().First();
            }
            else
            {
                Logger.Error($"The output port lineked to the node with ID:{linkedNodeID} has not founded that inside the loaded nodes.");
            }
            return linkedNodeInputPort;
        }

    }
}
