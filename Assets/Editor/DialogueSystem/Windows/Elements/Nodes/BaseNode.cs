using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using System.Linq;


namespace DS.Editor.Elements
{
    using Enums;
    using Editor.Data;
    using Editor.Utilities;
    using Windows;


    /// <summary>
    /// Base dialogue system node class.
    /// </summary>
    public abstract class BaseNode : Node
    {
        [SerializeField] private BaseNodeData data = new();
        public BaseNodeData Data { get => data; }
        public DS_Group Group { get; set; }

        protected DS_GraphView graphView;
        protected TextField dialogueNameField;
        protected StyleColor defaultColor;



        public virtual void Initialize(string nodeName, DS_GraphView context, Vector2 spawnPosition)
        {
            Data.NodeID = Guid.NewGuid().ToString();
            Data.Name = nodeName;                   
            Data.Position = spawnPosition;
            SetPosition(new Rect(spawnPosition, Vector2.zero));
            graphView = context;
            SetNodeStyle();
            //graphView.GraphLenguageChanged.AddListener(OnGraphViewLenguageChanged);
        }


        public virtual void Draw()
        {
            //Dialogue name text field 
            dialogueNameField = ElementsUtilities.CreateTextField(Data.Name, null, callback => OnDialogueNameChanged(callback));
            dialogueNameField.AddToClassLists("ds-node-textfield", "ds-node-filename-textfield", "ds-node-textfield_hidden");
            titleContainer.Insert(0, dialogueNameField);                 
        }



        #region Overrides

        public override void SetPosition(Rect newPos)
        {
            Data.Position = newPos.position;
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
                if(string.IsNullOrEmpty(Data.Name) == false)
                {
                    graphView.NameErrorsAmount++;
                }
            }
            else
            {
                if (string.IsNullOrEmpty(Data.Name) == true)
                {
                    graphView.NameErrorsAmount--;
                }
            }

            if (Group == null)
            {
                graphView.Remove_Node_FromUngrouped(this);
                Data.Name = target.value;
                graphView.Add_Node_ToUngrouped(this);
            }
            else
            {
                DS_Group groupRef = Group;
                graphView.Remove_Node_FromGroup(this, Group);
                Data.Name = target.value;
                graphView.Add_Node_ToGroup(this, groupRef);
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
        protected void SetDialogueType(DialogueType dialogueType)
        {
            Data.DialogueType = dialogueType;
        }

        /// <summary>
        /// Return true if this node is a starting node.
        /// </summary>
        /// <returns></returns>
        public virtual bool IsStartingNode()
        {
            Port inputPort = (Port) inputContainer.Children().First();
            return !inputPort.connected;
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
