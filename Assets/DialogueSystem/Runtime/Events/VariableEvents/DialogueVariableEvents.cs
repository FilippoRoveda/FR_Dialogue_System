using System.Collections.Generic;
using UnityEngine;

namespace DS.Runtime.Events
{
    [System.Serializable]
    public class DialogueVariableEvents
    {
        [SerializeField] private List<DialogueVariableEvent<int>> _integerEvents;
        [SerializeField] private List<DialogueVariableEvent<float>> _floatEvents;
        [SerializeField] private List<DialogueVariableEvent<bool>> _boolEvents;

        public List<DialogueVariableEvent<int>> IntEvents { get { return _integerEvents; } }
        public List<DialogueVariableEvent<float>> FloatEvents { get { return _floatEvents; } }
        public List<DialogueVariableEvent<bool>> BoolEvents { get { return _boolEvents; } }
        public DialogueVariableEvents()
        {
            Initialize();
        }

        public void Initialize()
        {
            _integerEvents = new List<DialogueVariableEvent<int>>();
            _floatEvents = new List<DialogueVariableEvent<float>>();
            _boolEvents = new List<DialogueVariableEvent<bool>>();
        }
        public void Reload(DialogueVariableEvents dialogueVariableEvents)
        {
            _integerEvents = new List<DialogueVariableEvent<int>>(dialogueVariableEvents.IntEvents);
            _floatEvents = new List<DialogueVariableEvent<float>>(dialogueVariableEvents.FloatEvents);
            _boolEvents = new List<DialogueVariableEvent<bool>>(dialogueVariableEvents.BoolEvents);
        }

        public DialogueVariableEvent<int> AddIntEvent(DialogueVariableEvent<int> _event = null)
        {
            if (_event == null)
            {
                var newCondition = new DialogueVariableEvent<int>();
                _integerEvents.Add(newCondition);
                return newCondition;
            }
            _integerEvents.Add(_event);
            return _event;
        }
        public DialogueVariableEvent<float> AddFloatEvent(DialogueVariableEvent<float> _event = null)
        {
            if (_event == null)
            {
                var newCondition = new DialogueVariableEvent<float>();
                _floatEvents.Add(newCondition);
                return newCondition;
            }
            _floatEvents.Add(_event);
            return _event;
        }
        public DialogueVariableEvent<bool> AddBoolEvent(DialogueVariableEvent<bool> _event = null)
        {
            if (_event == null)
            {
                var newCondition = new DialogueVariableEvent<bool>();
                _boolEvents.Add(newCondition);
                return newCondition;
            }
            _boolEvents.Add(_event);
            return _event;
        }
        public void RemoveIntEvent(DialogueVariableEvent<int> _event = null) { _integerEvents.Remove(_event); }
        public void RemoveFloatEvent(DialogueVariableEvent<float> _event = null) { _floatEvents.Remove(_event); }
        public void RemoveBoolEvent(DialogueVariableEvent<bool> _event = null) { _boolEvents.Remove(_event); }
    }
}
