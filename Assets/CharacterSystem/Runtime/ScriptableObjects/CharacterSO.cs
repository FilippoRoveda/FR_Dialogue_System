using System;
using UnityEngine;

namespace Characters.Runtime
{
    /// <summary>
    /// Base scriptable object for generateing characters.
    /// </summary>
    public class CharacterSO : ScriptableObject
    {
        [SerializeField] protected string _id = null;
        [SerializeField] protected string _name = null;
        [SerializeField] protected string _completeName = null;
        [SerializeField] protected Sprite _icon = null;


        public string ID 
        { 
            get { return _id; }
            set 
            {
                if(_id == null) _id = value;
                else 
                {
#if UNITY_EDITOR
                    Debug.LogError($"CharacterSO for {_name}  has already an ID: {_id}.");
#endif
                }
            }
        }
        public string Name 
        { 
            get { return _name; }
#if UNITY_EDITOR
            set
            {
                _name = value;
                string assetPath = UnityEditor.AssetDatabase.GetAssetPath(GetInstanceID());
                UnityEditor.AssetDatabase.RenameAsset(assetPath, value);
            }
#endif
        }
        public string CompleteName 
        { 
            get { return _completeName; }
#if UNITY_EDITOR
            set { _completeName = value; }
#endif
        }
        public Sprite Icon 
        { 
            get { return _icon; }
#if UNITY_EDITOR
            set { _icon = value; }
#endif
        }
#if UNITY_EDITOR
        public void Initialize(string initialName, string initialCompleteName = "", Sprite initialIcon = null)
        {
            if(_id == null)
            {
                ID = Guid.NewGuid().ToString();
                Name = initialName;
                CompleteName = initialCompleteName;
                Icon = initialIcon;
            }
            else
            {

                Debug.LogError($"CharacterSO for {_name}  has already been initialized.");
            }
        }
#endif
    }
}
