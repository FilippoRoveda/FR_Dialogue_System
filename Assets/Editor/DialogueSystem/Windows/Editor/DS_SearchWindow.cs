using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace DS.Editor.Windows
{
    using Enums;
    using Elements;

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
                    level = 2, userData = DS_DialogueType.Start
                },
                new SearchTreeEntry(new GUIContent("Single Choice", indentationTexture))
                {
                    level = 2, userData = DS_DialogueType.SingleChoice
                },
                new SearchTreeEntry(new GUIContent("Multiple Choice", indentationTexture))
                {
                    level = 2, userData = DS_DialogueType.MultipleChoice
                },
                new SearchTreeEntry(new GUIContent("Event Node", indentationTexture))
                {
                    level = 2, userData = DS_DialogueType.Event
                },
                new SearchTreeEntry(new GUIContent("End Node", indentationTexture))
                {
                    level = 2, userData = DS_DialogueType.End
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
                case DS_DialogueType.Start:
                    DS_StartNode startNode = (DS_StartNode)graphView.CreateNode("StartNode", localMousePosition, DS_DialogueType.Start);
                    graphView.AddElement(startNode);
                    return true;
                case DS_DialogueType.SingleChoice:
                    DS_SingleChoiceNode singleChoiceNode = (DS_SingleChoiceNode) graphView.CreateNode("DialogueName", localMousePosition, DS_DialogueType.SingleChoice);
                    graphView.AddElement(singleChoiceNode);
                    return true;
                case DS_DialogueType.MultipleChoice:
                    DS_MultipleChoiceNode multipleChoiceNode = (DS_MultipleChoiceNode) graphView.CreateNode("DialogueName", localMousePosition, DS_DialogueType.MultipleChoice);
                    graphView.AddElement(multipleChoiceNode);
                    return true;
                case DS_DialogueType.Event:
                    DS_EventNode eventNode = (DS_EventNode)graphView.CreateNode("EventNode", localMousePosition, DS_DialogueType.Event);
                    graphView.AddElement(eventNode);
                    return true;
                case DS_DialogueType.End:
                    DS_EndNode endNode = (DS_EndNode)graphView.CreateNode("EndNode", localMousePosition, DS_DialogueType.End);
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
