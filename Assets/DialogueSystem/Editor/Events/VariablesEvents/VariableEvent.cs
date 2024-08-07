using UnityEngine;

namespace DS.Editor.Events
{
    using Variables.Editor;
    public enum VariableEventType
    {
        SET,
        ADD,
        MINUS
    }

    /// <summary>
    /// Abstract class that implement basic VariableEvent features.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [System.Serializable]
    public abstract class VariableEvent<T>
    {
        [SerializeField] protected T eventValue;
        /// <summary>
        /// The value with which this event work on the target Variable. 
        /// </summary>
        public T EventValue
        {
            get { return eventValue; }
            set { eventValue = value; }
        }
        [SerializeField] private VariableEventType eventType = VariableEventType.SET;
        /// <summary>
        /// The type of this event, change the way of handling the target variable.
        /// </summary>
        public virtual VariableEventType EventType
        {
            get { return eventType; }
            set
            {
                eventType = value;
            }
        }
    }

    [System.Serializable]
    public class BoolEvent : VariableEvent<bool>
    {
        [SerializeField] private BooleanVariableData variable;
        /// <summary>
        /// The target Variable for this event.
        /// </summary>
        public BooleanVariableData Variable { get => variable; set => variable = value; }
        public override VariableEventType EventType { get => base.EventType; set => base.EventType = VariableEventType.SET; }

        public BoolEvent(BooleanVariableData variable = null)
        {
            this.variable = variable;
        }
    }

    [System.Serializable]
    public class IntegerEvent : VariableEvent<int>
    {
        [SerializeField] private IntegerVariableData variable;
        /// <summary>
        /// The target Variable for this event.
        /// </summary>
        public IntegerVariableData Variable { get => variable; set => variable = value; }

        public IntegerEvent(IntegerVariableData variable = null)
        {
            this.variable = variable;
        }
    }

    [System.Serializable]
    public class FloatEvent : VariableEvent<float>
    {
        [SerializeField] private FloatVariableData variable;
        /// <summary>
        /// The target Variable for this event.
        /// </summary>
        public FloatVariableData Variable { get => variable; set => variable = value; }

        public FloatEvent(FloatVariableData variable = null)
        {
            this.variable = variable;
        }
    }
}
