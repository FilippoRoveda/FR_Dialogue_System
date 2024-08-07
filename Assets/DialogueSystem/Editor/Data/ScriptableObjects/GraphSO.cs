using System.Collections.Generic;
using UnityEngine;

namespace DS.Editor.ScriptableObjects
{
    using Editor.Data;

    public class GraphSO : ScriptableObject
    {
        [SerializeField] public string _graphName;

        [SerializeField] public List<GroupData> _groups;

        [SerializeField] public List<DialogueNodeData> _dialogueNodes; //Start, Single, Multiple
        [SerializeField] public List<EventNodeData> _eventNodes;
        [SerializeField] public List<EndNodeData> _endNodes;
        [SerializeField] public List<BranchNodeData> _branchNodes;
        private List<BaseNodeData> allNodes;

        [SerializeField] public List<string> _oldGroupsNames;
        [SerializeField] public List<string> _oldUngroupedNames;
        [SerializeField] public Dictionary<string, List<string>> _oldGroupedNodesNames;

        
        public void Initialize(string fileName)
        {
            _graphName = fileName;
            _groups = new List<GroupData>();
            _dialogueNodes = new List<DialogueNodeData>();
            _eventNodes = new List<EventNodeData>();
            _endNodes = new List<EndNodeData>();
            _branchNodes = new List<BranchNodeData>();
        }

        public List<BaseNodeData> GetAllNodes()
        {
            List<BaseNodeData> allNodes = new List<BaseNodeData>();
            foreach (DialogueNodeData node in _dialogueNodes)
            {
                allNodes.Add(node);
            }
            foreach (DialogueNodeData evntNode in _eventNodes)
            {
                allNodes.Add(evntNode);
            }
            foreach (BaseNodeData endNode in _endNodes)
            {
                allNodes.Add(endNode);
            }
            foreach (BranchNodeData branchNode in _branchNodes)
            {
                allNodes.Add(branchNode);
            }
            return allNodes;           
        }


        public List<BaseNodeData> GetAllOrderedNodes()
        {
            List<BaseNodeData> orderedNodes = new List<BaseNodeData>();

            allNodes = GetAllNodes();
            var startingNodes = allNodes.FindAll(x => x.NodeType == Enumerations.NodeType.Start);
            foreach(var startNode in startingNodes)
            {
                allNodes.Remove(startNode);
            }

            foreach (var node in startingNodes)
            {
                GetAllLinkedNodes(node, ref orderedNodes);
            }
            foreach(var node in _endNodes)
            {
                orderedNodes.Remove(node);
            }
            orderedNodes.AddRange(_branchNodes);
            orderedNodes.AddRange(_endNodes);
            return orderedNodes;
        }
       
        /// <summary>
        /// This function aim to get every node linked to a certain one by descending every link in order to have a list more accurate possible as the dialogue flow coulb be.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="output"></param>
    private void GetAllLinkedNodes(BaseNodeData node, ref List<BaseNodeData> output)
        {
            if(output == null) output = new List<BaseNodeData>();

            if(output.Contains(node) == false) output.Add(node);
            if (node.NodeType != Enumerations.NodeType.End && node.NodeType != Enumerations.NodeType.Branch)
            {
                var dialogueNode = (DialogueNodeData)node;
                //Aggiungere opzione per branch node
                if (dialogueNode.Choices == null || dialogueNode.Choices.Count == 0) return;

                foreach (var choice in dialogueNode.Choices)
                {
                    if(choice.NextNodeID == "" || choice.NextNodeID == string.Empty || choice.NextNodeID == null)
                    { continue; }
                    var nextNode = allNodes.Find(x => x.NodeID == choice.NextNodeID);
                    if (allNodes.Contains(nextNode)) allNodes.Remove(nextNode);

                    if (output.Contains(nextNode) == false)
                    {
                        GetAllLinkedNodes(nextNode, ref output);
                    }
                }
            }
            return;
        }
    }
}