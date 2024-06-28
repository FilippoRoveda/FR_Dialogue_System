using UnityEditor;

namespace DS.Runtime.Inspectors
{
    using ScriptableObjects;

    [CustomEditor(typeof(DS_DialogueContainerSO))]
    public class DS_DialogueContainerSO_CustomInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            EditorGUI.BeginDisabledGroup(true);
            DrawDefaultInspector();
            EditorGUI.EndDisabledGroup();
        }
    }
}

