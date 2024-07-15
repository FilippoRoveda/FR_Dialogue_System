using System.Collections.Generic;
using UnityEngine;

namespace DS.Runtime.ScriptableObjects
{
    using Runtime.Utilities;

    [System.Serializable]
    public class DialogueContainerSO : ScriptableObject
    {
        [SerializeField] private string graphName;
        public string GraphName
        {
            get { return graphName; }
            set { graphName = value; }
        }
        [SerializeField] private SerializableDictionary<DialogueGroupSO, List<DialogueSO>> dialogueGroups;

        /// <summary>
        /// Dictionary containing as key a group scriptable object and as a value a list of owned dialogue scriptable objects.
        /// </summary>
        public SerializableDictionary<DialogueGroupSO, List<DialogueSO>> DialogueGroups
        {
            get { return dialogueGroups; }
            set { dialogueGroups = value; }
        }

        [SerializeField] private List<DialogueSO> ungroupedDialogues;
        /// <summary>
        /// List of ungrouped dialogues scriptable objects.
        /// </summary>
        public List<DialogueSO> UngroupedDialogues
        {
            get { return ungroupedDialogues; }
            set { ungroupedDialogues = value; }
        }

        public void Initialize(string filename)
        {
            GraphName = filename;
            DialogueGroups = new SerializableDictionary<DialogueGroupSO, List<DialogueSO>>();
            UngroupedDialogues = new List<DialogueSO>();
        }


        public List<DialogueSO> GetAllDialogues()
        {
            List<DialogueSO> dialogues = new List<DialogueSO>();
            foreach (DialogueGroupSO group in DialogueGroups.Keys)
            {
                foreach (DialogueSO dialogue in DialogueGroups[group])
                {
                    dialogues.Add(dialogue);
                }
            }
            foreach (DialogueSO dialogue in UngroupedDialogues)
            {
                dialogues.Add(dialogue);
            }
            return dialogues;
        }
        public List<DialogueSO> GetStartingDialogues()
        {
            List<DialogueSO> startingDialogues = new List<DialogueSO>();
            foreach(DialogueGroupSO group in DialogueGroups.Keys)
            {
                foreach(DialogueSO dialogue in DialogueGroups[group])
                {
                    if(dialogue.IsStartingDialogue == true)
                    {
                        startingDialogues.Add(dialogue);
                    }
                }
            }
            foreach(DialogueSO dialogue in UngroupedDialogues)
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
