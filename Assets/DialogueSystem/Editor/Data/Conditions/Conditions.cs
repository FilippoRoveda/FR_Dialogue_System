using UnityEngine;
using Variables.Editor;

namespace DS.Editor
{
    using Editor.Enumerations;

    [System.Serializable]
    public abstract class ConditionData<T>
    {
        [SerializeField] private T comparisonValue;
        public T ComparisonValue { get { return comparisonValue; } 
            set { 
                comparisonValue = value; 
                Debug.Log("Variable value changed to" + comparisonValue.ToString());
            }
        }
    }

    [System.Serializable]
    public class BoolCondition : ConditionData<bool>
    {
        [SerializeField] protected BooleanVariableData variable;
        public BooleanVariableData Variable 
        { 
            get { return variable; } 
            set { variable = value; Debug.Log("Changing target bool variable for condition."); }
        }
        public BoolCondition(BooleanVariableData boolVariable = null)
        {
            variable = boolVariable;
        }
    }

    [System.Serializable]
    public class IntCondition : ConditionData<int>
    {
        [SerializeField] protected IntegerVariableData variable;
        public IntegerVariableData Variable 
        { get { return variable; }
            set { variable = value; Debug.Log("Changing target int variable for condition."); }
        }


        [SerializeField] private ComparisonType comparisonType = ComparisonType.EQUAL;
        public ComparisonType ComparisonType { get { return comparisonType; } 
            set 
            { 
                comparisonType = value;
                Debug.Log("cAHNGING COMPARISON TYPE");
            } 
        }

        public IntCondition(IntegerVariableData intVariable = null)
        {
            variable = intVariable;
        }
    }

    [System.Serializable]
    public class FloatCondition : ConditionData<float>
    {
        [SerializeField] protected FloatVariableData variable;
        public FloatVariableData Variable 
        { 
            get { return variable; }
            set { variable = value; Debug.Log("Changing target float variable for condition."); }
        }

        [SerializeField] private ComparisonType comparisonType = ComparisonType.EQUAL;
        public ComparisonType ComparisonType { get { return comparisonType; } set { comparisonType = value; Debug.Log("cAHNGING COMPARISON TYPE"); } }

        public FloatCondition(FloatVariableData floatVariable = null)
        {
            variable = floatVariable;
        }
    }
}
