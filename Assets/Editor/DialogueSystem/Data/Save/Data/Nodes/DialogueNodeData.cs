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
    public class DialogueNodeData : TextedNodeData
    {
       
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

       
        public DialogueNodeData() : base()
        {
            
        }

        public DialogueNodeData(string _nodeID, string _dialogueName, List<ChoiceData> _choices,
                                List<LenguageData<string>> _texts, DialogueType _dialogueType,
                                string _groupID, Vector2 _position) 
                                : base(_nodeID, _dialogueName, _texts, _dialogueType, _groupID, _position)
        {
            
            
            List<ChoiceData> choices = new List<ChoiceData>();
            foreach(ChoiceData choice in _choices)
            {
                ChoiceData choice_SaveData = new ChoiceData(choice);
                
                choices.Add(choice_SaveData);
            }

            this.Choices = new List<ChoiceData>(choices);                    
        }
    }
}
