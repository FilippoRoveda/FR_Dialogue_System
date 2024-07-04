using System.Collections.Generic;
using UnityEngine;


namespace DS.Editor.Data
{
    using DS.Enums;
    using Runtime.Data;

    [System.Serializable]
    public class DS_EndNodeData : DS_NodeData
    {
        [SerializeField] private bool isDialogueRepetable;
        public bool IsDialogueRepetable {  get { return isDialogueRepetable; } set { isDialogueRepetable = value; } }

        public DS_EndNodeData(string _nodeID, string _dialogueName, List<DS_ChoiceData> _choices,
                        List<LenguageData<string>> _texts, DS_DialogueType _dialogueType,
                        string _groupID, Vector2 _position, bool _isDialogueRepetable) : base(_nodeID, _dialogueName, _choices, _texts, _dialogueType, _groupID, _position)
        {

            IsDialogueRepetable = _isDialogueRepetable;
        }
    }
}
