using System.Collections.Generic;
using UnityEngine;

namespace DS.ScriptableObjects
{
    [System.Serializable]
    public class DS_DialogueContainerSO : ScriptableObject
    {
        [SerializeField] private string graphName;
        public string GraphName 
        {  
            get {  return graphName; }
            set {  graphName = value; } 
        }
        [SerializeField] private SerializableDictionary<DS_DialogueGroupSO, List<DS_DialogueSO>> dialogueGroups;

        /// <summary>
        /// Dictionary containing as key a group scriptable object and as a value a list of owned dialogue scriptable objects.
        /// </summary>
        public SerializableDictionary<DS_DialogueGroupSO, List<DS_DialogueSO>> DialogueGroups 
        { 
            get { return dialogueGroups; } 
            set {  dialogueGroups = value; } 
        }

        [SerializeField] private List<DS_DialogueSO> ungroupedDialogues;
        /// <summary>
        /// List of ungrouped dialogues scriptable objects.
        /// </summary>
        public List<DS_DialogueSO> UngroupedDialogues 
        {  
            get { return ungroupedDialogues; } 
            set {  ungroupedDialogues = value; } 
        }

        public void Initialize(string filename)
        {
            GraphName = filename;
            DialogueGroups = new SerializableDictionary<DS_DialogueGroupSO, List<DS_DialogueSO>>();
            UngroupedDialogues = new List<DS_DialogueSO>();
        }
    }
}
