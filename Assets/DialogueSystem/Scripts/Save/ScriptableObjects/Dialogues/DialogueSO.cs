using UnityEngine;
using System.Collections.Generic;

namespace DS.Runtime.ScriptableObjects
{
    using Data;
    using Enums;

    /// <summary>
    /// Scriptable object conaining all dialogue node informations for saving and loading operations.
    /// </summary>
    public class DialogueSO : ScriptableObject
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



        [SerializeField] protected List<LenguageData<string>> texts;

        /// <summary>
        /// Content text for the dialogue.
        /// </summary>
        public List<LenguageData<string>> Texts
        {
            get { return texts; }
            set { texts = value; }
        }

        [SerializeField] protected List<DialogueChoiceData> choices;

        /// <summary>
        /// The list of choices availables in this node.
        /// </summary>
        public List<DialogueChoiceData> Choices
        {
            get { return choices; }
            set { choices = value; }
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
        /// <param name="text"></param>
        /// <param name="choices"></param>
        /// <param name="dialogueType"></param>
        /// <param name="isStartingDialogue"></param>
        public virtual void Initialize(string dialogueName, string dialogueID, List<LenguageData<string>> texts, List<DialogueChoiceData> choices, DialogueType dialogueType, bool isStartingDialogue)
        {
            DialogueName = dialogueName;
            this.dialogueID = dialogueID;
            Texts = texts;
            Choices = choices;
            IsStartingDialogue = isStartingDialogue;
            DialogueType = dialogueType;
        }

    
    }
}
