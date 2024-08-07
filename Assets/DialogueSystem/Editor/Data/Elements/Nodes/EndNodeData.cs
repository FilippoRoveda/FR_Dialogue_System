using UnityEngine;

namespace DS.Editor.Data
{
    using Editor.Elements;
    using System.Collections.Generic;

    /// <summary>
    /// NodeData class that hold every information for an EndNode.
    /// </summary>
    [System.Serializable]
    public class EndNodeData : TextedNodeData
    {
        [SerializeField] private bool isDialogueRepetable;
        public bool IsDialogueRepetable {  get { return isDialogueRepetable; } set { isDialogueRepetable = value; } }


        public EndNodeData() : base() { }

        public EndNodeData(EndNodeData data) : base(data)
        {
            isDialogueRepetable = data.IsDialogueRepetable;
        }
        public EndNodeData(EndNode node)  : base()                
        {
            _nodeID = node._nodeID;
            _name = node._nodeName;
            texts = new List<LenguageData<string>>(node._texts);
            texts = LenguageUtilities.UpdateLenguageDataSet(Texts);
            if (node._groupID != null) _groupID = node._groupID;

            _nodeType = node._nodeType;
            _position = node._position;

            isDialogueRepetable = node._isDialogueRepetable;
        }
    }
}
