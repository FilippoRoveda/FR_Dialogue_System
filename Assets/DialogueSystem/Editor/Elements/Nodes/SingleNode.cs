using UnityEngine;


namespace DS.Editor.Elements
{
    using Editor.Enumerations;
    using Editor.Data;
    using DS.Editor.Windows;
    using UnityEditor.Experimental.GraphView;

    /// <summary>
    /// Child class that represent a single choice version of the base DS_Node.
    /// </summary>
    public class SingleNode : DialogueNode
    {
        protected Port inputPort;
        protected Port outputPort;

        #region Unity callbacks
        public override void Initialize(string nodeName, DS_GraphView context, Vector2 spawnPosition)
        {
            base.Initialize(nodeName, context, spawnPosition);
            SetDialogueType(NodeType.Single);

            ChoiceData choiceData = new ChoiceData("Next Choice");
            Data.Choices.Add(choiceData);
        }
        public override void Draw()
        {
            base.Draw();

            inputPort = CreateInputPort();
            outputPort = CreateOutputPortFromChoices()[0];

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
