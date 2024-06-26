using System.Collections.Generic;
using UnityEngine;

namespace DS.Data.Save
{
    using Elements;
    using ScriptableObjects;
    using Utilities;
    using Enumerations;

    /// <summary>
    /// Class that hold node informations to be contained in a graph scriptable object.
    /// </summary>
    [System.Serializable]
    public class DS_Node_SaveData
    {
        [SerializeField] private string name;
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }

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

        [SerializeField] private List<LenguageData<string>> texts;
        public List<LenguageData<string>> Texts
        { 
            get
            {
                return texts;
            }
            set
            {
                texts = value;
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


        [SerializeField] private List<DS_DialogueEventSO> events;
        public List<DS_DialogueEventSO> Events
        {
            get { return events; }
            set { events = value; }
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

        public DS_Node_SaveData() 
        {
            Texts = DS_LenguageUtilities.InitLenguageDataSet<string>();
        }
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


            Choices = new List<DS_NodeChoiceData>(choices);

            
            Texts = new List<LenguageData<string>>(node.Texts);
            Texts = DS_LenguageUtilities.UpdateLenguageDataSet(Texts);


            if (node.DialogueType == DS_DialogueType.Event)
            {
                events = new List<DS_DialogueEventSO>();
                var _eventNode = (DS_EventNode)node;
                if (_eventNode.DialogueEvents != null && _eventNode.DialogueEvents.Count > 0)
                {
                    foreach (var _event in _eventNode.DialogueEvents)
                    {
                        Debug.Log(_event.name);
                        events.Add(_event);
                    }
                }
            }
            else events = null;

            if(node.Group != null) GroupID = node.Group.ID;
            else GroupID = null;

            DialogueType = node.DialogueType;
            Position = node.GetPosition().position;
        }
    }
}
