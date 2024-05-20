using UnityEngine;

namespace DS.ScriptableObjects
{
    public class DS_DialogueGroup_SO : ScriptableObject
    {
        [SerializeField] public string GroupName {  get; set; }

        public void Initialize(string groupName)
        {
            GroupName = groupName;
        }
    }
}
