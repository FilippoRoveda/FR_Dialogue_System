using UnityEngine;

namespace DS.Data
{
    using ScriptableObjects;

    /// <summary>
    /// Data structure for a dialogue choice.
    /// </summary>
    [System.Serializable]
    public class DS_ChoiceData
    {
        [SerializeField] private string labelText;
        /// <summary>
        /// Label text for the choice.
        /// </summary>
        public string LabelText 
        { 
            get {  return labelText; } 
            set {  labelText = value; } 
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
