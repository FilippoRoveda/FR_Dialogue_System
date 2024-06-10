using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace DS.Windows
{
    using System;
    using System.IO;
    using UnityEditor.UIElements;
    using Utilities;

    /// <summary>
    /// 
    /// </summary>
    public class DS_EditorWindow : EditorWindow
    {
        private Toolbar toolbar;

        private readonly string defaultFileName = "DialogueFileName";

        private static TextField filenameTextField;

        private Button saveGraphButton;
        private Button loadButton;
        private Button clearButton;
        private Button resetButton;
        private Button toggleMinimapButton;

        private DS_GraphView graph_View;


        [MenuItem("DialogueSystem/Editor_Window")]
        public static void Open()
        {
            DS_EditorWindow wnd = GetWindow<DS_EditorWindow>();
            wnd.titleContent = new GUIContent("DS_Editor_Window");
        }

        private void CreateGUI()
        {
            AddGraphView();
            AddToolbar();
            AddStyles();
        }

        private void AddGraphView()
        {
            graph_View = new DS_GraphView(this);
            graph_View.StretchToParentSize();
            rootVisualElement.Add(graph_View);
        }

        /// <summary>
        /// Load style sheet from resources and add that to the visual element.
        /// </summary>
        private void AddStyles()
        {
            rootVisualElement.AddStyleSheet("DS_Variables.uss");
        }

        /// <summary>
        /// 
        /// </summary>
        private void AddToolbar()
        {
            toolbar = new Toolbar();
            filenameTextField = DS_ElementsUtilities.CreateTextField(defaultFileName, "File Name:", callback => OnFilenameChanged(callback));
            saveGraphButton = DS_ElementsUtilities.CreateButton("Save", () => OnSaveButtonPressed());
            loadButton = DS_ElementsUtilities.CreateButton("Load", () => OnLoadButtonPressed());
            clearButton = DS_ElementsUtilities.CreateButton("Clear", () => OnClearButtonPressed());
            resetButton = DS_ElementsUtilities.CreateButton("Reset", () => OnResetGraphButtonPressed());
            toggleMinimapButton = DS_ElementsUtilities.CreateButton("Toggle Minimap", () => OnToggleMinimapButtonPressed());


            toolbar.Add(filenameTextField);
            toolbar.Add(saveGraphButton);
            toolbar.Add(loadButton);
            toolbar.Add(clearButton);
            toolbar.Add(resetButton);
            toolbar.Add(toggleMinimapButton);

            toolbar.AddStyleSheet("DS_ToolbarStyles.uss");

            rootVisualElement.Add(toolbar);
        }




        #region Callbacks
        private void OnSaveButtonPressed()
        {
            if(string.IsNullOrEmpty(filenameTextField.value))
            {
                EditorUtility.DisplayDialog("Invalid file name.", "Please ensure the file name is not empty or invalid.", "Ok");
                return;
            }

            DS_IOUtilities.Initialize(graph_View, filenameTextField.value);
            DS_IOUtilities.SaveGraph();
        }

        private void OnLoadButtonPressed()
        {
            string filePath = EditorUtility.OpenFilePanel("Dialogue Graphs", "Assets/Editor/DialogueSystem/Graphs", "asset");
            if(string.IsNullOrEmpty(filePath) == false)
            {
                OnClearButtonPressed();
                DS_IOUtilities.Initialize(graph_View, Path.GetFileNameWithoutExtension(filePath));
                DS_IOUtilities.LoadGraph();
            }
        }

        private void OnClearButtonPressed()
        {
            graph_View?.ClearGraph();
        }

        private void OnResetGraphButtonPressed()
        {
            OnClearButtonPressed();
            UpdateFilename(defaultFileName);
        }

        private void OnToggleMinimapButtonPressed()
        {
            graph_View.ToggleMinimap();

            toggleMinimapButton.ToggleInClassList("ds-toolbar_button_selected");
        }
        #endregion

        #region Utilities
        public static void UpdateFilename(string newFilename)
        {
            filenameTextField.value = newFilename;
        }

        private void OnFilenameChanged(ChangeEvent<string> callback)
        {
            filenameTextField.value = callback.newValue.RemoveWhitespaces().RemoveSpecialCharacters();
        }

        public void EnableSaving()
        {
            saveGraphButton.SetEnabled(true);
        }
        public void DisableSaving()
        {
            saveGraphButton.SetEnabled(false);
        }
        #endregion
    }
}
