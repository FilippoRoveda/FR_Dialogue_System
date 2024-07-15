#if UNITY_EDITOR
using UnityEditor;

namespace DS.Runtime.Inspectors
{
    using ScriptableObjects;

    [CustomEditor(typeof(DialogueSO))]
    public class DialogueSO_Inspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            EditorGUI.BeginDisabledGroup(true);
            DrawDefaultInspector();
            EditorGUI.EndDisabledGroup();
        }
    }
}
#endif