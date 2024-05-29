using System.Collections.Generic;
using UnityEngine;

namespace DS.Data.Save
{
    public class DS_Graph_SaveData_SO : ScriptableObject
    {
        [SerializeField] public string FileName { get; set; }
        [SerializeField] public List<DS_Group_SaveData> Groups { get; set; }
        [SerializeField] public List<DS_Node_SaveData> Nodes { get; set; }
        [SerializeField] public List<string> OldGroupsNames { get; set; }
        [SerializeField] public List<string> OldUngroupedNodesNames { get; set; }
        [SerializeField] public SerializableDictionary<string, List<string>> OldGroupedNodesNames { get; set; }


        public void Initialize(string fileName)
        {
            FileName = fileName;
            Groups = new List<DS_Group_SaveData>();
            Nodes = new List<DS_Node_SaveData>();
        }
    }
}