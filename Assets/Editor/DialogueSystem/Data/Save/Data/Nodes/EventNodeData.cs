
using System.Collections.Generic;
using UnityEngine;


namespace DS.Editor.Data
{
    using DS.Enums;
    using DS.Runtime.ScriptableObjects;
    using Runtime.Data;

    [System.Serializable]
    public class EventNodeData : DialogueNodeData
    {
        [SerializeField] protected List<DS_EventSO> events;
        public List<DS_EventSO> Events
        {
            get { return events; }
            set { events = value; }
        }
        public EventNodeData(string _nodeID, string _dialogueName, List<ChoiceData> _choices,
                                List<LenguageData<string>> _texts, DialogueType _dialogueType,
                                string _groupID, Vector2 _position, List<DS_EventSO> _events = null) 
                                : base(_nodeID, _dialogueName,_choices, _texts, _dialogueType, _groupID, _position)
        {
            events = new List<DS_EventSO>();
            if (_events != null && _events.Count > 0)
            {
                foreach (var _event in _events)
                {
                    events.Add(_event);
                }
            }
            else events = null;
        }          
    }
}
