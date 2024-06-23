using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace DS.Elements
{
    using Data.Save;
    using Enumerations;
    using Windows;

    public class DS_EndNode : DS_BaseNode
    {
        public override void Initialize(string nodeName, DS_GraphView context, Vector2 spawnPosition)
        {
            base.Initialize(nodeName, context, spawnPosition);
            ID = Guid.NewGuid().ToString();
            DialogueName = nodeName;
            Choices = new List<DS_NodeChoiceData>();
            defaultColor = new Color(20f / 255f, 20f / 255f, 255f / 255f);
            graphView = context;
            SetPosition(new Rect(spawnPosition, Vector2.zero));


            Text = "End Dialogue Text";
            SetDialogueType(DS_DialogueType.End);

            extensionContainer.AddToClassList("ds-start-node_extension-container");
            mainContainer.AddToClassList("ds-start-node_main-container");
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

        /*enumField = new EnumField();
         * {
                value = endNodeType;
            }
            enumField.Init(endNodeType);
        enumField.RegisterValueChengedCallback((value) 
            {        
                endNodeType = (EndNodeType)value.newValue;
            });
        enumField.SetValueWithoutNotify();
        mainContaine.Add(enumField);
         * */
    }
}
