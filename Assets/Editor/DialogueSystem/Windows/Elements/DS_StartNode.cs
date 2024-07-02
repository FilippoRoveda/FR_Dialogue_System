using UnityEngine;
using UnityEngine.UIElements;

namespace DS.Editor.Windows.Elements
{
    using Runtime.Data;
    using Editor.Data;
    using Enumerations;

    public class DS_StartNode : DS_BaseNode
    {
        public override void Initialize(string nodeName, DS_GraphView context, Vector2 spawnPosition)
        {
            base.Initialize(nodeName, context, spawnPosition);

            Texts = DS_LenguageUtilities.InitLenguageDataSet("Start Dialogue Text");
            SetDialogueType(DS_DialogueType.Start);
            DS_ChoiceData choiceData = new DS_ChoiceData("Starting Choice");
            Choices.Add(choiceData);
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
        /// <summary>
        /// Return true if this node is a starting node.
        /// </summary>
        /// <returns></returns>
        public override bool IsStartingNode()
        {
            return true;
        }
    }
}
