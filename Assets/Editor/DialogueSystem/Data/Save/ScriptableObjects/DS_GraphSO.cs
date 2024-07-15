using System.Collections.Generic;
using UnityEngine;

namespace DS.Editor.ScriptableObjects
{

    using Runtime.Utilities;
    using Data;

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


        [SerializeField] private List<NodeData> nodes;
        [SerializeField] private List<EventNodeData> eventNodes;
        [SerializeField] private List<EndNodeData> endNodes;

        public List<NodeData> Nodes { get => nodes; set => nodes = value; }
        public List<EventNodeData> EventNodes { get => eventNodes; set => eventNodes = value; }
        public List<EndNodeData> EndNodes { get => endNodes; set => endNodes = value; }


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


        [SerializeField] private SerializableDictionary<string, List<string>> oldGroupedNodesNames;
        /// <summary>
        /// Serialized dictionary that as key hold the name of an old group and as value hold a list of old nodes for updating graph functions.
        /// </summary>
        public SerializableDictionary<string, List<string>> OldGroupedNodesNames 
        { 
            get { return oldGroupedNodesNames; }
            set { oldGroupedNodesNames = value; }
        }



        public void Initialize(string fileName)
        {
            GraphName = fileName;
            Groups = new List<GroupData>();
            nodes = new List<NodeData>();
            eventNodes = new List<EventNodeData>();
            endNodes = new List<EndNodeData>();
        }

        public List<NodeData> GetAllNodes()
        {
            List<NodeData> allNodes = new List<NodeData>();
            foreach (NodeData node in Nodes)
            {
                allNodes.Add(node);
            }
            foreach (NodeData evntNode in EventNodes)
            {
                allNodes.Add(evntNode);
            }
            foreach (NodeData endNode in EndNodes)
            {
                allNodes.Add(endNode);
            }
            return allNodes;           
        }

        List<NodeData> allNodes;
        public List<NodeData> GetAllOrderedNodes()
        {
            List<NodeData> orderedNodes = new List<NodeData>();

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
       
    private void GetAllLinkedNodes(NodeData node, ref List<NodeData> output)
        {
            if(output == null) output = new List<NodeData>();

            if(output.Contains(node) == false) output.Add(node);

            if(node.Choices == null || node.Choices.Count == 0) return;

            foreach(var choice in node.Choices)
            {
                var nextNode = allNodes.Find(x => x.NodeID == choice.NextNodeID);
                if(allNodes.Contains(nextNode)) allNodes.Remove(nextNode);

                if (output.Contains(nextNode) == false)
                {
                    GetAllLinkedNodes(nextNode, ref output);
                }
            }
            return;
        }
    }
}