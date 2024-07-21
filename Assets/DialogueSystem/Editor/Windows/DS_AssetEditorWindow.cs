using UnityEngine;
using System.IO;
using UnityEditor.UIElements;

namespace DS.Editor.Windows
{
    using Elements;
    using Editor.ScriptableObjects;
    using Editor.Utilities;

    public class DS_AssetEditorWindow : DS_EditorWindow
    {
        public GraphSO assetGraph = null;


        public static void OpenWindow(GraphSO asset)
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
            titleContent = new GUIContent($"DS_{assetGraph.graphName}_Window");
        }
        private void SetAsset(GraphSO assetGraph)
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
            string filePath = $"{defaultSavedGraphPath}/{assetGraph.graphName}_Graph.asset";
            if (string.IsNullOrEmpty(filePath) == false)
            {
                OnClearButtonPressed();
                saveSystem.Initialize(linkedGraphView, Path.GetFileNameWithoutExtension(filePath));
                saveSystem.LoadGraph();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void AddToolbar()
        {
            toolbar = new Toolbar();
            filenameTextField = ElementsUtilities.CreateTextField(assetGraph.graphName, "File Name:");
            saveGraphButton = ElementsUtilities.CreateButton("Save", () => OnSaveButtonPressed());
            clearButton = ElementsUtilities.CreateButton("Clear", () => OnClearButtonPressed());
            openVariableEditor = ElementsUtilities.CreateButton("Variable Editor", () => OnVariableEditorButtonPressed());
            toggleMinimapButton = ElementsUtilities.CreateButton("Toggle Minimap", () => OnToggleMinimapButtonPressed());


            toolbar.Add(filenameTextField);
            toolbar.Add(saveGraphButton);
            toolbar.Add(clearButton);
            toolbar.Add(openVariableEditor);

            toolbar.Add(toggleMinimapButton);

            toolbar.AddStyleSheet("DS_ToolbarStyles.uss");

            rootVisualElement.Add(toolbar);
        }
    }
}
