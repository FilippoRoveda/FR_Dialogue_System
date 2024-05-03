using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace DialogueSystem.Eelements
{
    using Utilities;
    using Enumerations;

    public class DS_MultipleChoiceNode : DS_Node
    {
        public override void Initialize(Vector2 spawnPosition)
        {
            base.Initialize(spawnPosition);
            DialogueType = DS_DialogueType.MultipleChoice;
            Choices.Add("New Choice");
        }
        public override void Draw()
        {
            base.Draw();
            Button addChoiceButton = DS_ElementsUtilities.CreateButton("Add Choice", () =>
            {
                //Here i can add a int to detect the progress of adding new choices
                Port choicePort = CreateChoicePort("New choice");
                Choices.Add("New choice");
                outputContainer.Add(choicePort);
            });

            addChoiceButton.AddToClassList("ds-node-button");

            mainContainer.Insert(1, addChoiceButton);

            foreach (string choice in Choices)
            {
                Port choicePort = CreateChoicePort(choice);
                outputContainer.Add(choicePort);
            }

            RefreshExpandedState();
        }

        private Port CreateChoicePort(string choice)
        {
            Port choicePort = this.CreatePort(choice, Orientation.Horizontal, Direction.Output, Port.Capacity.Multi);
            choicePort.portName = "";           

            Button deleteChoiceButton = DS_ElementsUtilities.CreateButton("X");

            deleteChoiceButton.AddToClassList("ds-node-button");

            TextField choiceTextField = DS_ElementsUtilities.CreateTextField(choice);

            choiceTextField.AddToClassLists("ds-node-textfield", "ds-node-choice-textfield", "ds-node-textfield_hidden");
            //choiceTextField.AddToClassList("ds-node-textfield");
            //choiceTextField.AddToClassList("ds-node-choice-textfield");
            //choiceTextField.AddToClassList("ds-node-textfield_hidden");

            choiceTextField.style.flexDirection = FlexDirection.Column;

            choicePort.Add(deleteChoiceButton);
            choicePort.Insert(1, choiceTextField);


            return choicePort;
        }
    }
}
