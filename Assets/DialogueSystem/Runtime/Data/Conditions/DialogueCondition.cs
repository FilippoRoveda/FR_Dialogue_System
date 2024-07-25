using UnityEngine;

namespace DS.Runtime.Data
{
    using Runtime.Enumerations;

    using VariableEnum = Variables.Generated.VariablesGenerated.VariablesKey;

    [System.Serializable]
    public abstract class DialogueCondition<T>
    {
        [SerializeField] protected VariableEnum variableEnum;
        public VariableEnum VariableEnum {  get { return variableEnum; } }

        [SerializeField] protected T comparisonValue;
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
            this.variableEnum = variableEnum;
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
        [SerializeField] private ComparisonType comparisonType;
        public ComparisonType ComparisonType { get { return comparisonType; } }
        public IntDialogueCondition() { }
        public IntDialogueCondition(VariableEnum variableEnum, int comparisonValue, ComparisonType comparisonType) : base(variableEnum, comparisonValue)
        {
            this.comparisonType = comparisonType;
        }

    }


    [System.Serializable]
    public class FloatDialogueCondition : DialogueCondition<float>
    {
        [SerializeField] private ComparisonType comparisonType;
        public ComparisonType ComparisonType { get { return comparisonType; } }
        public FloatDialogueCondition() { }
        public FloatDialogueCondition(VariableEnum variableEnum, float comparisonValue, ComparisonType comparisonType) : base(variableEnum, comparisonValue)
        {
            this.comparisonType = comparisonType;
        }
    }
}
