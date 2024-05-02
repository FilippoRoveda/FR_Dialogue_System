using UnityEngine;

namespace DialogueSystem.Eelements
{
    using DS.Utilities;
    using Enumerations;
    using UnityEditor.Experimental.GraphView;
    using UnityEngine.UIElements;

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
            Port choicePort = DS_ElementsUtilities.CreatePort(this);

            Button deleteChoiceButton = DS_ElementsUtilities.CreateButton("X");

            deleteChoiceButton.AddToClassList("ds-node-button");

            TextField choiceTextField = DS_ElementsUtilities.CreateTextField(choice);

            choiceTextField.AddToClassList("ds-node-textfield");
            choiceTextField.AddToClassList("ds-node-choice-textfield");
            choiceTextField.AddToClassList("ds-node-textfield_hidden");

            choiceTextField.style.flexDirection = FlexDirection.Column;

            choicePort.Insert(0, deleteChoiceButton);
            choicePort.Insert(1, choiceTextField);


            return choicePort;
        }
    }
}
