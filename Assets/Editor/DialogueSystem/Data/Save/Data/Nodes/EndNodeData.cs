using System.Collections.Generic;
using UnityEngine;


namespace DS.Editor.Data
{
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
        public EndNodeData(string _nodeID, string _dialogueName, List<LenguageData<string>> _texts, 
                           DialogueType _dialogueType, string _groupID, Vector2 _position, bool _isDialogueRepetable) 
                           : base(_nodeID, _dialogueName, _texts, _dialogueType, _groupID, _position)
        {

            IsDialogueRepetable = _isDialogueRepetable;
        }
    }
}
