using System.Collections.Generic;
using UnityEngine;

namespace DS.Editor.Data
{
    using Runtime.Data; 
    using Enums;

    /// <summary>
    /// Class that hold node informations to be contained in a graph scriptable object.
    /// </summary>
    [System.Serializable]
    public class NodeData
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
        [SerializeField] protected List<ChoiceData> choices;
        public List<ChoiceData> Choices
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
        [SerializeField] protected DialogueType dialogueType;
        public DialogueType DialogueType 
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

        public NodeData() 
        {
            Texts = LenguageUtilities.InitLenguageDataSet<string>();
        }

        public NodeData(string _nodeID, string _dialogueName, List<ChoiceData> _choices,
                                List<LenguageData<string>> _texts, DialogueType _dialogueType,
                                string _groupID, Vector2 _position)
        {
            //Debug.Log($"Saving DS_Node_Data for {node.DialogueName}");
            this.NodeID = _nodeID;
            Name = _dialogueName;

            List<ChoiceData> choices = new List<ChoiceData>();
            foreach(ChoiceData choice in _choices)
            {
                ChoiceData choice_SaveData = new ChoiceData(choice);
                //Debug.Log($"Saving a choice LINKED TO {choice.NextNodeID}");
                choices.Add(choice_SaveData);
            }

            this.Choices = new List<ChoiceData>(choices);
            
            this.Texts = new List<LenguageData<string>>(_texts);
            this.Texts = LenguageUtilities.UpdateLenguageDataSet(_texts);    

            if(_groupID != null) this.GroupID = _groupID;
            else this.GroupID = null;

            this.DialogueType =  _dialogueType;
            this.Position = _position;
        }
    }
}
