using UnityEngine;
using UnityEditor;
using System.IO;


namespace Variables.Editor
{
    [InitializeOnLoad]
    public class VariableSystem
    {

        static VariableSystem()
        {
            EditorApplication.delayCall += Startup;
        }

        public static void Startup()
        {

            CreateDirectoryIfNotExists("Assets/Editor");
            CreateDirectoryIfNotExists("Assets/Editor/Data");
            CreateDirectoryIfNotExists("Assets/Editor/Data/Variables");
            CreateDirectoryIfNotExists("Assets/Editor/Data/Variables/Integers");
            CreateDirectoryIfNotExists("Assets/Editor/Data/Variables/Decimals");
            CreateDirectoryIfNotExists("Assets/Editor/Data/Variables/Booleans");
            CreateDirectoryIfNotExists("Assets/Data");
            CreateDirectoryIfNotExists("Assets/Data/Generated");


            if (!File.Exists(GetDatabasePath()))
            {
                var db = ScriptableObject.CreateInstance<VariablesDatabase>();
                AssetDatabase.CreateAsset(db, GetDatabasePath());
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
            CreateFileIfNotExists(GetGeneratedFilePath(), "");


            // TODO: Initialize Editor Window here

        }
        public static void Shutdown() { }


        static void CreateDirectoryIfNotExists(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                Debug.Log($"Created directory: {path}");
            }
        }

        static void CreateFileIfNotExists(string filePath, string initialContent)
        {
            if (!File.Exists(filePath))
            {
                File.WriteAllText(filePath, initialContent);
                Debug.Log($"Created file: {filePath}");
            }
        }

        public static string GetDatabasePath() => "Assets/Editor/Data/VariablesDatabase.asset";
        public static string GetGeneratedFilePath() => "Assets/Data/Generated/Variables.Generated.cs";
        public static string GetIntegerVariablesPath() => "Assets/Editor/Data/Variables/Integers";
        public static string GetFloatVariablesPath() => "Assets/Editor/Data/Variables/Decimals";
        public static string GetBoolVariablesPath() => "Assets/Editor/Data/Variables/Booleans";
    }
}