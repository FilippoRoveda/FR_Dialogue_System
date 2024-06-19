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
        [SerializeField] private List<DS_NodeChoiceData> choices;
        public List<DS_NodeChoiceData> Choices
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
        public DS_Node_SaveData(DS_BaseNode node)
        {
            //Debug.Log($"Saving DS_Node_Data for {node.DialogueName}");
            NodeID = node.ID;
            Name = node.DialogueName;

            List<DS_NodeChoiceData> choices = new List<DS_NodeChoiceData>();
            foreach(DS_NodeChoiceData choice in node.Choices)
            {
                DS_NodeChoiceData choice_SaveData = new DS_NodeChoiceData(choice);
                //Debug.Log($"Saving a choice LINKED TO {choice.NextNodeID}");
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
