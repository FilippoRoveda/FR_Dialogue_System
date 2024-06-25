using UnityEngine;
using System.Collections.Generic;

namespace DS.ScriptableObjects
{
    using Data;
    using Enumerations;

    /// <summary>
    /// Scriptable object conaining all dialogue node informations for saving and loading operations.
    /// </summary>
    public class DS_DialogueSO : ScriptableObject
    {
        //Add a field for the node id?
        [SerializeField] private string dialogueName;
        public string DialogueName
        {
            get { return dialogueName; }
            set { dialogueName = value; }
        }

        [SerializeField][field: TextArea] private string text;

        /// <summary>
        /// Content text for the dialogue.
        /// </summary>
        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        [SerializeField] private List<DS_DialogueChoiceData> choices;

        /// <summary>
        /// The list of choices availables in this node.
        /// </summary>
        public List<DS_DialogueChoiceData> Choices
        {
            get { return choices; }
            set { choices = value; }
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
            get { return isStartingDialogue; }
            set { isStartingDialogue = value; }
        }

        [SerializeField] private List<DS_DialogueEventSO> events;
        public List<DS_DialogueEventSO> Events
        {
            get { return events; }
            set { events = value; }
        }
        /// <summary>
        /// Initialize the scriptble object informations.
        /// </summary>
        /// <param name="dialogueName"></param>
        /// <param name="text"></param>
        /// <param name="choices"></param>
        /// <param name="dialogueType"></param>
        /// <param name="isStartingDialogue"></param>
        public void Initialize(string dialogueName, string text, List<DS_DialogueChoiceData> choices, DS_DialogueType dialogueType, bool isStartingDialogue, List<DS_DialogueEventSO> events = null)
        {
            DialogueName = dialogueName;
            Text = text;
            Choices = choices;
            IsStartingDialogue = isStartingDialogue;
            DialogueType = dialogueType;
            if (events != null)
            {
                SaveEvents(events);
            }
        }

        public void SaveEvents(List<DS_DialogueEventSO> events)
        {
            Events = new List<DS_DialogueEventSO>();
            foreach (var _event in events)
            {
                Events.Add(_event);
            }
        }      
    }
}
