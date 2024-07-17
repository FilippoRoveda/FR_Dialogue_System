using UnityEngine;
using System.Collections.Generic;

namespace DS.Runtime.ScriptableObjects
{
    using Data;
    using Enums;

    /// <summary>
    /// Scriptable object conaining all dialogue node informations for saving and loading operations.
    /// </summary>
    public class DialogueSO : TextedDialogueSO
    {

        [SerializeField] protected List<DialogueChoiceData> choices;

        /// <summary>
        /// The list of choices availables in this node.
        /// </summary>
        public List<DialogueChoiceData> Choices
        {
            get { return choices; }
            set { choices = value; }
        }


        /// <summary>
        /// Initialize the scriptble object informations.
        /// </summary>
        /// <param name="dialogueName"></param>
        /// <param name="text"></param>
        /// <param name="choices"></param>
        /// <param name="dialogueType"></param>
        /// <param name="isStartingDialogue"></param>
        public virtual void Initialize(string dialogueName, string dialogueID, DialogueType dialogueType, bool isStartingDialogue, List<LenguageData<string>> texts, List<DialogueChoiceData> choices)
        {
            base.Initialize(dialogueName, dialogueID, dialogueType, isStartingDialogue, texts);
            Choices = new List<DialogueChoiceData>(choices);
        }

    
    }
}
