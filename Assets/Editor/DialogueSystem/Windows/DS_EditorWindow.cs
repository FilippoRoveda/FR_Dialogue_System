using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace DialogueSystem.Windows
{
    public class DS_EditorWindow : EditorWindow
    {
        [MenuItem("DialogueSystem/Editor_Window")]
        public static void Open()
        {
            DS_EditorWindow wnd = GetWindow<DS_EditorWindow>();
            wnd.titleContent = new GUIContent("DS_Editor_Window");
        }

        private void CreateGUI()
        {
            AddGraphView();
            AddStyle();
        }

        private void AddGraphView()
        {
            DS_GraphView graph_View = new DS_GraphView();
            graph_View.StretchToParentSize();
            rootVisualElement.Add(graph_View);
        }

        /// <summary>
        /// Load style sheet from resources and add that to the visual element.
        /// </summary>
        private void AddStyle()
        {
            StyleSheet variablesTable = (StyleSheet)EditorGUIUtility.Load("DialogueSystem/DS_Variables.uss");
            rootVisualElement.styleSheets.Add(variablesTable);
        }
    }
}
