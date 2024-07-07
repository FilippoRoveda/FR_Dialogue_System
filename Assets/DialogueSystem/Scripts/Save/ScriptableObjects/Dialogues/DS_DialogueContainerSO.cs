using System.Collections.Generic;
using UnityEngine;

namespace DS.Runtime.ScriptableObjects
{
    using Runtime.Utilities;

    [System.Serializable]
    public class DS_DialogueContainerSO : ScriptableObject
    {
        [SerializeField] private string graphName;
        public string GraphName
        {
            get { return graphName; }
            set { graphName = value; }
        }
        [SerializeField] private SerializableDictionary<DS_DialogueGroupSO, List<DS_DialogueSO>> dialogueGroups;

        /// <summary>
        /// Dictionary containing as key a group scriptable object and as a value a list of owned dialogue scriptable objects.
        /// </summary>
        public SerializableDictionary<DS_DialogueGroupSO, List<DS_DialogueSO>> DialogueGroups
        {
            get { return dialogueGroups; }
            set { dialogueGroups = value; }
        }

        [SerializeField] private List<DS_DialogueSO> ungroupedDialogues;
        /// <summary>
        /// List of ungrouped dialogues scriptable objects.
        /// </summary>
        public List<DS_DialogueSO> UngroupedDialogues
        {
            get { return ungroupedDialogues; }
            set { ungroupedDialogues = value; }
        }

        public void Initialize(string filename)
        {
            GraphName = filename;
            DialogueGroups = new SerializableDictionary<DS_DialogueGroupSO, List<DS_DialogueSO>>();
            UngroupedDialogues = new List<DS_DialogueSO>();
        }


        public List<DS_DialogueSO> GetAllDialogues()
        {
            List<DS_DialogueSO> dialogues = new List<DS_DialogueSO>();
            foreach (DS_DialogueGroupSO group in DialogueGroups.Keys)
            {
                foreach (DS_DialogueSO dialogue in DialogueGroups[group])
                {
                    dialogues.Add(dialogue);
                }
            }
            foreach (DS_DialogueSO dialogue in UngroupedDialogues)
            {
                dialogues.Add(dialogue);
            }
            return dialogues;
        }
        public List<DS_DialogueSO> GetStartingDialogues()
        {
            List<DS_DialogueSO> startingDialogues = new List<DS_DialogueSO>();
            foreach(DS_DialogueGroupSO group in DialogueGroups.Keys)
            {
                foreach(DS_DialogueSO dialogue in DialogueGroups[group])
                {
                    if(dialogue.IsStartingDialogue == true)
                    {
                        startingDialogues.Add(dialogue);
                    }
                }
            }
            foreach(DS_DialogueSO dialogue in UngroupedDialogues)
            {
                if (dialogue.IsStartingDialogue == true)
                {
                    startingDialogues.Add(dialogue);
                }
            }
            return startingDialogues;
        }

    }
}
