using UnityEditor;

namespace DS.Inspectors
{
    using Data.Save;

    [CustomEditor(typeof(DS_GraphSO))]
    public class DS_GraphSO_CustomInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            EditorGUI.BeginDisabledGroup(true);
            DrawDefaultInspector();
            EditorGUI.EndDisabledGroup();
        }
    }
}
