using UnityEditor;
using UnityEngine;
using UnityEditor.Callbacks;

namespace DS.Windows
{
    using DS.Data.Save;
    using DS.Utilities;
    using System;
    using System.IO;
    using UnityEditor.UIElements;

    public class DS_GraphAssetEditorWindow : DS_EditorWindow
    {
        private static DS_GraphSO assetGraph;

        [OnOpenAsset(1)]
        public static bool OpenAssetEditorWindow(Int32 _instanceID)
        {
            Object item = EditorUtility.InstanceIDToObject(_instanceID);

            if (item is DS_GraphSO)
            {
                assetGraph = item as DS_GraphSO;

                DS_GraphAssetEditorWindow wnd = GetWindow<DS_GraphAssetEditorWindow>();
                wnd.titleContent = new GUIContent($"DS_{assetGraph.GraphName}_Window");
                return true;
            }
            else return false;
        }

        protected override void CreateGUI()
        {
            AddGraphView();
            AddToolbar();
            AddStyles();
            string filePath = $"Assets/Editor/DialogueSystem/Graphs/{assetGraph.GraphName}_Graph.asset";
            if (string.IsNullOrEmpty(filePath) == false)
            {
                OnClearButtonPressed();
                DS_IOUtilities.Initialize(graph_View, Path.GetFileNameWithoutExtension(filePath));
                DS_IOUtilities.LoadGraph();
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

        #region Callbacks
        private void OnSaveButtonPressed()
        {
            if (string.IsNullOrEmpty(filenameTextField.value))
            {
                EditorUtility.DisplayDialog("Invalid file name.", "Please ensure the file name is not empty or invalid.", "Ok");
                return;
            }

            DS_IOUtilities.Initialize(graph_View, filenameTextField.value);
            DS_IOUtilities.SaveGraph();
        }
        #endregion
    }
}
