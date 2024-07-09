using UnityEngine;
using System.IO;
using UnityEditor.UIElements;

namespace DS.Editor.Windows
{
    using Editor.ScriptableObjects;
    using Editor.Utilities;
    using Elements;

    public class DS_AssetEditorWindow : DS_EditorWindow
    {
        public DS_GraphSO assetGraph = null;


        public static void OpenWindow(DS_GraphSO asset)
        {
            var windows = Resources.FindObjectsOfTypeAll<DS_AssetEditorWindow>();
            foreach (var window in windows)
            {
                if (window.assetGraph == asset)
                {
                    window.Focus();
                    return;
                }
            }

            DS_AssetEditorWindow windowInstance = CreateInstance<DS_AssetEditorWindow>();
            windowInstance.SetAsset(asset);
            windowInstance.SetTitleContent();
            windowInstance.Show();
        }

       
        private void SetTitleContent()
        {
            titleContent = new GUIContent($"DS_{assetGraph.GraphName}_Window");
        }
        private void SetAsset(DS_GraphSO assetGraph)
        {
            this.assetGraph = assetGraph;
        }

        protected override void CreateGUI()
        {
            AddGraphView();
            AddToolbar();
            AddToolbarMenu();
            AddStyles();
            LoadTargetGraphAsset();
        }

        private void LoadTargetGraphAsset()
        {
            string filePath = $"{defaultSavedGraphPath}/{assetGraph.GraphName}_Graph.asset";
            if (string.IsNullOrEmpty(filePath) == false)
            {
                OnClearButtonPressed();
                ioUtilities.Initialize(graph_View, Path.GetFileNameWithoutExtension(filePath));
                ioUtilities.LoadGraph();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void AddToolbar()
        {
            toolbar = new Toolbar();
            filenameTextField = DS_ElementsUtilities.CreateTextField(assetGraph.GraphName, "File Name:");
            saveGraphButton = DS_ElementsUtilities.CreateButton("Save", () => OnSaveButtonPressed());
            clearButton = DS_ElementsUtilities.CreateButton("Clear", () => OnClearButtonPressed());
            toggleMinimapButton = DS_ElementsUtilities.CreateButton("Toggle Minimap", () => OnToggleMinimapButtonPressed());


            toolbar.Add(filenameTextField);
            toolbar.Add(saveGraphButton);
            toolbar.Add(clearButton);
            toolbar.Add(toggleMinimapButton);

            toolbar.AddStyleSheet("DS_ToolbarStyles.uss");

            rootVisualElement.Add(toolbar);
        }
    }
}
