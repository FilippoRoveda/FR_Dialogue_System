using System.Collections.Generic;
using UnityEngine;


 namespace DS.Runtime.Data
{
    [System.Serializable]
    public class DialogueConditionContainer
    {
        [SerializeField] private List<IntDialogueCondition> intConditions;
        [SerializeField] private List<FloatDialogueCondition> floatConditions;
        [SerializeField] private List<BoolDialogueCondition> boolConditions;

        public List<IntDialogueCondition> IntConditions { get { return intConditions; } }
        public List<FloatDialogueCondition> FloatConditions { get { return floatConditions; } }
        public List<BoolDialogueCondition> BoolConditions { get { return boolConditions; } }
        public DialogueConditionContainer()
        {
            Initialize();
        }

        public void Initialize()
        {
            intConditions = new List<IntDialogueCondition>();
            floatConditions = new List<FloatDialogueCondition>();
            boolConditions = new List<BoolDialogueCondition>();
        }
        public void Reload(DialogueConditionContainer conditionsContainer)
        {
            intConditions = new List<IntDialogueCondition>(conditionsContainer.IntConditions);
            floatConditions = new List<FloatDialogueCondition>(conditionsContainer.FloatConditions);
            boolConditions = new List<BoolDialogueCondition>(conditionsContainer.BoolConditions);
        }

        public IntDialogueCondition AddIntCondition(IntDialogueCondition condition = null)
        {
            if (condition == null)
            {
                var newCondition = new IntDialogueCondition();
                intConditions.Add(newCondition);
                return newCondition;
            }
            intConditions.Add(condition);
            return condition;
        }
        public FloatDialogueCondition AddFloatCondition(FloatDialogueCondition condition = null)
        {
            if (condition == null)
            {
                var newCondition = new FloatDialogueCondition();
                floatConditions.Add(newCondition);
                return newCondition;
            }
            floatConditions.Add(condition);
            return condition;
        }
        public BoolDialogueCondition AddBoolCondition(BoolDialogueCondition condition = null)
        {
            if (condition == null)
            {
                var newCondition = new BoolDialogueCondition();
                boolConditions.Add(newCondition);
                return newCondition;
            }
            boolConditions.Add(condition);
            return condition;
        }
        public void RemoveIntCondition(IntDialogueCondition condition = null) { intConditions.Remove(condition); }
        public void RemoveFloatCondition(FloatDialogueCondition condition = null) { floatConditions.Remove(condition); }
        public void RemoveBoolCondition(BoolDialogueCondition condition = null) { boolConditions.Remove(condition); }
    }
}
