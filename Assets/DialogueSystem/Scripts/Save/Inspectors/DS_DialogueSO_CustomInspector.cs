using UnityEditor;

namespace DS.Runtime.Inspectors
{
    using ScriptableObjects;

    [CustomEditor(typeof(DS_DialogueSO))]
    public class DS_DialogueSO_CustomInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            EditorGUI.BeginDisabledGroup(true);
            DrawDefaultInspector();
            EditorGUI.EndDisabledGroup();
        }
    }
}
