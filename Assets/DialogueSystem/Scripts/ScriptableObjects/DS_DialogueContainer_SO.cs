using System.Collections.Generic;
using UnityEngine;


namespace DS.ScriptableObjects
{
    [System.Serializable]
    public class DS_DialogueContainer_SO : ScriptableObject
    {
        [SerializeField] private string filename;
        public string Filename 
        {  
            get {  return filename; }
            set {  filename = value; } 
        }
        [SerializeField] private SerializableDictionary<DS_DialogueGroup_SO, List<DS_Dialogue_SO>> dialogueGroups;
        public SerializableDictionary<DS_DialogueGroup_SO, List<DS_Dialogue_SO>> DialogueGroups 
        { 
            get { return dialogueGroups; } 
            set {  dialogueGroups = value; } 
        }
        [SerializeField] private List<DS_Dialogue_SO> ungroupedDialogues;
        public List<DS_Dialogue_SO> UngroupedDialogues 
        {  
            get { return ungroupedDialogues; } 
            set {  ungroupedDialogues = value; } 
        }

        public void Initialize(string filename)
        {
            Filename = filename;
            DialogueGroups = new SerializableDictionary<DS_DialogueGroup_SO, List<DS_Dialogue_SO>>();
            UngroupedDialogues = new List<DS_Dialogue_SO>();
        }
    }
}
