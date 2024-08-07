using System.Collections.Generic;
using UnityEngine;

namespace DS.Editor.Data
{
    using Editor.Elements;
    using Editor.Conditions;

    /// <summary>
    /// NodeData class that hold every information for a BranchNode.
    /// </summary>
    [System.Serializable]
    public class BranchNodeData : BaseNodeData
    {
        [SerializeField] protected Conditions _conditions;
        public Conditions Conditions
        {
            get { return _conditions; }
            set { _conditions = value; }
        }
        [SerializeField] protected List<ChoiceData> _choices;
        public List<ChoiceData> Choices 
        {
            get { return _choices; }
            set { _choices = value; }
        }
        public BranchNodeData() : base(){ }
        public BranchNodeData(BranchNodeData data) :base(data)
        {
            _conditions = new Conditions();
            _conditions.Reload(data.Conditions);
            _choices = new List<ChoiceData>(data.Choices);
        }
        public BranchNodeData(BranchNode node) : base(node)
        {
            _conditions = new Conditions();
            _conditions.Reload(node.conditions);
            _choices = new List<ChoiceData>(node.choices);
        }
    }
}
