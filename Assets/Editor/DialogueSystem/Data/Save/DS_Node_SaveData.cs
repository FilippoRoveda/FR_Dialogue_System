using System.Collections.Generic;
using UnityEngine;

namespace DS.Data.Save
{
    using DS.Elements;
    using Enumerations;

    /// <summary>
    /// Class that hold node informations to be contained in a graph scriptable object.
    /// </summary>
    [System.Serializable]
    public class DS_Node_SaveData
    {
        [SerializeField] private string nodeID;
        public string NodeID 
        {
            get
            {
                return nodeID;
            }
            set 
            {
                nodeID = value;
            } 
        }
        [SerializeField] private string name;
        public string Name 
        {  get
            {
                return name;
            }
           set
            {
                name = value;
            }
        }
        [SerializeField] private string text;
        public string Text 
        { 
            get
            {
                return text;
            }
            set
            {
                text = value;
            }
        }
        [SerializeField] private List<DS_Choice_SaveData> choices;
        public List<DS_Choice_SaveData> Choices
        {  get
            { 
                return choices;
            }
           set
            {
                choices = value;
            }
        }

        [SerializeField] private string groupID;
        /// <summary>
        /// Group ID for the group that hold this node.
        /// </summary>
        public string GroupID 
        {
            get 
            {
                return groupID;
            } 
            set
            {
                groupID = value;
            }
        }
        [SerializeField] private DS_DialogueType dialogueType;
        public DS_DialogueType DialogueType 
        { 
            get
            {
                return dialogueType;
            }
            set
            {
                dialogueType = value;
            }
        }
        [SerializeField] private Vector2 position;
        public Vector2 Position 
        { 
            get
            {
                return position;
            }
            set
            {
                position = value;
            }
        }

        public DS_Node_SaveData() { }
        public DS_Node_SaveData(DS_Node node)
        {
            NodeID = node.ID;
            Name = node.DialogueName;

            List<DS_Choice_SaveData> choices = new List<DS_Choice_SaveData>();
            foreach(DS_Choice_SaveData choice in node.Choices)
            {
                DS_Choice_SaveData choice_SaveData = new DS_Choice_SaveData(choice);
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
