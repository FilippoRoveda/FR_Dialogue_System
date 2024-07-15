using System.Collections.Generic;
using UnityEngine;


namespace DS.Editor.Data
{
    using DS.Enums;
    using Runtime.Data;

    [System.Serializable]
    public class EndNodeData : NodeData
    {
        [SerializeField] private bool isDialogueRepetable;
        public bool IsDialogueRepetable {  get { return isDialogueRepetable; } set { isDialogueRepetable = value; } }

        public EndNodeData(string _nodeID, string _dialogueName, List<ChoiceData> _choices,
                        List<LenguageData<string>> _texts, DialogueType _dialogueType,
                        string _groupID, Vector2 _position, bool _isDialogueRepetable) : base(_nodeID, _dialogueName, _choices, _texts, _dialogueType, _groupID, _position)
        {

            IsDialogueRepetable = _isDialogueRepetable;
        }
    }
}
