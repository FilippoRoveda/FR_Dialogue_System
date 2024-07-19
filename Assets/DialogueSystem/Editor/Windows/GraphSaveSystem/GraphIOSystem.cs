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

    public class GraphIOSystem
    {
        /// <summary>
        /// 
        /// </summary>
        public IOUtilities BaseIO;
        public GraphSave graphSave;
        public GraphLoad graphLoad;

        /// <summary>
        /// Reference to the current displayed graph in the Dialogue System Graph View Editor Window.
        /// </summary>
        public DS_GraphView graphView;
        /// <summary>
        /// Base assets path for all saved dialogues.
        /// </summary>
        //public static readonly string commonAssetsPath = "Assets/DialogueSystem/CreatedDialogues";
        /// <summary>
        /// Base editor assets path for all saved diallgues graphs.
        /// </summary>
        public static readonly string commonEditorPath = "Assets/Editor/Data/Graphs";


        public string graphFileName;
        public string containerFolderPath;

        public List<DS_Group> groups;

        public List<BaseNode> nodes;
        public List<EventNode> eventNodes;

        //public Dictionary<string, DialogueGroupSO> createdGroupsSO;
        //public Dictionary<string, BaseDialogueSO> createdDialoguesSO;

        public Dictionary<string, DS_Group> loadedGroups;

        public Dictionary<string, BaseNode> loadedNodes;

        /// <summary>
        /// Initialize the static IOUtilities class with informations for the current DS_Graph view created, displayed or loaded in the editor window.
        /// </summary>
        /// <param name="graphView"></param>
        /// <param name="graphName"></param>
        public void Initialize(DS_GraphView graphView, string graphName)
        {
            BaseIO = new IOUtilities();
            graphSave = new GraphSave(this);
            graphLoad = new GraphLoad(this);

            this.graphView = graphView;
            graphFileName = graphName;
            //containerFolderPath = commonAssetsPath + "/" + graphFileName;

            groups = new List<DS_Group>();
            nodes = new List<BaseNode>();
            eventNodes = new List<EventNode>();
            //createdGroupsSO = new Dictionary<string, DialogueGroupSO>();
            //createdDialoguesSO = new Dictionary<string, BaseDialogueSO>();

            loadedGroups = new Dictionary<string, DS_Group>();
            loadedNodes = new Dictionary<string, BaseNode>();
        }

        /// <summary>
        /// Save the current displayed DS_GraphView.
        /// </summary>
        public void SaveGraph()
        {
            CreateStaticFolders();
            GetElementsFromGraphView();

            GraphSO graphData = BaseIO.CreateAsset<GraphSO>(commonEditorPath, $"/{graphFileName}_Graph");
            graphData.Initialize(graphFileName);

            //DialogueContainerSO dialogueContainer = BaseIO.CreateAsset<DialogueContainerSO>(containerFolderPath, graphFileName);
            //dialogueContainer.Initialize(graphFileName);

            //graphSave.SaveGroups(graphData, dialogueContainer);
            //graphSave.SaveNodes(graphData, dialogueContainer);
            graphSave.SaveGroups(graphData);
            graphSave.SaveNodes(graphData);

            BaseIO.SaveAsset(graphData);
            //BaseIO.SaveAsset(dialogueContainer);
        }

        public void LoadGraph()
        {
            GraphSO graphData = BaseIO.LoadAsset<GraphSO>(commonEditorPath, graphFileName);
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
            graphView.EditorWindow.UpdateFilename(graphData.graphName);

            graphLoad.LoadGroups(graphData.groups);
            //LoadBranchNodes(graphData.BranchNodes);

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
            BaseIO.CreateFolder("Assets/Editor/Data", "Graphs");
            //BaseIO.CreateFolder("Assets", "DialogueSystem");
            //BaseIO.CreateFolder("Assets/DialogueSystem", "CreatedDialogues");
            //BaseIO.CreateFolder("Assets/DialogueSystem/CreatedDialogues", graphFileName);
           // BaseIO.CreateFolder(containerFolderPath, "Global");
            //BaseIO.CreateFolder(containerFolderPath, "Groups");
            //BaseIO.CreateFolder($"{containerFolderPath}/Global", "Dialogues");

        }
        #endregion

        #region Utility methods

        public void GetElementsFromGraphView()
        {
            graphView.graphElements.ForEach(FetchGraphElements());
        }
        
        public Action<GraphElement> FetchGraphElements()
        {
            return graphElement =>
            {
                if (graphElement.GetType() == typeof(SingleNode))
                {
                    nodes.Add((SingleNode)graphElement);
                }
                else if (graphElement.GetType() == typeof(MultipleNode))
                {
                    nodes.Add((MultipleNode)graphElement);
                }
                else if (graphElement.GetType() == typeof(StartNode))
                {
                    nodes.Add((StartNode)graphElement);
                }
                else if (graphElement.GetType() == typeof(EventNode))
                {
                    eventNodes.Add((EventNode)graphElement);
                }
                else if (graphElement.GetType() == typeof(EndNode))
                {
                    nodes.Add((EndNode)graphElement);
                }
                else if (graphElement.GetType() == typeof(BranchNode))
                {
                    nodes.Add((BranchNode)graphElement);
                }
                else if (graphElement.GetType() == typeof(DS_Group))
                {
                    groups.Add((DS_Group)graphElement);
                }
            };
        }
     

        public DS_Group GetLoadedGroup(string groupID)
        {
            if(loadedGroups.ContainsKey(groupID))
            {
                return loadedGroups[groupID];
            }
            else
            {
                Logger.Error($"No group with ID:{groupID} is currently loaded in the graph.", Color.red);
                return null;
            }
        }
        #endregion
    }
}
