using System.Collections.Generic;
using UnityEngine;


namespace DS.Editor.Data
{
    using DS.Editor.Elements;
    using DS.Editor.ScriptableObjects;
    using Variables.Editor;

    [System.Serializable]
    public class EventNodeData : DialogueNodeData
    {
        [SerializeField] protected List<GameEventSO> events;
        public List<GameEventSO> Events
        {
            get { return events; }
            set { events = value; }
        }

        [SerializeField] protected VariableEventsContainer eventsContainer;
        public VariableEventsContainer EventsContainer
        {
            get => eventsContainer;
            set { eventsContainer = value; }
        }
        public EventNodeData() : base()
        {
            events = new List<GameEventSO>();
            eventsContainer = new VariableEventsContainer();
        }
        public EventNodeData(EventNodeData data) : base(data)
        {
            events = new List<GameEventSO>();
            if (data.Events != null && data.Events.Count > 0)
            {
                foreach (var _event in data.Events)
                {
                    events.Add(_event);
                }
            }
            else events = null;

            eventsContainer = new VariableEventsContainer();
            eventsContainer.Reload(data.EventsContainer);
        }

        public EventNodeData(EventNode node) : base(node)
        { 
            events = new List<GameEventSO>();
            if (node.gameEvents != null && node.gameEvents.Count > 0)
            {
                foreach (var _event in node.gameEvents)
                {
                    events.Add(_event);
                }
            }
            else events = null;

            eventsContainer = new VariableEventsContainer();
            eventsContainer.Reload(node.variableEvents);
        }          
    }
}
