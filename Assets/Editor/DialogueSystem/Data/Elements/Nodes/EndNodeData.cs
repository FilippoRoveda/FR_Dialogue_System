using System.Collections.Generic;
using UnityEngine;


namespace DS.Editor.Data
{
    using DS.Editor.Elements;
    using DS.Enums;
    using Runtime.Data;

    [System.Serializable]
    public class EndNodeData : TextedNodeData
    {
        [SerializeField] private bool isDialogueRepetable;
        public bool IsDialogueRepetable {  get { return isDialogueRepetable; } set { isDialogueRepetable = value; } }


        public EndNodeData() : base()
        {
            
        }
        public EndNodeData(EndNode node) 
                           : base(node)
        {

            IsDialogueRepetable = node.Data.IsDialogueRepetable;
        }
    }
}
