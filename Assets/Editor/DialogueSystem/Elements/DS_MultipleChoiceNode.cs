using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace DS.Elements
{
    using Utilities;
    using Enumerations;
    using Windows;
    using Data.Save;

    public class DS_MultipleChoiceNode : DS_Node
    {
        private int choiceCounter;

        public override void Initialize(DS_GraphView context, Vector2 spawnPosition)
        {
            base.Initialize(context, spawnPosition);

            SetDialogueType(DS_DialogueType.MultipleChoice);
            choiceCounter = 1;

            DS_ChoiceData choiceData = new DS_ChoiceData() { ChoiceName = "New Choice 1" };
            Choices.Add(choiceData);
        }

        public override void Draw()
        {
            base.Draw();
            Button addChoiceButton = DS_ElementsUtilities.CreateButton("Add Choice", () => OnAddChoiceButtonPressed());

            addChoiceButton.AddToClassList("ds-node-button");

            mainContainer.Insert(1, addChoiceButton);

            foreach (DS_ChoiceData choice in Choices)
            {
                Port choicePort = CreateChoicePort(choice);
                outputContainer.Add(choicePort);
            }

            RefreshExpandedState();
        }
        private void OnAddChoiceButtonPressed()
        {
            DS_ChoiceData choiceData = new DS_ChoiceData() { ChoiceName = $"New choice {choiceCounter + 1}" };
            choiceCounter++;

            Port choicePort = CreateChoicePort(choiceData);
            Choices.Add(choiceData);
            outputContainer.Add(choicePort);
        }
        private Port CreateChoicePort(object userData)
        {
            DS_ChoiceData choiceData = (DS_ChoiceData)userData;

            Port choicePort = this.CreatePort(choiceData.ChoiceName, Orientation.Horizontal, Direction.Output, Port.Capacity.Multi);
            choicePort.portName = "";

            Button deleteChoiceButton = DS_ElementsUtilities.CreateButton("X", () => OnDeleteChoiceClick(choicePort, choiceData));

            deleteChoiceButton.AddToClassList("ds-node-button");

            TextField choiceTextField = DS_ElementsUtilities.CreateTextField(choiceData.ChoiceName, null, callback => choiceData.ChoiceName = callback.newValue);

            choiceTextField.AddToClassLists("ds-node-textfield", "ds-node-choice-textfield", "ds-node-textfield_hidden");

            choiceTextField.style.flexDirection = FlexDirection.Column;

            choicePort.Add(deleteChoiceButton);
            choicePort.Insert(1, choiceTextField);

            return choicePort;
        }

        private void OnDeleteChoiceClick(Port choicePort, DS_ChoiceData choiceData)
        {
            if (Choices.Count == 1) return;

            else if (choicePort.connected == true) graphView.DeleteElements(choicePort.connections);

            Choices.Remove(choiceData);

            graphView.RemoveElement(choicePort);
        }
    }
}
