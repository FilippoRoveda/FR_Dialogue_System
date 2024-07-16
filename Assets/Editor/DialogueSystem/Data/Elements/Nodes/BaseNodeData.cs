namespace DS.Editor.Data
{
    using Enums;
    using UnityEngine;

    /// <summary>
    /// Class that hold node informations to be contained in a graph scriptable object.
    /// </summary>
    [System.Serializable]
    public abstract class BaseNodeData
    {
        [SerializeField] protected string name;
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }

        [SerializeField] protected string nodeID;
        public string NodeID
        {
            get
            {
                return nodeID;
            }
            set
            {
                nodeID = value;
            }
        }

              
        [SerializeField] protected string groupID;
        /// <summary>
        /// Group ID for the group that hold this node.
        /// </summary>
        public string GroupID
        {
            get
            {
                return groupID;
            }
            set
            {
                groupID = value;
            }
        }
        [SerializeField] protected DialogueType dialogueType;
        public DialogueType DialogueType
        {
            get
            {
                return dialogueType;
            }
            set
            {
                dialogueType = value;
            }
        }

        [SerializeField] protected Vector2 position;
        public Vector2 Position
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
            }
        }

        public BaseNodeData()
        {
            
        }

        public BaseNodeData(string _nodeID, string _dialogueName, DialogueType _dialogueType, string _groupID, Vector2 _position)
        {    
            this.NodeID = _nodeID;
            Name = _dialogueName;
         
            if (_groupID != null) this.GroupID = _groupID;
            else this.GroupID = null;

            this.DialogueType = _dialogueType;
            this.Position = _position;
        }
    }
}