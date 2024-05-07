using DS.Elements;
using DS.Enumerations;
using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace DS.Windows
{
    using Data.Error;
    using Utilities;

    /// <summary>
    /// Dialogue system GraphView window class.
    /// </summary>
    public class DS_GraphView : GraphView
    {
        private DS_EditorWindow editorWindow; //Reference to the editor window class
        private DS_SearchWindow searchWindow; //Reference to the search window owned class

        public SerializableDictionary<string, DS_NodeErrorData> ungroupedNodes;
        public SerializableDictionary<Group, SerializableDictionary<string, DS_NodeErrorData>> groupedNodes;

        public DS_GraphView(DS_EditorWindow editorWindow)
        {
            //Fields initializings
            this.editorWindow = editorWindow;
            ungroupedNodes = new SerializableDictionary<string, DS_NodeErrorData>();
            groupedNodes = new SerializableDictionary<Group, SerializableDictionary<string, DS_NodeErrorData>>();

            //Update callbacks setups
            UpdateDeleteSelection();
            UpdateElementsAddedToGroup();
            UpdateElementRemovedFromGroup();

            //Adding
            Add_SearchWindow();
            Add_GridBackground();
            Add_Styles();
            Add_Manipulators();
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

        #region Callbacks update
        private void UpdateDeleteSelection()
        {
            deleteSelection = (operationName, askUser) =>
            {
                var count = selection.Count;

                for (var i = count - 1; i >= 0; i--)
                {
                    if (selection[i] is DS_Node node)
                    {
                        if (node.Group != null)
                        {
                            Remove_Node_FromGroup(node, node.Group);
                            Add_Node_ToUngrouped(node);
                        }
                        Remove_Node_FromUngrouped(node);

                        RemoveElement(node);
                    }
                    //else RemoveElement((GraphElement)selection[i]);
                }
            };
        }

        private void UpdateElementsAddedToGroup()
        {
            elementsAddedToGroup = (group, elements) =>
            {
                foreach(GraphElement element in elements)
                {
                    if ((element is DS_Node) == false) continue;
                    else
                    {
                        DS_Node node = (DS_Node)element;
                        Remove_Node_FromUngrouped(node);
                        Add_Node_ToGroup(node, group);
                    }
                }
            };
        }

        private void UpdateElementRemovedFromGroup()
        {
            elementsRemovedFromGroup = (group, elements) =>
            {
                foreach (GraphElement element in elements)
                {
                    if (!(element is DS_Node)) continue;
                    else
                    {
                        DS_Node node = (DS_Node)element;
                        Remove_Node_FromGroup(node, group);
                        Add_Node_ToUngrouped(node);
                    }
                }
            };
        }
        #endregion

        #region Manipulators

        /// <summary>
        /// Function that contain all the manipulators setup operations for zoom, manipulators and context men� options.
        /// </summary>
        private void Add_Manipulators()
        {
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());

            this.AddManipulator(CreateNode_CtxMenu_Option("Create Node(Single Choice)", DS_DialogueType.SingleChoice));
            this.AddManipulator(CreateNode_CtxMenu_Option("Create Node(Multiple Choice)", DS_DialogueType.MultipleChoice));
            this.AddManipulator(CreateGroup_CtxMenu_Option());
        }
        /// <summary>
        /// Add the group creation context menu option.
        /// </summary>
        /// <returns></returns>
        private IManipulator CreateGroup_CtxMenu_Option()
        {
            ContextualMenuManipulator contextualMenuManipulator = new ContextualMenuManipulator(
                menuEvent => menuEvent.menu.AppendAction("Create Group", actionEvent =>
                AddElement(CreateGroup("New node group", WorldToLocalMousePosition(actionEvent.eventInfo.localMousePosition)))));
            return contextualMenuManipulator;
        }
        /// <summary>
        /// Add a contextual menu option to create a new Node.
        /// </summary>
        /// <param name="actionTitle">The context option title.</param>
        /// <param name="dialogueType">Dialogue type that this option handle.</param>
        /// <returns></returns>
        private IManipulator CreateNode_CtxMenu_Option(string actionTitle, DS_DialogueType dialogueType)
        {
            ContextualMenuManipulator contextualMenuManipulator = new ContextualMenuManipulator(
                menuEvent => menuEvent.menu.AppendAction(actionTitle, actionEvent => 
                AddElement(CreateNode(WorldToLocalMousePosition(actionEvent.eventInfo.localMousePosition), dialogueType))));
            return contextualMenuManipulator;
        }
        #endregion

        #region Elements creation
        /// <summary>
        /// Function to create a specified type of Node during GrapView operations.
        /// </summary>
        /// <param name="spawnPosition">The screen spawn position for the node.</param>
        /// <param name="dialogueType">Enumerator selecting which type of node to spawn.</param>
        /// <returns>Pointer to the created node as a generic DS_Node.</returns>
        public DS_Node CreateNode(Vector2 spawnPosition, DS_DialogueType dialogueType)
        {
            Type nodeType = Type.GetType($"DS.Elements.DS_{dialogueType}Node"); //Generating the Type variable according to the indicated Name.
            DS_Node node = (DS_Node) Activator.CreateInstance(nodeType);

            node.Initialize(this, spawnPosition);
            node.Draw();

            Add_Node_ToUngrouped(node);
            return node;
        }

        /// <summary>
        /// Function to instantiate a Group during GraphView operations.
        /// </summary>
        /// <param name="groupName">The name of the group.</param>
        /// <param name="localMousePosition">The position, local to the window, on which spawn the group.</param>
        /// <returns>Pointer to the newly generated Group.</returns>
        public Group CreateGroup(string groupName, Vector2 localMousePosition)
        {
            Group group = new Group()
            {
                title = groupName
            };
            group.SetPosition(new Rect(localMousePosition, Vector2.zero));
            return group;
        }
        #endregion

        #region GraphView parts addition
        /// <summary>
        /// Add GridBackground class to this graph view container.
        /// </summary>
        private void Add_GridBackground()
        {
            GridBackground gridBackground = new GridBackground();
            gridBackground.StretchToParentSize();
            Insert(0, gridBackground);
        }

        /// <summary>
        /// Function tha add a Search window feature to the GraphView window.
        /// </summary>
        private void Add_SearchWindow()
        {
            if(searchWindow == null)
            {
                searchWindow = ScriptableObject.CreateInstance<DS_SearchWindow>();
                searchWindow.Initialize(this);
            }
            nodeCreationRequest = context => SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), searchWindow);
        }

        /// <summary>
        /// Load style sheet from resources and add that to the graph view visual elemente.
        /// </summary>
        private void Add_Styles()
        {

            this.AddStyleSheet( "DS_GridBackground.uss",
                                "DS_NodeStyles.uss");
        }
        #endregion

        #region Utilities
        /// <summary>
        /// Utility to convert a global screen pixel position to a local window view position.
        /// </summary>
        /// <param name="mousePos">Global mouse position when the function is called.</param>
        /// <param name="isSearchWindow">Boolean to detect if considerate the SearchWindow position during the operation.</param>
        /// <returns>A Vector2 representing the new window related local position.</returns>
        public Vector2 WorldToLocalMousePosition(Vector2 mousePos, bool isSearchWindow = false)
        {
           Vector2 worldMousePos = mousePos;

           if(isSearchWindow) 
           {
                worldMousePos -= editorWindow.position.position; 
           }

           Vector2 localMousePos = contentViewContainer.WorldToLocal(worldMousePos);
           return localMousePos;
        }
        #endregion

        /// <summary>
        /// Add the passed node to the ungrouped node dictionary.
        /// </summary>
        /// <param name="node">The node that need to be added to the dictionary.</param>
        public void Add_Node_ToUngrouped(DS_Node node)
        {
            string nodeName = node.DialogueName;

            if (ungroupedNodes.ContainsKey(nodeName) == false)
            {
                DS_NodeErrorData nodeErrorData = new();
                nodeErrorData.Nodes.Add(node);

                ungroupedNodes.Add(nodeName, nodeErrorData);
                return;
            }
            else
            {
                ungroupedNodes[nodeName].Nodes.Add(node);
                Color groupErrorColor = ungroupedNodes[nodeName].ErrorData.ErrorColor;

                node.SetErrorStyle(groupErrorColor);
                if (ungroupedNodes[nodeName].Nodes.Count == 2)
                {
                    ungroupedNodes[nodeName].Nodes[0].SetErrorStyle(groupErrorColor);
                }
                return;
            }
        }

        /// <summary>
        /// Remove an ungrouped node from its ungrouped node list.
        /// </summary>
        /// <param name="node">Node to remove from ungrouped nodes.</param>
        public void Remove_Node_FromUngrouped(DS_Node node)
        {
            string nodeName = node.DialogueName;
            List<DS_Node> nodeList = ungroupedNodes[nodeName].Nodes;

            nodeList.Remove(node);
            node.ResetStyle();

            if (nodeList.Count == 1)
            {
                nodeList[0].ResetStyle(); return;
            }
            if (nodeList.Count == 0)
            {
                ungroupedNodes.Remove(nodeName); return;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <param name="group"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void Add_Node_ToGroup(DS_Node node, Group group)
        {
            string nodeName = node.DialogueName;
            node.SetGroup(group);

            if (groupedNodes.ContainsKey(group) == false)
            {
                groupedNodes.Add(group, new SerializableDictionary<string, DS_NodeErrorData>());
                Debug.Log("Group added in group dictionary as a key:  " + group.name);
            }

            if (groupedNodes[group].ContainsKey(nodeName) == false)
            {
                DS_NodeErrorData nodeErrorData = new DS_NodeErrorData();
                nodeErrorData.Nodes.Add(node);
                groupedNodes[group].Add(nodeName, nodeErrorData);
                Debug.Log("Node added in group dictionary as new nodename:  " + nodeName);
                return;
            }
            else
            {
                List<DS_Node> groupedNodeList = groupedNodes[group][nodeName].Nodes;
                groupedNodes[group][nodeName].Nodes.Add(node);
                Color errorColor = groupedNodes[group][nodeName].ErrorData.ErrorColor;
                node.SetErrorStyle(errorColor);
                if (groupedNodeList.Count == 2)
                {
                    groupedNodeList[0].SetErrorStyle(errorColor);
                }
                Debug.Log("Node added in group dictionary as already existing nodename:  " + nodeName);
            }

        }

        public void Remove_Node_FromGroup(DS_Node node, Group group)
        {
            string nodeName = node.DialogueName;
            node.RemoveFromGroup();

            List<DS_Node> groupedNodesList = groupedNodes[group][nodeName].Nodes;
            groupedNodesList.Remove(node);
            node.ResetStyle();

            if (groupedNodesList.Count == 1)
            {
                groupedNodesList[0].ResetStyle();
                return;
            }
            if (groupedNodesList.Count == 0) groupedNodes[group].Remove(nodeName);
            if (groupedNodes[group].Count == 0) groupedNodes.Remove(group);
        }
    }
}