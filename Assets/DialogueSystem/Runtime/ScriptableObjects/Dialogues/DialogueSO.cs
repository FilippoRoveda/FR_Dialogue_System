using UnityEngine;
using System.Collections.Generic;

namespace DS.Runtime.ScriptableObjects
{
    using Runtime.Enumerations;
    using Runtime.Data;

    /// <summary>
    /// Scriptable object conaining all dialogue node informations for saving and loading operations.
    /// </summary>
    public class DialogueSO : TextedDialogueSO
    {

        [SerializeField] protected List<DialogueChoice> _choices;

        /// <summary>
        /// The list of choices availables in this node.
        /// </summary>
        public List<DialogueChoice> Choices
        {
            get { return _choices; }
#if UNITY_EDITOR
            set { _choices = value; }
#endif
        }

#if UNITY_EDITOR
        /// <summary>
        /// Initialize the scriptble object informations.
        /// </summary>
        /// <param name="dialogueName"></param>
        /// <param name="text"></param>
        /// <param name="choices"></param>
        /// <param name="dialogueType"></param>
        /// <param name="isStartingDialogue"></param>
        public virtual void Initialize(string dialogueName, string dialogueID, DialogueType dialogueType, List<LenguageData<string>> texts, List<DialogueChoice> choices)
        {
            base.Initialize(dialogueName, dialogueID, dialogueType, texts);
            _choices = new List<DialogueChoice>(choices);
        }
#endif

    }
}
