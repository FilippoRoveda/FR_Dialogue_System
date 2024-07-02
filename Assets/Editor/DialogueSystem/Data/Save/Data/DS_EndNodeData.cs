using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DS.Editor.Data
{
    using Runtime.Data;
    public class DS_EndNodeData : DS_NodeData
    {
        [SerializeField] private bool isDialogueRepetable;
        public bool IsDialogueRepetable {  get { return isDialogueRepetable; } set { isDialogueRepetable = value; } }
    }
}
