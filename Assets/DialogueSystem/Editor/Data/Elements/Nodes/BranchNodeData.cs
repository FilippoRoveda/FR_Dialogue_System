using System.Collections.Generic;
using UnityEngine;

namespace DS.Editor.Data
{
    using Editor.Elements;
    using Editor.Conditions;

    [System.Serializable]
    public class BranchNodeData : BaseNodeData
    {
        [SerializeField] protected Conditions conditions;
        public Conditions Conditions
        {
            get { return conditions; }
            set { conditions = value; }
        }
        [SerializeField] protected List<ChoiceData> choices;
        public List<ChoiceData> Choices 
        {
            get { return choices; }
            set { choices = value; }
        }
        public BranchNodeData() : base(){ }
        public BranchNodeData(BranchNodeData data) :base(data)
        {
            conditions = new Conditions();
            conditions.Reload(data.Conditions);
            choices = new List<ChoiceData>(data.Choices);
        }
        public BranchNodeData(BranchNode node) : base(node)
        {
            conditions = new Conditions();
            conditions.Reload(node.conditions);
            choices = new List<ChoiceData>(node.choices);
        }
    }
}
