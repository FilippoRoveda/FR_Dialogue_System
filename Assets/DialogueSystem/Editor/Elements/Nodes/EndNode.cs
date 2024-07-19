using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;

namespace DS.Editor.Elements
{
    using Editor.Enumerations;
    using Editor.Data;
    using Editor.Windows;
    using Editor.Utilities;

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
            //dialogueNameField = ElementsUtilities.CreateTextField(_nodeName, null, callback => OnDialogueNameChanged(callback));
            //dialogueNameField.AddToClassLists("ds-node-textfield", "ds-node-filename-textfield", "ds-node-textfield_hidden");
            //titleContainer.Insert(0, dialogueNameField);

            //customDataContainer = new VisualElement();
            //customDataContainer.AddToClassList("ds-node-custom-data-container");

            ////Dialogue text foldout and text field
            //dialogueTextFoldout = ElementsUtilities.CreateFoldout("DialogueText");

            //dialogueTextTextField = ElementsUtilities.CreateTextArea(CurrentText, null, callback =>
            //{
            //    data.Texts.GetLenguageData(_graphView.GetEditorCurrentLenguage()).Data = callback.newValue;
            //});

            //dialogueTextTextField.AddToClassLists("ds-node-textfield", "ds-node-quote-textfield");

            //dialogueTextFoldout.Add(dialogueTextTextField);
            //customDataContainer.Add(dialogueTextFoldout);
            //extensionContainer.Add(customDataContainer);


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
        /// Disconnect all ports in the passed container.
        /// </summary>
        /// <param name="container"></param>
        public void DisconnectPorts(VisualElement container)
        {
            foreach (Port port in container.Children())
            {
                if (port.connected == true)
                {
                    _graphView.DeleteElements(port.connections);
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
        /// Callback called when the dialogue name changes.
        /// </summary>
        /// <param name="newDialogueName"></param>
        protected new void OnDialogueNameChanged(ChangeEvent<string> callback)
        {
            TextField target = (TextField)callback.target;
            target.value = callback.newValue.RemoveWhitespaces().RemoveSpecialCharacters();

            if (string.IsNullOrEmpty(target.value))
            {
                if (string.IsNullOrEmpty(_nodeName) == false)
                {
                    _graphView.NameErrorsAmount++;
                }
            }
            else
            {
                if (string.IsNullOrEmpty(_nodeName) == true)
                {
                    _graphView.NameErrorsAmount--;
                }
            }

            if (Group == null)
            {
                _graphView.Remove_Node_FromUngrouped(this);
                _nodeName = target.value;
                _graphView.Add_Node_ToUngrouped(this);
            }
            else
            {
                DS_Group groupRef = Group;
                _graphView.Remove_Node_FromGroup(this, Group);
                _nodeName = target.value;
                _graphView.Add_Node_ToGroup(this, groupRef);
            }
        }
    }
}
