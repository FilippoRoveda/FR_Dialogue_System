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
        private readonly GraphSystem graphSystem;
        public GraphLoad(GraphSystem graphSystem) { this.graphSystem = graphSystem; }


        public void LoadGroups(List<GroupData> groups)
        {
            foreach (GroupData groupData in groups)
            {
                DS_Group group = graphSystem.linkedGraphView.CreateGroup(groupData.Name, groupData.Position);
                group.ID = groupData.ID;

                graphSystem.loadedGroups.Add(group.ID, group);
            }
        }

        public void LoadDialogueNodes(List<DialogueNodeData> nodes)
        {
            foreach (DialogueNodeData nodeData in nodes)
            {
                if (nodeData.NodeType == Enumerations.NodeType.Single) 
                {
                    var dialogueNode = graphSystem.linkedGraphView.CreateNode<SingleNode, DialogueNodeData>(nodeData);
                    dialogueNode.Initialize(nodeData, graphSystem.linkedGraphView);

                    dialogueNode._texts = new(LenguageUtilities.UpdateLenguageDataSet(nodeData.Texts));
                    dialogueNode.Draw();
                    graphSystem.linkedGraphView.AddElement(dialogueNode);
                    if (string.IsNullOrEmpty(nodeData.GroupID) == false)
                    {
                        AddToGroup(nodeData, dialogueNode);

                    }
                    graphSystem.loadedDialogueNodes.Add(dialogueNode._nodeID, dialogueNode);
                }
                else if (nodeData.NodeType == Enumerations.NodeType.Multiple) 
                {
                    var dialogueNode = graphSystem.linkedGraphView.CreateNode<MultipleNode, DialogueNodeData>(nodeData);
                    dialogueNode.Initialize(nodeData, graphSystem.linkedGraphView);
                    dialogueNode._texts = new(LenguageUtilities.UpdateLenguageDataSet(nodeData.Texts));
                    dialogueNode.Draw();
                    graphSystem.linkedGraphView.AddElement(dialogueNode);
                    if (string.IsNullOrEmpty(nodeData.GroupID) == false)
                    {
                        AddToGroup(nodeData, dialogueNode);

                    }
                    graphSystem.loadedDialogueNodes.Add(dialogueNode._nodeID, dialogueNode);
                }
                else if(nodeData.NodeType == Enumerations.NodeType.Start)
                {
                    var dialogueNode = graphSystem.linkedGraphView.CreateNode<StartNode, DialogueNodeData>(nodeData);
                    dialogueNode.Initialize(nodeData, graphSystem.linkedGraphView);

                    dialogueNode._texts = new(LenguageUtilities.UpdateLenguageDataSet(nodeData.Texts));
                    dialogueNode.Draw();
                    graphSystem.linkedGraphView.AddElement(dialogueNode);
                    if (string.IsNullOrEmpty(nodeData.GroupID) == false)
                    {
                        AddToGroup(nodeData, dialogueNode);

                    }
                    graphSystem.loadedDialogueNodes.Add(dialogueNode._nodeID, dialogueNode);
                }           
            }
        }

        private void AddToGroup(BaseNodeData nodeData, BaseNode dialogueNode)
        {
            DS_Group group = graphSystem.loadedGroups[nodeData.GroupID];
            dialogueNode.Group = group;
            group.AddElement(dialogueNode);
        }

        public void LoadBranchNodes(List<BranchNodeData> branchNodes)
        {
            foreach (BranchNodeData branchNodeData in branchNodes)
            {
                BranchNode branchNode = graphSystem.linkedGraphView.CreateNode<BranchNode, BranchNodeData>(branchNodeData);
                branchNode.Initialize(branchNodeData, graphSystem.linkedGraphView);
               
                branchNode.Draw();
                graphSystem.linkedGraphView.AddElement(branchNode);
                if (string.IsNullOrEmpty(branchNodeData.GroupID) == false)
                {
                    AddToGroup(branchNodeData, branchNode);

                }
                graphSystem.loadedBranchNodes.Add(branchNode._nodeID, branchNode);
            }
        }

        public void LoadEventNodes(List<EventNodeData> eventNodes)
        {
            foreach (EventNodeData evntNodeData in eventNodes)
            {
                EventNode eventNode = graphSystem.linkedGraphView.CreateNode<EventNode, EventNodeData>(evntNodeData);
                eventNode.Initialize(evntNodeData, graphSystem.linkedGraphView);

                eventNode.Draw();
                graphSystem.linkedGraphView.AddElement(eventNode);
                if (string.IsNullOrEmpty(evntNodeData.GroupID) == false)
                {
                    AddToGroup(evntNodeData, eventNode);

                }
                graphSystem.loadedEventNodes.Add(eventNode._nodeID, eventNode);
            }
        }
        public void LoadEndNodes(List<EndNodeData> endNodes)
        {
            foreach (EndNodeData endNodeData in endNodes)
            {
                EndNode endNode = graphSystem.linkedGraphView.CreateNode<EndNode, EndNodeData>(endNodeData);
                endNode.Initialize(endNodeData, graphSystem.linkedGraphView);

                graphSystem.linkedGraphView.AddElement(endNode);
                endNode.Draw();
                if (string.IsNullOrEmpty(endNodeData.GroupID) == false)
                {
                    AddToGroup(endNodeData, endNode);

                }
                graphSystem.loadedEndNodes.Add(endNode._nodeID, endNode);
            }
        }

        /// <summary>
        /// From each linked node ID in nodeChoices recreate linkung edges and add them to the graphView.
        /// </summary>
        public void LoadNodesConnections()
        {
            foreach (KeyValuePair<string, DialogueNode> loadedNode in graphSystem.loadedDialogueNodes)
            {
                foreach (Box box in loadedNode.Value.outputContainer.Children().Cast<Box>())
                {
                    var choicePort = box.Children().ToList().Find(x => x is Port) as Port;
                    ChoiceData choiceData = (ChoiceData)choicePort.userData;

                    if (string.IsNullOrEmpty(choiceData.NextNodeID) == false)
                    {
                        Port linkedPort = FindLinkedPort(choiceData.NextNodeID);
                        Edge edge = choicePort.ConnectTo(linkedPort);
                        graphSystem.linkedGraphView.AddElement(edge);
                        loadedNode.Value.RefreshPorts();
                    }
                }
            }
            foreach (KeyValuePair<string, EventNode> loadedNode in graphSystem.loadedEventNodes)
            {
                foreach (Box box in loadedNode.Value.outputContainer.Children().Cast<Box>())
                {
                    var choicePort = box.Children().First(x => x is Port) as Port;
                    ChoiceData choiceData = (ChoiceData)choicePort.userData;

                    if (string.IsNullOrEmpty(choiceData.NextNodeID) == false)
                    {
                        Port linkedPort = FindLinkedPort(choiceData.NextNodeID);
                        Edge edge = choicePort.ConnectTo(linkedPort);
                        graphSystem.linkedGraphView.AddElement(edge);
                        loadedNode.Value.RefreshPorts();
                    }
                }
            }
            foreach (KeyValuePair<string, BranchNode> loadedNode in graphSystem.loadedBranchNodes)
            {
                foreach (Box box in loadedNode.Value.outputContainer.Children().Cast<Box>())
                {
                    var choicePort = box.Children().First(x => x is Port) as Port;
                    ChoiceData choiceData = (ChoiceData)choicePort.userData;

                    if (string.IsNullOrEmpty(choiceData.NextNodeID) == false)
                    {
                        Port linkedPort = FindLinkedPort(choiceData.NextNodeID);
                        Edge edge = choicePort.ConnectTo(linkedPort);
                        graphSystem.linkedGraphView.AddElement(edge);
                        loadedNode.Value.RefreshPorts();
                    }
                }
            }
        }

        /// <summary>
        /// Get the InputPort from the loaded node with the specified ID.
        /// </summary>
        /// <param name="linkedNodeID"></param>
        /// <returns></returns>
       private Port FindLinkedPort(string linkedNodeID)
        {
            Port linkedNodeInputPort = null;
            if(graphSystem.loadedDialogueNodes.ContainsKey(linkedNodeID) == true)
            {
                DialogueNode linkedNode = graphSystem.loadedDialogueNodes[linkedNodeID];
                linkedNodeInputPort = (Port)linkedNode.inputContainer.Children().First();
            }
            else if (graphSystem.loadedEventNodes.ContainsKey(linkedNodeID) == true)
            {
                EventNode linkedNode = graphSystem.loadedEventNodes[linkedNodeID];
                linkedNodeInputPort = (Port)linkedNode.inputContainer.Children().First();
            }
            else if(graphSystem.loadedEndNodes.ContainsKey(linkedNodeID) == true)
            {
                EndNode linkedNode = graphSystem.loadedEndNodes[linkedNodeID];
                linkedNodeInputPort = (Port)linkedNode.inputContainer.Children().First();
            }
            else if (graphSystem.loadedBranchNodes.ContainsKey(linkedNodeID) == true)
            {
                BranchNode linkedNode = graphSystem.loadedBranchNodes[linkedNodeID];
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
