using UnityEngine;
using System.Collections.Generic;

namespace DS.ScriptableObjects
{
    using Data;
    using Enumerations;

    public class DS_Dialogue_SO : ScriptableObject
    {
        //Add a field for the node id?
        [SerializeField] private string dialogueName;
        public string DialogueName 
        {  
            get {  return dialogueName; } 
            set {  dialogueName = value; } 
        }

        [SerializeField][field: TextArea] private string text;
        public string Text 
        {  
            get { return text; } 
            set {  text = value; } 
        }

        [SerializeField] private List<DS_DialogueChoiceData> choices;
        public List<DS_DialogueChoiceData> Choices 
        {  
            get { return choices; } 
            set {  choices = value; } 
        }

        [SerializeField] private DS_DialogueType dialogueType;
        public DS_DialogueType DialogueType 
        {
            get { return dialogueType; }
            set { dialogueType = value; }
        }

        [SerializeField] private bool isStartingDialogue;
        public bool IsStartingDialogue 
        { 
            get { return isStartingDialogue;}
            set {  isStartingDialogue = value;} 
        }

        public void Initialize(string dialogueName, string text, List<DS_DialogueChoiceData> choices, DS_DialogueType dialogueType, bool isStartingDialogue)
        {
            DialogueName = dialogueName;
            Text = text;
            Choices = choices;
            IsStartingDialogue = isStartingDialogue;
            DialogueType = dialogueType;
        }
    }
}
