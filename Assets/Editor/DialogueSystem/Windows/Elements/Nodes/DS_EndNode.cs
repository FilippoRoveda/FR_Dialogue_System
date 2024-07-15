using UnityEngine;
using UnityEngine.UIElements;

namespace DS.Editor.Windows.Elements
{
    using Runtime.Data;
    using Enums;

    public class DS_EndNode : DS_BaseNode
    {
        [SerializeField] private bool isRepetableDialogue = false;
        public bool IsRepetableDialogue { get { return isRepetableDialogue; }  set { isRepetableDialogue = value; } }


        public override void Initialize(string nodeName, DS_GraphView context, Vector2 spawnPosition)
        {         
            base.Initialize(nodeName, context, spawnPosition);
            Texts = LenguageUtilities.InitLenguageDataSet("End Dialogue Text");
            SetDialogueType(DialogueType.End);
        }
        public override void Draw()
        {
            base.Draw();
            DrawIsRepetableField();
            CreateInputPort("EndNode connection");
            RefreshExpandedState();
        }

        private void DrawIsRepetableField()
        {
            VisualElement boolFieldContainer = new VisualElement();
            boolFieldContainer.style.flexDirection = FlexDirection.Row;
            boolFieldContainer.style.alignItems = Align.FlexStart;

            Label boolFieldLabel = new Label("Is Repeatable:");
            boolFieldLabel.style.marginRight = 10;
            Toggle isRepetableField = new Toggle
            {
                value = isRepetableDialogue
            };

            isRepetableField.RegisterValueChangedCallback(evt =>
            {
                isRepetableDialogue = evt.newValue;
            });

            boolFieldContainer.Add(boolFieldLabel);
            boolFieldContainer.Add(isRepetableField);

            extensionContainer.Insert(0, boolFieldContainer);
        }

        protected override void SetNodeStyle()
        {
            extensionContainer.AddToClassList("ds-node_extension-container");
            mainContainer.AddToClassList("ds-end-node_main-container");
            SetDefaultColor(mainContainer.style.backgroundColor);
        }
        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            evt.menu.AppendAction("Disconnect Inputs Ports", actionEvent => DisconnectPorts(inputContainer));
            base.BuildContextualMenu(evt);
        }
        /// <summary>
        /// Return true if this node is a starting node.
        /// </summary>
        /// <returns></returns>
        public override bool IsStartingNode()
        {
            return false;
        }
    }
}
