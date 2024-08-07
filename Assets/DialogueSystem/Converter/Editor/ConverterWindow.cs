using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Converter.Editor
{
    using DS.Editor.ScriptableObjects;

    /// <summary>
    /// Converter editor class that convert Graph scriptables objects in to Dialogues scriptable objects to be played at runtime.
    /// </summary>
    public class ConverterWindow : EditorWindow
    {
        private IOUtilities IO = new IOUtilities();
        private readonly string graphFolderPath = "Assets/Editor/Data/Graphs";

        private List<GraphSO> allGrphsSO = new List<GraphSO>();
        private GraphSO _selectedGraph;
        private Vector2 _scrollPos;

        [MenuItem("DialogueSystem/Graph to Dialogue converter")]
        public static void ShowWindow()
        {
            GetWindow<ConverterWindow>("Graph to Dialogue converter");
        }

        #region Unity callbacks
        private void OnEnable()
        {
            LoadAllGraphs();
        }

        private void OnGUI()
        {
            GUILayout.Label("Converter editor", EditorStyles.boldLabel);

            if (GUILayout.Button("Convert All Graphs", GUILayout.MaxWidth(200)))
            {
                OnConvertAllGraphsButtonPressed();
            }

            GUILayout.Space(10);

            _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);

            foreach (var graph in allGrphsSO)
            {
                EditorGUILayout.BeginHorizontal();


                EditorGUILayout.ObjectField("", graph, typeof(GraphSO), allowSceneObjects: false);
                if (GUILayout.Button(graph._graphName, GUILayout.ExpandWidth(true), GUILayout.MaxWidth(200)))
                {
                    _selectedGraph = graph;
                }
                if (GUILayout.Button("Convert", GUILayout.Width(60)))
                {
                    OnConvertGraphButtonPressed(graph);
                }
                GUILayout.FlexibleSpace();


                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndScrollView();

            GUILayout.Space(10);

            if (_selectedGraph != null)
            {
                EditorGUILayout.LabelField("Selected Graph", EditorStyles.boldLabel);
                UnityEditor.Editor editor = UnityEditor.Editor.CreateEditor(_selectedGraph);
                editor.OnInspectorGUI();
            }
        }
        private void OnValidate()
        {
            LoadAllGraphs();
        }
        #endregion

        #region Callbakcs
        private void OnConvertAllGraphsButtonPressed() 
        { 
            foreach (var graphSO in allGrphsSO)
            {
                ConvertGraph(graphSO);
            }
        }

        private void OnConvertGraphButtonPressed(GraphSO graphSO) 
        {
            ConvertGraph(graphSO);
        }
        #endregion

        private void ConvertGraph(GraphSO graphSO)
        {
            Converter converter = new();        
            converter.Initialize(graphSO, graphSO._graphName);
            converter.ConvertGraph();
        }
        private void LoadAllGraphs()
        {
            allGrphsSO.Clear();
            List<string> allGraphs = IO.ListAssetsInFolder(graphFolderPath);
            foreach (string graph in allGraphs)
            {
                var graphSO = IO.LoadAsset<GraphSO>(graphFolderPath, graph);
                allGrphsSO.Add(graphSO);
            }
        }
    }
}
