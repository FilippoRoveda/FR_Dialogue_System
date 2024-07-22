
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

        public MultipleNode() { }
        #region Unity callbacks
        public override void Initialize(string nodeName, DS_GraphView context, Vector2 spawnPosition)
        {
            _nodeID = System.Guid.NewGuid().ToString();
            _nodeName = nodeName;
            _position = spawnPosition;
            SetPosition(new Rect(spawnPosition, Vector2.zero));
            _graphView = context;
            SetNodeStyle();

            _texts = LenguageUtilities.InitLenguageDataSet("Multiple Dialogue Text");
            _graphView.GraphLenguageChanged.AddListener(OnGraphViewLenguageChanged);

            _nodeType = NodeType.Multiple;

            _choices = new List<ChoiceData>();
            ChoiceData choiceData = new ChoiceData("New Choice 1");
            _choices.Add(choiceData);
            _graphView.GraphLenguageChanged.AddListener(OnGraphViewLenguageChanged);

            choiceCounter = 1;
            outputPorts = new List<Port>();


        }
        public override void Initialize(DialogueNodeData _data, DS_GraphView context)
        {
            base.Initialize(_data, context);
            choiceCounter = _data.Choices.Count;
            outputPorts = new List<Port>();
        }
        public override void Draw()
        {
            base.Draw();
            Debug.Log(_choices.Count.ToString());
            inputPort = CreateInputPort();

            addChoiceButton = ElementsUtilities.CreateButton("Add Choice", () => OnAddChoiceButtonPressed());
            addChoiceButton.AddToClassList("ds-node-button");
            mainContainer.Insert(1, addChoiceButton);

            foreach (ChoiceData choice in _choices)
            {
                Port choicePort = CreateDeletableChoicePort(choice);
                outputPorts.Add(choicePort);
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
            _choices.Add(choiceData);
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
            if (_choices.Count == 1) return;

            else if (choicePort.connected == true) _graphView.DeleteElements(choicePort.connections);

            _choices.Remove(choiceData);
            outputPorts.Remove(choicePort);

            _graphView.RemoveElement(choicePort);
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

            Port choicePort = CreateChoicePort(_choice);

            Button deleteChoiceButton = ElementsUtilities.CreateButton("X", () => OnDeleteChoicePressed(choicePort, choiceData));
            deleteChoiceButton.AddToClassList("ds-node-button");

            choicePort.Add(deleteChoiceButton);
            return choicePort;
        }
        #endregion
    }
}
