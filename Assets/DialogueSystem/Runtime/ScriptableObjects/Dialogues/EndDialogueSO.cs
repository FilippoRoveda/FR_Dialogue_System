using UnityEngine;

namespace DS.Runtime.ScriptableObjects
{
    public class EndDialogueSO : TextedDialogueSO
    {
        [SerializeField] private bool _isRepetable;
        public bool IsRepetable 
        { 
            get { return _isRepetable; }
#if UNITY_EDITOR
            set { _isRepetable = value; }
#endif
        }
    }
}
