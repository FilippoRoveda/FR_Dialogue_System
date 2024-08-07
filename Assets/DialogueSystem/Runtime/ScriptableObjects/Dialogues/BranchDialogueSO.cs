using System.Collections.Generic;
using UnityEngine;


namespace DS.Runtime.ScriptableObjects
{
    using Runtime.Data;
    using Runtime.Conditions;
    public class BranchDialogueSO : BaseDialogueSO
    {
        //List of conditions as a condition container
        [SerializeField] private DialogueConditions _condtitions;
        public DialogueConditions Condtitions 
        { 
            get { return _condtitions; } 
            set { _condtitions = value; }
        }
        [SerializeField] protected List<DialogueChoice> _choices;
        public List<DialogueChoice> Choices
        {
            get { return _choices; }
            set { _choices = value; }
        }
    }
}