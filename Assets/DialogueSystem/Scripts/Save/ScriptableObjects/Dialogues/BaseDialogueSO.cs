using UnityEngine;

namespace DS.Runtime.ScriptableObjects
{
    using Enums;

    public abstract class BaseDialogueSO : ScriptableObject
    {
        [SerializeField] protected string dialogueName;
        public string DialogueName
        {
            get { return dialogueName; }
            set { dialogueName = value; }
        }

        [SerializeField] protected string dialogueID;
        public string DialogueID
        {
            get { return dialogueID; }
        }
        [SerializeField] protected DialogueType dialogueType;
        public DialogueType DialogueType
        {
            get { return dialogueType; }
            set { dialogueType = value; }
        }

        [SerializeField] protected bool isStartingDialogue;
        public bool IsStartingDialogue
        {
            get { return isStartingDialogue; }
            set { isStartingDialogue = value; }
        }
        /// <summary>
        /// Initialize the scriptble object informations.
        /// </summary>
        /// <param name="dialogueName"></param>
        /// <param name="dialogueType"></param>
        /// <param name="isStartingDialogue"></param>
        public void Initialize(string dialogueName, string dialogueID, DialogueType dialogueType, bool isStartingDialogue)
        {
            DialogueName = dialogueName;
            this.dialogueID = dialogueID;
            IsStartingDialogue = isStartingDialogue;
            DialogueType = dialogueType;
        }
    }
}
