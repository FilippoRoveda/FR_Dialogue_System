using UnityEngine;
using UnityEngine.UIElements;

namespace DS.Elements
{

    using DS.Utilities;
    using Enumerations;
    using Windows;

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
