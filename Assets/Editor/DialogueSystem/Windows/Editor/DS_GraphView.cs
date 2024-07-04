using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Events;

namespace DS.Editor.Windows
{
    using Errors;
    using Data;
    using Elements;
    using Enums;
    using Editor.Utilities;
    using Runtime.Utilities;

    /// <summary>
    /// Dialogue system GraphView window class.
    /// </summary>
    public class DS_GraphView : GraphView
    {
        private DS_MainEditorWindow editorWindow; //Reference to the editor window class
        public DS_MainEditorWindow EditorWindow { get { return editorWindow; } private set { editorWindow = value; } }
        public DS_LenguageType GetEditorCurrentLenguage() { return editorWindow.currentLenguage; }
        private DS_SearchWindow searchWindow; //Reference to the search window owned class

        private GridBackground gridBackground;
        private MiniMap miniMap;

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


        private float viewMovementSpeed = 40.0f;
        public float ViewMovementSpeed
        {
            get; set;
        }

        private Dictionary<KeyCode, bool> keyStates = new Dictionary<KeyCode, bool>
    {
        { KeyCode.UpArrow, false },
        { KeyCode.LeftArrow, false },
        { KeyCode.DownArrow, false },
        { KeyCode.RightArrow, false }
    };
        public UnityEvent<DS_LenguageType> GraphLenguageChanged = new();

        public DS_GraphView(DS_MainEditorWindow editorWindow)
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
            Add_MinimapStyles();
            Add_Manipulators();

            this.editorWindow.EditorWindowLenguageChanged.AddListener(OnEditorLenguageChanged);
            RegisterCallback<KeyDownEvent>(OnKeyDown);
            RegisterCallback<KeyUpEvent>(OnKeyUp);

            schedule.Execute(UpdateViewPosition).Every(32);
        }

        #region Overrides

        private void UpdateViewPosition()
        {
            Vector3 newPosition = contentViewContainer.transform.position;           

            if (keyStates[KeyCode.UpArrow])
            {
                newPosition.y += viewMovementSpeed * Time.deltaTime;
            }
            if (keyStates[KeyCode.LeftArrow])
            {
                newPosition.x += viewMovementSpeed * Time.deltaTime;
            }
            if (keyStates[KeyCode.DownArrow])
            {
                newPosition.y -= viewMovementSpeed * Time.deltaTime;
            }
            if (keyStates[KeyCode.RightArrow])
            {
                newPosition.x -= viewMovementSpeed * Time.deltaTime;
            }

            if (newPosition != contentViewContainer.transform.position)
            {
                contentViewContainer.transform.position = newPosition;
                contentViewContainer.MarkDirtyRepaint();
            }
        }

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
                DS_Logger.Error("Delete Selection callback");

                List<DS_Group> groupsToDelete = new List<DS_Group>();
                List<Edge> edgesToDelete = new List<Edge>();
                List<DS_BaseNode> nodesToDelete = new List<DS_BaseNode>();

                foreach(GraphElement element in selection)
                {
                    if(element is DS_BaseNode node)
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
                    List<DS_BaseNode> groupNodes = new List<DS_BaseNode>();
                    foreach (GraphElement element in group.containedElements)
                    {
                        if (element is DS_BaseNode == true)
                        {
                            groupNodes.Add((DS_BaseNode)element);
                        }
                    }
                    group.RemoveElements(groupNodes);
                    Remove_Group_FromDictionary(group);
                    RemoveElement(group);
                }

                DeleteElements(edgesToDelete);

