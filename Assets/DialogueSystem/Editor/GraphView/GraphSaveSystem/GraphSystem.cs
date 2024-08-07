using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace DS.Editor.Windows.Utilities
{
    using Editor.ScriptableObjects;
    using Editor.Utilities;
    using Editor.Elements;

    public class GraphSystem
    {
        /// <summary>
        /// Base editor assets path for all saved diallgues graphs.
        /// </summary>
        public static readonly string commonEditorPath = "Assets/Editor/Data/Graphs";

        public IOUtilities IO;
        public GraphSave graphSave;
        public GraphLoad graphLoad;

        /// <summary>
        /// Reference to the current displayed graph in the Dialogue System Graph View Editor Window.
        /// </summary>
        public DS_GraphView linkedGraphView;

        public string graphFileName;
        public string containerFolderPath;


        #region Saving lists
        public List<DS_Group> groups;

        public List<DialogueNode> dialogueNodes;
        public List<EventNode> eventNodes;
        public List<EndNode> endNodes;
        public List<BranchNode> branchNodes;
        #endregion

        #region Loading lists
        public Dictionary<string, DS_Group> loadedGroups;

        public Dictionary<string, DialogueNode> loadedDialogueNodes;
        public Dictionary<string, EventNode> loadedEventNodes;
        public Dictionary<string, EndNode> loadedEndNodes;
        public Dictionary<string, BranchNode> loadedBranchNodes;
        #endregion

        /// <summary>
        /// Initialize the static IOUtilities class with informations for the current DS_Graph view created, displayed or loaded in the editor window.
        /// </summary>
        /// <param name="graphView"></param>
        /// <param name="graphName"></param>
        public void Initialize(DS_GraphView graphView, string graphName)
        {
            IO = new IOUtilities();
            graphSave = new GraphSave(this);
            graphLoad = new GraphLoad(this);

            linkedGraphView = graphView;
            graphFileName = graphName;
            
            groups = new List<DS_Group>();

            eventNodes = new List<EventNode>();
            endNodes = new List<EndNode>();
            dialogueNodes = new List<DialogueNode>();
            branchNodes = new List<BranchNode>();


            loadedGroups = new Dictionary<string, DS_Group>();
            loadedDialogueNodes = new Dictionary<string, DialogueNode>();
            loadedEventNodes = new Dictionary<string, EventNode>();
            loadedEndNodes = new Dictionary<string, EndNode>();
            loadedBranchNodes = new Dictionary<string, BranchNode>();
        }

        /// <summary>
        /// Save the current displayed DS_GraphView.
        /// </summary>
        public void SaveGraph()
        {
            CreateStaticFolders();
            GetElementsFromGraphView();

            GraphSO graphData = IO.CreateAsset<GraphSO>(commonEditorPath, $"/{graphFileName}_Graph");
            graphData.Initialize(graphFileName);

            graphSave.SaveGroups(graphData);
            graphSave.SaveNodes(graphData);

            IO.SaveAsset(graphData);
        }

        public void LoadGraph()
        {
            GraphSO graphData = IO.LoadAsset<GraphSO>(commonEditorPath, graphFileName);
            if(graphData == null)
            {
                EditorUtility.DisplayDialog(
                    "Could not load the file.",
                    "The file at the following path could not be found:\n\n" +
                    $"Assets/Editor/DialogueSystem/Graphs/{graphFileName}.",
                    "Ok"
                    );
                return;
            }
            linkedGraphView.EditorWindow.UpdateFilename(graphData.graphName);

            graphLoad.LoadGroups(graphData.groups);
            graphLoad.LoadBranchNodes(graphData.branchNodes);
            graphLoad.LoadDialogueNodes(graphData.dialogueNodes);
            graphLoad.LoadEventNodes(graphData.eventNodes);
            graphLoad.LoadEndNodes(graphData.endNodes);

            graphLoad.LoadNodesConnections();
        }


        #region Creation methods
        /// <summary>
        /// Create every needed directory, both assets and editor side, in which save the graph elements.
        /// </summary>
        private void CreateStaticFolders()
        {
            IO.CreateFolder("Assets/Editor/Data", "Graphs");
        }
        #endregion

        #region Utility methods

        public void GetElementsFromGraphView()
        {
            linkedGraphView.graphElements.ForEach(FetchGraphElements());
        }
        
        /// <summary>
        /// Fetch every GraphView element and add it to his specific list.
        /// </summary>
        /// <returns></returns>
        public Action<GraphElement> FetchGraphElements()
        {
            return graphElement =>
            {
                if (graphElement.GetType() == typeof(SingleNode))
                {
                    dialogueNodes.Add((SingleNode)graphElement);
                }
                else if (graphElement.GetType() == typeof(MultipleNode))
                {
                    dialogueNodes.Add((MultipleNode)graphElement);
                }
                else if (graphElement.GetType() == typeof(StartNode))
                {
                    dialogueNodes.Add((StartNode)graphElement);
                }
                else if (graphElement.GetType() == typeof(EventNode))
                {
                    eventNodes.Add((EventNode)graphElement);
                }
                else if (graphElement.GetType() == typeof(EndNode))
                {
                    endNodes.Add((EndNode)graphElement);
                }
                else if (graphElement.GetType() == typeof(BranchNode))
                {
                    branchNodes.Add((BranchNode)graphElement);
                }
                else if (graphElement.GetType() == typeof(DS_Group))
                {
                    groups.Add((DS_Group)graphElement);
                }
            };
        }
        #endregion
    }
}
