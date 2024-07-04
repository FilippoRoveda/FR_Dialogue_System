using UnityEngine;


namespace DS.Editor.Windows.Elements
{
    using Enums;
    using Editor.Data;

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

            DS_ChoiceData choiceData = new DS_ChoiceData("Next Choice");
            Choices.Add(choiceData);
        }
        public override void Draw()
        {
            base.Draw();
            CreateInputPort();

            CreateOutputPortFromChoices();
            RefreshExpandedState();
        }
        protected override void SetNodeStyle()
        {
            extensionContainer.AddToClassList("ds-node_extension-container");
            mainContainer.AddToClassList("ds-single-node_main-container");
            SetDefaultColor(mainContainer.style.backgroundColor);
        }
        #endregion
    }
}
