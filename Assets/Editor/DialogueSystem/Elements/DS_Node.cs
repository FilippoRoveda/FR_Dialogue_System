using DialogueSystem.Enumerations;
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
        public List<string> Choiches { get; set; }
        public string Text { get; set; }
        public DS_DialogueType DialogueType { get; set; }


        public virtual void Initialize(Vector2 spawnPosition)
        {
            DialogueName = "Dialogue Name";
            Choiches = new List<string>();
            Text = "Dialogue Text";
            SetPosition(new Rect(spawnPosition, Vector2.zero));

            extensionContainer.AddToClassList("ds-node_extension-container");
            mainContainer.AddToClassList("ds-node_main-container");
        }

        public virtual void Draw()
        {
            //Dialogue name text field 
            TextField dialogueNameField = new TextField()
            {
                value = "DialogueName"
            };

            dialogueNameField.AddToClassList("ds-node-textfield");
            dialogueNameField.AddToClassList("ds-node-filename-textfield");
            dialogueNameField.AddToClassList("ds-node-textfield_hidden");

            titleContainer.Insert(0, dialogueNameField);

            //Input port element
            Port inputPort = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(bool));
            inputPort.portName = "DialogueConnection";
            inputContainer.Add(inputPort);
            
            VisualElement customDataContainer = new VisualElement();
            customDataContainer.AddToClassList("ds-node-custom-data-container");


            //Dialogue text foldout and text field
            Foldout dialogueTextFoldout = new Foldout()
            {
                text = "DialogueText"
            };
            TextField dialogueTextTextField = new TextField()
            {
                value = "Dialogue text..."
            };

            dialogueTextTextField.AddToClassList("ds-node-textfield");
            dialogueTextTextField.AddToClassList("ds-node-quote-textfield");

            dialogueTextFoldout.Add(dialogueTextTextField);
            customDataContainer.Add(dialogueTextFoldout);
            extensionContainer.Add(customDataContainer);
        }
    }
}
