using UnityEngine;


namespace DS.Editor.Elements
{
    using Editor.Enumerations;
    using Editor.Data;
    using DS.Editor.Windows;
    using UnityEditor.Experimental.GraphView;
    using System.Collections.Generic;

    /// <summary>
    /// Child class that represent a single choice version of the base DS_Node.
    /// </summary>
    public class SingleNode : DialogueNode
    {
        protected Port inputPort;
        protected Port outputPort;

        public SingleNode() { }
        #region Unity callbacks
        public override void Initialize(string nodeName, DS_GraphView context, Vector2 spawnPosition)
        {
            _nodeID = System.Guid.NewGuid().ToString();
            _nodeName = nodeName;
            _position = spawnPosition;
            SetPosition(new Rect(spawnPosition, Vector2.zero));
            _graphView = context;
            SetNodeStyle();

            _texts = LenguageUtilities.InitLenguageDataSet("Single Dialogue Text");
            _graphView.GraphLenguageChanged.AddListener(OnGraphViewLenguageChanged);


            _choices = new List<ChoiceData>();
            _graphView.GraphLenguageChanged.AddListener(OnGraphViewLenguageChanged);


            _nodeType = NodeType.Single;

            ChoiceData choiceData = new ChoiceData("Next Single Choice");
            _choices.Add(choiceData);
        }
        public override void Draw()
        {
            base.Draw();

            inputPort = CreateInputPort();
            outputPort = CreateOutputPortFromChoices()[0];

            RefreshExpandedState();
        }
        protected override void SetNodeStyle()
        {
            extensionContainer.AddToClassList("ds-node_extension-container");
            mainContainer.AddToClassList("ds-single-node_main-container");
            SetDefaultColor(mainContainer.style.backgroundColor);
        }
        #endregion
    }
}
