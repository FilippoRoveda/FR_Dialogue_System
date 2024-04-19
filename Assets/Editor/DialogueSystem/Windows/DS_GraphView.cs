using DialogueSystem.Eelements;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace DialogueSystem.Windows
{
    public class DS_GraphView : GraphView
    {
        public DS_GraphView()
        {
            AddGridBackground();
            AddStyle();
            AddManipulators();
            AddNode();
        }

        private void AddNode()
        {
            DS_Node node = new DS_Node();
            node.Initialize();
            node.Draw();
            AddElement(node);
        }

        private void AddManipulators()
        {
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
            this.AddManipulator(new ContentDragger());
        }

        /// <summary>
        /// Add GridBackground class to this graph view container.
        /// </summary>
        private void AddGridBackground()
        {
            GridBackground gridBackground = new GridBackground();
            gridBackground.StretchToParentSize();
            Insert(0, gridBackground);
        }

        /// <summary>
        /// Load style sheet from resources and add that to the graph view visual elemente.
        /// </summary>
        private void AddStyle()
        {
            StyleSheet style = (StyleSheet) EditorGUIUtility.Load("DialogueSystem/DS_GridBackground.uss");
            styleSheets.Add(style);
        }
    }
}