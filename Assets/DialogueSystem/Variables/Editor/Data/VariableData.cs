using UnityEngine;

namespace Variables.Editor
{
    public abstract class VariableData<T> : ScriptableObject
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
                _name = value;

                var valueBackUp = Value;
                var nameBackUp = Name;
                var idBBackup = Id;

                string assetPath = UnityEditor.AssetDatabase.GetAssetPath(GetInstanceID());
                UnityEditor.AssetDatabase.RenameAsset(assetPath, value);

                _name = nameBackUp;
                Value = valueBackUp;
                Id = idBBackup;
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
            set { this.value = value; }
        }
    }
}