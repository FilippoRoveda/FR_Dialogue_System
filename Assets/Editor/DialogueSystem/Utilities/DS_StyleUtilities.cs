using UnityEditor;
using UnityEngine.UIElements;

namespace DialogueSystem.Utilities
{
    /// <summary>
    /// 
    /// </summary>
    public static class DS_StyleUtilities
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <param name="styleSheetNames"></param>
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
        /// 
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