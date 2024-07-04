using UnityEditor;

namespace DS.Runtime.Inspectors
{
    using ScriptableObjects;

    [CustomEditor(typeof(DS_EndDialogueSO))]
    public class DS_EndDialogueSO_CusomInspector : UnityEditor.Editor
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
