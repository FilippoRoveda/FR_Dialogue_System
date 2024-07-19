using System.Collections.Generic;
using UnityEngine;


namespace DS.Editor.Data
{
    using DS.Editor.Elements;
    using DS.Editor.ScriptableObjects;

    [System.Serializable]
    public class EventNodeData : DialogueNodeData
    {
        [SerializeField] protected List<DS_EventSO> events;
        public List<DS_EventSO> Events
        {
            get { return events; }
            set { events = value; }
        }
        public EventNodeData() : base()
        {
            events = new List<DS_EventSO>();
        }
        public EventNodeData(EventNodeData data) : base(data)
        {
            events = new List<DS_EventSO>();
            if (data.Events != null && data.Events.Count > 0)
            {
                foreach (var _event in data.Events)
                {
                    events.Add(_event);
                }
            }
            else events = null;
        }

        public EventNodeData(EventNode node) : base(node)
        { 
            events = new List<DS_EventSO>();
            if (node._events != null && node._events.Count > 0)
            {
                foreach (var _event in node._events)
                {
                    events.Add(_event);
                }
            }
            else events = null;
        }          
    }
}
