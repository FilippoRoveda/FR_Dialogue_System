using UnityEditor;

namespace DS.Editor.Inspectors
{
    using ScriptableObjects;

    [CustomEditor(typeof(DS_GraphSO))]
    public class GraphSO_Inspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            EditorGUI.BeginDisabledGroup(true);
            DrawDefaultInspector();
            EditorGUI.EndDisabledGroup();
        }
    }
}
