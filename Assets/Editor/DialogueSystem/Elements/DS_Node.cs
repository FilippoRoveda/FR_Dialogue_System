using DialogueSystem.Enumerations;
using DS.Utilities;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace DialogueSystem.Eelements
{
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

            dialogueNameField.AddToClassList("ds-node-textfield");
            dialogueNameField.AddToClassList("ds-node-filename-textfield");
            dialogueNameField.AddToClassList("ds-node-textfield_hidden");

            titleContainer.Insert(0, dialogueNameField);

            //Input port element
            Port choicePort = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(bool));
            choicePort.portName = "DialogueConnection";
            inputContainer.Add(choicePort);

            VisualElement customDataContainer = new VisualElement();
            customDataContainer.AddToClassList("ds-node-custom-data-container");


            //Dialogue text foldout and text field
            Foldout dialogueTextFoldout = DS_ElementsUtilities.CreateFoldout("DialogueText");
            TextField dialogueTextTextField = DS_ElementsUtilities.CreateTextArea("Dialogue text...");

            dialogueTextTextField.AddToClassList("ds-node-textfield");
            dialogueTextTextField.AddToClassList("ds-node-quote-textfield");

            dialogueTextFoldout.Add(dialogueTextTextField);
            customDataContainer.Add(dialogueTextFoldout);
            extensionContainer.Add(customDataContainer);
        }
    }
}
