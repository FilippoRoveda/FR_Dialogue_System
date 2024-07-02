using System.Collections.Generic;
using UnityEngine;

namespace DS.Editor.Data
{
    using Runtime.Data; 
    using Enumerations;
    using Runtime.ScriptableObjects;

    /// <summary>
    /// Class that hold node informations to be contained in a graph scriptable object.
    /// </summary>
    [System.Serializable]
    public class DS_NodeData
    {
        [SerializeField] protected string name;
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

        [SerializeField] protected string nodeID;
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

        [SerializeField] protected List<LenguageData<string>> texts;
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
        [SerializeField] protected List<DS_ChoiceData> choices;
        public List<DS_ChoiceData> Choices
        {  get
            { 
                return choices;
            }
           set
            {
                choices = value;
            }
        }

        [SerializeField] protected string groupID;
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
        [SerializeField] protected DS_DialogueType dialogueType;
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


        [SerializeField] protected List<DS_DialogueEventSO> events;
        public List<DS_DialogueEventSO> Events
        {
            get { return events; }
            set { events = value; }
        }

        [SerializeField] protected Vector2 position;
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

        public DS_NodeData() 
        {
            Texts = DS_LenguageUtilities.InitLenguageDataSet<string>();
        }

        public DS_NodeData(string NodeID, string DialogueName, List<DS_ChoiceData> Choices,
                                List<LenguageData<string>> Texts, DS_DialogueType dialogueType,
                                List<DS_DialogueEventSO> Events, string GroupID, Vector2 Position)
        {
            //Debug.Log($"Saving DS_Node_Data for {node.DialogueName}");
            this.NodeID = NodeID;
            Name = DialogueName;

            List<DS_ChoiceData> choices = new List<DS_ChoiceData>();
            foreach(DS_ChoiceData choice in Choices)
            {
                DS_ChoiceData choice_SaveData = new DS_ChoiceData(choice);
                //Debug.Log($"Saving a choice LINKED TO {choice.NextNodeID}");
                Debug.Log(choice_SaveData.ChoiceTexts);
                choices.Add(choice_SaveData);
            }


            this.Choices = new List<DS_ChoiceData>(choices);

            
            this.Texts = new List<LenguageData<string>>(Texts);
            this.Texts = DS_LenguageUtilities.UpdateLenguageDataSet(Texts);


            if (dialogueType == DS_DialogueType.Event)
            {
                events = new List<DS_DialogueEventSO>();
                if (Events != null && Events.Count > 0)
                {
                    foreach (var _event in Events)
                    {
                        Debug.Log(_event.name);
                        events.Add(_event);
                    }
                }
            }
            else events = null;

            if(GroupID != null) this.GroupID = GroupID;
            else this.GroupID = null;

            this.DialogueType =  dialogueType;
            this.Position = Position;
        }
    }
}
