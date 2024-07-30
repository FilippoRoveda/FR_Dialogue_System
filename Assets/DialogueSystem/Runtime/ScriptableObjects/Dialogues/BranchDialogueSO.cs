using System.Collections.Generic;
using UnityEngine;


namespace DS.Runtime.ScriptableObjects
{
    using Runtime.Data;
    using Runtime.Conditions;
    public class BranchDialogueSO : BaseDialogueSO
    {
        //List of conditions as a condition container
        [SerializeField] private DialogueConditionContainer condtitions;
        public DialogueConditionContainer Condtitions 
        { 
            get { return condtitions; } 
            set { condtitions = value; }
        }
        [SerializeField] protected List<DialogueChoice> choices;
        public List<DialogueChoice> Choices
        {
            get { return choices; }
            set { choices = value; }
        }
    }
}