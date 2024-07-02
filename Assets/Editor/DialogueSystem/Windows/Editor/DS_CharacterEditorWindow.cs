using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DS.Editor.Windows
{
    using Runtime.ScriptableObjects;
    using UnityEngine.TextCore.Text;
    using Utilities;

    public class DS_CharacterEditorWindow : EditorWindow
    {
        private DS_IOUtilities IOUtilities = new DS_IOUtilities();
        private readonly string charactersFolderPath = "Assets/DialogueSystem/ScriptableObjects/Characters";


        private string newCharacterName = "NewCharacter";
        private string NewCharacterName
        {
            get { return newCharacterName + "_" + characters.Count.ToString(); }
        }

        private List<CharacterSO> characters = new List<CharacterSO>();
        private CharacterSO selectedCharacter;


        private Vector2 scrollPos;


        [MenuItem("DialogueSystem/Character Manager")]
        public static void ShowWindow()
        {
            GetWindow<DS_CharacterEditorWindow>("Character Editor");
        }

        #region Unity callbacks
        private void OnEnable()
        {
            LoadAllCharacters();
        }

        private void OnGUI()
        {
            GUILayout.Label("Character Editor", EditorStyles.boldLabel);

            if (GUILayout.Button("Create New Character", GUILayout.MaxWidth(200)))
            {
                CreateNewCharacter();
            }

            GUILayout.Space(10);

            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

            foreach (CharacterSO character in characters)
            {
                EditorGUILayout.BeginHorizontal();
                

                EditorGUILayout.ObjectField("", character.Icon, typeof(Sprite), allowSceneObjects: false);
                if (GUILayout.Button(character.Name, GUILayout.ExpandWidth(true), GUILayout.MaxWidth(200)))
                {
                    selectedCharacter = character;
                }
                if (GUILayout.Button("Delete", GUILayout.Width(60)))
                {
                    DeleteCharacter(character);
                    break;
                }
                GUILayout.FlexibleSpace();


                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndScrollView();

            GUILayout.Space(10);

            if (selectedCharacter != null)
            {
                EditorGUILayout.LabelField("Selected Character", EditorStyles.boldLabel);
                UnityEditor.Editor editor = UnityEditor.Editor.CreateEditor(selectedCharacter);
                editor.OnInspectorGUI();
            }
        }
        #endregion
        private void LoadAllCharacters()
        {
            characters.Clear();
            List<string> allCharacters = IOUtilities.ListAssetsInFolder(charactersFolderPath);
            foreach (string character in allCharacters) 
            {
                var characterSO = IOUtilities.LoadAsset<CharacterSO>(charactersFolderPath, character);
                characters.Add(characterSO);
            }
        }

        private void CreateNewCharacter()
        {
            var newCharacter = IOUtilities.CreateAsset<CharacterSO>(charactersFolderPath, NewCharacterName);
            newCharacter.Initialize(NewCharacterName, NewCharacterName);
            IOUtilities.SaveAsset(newCharacter);
            LoadAllCharacters();
        }

        private void DeleteCharacter(CharacterSO character)
        {
            if (character != null)
            {
                IOUtilities.RemoveAsset(charactersFolderPath, character.name);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                LoadAllCharacters();
            }
        }
    }
}
