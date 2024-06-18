using UnityEditor;
using UnityEngine.UIElements;

namespace DS.Utilities
{
    /// <summary>
    /// Utilities class to facilitate common style .uss related operations.
    /// </summary>
    public static class DS_StyleUtilities
    {
        /// <summary>
        /// Add a series of stylesheet for this visual element.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="styleSheetNames">Names of the sheet to add.</param>
        public static VisualElement AddStyleSheet(this VisualElement element, params string [] styleSheetNames)
        {
            foreach( string styleSheet in styleSheetNames)
            {
                StyleSheet sheet = (StyleSheet)EditorGUIUtility.Load($"DialogueSystem/{styleSheet}");
                element.styleSheets.Add(sheet);
            }
            return element;
        }

        /// <summary>
        /// Add this visual element to a series of Style class list.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="classNames"></param>
        /// <returns></returns>
        public static VisualElement AddToClassLists(this VisualElement element, params string[] classNames)
        {
            foreach(string className in classNames)
            {
                element.AddToClassList(className);
            }
            return element;
        }
    }
}