using System.Collections.Generic;
using UnityEngine;


namespace DS.Editor.Data
{
    using Editor.Elements;
    using Editor.Events;

    [System.Serializable]
    public class EventNodeData : DialogueNodeData
    {
        [SerializeField] protected List<GameEventSO> gameEvents;
        public List<GameEventSO> GameEvents
        {
            get { return gameEvents; }
            set { gameEvents = value; }
        }

        [SerializeField] protected VariableEventsContainer variableEventsContainer;
        public VariableEventsContainer VariableEventsContainer
        {
            get => variableEventsContainer;
            set { variableEventsContainer = value; }
        }
        public EventNodeData() : base()
        {
            gameEvents = new List<GameEventSO>();
            variableEventsContainer = new VariableEventsContainer();
        }
        public EventNodeData(EventNodeData data) : base(data)
        {
            gameEvents = new List<GameEventSO>();
            if (data.GameEvents != null && data.GameEvents.Count > 0)
            {
                foreach (var _event in data.GameEvents)
                {
                    gameEvents.Add(_event);
                }
            }
            else gameEvents = null;

            variableEventsContainer = new VariableEventsContainer();
            variableEventsContainer.Reload(data.VariableEventsContainer);
        }

        public EventNodeData(EventNode node) : base(node)
        { 
            gameEvents = new List<GameEventSO>();
            if (node.gameEvents != null && node.gameEvents.Count > 0)
            {
                foreach (var _event in node.gameEvents)
                {
                    gameEvents.Add(_event);
                }
            }
            else gameEvents = null;

            variableEventsContainer = new VariableEventsContainer();
            variableEventsContainer.Reload(node.variableEvents);
        }          
    }
}
