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
        protected GraphIOSystem saveSystem;

        private readonly LenguageType defaultLenguage = LenguageType.Italian;
        public LenguageType currentLenguage;

        protected readonly string defaultSavedGraphPath = "Assets/Editor/Data/Graphs";
        protected readonly string defaultFileName = "DialogueFileName";

        protected Toolbar toolbar;
        protected TextField filenameTextField;
        protected Button saveGraphButton;
        protected Button clearButton;
        protected Button toggleMinimapButton;
        protected Button openVariableEditor;
        VariableEditorWindow variableEditorWindow;
        bool isVariableEditorOpen = false;
        protected ToolbarMenu toolbarMenu;

        private Button loadButton;
        private Button resetButton;

        protected DS_GraphView linkedGraphView;

        public UnityEvent<LenguageType> EditorWindowLenguageChanged = new();

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
            AddStyles();
        }

        protected void AddToolbarMenu()
        {
            toolbarMenu = new ToolbarMenu();
            foreach(LenguageType lenguage in (LenguageType[])System.Enum.GetValues(typeof(LenguageType)))
            {
                toolbarMenu.menu.AppendAction(lenguage.ToString(), callback => SelectLenguage(lenguage, toolbarMenu));
            }
            SelectLenguage(defaultLenguage, toolbarMenu);
            toolbar.Add(toolbarMenu);
        }

        protected Action<DropdownMenuAction> SelectLenguage(LenguageType lenguage, ToolbarMenu toolbarMenu)
        {
            toolbarMenu.text = "Lenguage: " + lenguage.ToString();
            currentLenguage = lenguage;
            Debug.Log("Changing lenguage");
            EditorWindowLenguageChanged?.Invoke(currentLenguage);
            return null;
        }

        protected void AddGraphView()
        {
            saveSystem = new GraphIOSystem();
            linkedGraphView = new DS_GraphView(this);
            linkedGraphView.StretchToParentSize();
            rootVisualElement.Add(linkedGraphView);
        }

        /// <summary>
        /// Load style sheet from resources and add that to the visual element.
        /// </summary>
        protected void AddStyles()
        {
            //rootVisualElement.AddStyleSheet("DS_Variables.uss");
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

        protected void OnVariableEditorButtonPressed()
        {
            if(isVariableEditorOpen == true)
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




        #region Callbacks
        protected void OnSaveButtonPressed()
        {
            if(string.IsNullOrEmpty(filenameTextField.value))
            {
                EditorUtility.DisplayDialog("Invalid file name.", "Please ensure the file name is not empty or invalid.", "Ok");
                return;
            }

            saveSystem.Initialize(linkedGraphView, filenameTextField.value);
            saveSystem.SaveGraph();
        }

        private void OnLoadButtonPressed()
        {
            string filePath = EditorUtility.OpenFilePanel("Dialogue Graphs", defaultSavedGraphPath, "asset");
            if(string.IsNullOrEmpty(filePath) == false)
            {
                OnClearButtonPressed();
                saveSystem.Initialize(linkedGraphView, Path.GetFileNameWithoutExtension(filePath));
                saveSystem.LoadGraph();
            }
        }

        protected void OnClearButtonPressed()
        {
            linkedGraphView?.ClearGraph();
        }

        private void OnResetGraphButtonPressed()
        {
            OnClearButtonPressed();
            UpdateFilename(defaultFileName);
        }

        protected void OnToggleMinimapButtonPressed()
        {
            linkedGraphView.ToggleMinimap();

            toggleMinimapButton.ToggleInClassList("ds-toolbar_button_selected");
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