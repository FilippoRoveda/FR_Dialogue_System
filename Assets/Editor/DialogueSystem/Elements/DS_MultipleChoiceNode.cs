using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace DS.Elements
{
    using Utilities;
    using Enumerations;
    using Windows;
    using Data.Save;

    /// <summary>
    /// Child class that represent a multiple choice version of the base DS_Node.
    /// </summary>
    public class DS_MultipleChoiceNode : DS_BaseNode
    {
        private int choiceCounter;

        #region Unity callbacks
        public override void Initialize(string nodeName, DS_GraphView context, Vector2 spawnPosition)
        {
            base.Initialize(nodeName, context, spawnPosition);

            SetDialogueType(DS_DialogueType.MultipleChoice);
            choiceCounter = 1;

            DS_Choice_SaveData choiceData = new DS_Choice_SaveData() { ChoiceName = "New Choice 1" };
            Choices.Add(choiceData);
        }
        public override void Draw()
        {
            base.Draw();

            Port inputChoicePort = this.CreatePort("DialogueConnection", Orientation.Horizontal, Direction.Input, Port.Capacity.Single);
            inputChoicePort.portName = "DialogueConnection";
            inputContainer.Add(inputChoicePort);

            Button addChoiceButton = DS_ElementsUtilities.CreateButton("Add Choice", () => OnAddChoiceButtonPressed());

            addChoiceButton.AddToClassList("ds-node-button");

            mainContainer.Insert(1, addChoiceButton);

            foreach (DS_Choice_SaveData choice in Choices)
            {
                Port choicePort = CreateChoicePort(choice);
                outputContainer.Add(choicePort);
            }

            RefreshExpandedState();
        }
        #endregion

        #region Callbacks

        /// <summary>
        /// Callback for generate a new choiche for this DS_MultipleChoiceNode.
        /// </summary>
        private void OnAddChoiceButtonPressed()
        {
            DS_Choice_SaveData choiceData = new DS_Choice_SaveData() { ChoiceName = $"New choice {choiceCounter + 1}" };
            choiceCounter++;

            Port choicePort = CreateChoicePort(choiceData);
            Choices.Add(choiceData);
            outputContainer.Add(choicePort);
        }

        /// <summary>
        /// Callback for deleting a choice from this DS_MultipleChoiceNode.
        /// </summary>
        /// <param name="choicePort"></param>
        /// <param name="choiceData"></param>
        private void OnDeleteChoiceClick(Port choicePort, DS_Choice_SaveData choiceData)
        {
            if (Choices.Count == 1) return;

            else if (choicePort.connected == true) graphView.DeleteElements(choicePort.connections);

            Choices.Remove(choiceData);

            graphView.RemoveElement(choicePort);
        }
        #endregion

        /// <summary>
        /// Create a choice port.
        /// </summary>
        /// <param name="userData"></param>
        /// <returns></returns>
        private Port CreateChoicePort(object userData)
        {
            DS_Choice_SaveData choiceData = (DS_Choice_SaveData)userData;

            Port choicePort = this.CreatePort(choiceData.ChoiceName, Orientation.Horizontal, Direction.Output, Port.Capacity.Multi);
            choicePort.portName = "";
            choicePort.userData = choiceData;

            Button deleteChoiceButton = DS_ElementsUtilities.CreateButton("X", () => OnDeleteChoiceClick(choicePort, choiceData));

            deleteChoiceButton.AddToClassList("ds-node-button");

            TextField choiceTextField = DS_ElementsUtilities.CreateTextField(choiceData.ChoiceName, null, callback => choiceData.ChoiceName = callback.newValue);

            choiceTextField.AddToClassLists("ds-node-textfield", "ds-node-choice-textfield", "ds-node-textfield_hidden");

            choiceTextField.style.flexDirection = FlexDirection.Column;

            choicePort.Add(deleteChoiceButton);
            choicePort.Insert(1, choiceTextField);

            return choicePort;
        }
    }
}
