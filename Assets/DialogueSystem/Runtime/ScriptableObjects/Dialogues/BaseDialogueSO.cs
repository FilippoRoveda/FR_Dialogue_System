using UnityEngine;

namespace DS.Runtime.ScriptableObjects
{
    using Runtime.Enumerations;
    /// <summary>
    /// Base dialogue scriptable object class with basic data and functions.
    /// </summary>
    public abstract class BaseDialogueSO : ScriptableObject
    {
        [SerializeField] protected string _dialogueName;
        public string DialogueName
        {
            get { return _dialogueName; }
#if UNITY_EDITOR
            set { _dialogueName = value; }
#endif
        }

        [SerializeField] protected string _dialogueID;
        public string DialogueID
        {
            get { return _dialogueID; }
        }
        [SerializeField] protected DialogueType _dialogueType;
        public DialogueType DialogueType
        {
            get { return _dialogueType; }
#if UNITY_EDITOR
            set { _dialogueType = value; }
#endif
        }
#if UNITY_EDITOR
        /// <summary>
        /// Initialize the scriptble object informations.
        /// </summary>
        /// <param name="dialogueName"></param>
        /// <param name="dialogueType"></param>
        /// <param name="isStartingDialogue"></param>
        public virtual void Initialize(string dialogueName, string dialogueID, DialogueType dialogueType)
        {
            _dialogueName = dialogueName;
            _dialogueID = dialogueID;
            _dialogueType = dialogueType;
        }
#endif
        public virtual bool IsStartingDialogue()
        {
            if(_dialogueType == DialogueType.Start) return true;
            return false;
        }
    }
}
