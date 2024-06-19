using UnityEngine;

namespace DS.Data
{
    using ScriptableObjects;

    /// <summary>
    /// Data structure for a dialogue choice.
    /// </summary>
    [System.Serializable]
    public class DS_DialogueChoiceData
    {
        [SerializeField] private string choiceText;
        /// <summary>
        /// Label text for the choice.
        /// </summary>
        public string ChoiceText 
        { 
            get {  return choiceText; } 
            set {  choiceText = value; } 
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
