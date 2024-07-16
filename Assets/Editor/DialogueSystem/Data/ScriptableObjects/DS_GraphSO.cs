using System.Collections.Generic;
using UnityEngine;

namespace DS.Editor.ScriptableObjects
{
    using Editor.Data;

    public class DS_GraphSO : ScriptableObject
    {
        [SerializeField] private string graphName;
        /// <summary>
        /// Filename for the graph.
        /// </summary>
        public string GraphName 
        {
            get { return graphName; } 
            set {  graphName = value; } 
        }


        [SerializeField] private List<GroupData> groups;
        /// <summary>
        /// List of group save data for the groups contained in this graph.
        /// </summary>
        public List<GroupData> Groups 
        { 
            get { return groups; } 
            set {  groups = value; } 
        }


        [SerializeField] private List<DialogueNodeData> dialogueNodes; //Start, Single, Multiple
        [SerializeField] private List<EventNodeData> eventNodes;
        [SerializeField] private List<EndNodeData> endNodes;
        //[SerializeField] private List<BranchNodeData> branchNodes;

        public List<DialogueNodeData> DialogueNodes { get => dialogueNodes; set => dialogueNodes = value; }
        public List<EventNodeData> EventNodes { get => eventNodes; set => eventNodes = value; }
        public List<EndNodeData> EndNodes { get => endNodes; set => endNodes = value; }
        //public List<BranchNodeData> BranchNodes { get => branchNodes; set => branchNodes = value; }

        [SerializeField] private List<string> oldGroupsNames;
        /// <summary>
        /// List of old group names for updating graph functions.
        /// </summary>
        public List<string> OldGroupsNames 
        { 
            get { return oldGroupsNames; } 
            set {  oldGroupsNames = value; } 
        }


        [SerializeField] private List<string> oldUngroupedNames;
        /// <summary>
        /// List of old ungrouped nodes names for updating graph functions.
        /// </summary>
        public List<string> OldUngroupedNodesNames 
        { 
            get { return oldUngroupedNames; } 
            set { oldUngroupedNames = value; }
        }


        [SerializeField] private Dictionary<string, List<string>> oldGroupedNodesNames;
        /// <summary>
        /// Serialized dictionary that as key hold the name of an old group and as value hold a list of old nodes for updating graph functions.
        /// </summary>
        public Dictionary<string, List<string>> OldGroupedNodesNames 
        { 
            get { return oldGroupedNodesNames; }
            set { oldGroupedNodesNames = value; }
        }



        public void Initialize(string fileName)
        {
            GraphName = fileName;
            Groups = new List<GroupData>();
            dialogueNodes = new List<DialogueNodeData>();
            eventNodes = new List<EventNodeData>();
            endNodes = new List<EndNodeData>();
        }

        public List<BaseNodeData> GetAllNodes()
        {
            List<BaseNodeData> allNodes = new List<BaseNodeData>();
            foreach (DialogueNodeData node in DialogueNodes)
            {
                allNodes.Add(node);
            }
            foreach (DialogueNodeData evntNode in EventNodes)
            {
                allNodes.Add(evntNode);
            }
            foreach (BaseNodeData endNode in EndNodes)
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
            var startingNodes = allNodes.FindAll(x => x.DialogueType == Enums.DialogueType.Start);
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
            if (node.DialogueType != Enums.DialogueType.End)
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