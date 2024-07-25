using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Characters.Editor
{
    using Utilities;
    using Runtime;

    /// <summary>
    /// Window for the custom character generation system.
    /// </summary>
    public class CharacterEditorWindow : EditorWindow
    {
        private IOUtilities IOUtils = new IOUtilities();
        /// <summary>
        /// Folder path for all created and stored CharacterSO assets.
        /// </summary>
        private readonly string charactersFolderPath = "Assets/Data/Characters";


        private readonly string newCharacterName = "NewCharacter";
        private string NewCharacterName
        {
            get { return newCharacterName + "_" + allCharacterSO.Count.ToString(); }
        }

        private List<CharacterSO> allCharacterSO = new List<CharacterSO>();
        private CharacterSO selectedCharacter;

        private Vector2 scrollPos;


        [MenuItem("DialogueSystem/Character Editor")]
        public static void ShowWindow()
        {
            GetWindow<CharacterEditorWindow>("Character Editor");
        }

        #region Unity callbacks
        private void OnEnable()
        {
            LoadAllCharacters();
        }

        private void OnGUI()
        {
            //Upper label.
            GUILayout.Label("Character Editor", EditorStyles.boldLabel);

            if (GUILayout.Button("Create New Character", GUILayout.MaxWidth(200)))
            {
                OnCreateNewCharacterButtonPressed();
            }

            GUILayout.Space(10);

            //All characterSO view.
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

            foreach (CharacterSO characterSO in allCharacterSO)
            {
                EditorGUILayout.BeginHorizontal();
                

                EditorGUILayout.ObjectField("", characterSO.Icon, typeof(Sprite), allowSceneObjects: false);
                if (GUILayout.Button(characterSO.Name, GUILayout.ExpandWidth(true), GUILayout.MaxWidth(200)))
                {
                    selectedCharacter = characterSO;
                }
                if (GUILayout.Button("Delete", GUILayout.Width(60)))
                {
                    OnDeleteCharacterButtonPressed(characterSO);
                    break;
                }
                GUILayout.FlexibleSpace();


                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndScrollView();

            GUILayout.Space(10);
            //Selected character inspector view.
            if (selectedCharacter != null)
            {
                EditorGUILayout.LabelField("Selected Character", EditorStyles.boldLabel);
                UnityEditor.Editor editor = UnityEditor.Editor.CreateEditor(selectedCharacter);
                editor.OnInspectorGUI();
            }
        }
        #endregion
        /// <summary>
        /// Load alla CharacterSO assets from the character assets folder path.
        /// </summary>
        private void LoadAllCharacters()
        {
            allCharacterSO.Clear();
            List<string> allCharacterAssetsName = IOUtils.ListAssetsInFolder(charactersFolderPath);
            foreach (string assetName in allCharacterAssetsName) 
            {
                var characterSO = IOUtils.LoadAsset<CharacterSO>(charactersFolderPath, assetName);
                allCharacterSO.Add(characterSO);
            }
        }

        #region Callbacks
        private void OnCreateNewCharacterButtonPressed()
        {
            var newCharacterSO = IOUtils.CreateAsset<CharacterSO>(charactersFolderPath, NewCharacterName);
            newCharacterSO.Initialize(NewCharacterName, NewCharacterName);
            IOUtils.SaveAsset(newCharacterSO);
            LoadAllCharacters();
        }

        private void OnDeleteCharacterButtonPressed(CharacterSO characterSO)
        {
            if (characterSO != null)
            {
                IOUtils.RemoveAsset(charactersFolderPath, characterSO.name);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                LoadAllCharacters();
            }
        }
        #endregion
    }
}
