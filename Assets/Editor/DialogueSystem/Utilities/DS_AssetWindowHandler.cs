using System;
using UnityEditor.Callbacks;
using UnityEditor;

namespace DS.Utilities
{
    using DS.Data.Save;
    using DS.Windows;
    public static class DS_AssetWindowHandler
    {
        [OnOpenAsset(1)]
        public static bool OpenAssetEditorWindow(Int32 _instanceID)
        {
            UnityEngine.Object item = EditorUtility.InstanceIDToObject(_instanceID);

            if (item is DS_GraphSO)
            {
                DS_GraphSO assetGraph = item as DS_GraphSO;
                DS_GraphAssetEditorWindow.OpenWindow(assetGraph);
                return true;
            }
            else return false;
        }
    }
}
