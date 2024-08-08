using UnityEditor;
using UnityEngine;

namespace Variables.Editor
{
    public class VariableData<T> : ScriptableObject
    {
        private string _assetPath;

        /// <summary>
        /// The displayed name for the variable
        /// </summary>
        [SerializeField] protected string _name;
        public string Name
        {
            get { return _name; }
            set
            {
#if UNITY_EDITOR
                if (string.IsNullOrEmpty(value)) return;
                _name = value;

                var valueBackUp = Value;
                var nameBackUp = Name;
                var idBBackup = ID;

                _assetPath = AssetDatabase.GetAssetPath(GetInstanceID());
                AssetDatabase.RenameAsset(_assetPath, value);

                _name = nameBackUp;
                Value = valueBackUp;
                ID = idBBackup;
                EditorUtility.SetDirty(this);
                AssetDatabase.SaveAssetIfDirty(this);
#endif
            }
        }

        /// <summary>
        /// The unique identifier of the variable
        /// </summary>
        [SerializeField] protected string _id;
        public string ID
        {
            get { return _id; }
            set
            {
                if (_id == null) { _id = value; }
                else
                {
                    _id = value;
                    EditorUtility.SetDirty(this);
                    AssetDatabase.SaveAssetIfDirty(this);
                }
            }
        }

        /// <summary>
        /// The value of the variable
        /// </summary>
        [SerializeField] protected T value;
        public T Value
        {
            get { return value; }
            set 
            { 
                this.value = value;
                EditorUtility.SetDirty(this);
                AssetDatabase.SaveAssetIfDirty(this);
            }
        }
    }
}