using UnityEditor;
using UnityEngine;

namespace Variables.Editor
{
    public class VariableData<T> : ScriptableObject
    {

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
                var previewsName = _name;
                if (string.IsNullOrEmpty(value)) return;
                _name = value;

                var valueBackUp = Value;
                var nameBackUp = Name;
                var idBBackup = Id;

                string assetPath = AssetDatabase.GetAssetPath(GetInstanceID());
                AssetDatabase.RenameAsset(assetPath, value);

                _name = nameBackUp;
                Value = valueBackUp;
                Id = idBBackup;
                EditorUtility.SetDirty(this);
                AssetDatabase.SaveAssetIfDirty(this);
#endif
            }
        }

        /// <summary>
        /// The unique identifier of the variable
        /// </summary>
        [SerializeField] protected string id;
        public string Id
        {
            get { return id; }
            set
            {
                if (id == null) { id = value; }
                else
                {
                    id = value;
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