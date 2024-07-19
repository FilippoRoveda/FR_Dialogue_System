using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace DS.Editor.Windows
{
    using Editor.Enumerations;
    using Editor.Elements;

    /// <summary>
    /// 
    /// </summary>
    public class DS_SearchWindow : ScriptableObject, ISearchWindowProvider
    {
        private DS_GraphView graphView;
        private Texture2D indentationTexture;

        public void Initialize(DS_GraphView dS_GraphView)
        {
            this.graphView = dS_GraphView;
            indentationTexture = new Texture2D(1, 1);
            indentationTexture.SetPixel(0, 0, Color.clear);
            indentationTexture.Apply();
        }

        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            List<SearchTreeEntry> searchTreeEntries = new List<SearchTreeEntry>()
            {
                new SearchTreeGroupEntry(new GUIContent("Create element")),
                new SearchTreeGroupEntry(new GUIContent("Dialogue Node"), 1),
                new SearchTreeEntry(new GUIContent("Start Node", indentationTexture))
                {
                    level = 2, userData = NodeType.Start
                },
                new SearchTreeEntry(new GUIContent("Single Choice", indentationTexture))
                {
                    level = 2, userData = NodeType.Single
                },
                new SearchTreeEntry(new GUIContent("Multiple Choice", indentationTexture))
                {
                    level = 2, userData = NodeType.Multiple
                },
                new SearchTreeEntry(new GUIContent("Event Node", indentationTexture))
                {
                    level = 2, userData = NodeType.Event
                },
                new SearchTreeEntry(new GUIContent("End Node", indentationTexture))
                {
                    level = 2, userData = NodeType.End
                },
                new SearchTreeGroupEntry(new GUIContent("Dialogue Group"), 1),
                new SearchTreeEntry(new GUIContent("Single Group", indentationTexture))
                {
                    level = 2, userData = new Group()
                },
            };
            return searchTreeEntries;
        }

        public bool OnSelectEntry(SearchTreeEntry SearchTreeEntry, SearchWindowContext context)
        {
            Vector2 localMousePosition = graphView.WorldToLocalMousePosition(context.screenMousePosition, true);

            switch (SearchTreeEntry.userData) 
            {
                case NodeType.Start:
                    StartNode startNode = (StartNode)graphView.CreateNode("StartNode", localMousePosition, NodeType.Start);
                    graphView.AddElement(startNode);
                    return true;
                case NodeType.Single:
                    SingleNode singleChoiceNode = (SingleNode) graphView.CreateNode("DialogueName", localMousePosition, NodeType.Single);
                    graphView.AddElement(singleChoiceNode);
                    return true;
                case NodeType.Multiple:
                    MultipleNode multipleChoiceNode = (MultipleNode) graphView.CreateNode("DialogueName", localMousePosition, NodeType.Multiple);
                    graphView.AddElement(multipleChoiceNode);
                    return true;
                case NodeType.Event:
                    EventNode eventNode = (EventNode)graphView.CreateNode("EventNode", localMousePosition, NodeType.Event);
                    graphView.AddElement(eventNode);
                    return true;
                case NodeType.End:
                    EndNode endNode = (EndNode)graphView.CreateNode("EndNode", localMousePosition, NodeType.End);
                    graphView.AddElement(endNode);
                    return true;
                case Group _:
                     graphView.CreateGroup("DialogueGroup", localMousePosition);
                    return true;
                default:
                    return false;
            }
        }
    }
}
