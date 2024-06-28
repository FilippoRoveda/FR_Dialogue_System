using DS.Runtime.ScriptableObjects;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DS_CharacterEditorWindow : EditorWindow
{
    private List<CharacterSO> characters = new List<CharacterSO>();
    private string newCharacterName = "NewCharacter";
    private string NewCharacterName
    {
        get { return newCharacterName + "_" + characters.Count.ToString(); }
    }
    private Vector2 scrollPos;
    private CharacterSO selectedCharacter;

    [MenuItem("DialogueSystem/Character Manager")]
    public static void ShowWindow()
    {
        GetWindow<DS_CharacterEditorWindow>("Character Editor");
    }

    private void OnEnable()
    {
        LoadCharacters();
    }

    private void OnGUI()
    {
        GUILayout.Label("Character Editor", EditorStyles.boldLabel);

        if (GUILayout.Button("Create New Character"))
        {
            CreateNewCharacter();
        }

        GUILayout.Space(10);

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        foreach (CharacterSO character in characters)
        {
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button(character.Name, GUILayout.Width(60)))
            {
                selectedCharacter = character;
            }
            if (GUILayout.Button("Delete", GUILayout.Width(60)))
            {
                DeleteCharacter(character);
                break;
            }
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndScrollView();

        GUILayout.Space(10);

        if (selectedCharacter != null)
        {
            EditorGUILayout.LabelField("Selected Character", EditorStyles.boldLabel);
            Editor editor = Editor.CreateEditor(selectedCharacter);
            editor.OnInspectorGUI();
        }
    }

    private void LoadCharacters()
    {
        characters.Clear();
        string[] guids = AssetDatabase.FindAssets("t:CharacterSO");
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            CharacterSO character = AssetDatabase.LoadAssetAtPath<CharacterSO>(path);
            characters.Add(character);
        }
    }

    private void CreateNewCharacter()
    {
        CharacterSO newCharacter = CreateInstance<CharacterSO>();
        string newName = NewCharacterName;
        string path = $"Assets/DialogueSystem/ScriptableObjects/Characters/{newName}.asset";
        newCharacter.Name = newName;
        AssetDatabase.CreateAsset(newCharacter, path);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        LoadCharacters();
    }

    private void DeleteCharacter(CharacterSO character)
    {
        if (character != null)
        {
            string path = AssetDatabase.GetAssetPath(character);
            AssetDatabase.DeleteAsset(path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            LoadCharacters();
        }
    }
}
