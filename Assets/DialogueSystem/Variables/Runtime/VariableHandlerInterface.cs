namespace Variables.Runtime
{
    public interface IVariableHandler
    {
        void OnVariableAddedToDatabase(string id);
        void OnVariableRemovedFromDatabase(string id);
        void OnVariableNameChanged(string id, string name);
        void OnVariableValueChanged(string id);
    }
}