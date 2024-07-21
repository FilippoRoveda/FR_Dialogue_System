using UnityEngine.Events;

namespace Variables.Runtime
{
    public static class VariableEvents
    {
        /// <summary>
        /// Event called when the variable names changes.
        /// </summary>
        public static UnityEvent<string, string> VariableNameChanged = new UnityEvent<string, string>();

        /// <summary>
        /// Event calle when a varibale value changes
        /// </summary>
        public static UnityEvent<string> VariableValueChanged = new UnityEvent<string>();

#if UNITY_EDITOR
        public static UnityEvent<string> VariableAddedToDatabase = new UnityEvent<string>();
        public static UnityEvent<string> VariableRemovedFromDatabase = new UnityEvent<string>();
#endif
    }
}