using System.Collections.Generic;
using UnityEngine;

namespace DS.Data.Save
{
    using DS.Elements;
    using Enumerations;

    [System.Serializable]
    public class DS_Node_SaveData
    {
        [SerializeField] public string NodeID {  get; set; }
        [SerializeField] public string Name {  get; set; }
        [SerializeField] public string Text { get; set; }
        [SerializeField] public List<DS_ChoiceData> Choices {  get; set; }
        [SerializeField] public string GroupID { get; set; }
        [SerializeField] public DS_DialogueType DialogueType { get; set; }
        [SerializeField] public Vector2 Position { get; set; }

        public DS_Node_SaveData() { }
        public DS_Node_SaveData(DS_Node node)
        {
            NodeID = node.ID;
            Name = node.DialogueName;

            List<DS_ChoiceData> choices = new List<DS_ChoiceData>();
            foreach(DS_ChoiceData choice in node.Choices)
            {
                DS_ChoiceData choice_SaveData = new DS_ChoiceData(choice);
                choices.Add(choice_SaveData);
            }

            Choices = choices;
            Text = node.Text;
            if(node.Group != null) GroupID = node.Group.ID;
            else GroupID = null;
            DialogueType = node.DialogueType;
            Position = node.GetPosition().position;
        }
    }
}
