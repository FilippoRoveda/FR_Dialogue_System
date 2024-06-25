using System.Collections.Generic;
using UnityEngine;

namespace DS.Data.Save
{
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


        [SerializeField] private List<DS_Group_SaveData> groups;
        /// <summary>
        /// List of group save data for the groups contained in this graph.
        /// </summary>
        public List<DS_Group_SaveData> Groups 
        { 
            get { return groups; } 
            set {  groups = value; } 
        }


        /// <summary>
        /// List of nodes for DS_Nodes contained in this graph.
        /// </summary>
        [SerializeField] private List<DS_Node_SaveData> nodes;
        public List<DS_Node_SaveData> Nodes 
        { 
            get { return nodes; }
            set {  nodes = value; } 
        }

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
            Groups = new List<DS_Group_SaveData>();
            Nodes = new List<DS_Node_SaveData>();
        }
    }
}