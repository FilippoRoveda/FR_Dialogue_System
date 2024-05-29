using UnityEngine;
using System.Collections.Generic;

namespace DS.ScriptableObjects
{
    using Data;
    using Enumerations;

    public class DS_Dialogue_SO : ScriptableObject
    {
        //Add a field for the node id?
        [SerializeField] public string DialogueName {  get; set; }
        [SerializeField] [field: TextArea] public string Text {  get; set; }
        [SerializeField] public List<DS_DialogueChoiceData> Choices {  get; set; }
        [SerializeField] public DS_DialogueType DialogueTyoe { get; set; }
        [SerializeField] public bool IsStartingDialogue { get; set; }

        public void Initialize(string dialogueName, string text, List<DS_DialogueChoiceData> choices, DS_DialogueType dialogueType, bool isStartingDialogue)
        {
            DialogueName = dialogueName;
            Text = text;
            Choices = choices;
            IsStartingDialogue = isStartingDialogue;
            DialogueTyoe = dialogueType;
        }
    }
}
