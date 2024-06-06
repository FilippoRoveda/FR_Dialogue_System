using UnityEngine;


namespace DS.Data
{
    using ScriptableObjects;

    [System.Serializable]
    public class DS_DialogueChoiceData
    {
        [SerializeField] private string text;
        public string Text 
        { 
            get {  return text; } 
            set {  text = value; } 
        }
        [SerializeField] private DS_Dialogue_SO nextDialogue;
        public DS_Dialogue_SO NextDialogue 
        { 
            get { return nextDialogue; } 
            set {  nextDialogue = value; } 
        }
    }
}
