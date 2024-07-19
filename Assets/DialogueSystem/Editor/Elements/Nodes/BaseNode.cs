using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using System.Linq;


namespace DS.Editor.Elements
{
    using Editor.Enumerations;
    using Editor.Data;
    using Editor.Utilities;
    using Editor.Windows;


    /// <summary>
    /// Base dialogue system node class.
    /// </summary>
    public abstract class BaseNode : Node
    {
        public string _nodeName;
        public string _nodeID;
        public string _groupID;
        public NodeType _nodeType;
        public Vector2 _position;

        public DS_Group Group { get; set; }

        protected DS_GraphView _graphView;
        protected TextField dialogueNameField;
        protected StyleColor defaultColor;



        public virtual void Initialize(string nodeName, DS_GraphView context, Vector2 spawnPosition)
        {
            _nodeID = Guid.NewGuid().ToString();
            _nodeName = nodeName;
            _position = spawnPosition;
            SetPosition(new Rect(spawnPosition, Vector2.zero));
            _graphView = context;
            SetNodeStyle();
        }
        public void Initialize(BaseNodeData _data, DS_GraphView context)
        {

            _nodeID = _data.NodeID;
            _nodeName = _data.Name;
            _position = _data.Position;
            _nodeType = _data.NodeType;
            SetPosition(new Rect(_position, Vector2.zero));
            _graphView = context;
            SetNodeStyle();
            Debug.Log("Calling base node initializer with data");
        }


        public virtual void Draw()
        {
            //Dialogue name text field 
            dialogueNameField = ElementsUtilities.CreateTextField(_nodeName, null, callback => OnDialogueNameChanged(callback));
            dialogueNameField.AddToClassLists("ds-node-textfield", "ds-node-filename-textfield", "ds-node-textfield_hidden");
            titleContainer.Insert(0, dialogueNameField);
        }



        #region Overrides

        public override void SetPosition(Rect newPos)
        {
            _position = newPos.position;
            base.SetPosition(newPos);
        }

        #endregion

        #region Callbacks

        /// <summary>
        /// Callback called when the dialogue name changes.
        /// </summary>
        /// <param name="newDialogueName"></param>
        protected void OnDialogueNameChanged(ChangeEvent<string> callback)
        {
            TextField target = (TextField)callback.target;
            target.value = callback.newValue.RemoveWhitespaces().RemoveSpecialCharacters();

            if(string.IsNullOrEmpty(target.value))
            {
                if(string.IsNullOrEmpty(_nodeName) == false)
                {
                    _graphView.NameErrorsAmount++;
                }
            }
            else
            {
                if (string.IsNullOrEmpty(_nodeName) == true)
                {
                    _graphView.NameErrorsAmount--;
                }
            }

            if (Group == null)
            {
                _graphView.Remove_Node_FromUngrouped(this);
                _nodeName = target.value;
                _graphView.Add_Node_ToUngrouped(this);
            }
            else
            {
                DS_Group groupRef = Group;
                _graphView.Remove_Node_FromGroup(this, Group);
                _nodeName = target.value;
                _graphView.Add_Node_ToGroup(this, groupRef);
            }
        }

        #endregion

        #region Appearence style
        protected virtual void SetNodeStyle()
        {
            extensionContainer.AddToClassList("ds-node_extension-container");
            mainContainer.AddToClassList("ds-node_main-container");
            SetDefaultColor(mainContainer.style.backgroundColor);
        }
        public void SetDefaultColor(StyleColor defaultColor)
        {
            this.defaultColor = defaultColor;

        }
        public void SetErrorStyle(Color errorColor)
        {
            mainContainer.style.backgroundColor = errorColor;
        }
        public void ResetStyle()
        {
            mainContainer.style.backgroundColor = defaultColor;
        }
        #endregion

        #region Utilities

        /// <summary>
        /// Set the variable which indicates the group that owns this DS_Node.
        /// </summary>
        /// <param name="group"></param>
        public void SetGroup(DS_Group group)
        {
            Group = group;
        }
        public void RemoveFromGroup()
        {
            Group = null;
        }

        /// <summary>
        /// Return true if this node is a starting node.
        /// </summary>
        /// <returns></returns>
        public virtual bool IsStartingNode()
        {
            if(_nodeType == NodeType.Start) return true;
            else return false;
        }

        /// <summary>
        /// Return true if the node is overlapping with the passed node.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public bool IsOverlapping(BaseNode node)
        {
            return GetPosition().Overlaps(node.GetPosition());
        }

        public virtual void DisconnectAllPorts() { }
        #endregion
    }
}
