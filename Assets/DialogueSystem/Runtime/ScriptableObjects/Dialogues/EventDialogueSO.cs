using System.Collections.Generic;
using UnityEngine;


namespace DS.Runtime.ScriptableObjects
{
    using DS.Runtime.Events;
    using Runtime.Data;
    using Runtime.Enumerations;

    /// <summary>
    /// Dialogue scriptable object for event dialogues.
    /// </summary>
    public class EventDialogueSO : DialogueSO
    {
        [SerializeField] private List<GameEvent> _events;
        public List<GameEvent> Events
        {
            get { return _events; }
            private set { _events = value; }
        }

        [SerializeField] private DialogueVariableEvents _variableEvents;
        public DialogueVariableEvents VariableEvents
        {
            get => _variableEvents;
            set { _variableEvents = value; }
        }

        public void SetGameEvents(List<GameEvent> events)
        {
            if (events != null)
            {
                Events = new List<GameEvent>();
                foreach (var _event in events)
                {
                    Events.Add(_event);
                }
            }
            else { Events = null; }
        }
        public void SetVariableEvents(DialogueVariableEvents eventsContainer)
        {
            _variableEvents = new();
            _variableEvents.Reload(eventsContainer);
        }

        /// <summary>
        /// Initialize the scriptble object informations.
        /// </summary>
        /// <param name="dialogueName"></param>
        /// <param name="text"></param>
        /// <param name="choices"></param>
        /// <param name="dialogueType"></param>
        /// <param name="isStartingDialogue"></param>
        public override void Initialize(string dialogueName, string dialogueID, DialogueType dialogueType, List<LenguageData<string>> texts, List<DialogueChoice> choices)
        {
            base.Initialize(dialogueName, dialogueID, dialogueType, texts);      
            Choices = new List<DialogueChoice>(choices);
        }
    }
}
