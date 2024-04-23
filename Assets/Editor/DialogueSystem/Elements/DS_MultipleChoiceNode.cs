using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem.Eelements
{
    using Enumerations;
    using UnityEditor.Experimental.GraphView;
    using UnityEngine.UIElements;

    public class DS_MultipleChoiceNode : DS_Node
    {
        public override void Initialize(Vector2 spawnPosition)
        {
            base.Initialize(spawnPosition);
            DialogueType = DS_DialogueType.MultipleChoice;
            Choiches.Add("New Choice");
        }
        public override void Draw()
        {
            base.Draw();
            Button addChoiceButton = new Button()
            {
                text = "Add Choice"
            };
            mainContainer.Insert(1, addChoiceButton);
            foreach (string choice in Choiches)
            {
                Port choicePort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(bool));
                choicePort.portName = "";

                Button deleteChoiceButton = new Button()
                {
                    text = "X"
                };
                TextField choiceTextField = new TextField()
                {
                    value = choice
                };
                choiceTextField.style.flexDirection = FlexDirection.Column;
                choicePort.Add(choiceTextField);
                choicePort.Add(deleteChoiceButton);

                outputContainer.Add(choicePort);
            }

            RefreshExpandedState();
        }
    }
}
