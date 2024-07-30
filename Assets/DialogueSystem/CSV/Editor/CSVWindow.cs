using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CSVPlugin
{
    using DS.Editor.Utilities;
    using DS.Editor.ScriptableObjects;

    /// <summary>
    /// CSV Editor window class that handle Save/Load operation between Graphs ans CSV files.
    /// </summary>
    public class CSVWindow : EditorWindow
    {
        public static readonly string CSVFilesPath = "Assets/Editor/Data/CSV/";
        public static readonly string graphFilesPath = "Assets/Editor/Data/Graphs";

        public static readonly string generateDataFolderName = "Data";
        public static readonly string generatedCSVFolderName = "CSV";
        public static readonly string generatedGraphsFolderName = "Graphs";

        readonly IOUtilities ioUtility = new();

        private List<GraphSO> allGrphsSO = new();
        private List<TextAsset> allCSV = new();
        List<string> csvToDelete = new();
        private TextAsset csvFile;

        private GraphSO _selectedGraph;

        private Vector2 graphPreviewPos;
        private Vector2 csvPreviewPos;


        [MenuItem("DialogueSystem/CSV")]
        public static void ShowWindow()
        {
            GetWindow<CSVWindow>("CSV Editor");
        }

        #region Unity Callbacks
        private void OnEnable()
        {
            LoadAllGraphs();
            LoadAllCSVTextFiles();
        }
        private void OnGUI()
        {
            EditorGUILayout.BeginHorizontal(GUILayout.MaxWidth(435));

            GUI.color = Color.green;
            if (GUILayout.Button("Save All CSVs", GUILayout.MaxWidth(200), GUILayout.MinHeight(50)))
            {
                SaveAllCSV();
            }
            GUI.color = Color.yellow;
            if (GUILayout.Button("Reload All CSVs", GUILayout.MaxWidth(200), GUILayout.MinHeight(50)))
            {
                LoadAllCSV();
            }
            GUI.color = Color.green;
            if (GUILayout.Button("Update Lenguages", GUILayout.MaxWidth(200), GUILayout.MinHeight(50)))
            {
                UpdateLenguages();
            }
            GUI.color = Color.white;
            EditorGUILayout.EndHorizontal();
            GUILayout.Space(10);

            graphPreviewPos = EditorGUILayout.BeginScrollView(graphPreviewPos, GUILayout.MinHeight(200));

            foreach (var graph in allGrphsSO)
            {
                EditorGUILayout.BeginHorizontal(GUILayout.MaxWidth(320));

                EditorGUILayout.ObjectField("", graph, typeof(GraphSO), allowSceneObjects: false);
                if (GUILayout.Button(graph.graphName, GUILayout.ExpandWidth(true), GUILayout.MaxWidth(200)))
                {
                    _selectedGraph = graph;
                }
                GUI.color = Color.green;
                if (GUILayout.Button("Save", GUILayout.Width(60)))
                {
                    SaveCSV(graph);
                    LoadAllCSVTextFiles();
                }
                GUI.color = Color.yellow;
                if (GUILayout.Button("Load", GUILayout.Width(60)))
                {
                    LoadCSV(graph);
                }
                GUILayout.FlexibleSpace();
                GUI.color = Color.white;
                EditorGUILayout.EndHorizontal();

                csvFile = allCSV.Find(x => x.name == graph.graphName);

                if (csvFile != null)
                {
                    EditorGUILayout.BeginHorizontal(GUILayout.MaxWidth(433));
                    EditorGUILayout.ObjectField("", csvFile, typeof(TextAsset), allowSceneObjects: false, GUILayout.MaxWidth(205));

                    GUILayout.FlexibleSpace();
                    GUI.color = Color.red;
                    if (GUILayout.Button("Delete", GUILayout.Width(60)))
                    {
                        csvToDelete.Add(graph.graphName);
                    }
                    GUI.color = Color.white;
                    EditorGUILayout.EndHorizontal();
                }
                csvFile = null;
                GUILayout.Space(10);
            }
            EditorGUILayout.EndScrollView();

            GUILayout.Space(10);

            if (_selectedGraph != null && allCSV.Find(x => x.name == _selectedGraph.graphName) != null)
            {
                csvPreviewPos = EditorGUILayout.BeginScrollView(csvPreviewPos);
                EditorGUILayout.LabelField("Selected CSV", EditorStyles.boldLabel);
                Editor editor2 = Editor.CreateEditor(allCSV.Find(x => x.name == _selectedGraph.graphName));
                editor2.OnInspectorGUI();
                EditorGUILayout.EndScrollView();
            }

            if (csvToDelete.Count != 0)
            {
                foreach (var csv in csvToDelete)
                {
                    AssetDatabase.DeleteAsset($"{CSVFilesPath}{csv}.csv");
                }
                csvToDelete.Clear();
                LoadAllCSVTextFiles();
            }
        }
        #endregion
      
        #region Callbacks
        private void LoadCSV(GraphSO graph)
        {
            LoadCSV loadCSV = new LoadCSV();
            bool errorFlag = false;
            loadCSV.LoadCSVInGraph(graph, out errorFlag);

            if (errorFlag)
            {
                EditorApplication.Beep();
                EditorApplication.Beep();
                Debug.Log("<color=red> All CSV file had been loaded to all DS_GraphSO objects but some problems had happened during the loading phase. </color>");
            }
            else
            {
                EditorApplication.Beep();
                Debug.Log("<color=green> All CSV file had been loaded to all DS_GraphSO objects! Every Graph is now updated. </color>");
            }
        }
        public static void LoadAllCSV()
        {
            LoadCSV loadCSV = new LoadCSV();
            bool errorFlag = loadCSV.LoadAllCSVInGraphs();

            if (errorFlag)
            {
                EditorApplication.Beep();
                EditorApplication.Beep();
                Debug.Log("<color=red> All CSV file had been loaded to all DS_GraphSO objects but some problems had happened during the loading phase. </color>");
            }
            else
            {
                EditorApplication.Beep();
                Debug.Log("<color=green> All CSV file had been loaded to all DS_GraphSO objects! Every Graph is now updated. </color>");
            }
        }
        private void SaveCSV(GraphSO graph)
        {
            SaveCSV saveCSV = new SaveCSV();
            saveCSV.Initalize();
            saveCSV.SaveGraphToCSV(graph);

            EditorApplication.Beep();
            Debug.Log("<color=green> Graph had been saved to CSV files! </color>");
        }
        public static void SaveAllCSV()
        {
            SaveCSV saveCSV = new SaveCSV();
            saveCSV.Initalize();
            saveCSV.SaveAllGraphsToCSV();

            EditorApplication.Beep();
            Debug.Log("<color=green> All graph had been saved to CSV files! </color>");
        }
        public static void UpdateLenguages()
        {
            CSVLenguageHelper helper = new CSVLenguageHelper();
            helper.UpdateLenguages();

            EditorApplication.Beep();
            Debug.Log("<color=green> Update all dialogues lenguages completed! </color>");
        }
        #endregion

        #region Utilities

        private void LoadAllGraphs()
        {
            allGrphsSO.Clear();
            foreach (string graph in ioUtility.ListAssetsInFolder(graphFilesPath))
            {
                var graphSO = ioUtility.LoadAsset<GraphSO>(graphFilesPath, graph);
                allGrphsSO.Add(graphSO);
            }
        }
        private void LoadAllCSVTextFiles()
        {
            allCSV.Clear();
            foreach (string csv in ioUtility.ListAssetsInFolder("Assets/Editor/Data/CSV"))
            {
                this.allCSV.Add((TextAsset)AssetDatabase.LoadAssetAtPath($"{CSVFilesPath}{csv}.csv", typeof(TextAsset)));
            }
        }
        #endregion
    }
}
