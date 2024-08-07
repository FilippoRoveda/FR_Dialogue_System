using System.Collections.Generic;
using UnityEngine;

namespace DS.Runtime.Events
{
    [System.Serializable]
    public class DialogueVariableEvents
    {
        [SerializeField] private List<DialogueVariableEvent<int>> integerEvents;
        [SerializeField] private List<DialogueVariableEvent<float>> floatEvents;
        [SerializeField] private List<DialogueVariableEvent<bool>> boolEvents;

        public List<DialogueVariableEvent<int>> IntEvents { get { return integerEvents; } }
        public List<DialogueVariableEvent<float>> FloatEvents { get { return floatEvents; } }
        public List<DialogueVariableEvent<bool>> BoolEvents { get { return boolEvents; } }
        public DialogueVariableEvents()
        {
            Initialize();
        }

        public void Initialize()
        {
            integerEvents = new List<DialogueVariableEvent<int>>();
            floatEvents = new List<DialogueVariableEvent<float>>();
            boolEvents = new List<DialogueVariableEvent<bool>>();
        }
        public void Reload(DialogueVariableEvents dialogueVariableEvents)
        {
            integerEvents = new List<DialogueVariableEvent<int>>(dialogueVariableEvents.IntEvents);
            floatEvents = new List<DialogueVariableEvent<float>>(dialogueVariableEvents.FloatEvents);
            boolEvents = new List<DialogueVariableEvent<bool>>(dialogueVariableEvents.BoolEvents);
        }

        public DialogueVariableEvent<int> AddIntEvent(DialogueVariableEvent<int> _event = null)
        {
            if (_event == null)
            {
                var newCondition = new DialogueVariableEvent<int>();
                integerEvents.Add(newCondition);
                return newCondition;
            }
            integerEvents.Add(_event);
            return _event;
        }
        public DialogueVariableEvent<float> AddFloatEvent(DialogueVariableEvent<float> _event = null)
        {
            if (_event == null)
            {
                var newCondition = new DialogueVariableEvent<float>();
                floatEvents.Add(newCondition);
                return newCondition;
            }
            floatEvents.Add(_event);
            return _event;
        }
        public DialogueVariableEvent<bool> AddBoolEvent(DialogueVariableEvent<bool> _event = null)
        {
            if (_event == null)
            {
                var newCondition = new DialogueVariableEvent<bool>();
                boolEvents.Add(newCondition);
                return newCondition;
            }
            boolEvents.Add(_event);
            return _event;
        }
        public void RemoveIntEvent(DialogueVariableEvent<int> _event = null) { integerEvents.Remove(_event); }
        public void RemoveFloatEvent(DialogueVariableEvent<float> _event = null) { floatEvents.Remove(_event); }
        public void RemoveBoolEvent(DialogueVariableEvent<bool> _event = null) { boolEvents.Remove(_event); }
    }
}
