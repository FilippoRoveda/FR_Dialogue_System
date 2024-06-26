using UnityEditor;

namespace DS.Inspectors
{
    using ScriptableObjects;

    [CustomEditor(typeof(DS_DialogueContainerSO))]
    public class DS_DialogueContainerSO_CustomInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            EditorGUI.BeginDisabledGroup(true);
            DrawDefaultInspector();
            EditorGUI.EndDisabledGroup();
        }
    }
}

