using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;


namespace DialogueSystem.Eelements
{
    using Utilities;
    using Enumerations;
    using Windows;
    using UnityEditor.TestTools.TestRunner.Api;

    public class DS_Node : Node
    {
        [field: SerializeField]
        public string DialogueName { get; set; }
        public List<string> Choices { get; set; }
        public string Text { get; set; }
        public DS_DialogueType DialogueType { get; set; }


        private DS_GraphView context;
        private Color defaultColor;


        public virtual void Initialize(DS_GraphView context, Vector2 spawnPosition)
        {
            DialogueName = "Dialogue Name";
            Choices = new List<string>();
            Text = "Dialogue Text";
            defaultColor = new Color(29f / 255f, 29f / 255f, 30f / 255f);
            this.context = context;

            SetPosition(new Rect(spawnPosition, Vector2.zero));

            extensionContainer.AddToClassList("ds-node_extension-container");
            mainContainer.AddToClassList("ds-node_main-container");
        }

        public virtual void Draw()
        {
            //Dialogue name text field 
            TextField dialogueNameField = DS_ElementsUtilities.CreateTextField("DialogueName", callback => OnDialogueNameChanged(callback.newValue));

            dialogueNameField.AddToClassLists("ds-node-textfield", "ds-node-filename-textfield", "ds-node-textfield_hidden");

            titleContainer.Insert(0, dialogueNameField);

            Port choicePort = this.CreatePort("DialogueConnection", Orientation.Horizontal, Direction.Input, Port.Capacity.Single);
            choicePort.portName = "DialogueConnection";
            inputContainer.Add(choicePort);

            VisualElement customDataContainer = new VisualElement();
            customDataContainer.AddToClassList("ds-node-custom-data-container");


            //Dialogue text foldout and text field
            Foldout dialogueTextFoldout = DS_ElementsUtilities.CreateFoldout("DialogueText");
            TextField dialogueTextTextField = DS_ElementsUtilities.CreateTextArea("Dialogue text...");

            dialogueTextTextField.AddToClassLists("ds-node-textfield", "ds-node-quote-textfield");

            dialogueTextFoldout.Add(dialogueTextTextField);
            customDataContainer.Add(dialogueTextFoldout);
            extensionContainer.Add(customDataContainer);
        }

        #region Callbacks

        /// <summary>
        /// Callback called when the dialogue name 
        /// </summary>
        /// <param name="newDialogueName"></param>
        private void OnDialogueNameChanged(string newDialogueName)
        {
            context.RemoveUngroupedNode(this);
            this.DialogueName = newDialogueName;
            context.AddUngroupedNode(this);
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
    }
}
