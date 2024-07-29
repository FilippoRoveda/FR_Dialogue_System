using System.Collections.Generic;
using UnityEngine;

namespace Variables.Editor
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

        public IntegerEvent AddIntEvent(IntegerEvent condition = null)
        {
            if (condition == null)
            {
                var newCondition = new IntegerEvent();
                integerEvents.Add(newCondition);
                return newCondition;
            }
            integerEvents.Add(condition);
            return condition;
        }
        public FloatEvent AddFloatEvent(FloatEvent condition = null)
        {
            if (condition == null)
            {
                var newCondition = new FloatEvent();
                floatEvents.Add(newCondition);
                return newCondition;
            }
            floatEvents.Add(condition);
            return condition;
        }
        public BoolEvent AddBoolEvent(BoolEvent condition = null)
        {
            if (condition == null)
            {
                var newCondition = new BoolEvent();
                boolEvents.Add(newCondition);
                return newCondition;
            }
            boolEvents.Add(condition);
            return condition;
        }
        public void RemoveIntEvent(IntegerEvent condition = null) { integerEvents.Remove(condition); }
        public void RemoveFloatEvent(FloatEvent condition = null) { floatEvents.Remove(condition); }
        public void RemoveBoolEvent(BoolEvent condition = null) { boolEvents.Remove(condition); }
    }
}
