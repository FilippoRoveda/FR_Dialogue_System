using System.Collections.Generic;
using UnityEngine;

namespace DS.Editor
{
    [System.Serializable]
    public class ConditionsContainer
    {
        [SerializeField] private List<IntCondition> intConditions;
        [SerializeField] private List<FloatCondition> floatConditions;
        [SerializeField] private List<BoolCondition> boolConditions;

        public List<IntCondition> IntConditions {  get { return intConditions; } } 
        public List<FloatCondition> FloatConditions { get { return floatConditions; } }
        public List<BoolCondition> BoolConditions { get { return boolConditions; } }
        public ConditionsContainer()
        {
            Initialize();
        }

        public void Initialize()
        {
            intConditions = new List<IntCondition>();
            floatConditions = new List<FloatCondition>();
            boolConditions = new List<BoolCondition>();
        }
        public void Reload(ConditionsContainer conditionsContainer)
        {
            intConditions = new List<IntCondition>(conditionsContainer.IntConditions);
            floatConditions = new List<FloatCondition>(conditionsContainer.FloatConditions);
            boolConditions = new List<BoolCondition>(conditionsContainer.BoolConditions);
        }

        public IntCondition AddIntCondition(IntCondition condition = null) 
        {           
            if (condition == null) 
            {
                var newCondition = new IntCondition();
                intConditions.Add(newCondition);
                return newCondition;
            }
            intConditions.Add(condition); 
            return condition;
        }
        public FloatCondition AddFloatCondition(FloatCondition condition = null) 
        {
            if (condition == null)
            {
                var newCondition = new FloatCondition();
                floatConditions.Add(newCondition);
                return newCondition;
            }
            floatConditions.Add(condition);
            return condition;
        }
        public BoolCondition AddBoolCondition(BoolCondition condition = null) 
        {
            if (condition == null)
            {
                var newCondition = new BoolCondition();
                boolConditions.Add(newCondition);
                return newCondition;
            }
            boolConditions.Add(condition);
            return condition;
        }
        public void RemoveIntCondition(IntCondition condition = null) { intConditions.Remove(condition); }
        public void RemoveFloatCondition(FloatCondition condition = null) { floatConditions.Remove(condition); }
        public void RemoveBoolCondition(BoolCondition condition = null) { boolConditions.Remove(condition); }
    }
}
