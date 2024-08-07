using UnityEngine;
using Variables.Editor;

namespace DS.Editor.Conditions
{
    /// <summary>
    /// Base abstract class that have basic feature for condition implementation.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [System.Serializable]
    public abstract class ConditionData<T>
    {
        /// <summary>
        /// The value on which this condition compare its target variable.
        /// </summary>
        [SerializeField] protected T comparisonValue;
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
        /// <summary>
        /// Target boolean variable scriptable object for this condition.
        /// </summary>
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
        /// <summary>
        /// Target integer variable scriptable object for this condition.
        /// </summary>
        public IntegerVariableData Variable 
        { get { return variable; }
            set { variable = value; Debug.Log("Changing target int variable for condition."); }
        }


        [SerializeField] private ComparisonType comparisonType = ComparisonType.EQUAL;
        /// <summary>
        /// Comparison type with which the outcome of this condition is decided.
        /// </summary>
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
        /// <summary>
        /// Target float variable scriptable object for this condition.
        /// </summary>
        public FloatVariableData Variable 
        { 
            get { return variable; }
            set { variable = value; Debug.Log("Changing target float variable for condition."); }
        }

        [SerializeField] private ComparisonType comparisonType = ComparisonType.EQUAL;
        /// <summary>
        /// Comparison type with which the outcome of this condition is decided.
        /// </summary>
        public ComparisonType ComparisonType { get { return comparisonType; } set { comparisonType = value; Debug.Log("cAHNGING COMPARISON TYPE"); } }

        public FloatCondition(FloatVariableData floatVariable = null)
        {
            variable = floatVariable;
        }
    }
}
