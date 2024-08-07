using UnityEngine;
using UnityEngine.UIElements;

namespace DS.Editor.Elements
{
    using Editor.Enumerations;
    using Editor.Data;
    using Editor.Windows;
    using System.Collections.Generic;

    /// <summary>
    /// Start node class which represent the begin of a dialogue.
    /// </summary>
    public class StartNode : DialogueNode
    {
        public StartNode() { }
        public override void Initialize(string nodeName, DS_GraphView context, Vector2 spawnPosition)
        {
            _nodeID = System.Guid.NewGuid().ToString();
            _nodeName = nodeName;
            _position = spawnPosition;
            SetPosition(new Rect(spawnPosition, Vector2.zero));
            _graphView = context;
            SetNodeStyle();

            _texts = LenguageUtilities.InitLenguageDataSet("Start Dialogue Text");

            _choices = new List<ChoiceData>();
            _graphView.GraphLenguageChanged.AddListener(OnGraphViewLenguageChanged);

            _nodeType = NodeType.Start;
           
            ChoiceData choiceData = new ChoiceData("Starting Choice");
            _choices.Add(choiceData);
        }
        public override void Draw()
        {
            base.Draw();

            CreateOutputPortFromChoices();
            RefreshExpandedState();
        }
        protected override void SetNodeStyle()
        {
            extensionContainer.AddToClassList("ds-node_extension-container");
            mainContainer.AddToClassList("ds-start-node_main-container");
            SetDefaultColor(mainContainer.style.backgroundColor);
        }
        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {            
            evt.menu.AppendAction("Disconnect Output Ports", actionEvent => DisconnectPorts(outputContainer));
            base.BuildContextualMenu(evt);
        }
    }
}
