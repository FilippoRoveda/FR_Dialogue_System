using UnityEngine;

namespace DS.Runtime.Events
{
    using Variables.Generated;
    using Variables.Runtime;

    public class VariableEventsExecuter
    {
        /// <summary>
        /// Execute every event contained in a variable events container.
        /// </summary>
        /// <param name="container"></param>
        /// <returns></returns>
        public bool ExecuteEvents(DialogueVariableEvents container)
        {
            foreach (var varEvent in container.IntEvents)
            {
                if (ExecuteEvent<int>(varEvent) == false) return false;
                else continue;
            }
            foreach (var varEvent in container.FloatEvents)
            {
                if (ExecuteEvent<float>(varEvent) == false) return false;
                else continue;
            }
            foreach (var varEvent in container.BoolEvents)
            {
                if (ExecuteEvent<bool>(varEvent) == false) return false;
                else continue;
            }

            return true;
        }

        /// <summary>
        /// Execute a single variable event.
        /// </summary>
        /// <typeparam name="T">Type of variable the event targets.</typeparam>
        /// <param name="varEvent">The variable event itself.</param>
        /// <returns></returns>
        public bool ExecuteEvent<T>(DialogueVariableEvent<T> varEvent) where T : struct
        {
            string variableID = VariablesGenerated.Instance.variableMap[varEvent.VariableEnum];
            if (variableID == null)
            {
#if UNITY_EDITOR
                Debug.LogError($"Variable ID not founded in Generated variableMap for Enum: {varEvent.VariableEnum}.");
#endif
                return false;
            }

            Variable<T> variableGeneric = null;
            if (typeof(T) == typeof(int))
            {
                var variable = VariablesGenerated.Instance.intVariables[variableID];
                variableGeneric = variable as Variable<T>;
            }
            else if (typeof(T) == typeof(float)) 
            {
                var variable = VariablesGenerated.Instance.floatVariables[variableID];
                variableGeneric = variable as Variable<T>;
            }
            else if (typeof(T) == typeof(bool))
            {
                var variable = VariablesGenerated.Instance.boolVariables[variableID];
                variableGeneric = variable as Variable<T>;
            }
            if (variableGeneric == null)
            {
#if UNITY_EDITOR
                Debug.LogError($"Variable not founded in Generated with ID: {variableID}.");
#endif
                return false;
            }

            switch (varEvent.EventType)
            {
                case VariableEventType.SET:
                    variableGeneric.SetValue(varEvent.EventValue);
                    return true;
                case VariableEventType.ADD:
                    if(typeof(T) == typeof(int)) (variableGeneric as IntVariable).AddValue(int.Parse(varEvent.EventValue.ToString()));
                    else if(typeof(T) == typeof(float)) (variableGeneric as FloatVariable).AddValue(float.Parse(varEvent.EventValue.ToString()));

                    return true;
                case VariableEventType.MINUS:
                    if (typeof(T) == typeof(int)) (variableGeneric as IntVariable).SubtractValue(int.Parse(varEvent.EventValue.ToString()));
                    else if (typeof(T) == typeof(float)) (variableGeneric as FloatVariable).SubtractValue(float.Parse(varEvent.EventValue.ToString()));
                    return true;
                default:
#if UNITY_EDITOR
                    Debug.LogError($"{varEvent.EventType} not matching available event types.");
#endif
                    return false;
            }
        }

//        public bool HandleIntEvent(DialogueVariableEvent<int> intEvent)
//        {
//            var variableID = VariablesGenerated.Instance.variableMap[intEvent.VariableEnum];
//            if (variableID == null)
//            {
//#if UNITY_EDITOR
//                Debug.LogError($"Variable ID not founded in Generated variableMap for Enum: {intEvent.VariableEnum}.");
//#endif
//                return false;
//            }

//            var variable = VariablesGenerated.Instance.intVariables[variableID];
//            if (variable == null)
//            {
//#if UNITY_EDITOR
//                Debug.LogError($"IntVariable not founded in Generated with ID: {variableID}.");
//#endif
//                return false;
//            }

//            switch (intEvent.EventType)
//            {
//                case VariableEventType.SET:
//                    variable.SetValue(intEvent.EventValue);
//                    return true;
//                case VariableEventType.ADD:
//                    variable.AddValue(intEvent.EventValue);
//                    return true;
//                case VariableEventType.MINUS:
//                    variable.SubtractValue(intEvent.EventValue);
//                    return true;
//                default:
//#if UNITY_EDITOR
//                    Debug.LogError($"{intEvent.EventType} not matching available event types.");
//#endif
//                    return false;
//            }
//        }

//        public bool HandleFloatEvent(DialogueVariableEvent<float> floatEvent)
//        {
//            var variableID = VariablesGenerated.Instance.variableMap[floatEvent.VariableEnum];
//            if (variableID == null)
//            {
//#if UNITY_EDITOR
//                Debug.LogError($"Variable ID not founded in Generated variableMap for Enum: {floatEvent.VariableEnum}.");
//#endif
//                return false;
//            }

//            var variable = VariablesGenerated.Instance.floatVariables[variableID];
//            if (variable == null)
//            {
//#if UNITY_EDITOR
//                Debug.LogError($"FloatVariable not founded in Generated with ID: {variableID}.");
//#endif
//                return false;
//            }

//            switch (floatEvent.EventType)
//            {
//                case VariableEventType.SET:
//                    variable.SetValue(floatEvent.EventValue);
//                    return true;
//                case VariableEventType.ADD:
//                    variable.AddValue(floatEvent.EventValue);
//                    return true;
//                case VariableEventType.MINUS:
//                    variable.SubtractValue(floatEvent.EventValue);
//                    return true;
//                default:
//#if UNITY_EDITOR
//                    Debug.LogError($"{floatEvent.EventType} not matching available event types.");
//#endif
//                    return false;
//            }
//        }

//        public bool HandleBoolEvent(DialogueVariableEvent<bool> boolEvent)
//        {
//            var variableID = VariablesGenerated.Instance.variableMap[boolEvent.VariableEnum];
//            if (variableID == null)
//            {
//#if UNITY_EDITOR
//                Debug.LogError($"Variable ID not founded in Generated variableMap for Enum: {boolEvent.VariableEnum}.");
//#endif
//                return false;
//            }

//            var variable = VariablesGenerated.Instance.boolVariables[variableID];
//            if (variable == null)
//            {
//#if UNITY_EDITOR
//                Debug.LogError($"BoolVariable not founded in Generated with ID: {variableID}.");
//#endif
//                return false;
//            }

//            variable.SetValue(boolEvent.EventValue);
//            return true;
//        }
    }
}
