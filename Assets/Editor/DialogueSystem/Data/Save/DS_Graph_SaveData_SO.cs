using System.Collections.Generic;
using UnityEngine;

namespace DS.Data.Save
{
    public class DS_Graph_SaveData_SO : ScriptableObject
    {
        [SerializeField] private string fileName;
        public string FileName 
        {
            get { return fileName; } 
            set {  fileName = value; } 
        }
        [SerializeField] private List<DS_Group_SaveData> groups;
        public List<DS_Group_SaveData> Groups 
        { 
            get { return groups; } 
            set {  groups = value; } 
        }
        [SerializeField] private List<DS_Node_SaveData> nodes;
        public List<DS_Node_SaveData> Nodes 
        { 
            get { return nodes; }
            set {  nodes = value; } 
        }
        [SerializeField] private List<string> oldGroupsNames;
        public List<string> OldGroupsNames 
        { 
            get { return oldGroupsNames; } 
            set {  oldGroupsNames = value; } 
        }
        [SerializeField] private List<string> oldUngroupedNames;
        public List<string> OldUngroupedNodesNames 
        { 
            get { return oldUngroupedNames; } 
            set { oldUngroupedNames = value; }
        }
        [SerializeField] private SerializableDictionary<string, List<string>> oldGroupedNodesNames;
        public SerializableDictionary<string, List<string>> OldGroupedNodesNames 
        { 
            get { return oldGroupedNodesNames; }
            set { oldGroupedNodesNames = value; }
        }


        public void Initialize(string fileName)
        {
            FileName = fileName;
            Groups = new List<DS_Group_SaveData>();
            Nodes = new List<DS_Node_SaveData>();
        }
    }
}