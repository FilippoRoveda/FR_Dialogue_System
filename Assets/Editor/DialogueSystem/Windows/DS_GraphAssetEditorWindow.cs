using UnityEngine;
using System.IO;
using UnityEditor.UIElements;

namespace DS.Windows
{
    using DS.Data.Save;
    using DS.Utilities;

    public class DS_GraphAssetEditorWindow : DS_MainEditorWindow
    {
        public DS_GraphSO assetGraph = null;


        public static void OpenWindow(DS_GraphSO asset)
        {
            var windows = Resources.FindObjectsOfTypeAll<DS_GraphAssetEditorWindow>();
            foreach (var window in windows)
            {
                if (window.assetGraph == asset)
                {
                    window.Focus();
                    return;
                }
            }

            DS_GraphAssetEditorWindow windowInstance = CreateInstance<DS_GraphAssetEditorWindow>();
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
            string filePath = $"Assets/Editor/DialogueSystem/Graphs/{assetGraph.GraphName}_Graph.asset";
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
