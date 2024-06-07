using UnityEngine;
using UnityEditor.Experimental.GraphView;


namespace DS.Elements
{
    using Utilities;
    using Enumerations;
    using Windows;
    using Data.Save;

    public class DS_SingleChoiceNode : DS_Node
    {
        public override void Initialize(string nodeName, DS_GraphView context, Vector2 spawnPosition)
        {
            base.Initialize(nodeName, context, spawnPosition);

            SetDialogueType(DS_DialogueType.SingleChoice);
            DS_ChoiceData choiceData = new DS_ChoiceData() { ChoiceName = "Next Choice" };

            Choices.Add(choiceData);
            
        }
        public override void Draw()
        {
            base.Draw();

            foreach (DS_ChoiceData choice in Choices)
            {
                Port choicePort = this.CreatePort(choice.ChoiceName, Orientation.Horizontal, Direction.Output, Port.Capacity.Multi);
                choicePort.portName = choice.ChoiceName;
                choicePort.userData = choice;

                outputContainer.Add(choicePort);
            }

            RefreshExpandedState();
        }
    }
}
