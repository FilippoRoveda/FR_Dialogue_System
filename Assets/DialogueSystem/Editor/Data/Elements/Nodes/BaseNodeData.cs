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
        [SerializeField] protected NodeType nodeType;
        public NodeType NodeType
        {
            get
            {
                return nodeType;
            }
            set
            {
                nodeType = value;
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
        public BaseNodeData(BaseNodeData data)
        {
            nodeID = data.NodeID;
            name = data.Name;

            if (data.groupID != null) groupID = data.groupID;

            nodeType = data.NodeType;
            position = data.Position;
        }
        public BaseNodeData(BaseNode node)
        {
            nodeID = node._nodeID;
            name = node._nodeName;

            if (node._groupID != null) groupID = node._groupID;

            nodeType = node._nodeType;
            position = node._position;
        }
    }
}