using UnityEngine;
using UnityEditor.Experimental.GraphView;


namespace DS.Elements
{
    using Utilities;
    using Enumerations;
    using Windows;

    public class DS_SingleChoiceNode : DS_Node
    {
        public override void Initialize(DS_GraphView context, Vector2 spawnPosition)
        {
            base.Initialize(context, spawnPosition);

            SetDialogueType(DS_DialogueType.SingleChoice);
            Choices.Add("Next Choice");
            
        }
        public override void Draw()
        {
            base.Draw();
            foreach (string choice in Choices)
            {
                Port choicePort = this.CreatePort(choice, Orientation.Horizontal, Direction.Output, Port.Capacity.Multi);
                choicePort.portName = choice;
                outputContainer.Add(choicePort);
            }
            RefreshExpandedState();
        }
    }
}
