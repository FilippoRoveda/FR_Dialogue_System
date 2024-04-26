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

            addChoiceButton.AddToClassList("ds-node-button");

            mainContainer.Insert(1, addChoiceButton);
            foreach (string choice in Choiches)
            {
                Port choicePort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(bool));
                choicePort.portName = "";

                Button deleteChoiceButton = new Button()
                {
                    text = "X"
                };

                deleteChoiceButton.AddToClassList("ds-node-button");

                TextField choiceTextField = new TextField()
                {
                    value = choice
                };
                choiceTextField.AddToClassList("ds-node-textfield");
                choiceTextField.AddToClassList("ds-node-choice-textfield");
                choiceTextField.AddToClassList("ds-node-textfield_hidden");

                choiceTextField.style.flexDirection = FlexDirection.Column;
                choicePort.Add(choiceTextField);
                choicePort.Add(deleteChoiceButton);

                outputContainer.Add(choicePort);
            }

            RefreshExpandedState();
        }
    }
}
