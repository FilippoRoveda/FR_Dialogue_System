#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;


namespace Characters.Runtime
{
    using ScriptableObjects;
    [CustomEditor(typeof(CharacterSO))]
    public class CharacterSO_Inspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            CharacterSO characterSO = (CharacterSO)target;

            EditorGUI.BeginChangeCheck();

            GUI.enabled = false;
            EditorGUILayout.TextField("ID", characterSO.ID);
            GUI.enabled = true;

            string newName = EditorGUILayout.TextField("Name: ", characterSO.Name);
            string newCompleteName = EditorGUILayout.TextField("Complete Name: ", characterSO.CompleteName);
            Sprite newIcon = (Sprite)EditorGUILayout.ObjectField("Icon: ", characterSO.Icon, typeof(Sprite), allowSceneObjects: false, 
                                                            GUILayout.ExpandWidth(true), GUILayout.MaxWidth(300),
                                                            GUILayout.ExpandHeight(true), GUILayout.MaxHeight(300));

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(characterSO, $"Modified CharacterSO with id: {characterSO.ID}");

                if (newName != characterSO.Name)
                {
                    characterSO.Name = newName;
                }
                if (newCompleteName != characterSO.CompleteName)
                {
                    characterSO.CompleteName = newCompleteName;
                }
                if (newIcon != characterSO.Icon)
                {
                    characterSO.Icon = newIcon;
                }
                EditorUtility.SetDirty(characterSO);
            }
        }
    }
}
#endif
