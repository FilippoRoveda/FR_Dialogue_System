using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace DialogueSystem.Windows
{
    public class DS_Graph_View : GraphView
    {
        public DS_Graph_View()
        {
            AddGridBackground();
            AddStyle();
        }

        private void AddGridBackground()
        {
            GridBackground gridBackground = new GridBackground();
            gridBackground.StretchToParentSize();
            Insert(0, gridBackground);
        }

        private void AddStyle()
        {
            StyleSheet style = (StyleSheet) EditorGUIUtility.Load("DialogueSystem/GridBackground.uss");
            styleSheets.Add(style);
        }
    }
}