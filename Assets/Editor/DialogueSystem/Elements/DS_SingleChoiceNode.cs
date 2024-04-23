using UnityEngine;

namespace DialogueSystem.Eelements
{
    using Enumerations;
    using UnityEditor.Experimental.GraphView;

    public class DS_SingleChoiceNode : DS_Node
    {
        public override void Draw()
        {
            base.Draw();
            DialogueType = DS_DialogueType.SingleChoice;
            Choiches.Add("Next Choice");
        }

        public override void Initialize(Vector2 spawnPosition)
        {
            base.Initialize(spawnPosition);
            foreach (string choice in Choiches)
            {
                Port choicePort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));
                choicePort.portName = choice;
                outputContainer.Add(choicePort);
            }

            RefreshExpandedState();
        }
    }
}
