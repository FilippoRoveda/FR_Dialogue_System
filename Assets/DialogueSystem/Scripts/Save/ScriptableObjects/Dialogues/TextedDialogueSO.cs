using System.Collections.Generic;
using UnityEngine;

namespace DS.Runtime.ScriptableObjects
{
    using Data;
    using DS.Enums;

    public abstract class TextedDialogueSO : BaseDialogueSO
    {
        [SerializeField] protected List<LenguageData<string>> texts;

        /// <summary>
        /// Content text for the dialogue.
        /// </summary>
        public List<LenguageData<string>> Texts
        {
            get { return texts; }
            set { texts = value; }
        }

        public void Initialize(string dialogueName, string dialogueID, DialogueType dialogueType, bool isStartingDialogue, List<LenguageData<string>> texts)
        {
            base.Initialize(dialogueName, dialogueID, dialogueType, isStartingDialogue);
            Texts = texts;
        }
    }
}
