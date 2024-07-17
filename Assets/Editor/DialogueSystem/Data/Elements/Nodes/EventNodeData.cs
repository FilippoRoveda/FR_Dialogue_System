
using System.Collections.Generic;
using UnityEngine;


namespace DS.Editor.Data
{
    using DS.Editor.Elements;
    using DS.Runtime.ScriptableObjects;

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
        public EventNodeData(EventNode node) : base(node)
        {
            events = new List<DS_EventSO>();
            if (node.Data.Events != null && node.Data.Events.Count > 0)
            {
                foreach (var _event in node.Data.Events)
                {
                    events.Add(_event);
                }
            }
            else events = null;
        }          
    }
}
