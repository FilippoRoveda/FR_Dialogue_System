#if UNITY_EDITOR
using UnityEditor;

namespace DS.Runtime.Inspectors
{
    using ScriptableObjects;

    [CustomEditor(typeof(EndDialogueSO))]
    public class EndDialogueSO_Inspector : UnityEditor.Editor
    {
        // Start is called before the first frame update
        public override void OnInspectorGUI()
        {
            EditorGUI.BeginDisabledGroup(true);
            DrawDefaultInspector();
            EditorGUI.EndDisabledGroup();
        }
    }
}
#endif
