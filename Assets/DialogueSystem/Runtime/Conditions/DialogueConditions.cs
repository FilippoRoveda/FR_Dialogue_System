using System.Collections.Generic;
using UnityEngine;


 namespace DS.Runtime.Conditions
{
    [System.Serializable]
    public class DialogueConditions
    {
        [SerializeField] private List<IntDialogueCondition> intConditions;
        [SerializeField] private List<FloatDialogueCondition> floatConditions;
        [SerializeField] private List<BoolDialogueCondition> boolConditions;

        public List<IntDialogueCondition> IntConditions { get { return intConditions; } }
        public List<FloatDialogueCondition> FloatConditions { get { return floatConditions; } }
        public List<BoolDialogueCondition> BoolConditions { get { return boolConditions; } }
        public DialogueConditions()
        {
            Initialize();
        }

        public void Initialize()
        {
            intConditions = new List<IntDialogueCondition>();
            floatConditions = new List<FloatDialogueCondition>();
            boolConditions = new List<BoolDialogueCondition>();
        }
        public void Reload(DialogueConditions dialogueConditions)
        {
            intConditions = new List<IntDialogueCondition>(dialogueConditions.IntConditions);
            floatConditions = new List<FloatDialogueCondition>(dialogueConditions.FloatConditions);
            boolConditions = new List<BoolDialogueCondition>(dialogueConditions.BoolConditions);
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
