using System.Collections.Generic;
using UnityEngine;

namespace DS.Runtime.ScriptableObjects
{
    using Runtime.Utilities;

    /// <summary>
    /// ScriptableObject that represent a dialogue graph conversion.
    /// </summary>
    [System.Serializable]
    public class DialogueContainerSO : ScriptableObject
    {
        [SerializeField] private string _graphName;
        public string GraphName
        {
            get { return _graphName; }
#if UNITY_EDITOR
            set { _graphName = value; }
#endif
        }
        [SerializeField] private SerializableDictionary<DialogueGroupSO, List<BaseDialogueSO>> _dialogueGroups;

        /// <summary>
        /// Dictionary containing as key a group scriptable object and as a value a list of owned dialogue scriptable objects.
        /// </summary>
        public SerializableDictionary<DialogueGroupSO, List<BaseDialogueSO>> DialogueGroups
        {
            get { return _dialogueGroups; }
#if UNITY_EDITOR
            set { _dialogueGroups = value; }
#endif
        }

        [SerializeField] private List<BaseDialogueSO> _ungroupedDialogues;
        /// <summary>
        /// List of ungrouped dialogues scriptable objects.
        /// </summary>
        public List<BaseDialogueSO> UngroupedDialogues
        {
            get { return _ungroupedDialogues; }
#if UNITY_EDITOR
            set { _ungroupedDialogues = value; }
#endif
        }

#if UNITY_EDITOR
        public void Initialize(string filename)
        {
            GraphName = filename;
            DialogueGroups = new();
            UngroupedDialogues = new();
        }
#endif


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
                    if(dialogue.IsStartingDialogue() == true)
                    {
                        startingDialogues.Add(dialogue);
                    }
                }
            }
            foreach(BaseDialogueSO dialogue in UngroupedDialogues)
            {
                if (dialogue.IsStartingDialogue() == true)
                {
                    startingDialogues.Add(dialogue);
                }
            }
            return startingDialogues;
        }

    }
}
