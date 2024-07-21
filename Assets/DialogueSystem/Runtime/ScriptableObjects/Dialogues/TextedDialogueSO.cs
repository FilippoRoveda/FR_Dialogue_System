using System.Collections.Generic;
using UnityEngine;

namespace DS.Runtime.ScriptableObjects
{
    using Runtime.Enumerations;
    using Runtime.Data;

    public abstract class TextedDialogueSO : BaseDialogueSO
    {
        [SerializeField] protected List<LenguageData<string>> _texts;

        /// <summary>
        /// Content text for the dialogue.
        /// </summary>
        public List<LenguageData<string>> Texts
        {
            get { return _texts; }
#if UNITY_EDITOR
            set { _texts = value; }
#endif
        }

#if UNITY_EDITOR
        public void Initialize(string dialogueName, string dialogueID, DialogueType dialogueType, List<LenguageData<string>> texts)
        {
            base.Initialize(dialogueName, dialogueID, dialogueType);
            _texts = new List<LenguageData<string>>(texts);
        }
#endif
    }
}
