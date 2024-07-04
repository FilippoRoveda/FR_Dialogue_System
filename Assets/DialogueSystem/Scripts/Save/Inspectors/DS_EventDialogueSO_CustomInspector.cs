using UnityEditor;

namespace DS.Runtime.Inspectors
{
    using ScriptableObjects;

    [CustomEditor(typeof(DS_EventDialogueSO))]
    public class DS_EventDialogueSO_CustomInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            EditorGUI.BeginDisabledGroup(true);
            DrawDefaultInspector();
            EditorGUI.EndDisabledGroup();
        }
    }
}
