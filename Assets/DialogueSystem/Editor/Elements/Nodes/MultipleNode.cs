
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace DS.Editor.Elements
{
    using Editor.Enumerations;
    using Editor.Windows;
    using Editor.Data;

    /// <summary>
    /// Child class that represent a multiple choice version of the base DS_Node.
    /// </summary>
    public class MultipleNode : DialogueNode
    {
        private int choiceCounter;

        protected Port inputPort;
        protected List<Port> outputPorts;
        protected Button addChoiceButton;

        #region Unity callbacks
        public override void Initialize(string nodeName, DS_GraphView context, Vector2 spawnPosition)
        {
            base.Initialize(nodeName, context, spawnPosition);

            SetDialogueType(NodeType.Multiple);
            choiceCounter = 1;
            outputPorts = new List<Port>();

            ChoiceData choiceData = new ChoiceData("New Choice 1");
            Data.Choices.Add(choiceData);
        }
        public override void Draw()
        {
            base.Draw();

            inputPort = CreateInputPort();

            addChoiceButton = ElementsUtilities.CreateButton("Add Choice", () => OnAddChoiceButtonPressed());
            addChoiceButton.AddToClassList("ds-node-button");
            mainContainer.Insert(1, addChoiceButton);

            foreach (ChoiceData choice in Data.Choices)
            {
                Port choicePort = CreateDeletableChoicePort(choice);
                outputPorts.Add(choicePort);
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
            Data.Choices.Add(choiceData);
            outputPorts.Add(choicePort);
            outputContainer.Add(choicePort);
        }

        /// <summary>
        /// Callback for deleting a choice from this DS_MultipleChoiceNode.
        /// </summary>
        /// <param name="choicePort"></param>
        /// <param name="choiceData"></param>
        private void OnDeleteChoicePressed(Port choicePort, ChoiceData choiceData)
        {
            if (Data.Choices.Count == 1) return;

            else if (choicePort.connected == true) graphView.DeleteElements(choicePort.connections);

            Data.Choices.Remove(choiceData);
            outputPorts.Remove(choicePort);

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
