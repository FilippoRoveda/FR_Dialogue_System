using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditor.Experimental.GraphView;


namespace DS.Utilities
{
    using Elements;
    using System;
    using Windows;

    public static class DS_IO_Utilities
    {
        private static DS_GraphView graphView;
        private static string graphFileName;
        private static string containerFolderPath;

        private static List<DS_Group> groups;
        private static List<DS_Node> nodes;

        public static void Initialize(DS_GraphView graphView, string graphName)
        {
            DS_IO_Utilities.graphView = graphView;
            graphFileName = graphName;
            containerFolderPath = $"Assets/DialogueSystem/Dialogues/{graphFileName}";

            groups = new List<DS_Group>();
            nodes = new List<DS_Node>();
        }

        #region Save methods
        public static void SaveGraph()
        {
            CreateStaticFolders();
            GetElementsFromGraphView();
            CreateAsset();
        }

        #endregion

        #region Creation methods
        private static void CreateStaticFolders()
        {
            CreateFolders("Assets/Editor/DialogueSystem", "Graphs");
            CreateFolders("Assets", "DialogueSystem");
            CreateFolders("Assets/DialogueSystem", "Dialogues");
            CreateFolders("Assets/DialogueSystem/Dialogues", graphFileName);
            CreateFolders(containerFolderPath, "Global");
            CreateFolders(containerFolderPath, "Groups");
            CreateFolders($"{containerFolderPath}/Global", "Dialogues");

        }
        #endregion

        #region Utility methods
        private static void CreateFolders(string path, string folderName)
        {
            if (AssetDatabase.IsValidFolder($"{path}/{folderName}") == true) return;
            else AssetDatabase.CreateFolder(path, folderName);
            
        }
        private static void GetElementsFromGraphView()
        {
            graphView.graphElements.ForEach(FetchGraphElements());
        }
        private static Action<GraphElement> FetchGraphElements()
        {
            return graphElement =>
            {
                if (graphElement.GetType() == typeof(DS_Node))
                {
                    nodes.Add((DS_Node)graphElement);
                }
                else if (graphElement.GetType() == typeof(DS_Group))
                {
                    groups.Add((DS_Group)graphElement);
                }
            };
        }


        private static T CreateAsset<T>(string path, string assetName) where T : ScriptableObject
        {
            string fullPath = $"{path}/{assetName}.asset";
            T asset = ScriptableObject.CreateInstance<T>();
            AssetDatabase.CreateAsset(asset, fullPath);
            return asset;
        }
        #endregion
    }
}
