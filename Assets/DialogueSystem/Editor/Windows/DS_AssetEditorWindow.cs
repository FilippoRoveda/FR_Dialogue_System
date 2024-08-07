using UnityEngine;
using System.IO;
using UnityEditor.UIElements;

namespace DS.Editor.Windows
{
    using Elements;
    using Editor.ScriptableObjects;
    using Editor.Utilities;

    /// <summary>
    /// DialogueEditor window for specific assets opening.
    /// </summary>
    public class DS_AssetEditorWindow : DS_EditorWindow
    {
        private GraphSO linkedAssetsGraph = null;

        public static void OpenWindow(GraphSO asset)
        {
            var windows = Resources.FindObjectsOfTypeAll<DS_AssetEditorWindow>();
            foreach (var window in windows)
            {
                if (window.linkedAssetsGraph == asset)
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


        protected override void CreateGUI()
        {
            AddGraphView();
            AddToolbar();
            AddToolbarMenu();
            LoadTargetGraphAsset();
        }


        /// <summary>
        /// 
        /// </summary>
        protected override void AddToolbar()
        {
            toolbar = new Toolbar();
            filenameTextField = ElementsUtilities.CreateTextField(linkedAssetsGraph.graphName, "File Name:");
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


        private void LoadTargetGraphAsset()
        {
            string filePath = $"{defaultSavedGraphPath}/{linkedAssetsGraph.graphName}_Graph.asset";
            if (string.IsNullOrEmpty(filePath) == false)
            {
                OnClearButtonPressed();
                graphSystem.Initialize(linkedGraph, Path.GetFileNameWithoutExtension(filePath));
                graphSystem.LoadGraph();
            }
        }

        private void SetTitleContent()
        {
            titleContent = new GUIContent($"DS_{linkedAssetsGraph.graphName}_Window");
        }
        private void SetAsset(GraphSO assetGraph)
        {
            linkedAssetsGraph = assetGraph;
        }
    }
}
