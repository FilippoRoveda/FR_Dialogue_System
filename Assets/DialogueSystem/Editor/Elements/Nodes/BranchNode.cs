using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace DS.Editor.Elements
{
    using Editor.Data;
    using Editor.Windows;
    using Editor.Conditions;

    public class BranchNode : BaseNode
    {
        public ConditionsContainer conditions;
        public List<ChoiceData> choices;

        protected Port inputPort;
        protected Port truePort;
        protected Port falsePort;

        public override void Initialize(string nodeName, DS_GraphView context, Vector2 spawnPosition)
        {
            base.Initialize(nodeName, context, spawnPosition);
            _nodeType = Enumerations.NodeType.Branch;
            conditions = new ConditionsContainer();
            choices = new List<ChoiceData>();
            var trueChoice = new ChoiceData("TRUE");
            var falseChoice = new ChoiceData("FALSE");
            choices.Add(trueChoice);
            choices.Add(falseChoice);
        }
        public void Initialize(BranchNodeData _data, DS_GraphView context)
        {
            _nodeID = _data.NodeID;
            _nodeName = _data.Name;
            _position = _data.Position;
            _nodeType = _data.NodeType;
            SetPosition(new Rect(_position, Vector2.zero));
            _graphView = context;
            SetNodeStyle();
            _nodeType = Enumerations.NodeType.Branch;
            conditions = new ConditionsContainer();
            conditions.Reload(_data.Conditions);
            choices = new List<ChoiceData>(_data.Choices);
        }
        public override void Draw()
        {
            base.Draw();

            var toolbarMenu = new ToolbarMenu();

            inputPort = CreateInputPort("Branch Input");
            toolbarMenu.text = "Add Condition";
            toolbarMenu.menu.AppendAction("Int Condition", callback => ElementsUtilities.AddIntCondition(conditions, contentContainer));
            toolbarMenu.menu.AppendAction("Float Condition", callback => ElementsUtilities.AddFloatCondition(conditions, contentContainer));
            toolbarMenu.menu.AppendAction("Bool Condition", callback => ElementsUtilities.AddBoolCondition(conditions, contentContainer));

            contentContainer.Add(toolbarMenu);
            truePort = CreateOutputPort(choices[0]);
            falsePort = CreateOutputPort(choices[1]);

            foreach (var intCondition in conditions.IntConditions) { ElementsUtilities.AddIntCondition(conditions, contentContainer, intCondition); }
            foreach (var floatCondition in conditions.FloatConditions) { ElementsUtilities.AddFloatCondition(conditions, contentContainer, floatCondition); }
            foreach (var boolCondition in conditions.BoolConditions) { ElementsUtilities.AddBoolCondition(conditions, contentContainer, boolCondition); }
        }

        protected Port CreateInputPort(string inputPortName = "DialogueConnection", Port.Capacity capacity = Port.Capacity.Multi)
        {
            Port choicePort = this.CreatePort(inputPortName, Orientation.Horizontal, Direction.Input, capacity);
            choicePort.portName = inputPortName;
            inputContainer.Add(choicePort);
            return choicePort;
        }

        protected virtual Port CreateOutputPort(object _choice)
        {
            ChoiceData choice = (ChoiceData)_choice;

            Box box = new Box();
            box.style.flexDirection = FlexDirection.Column;

            Port choicePort = this.CreatePort(choice.ChoiceTexts.GetLenguageData(_graphView.GetEditorCurrentLenguage()).Data,
                                Orientation.Horizontal, Direction.Output, Port.Capacity.Single);
            choicePort.portName = choice.ChoiceTexts[0].Data;
            choicePort.userData = choice;

            box.Add(choicePort);

            outputContainer.Add(box);
            return choicePort;
        }

        //Implementare disconnect all ports
        public override void DisconnectAllPorts()
        {
            DisconnectPorts(inputContainer);
            DisconnectPorts(outputContainer);
        }
    }
}
