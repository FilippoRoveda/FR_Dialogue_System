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
        [SerializeField] private SerializableDictionary<DialogueGroupSO, List<BaseDialogueSO>> dialogueGroups;

        /// <summary>
        /// Dictionary containing as key a group scriptable object and as a value a list of owned dialogue scriptable objects.
        /// </summary>
        public SerializableDictionary<DialogueGroupSO, List<BaseDialogueSO>> DialogueGroups
        {
            get { return dialogueGroups; }
            set { dialogueGroups = value; }
        }

        [SerializeField] private List<BaseDialogueSO> ungroupedDialogues;
        /// <summary>
        /// List of ungrouped dialogues scriptable objects.
        /// </summary>
        public List<BaseDialogueSO> UngroupedDialogues
        {
            get { return ungroupedDialogues; }
            set { ungroupedDialogues = value; }
        }

        public void Initialize(string filename)
        {
            GraphName = filename;
            DialogueGroups = new SerializableDictionary<DialogueGroupSO, List<BaseDialogueSO>>();
            UngroupedDialogues = new List<BaseDialogueSO>();
        }


        public List<BaseDialogueSO> GetAllDialogues()
        {
            List<BaseDialogueSO> dialogues = new List<BaseDialogueSO>();
            foreach (DialogueGroupSO group in DialogueGroups.Keys)
            {
                foreach (BaseDialogueSO dialogue in DialogueGroups[group])
                {
                    dialogues.Add(dialogue);
                }
            }
            foreach (BaseDialogueSO dialogue in UngroupedDialogues)
            {
                dialogues.Add(dialogue);
            }
            return dialogues;
        }
        public List<BaseDialogueSO> GetStartingDialogues()
        {
            List<BaseDialogueSO> startingDialogues = new List<BaseDialogueSO>();
            foreach(DialogueGroupSO group in DialogueGroups.Keys)
            {
                foreach(BaseDialogueSO dialogue in DialogueGroups[group])
                {
                    if(dialogue.IsStartingDialogue == true)
                    {
                        startingDialogues.Add(dialogue);
                    }
                }
            }
            foreach(BaseDialogueSO dialogue in UngroupedDialogues)
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
