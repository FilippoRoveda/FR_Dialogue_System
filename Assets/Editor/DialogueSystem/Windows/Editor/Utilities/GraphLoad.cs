using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using System.Linq;

namespace DS.Editor.Windows.Utilities
{
    using Elements;
    using Runtime.Data;
    using Runtime.ScriptableObjects;
    using Editor.Data;

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
                LoadNode(nodeData, true);
            }
        }

        public void LoadEventNodes(List<EventNodeData> eventNodes)
        {
            foreach (EventNodeData evntNodeData in eventNodes)
            {
                var node = (EventNode)LoadNode(evntNodeData, false);
                if (evntNodeData.Events != null && evntNodeData.Events.Count != 0)
                {
                    foreach (var _event in evntNodeData.Events)
                    {
                        node.Data.Events.Add(_event);
                    }
                }
                else node.Data.Events = new List<DS_EventSO> { };
                node.Draw();
            }
        }
        public void LoadEndNodes(List<EndNodeData> endNodes)
        {
            foreach (EndNodeData endNodeData in endNodes)
            {
                var node = _system.graphView.CreateNode(endNodeData.Name, endNodeData.Position, endNodeData.DialogueType, false) as EndNode;

                node.Data.NodeID = endNodeData.NodeID;
                node.Data.Texts = new(LenguageUtilities.UpdateLenguageDataSet(endNodeData.Texts));

                _system.graphView.AddElement(node);

                if (string.IsNullOrEmpty(endNodeData.GroupID) == false)
                {
                    DS_Group group = _system.loadedGroups[endNodeData.GroupID];
                    node.Group = group;
                    group.AddElement(node);

                }
                _system.loadedNodes.Add(node.Data.NodeID, node);

                node.Data.IsDialogueRepetable = endNodeData.IsDialogueRepetable;
                node.Draw();
            }
        }

        private BaseNode LoadNode(DialogueNodeData nodeData, bool draw = false)
        {
            BaseNode node = _system.graphView.CreateNode(nodeData.Name, nodeData.Position, nodeData.DialogueType, false);
            var dialogueNode = (DialogueNode)node;

            node.Data.NodeID = nodeData.NodeID;
            List<ChoiceData> clonedChoices = CloneChoices(nodeData.Choices);

            dialogueNode.Data.Choices = clonedChoices;
            dialogueNode.Data.Texts = new(LenguageUtilities.UpdateLenguageDataSet(nodeData.Texts));

            if (draw == true) node.Draw();
            _system.graphView.AddElement(node);

            if (string.IsNullOrEmpty(nodeData.GroupID) == false)
            {
                DS_Group group = _system.loadedGroups[nodeData.GroupID];
                node.Group = group;
                group.AddElement(node);

            }
            _system.loadedNodes.Add(node.Data.NodeID, node);

            return node;
        }
        public void LoadNodesConnections()
        {
            foreach (KeyValuePair<string, BaseNode> loadedNode in _system.loadedNodes)
            {
                foreach (Port choicePort in loadedNode.Value.outputContainer.Children())
                {
                    ChoiceData choiceData = (ChoiceData)choicePort.userData;

                    if (string.IsNullOrEmpty(choiceData.NextNodeID) == false)
                    {
                        BaseNode linkedNode = _system.loadedNodes[choiceData.NextNodeID];
                        Port linkedNodeInputPort = (Port)linkedNode.inputContainer.Children().First();
                        Edge edge = choicePort.ConnectTo(linkedNodeInputPort);
                        _system.graphView.AddElement(edge);
                        loadedNode.Value.RefreshPorts();
                    }
                }
            }
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
