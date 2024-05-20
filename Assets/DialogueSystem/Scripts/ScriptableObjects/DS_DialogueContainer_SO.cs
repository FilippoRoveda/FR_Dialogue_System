using System.Collections.Generic;
using UnityEngine;


namespace DS.ScriptableObjects
{
    public class DS_DialogueContainer_SO : ScriptableObject
    {
        [SerializeField] public string Filename {  get; set; }
        [SerializeField] public SerializableDictionary<DS_DialogueGroup_SO, List<DS_Dialogue_SO>> DialogueGroups { get; set; }
        [SerializeField] public List<DS_Dialogue_SO> UngroupedDialogue {  get; set; }

        public void Initialize(string filename)
        {
            Filename = filename;
            DialogueGroups = new SerializableDictionary<DS_DialogueGroup_SO, List<DS_Dialogue_SO>>();
            UngroupedDialogue = new List<DS_Dialogue_SO>();
        }
    }
}
