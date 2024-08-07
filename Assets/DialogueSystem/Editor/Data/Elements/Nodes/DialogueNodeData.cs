using System.Collections.Generic;
using UnityEngine;

namespace DS.Editor.Data
{
    using Editor.Elements;

    /// <summary>
    /// NodeData class that hold every information for a DialogueNode.
    /// </summary>
    [System.Serializable]
    public class DialogueNodeData : TextedNodeData
    {
       
        [SerializeField] protected List<ChoiceData> _choices;
        public List<ChoiceData> Choices
        { 
            get => _choices; set => _choices = value;
        }

        public DialogueNodeData() : base()
        {
            _choices = new List<ChoiceData>();
        }
        public DialogueNodeData(DialogueNodeData data) : base(data)
        {
            _choices = new List<ChoiceData>(data.Choices);
        }
        public DialogueNodeData(DialogueNode node) : base(node)
        {          
            List<ChoiceData> choices = new List<ChoiceData>();
            foreach(ChoiceData choice in node._choices)
            {
                ChoiceData choice_SaveData = new ChoiceData(choice);
                
                choices.Add(choice_SaveData);
            }

            this._choices = new List<ChoiceData>(choices);                    
        }
    }
}
