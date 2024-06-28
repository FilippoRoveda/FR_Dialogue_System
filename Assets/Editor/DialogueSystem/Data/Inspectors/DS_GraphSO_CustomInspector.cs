using UnityEditor;

namespace DS.Editor.Inspectors
{
    using ScriptableObjects;

    [CustomEditor(typeof(DS_GraphSO))]
    public class DS_GraphSO_CustomInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            EditorGUI.BeginDisabledGroup(true);
            DrawDefaultInspector();
            EditorGUI.EndDisabledGroup();
        }
    }
}