                foreach(DS_BaseNode node in nodesToDelete)
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
                DS_Logger.Error("Elements addition to group callback");
                foreach (GraphElement element in elements)
                {
                    if ((element is DS_BaseNode) == false) continue;
                    else
                    {
                        DS_Group nodeGroup = (DS_Group)group; 
                        DS_BaseNode node = (DS_BaseNode)element;
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
                DS_Logger.Error("Elements removed from group callback");
                foreach (GraphElement element in elements)
                {
                    if (!(element is DS_BaseNode)) continue;
                    else
                    {
                        DS_Group nodeGroup = (DS_Group)group;
                        DS_BaseNode node = (DS_BaseNode)element;
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



                        DS_BaseNode nextNode = (DS_BaseNode)edge.input.node;
                        DS_ChoiceData choiceData = (DS_ChoiceData)edge.output.userData;
                        choiceData.NextNodeID = nextNode.ID;
                        DS_Logger.Warning($"Edge created between node: {((DS_BaseNode)edge.output.node).DialogueName} and node: {nextNode.DialogueName}");
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
                            choiceData.NextNodeID = "";
                        }
                    }
                }

                if(changes.movedElements != null)
                {
                    foreach(GraphElement element in changes.movedElements)
                    {
                        if(element.GetType() == typeof(DS_BaseNode))
                        {
                            AvoidNodeOverlap((DS_BaseNode)element);
                        }
                    }
                }
            return changes;
            };
        }

        #endregion

        #region Callbacks
        private void OnEditorLenguageChanged(DS_LenguageType newLenguage)
        {
            GraphLenguageChanged?.Invoke(newLenguage);
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
            //this.AddManipulator(new FreehandSelector());

            this.AddManipulator(CreateNode_CtxMenu_Option("Create Starting Node", DS_DialogueType.Start));
            this.AddManipulator(CreateNode_CtxMenu_Option("Create Node(Single Choice)", DS_DialogueType.SingleChoice));
            this.AddManipulator(CreateNode_CtxMenu_Option("Create Node(Multiple Choice)", DS_DialogueType.MultipleChoice));
            this.AddManipulator(CreateNode_CtxMenu_Option("Create Event Node", DS_DialogueType.Event));
            this.AddManipulator(CreateNode_CtxMenu_Option("Create End Node", DS_DialogueType.End));
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
        public DS_BaseNode CreateNode(string nodeName, Vector2 spawnPosition, DS_DialogueType dialogueType, bool shouldDraw = true)
        {
            Type nodeType = Type.GetType($"DS.Editor.Windows.Elements.DS_{dialogueType}Node"); //Generating the Type variable according to the indicated Name.
            DS_BaseNode node = (DS_BaseNode) Activator.CreateInstance(nodeType);
            node.Initialize(nodeName, this, spawnPosition);
            if(shouldDraw == true) node.Draw();

            Add_Node_ToUngrouped(node);

            AvoidNodeOverlap(node);
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
                if(selectedElement is DS_BaseNode == true)
                {
                    DS_BaseNode node = (DS_BaseNode)selectedElement;
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
            gridBackground = new GridBackground();
            gridBackground.StretchToParentSize();
            Insert(0, gridBackground);
        }

        private void AddMinimap()
        {
            miniMap = new MiniMap();
            miniMap.anchored = true;
            miniMap.SetPosition(new Rect(15, 50, 200, 180));
            Add(miniMap);
            miniMap.visible = false;
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


        private void Add_MinimapStyles()
        {
            StyleColor backgroundColor = new StyleColor(new Color32(29, 29, 30, 255));
            StyleColor borderColor = new StyleColor(new Color32(51, 51, 51, 255));

            miniMap.style.backgroundColor = backgroundColor;
            miniMap.style.borderTopColor = borderColor;
            miniMap.style.borderRightColor = borderColor;
            miniMap.style.borderLeftColor = borderColor;
            miniMap.style.borderBottomColor = borderColor;
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

        public void ToggleMinimap()
        {
            miniMap.visible = !miniMap.visible;
        }

        private void OnKeyDown(KeyDownEvent evt)
        {
            if (keyStates.ContainsKey(evt.keyCode))
            {
                keyStates[evt.keyCode] = true;
                evt.StopPropagation(); 
                evt.PreventDefault();
            }
        }

        private void OnKeyUp(KeyUpEvent evt)
        {
            if (keyStates.ContainsKey(evt.keyCode))
            {
                keyStates[evt.keyCode] = false;
                evt.StopPropagation();
                evt.PreventDefault();
            }
        }
        #endregion

        #region Dictionaries handling
        /// <summary>
        /// Add the passed node to the ungrouped node dictionary.
        /// </summary>
        /// <param name="node">The node that need to be added to the dictionary.</param>
        public void Add_Node_ToUngrouped(DS_BaseNode node)
        {
            DS_Logger.Error("Ungrouped addition");
            string nodeName = node.DialogueName.ToLower();

            if (ungroupedNodes.ContainsKey(nodeName) == false)
            {
                DS_NodeErrorData nodeErrorData = new();
                nodeErrorData.Nodes.Add(node);

                ungroupedNodes.Add(nodeName, nodeErrorData);
                DS_Logger.Message($"NEW KEY: [{nodeName}] COUNT: [{nodeErrorData.Nodes.Count}]");
            }
            else
            {
                ungroupedNodes[nodeName].Nodes.Add(node);
                Color groupErrorColor = ungroupedNodes[nodeName].ErrorData.Color;

                DS_Logger.Message($"UNGROUPED NODE ADDED TO KEY: [{nodeName}] COUNT: [{ungroupedNodes[nodeName].Nodes.Count}]");

                node.SetErrorStyle(groupErrorColor);
                if (ungroupedNodes[nodeName].Nodes.Count == 2)
                {
                    ++NameErrorsAmount; 
                    ((DS_BaseNode)ungroupedNodes[nodeName].Nodes[0]).SetErrorStyle(groupErrorColor);
                }
            }

            DS_Logger.Error("Ungrouped recap");
            foreach (var key in ungroupedNodes.Keys)
            {
                DS_Logger.Warning($"KEY:[{key}] COUNT:[{ungroupedNodes[key].Nodes.Count}]");
            }
        }
        /// <summary>
        /// Remove an ungrouped node from its ungrouped node list.
        /// </summary>
        /// <param name="node">Node to remove from ungrouped nodes.</param>
        public void Remove_Node_FromUngrouped(DS_BaseNode node)
        {
            DS_Logger.Error("Ungrouped removing");

            string nodeName = node.DialogueName.ToLower();
            List<Node> nodeList = ungroupedNodes[nodeName].Nodes;

            DS_Logger.Message($"IN KEY: [{nodeName}] / COUNT: [{ungroupedNodes[nodeName].Nodes.Count}]");
            nodeList.Remove(node);
            node.ResetStyle();

            DS_Logger.Message($"NEW COUNT: [{ungroupedNodes[nodeName].Nodes.Count}]");
            if (nodeList.Count == 1)
            {
                --NameErrorsAmount;
                ((DS_BaseNode)nodeList[0]).ResetStyle(); 
                return;
            }
            if (nodeList.Count == 0)
            {
                DS_Logger.Warning($"UNGROUPED REMOVED KEY: {nodeName}");
                ungroupedNodes.Remove(nodeName); return;
            }

            DS_Logger.Error("Ungrouped recap");
            foreach (var key in ungroupedNodes.Keys)
            {
                DS_Logger.Warning($"KEY:[{key}] COUNT:[{ungroupedNodes[key].Nodes.Count}]");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <param name="group"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void Add_Node_ToGroup(DS_BaseNode node, DS_Group group)
        {
            DS_Logger.Error("Grouped nodes");
            string nodeName = node.DialogueName.ToLower();
            node.SetGroup(group);

            if (groupedNodes.ContainsKey(group) == false)
            {
                DS_Logger.Warning($"ADDING KEY [[{group.name}][{group.title}]");
                var innerDictionary = new SerializableDictionary<string, DS_NodeErrorData>();

                groupedNodes.Add(group, innerDictionary);   
            }

            if (groupedNodes[group].ContainsKey(nodeName) == false)
            {
                DS_Logger.Warning($"ADD INNER-DICT TO KEY [[{group.name}][{group.title}]");

                DS_NodeErrorData nodeErrorData = new DS_NodeErrorData();
                nodeErrorData.Nodes.Add(node);
                groupedNodes[group].Add(nodeName, nodeErrorData);
                DS_Logger.Message($"Node added as new: [{nodeName}] COUNT: [{groupedNodes[group][nodeName].Nodes.Count}]");
                return;
            }
            else
            {
                DS_Logger.Warning($"ADD NODE TO GROUPED KEY [[{group.name}][{group.title}] WITH INNER-DICT key: [{nodeName}] COUNT: [{groupedNodes[group][nodeName].Nodes.Count}]");
                List<Node> groupedNodeList = groupedNodes[group][nodeName].Nodes;
                groupedNodes[group][nodeName].Nodes.Add(node);
                Color errorColor = groupedNodes[group][nodeName].ErrorData.Color;
                node.SetErrorStyle(errorColor);

                DS_Logger.Message($"NEW COUNT: [{groupedNodes[group][nodeName].Nodes.Count}]");
                if (groupedNodeList.Count == 2)
                {
                    ++NameErrorsAmount;
                    ((DS_BaseNode)groupedNodeList[0]).SetErrorStyle(errorColor);
                }  
            }
            DS_Logger.Error("Grouped nodes recap");
            foreach (var key in groupedNodes.Keys)
            {
                DS_Logger.Warning($"KEY:[{key}]");
                foreach (var key2 in groupedNodes[key].Keys)
                {
                    DS_Logger.Warning($"Inner key:[{key2}] + COUNT:[{groupedNodes[key][key2].Nodes.Count}]");
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <param name="group"></param>
        public bool Remove_Node_FromGroup(DS_BaseNode node, DS_Group group)
        {
            DS_Logger.Error("Groupednodes");
            string nodeName = node.DialogueName.ToLower();
            node.RemoveFromGroup();

            if (groupedNodes.ContainsKey(group) == false) 
            {
                return false;
            }
            List<Node> groupedNodesList = groupedNodes[group][nodeName].Nodes;
            groupedNodesList.Remove(node);

            DS_Logger.Warning($"REMOVING NODE [{nodeName}] IN GROUPED KEY [{group.name}/{group.title}]");
            node.ResetStyle();

            if (groupedNodesList.Count == 1)
            {
                --NameErrorsAmount;
                ((DS_BaseNode)groupedNodesList[0]).ResetStyle();              
            }
            if (groupedNodesList.Count == 0) 
            {
                groupedNodes[group].Remove(nodeName);
                DS_Logger.Warning($"REMOVING INNER KEY [{nodeName}]");
            } 
            if (groupedNodes[group].Count == 0) 
            {
                groupedNodes.Remove(group);
                DS_Logger.Warning($"REMOVING GROUP KEY [{group.name}/{group.title}]");
            }

            DS_Logger.Error("Grouped nodes recap");
            foreach (var key in groupedNodes.Keys)
            {
                DS_Logger.Warning($"GROUP KEY:[{key}]");
                foreach (var key2 in groupedNodes[key].Keys)
                {
                    DS_Logger.Warning($"INNER NODENAME KEY:[{key2}]  +  COUNT:[{groupedNodes[key][key2].Nodes.Count}]");
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
            DS_Logger.Error("Groups addition");
            string groupTitle = group.title.ToLower();

            if(groups.ContainsKey(groupTitle) == false)
            {
                DS_GroupErrorData groupErrorData = new DS_GroupErrorData();
                groupErrorData.Groups.Add(group);
                groups.Add(groupTitle, groupErrorData);

                DS_Logger.Message($"NEW KEY: {groupTitle}  / COUNT: {groups[groupTitle].Groups.Count}");

            }
            else 
            {
                List<Group> groupList = groups[groupTitle].Groups;
                groupList.Add(group);
                Color errorColor = groups[groupTitle].ErrorColor.Color;
                group.SetErrorStyle(errorColor);

                DS_Logger.Message($"KEY:  {groupTitle}  /  COUNT: {groups[groupTitle].Groups.Count}");

                if (groupList.Count == 2)
                {
                    ++NameErrorsAmount;
                    ((DS_Group)groupList[0]).SetErrorStyle(errorColor);
                }
            }
            DS_Logger.Error("Groups recap");
            foreach (var key in groups.Keys)
            {
                DS_Logger.Warning($"KEY:[{key}] + COUNT:[{groups[key].Groups.Count}]");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="group"></param>
        public void Remove_Group_FromDictionary(DS_Group group)
        {
            DS_Logger.Error("Group removing");
            string groupTitle = group.oldTitle.ToLower();

            List<Group> groupList = groups[groupTitle].Groups;
            DS_Logger.Message($"KEY: {groupTitle} / COUNT BEFORE: {groups[groupTitle].Groups.Count}");

            groupList.Remove(group);
            group.ResetStyle();

            DS_Logger.Message($"AFTER: {groups[groupTitle].Groups.Count}");

            if (groupList.Count == 1)
            {
                --NameErrorsAmount;
                ((DS_Group)groupList[0]).ResetStyle();
            }

            else if(groupList.Count == 0)
            {
                groups.Remove(groupTitle);
                DS_Logger.Warning($" DELETED KEY: {groupTitle}");
            }

            DS_Logger.Error("Groups recap");
            foreach (var key in groups.Keys)
            {
                DS_Logger.Warning($" KEY:[{key}] + COUNT:[{groups[key].Groups.Count}]");
            }
        }
        #endregion


        private void AvoidNodeOverlap(DS_BaseNode nodeA)
        {
            List<DS_BaseNode> otherNodes = new List<DS_BaseNode>(this.Query<DS_BaseNode>().ToList());
            otherNodes.Remove(nodeA);

            bool hasOverlap = false;
            do
            {
                hasOverlap = false;

                foreach (DS_BaseNode nodeB in otherNodes)
                {
                    if (nodeA.IsOverlapping(nodeB))
                    {
                        Debug.LogError("Node is overlapping");
                        ResolveOverlap(nodeA, nodeB);
                    }
                }
            } while (hasOverlap == true);
        }
        private void ResolveOverlap(Node nodeA, Node nodeB)
        {
            Rect rectA = nodeA.GetPosition();
            Rect rectB = nodeB.GetPosition();

            float overlapX = 0.0f;
            float overlapY = 0.0f;

            if (rectA.xMax < rectB.xMax && rectA.yMax < rectB.yMax)
            {
                if(Mathf.Abs(rectA.xMax - rectB.xMin) < Mathf.Abs(rectA.yMax - rectB.yMin))
                {
                    //push sx
                    overlapX = Mathf.Abs(rectA.xMax - rectB.xMin);
                    rectA.x -= overlapX;
                    Debug.LogWarning($"Overlapping movement {overlapX}");
                    Debug.LogError("push sx");
                }
                else
                {
                    //push down(up in inverse axis of the graphview)
                    overlapY = Mathf.Abs(rectA.yMax - rectB.yMin);
                    rectA.y -= overlapY;
                    Debug.LogWarning($"Overlapping movement {overlapY}");
                    Debug.LogError("push up");
                }
            }
            else if(rectA.xMax < rectB.xMax && rectA.yMax > rectB.yMax)
            {
                if(Mathf.Abs(rectA.xMax - rectB.xMin) < Mathf.Abs(rectA.yMin - rectB.yMax))
                {
                    //push sx
                    overlapX = Mathf.Abs(rectA.xMax - rectB.xMin);
                    rectA.x -= overlapX;
                    Debug.LogWarning($"Overlapping movement {overlapX}");
                    Debug.LogError("push sx");
                }
                else
                {
                    //push up
                    overlapY = Mathf.Abs(rectA.yMin - rectB.yMax);
                    rectA.y += overlapY;
                    Debug.LogWarning($"Overlapping movement {overlapY}");
                    Debug.LogError("push down");
                }
            }
            else if(rectA.xMax > rectB.xMax && rectA.yMax > rectB.yMax)
            {
                if(Mathf.Abs(rectA.xMin - rectB.xMax) < Mathf.Abs(rectA.yMin - rectB.yMax))
                {
                    //push dx
                    overlapX = Mathf.Abs(rectA.xMin - rectB.xMax);
                    rectA.x += overlapX;
                    Debug.LogWarning($"Overlapping movement {overlapX}");
                    Debug.LogError("push dx");
                }
                else
                {
                    //push down
                    overlapY = Mathf.Abs(rectA.yMin - rectB.yMax);
                    rectA.y += overlapY;
                    Debug.LogWarning($"Overlapping movement {overlapY}");
                    Debug.LogError("push down");
                }
            }
            else if(rectA.xMax > rectB.xMax && rectA.yMax < rectB.yMax)
            {
                if(Mathf.Abs(rectA.xMin - rectB.xMax) < Mathf.Abs(rectA.yMax - rectB.yMin))
                {
                    //push dx
                    overlapX = Mathf.Abs(rectA.xMin - rectB.xMax);
                    rectA.x += overlapX;
                    Debug.LogWarning($"Overlapping movement {overlapX}");
                    Debug.LogError("push dx");
                }
                else
                {
                    //push up
                    overlapY = Mathf.Abs(rectA.yMax - rectB.yMin);
                    rectA.y -= overlapY;
                    Debug.LogWarning($"Overlapping movement {overlapY}");
                    Debug.LogError("push up");
                }
            }
            else if(rectA.xMax == rectB.xMax && rectA.yMax > rectB.yMax)
            {
                //push up
                overlapY = Mathf.Abs(rectA.yMin - rectB.yMax);
                rectA.y += overlapY;
                Debug.LogWarning($"Overlapping movement {overlapY}");
                Debug.LogError("push down");
            }
            else if(rectA.xMax == rectB.xMax && rectA.yMax < rectB.yMax)
            {
                //push down
                overlapY = Mathf.Abs(rectA.yMax - rectB.yMin);
                rectA.y -= overlapY;
                Debug.LogWarning($"Overlapping movement {overlapY}");
                Debug.LogError("push up");
            }
            else if(rectA.xMax < rectB.xMax && rectA.yMax == rectB.yMax)
            {
                //push sx
                overlapX = Mathf.Abs(rectA.xMax - rectB.xMin);
                rectA.x -= overlapX;
                Debug.LogWarning($"Overlapping movement {overlapX}");
                Debug.LogError("push sx");
            }
            else if(rectA.xMax > rectB.xMax && rectA.yMax == rectB.yMax)
            {
                //push dx
                overlapX = Mathf.Abs(rectA.xMin - rectB.xMax);
                rectA.x += overlapX;
                Debug.LogWarning($"Overlapping movement {overlapX}");
                Debug.LogError("push dx");
            }
            else if(rectA.xMax == rectB.xMax && rectA.yMax == rectB.yMax)
            {
                //push sx
                overlapX = rectA.width;
                rectA.x -= overlapX;
                Debug.LogWarning($"Overlapping movement {overlapX}");
                Debug.LogError("push sx");
            }
            nodeA.SetPosition(rectA);
            return;
        }
    }
}