using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Converter.Editor
{
    using DS.Editor.ScriptableObjects;

    public class ConverterWindow : EditorWindow
    {
        private IOUtilities IO = new IOUtilities();
        private readonly string graphFolderPath = "Assets/Editor/Data/Graphs";

        private List<GraphSO> _graphs = new List<GraphSO>();
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
                ConvertAllGraphs();
            }

            GUILayout.Space(10);

            _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);

            foreach (var graph in _graphs)
            {
                EditorGUILayout.BeginHorizontal();


                EditorGUILayout.ObjectField("", graph, typeof(GraphSO), allowSceneObjects: false);
                if (GUILayout.Button(graph.graphName, GUILayout.ExpandWidth(true), GUILayout.MaxWidth(200)))
                {
                    _selectedGraph = graph;
                }
                if (GUILayout.Button("Convert", GUILayout.Width(60)))
                {
                    ConvertGraph(graph);
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
        #endregion

        private void ConvertAllGraphs() 
        { 
            foreach (var graph in _graphs) 
            {
                Converter converter = new();
                converter.Initialize(graph, graph.graphName);
                converter.ConvertGraph();
            }
        }
        private void ConvertGraph(GraphSO graph) 
        {
            Converter converter = new();
            converter.Initialize(graph, graph.graphName);
            converter.ConvertGraph();
        }
        private void LoadAllGraphs()
        {
            _graphs.Clear();
            List<string> allGraphs = IO.ListAssetsInFolder(graphFolderPath);
            foreach (string graph in allGraphs)
            {
                var graphSO = IO.LoadAsset<GraphSO>(graphFolderPath, graph);
                _graphs.Add(graphSO);
            }
        }
    }
}
