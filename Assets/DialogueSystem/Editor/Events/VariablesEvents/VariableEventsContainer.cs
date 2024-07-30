using System.Collections.Generic;
using UnityEngine;

namespace DS.Editor.Events
{
    [System.Serializable]
    public class VariableEventsContainer
    {
        [SerializeField] private List<IntegerEvent> integerEvents;
        [SerializeField] private List<FloatEvent> floatEvents;
        [SerializeField] private List<BoolEvent> boolEvents;

        public List<IntegerEvent> IntEvents { get { return integerEvents; } }
        public List<FloatEvent> FloatEvents { get { return floatEvents; } }
        public List<BoolEvent> BoolEvents { get { return boolEvents; } }
        public VariableEventsContainer()
        {
            Initialize();
        }

        public void Initialize()
        {
            integerEvents = new List<IntegerEvent>();
            floatEvents = new List<FloatEvent>();
            boolEvents = new List<BoolEvent>();
        }
        public void Reload(VariableEventsContainer conditionsContainer)
        {
            integerEvents = new List<IntegerEvent>(conditionsContainer.IntEvents);
            floatEvents = new List<FloatEvent>(conditionsContainer.FloatEvents);
            boolEvents = new List<BoolEvent>(conditionsContainer.BoolEvents);
        }

        public IntegerEvent AddIntEvent(IntegerEvent _event = null)
        {
            if (_event == null)
            {
                var newCondition = new IntegerEvent();
                integerEvents.Add(newCondition);
                return newCondition;
            }
            integerEvents.Add(_event);
            return _event;
        }
        public FloatEvent AddFloatEvent(FloatEvent _event = null)
        {
            if (_event == null)
            {
                var newCondition = new FloatEvent();
                floatEvents.Add(newCondition);
                return newCondition;
            }
            floatEvents.Add(_event);
            return _event;
        }
        public BoolEvent AddBoolEvent(BoolEvent _event = null)
        {
            if (_event == null)
            {
                var newCondition = new BoolEvent();
                boolEvents.Add(newCondition);
                return newCondition;
            }
            boolEvents.Add(_event);
            return _event;
        }
        public void RemoveIntEvent(IntegerEvent _event = null) { integerEvents.Remove(_event); }
        public void RemoveFloatEvent(FloatEvent _event = null) { floatEvents.Remove(_event); }
        public void RemoveBoolEvent(BoolEvent _event = null) { boolEvents.Remove(_event); }
    }
}
