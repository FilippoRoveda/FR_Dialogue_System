using UnityEngine;
using System.Collections.Generic;

namespace DS.Runtime.Data
{
    using Runtime.ScriptableObjects;
    using Runtime.Conditions;

    /// <summary>
    /// Data structure for a dialogue choice.
    /// </summary>
    [System.Serializable]
    public class DialogueChoice
    {
        [SerializeField] private List<LenguageData<string>> _choiceTexts;
        /// <summary>
        /// Label text for the choice.
        /// </summary>
        public List<LenguageData<string>> ChoiceTexts
        { 
            get {  return _choiceTexts; }
#if UNITY_EDITOR
            set {  _choiceTexts = value; }
#endif
        }

        [SerializeField] private string _choiceID;
        public string ChoiceID
        {
            get { return _choiceID; }
#if UNITY_EDITOR
            set { _choiceID = value; }
#endif
        }


        [SerializeField] private BaseDialogueSO _nextDialogue;
        /// <summary>
        /// Reference the next dialogue scriptable object data.
        /// </summary>
        public BaseDialogueSO NextDialogue 
        { 
            get { return _nextDialogue; }
#if UNITY_EDITOR
            set {  _nextDialogue = value; }
#endif
        }

        [SerializeField] private DialogueConditions _conditions;
        public DialogueConditions Conditions 
        { 
            get { return _conditions; }
#if UNITY_EDITOR
            set { _conditions = value; }
#endif
        }
    }
}
