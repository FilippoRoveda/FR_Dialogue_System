using UnityEditor;

namespace DS.Inspectors
{
    using ScriptableObjects;

    [CustomEditor(typeof(DS_DialogueSO))]
    public class DS_DialogueSO_CustomInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            EditorGUI.BeginDisabledGroup(true);
            DrawDefaultInspector();
            EditorGUI.EndDisabledGroup();
        }
    }
}
