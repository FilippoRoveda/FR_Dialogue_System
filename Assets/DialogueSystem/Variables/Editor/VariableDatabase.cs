using UnityEngine;
using System;
using System.Collections.Generic;


namespace Variables.Editor
{
    using Variables.Runtime;
    public class VariablesDatabase : ScriptableObject
    {
        IOUtilities IO = new IOUtilities();
        /**
        * A collection containing all the integers variables
        */
        [SerializeField] protected List<IntegerVariableData> integers = new List<IntegerVariableData>();
        [SerializeField] protected List<FloatVariableData> decimals = new List<FloatVariableData>();
        [SerializeField] protected List<BooleanVariableData> booleans = new List<BooleanVariableData>();


        public IntegerVariableData AddIntegerVariable()
        {
            IntegerVariableData variable = IO.CreateAsset<IntegerVariableData>
                                           (VariableSystem.GetIntegerVariablesPath(), $"New Integer Variable {integers.Count}");
            if (variable == null)
            {
                Debug.LogError("Can't create integer variable!");
                return null;
            }

            variable.Id = Guid.NewGuid().ToString();
            variable.Name = $"New Integer Variable {integers.Count}";
            variable.Value = 0;

            integers.Add(variable);
            VariableEvents.VariableAddedToDatabase.Invoke(variable.Id);
            return variable;
        }

        public BooleanVariableData AddBooleanVariable()
        {
            BooleanVariableData variable = IO.CreateAsset<BooleanVariableData>
                                           (VariableSystem.GetBoolVariablesPath(), $"New Boolean Variable {booleans.Count}");

            if (variable == null)
            {
                Debug.LogError("Can't create boolean variable");
                return null;
            }

            variable.Id = Guid.NewGuid().ToString();
            variable.Name = $"New Boolean Variable {booleans.Count}";
            variable.Value = false;

            booleans.Add(variable);
            VariableEvents.VariableAddedToDatabase.Invoke(variable.Id);
            return variable;
        }


        public FloatVariableData AddFloatVariable()
        {
            FloatVariableData variable = IO.CreateAsset<FloatVariableData>
                                           (VariableSystem.GetFloatVariablesPath(), $"New Float Variable {decimals.Count}");
            if (variable == null)
            {
                Debug.LogError("Can't create float variable");
                return null;
            }

            variable.Id = Guid.NewGuid().ToString();
            variable.Name = $"New Float Variable {decimals.Count}";
            variable.Value = 0.0f;

            decimals.Add(variable);
            VariableEvents.VariableAddedToDatabase.Invoke(variable.Id);
            return variable;
        }


        public void RemoveVariable<T>(string id) where T :ScriptableObject
        {
            T variable = GetVariable<T>(id);
            if (variable == null)
            {
                Debug.LogError("Can't get variable with id " + id);
                return;
            }

            if (variable is FloatVariableData floatVar)
            {
                decimals.Remove(floatVar);
                IO.RemoveAsset(VariableSystem.GetFloatVariablesPath(), floatVar.Name);
                return;
            }

            if (variable is IntegerVariableData intVar)
            {
                integers.Remove(intVar);
                IO.RemoveAsset(VariableSystem.GetIntegerVariablesPath(), intVar.Name);
                return;
            }

            if (variable is BooleanVariableData boolVar)
            {
                booleans.Remove(boolVar);
                IO.RemoveAsset(VariableSystem.GetBoolVariablesPath(), boolVar.Name);
                return;
            }

            Debug.LogError("Variable type is not supported for removal");
        }


        public T GetVariable<T>(string id) where T : ScriptableObject
        {
            foreach (var variable in integers)
            {
                if (variable.Id == id && variable is T) return variable as T;
            }

            foreach (var variable in decimals)
            {
                if (variable.Id == id && variable is T) return variable as T;
            }

            foreach (var variable in booleans)
            {
                if (variable.Id == id && variable is T) return variable as T;
            }

            return null;
        }


        public List<IntegerVariableData> GetIntegers() => integers;
        public List<FloatVariableData> GetDecimals() => decimals;
        public List<BooleanVariableData> GetBooleans() => booleans;

        public void SetVariableName<T>(string id, string name) where T : VariableData<T>
        {
            var variable = GetVariable<T>(id);
            if (variable == null)
            {
                Debug.LogError("Can't find " + id + " inside the variable database");
                return;
            }

            variable.Name = name;
            VariableEvents.VariableNameChanged.Invoke(id, name);
        }
        public void SetVariableValue<T>(string id, T value) where T : VariableData<T>
        {
            var variable = GetVariable<T>(id);
            if (variable == null)
            {
                Debug.LogError("Can't find " + id + " inside the variable database");
                return;
            }

            if (variable is FloatVariableData && value is float floatValue)
            {
                (variable as FloatVariableData).Value = floatValue;
            }
            else if (variable is IntegerVariableData && value is int intValue)
            {
                (variable as IntegerVariableData).Value = intValue;
            }
            else if (variable is BooleanVariableData && value is bool boolValue)
            {
                (variable as BooleanVariableData).Value = boolValue;
            }
            else
            {
                Debug.LogError("Variable value type mismatch!");
                return;
            }

            VariableEvents.VariableValueChanged.Invoke(id);
        }
    }
}