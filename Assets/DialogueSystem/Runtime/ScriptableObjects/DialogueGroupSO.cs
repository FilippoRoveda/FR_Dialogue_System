using UnityEngine;

namespace DS.Runtime.ScriptableObjects
{
    /// <summary>
    /// Scriptable object to contain informations of a group for saving and loading operations.
    /// </summary>
    public class DialogueGroupSO : ScriptableObject
    {
        [SerializeField]
        private string groupName;
        public string GroupName 
        {  
            get { return groupName; }
            set { groupName = value; }
        }

        public void Initialize(string groupName)
        {
            GroupName = groupName;
        }
    }
}
