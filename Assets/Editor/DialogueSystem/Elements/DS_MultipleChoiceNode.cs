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

            DS_NodeChoiceData choiceData = new DS_NodeChoiceData("New Choice 1");
            Choices.Add(choiceData);
        }
        public override void Draw()
        {
            base.Draw();

            CreateInputPort();

            Button addChoiceButton = DS_ElementsUtilities.CreateButton("Add Choice", () => OnAddChoiceButtonPressed());

            addChoiceButton.AddToClassList("ds-node-button");

            mainContainer.Insert(1, addChoiceButton);

            foreach (DS_NodeChoiceData choice in Choices)
            {
                Port choicePort = CreateDeletableChoicePort(choice);
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
            DS_NodeChoiceData choiceData = new DS_NodeChoiceData($"New choice {choiceCounter + 1}");
            choiceCounter++;

            Port choicePort = CreateDeletableChoicePort(choiceData);
            Choices.Add(choiceData);
            outputContainer.Add(choicePort);
        }

        /// <summary>
        /// Callback for deleting a choice from this DS_MultipleChoiceNode.
        /// </summary>
        /// <param name="choicePort"></param>
        /// <param name="choiceData"></param>
        private void OnDeleteChoicePressed(Port choicePort, DS_NodeChoiceData choiceData)
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
        protected Port CreateDeletableChoicePort(object _choice)
        {
            DS_NodeChoiceData choiceData= (DS_NodeChoiceData)_choice;

            Port choicePort = base.CreateChoicePort(_choice);

            Button deleteChoiceButton = DS_ElementsUtilities.CreateButton("X", () => OnDeleteChoicePressed(choicePort, choiceData));
            deleteChoiceButton.AddToClassList("ds-node-button");

            choicePort.Add(deleteChoiceButton);
            return choicePort;
        }
    }
}
