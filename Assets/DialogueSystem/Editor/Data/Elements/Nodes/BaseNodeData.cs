using UnityEngine;

namespace DS.Editor.Data
{
    using Editor.Elements;
    using Editor.Enumerations;

    /// <summary>
    /// Class that hold node informations to be contained in every node.
    /// </summary>
    [System.Serializable]
    public class BaseNodeData
    {
        [SerializeField] protected string _name;
        public string Name
        {
            get => _name;
            set => _name = value;
        }

        [SerializeField] protected string _nodeID;
        public string NodeID
        {
            get => _nodeID;
            set => _nodeID = value;
        }

              
        [SerializeField] protected string _groupID = null;
        /// <summary>
        /// Group ID for the group that hold this node.
        /// </summary>
        public string GroupID
        {
            get => _groupID;
            set => _groupID = value;
        }
        [SerializeField] protected NodeType _nodeType;
        public NodeType NodeType
        {
            get => NodeType;
            set => NodeType = value;
        }

        [SerializeField] protected Vector2 _position;
        public Vector2 Position
        {
            get => _position;
            set => _position = value;
        }

        public BaseNodeData() { }
        public BaseNodeData(BaseNodeData data)
        {
            _nodeID = data.NodeID;
            _name = data.Name;

            if (data._groupID != null) _groupID = data._groupID;

            _nodeType = data.NodeType;
            _position = data.Position;
        }
        public BaseNodeData(BaseNode node)
        {
            _nodeID = node._nodeID;
            _name = node._nodeName;

            if (node._groupID != null) _groupID = node._groupID;

            _nodeType = node._nodeType;
            _position = node._position;
        }
    }
}