using System;
using UnityEditor.Callbacks;
using UnityEditor;

namespace DS.Editor.Windows
{
    using Editor.ScriptableObjects;
    public static class GraphAssetWindowHandler
    {
        [OnOpenAsset(1)]
        public static bool OpenAssetEditorWindow(Int32 _instanceID)
        {
            UnityEngine.Object item = EditorUtility.InstanceIDToObject(_instanceID);

            if (item is GraphSO)
            {
                GraphSO assetGraph = item as GraphSO;
                DS_AssetEditorWindow.OpenWindow(assetGraph);
                return true;
            }
            else return false;
        }
    }
}
