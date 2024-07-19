using System.Collections.Generic;
using UnityEngine;

namespace DS.Editor.Data
{
    using Editor.Elements;

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
            choices = new List<ChoiceData>();
        }

        public DialogueNodeData(DialogueNode node) : base(node)
        {          
            List<ChoiceData> choices = new List<ChoiceData>();
            foreach(ChoiceData choice in node.Data.Choices)
            {
                ChoiceData choice_SaveData = new ChoiceData(choice);
                
                choices.Add(choice_SaveData);
            }

            Choices = new List<ChoiceData>(choices);                    
        }
    }
}
