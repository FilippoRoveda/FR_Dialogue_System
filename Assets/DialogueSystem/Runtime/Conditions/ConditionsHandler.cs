using UnityEngine;

namespace DS.Runtime.Conditions
{
    using Variables.Generated;
    /// <summary>
    /// Handle and solve conditions for choices and branch dialogue nodes during dialogues.
    /// </summary>
    public class ConditionsHandler
    {
        public bool HandleConditions(DialogueConditions container)
        {
            foreach (var condition in container.IntConditions)
            {
                if(SolveIntCondition(condition) == false) return false;
                else continue;
            }
            foreach (var condition in container.FloatConditions)
            {
                if (SolveFloatCondition(condition) == false) return false;
                else continue;
            }
            foreach (var condition in container.BoolConditions)
            {
                if (SolveBoolCondition(condition) == false) return false;
                else continue;
            }

            return true;
        }
        public bool SolveIntCondition(IntDialogueCondition intCondition) 
        {
            var variableID = VariablesGenerated.Instance.variableMap[intCondition.VariableEnum];
            if (variableID == null)
            {
#if UNITY_EDITOR
                Debug.LogError($"Variable ID not founded in Generated variableMap for Enum: {intCondition.VariableEnum}.");
#endif
                return false;
            }

            var variable = VariablesGenerated.Instance.intVariables[variableID];
            if (variable == null)
            {
#if UNITY_EDITOR
                Debug.LogError($"IntVariable not founded in Generated with ID: {variableID}.");
#endif
                return false;
            }

            switch(intCondition.ComparisonType)
            {
                case ComparisonType.EQUAL:
                    if(variable.Value == intCondition.ComparisonValue) return true;
                    else return false;

                case ComparisonType.HIGHER:
                    if (variable.Value > intCondition.ComparisonValue) return true;
                    else return false;

                case ComparisonType.LOWER:
                    if (variable.Value < intCondition.ComparisonValue) return true;
                    else return false;

                case ComparisonType.HIGHER_EQUAL:
                    if (variable.Value >= intCondition.ComparisonValue) return true;
                    else return false;

                case ComparisonType.LOWER_EQUAL:
                    if (variable.Value <= intCondition.ComparisonValue) return true;
                    else return false;
                default:
#if UNITY_EDITOR
                    Debug.LogError($"{intCondition.ComparisonType} not matching comparison possibilities.");
#endif
                    return false;
            }
        }

        public bool SolveFloatCondition(FloatDialogueCondition floatCondition)
        {
            var variableID = VariablesGenerated.Instance.variableMap[floatCondition.VariableEnum];
            if (variableID == null)
            {
#if UNITY_EDITOR
                Debug.LogError($"Variable ID not founded in Generated variableMap for Enum: {floatCondition.VariableEnum}.");
#endif
                return false;
            }

            var variable = VariablesGenerated.Instance.floatVariables[variableID];
            if (variable == null)
            {
#if UNITY_EDITOR
                Debug.LogError($"FloatVariable not founded in Generated with ID: {variableID}.");
#endif
                return false;
            }

            switch (floatCondition.ComparisonType)
            {
                case ComparisonType.EQUAL:
                    if (variable.Value == floatCondition.ComparisonValue) return true;
                    else return false;

                case ComparisonType.HIGHER:
                    if (variable.Value > floatCondition.ComparisonValue) return true;
                    else return false;

                case ComparisonType.LOWER:
                    if (variable.Value < floatCondition.ComparisonValue) return true;
                    else return false;

                case ComparisonType.HIGHER_EQUAL:
                    if (variable.Value >= floatCondition.ComparisonValue) return true;
                    else return false;

                case ComparisonType.LOWER_EQUAL:
                    if (variable.Value <= floatCondition.ComparisonValue) return true;
                    else return false;
                default:
#if UNITY_EDITOR
                    Debug.LogError($"{floatCondition.ComparisonType} not matching comparison possibilities.");
#endif
                    return false;
            }
        }

        public bool SolveBoolCondition(BoolDialogueCondition boolCondition) 
        {
            var variableID = VariablesGenerated.Instance.variableMap[boolCondition.VariableEnum];
            if (variableID == null)
            {
#if UNITY_EDITOR
                Debug.LogError($"Variable ID not founded in Generated variableMap for Enum: {boolCondition.VariableEnum}.");
#endif
                return false;
            }

            var variable = VariablesGenerated.Instance.boolVariables[variableID];
            if (variable == null)
            {
#if UNITY_EDITOR
                Debug.LogError($"BoolVariable not founded in Generated with ID: {variableID}.");
#endif
                return false;
            }

            if (variable.Value == boolCondition.ComparisonValue) return true;
            else return false;
        }
    }
}
