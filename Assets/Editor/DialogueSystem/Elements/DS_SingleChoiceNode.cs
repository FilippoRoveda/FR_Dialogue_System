using UnityEngine;


namespace DS.Elements
{
    using Enumerations;
    using Windows;
    using Data.Save;

    /// <summary>
    /// Child class that represent a single choice version of the base DS_Node.
    /// </summary>
    public class DS_SingleChoiceNode : DS_BaseNode
    {

        #region Unity callbacks
        public override void Initialize(string nodeName, DS_GraphView context, Vector2 spawnPosition)
        {
            base.Initialize(nodeName, context, spawnPosition);
            SetDialogueType(DS_DialogueType.SingleChoice);

            DS_NodeChoiceData choiceData = new DS_NodeChoiceData() { ChoiceText = "Next Choice" };
            Choices.Add(choiceData);
        }
        public override void Draw()
        {
            base.Draw();
            CreateInputPort();

            CreateOutputPortFromChoices();
            RefreshExpandedState();
        }
        #endregion
    }
}
