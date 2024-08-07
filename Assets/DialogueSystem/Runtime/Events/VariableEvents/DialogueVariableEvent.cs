using UnityEngine;

namespace DS.Runtime.Events
{
    using VariableEnum = Variables.Generated.VariablesGenerated.VariablesKey;

    /// <summary>
    /// Type of operation that will be operated by the event.
    /// </summary>
    public enum VariableEventType
    {
        SET,
        ADD,
        MINUS
    }

    /// <summary>
    /// Generic class for variable events.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [System.Serializable]
    public class DialogueVariableEvent<T>
    {
        [SerializeField] protected VariableEnum _variableEnum;
        /// <summary>
        /// VariableEnum for the target generated dialogue variable.
        /// </summary>
        public VariableEnum VariableEnum { get { return _variableEnum; } }

        [SerializeField] private VariableEventType _eventType;
        /// <summary>
        /// The type of event, so the operations, that will be executed on the target variable.
        /// </summary>
        public VariableEventType EventType { get { return _eventType; } }

        [SerializeField] protected T _eventValue;
        /// <summary>
        /// Operating value for this event.
        /// </summary>
        public T EventValue
        {
            get { return _eventValue; }
            set
            {
                _eventValue = value;       
            }
        }

        public DialogueVariableEvent() { }
        public DialogueVariableEvent(VariableEnum variableEnum, VariableEventType eventType, T eventValue) 
        {
            this._variableEnum = variableEnum;
            this._eventType = eventType;
            this._eventValue = eventValue;
        }
    }
}
