using UnityEngine;
using UnityEngine.UIElements;

namespace DS.Editor.Windows.Elements
{
    using Runtime.Data;
    using Enumerations;

    public class DS_EndNode : DS_BaseNode
    {
        public override void Initialize(string nodeName, DS_GraphView context, Vector2 spawnPosition)
        {         
            base.Initialize(nodeName, context, spawnPosition);
            Texts = DS_LenguageUtilities.InitLenguageDataSet("End Dialogue Text");
            SetDialogueType(DS_DialogueType.End);
        }
        public override void Draw()
        {
            base.Draw();
            CreateInputPort("EndNode connection");
            RefreshExpandedState();
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
        /// Return true if this node is a starting node.
        /// </summary>
        /// <returns></returns>
        public override bool IsStartingNode()
        {
            return false;
        }
    }
}
