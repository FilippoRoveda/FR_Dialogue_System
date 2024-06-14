using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using System.Linq;


namespace DS.Elements
{
    using Data.Save;
    using Enumerations;
    using Utilities;
    using Windows;

    public class DS_Node : Node
    {
        [SerializeField] public string ID {  get; set; }
        [SerializeField] public string DialogueName { get; set; }
        [SerializeField] public List<DS_ChoiceData> Choices { get; set; }
        [SerializeField] public string Text { get; set; }
        [SerializeField] public DS_DialogueType DialogueType { get; private set; }
        [SerializeField] public DS_Group Group { get; set; } //Da far diventare group ID come stringa


        protected DS_GraphView graphView;
        private Color defaultColor;
        private Rect oldPostition;


        public virtual void Initialize(string nodeName, DS_GraphView context, Vector2 spawnPosition)
        {
            ID = Guid.NewGuid().ToString();
            DialogueName = nodeName;
            Choices = new List<DS_ChoiceData>();
            Text = "Dialogue Text";
            defaultColor = new Color(29f / 255f, 29f / 255f, 30f / 255f);
            this.graphView = context;

            SetPosition(new Rect(spawnPosition, Vector2.zero));

            extensionContainer.AddToClassList("ds-node_extension-container");
            mainContainer.AddToClassList("ds-node_main-container");
        }

        public virtual void Draw()
        {
            //Dialogue name text field 
            TextField dialogueNameField = DS_ElementsUtilities.CreateTextField("DialogueName", null,  callback => OnDialogueNameChanged(callback));

            dialogueNameField.AddToClassLists("ds-node-textfield", "ds-node-filename-textfield", "ds-node-textfield_hidden");

            titleContainer.Insert(0, dialogueNameField);

            Port choicePort = this.CreatePort("DialogueConnection", Orientation.Horizontal, Direction.Input, Port.Capacity.Single);
            choicePort.portName = "DialogueConnection";
            inputContainer.Add(choicePort);

            VisualElement customDataContainer = new VisualElement();
            customDataContainer.AddToClassList("ds-node-custom-data-container");


            //Dialogue text foldout and text field
            Foldout dialogueTextFoldout = DS_ElementsUtilities.CreateFoldout("DialogueText");
            TextField dialogueTextTextField = DS_ElementsUtilities.CreateTextArea(Text, null, callback => 
            { 
                Text = callback.newValue; 
            });

            dialogueTextTextField.AddToClassLists("ds-node-textfield", "ds-node-quote-textfield");

            dialogueTextFoldout.Add(dialogueTextTextField);
            customDataContainer.Add(dialogueTextFoldout);
            extensionContainer.Add(customDataContainer);
        }

        #region Overrides
        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            evt.menu.AppendAction("Disconnect Input Ports", actionEvent => DisconnectPorts(inputContainer));
            evt.menu.AppendAction("Disconnect Output Ports", actionEvent => DisconnectPorts(outputContainer));
            base.BuildContextualMenu(evt);
        }

        public override void SetPosition(Rect newPos)
        {
            oldPostition = GetPosition();
            base.SetPosition(newPos);
        }
        #endregion

        #region Callbacks

        /// <summary>
        /// Callback called when the dialogue name changes.
        /// </summary>
        /// <param name="newDialogueName"></param>
        private void OnDialogueNameChanged(ChangeEvent<string> callback)
        {
            TextField target = (TextField)callback.target;
            target.value = callback.newValue.RemoveWhitespaces().RemoveSpecialCharacters();

            if(string.IsNullOrEmpty(target.value))
            {
                if(string.IsNullOrEmpty(DialogueName) == false)
                {
                    graphView.NameErrorsAmount++;
                }
            }
            else
            {
                if (string.IsNullOrEmpty(DialogueName) == true)
                {
                    graphView.NameErrorsAmount--;
                }
            }

            if (Group == null)
            {
                graphView.Remove_Node_FromUngrouped(this);
                DialogueName = target.value;
                graphView.Add_Node_ToUngrouped(this);
            }
            else
            {
                DS_Group groupRef = Group;
                graphView.Remove_Node_FromGroup(this, Group);
                DialogueName = target.value;
                graphView.Add_Node_ToGroup(this, groupRef);
            }
        }
        #endregion

        #region Appearence style
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
        public void SetGroup(DS_Group group)
        {
            Group = group;
        }
        public void RemoveFromGroup()
        {
            Group = null;
        }
        protected void SetDialogueType(DS_DialogueType dialogueType)
        {
            DialogueType = dialogueType;
        }
        public void DisconnectPorts(VisualElement container)
        {
            foreach(Port port in container.Children())
            {
                if(port.connected == true)
                {
                    graphView.DeleteElements(port.connections);
                }
            }
        }
        public void DisconnectAllPorts()
        {
            DisconnectPorts(inputContainer);
            DisconnectPorts(outputContainer);
        }
        public bool IsStartingNode()
        {
            Port inputPort = (Port) inputContainer.Children().First();
            return !inputPort.connected;
        }

        public bool IsOverlapping(DS_Node node)
        {
            return GetPosition().Overlaps(node.GetPosition());
        }
        #endregion
    }
}
