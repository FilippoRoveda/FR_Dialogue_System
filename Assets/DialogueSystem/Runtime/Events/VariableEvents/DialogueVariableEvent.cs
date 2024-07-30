using UnityEngine;

namespace DS.Runtime.Events
{
    using VariableEnum = Variables.Generated.VariablesGenerated.VariablesKey;

    public enum VariableEventType
    {
        SET,
        ADD,
        MINUS
    }

    [System.Serializable]
    public class DialogueVariableEvent<T>
    {
        [SerializeField] protected VariableEnum variableEnum;
        public VariableEnum VariableEnum { get { return variableEnum; } }

        [SerializeField] private VariableEventType eventType;
        public VariableEventType EventType { get { return eventType; } }

        [SerializeField] protected T eventValue;
        public T EventValue
        {
            get { return eventValue; }
            set
            {
                eventValue = value;       
            }
        }

        public DialogueVariableEvent() { }
        public DialogueVariableEvent(VariableEnum variableEnum, VariableEventType eventType, T eventValue) 
        {
            this.variableEnum = variableEnum;
            this.eventType = eventType;
            this.eventValue = eventValue;
        }
    }
}
