using System;
using UnityEngine;

namespace DS.Runtime.ScriptableObjects
{
    public class CharacterSO : ScriptableObject
    {
        [IsInteractable(false)][SerializeField] protected string _id = null;
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
            set 
            { 
                _name = value;
                string assetPath = UnityEditor.AssetDatabase.GetAssetPath(GetInstanceID());
                UnityEditor.AssetDatabase.RenameAsset(assetPath, value);
               
            } 
        }
        public string CompleteName { get { return _completeName; } set { _completeName = value; } }
        public Sprite Icon { get { return _icon; } set { _icon = value; } }

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
#if UNITY_EDITOR
                Debug.LogError($"CharacterSO for {_name}  has already been initialized.");
#endif
            }
        }

    }
}
