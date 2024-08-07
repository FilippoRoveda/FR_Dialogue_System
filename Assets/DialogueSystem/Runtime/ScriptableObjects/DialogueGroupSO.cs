using UnityEngine;

namespace DS.Runtime.ScriptableObjects
{
    /// <summary>
    /// Scriptable object to contain informations of a group for saving and loading operations.
    /// </summary>
    public class DialogueGroupSO : ScriptableObject
    {
        [SerializeField]
        private string _groupName;
        public string GroupName 
        {  
            get { return _groupName; }
            set { _groupName = value; }
        }

        public void Initialize(string groupName)
        {
            GroupName = groupName;
        }
    }
}
