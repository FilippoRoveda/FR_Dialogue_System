using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using System.Linq;
using System.Collections.Generic;


namespace DS.Editor.Elements
{
    using Editor.Enumerations;
    using Editor.Data;
    using Editor.Utilities;
    using Editor.Windows;


    public abstract class DialogueNode : TextedNode
    {
        public List<ChoiceData> _choices;

        public override void Initialize(string nodeName, DS_GraphView context, Vector2 spawnPosition)
        {
            _nodeID = System.Guid.NewGuid().ToString();
            _nodeName = nodeName;
            _position = spawnPosition;
            SetPosition(new Rect(spawnPosition, Vector2.zero));
            _graphView = context;
            SetNodeStyle();

            _texts = LenguageUtilities.InitLenguageDataSet("Dialogue Text");

            _choices = new List<ChoiceData>();
            _graphView.GraphLenguageChanged.AddListener(OnGraphViewLenguageChanged);
        }

        public virtual void Initialize(DialogueNodeData _data, DS_GraphView context)
        {

            _nodeID = _data.NodeID;
            _nodeName = _data.Name;
            _position = _data.Position;
            SetPosition(new Rect(_data.Position, Vector2.zero));
            _graphView = context;
            SetNodeStyle();

            _texts = new System.Collections.Generic.List<LenguageData<string>>(_data.Texts);
            _graphView.GraphLenguageChanged.AddListener(OnGraphViewLenguageChanged);

            _choices = new List<ChoiceData>(_data.Choices);
            Debug.Log("Calling dialogue node initializer with data");
        }

        #region Overrides
        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            evt.menu.AppendAction("Disconnect Input Ports", actionEvent => DisconnectPorts(inputContainer));
            evt.menu.AppendAction("Disconnect Output Ports", actionEvent => DisconnectPorts(outputContainer));
            base.BuildContextualMenu(evt);
        }
        protected override void OnGraphViewLenguageChanged(LenguageType newLenguage)
        {
            base.OnGraphViewLenguageChanged(newLenguage);

            foreach (var element in outputContainer.Children())
            {
                var port = (Port)element;
                var field = port.contentContainer.Children().ToList().Find(x => x.GetType() == typeof(TextField)) as TextField;
                field.SetValueWithoutNotify(((ChoiceData)port.userData).ChoiceTexts.Find(x => x.LenguageType == newLenguage).Data);
            }
        }
        #endregion
        #region Node parts creation

        protected Port CreateInputPort(string inputPortName = "DialogueConnection", Port.Capacity capacity = Port.Capacity.Multi)
        {
            Port choicePort = this.CreatePort(inputPortName, Orientation.Horizontal, Direction.Input, capacity);
            choicePort.portName = inputPortName;
            inputContainer.Add(choicePort);
            return choicePort;
        }

        protected virtual List<Port> CreateOutputPortFromChoices()
        {
            List<Port> choices = new List<Port>();
            foreach (ChoiceData choice in _choices)
            {
                choices.Add(CreateChoicePort(choice));
            }
            return choices;
        }

        protected virtual Port CreateChoicePort(object _choice)
        {
            ChoiceData choice = (ChoiceData)_choice;

            Port choicePort = this.CreatePort(choice.ChoiceTexts.GetLenguageData(_graphView.GetEditorCurrentLenguage()).Data,
                                Orientation.Horizontal, Direction.Output, Port.Capacity.Single);
            choicePort.portName = "";
            choicePort.userData = choice;

            TextField choiceTextField = ElementsUtilities.CreateTextField(choice.ChoiceTexts.GetLenguageData(_graphView.GetEditorCurrentLenguage()).Data,
                                            null,
                                            callback => UpdateChoiceLenguageData(callback, choice));


            choiceTextField.AddToClassLists("ds-node-textfield", "ds-node-choice-textfield", "ds-node-textfield_hidden");
            choiceTextField.style.flexDirection = FlexDirection.Column;

            choicePort.Insert(1, choiceTextField);

            outputContainer.Add(choicePort);
            return choicePort;
        }
        #endregion

        #region Utilities

        protected void AddNodeChoice(string choiceText)
        {
            ChoiceData choiceData = new ChoiceData(choiceText);
            _choices.Add(choiceData);
        }

        /// <summary>
        /// Disconnect all ports in both input container and output container.
        /// </summary>
        public override void DisconnectAllPorts()
        {
            DisconnectPorts(inputContainer);
            DisconnectPorts(outputContainer);
        }

        protected void UpdateChoiceLenguageData(ChangeEvent<string> callback, ChoiceData choice)
        {
            choice.ChoiceTexts.Find(x => x.LenguageType == _graphView.GetEditorCurrentLenguage()).Data = callback.newValue;
        }
        #endregion
    }
}
