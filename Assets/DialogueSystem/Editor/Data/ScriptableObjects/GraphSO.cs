using System.Collections.Generic;
using UnityEngine;

namespace DS.Editor.ScriptableObjects
{
    using Editor.Data;

    //[CreateAssetMenu(menuName = "sss", fileName = "ss")]
    public class GraphSO : ScriptableObject
    {
        [SerializeField] public string graphName;


        [SerializeField] public List<GroupData> groups;


        [SerializeField] public List<DialogueNodeData> dialogueNodes; //Start, Single, Multiple
        [SerializeField] public List<EventNodeData> eventNodes;
        [SerializeField] public List<EndNodeData> endNodes;
        //[SerializeField] private List<BranchNodeData> branchNodes;


        [SerializeField] public List<string> oldGroupsNames;
        [SerializeField] public List<string> oldUngroupedNames;
        [SerializeField] public Dictionary<string, List<string>> oldGroupedNodesNames;



        public void Initialize(string fileName)
        {
            graphName = fileName;
            groups = new List<GroupData>();
            dialogueNodes = new List<DialogueNodeData>();
            eventNodes = new List<EventNodeData>();
            endNodes = new List<EndNodeData>();
        }

        public List<BaseNodeData> GetAllNodes()
        {
            List<BaseNodeData> allNodes = new List<BaseNodeData>();
            foreach (DialogueNodeData node in dialogueNodes)
            {
                allNodes.Add(node);
            }
            foreach (DialogueNodeData evntNode in eventNodes)
            {
                allNodes.Add(evntNode);
            }
            foreach (BaseNodeData endNode in endNodes)
            {
                allNodes.Add(endNode);
            }
            //foreach (BranchNodeData branchNode in BranchNodes)
            //{
            //    allNodes.Add(branchNode);
            //}
            return allNodes;           
        }

        List<BaseNodeData> allNodes;
        public List<BaseNodeData> GetAllOrderedNodes()
        {
            List<BaseNodeData> orderedNodes = new List<BaseNodeData>();

            allNodes = GetAllNodes();
            var startingNodes = allNodes.FindAll(x => x.DialogueType == Enumerations.NodeType.Start);
            foreach(var startNode in startingNodes)
            {
                allNodes.Remove(startNode);
            }

            foreach (var node in startingNodes)
            {
                GetAllLinkedNodes(node, ref orderedNodes);
            }
            foreach(var node in endNodes)
            {
                orderedNodes.Remove(node);
            }
            orderedNodes.AddRange(endNodes);
            return orderedNodes;
        }
       
    private void GetAllLinkedNodes(BaseNodeData node, ref List<BaseNodeData> output)
        {
            if(output == null) output = new List<BaseNodeData>();

            if(output.Contains(node) == false) output.Add(node);
            if (node.DialogueType != Enumerations.NodeType.End)
            {
                var dialogueNode = (DialogueNodeData)node;
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