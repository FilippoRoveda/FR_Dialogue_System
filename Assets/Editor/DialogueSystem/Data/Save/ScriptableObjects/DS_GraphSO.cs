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


        [SerializeField] private List<DS_GroupData> groups;
        /// <summary>
        /// List of group save data for the groups contained in this graph.
        /// </summary>
        public List<DS_GroupData> Groups 
        { 
            get { return groups; } 
            set {  groups = value; } 
        }


        [SerializeField] private List<DS_NodeData> nodes;
        [SerializeField] private List<DS_EventNodeData> eventNodes;
        [SerializeField] private List<DS_EndNodeData> endNodes;

        public List<DS_NodeData> Nodes { get => nodes; set => nodes = value; }
        public List<DS_EventNodeData> EventNodes { get => eventNodes; set => eventNodes = value; }
        public List<DS_EndNodeData> EndNodes { get => endNodes; set => endNodes = value; }


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
            Groups = new List<DS_GroupData>();
            nodes = new List<DS_NodeData>();
            eventNodes = new List<DS_EventNodeData>();
            endNodes = new List<DS_EndNodeData>();
        }

        public List<DS_NodeData> GetAllNodes()
        {
            List<DS_NodeData> allNodes = new List<DS_NodeData>();
            foreach (DS_NodeData node in Nodes)
            {
                allNodes.Add(node);
            }
            foreach (DS_NodeData evntNode in EventNodes)
            {
                allNodes.Add(evntNode);
            }
            foreach (DS_NodeData endNode in EndNodes)
            {
                allNodes.Add(endNode);
            }
            return allNodes;           
        }

        List<DS_NodeData> allNodes;
        public List<DS_NodeData> GetAllOrderedNodes()
        {
            List<DS_NodeData> orderedNodes = new List<DS_NodeData>();

            allNodes = GetAllNodes();
            var startingNodes = allNodes.FindAll(x => x.DialogueType == Enums.DS_DialogueType.Start);
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
       
    private void GetAllLinkedNodes(DS_NodeData node, ref List<DS_NodeData> output)
        {
            if(output == null) output = new List<DS_NodeData>();

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