using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using System.IO;
using UnityEditor.UIElements;
using UnityEngine.Events;

namespace DS.Editor.Windows
{
    using Enumerations;
    using Utilities;
    using Elements;

    /// <summary>
    /// 
    /// </summary>
    public class DS_MainEditorWindow : EditorWindow
    {
        protected DS_IOUtilities ioUtilities;
        protected Toolbar toolbar;



        private readonly DS_LenguageType defaultLenguage = DS_LenguageType.Italian;
        public DS_LenguageType currentLenguage;

        protected readonly string defaultSavedGraphPath = "Assets/Editor/DialogueSystem/Graphs";
        protected readonly string defaultFileName = "DialogueFileName";

        protected TextField filenameTextField;

        protected Button saveGraphButton;
        private Button loadButton;
        protected Button clearButton;
        private Button resetButton;
        protected Button toggleMinimapButton;

        protected ToolbarMenu toolbarMenu;

        protected DS_GraphView graph_View;

        public UnityEvent<DS_LenguageType> EditorWindowLenguageChanged = new();

        [MenuItem("DialogueSystem/Main_Editor_Window")]
        public static void Open()
        {
            DS_MainEditorWindow wnd = CreateInstance<DS_MainEditorWindow>();
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
            foreach(DS_LenguageType lenguage in (DS_LenguageType[])Enum.GetValues(typeof(DS_LenguageType)))
            {
                toolbarMenu.menu.AppendAction(lenguage.ToString(), callback => SelectLenguage(lenguage, toolbarMenu));
            }
            SelectLenguage(defaultLenguage, toolbarMenu);
            toolbar.Add(toolbarMenu);
        }

        protected Action<DropdownMenuAction> SelectLenguage(DS_LenguageType lenguage, ToolbarMenu toolbarMenu)
        {
            toolbarMenu.text = "Lenguage: " + lenguage.ToString();
            currentLenguage = lenguage;

            DS_Logger.Error("To implement lenguage selection!");
            EditorWindowLenguageChanged?.Invoke(currentLenguage);
            return null;
        }

        protected void AddGraphView()
        {
            ioUtilities = new DS_IOUtilities();
            graph_View = new DS_GraphView(this);
            graph_View.StretchToParentSize();
            rootVisualElement.Add(graph_View);
        }

        /// <summary>
        /// Load style sheet from resources and add that to the visual element.
        /// </summary>
        protected void AddStyles()
        {
            rootVisualElement.AddStyleSheet("DS_Variables.uss");
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void AddToolbar()
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
        protected void OnSaveButtonPressed()
        {
            if(string.IsNullOrEmpty(filenameTextField.value))
            {
                EditorUtility.DisplayDialog("Invalid file name.", "Please ensure the file name is not empty or invalid.", "Ok");
                return;
            }

            ioUtilities.Initialize(graph_View, filenameTextField.value);
            ioUtilities.SaveGraph();
        }

        private void OnLoadButtonPressed()
        {
            string filePath = EditorUtility.OpenFilePanel("Dialogue Graphs", "Assets/Editor/DialogueSystem/Graphs", "asset");
            if(string.IsNullOrEmpty(filePath) == false)
            {
                OnClearButtonPressed();
                ioUtilities.Initialize(graph_View, Path.GetFileNameWithoutExtension(filePath));
                ioUtilities.LoadGraph();
            }
        }

        protected void OnClearButtonPressed()
        {
            graph_View?.ClearGraph();
        }

        private void OnResetGraphButtonPressed()
        {
            OnClearButtonPressed();
            UpdateFilename(defaultFileName);
        }

        protected void OnToggleMinimapButtonPressed()
        {
            graph_View.ToggleMinimap();

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
