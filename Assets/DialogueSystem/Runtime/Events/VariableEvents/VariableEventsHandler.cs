using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DS.Runtime.Events
{
    using UnityEngine.UIElements;
    using Variables.Generated;
    public class VariableEventsHandler
    {
        public bool HandleEvents(DialogueVariableEvents container)
        {
            foreach (var varEvent in container.IntEvents)
            {
                if (HandleIntEvent(varEvent) == false) return false;
                else continue;
            }
            foreach (var varEvent in container.FloatEvents)
            {
                if (HandleFloatEvent(varEvent) == false) return false;
                else continue;
            }
            foreach (var varEvent in container.BoolEvents)
            {
                if (HandleBoolEvent(varEvent) == false) return false;
                else continue;
            }

            return true;
        }
        public bool HandleIntEvent(DialogueVariableEvent<int> intEvent)
        {
            var variableID = VariablesGenerated.Instance.variableMap[intEvent.VariableEnum];
            if (variableID == null)
            {
#if UNITY_EDITOR
                Debug.LogError($"Variable ID not founded in Generated variableMap for Enum: {intEvent.VariableEnum}.");
#endif
                return false;
            }

            var variable = VariablesGenerated.Instance.intVariables[variableID];
            if (variable == null)
            {
#if UNITY_EDITOR
                Debug.LogError($"IntVariable not founded in Generated with ID: {variableID}.");
#endif
                return false;
            }

            switch (intEvent.EventType)
            {
                case VariableEventType.SET:
                    variable.SetValue(intEvent.EventValue);
                    return true;
                case VariableEventType.ADD:
                    variable.AddValue(intEvent.EventValue);
                    return true;
                case VariableEventType.MINUS:
                    variable.SubtractValue(intEvent.EventValue);
                    return true;
                default:
#if UNITY_EDITOR
                    Debug.LogError($"{intEvent.EventType} not matching available event types.");
#endif
                    return false;
            }
        }

        public bool HandleFloatEvent(DialogueVariableEvent<float> floatEvent)
        {
            var variableID = VariablesGenerated.Instance.variableMap[floatEvent.VariableEnum];
            if (variableID == null)
            {
#if UNITY_EDITOR
                Debug.LogError($"Variable ID not founded in Generated variableMap for Enum: {floatEvent.VariableEnum}.");
#endif
                return false;
            }

            var variable = VariablesGenerated.Instance.floatVariables[variableID];
            if (variable == null)
            {
#if UNITY_EDITOR
                Debug.LogError($"FloatVariable not founded in Generated with ID: {variableID}.");
#endif
                return false;
            }

            switch (floatEvent.EventType)
            {
                case VariableEventType.SET:
                    variable.SetValue(floatEvent.EventValue);
                    return true;
                case VariableEventType.ADD:
                    variable.AddValue(floatEvent.EventValue);
                    return true;
                case VariableEventType.MINUS:
                    variable.SubtractValue(floatEvent.EventValue);
                    return true;
                default:
#if UNITY_EDITOR
                    Debug.LogError($"{floatEvent.EventType} not matching available event types.");
#endif
                    return false;
            }
        }

        public bool HandleBoolEvent(DialogueVariableEvent<bool> boolEvent)
        {
            var variableID = VariablesGenerated.Instance.variableMap[boolEvent.VariableEnum];
            if (variableID == null)
            {
#if UNITY_EDITOR
                Debug.LogError($"Variable ID not founded in Generated variableMap for Enum: {boolEvent.VariableEnum}.");
#endif
                return false;
            }

            var variable = VariablesGenerated.Instance.boolVariables[variableID];
            if (variable == null)
            {
#if UNITY_EDITOR
                Debug.LogError($"BoolVariable not founded in Generated with ID: {variableID}.");
#endif
                return false;
            }

            variable.SetValue(boolEvent.EventValue);
            return true;
        }
    }
}
