using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace DS.Windows
{
    using Data.Error;
    using DS.Data.Save;
    using Elements;
    using Enumerations;
    using Utilities;
    using static UnityEngine.GraphicsBuffer;

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

        private int nameErrorsAmount;
        public int NameErrorsAmount
        {
            get { return nameErrorsAmount; }
            set {
                int previewsValue = nameErrorsAmount;

                nameErrorsAmount = value;
                if(nameErrorsAmount == 0)
                {
                    editorWindow.EnableSaving();
                }
                if(previewsValue == 0 && nameErrorsAmount == 1)
                {
                    editorWindow.DisableSaving();
                }
            }
        }



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
            UpdateGraphViewChanged();

            //Adding
            Add_SearchWindow();
            AddMinimap();
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
                Logger.Error("Delete Selection callback");

                List<DS_Group> groupsToDelete = new List<DS_Group>();
                List<Edge> edgesToDelete = new List<Edge>();
                List<DS_Node> nodesToDelete = new List<DS_Node>();

                foreach(GraphElement element in selection)
                {
                    if(element is DS_Node node)
                    {
                        nodesToDelete.Add(node);
                        continue;
                    }
                    if(element is Edge edge)
                    {
                        edgesToDelete.Add(edge);
                        continue;
                    }
                    if(element is DS_Group group)
                    {
                        groupsToDelete.Add(group);
                        continue;
                    }
                    else
                    {
                        RemoveElement(element);
                    }
                }

                foreach(DS_Group group in groupsToDelete)
                {
                    List<DS_Node> groupNodes = new List<DS_Node>();
                    foreach (GraphElement element in group.containedElements)
                    {
                        if (element is DS_Node == true)
                        {
                            groupNodes.Add((DS_Node)element);
                        }
                    }
                    group.RemoveElements(groupNodes);
                    Remove_Group_FromDictionary(group);
                    RemoveElement(group);
                }

                DeleteElements(edgesToDelete);

                foreach(DS_Node node in nodesToDelete)
                {
                    if (node.Group != null)
                    {
                        Remove_Node_FromGroup(node, node.Group);
                        Add_Node_ToUngrouped(node);
                    }
                    Remove_Node_FromUngrouped(node);
                    node.DisconnectAllPorts();
                    RemoveElement(node);
                }
            };
        }
        private void UpdateElementsAddedToGroup()
        {
            elementsAddedToGroup = (group, elements) =>
            {
                Logger.Error("Elements addition to group callback");
                foreach (GraphElement element in elements)
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
                Logger.Error("Elements removed from group callback");
                foreach (GraphElement element in elements)
                {
                    if (!(element is DS_Node)) continue;
                    else
                    {
                        DS_Group nodeGroup = (DS_Group)group;
                        DS_Node node = (DS_Node)element;
                        if(Remove_Node_FromGroup(node, nodeGroup) == false) return;
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

                dS_Group.title = newTitle.RemoveWhitespaces().RemoveSpecialCharacters();

                if (string.IsNullOrEmpty(dS_Group.title))
                {
                    if (string.IsNullOrEmpty(dS_Group.oldTitle) == false)
                    {
                        NameErrorsAmount++;
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(dS_Group.oldTitle) == true)
                    {
                        NameErrorsAmount--;
                    }
                }

                dS_Group.oldTitle = dS_Group.title;
                Add_Group_ToDictionary(dS_Group);
            };
        }
        private void UpdateGraphViewChanged()
        {
            graphViewChanged = (changes) =>
            {
                if (changes.edgesToCreate != null)
                {
                    foreach (Edge edge in changes.edgesToCreate)
                    {
                        DS_Node nextNode = (DS_Node)edge.input.node;
                        DS_ChoiceData choiceData = (DS_ChoiceData)edge.output.userData;

                        choiceData.NodeID = nextNode.ID;
                    }
                }

                if (changes.elementsToRemove != null)
                {
                    foreach (GraphElement element in changes.elementsToRemove)
                    {
                        if (element.GetType() == typeof(Edge))
                        {
                            Edge edge = (Edge)element;
                            DS_ChoiceData choiceData = (DS_ChoiceData)edge.output.userData;
                            choiceData.NodeID = "";
                        }
                    }
                }
            return changes;
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
                CreateGroup("DialogueGroup", WorldToLocalMousePosition(actionEvent.eventInfo.localMousePosition))));
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
                AddElement(CreateNode("DialogueName", WorldToLocalMousePosition(actionEvent.eventInfo.localMousePosition), dialogueType))));
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
        public DS_Node CreateNode(string nodeName, Vector2 spawnPosition, DS_DialogueType dialogueType, bool shouldDraw = true)
        {
            Type nodeType = Type.GetType($"DS.Elements.DS_{dialogueType}Node"); //Generating the Type variable according to the indicated Name.
            DS_Node node = (DS_Node) Activator.CreateInstance(nodeType);

            node.Initialize(nodeName, this, spawnPosition);
            if(shouldDraw == true) node.Draw();

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
            DS_Group group = new DS_Group(groupName.RemoveWhitespaces().RemoveSpecialCharacters(), localMousePosition);
            Add_Group_ToDictionary(group);

            AddElement(group);

            foreach(GraphElement selectedElement in selection)
            {
                if(selectedElement is DS_Node == true)
                {
                    DS_Node node = (DS_Node)selectedElement;
                    group.AddElement(node);

                }
            }

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

        private void AddMinimap()
        {
            throw new NotImplementedException();
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

        public void ClearGraph()
        {
            graphElements.ForEach(graphElement => RemoveElement(graphElement));
            groups.Clear();
            groupedNodes.Clear();
            ungroupedNodes.Clear();
            nameErrorsAmount = 0;
        }
        #endregion



        #region Dictionaries handling
        /// <summary>
        /// Add the passed node to the ungrouped node dictionary.
        /// </summary>
        /// <param name="node">The node that need to be added to the dictionary.</param>
        public void Add_Node_ToUngrouped(DS_Node node)
        {
            Logger.Error("Ungrouped addition");
            string nodeName = node.DialogueName.ToLower();

            if (ungroupedNodes.ContainsKey(nodeName) == false)
            {
                DS_NodeErrorData nodeErrorData = new();
                nodeErrorData.Nodes.Add(node);

                ungroupedNodes.Add(nodeName, nodeErrorData);
                Logger.Message($"NEW KEY: [{nodeName}] COUNT: [{nodeErrorData.Nodes.Count}]");
            }
            else
            {
                ungroupedNodes[nodeName].Nodes.Add(node);
                Color groupErrorColor = ungroupedNodes[nodeName].ErrorData.ErrorColor;

                Logger.Message($"UNGROUPED NODE ADDED TO KEY: [{nodeName}] COUNT: [{ungroupedNodes[nodeName].Nodes.Count}]");

                node.SetErrorStyle(groupErrorColor);
                if (ungroupedNodes[nodeName].Nodes.Count == 2)
                {
                    ++NameErrorsAmount; 
                    ungroupedNodes[nodeName].Nodes[0].SetErrorStyle(groupErrorColor);
                }
            }

            Logger.Error("Ungrouped recap");
            foreach (var key in ungroupedNodes.Keys)
            {
                Logger.Warning($"KEY:[{key}] COUNT:[{ungroupedNodes[key].Nodes.Count}]");
            }
        }
        /// <summary>
        /// Remove an ungrouped node from its ungrouped node list.
        /// </summary>
        /// <param name="node">Node to remove from ungrouped nodes.</param>
        public void Remove_Node_FromUngrouped(DS_Node node)
        {
            Logger.Error("Ungrouped removing");

            string nodeName = node.DialogueName.ToLower();
            List<DS_Node> nodeList = ungroupedNodes[nodeName].Nodes;

            Logger.Message($"IN KEY: [{nodeName}] / COUNT: [{ungroupedNodes[nodeName].Nodes.Count}]");
            nodeList.Remove(node);
            node.ResetStyle();

            Logger.Message($"NEW COUNT: [{ungroupedNodes[nodeName].Nodes.Count}]");
            if (nodeList.Count == 1)
            {
                --NameErrorsAmount;
                nodeList[0].ResetStyle(); 
                return;
            }
            if (nodeList.Count == 0)
            {
                Logger.Warning($"UNGROUPED REMOVED KEY: {nodeName}");
                ungroupedNodes.Remove(nodeName); return;
            }

            Logger.Error("Ungrouped recap");
            foreach (var key in ungroupedNodes.Keys)
            {
                Logger.Warning($"KEY:[{key}] COUNT:[{ungroupedNodes[key].Nodes.Count}]");
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
            Logger.Error("Grouped nodes");
            string nodeName = node.DialogueName.ToLower();
            node.SetGroup(group);

            if (groupedNodes.ContainsKey(group) == false)
            {
                Logger.Warning($"ADDING KEY [[{group.name}][{group.title}]");
                var innerDictionary = new SerializableDictionary<string, DS_NodeErrorData>();

                groupedNodes.Add(group, innerDictionary);   
            }

            if (groupedNodes[group].ContainsKey(nodeName) == false)
            {
                Logger.Warning($"ADD INNER-DICT TO KEY [[{group.name}][{group.title}]");

                DS_NodeErrorData nodeErrorData = new DS_NodeErrorData();
                nodeErrorData.Nodes.Add(node);
                groupedNodes[group].Add(nodeName, nodeErrorData);
                Logger.Message($"Node added as new: [{nodeName}] COUNT: [{groupedNodes[group][nodeName].Nodes.Count}]");
                return;
            }
            else
            {
                Logger.Warning($"ADD NODE TO GROUPED KEY [[{group.name}][{group.title}] WITH INNER-DICT key: [{nodeName}] COUNT: [{groupedNodes[group][nodeName].Nodes.Count}]");
                List<DS_Node> groupedNodeList = groupedNodes[group][nodeName].Nodes;
                groupedNodes[group][nodeName].Nodes.Add(node);
                Color errorColor = groupedNodes[group][nodeName].ErrorData.ErrorColor;
                node.SetErrorStyle(errorColor);

                Logger.Message($"NEW COUNT: [{groupedNodes[group][nodeName].Nodes.Count}]");
                if (groupedNodeList.Count == 2)
                {
                    ++NameErrorsAmount;
                    groupedNodeList[0].SetErrorStyle(errorColor);
                }  
            }
            Logger.Error("Grouped nodes recap");
            foreach (var key in groupedNodes.Keys)
            {
                Logger.Warning($"KEY:[{key}]");
                foreach (var key2 in groupedNodes[key].Keys)
                {
                    Logger.Warning($"Inner key:[{key2}] + COUNT:[{groupedNodes[key][key2].Nodes.Count}]");
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <param name="group"></param>
        public bool Remove_Node_FromGroup(DS_Node node, DS_Group group)
        {
            Logger.Error("Groupednodes");
            string nodeName = node.DialogueName.ToLower();
            node.RemoveFromGroup();

            if (groupedNodes.ContainsKey(group) == false) 
            {
                return false;
            }
            List<DS_Node> groupedNodesList = groupedNodes[group][nodeName].Nodes;
            groupedNodesList.Remove(node);

            Logger.Warning($"REMOVING NODE [{nodeName}] IN GROUPED KEY [{group.name}/{group.title}]");
            node.ResetStyle();

            if (groupedNodesList.Count == 1)
            {
                --NameErrorsAmount;
                groupedNodesList[0].ResetStyle();              
            }
            if (groupedNodesList.Count == 0) 
            {
                groupedNodes[group].Remove(nodeName);
                Logger.Warning($"REMOVING INNER KEY [{nodeName}]");
            } 
            if (groupedNodes[group].Count == 0) 
            {
                groupedNodes.Remove(group);
                Logger.Warning($"REMOVING GROUP KEY [{group.name}/{group.title}]");
            }

            Logger.Error("Grouped nodes recap");
            foreach (var key in groupedNodes.Keys)
            {
                Logger.Warning($"GROUP KEY:[{key}]");
                foreach (var key2 in groupedNodes[key].Keys)
                {
                    Logger.Warning($"INNER NODENAME KEY:[{key2}]  +  COUNT:[{groupedNodes[key][key2].Nodes.Count}]");
                }
            }
            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="group"></param>
        public void Add_Group_ToDictionary(DS_Group group)
        {
            Logger.Error("Groups addition");
            string groupTitle = group.title.ToLower();

            if(groups.ContainsKey(groupTitle) == false)
            {
                DS_GroupErrorData groupErrorData = new DS_GroupErrorData();
                groupErrorData.Groups.Add(group);
                groups.Add(groupTitle, groupErrorData);

                Logger.Message($"NEW KEY: {groupTitle}  / COUNT: {groups[groupTitle].Groups.Count}");

            }
            else 
            {
                List<DS_Group> groupList = groups[groupTitle].Groups;
                groupList.Add(group);
                Color errorColor = groups[groupTitle].ErrorData.ErrorColor;
                group.SetErrorStyle(errorColor);

                Logger.Message($"KEY:  {groupTitle}  /  COUNT: {groups[groupTitle].Groups.Count}");

                if (groupList.Count == 2)
                {
                    ++NameErrorsAmount;
                    groupList[0].SetErrorStyle(errorColor);
                }
            }
            Logger.Error("Groups recap");
            foreach (var key in groups.Keys)
            {
                Logger.Warning($"KEY:[{key}] + COUNT:[{groups[key].Groups.Count}]");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="group"></param>
        public void Remove_Group_FromDictionary(DS_Group group)
        {
            Logger.Error("Group removing");
            string groupTitle = group.oldTitle.ToLower();

            List<DS_Group> groupList = groups[groupTitle].Groups;
            Logger.Message($"KEY: {groupTitle} / COUNT BEFORE: {groups[groupTitle].Groups.Count}");

            groupList.Remove(group);
            group.ResetStyle();

            Logger.Message($"AFTER: {groups[groupTitle].Groups.Count}");

            if (groupList.Count == 1)
            {
                --NameErrorsAmount;
                groupList[0].ResetStyle();
            }

            else if(groupList.Count == 0)
            {
                groups.Remove(groupTitle);
                Logger.Warning($" DELETED KEY: {groupTitle}");
            }

            Logger.Error("Groups recap");
            foreach (var key in groups.Keys)
            {
                Logger.Warning($" KEY:[{key}] + COUNT:[{groups[key].Groups.Count}]");
            }
        }
        #endregion
    }
}