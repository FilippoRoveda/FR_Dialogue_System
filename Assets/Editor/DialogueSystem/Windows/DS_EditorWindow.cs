using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace DS.Windows
{
    using System;
    using UnityEditor.UIElements;
    using Utilities;

    /// <summary>
    /// 
    /// </summary>
    public class DS_EditorWindow : EditorWindow
    {
        private readonly string defaultFileName = "DialogueFileName";
        private TextField filenameTextField;
        private Button saveGraphButton;

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
            Toolbar toolbar = new Toolbar();
            filenameTextField = DS_ElementsUtilities.CreateTextField(defaultFileName, "File Name:", callback => OnFilenameChanged(callback));
            saveGraphButton = DS_ElementsUtilities.CreateButton("Save", () => Save());

            toolbar.Add(filenameTextField);
            toolbar.Add(saveGraphButton);

            toolbar.AddStyleSheet("DS_ToolbarStyles.uss");

            rootVisualElement.Add(toolbar);
        }

        private void Save()
        {
            if(string.IsNullOrEmpty(filenameTextField.value))
            {
                EditorUtility.DisplayDialog("Invalid file name.", "Please ensure the file name is not empty or invalid.", "Ok");
                return;
            }

            DS_IOUtilities.Initialize(graph_View, filenameTextField.value);
            DS_IOUtilities.SaveGraph();
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
    }
}
