using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;


namespace DialogueSystem.Eelements
{
    using Utilities;
    using Enumerations;

    public class DS_Node : Node
    {
        [field: SerializeField]
        public string DialogueName { get; set; }
        public List<string> Choices { get; set; }
        public string Text { get; set; }
        public DS_DialogueType DialogueType { get; set; }


        public virtual void Initialize(Vector2 spawnPosition)
        {
            DialogueName = "Dialogue Name";
            Choices = new List<string>();
            Text = "Dialogue Text";
            SetPosition(new Rect(spawnPosition, Vector2.zero));

            extensionContainer.AddToClassList("ds-node_extension-container");
            mainContainer.AddToClassList("ds-node_main-container");
        }

        public virtual void Draw()
        {
            //Dialogue name text field 
            TextField dialogueNameField = DS_ElementsUtilities.CreateTextField("DialogueName");

            dialogueNameField.AddToClassLists("ds-node-textfield", "ds-node-filename-textfield", "ds-node-textfield_hidden");
            //dialogueNameField.AddToClassList("ds-node-textfield");
            //dialogueNameField.AddToClassList("ds-node-filename-textfield");
            //dialogueNameField.AddToClassList("ds-node-textfield_hidden");

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
            //dialogueTextTextField.AddToClassList("ds-node-textfield");
            //dialogueTextTextField.AddToClassList("ds-node-quote-textfield");

            dialogueTextFoldout.Add(dialogueTextTextField);
            customDataContainer.Add(dialogueTextFoldout);
            extensionContainer.Add(customDataContainer);
        }
    }
}
