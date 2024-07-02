using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace DS.Editor.Windows.Utilities
{
    /// <summary>
    /// 
    /// </summary>
    public class DS_IOUtilities
    {
        public T CreateAsset<T>(string path, string assetName) where T : ScriptableObject
        {
            string fullPath = $"{path}/{assetName}.asset";
            T asset = LoadAsset<T>(path, assetName);

            if (asset == null)
            {
                asset = ScriptableObject.CreateInstance<T>();
                AssetDatabase.CreateAsset(asset, fullPath);
            }
            return asset;
        }

        public T LoadAsset<T>(string path, string assetName) where T : ScriptableObject
        {
            string fullPath = $"{path}/{assetName}.asset";
            return AssetDatabase.LoadAssetAtPath<T>(fullPath);
        }

        private List<T> LoadAssetsByType<T>() where T : ScriptableObject
        {
            string typeName = typeof(T).Name;
            Debug.Log(typeName);
            List<T> list = new List<T>();
            string[] guids = AssetDatabase.FindAssets($"t:{typeName}");
            foreach (var guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                T asset = AssetDatabase.LoadAssetAtPath<T>(path);
                list.Add(asset);
            }

            return list;
        }

        public void SaveAsset(UnityEngine.Object asset)
        {
            EditorUtility.SetDirty(asset);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        public void RemoveFolder(string folderPath)
        {
            FileUtil.DeleteFileOrDirectory($"{folderPath}.meta");
            FileUtil.DeleteFileOrDirectory($"{folderPath}/");
        }

        public void RemoveAsset(string path, string assetName)
        {
            AssetDatabase.DeleteAsset($"{path}/{assetName}.asset");
        }

        public List<string> ListAssetsInFolder(string folderPath)
        {

            string[] assetPaths = AssetDatabase.FindAssets("", new[] { folderPath });

            List<string> assetNames = new List<string>();

            foreach (string assetPath in assetPaths)
            {
                string path = AssetDatabase.GUIDToAssetPath(assetPath);
                string assetName = System.IO.Path.GetFileNameWithoutExtension(path);
                assetNames.Add(assetName);
            }
            return assetNames;
        }
    }
}
