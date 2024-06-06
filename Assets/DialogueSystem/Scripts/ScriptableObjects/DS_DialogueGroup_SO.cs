using UnityEngine;

namespace DS.ScriptableObjects
{
    public class DS_DialogueGroup_SO : ScriptableObject
    {
        [SerializeField] [IsInteractable(false)] private string groupName;
        public string GroupName 
        {  
            get
            {
                return groupName;
            }
            set
            {
                groupName = value;
            }
        }

        public void Initialize(string groupName)
        {
            GroupName = groupName;
        }
    }
}
