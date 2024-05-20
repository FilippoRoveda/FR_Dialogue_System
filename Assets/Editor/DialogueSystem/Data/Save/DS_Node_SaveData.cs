using System.Collections.Generic;
using UnityEngine;

namespace DS.Data.Save
{
    using Enumerations;

    [System.Serializable]
    public class DS_Node_SaveData
    {
        [SerializeField] public string NodeID {  get; set; }
        [SerializeField] public string Name {  get; set; }
        [SerializeField] public string Text { get; set; }
        [SerializeField] public List<DS_Choice_SaveData> choices {  get; set; }
        [SerializeField] public string GroupID { get; set; }
        [SerializeField] public DS_DialogueType DialogueType { get; set; }
        [SerializeField] public Vector2 Position { get; set; }
    }
}
