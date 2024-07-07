using UnityEngine;
using System.Collections.Generic;

namespace DS.Runtime.Data
{
    using Runtime.ScriptableObjects;

    /// <summary>
    /// Data structure for a dialogue choice.
    /// </summary>
    [System.Serializable]
    public class DS_DialogueChoiceData
    {
        [SerializeField] private List<LenguageData<string>> choiceTexts;
        /// <summary>
        /// Label text for the choice.
        /// </summary>
        public List<LenguageData<string>> ChoiceTexts
        { 
            get {  return choiceTexts; } 
            set {  choiceTexts = value; } 
        }

        [SerializeField] private string choiceID;
        public string ChoiceID
        {
            get { return choiceID; }
            set { choiceID = value; }
        }

        [SerializeField] private DS_DialogueSO nextDialogue;
        /// <summary>
        /// Reference the next dialogue scriptable object data.
        /// </summary>
        public DS_DialogueSO NextDialogue 
        { 
            get { return nextDialogue; } 
            set {  nextDialogue = value; } 
        }
    }
}
