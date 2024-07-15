using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace DS.Editor.Windows.Elements
{
    using Editor.Data;
    using Enums;

    /// <summary>
    /// Child class that represent a multiple choice version of the base DS_Node.
    /// </summary>
    public class DS_MultipleNode : DS_BaseNode
    {
        private int choiceCounter;

        #region Unity callbacks
        public override void Initialize(string nodeName, DS_GraphView context, Vector2 spawnPosition)
        {
            base.Initialize(nodeName, context, spawnPosition);

            SetDialogueType(DialogueType.Multiple);
            choiceCounter = 1;

            ChoiceData choiceData = new ChoiceData("New Choice 1");
            Choices.Add(choiceData);
        }
        public override void Draw()
        {
            base.Draw();

            CreateInputPort();

            Button addChoiceButton = ElementsUtilities.CreateButton("Add Choice", () => OnAddChoiceButtonPressed());

            addChoiceButton.AddToClassList("ds-node-button");

            mainContainer.Insert(1, addChoiceButton);

            foreach (ChoiceData choice in Choices)
            {
                Port choicePort = CreateDeletableChoicePort(choice);
                outputContainer.Add(choicePort);
            }

            RefreshExpandedState();
        }
        protected override void SetNodeStyle()
        {
            
            extensionContainer.AddToClassList("ds-node_extension-container");
            mainContainer.AddToClassList("ds-multiple-node_main-container");
            SetDefaultColor(mainContainer.style.backgroundColor);
        }
        #endregion

        #region Callbacks

        /// <summary>
        /// Callback for generate a new choiche for this DS_MultipleChoiceNode.
        /// </summary>
        private void OnAddChoiceButtonPressed()
        {
            ChoiceData choiceData = new ChoiceData($"New choice {choiceCounter + 1}");
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
        private void OnDeleteChoicePressed(Port choicePort, ChoiceData choiceData)
        {
            if (Choices.Count == 1) return;

            else if (choicePort.connected == true) graphView.DeleteElements(choicePort.connections);

            Choices.Remove(choiceData);

            graphView.RemoveElement(choicePort);
        }
        #endregion

        #region Elements creation
        /// <summary>
        /// Create a choice port.
        /// </summary>
        /// <param name="userData"></param>
        /// <returns></returns>
        protected Port CreateDeletableChoicePort(object _choice)
        {
            ChoiceData choiceData= (ChoiceData)_choice;

            Port choicePort = base.CreateChoicePort(_choice);

            Button deleteChoiceButton = ElementsUtilities.CreateButton("X", () => OnDeleteChoicePressed(choicePort, choiceData));
            deleteChoiceButton.AddToClassList("ds-node-button");

            choicePort.Add(deleteChoiceButton);
            return choicePort;
        }
        #endregion
    }
}
