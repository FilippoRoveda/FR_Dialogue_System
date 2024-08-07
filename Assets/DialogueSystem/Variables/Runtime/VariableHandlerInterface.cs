namespace Variables.Runtime
{
    public interface IVariableHandler
    {
#if UNITY_EDITOR
        void OnVariableAddedToDatabase(string id);
        void OnVariableRemovedFromDatabase(string id);
#endif
        void OnVariableNameChanged(string id, string name);
        void OnVariableValueChanged(string id);
    }
}