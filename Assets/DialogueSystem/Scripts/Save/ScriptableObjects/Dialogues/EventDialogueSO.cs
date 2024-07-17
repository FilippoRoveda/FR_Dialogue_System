using DS.Enums;
using DS.Runtime.Data;
using System.Collections.Generic;
using UnityEngine;


namespace DS.Runtime.ScriptableObjects
{
    public class EventDialogueSO : DialogueSO
    {
        [SerializeField] private List<DS_EventSO> events;
        public List<DS_EventSO> Events
        {
            get { return events; }
            private set { events = value; }
        }

        public void SaveEvents(List<DS_EventSO> events)
        {
            if (events != null)
            {
                Events = new List<DS_EventSO>();
                foreach (var _event in events)
                {
                    Events.Add(_event);
                }
            }
            else { Events = null; }
        }
        /// <summary>
        /// Initialize the scriptble object informations.
        /// </summary>
        /// <param name="dialogueName"></param>
        /// <param name="text"></param>
        /// <param name="choices"></param>
        /// <param name="dialogueType"></param>
        /// <param name="isStartingDialogue"></param>
        public override void Initialize(string dialogueName, string dialogueID, DialogueType dialogueType, bool isStartingDialogue, List<LenguageData<string>> texts, List<DialogueChoiceData> choices)
        {
            base.Initialize(dialogueName, dialogueID, dialogueType, isStartingDialogue, texts);
            
            foreach (var _choice in choices)
            {
                Debug.Log(_choice.ChoiceTexts[0].Data);
            }
            Choices = new List<DialogueChoiceData>(choices);
        }
    }
}
