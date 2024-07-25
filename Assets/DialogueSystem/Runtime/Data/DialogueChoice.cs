using UnityEngine;
using System.Collections.Generic;

namespace DS.Runtime.Data
{
    using Runtime.ScriptableObjects;

    /// <summary>
    /// Data structure for a dialogue choice.
    /// </summary>
    [System.Serializable]
    public class DialogueChoice
    {
        [SerializeField] private List<LenguageData<string>> choiceTexts;
        /// <summary>
        /// Label text for the choice.
        /// </summary>
        public List<LenguageData<string>> ChoiceTexts
        { 
            get {  return choiceTexts; }
#if UNITY_EDITOR
            set {  choiceTexts = value; }
#endif
        }

        [SerializeField] private string choiceID;
        public string ChoiceID
        {
            get { return choiceID; }
#if UNITY_EDITOR
            set { choiceID = value; }
#endif
        }


        [SerializeField] private BaseDialogueSO nextDialogue;
        /// <summary>
        /// Reference the next dialogue scriptable object data.
        /// </summary>
        public BaseDialogueSO NextDialogue 
        { 
            get { return nextDialogue; }
#if UNITY_EDITOR
            set {  nextDialogue = value; }
#endif
        }

        [SerializeField] private DialogueConditionContainer conditions;
        public DialogueConditionContainer Conditions 
        { 
            get { return conditions; }
#if UNITY_EDITOR
            set { conditions = value; }
#endif
        }
    }
}
