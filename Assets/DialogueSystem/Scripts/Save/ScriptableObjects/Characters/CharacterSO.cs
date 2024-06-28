using UnityEngine;

namespace DS.Runtime.ScriptableObjects
{
    public class CharacterSO : ScriptableObject
    {
        [SerializeField] protected string _id;
        [SerializeField] protected string _name;
        [SerializeField] protected string _completeName;
        [SerializeField] protected Sprite _icon;

        public string ID { get { return _id; } }
        public string Name { get { return _name; } set { name = value; } }
        public string CompleteName { get { return _completeName; } }
        public Sprite Icon { get { return _icon; } }

    }
}
