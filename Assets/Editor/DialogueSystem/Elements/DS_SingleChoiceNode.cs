using UnityEngine;

namespace DialogueSystem.Eelements
{
    using DS.Utilities;
    using Enumerations;
    using UnityEditor.Experimental.GraphView;

    public class DS_SingleChoiceNode : DS_Node
    {
        public override void Initialize(Vector2 spawnPosition)
        {
            base.Initialize(spawnPosition);

            DialogueType = DS_DialogueType.SingleChoice;
            Choices.Add("Next Choice");
            
        }
        public override void Draw()
        {
            base.Draw();
            foreach (string choice in Choices)
            {
                //Input port element
                Port choicePort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));
                choicePort.portName = choice;
                outputContainer.Add(choicePort);
            }

            RefreshExpandedState();
        }


    }
}
