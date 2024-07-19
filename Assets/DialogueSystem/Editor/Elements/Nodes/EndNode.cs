using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;

namespace DS.Editor.Elements
{
    using Editor.Enumerations;
    using Editor.Data;
    using Editor.Windows;

    public class EndNode : TextedNode
    {
        [SerializeField] private EndNodeData data = new();
        public new EndNodeData Data { get { return data; } }


        public override void Initialize(string nodeName, DS_GraphView context, Vector2 spawnPosition)
        {         
            base.Initialize(nodeName, context, spawnPosition);
            SetDialogueType(NodeType.End);
            Data.Texts = LenguageUtilities.InitLenguageDataSet("End Dialogue Text");
        }
        public override void Draw()
        {
            base.Draw();
            DrawIsRepetableField();
            CreateInputPort("EndNode connection");
            RefreshExpandedState();
        }

        private void DrawIsRepetableField()
        {
            VisualElement boolFieldContainer = new VisualElement();
            boolFieldContainer.style.flexDirection = FlexDirection.Row;
            boolFieldContainer.style.alignItems = Align.FlexStart;

            Label boolFieldLabel = new Label("Is Repeatable:");
            boolFieldLabel.style.marginRight = 10;
            Toggle isRepetableField = new Toggle
            {
                value = Data.IsDialogueRepetable
            };

            isRepetableField.RegisterValueChangedCallback(evt =>
            {
                Data.IsDialogueRepetable = evt.newValue;
            });

            boolFieldContainer.Add(boolFieldLabel);
            boolFieldContainer.Add(isRepetableField);

            extensionContainer.Insert(0, boolFieldContainer);
        }

        protected override void SetNodeStyle()
        {
            extensionContainer.AddToClassList("ds-node_extension-container");
            mainContainer.AddToClassList("ds-end-node_main-container");
            SetDefaultColor(mainContainer.style.backgroundColor);
        }
        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            evt.menu.AppendAction("Disconnect Inputs Ports", actionEvent => DisconnectPorts(inputContainer));
            base.BuildContextualMenu(evt);
        }


        /// <summary>
        /// Disconnect all ports in the passed container.
        /// </summary>
        /// <param name="container"></param>
        public void DisconnectPorts(VisualElement container)
        {
            foreach (Port port in container.Children())
            {
                if (port.connected == true)
                {
                    graphView.DeleteElements(port.connections);
                }
            }
        }
        protected void CreateInputPort(string inputPortName = "DialogueConnection", Port.Capacity capacity = Port.Capacity.Multi)
        {
            Port choicePort = this.CreatePort(inputPortName, Orientation.Horizontal, Direction.Input, capacity);
            choicePort.portName = inputPortName;
            inputContainer.Add(choicePort);
        }

        /// <summary>
        /// Return true if this node is a starting node.
        /// </summary>
        /// <returns></returns>
        public override bool IsStartingNode()
        {
            return false;
        }
    }
}
