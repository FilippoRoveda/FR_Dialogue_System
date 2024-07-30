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

    [System.Serializable]
    public class VariableEvent<T>
    {
        [SerializeField] protected T eventValue;
        public T EventValue
        {
            get { return eventValue; }
            set { eventValue = value; }
        }
        [SerializeField] private VariableEventType eventType = VariableEventType.SET;
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
        public FloatVariableData Variable { get => variable; set => variable = value; }

        public FloatEvent(FloatVariableData variable = null)
        {
            this.variable = variable;
        }
    }
}
