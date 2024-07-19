using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using System.Linq;

namespace DS.Editor.Windows.Utilities
{
    using Editor.Elements;
    using Editor.Data;
    using Editor.Utilities;
    using System.Diagnostics;

    public class GraphLoad
    {
        private GraphIOSystem _system;
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
                //LoadNode(nodeData, true);
                //DialogueNode dialogueNode = null;
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

                


                //BaseNode node = _system.graphView.CreateNode(nodeData.Name, nodeData.Position, nodeData.DialogueType, false);
                //var dialogueNode = (DialogueNode)node;

                //node.Data.NodeID = nodeData.NodeID;
                //List<ChoiceData> clonedChoices = CloneChoices(nodeData.Choices);

                //dialogueNode.Data.Choices = clonedChoices;

            }
        }

        public void LoadEventNodes(List<EventNodeData> eventNodes)
        {
            foreach (EventNodeData evntNodeData in eventNodes)
            {
                //var node = (EventNode)LoadNode(evntNodeData, false);
                EventNode eventNode = _system.graphView.CreateNode<EventNode, EventNodeData>(evntNodeData);
                eventNode.Initialize(evntNodeData, _system.graphView);
                //if (evntNodeData.Events != null && evntNodeData.Events.Count != 0)
                //{
                //    foreach (var _event in evntNodeData.Events)
                //    {
                //        node.Data.Events.Add(_event);
                //    }
                //}
                //else node.Data.Events = new List<DS_EventSO> { };
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
                //var node = _system.graphView.CreateNode(endNodeData.Name, endNodeData.Position, endNodeData.DialogueType, false) as EndNode;
                EndNode endNode = _system.graphView.CreateNode<EndNode, EndNodeData>(endNodeData);
                endNode.Initialize(endNodeData, _system.graphView);
                //node.Data.NodeID = endNodeData.NodeID;
                //node.Data.Texts = new(LenguageUtilities.UpdateLenguageDataSet(endNodeData.Texts));

                _system.graphView.AddElement(endNode);
                endNode.Draw();
                if (string.IsNullOrEmpty(endNodeData.GroupID) == false)
                {
                    DS_Group group = _system.loadedGroups[endNodeData.GroupID];
                    endNode.Group = group;
                    group.AddElement(endNode);

                }
                _system.loadedEndNodes.Add(endNode._nodeID, endNode);

                //endNode.Data.IsDialogueRepetable = endNodeData.IsDialogueRepetable;
            }
        }

        //private BaseNode LoadNode(DialogueNodeData nodeData, bool draw = false)
        //{
        //    BaseNode node = _system.graphView.CreateNode(nodeData.Name, nodeData.Position, nodeData.DialogueType, false);
        //    var dialogueNode = (DialogueNode)node;

        //    node.Data.NodeID = nodeData.NodeID;
        //    List<ChoiceData> clonedChoices = CloneChoices(nodeData.Choices);

        //    dialogueNode.Data.Choices = clonedChoices;
        //    dialogueNode.Data.Texts = new(LenguageUtilities.UpdateLenguageDataSet(nodeData.Texts));

        //    if (draw == true) node.Draw();
        //    _system.graphView.AddElement(node);

        //    if (string.IsNullOrEmpty(nodeData.GroupID) == false)
        //    {
        //        DS_Group group = _system.loadedGroups[nodeData.GroupID];
        //        node.Group = group;
        //        group.AddElement(node);

        //    }
        //    _system.loadedNodes.Add(node.Data.NodeID, node);

        //    return node;
        //}
        public void LoadNodesConnections()
        {
            foreach (KeyValuePair<string, DialogueNode> loadedNode in _system.loadedDialogueNodes)
            {
                foreach (Port choicePort in loadedNode.Value.outputContainer.Children().Cast<Port>())
                {
                    ChoiceData choiceData = (ChoiceData)choicePort.userData;

                    if (string.IsNullOrEmpty(choiceData.NextNodeID) == false)
                    {
                        Port linkedPort = FindLinkedPort(choiceData.NextNodeID);
                        //BaseNode linkedNode = _system.loadedNodes[choiceData.NextNodeID];
                        //Port linkedNodeInputPort = (Port)linkedNode.inputContainer.Children().First();
                        Edge edge = choicePort.ConnectTo(linkedPort);
                        _system.graphView.AddElement(edge);
                        loadedNode.Value.RefreshPorts();
                    }
                }
            }
            foreach (KeyValuePair<string, EventNode> loadedNode in _system.loadedEventNodes)
            {
                foreach (Port choicePort in loadedNode.Value.outputContainer.Children().Cast<Port>())
                {
                    ChoiceData choiceData = (ChoiceData)choicePort.userData;

                    if (string.IsNullOrEmpty(choiceData.NextNodeID) == false)
                    {
                        Port linkedPort = FindLinkedPort(choiceData.NextNodeID);
                        //BaseNode linkedNode = _system.loadedNodes[choiceData.NextNodeID];
                        //Port linkedNodeInputPort = (Port)linkedNode.inputContainer.Children().First();
                        Edge edge = choicePort.ConnectTo(linkedPort);
                        _system.graphView.AddElement(edge);
                        loadedNode.Value.RefreshPorts();
                    }
                }
            }
            //foreach (KeyValuePair<string, EndNode> loadedNode in _system.loadedEndNodes)
            //{
            //    foreach (Port choicePort in loadedNode.Value.outputContainer.Children().Cast<Port>())
            //    {
            //        ChoiceData choiceData = (ChoiceData)choicePort.userData;

            //        if (string.IsNullOrEmpty(choiceData.NextNodeID) == false)
            //        {
            //            Port linkedPort = FindLinkedPort(choiceData.NextNodeID);
            //            //BaseNode linkedNode = _system.loadedNodes[choiceData.NextNodeID];
            //            //Port linkedNodeInputPort = (Port)linkedNode.inputContainer.Children().First();
            //            Edge edge = choicePort.ConnectTo(linkedPort);
            //            _system.graphView.AddElement(edge);
            //            loadedNode.Value.RefreshPorts();
            //        }
            //    }
            //}
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
            else
            {
                Logger.Error($"The output port lineked to the node with ID:{linkedNodeID} has not founded that inside the loaded nodes.");
            }
            return linkedNodeInputPort;
            //case per i brance node
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
    }
}
