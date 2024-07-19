using UnityEngine;


namespace DS.Editor.Data
{
    using Editor.Elements;
    using System.Collections.Generic;

    [System.Serializable]
    public class EndNodeData : TextedNodeData
    {
        [SerializeField] private bool isDialogueRepetable;
        public bool IsDialogueRepetable {  get { return isDialogueRepetable; } set { isDialogueRepetable = value; } }


        public EndNodeData() : base()
        {
            
        }
        public EndNodeData(EndNodeData data) : base(data)
        {
            isDialogueRepetable = data.IsDialogueRepetable;
        }
        public EndNodeData(EndNode node)  : base()                
        {
            nodeID = node._nodeID;
            name = node._nodeName;
            texts = new List<LenguageData<string>>(node._texts);
            texts = LenguageUtilities.UpdateLenguageDataSet(Texts);
            if (node._groupID != null) groupID = node._groupID;

            nodeType = node._nodeType;
            position = node._position;

            isDialogueRepetable = node._isDialogueRepetable;
        }
    }
}
