using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace DialogueSystem.Windows
{
    public class DS_Editor_Window : EditorWindow
    {
        [MenuItem("DialogueSystem/Editor_Window")]
        public static void Open()
        {
            DS_Editor_Window wnd = GetWindow<DS_Editor_Window>();
            wnd.titleContent = new GUIContent("DS_Editor_Window");
        }

        private void CreateGUI()
        {
            AddGraphView();
        }

        private void AddGraphView()
        {
            DS_Graph_View graph_View = new DS_Graph_View();
            graph_View.StretchToParentSize();
            rootVisualElement.Add(graph_View);
        }
    }
}
