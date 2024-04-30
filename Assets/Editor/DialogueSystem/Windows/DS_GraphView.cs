using DialogueSystem.Eelements;
using DialogueSystem.Enumerations;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace DialogueSystem.Windows
{
    public class DS_GraphView : GraphView
    {
        public DS_GraphView()
        {
            AddGridBackground();
            AddStyle();
            AddManipulators();
        }

        #region Overrides
        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            List<Port> compatiblePorts = new List<Port>();
            ports.ForEach(port =>
            {
                if (startPort == port) return;
                if (startPort.node == port.node) return;
                if (startPort.direction == port.direction) return;
                compatiblePorts.Add(port);
            });
            return compatiblePorts;
        }
        #endregion

        #region Manipulators
        private void AddManipulators()
        {
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());

            this.AddManipulator(CreateNode_CtxMenu_Option("Create Node(Single Choice)", DS_DialogueType.SingleChoice));
            this.AddManipulator(CreateNode_CtxMenu_Option("Create Node(Multiple Choice)", DS_DialogueType.MultipleChoice));
            this.AddManipulator(CreateGroup_CtxMenu_Option());
        }

        private IManipulator CreateGroup_CtxMenu_Option()
        {
            ContextualMenuManipulator contextualMenuManipulator = new ContextualMenuManipulator(
                menuEvent => menuEvent.menu.AppendAction("Create Group", actionEvent => AddElement(CreateGroup("New node group", actionEvent.eventInfo.localMousePosition)))
                );
            return contextualMenuManipulator;
        }
        private IManipulator CreateNode_CtxMenu_Option(string actionTitle, DS_DialogueType dialogueType)
        {
            ContextualMenuManipulator contextualMenuManipulator = new ContextualMenuManipulator(
                menuEvent => menuEvent.menu.AppendAction(actionTitle, actionEvent => AddElement(CreateNode(actionEvent.eventInfo.localMousePosition, dialogueType)))
                );
            return contextualMenuManipulator;
        }
        #endregion

        #region Create Elements
        private DS_Node CreateNode(Vector2 spawnPosition, DS_DialogueType dialogueType)
        {
            Type nodeType = Type.GetType($"DialogueSystem.Eelements.DS_{dialogueType.ToString()}Node");
            DS_Node node = (DS_Node) Activator.CreateInstance(nodeType);
            node.Initialize(spawnPosition);
            node.Draw();
            return node;
        }

        private Group CreateGroup(string groupName, Vector2 localMousePosition)
        {
            Group group = new Group()
            {
                title = groupName
            };
            group.SetPosition(new Rect(localMousePosition, Vector2.zero));
            return group;
        }
        #endregion


        /// <summary>
        /// Add GridBackground class to this graph view container.
        /// </summary>
        private void AddGridBackground()
        {
            GridBackground gridBackground = new GridBackground();
            gridBackground.StretchToParentSize();
            Insert(0, gridBackground);
        }

        /// <summary>
        /// Load style sheet from resources and add that to the graph view visual elemente.
        /// </summary>
        private void AddStyle()
        {
            StyleSheet gridStyle = (StyleSheet) EditorGUIUtility.Load("DialogueSystem/DS_GridBackground.uss");
            StyleSheet nodeStyle = (StyleSheet)EditorGUIUtility.Load("DialogueSystem/DS_NodeStyles.uss");
            styleSheets.Add(gridStyle);
            styleSheets.Add(nodeStyle);
        }

        
    }
}