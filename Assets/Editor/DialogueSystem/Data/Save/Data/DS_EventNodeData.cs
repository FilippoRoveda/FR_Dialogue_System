using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DS.Editor.Data
{
    using DS.Runtime.ScriptableObjects;
    using Runtime.Data;
    public class DS_EventNodeData : DS_NodeData
    {
        [SerializeField] protected List<DS_DialogueEventSO> events;
        public List<DS_DialogueEventSO> Events
        {
            get { return events; }
            set { events = value; }
        }
    }
}
