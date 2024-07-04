using UnityEngine;

namespace DS.Runtime.ScriptableObjects
{
    public class DS_EndDialogueSO : DS_DialogueSO
    {
        [SerializeField] private bool isDialogueRepetable;
        public bool IsDialogueRepetable { get { return isDialogueRepetable; } private set { isDialogueRepetable = value; } }

        public void SetRepetableDialogue(bool isRepetable)
        {
            IsDialogueRepetable = isRepetable;
        }
    }
}