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
        public bool _isDialogueRepetable;

        public EndNode() { }    
        public override void Initialize(string nodeName, DS_GraphView context, Vector2 spawnPosition)
        {
            _nodeID = System.Guid.NewGuid().ToString();
            _nodeName = nodeName;        
            _position = spawnPosition;
            SetPosition(new Rect(spawnPosition, Vector2.zero));
            _graphView = context;
            SetNodeStyle();

            _nodeType = NodeType.End;
            _texts = LenguageUtilities.InitLenguageDataSet("End Dialogue Text");
            _graphView.GraphLenguageChanged.AddListener(OnGraphViewLenguageChanged);
        }

        public void Initialize(EndNodeData _data, DS_GraphView context)
        {
            _nodeID = _data.NodeID;
            _nodeName = _data.Name;
            _position = _data.Position;
            SetPosition(new Rect(_position, Vector2.zero));
            _graphView = context;
            SetNodeStyle();

            _texts = new System.Collections.Generic.List<LenguageData<string>>(_data.Texts);
            _graphView.GraphLenguageChanged.AddListener(OnGraphViewLenguageChanged);

            _isDialogueRepetable = _data.IsDialogueRepetable;
            Debug.Log("Calling end node initializer with data");
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
                value = _isDialogueRepetable
            };

            isRepetableField.RegisterValueChangedCallback(evt =>
            {
                _isDialogueRepetable = evt.newValue;
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
        /// Disconnect all ports in both input container and output container.
        /// </summary>
        public override void DisconnectAllPorts()
        {
            DisconnectPorts(inputContainer);
        }
        protected void CreateInputPort(string inputPortName = "DialogueConnection", Port.Capacity capacity = Port.Capacity.Multi)
        {
            Port choicePort = this.CreatePort(inputPortName, Orientation.Horizontal, Direction.Input, capacity);
            choicePort.portName = inputPortName;
            inputContainer.Add(choicePort);
        }
    }
}
