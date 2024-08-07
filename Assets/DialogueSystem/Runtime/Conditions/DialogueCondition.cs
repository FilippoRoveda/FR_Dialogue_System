using UnityEngine;

namespace DS.Runtime.Conditions
{
    using VariableEnum = Variables.Generated.VariablesGenerated.VariablesKey;

    /// <summary>
    /// Generic abstract condition class holding common fields and methods to represent a dialogue condition.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [System.Serializable]
    public abstract class DialogueCondition<T>
    {
        [SerializeField] protected VariableEnum _variableEnum;
        /// <summary>
        /// VaribleEnum representing the target dialogue variable.
        /// </summary>
        public VariableEnum VariableEnum {  get { return _variableEnum; } }

        [SerializeField] protected T comparisonValue;
        /// <summary>
        /// The value on which this conditio will be decided.
        /// </summary>
        public T ComparisonValue
        {
            get { return comparisonValue; }
            set
            {
                comparisonValue = value;
                Debug.Log("Dialogue Condition comparison value changed to" + comparisonValue.ToString());
            }
        }

        public DialogueCondition() { }
        public DialogueCondition(VariableEnum variableEnum, T comaprisonValue)
        {
            this._variableEnum = variableEnum;
            this.comparisonValue = comaprisonValue;
        }
    }

    [System.Serializable]
    public class BoolDialogueCondition : DialogueCondition<bool>
    {
        public BoolDialogueCondition() { }
        public BoolDialogueCondition(VariableEnum variableEnum, bool comparisonValue) : base(variableEnum, comparisonValue) 
        { }
    }


    [System.Serializable]
    public class IntDialogueCondition : DialogueCondition<int>
    {
        [SerializeField] private ComparisonType _comparisonType;
        /// <summary>
        /// ComparisonType enumeration that will decide how the condition will be decided.
        /// </summary>
        public ComparisonType ComparisonType { get { return _comparisonType; } }
        public IntDialogueCondition() { }
        public IntDialogueCondition(VariableEnum variableEnum, int comparisonValue, ComparisonType comparisonType) : base(variableEnum, comparisonValue)
        {
            this._comparisonType = comparisonType;
        }

    }


    [System.Serializable]
    public class FloatDialogueCondition : DialogueCondition<float>
    {
        [SerializeField] private ComparisonType _comparisonType;
        /// <summary>
        /// ComparisonType enumeration that will decide how the condition will be decided.
        /// </summary>
        public ComparisonType ComparisonType { get { return _comparisonType; } }
        public FloatDialogueCondition() { }
        public FloatDialogueCondition(VariableEnum variableEnum, float comparisonValue, ComparisonType comparisonType) : base(variableEnum, comparisonValue)
        {
            this._comparisonType = comparisonType;
        }
    }
}
