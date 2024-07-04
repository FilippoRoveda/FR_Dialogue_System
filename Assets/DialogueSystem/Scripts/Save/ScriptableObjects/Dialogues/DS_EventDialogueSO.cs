using System.Collections.Generic;
using UnityEngine;


namespace DS.Runtime.ScriptableObjects
{
    public class DS_EventDialogueSO : DS_DialogueSO
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
    }
}
