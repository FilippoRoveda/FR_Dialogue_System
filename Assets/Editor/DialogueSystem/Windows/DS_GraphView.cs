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
        public SerializableDictionary<DS_Group, SerializableDictionary<string, DS_NodeErrorData>> groupedNodes;

        public SerializableDictionary<string, DS_GroupErrorData> groups;

        public DS_GraphView(DS_EditorWindow editorWindow)
        {
            //Fields initializings
            this.editorWindow = editorWindow;
            ungroupedNodes = new SerializableDictionary<string, DS_NodeErrorData>();
            groupedNodes = new SerializableDictionary<DS_Group, SerializableDictionary<string, DS_NodeErrorData>>();

            groups = new SerializableDictionary<string, DS_GroupErrorData>();

            //Update callbacks setups
            UpdateDeleteSelection();
            UpdateElementsAddedToGroup();
            UpdateElementRemovedFromGroup();
            UpdateGroupTitleChanged();

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

                        //RemoveElement(node);
                    }
                    else if (selection[i] is DS_Group group)
                    {
                        Remove_Group_FromDictionary(group);
                        //RemoveElement(group);
                    }
                    //else
                    //{
                    //    RemoveElement((GraphElement)selection[i]);
                    //}

                    //L'ordine di cancellazione degli elementi qui deve essere gruppi->nodi->resto, questo è separato dalle operazioni di rimozione dai dizionari
                    //che parrebbe essere precedente
                }
                for (var i = count - 1; i >= 0; i--)
                {
                    RemoveElement((GraphElement)selection[i]);
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
                        DS_Group nodeGroup = (DS_Group)group; 
                        DS_Node node = (DS_Node)element;
                        Remove_Node_FromUngrouped(node);
                        Add_Node_ToGroup(node, nodeGroup);
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
                        DS_Group nodeGroup = (DS_Group)group;
                        DS_Node node = (DS_Node)element;
                        Remove_Node_FromGroup(node, nodeGroup);
                        Add_Node_ToUngrouped(node);
                    }
                }
            };
        }
        private void UpdateGroupTitleChanged()
        {
            groupTitleChanged = (group, newTitle) =>
            {
                DS_Group dS_Group = (DS_Group)group;

                Remove_Group_FromDictionary(dS_Group);
                dS_Group.title = newTitle;
                Add_Group_ToDictionary(dS_Group);
            };
        }
        #endregion




        #region Manipulators

        /// <summary>
        /// Function that contain all the manipulators setup operations for zoom, manipulators and context menù options.
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
        public DS_Group CreateGroup(string groupName, Vector2 localMousePosition)
        {
            DS_Group group = new DS_Group(groupName, localMousePosition);
            Add_Group_ToDictionary(group);
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
                Debug.LogWarning("NEW UNGROUPED KEY ADDED");
                Debug.Log($"NEW KEY: [{nodeName}] NEW COUNT: [{nodeErrorData.Nodes.Count}]");
                //return;
            }
            else
            {
                ungroupedNodes[nodeName].Nodes.Add(node);
                Color groupErrorColor = ungroupedNodes[nodeName].ErrorData.ErrorColor;

                Debug.LogWarning("UNGROUPED NODE ADDED TO KEY");
                Debug.Log($" KEY: [{nodeName}] COUNT: [{ungroupedNodes[nodeName].Nodes.Count}]");

                node.SetErrorStyle(groupErrorColor);
                if (ungroupedNodes[nodeName].Nodes.Count == 2)
                {
                    ungroupedNodes[nodeName].Nodes[0].SetErrorStyle(groupErrorColor);
                }
                //return;
            }
            foreach (var key in ungroupedNodes.Keys)
            {
                Debug.LogWarning($"   KEY:[{key}] COUNT:[{ungroupedNodes[key].Nodes.Count}]");
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
            Debug.LogWarning("UNGROUPED REMOVED");
            Debug.Log($" KEY: [{nodeName}]  / OLD COUNT: [{ungroupedNodes[nodeName].Nodes.Count}]");
            nodeList.Remove(node);
            node.ResetStyle();
            Debug.Log($" NEW COUNT: [{ungroupedNodes[nodeName].Nodes.Count}]");
            if (nodeList.Count == 1)
            {
                nodeList[0].ResetStyle(); return;
            }
            if (nodeList.Count == 0)
            {
                Debug.LogWarning($"UNGROUPED REMOVED KEY: {nodeName}");
                ungroupedNodes.Remove(nodeName); return;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <param name="group"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void Add_Node_ToGroup(DS_Node node, DS_Group group)
        {
            string nodeName = node.DialogueName;
            node.SetGroup(group);

            if (groupedNodes.ContainsKey(group) == false)
            {
                Debug.LogWarning($"ADDING GROUPED KEY [[{group.name}][{group.title}]");
                var innerDictionary = new SerializableDictionary<string, DS_NodeErrorData>();

                groupedNodes.Add(group, innerDictionary);   
            }

            if (groupedNodes[group].ContainsKey(nodeName) == false)
            {
                Debug.LogWarning($"ADD INNER-DICT TO GROUPED KEY [[{group.name}][{group.title}]");

                DS_NodeErrorData nodeErrorData = new DS_NodeErrorData();
                nodeErrorData.Nodes.Add(node);
                groupedNodes[group].Add(nodeName, nodeErrorData);
                Debug.Log($"Node added as new nodename: [{nodeName}] COUNT: [{groupedNodes[group][nodeName].Nodes.Count}]");
                return;
            }
            else
            {
                Debug.LogWarning($"ADD NODE TO GROUPED KEY [[{group.name}][{group.title}] WITH EXISTING INNER-DICT nodename key: [{nodeName}] COUNT: [{groupedNodes[group][nodeName].Nodes.Count}]");
                List<DS_Node> groupedNodeList = groupedNodes[group][nodeName].Nodes;
                groupedNodes[group][nodeName].Nodes.Add(node);
                Color errorColor = groupedNodes[group][nodeName].ErrorData.ErrorColor;
                node.SetErrorStyle(errorColor);
                Debug.Log($"NEW COUNT: [{groupedNodes[group][nodeName].Nodes.Count}]");
                if (groupedNodeList.Count == 2)
                {
                    groupedNodeList[0].SetErrorStyle(errorColor);
                }  
            }
            foreach (var key in groupedNodes.Keys)
            {
                Debug.LogWarning($"   KEY:[{key}]");
                foreach (var key2 in groupedNodes[key].Keys)
                {
                    Debug.LogWarning($"   Inner nodename key:[{key2}]  +  COUNT:[{groupedNodes[key][key2].Nodes.Count}]");
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <param name="group"></param>
        public void Remove_Node_FromGroup(DS_Node node, DS_Group group)
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="group"></param>
        public void Add_Group_ToDictionary(DS_Group group)
        {
            string groupTitle = group.title;

            if(groups.ContainsKey(groupTitle) == false)
            {
                DS_GroupErrorData groupErrorData = new DS_GroupErrorData();
                groupErrorData.Groups.Add(group);
                groups.Add(groupTitle, groupErrorData);

                Debug.Log($"   NEW KEY:  {groupTitle}  /  NEW COUNT:  {groups[groupTitle].Groups.Count}");

                foreach (var key in groups.Keys)
                {
                    Debug.LogWarning($"   KEY:[{key}]  +  COUNT:[{groups[key].Groups.Count}]");
                }
                return;
            }
            else 
            {
                List<DS_Group> groupList = groups[groupTitle].Groups;
                groupList.Add(group);
                Color errorColor = groups[groupTitle].ErrorData.ErrorColor;
                group.SetErrorStyle(errorColor);

                Debug.Log($"   KEY:  {groupTitle}  /  COUNT:  {groups[groupTitle].Groups.Count}");

                if (groupList.Count == 2)
                {
                    groupList[0].SetErrorStyle(errorColor);
                }
                foreach (var key in groups.Keys)
                {
                    Debug.LogWarning($"   KEY:[{key}]  +  COUNT:[{groups[key].Groups.Count}]");
                }
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="group"></param>
        public void Remove_Group_FromDictionary(DS_Group group)
        {
            string groupTitle = group.title;
            List<DS_Group> groupList = groups[groupTitle].Groups;
            Debug.Log($"   SELECTED KEY:  {groupTitle}  /  COUNT BEFORE:  {groups[groupTitle].Groups.Count}");

            groupList.Remove(group);
            group.ResetStyle();

            Debug.Log($"   COUNT AFTER:  {groups[groupTitle].Groups.Count}");

            if (groupList.Count == 1)
            {
                groupList[0].ResetStyle();
            }
            else if(groupList.Count == 0)
            {
                groups.Remove(groupTitle);
                Debug.LogWarning($"   DELETED KEY: {groupTitle}");
            }

            foreach (var key in groups.Keys)
            {
                Debug.LogWarning($"   KEY:[{key}]  +  COUNT:[{groups[key].Groups.Count}]");
            }
        }
    }
}