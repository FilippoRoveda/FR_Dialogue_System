using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using System.IO;
using UnityEditor.UIElements;
using UnityEngine.Events;

namespace DS.Editor.Windows
{
    using Variables.Editor;

    using Editor.Enumerations;
    using Editor.Utilities;
    using Windows.Utilities;
    using Editor.Elements;

    /// <summary>
    /// 
    /// </summary>
    public class DS_EditorWindow : EditorWindow
    {
        protected readonly string defaultSavedGraphPath = "Assets/Editor/Data/Graphs";
        protected readonly string defaultFileName = "DialogueFileName";

        private readonly LenguageType defaultLenguage = LenguageType.Italian;
        public LenguageType currentLenguage;

        #region Window Elements
        protected Toolbar toolbar;
        protected TextField filenameTextField;
        protected Button saveGraphButton;
        protected Button clearButton;
        protected Button toggleMinimapButton;
        protected Button openVariableEditor;
        protected VariableEditorWindow variableEditorWindow;

        protected ToolbarMenu toolbarMenu;
        private Button loadButton;
        private Button resetButton;
        #endregion

        protected DS_GraphView linkedGraph;
        protected GraphSystem graphSystem;

        bool isVariableEditorOpen = false;

        public UnityEvent<LenguageType> EditorLenguageChanged = new();

        [MenuItem("DialogueSystem/Editor Window")]
        public static void Open()
        {
            DS_EditorWindow wnd = CreateInstance<DS_EditorWindow>();
            wnd.titleContent = new GUIContent("DS_Main_Editor_Window");          
            wnd.Show();
        }


        protected virtual void CreateGUI()
        {
            AddGraphView();
            AddToolbar();
            AddToolbarMenu();
        }

        #region Elements addition
        protected void AddToolbarMenu()
        {
            toolbarMenu = new ToolbarMenu();
            foreach(LenguageType lenguage in (LenguageType[])System.Enum.GetValues(typeof(LenguageType)))
            {
                toolbarMenu.menu.AppendAction(lenguage.ToString(), callback => OnSelectLenguage(lenguage, toolbarMenu));
            }
            OnSelectLenguage(defaultLenguage, toolbarMenu);
            toolbar.Add(toolbarMenu);
        }



        protected void AddGraphView()
        {
            graphSystem = new GraphSystem();
            linkedGraph = new DS_GraphView(this);
            linkedGraph.StretchToParentSize();
            rootVisualElement.Add(linkedGraph);
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void AddToolbar()
        {
            toolbar = new Toolbar();
            filenameTextField = ElementsUtilities.CreateTextField(defaultFileName, "File Name:", callback => OnFilenameChanged(callback));
            saveGraphButton = ElementsUtilities.CreateButton("Save", () => OnSaveButtonPressed());
            loadButton = ElementsUtilities.CreateButton("Load", () => OnLoadButtonPressed());
            clearButton = ElementsUtilities.CreateButton("Clear", () => OnClearButtonPressed());
            resetButton = ElementsUtilities.CreateButton("Reset", () => OnResetGraphButtonPressed());
            openVariableEditor = ElementsUtilities.CreateButton("Variable Editor", () => OnVariableEditorButtonPressed());
            toggleMinimapButton = ElementsUtilities.CreateButton("Toggle Minimap", () => OnToggleMinimapButtonPressed());


            toolbar.Add(filenameTextField);
            toolbar.Add(saveGraphButton);
            toolbar.Add(loadButton);
            toolbar.Add(clearButton);
            toolbar.Add(resetButton);
            toolbar.Add(openVariableEditor);
            toolbar.Add(toggleMinimapButton);

            toolbar.AddStyleSheet("DS_ToolbarStyles.uss");

            rootVisualElement.Add(toolbar);
        }
        #endregion

        #region Callbacks
        protected Action<DropdownMenuAction> OnSelectLenguage(LenguageType lenguage, ToolbarMenu toolbarMenu)
        {
            toolbarMenu.text = "Lenguage: " + lenguage.ToString();
            currentLenguage = lenguage;
            EditorLenguageChanged?.Invoke(currentLenguage);
            return null;
        }
        protected void OnSaveButtonPressed()
        {
            if(string.IsNullOrEmpty(filenameTextField.value))
            {
                EditorUtility.DisplayDialog("Invalid file name.", "Please ensure the file name is not empty or invalid.", "Ok");
                return;
            }

            graphSystem.Initialize(linkedGraph, filenameTextField.value);
            graphSystem.SaveGraph();
        }

        private void OnLoadButtonPressed()
        {
            string filePath = EditorUtility.OpenFilePanel("Dialogue Graphs", defaultSavedGraphPath, "asset");
            if(string.IsNullOrEmpty(filePath) == false)
            {
                OnClearButtonPressed();
                graphSystem.Initialize(linkedGraph, Path.GetFileNameWithoutExtension(filePath));
                graphSystem.LoadGraph();
            }
        }

        protected void OnClearButtonPressed()
        {
            linkedGraph?.ClearGraph();
        }

        private void OnResetGraphButtonPressed()
        {
            OnClearButtonPressed();
            UpdateFilename(defaultFileName);
        }

        protected void OnToggleMinimapButtonPressed()
        {
            linkedGraph.ToggleMinimap();

            toggleMinimapButton.ToggleInClassList("ds-toolbar_button_selected");
        }

        protected void OnVariableEditorButtonPressed()
        {
            if (isVariableEditorOpen == true)
            {
                isVariableEditorOpen = false;
                variableEditorWindow.Close();
                variableEditorWindow = null;
            }
            else
            {
                variableEditorWindow = VariableEditorWindow.OpenWindowInGraphView<DS_EditorWindow>();
                isVariableEditorOpen = true;
            }
        }
        #endregion

        #region Utilities
        public void UpdateFilename(string newFilename)
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
