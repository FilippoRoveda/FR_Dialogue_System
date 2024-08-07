using System.Collections.Generic;
using UnityEngine;


namespace DS.Editor.Data
{
    using Editor.Elements;
    using Editor.Events;

    /// <summary>
    /// NodeData class that hold every information for an EventNode.
    /// </summary>
    [System.Serializable]
    public class EventNodeData : DialogueNodeData
    {
        [SerializeField] protected List<GameEventSO> gameEvents;
        public List<GameEventSO> GameEvents
        {
            get { return gameEvents; }
            set { gameEvents = value; }
        }

        [SerializeField] protected VariableEvents variableEvents;
        public VariableEvents VariableEvents
        {
            get => variableEvents;
            set { variableEvents = value; }
        }
        public EventNodeData() : base()
        {
            gameEvents = new List<GameEventSO>();
            variableEvents = new VariableEvents();
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

            variableEvents = new VariableEvents();
            variableEvents.Reload(data.VariableEvents);
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

            variableEvents = new VariableEvents();
            variableEvents.Reload(node.variableEvents);
        }          
    }
}
