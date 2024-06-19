using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace DS.Elements
{
    using Data.Save;
    using Windows;
    using Enumerations;
    using Utilities;

    public class DS_StartNode : DS_BaseNode
    {
        public override void Initialize(string nodeName, DS_GraphView context, Vector2 spawnPosition)
        {
            ID = Guid.NewGuid().ToString();
            DialogueName = nodeName;
            Choices = new List<DS_NodeChoiceData>();
            defaultColor = new Color(27f / 255f, 27f / 255f, 7f / 255f);
            graphView = context;
            SetPosition(new Rect(spawnPosition, Vector2.zero));


            Text = "Start Dialogue Text";
            SetDialogueType(DS_DialogueType.Start);
            DS_NodeChoiceData choiceData = new DS_NodeChoiceData() { ChoiceText = "Starting Choice" };
            Choices.Add(choiceData);
            extensionContainer.AddToClassList("ds-start-node_extension-container");
            mainContainer.AddToClassList("ds-start-node_main-container");
        }
        public override void Draw()
        {
            base.Draw();
            foreach (DS_NodeChoiceData choice in Choices)
            {
                Port choicePort = this.CreatePort(choice.ChoiceText, Orientation.Horizontal, Direction.Output, Port.Capacity.Multi);
                choicePort.portName = choice.ChoiceText;
                choicePort.userData = choice;

                outputContainer.Add(choicePort);
            }
            RefreshExpandedState();
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
