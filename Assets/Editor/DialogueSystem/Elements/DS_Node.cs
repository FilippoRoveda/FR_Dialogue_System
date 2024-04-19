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


        public void Initialize()
        {
            DialogueName = "Dialogue Name";
            Choiches = new List<string>();
            Text = "Dialogue Text";
        }

        public void Draw()
        {
            //Dialogue name text field 
            TextField dialogueNameField = new TextField()
            {
                value = "DialogueName"
            };
            titleContainer.Insert(0, dialogueNameField);

            //Input port element
            Port inputPort = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(bool));
            inputPort.portName = "DialogueConnection";
            inputContainer.Add(inputPort);
            
            VisualElement customDataContainer = new VisualElement();

            //Dialogue text foldout and text field
            Foldout dialogueTextFoldout = new Foldout()
            {
                text = "DialogueText"
            };
            TextField dialogueTextTextField = new TextField()
            {
                value = "Dialogue text..."
            };
            dialogueTextFoldout.Add(dialogueTextTextField);
            customDataContainer.Add(dialogueTextFoldout);
            extensionContainer.Add(customDataContainer);

            RefreshExpandedState();
        }
    }
}