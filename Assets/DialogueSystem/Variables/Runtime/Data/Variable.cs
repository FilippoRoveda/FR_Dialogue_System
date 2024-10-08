using UnityEngine;

namespace Variables.Runtime
{
    [System.Serializable]
    public abstract class Variable<T>
    {
        [SerializeField] protected string _name;
        public string Name { get { return _name; } }
        [SerializeField] protected string _id;
        public string Id { get { return _id; } }
        [SerializeField] protected T _value;
        public T Value { get { return _value; } }

        public Variable(string name, string id, T value)
        {
            _name = name;
            _id = id;
            _value = value;
        }
        public void SetValue(T newValue)
        {
            _value = newValue;
            VariableEvents.VariableValueChanged?.Invoke(_id);
        }
        public void SetName(string newName)
        {
            _name = newName;
            VariableEvents.VariableNameChanged?.Invoke(_id, _name);
        }
    }


    [System.Serializable]
    public class IntVariable : Variable<int>
    {
        public IntVariable(string name, string id, int value) : base(name, id, value)
        {
        }
        public void AddValue(int addValue)
        {
            _value += addValue;
            VariableEvents.VariableValueChanged?.Invoke(_id);
        }
        public void SubtractValue(int subtractingValue)
        {
            _value -= subtractingValue;
            VariableEvents.VariableValueChanged?.Invoke(_id);
        }
    }
    [System.Serializable]
    public class FloatVariable : Variable<float>
    {
        public FloatVariable(string name, string id, float value) : base(name, id, value)
        {
        }
        public void AddValue(float addValue)
        {
            _value += addValue;
            VariableEvents.VariableValueChanged?.Invoke(_id);
        }
        public void SubtractValue(float subtractingValue)
        {
            _value -= subtractingValue;
            VariableEvents.VariableValueChanged?.Invoke(_id);
        }
    }
    [System.Serializable]
    public class BoolVariable : Variable<bool>
    {
        public BoolVariable(string name, string id, bool value) : base(name, id, value)
        {
        }
    }

}
