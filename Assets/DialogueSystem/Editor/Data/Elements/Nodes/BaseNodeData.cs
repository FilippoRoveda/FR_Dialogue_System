using UnityEngine;

namespace DS.Editor.Data
{  
    using Editor.Enumerations;
    using Editor.Elements;

    /// <summary>
    /// Class that hold node informations to be contained in a graph scriptable object.
    /// </summary>
    [System.Serializable]
    public class BaseNodeData
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

              
        [SerializeField] protected string groupID = null;
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
        [SerializeField] protected NodeType dialogueType;
        public NodeType DialogueType
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
        public BaseNodeData(BaseNode node)
        {
            var baseData = node.Data;

            NodeID = baseData.NodeID;
            Name = baseData.Name;

            if (baseData.groupID != null) GroupID = baseData.groupID;

            DialogueType = baseData.DialogueType;
            Position = baseData.Position;
        }
    }
}