using UnityEngine;
using UnityEngine.UIElements;

namespace DS.Editor.Elements
{
    using Editor.Enumerations;
    using Editor.Data;
    using Editor.Windows;
    using Editor.Utilities;


    public abstract class TextedNode : BaseNode
    {
        private TextedNodeData data = new();
        public new TextedNodeData Data { get { return data; } }
        public string CurrentText
        {
            get
            {
                return Data.Texts.GetLenguageData(graphView.GetEditorCurrentLenguage()).Data;
            }
            private set
            {
                Data.Texts.SetLenguageData(graphView.GetEditorCurrentLenguage(), value);
            }
        }

        protected TextField dialogueTextTextField;
        protected VisualElement customDataContainer;
        protected Foldout dialogueTextFoldout;

        public override void Initialize(string nodeName, DS_GraphView context, Vector2 spawnPosition)
        {
            base.Initialize(nodeName, context, spawnPosition);

            Data.Texts = LenguageUtilities.InitLenguageDataSet("Dialogue Text");
            graphView.GraphLenguageChanged.AddListener(OnGraphViewLenguageChanged);
        }

        public override void Draw()
        {
            base.Draw();

            customDataContainer = new VisualElement();
            customDataContainer.AddToClassList("ds-node-custom-data-container");

            //Dialogue text foldout and text field
            dialogueTextFoldout = ElementsUtilities.CreateFoldout("DialogueText");

            dialogueTextTextField = ElementsUtilities.CreateTextArea(CurrentText, null, callback =>
            {
                data.Texts.GetLenguageData(graphView.GetEditorCurrentLenguage()).Data = callback.newValue;
            });

            dialogueTextTextField.AddToClassLists("ds-node-textfield", "ds-node-quote-textfield");

            dialogueTextFoldout.Add(dialogueTextTextField);
            customDataContainer.Add(dialogueTextFoldout);
            extensionContainer.Add(customDataContainer);
        }

        #region Overrides
        protected virtual void OnGraphViewLenguageChanged(LenguageType newLenguage)
        {
            dialogueTextTextField.SetValueWithoutNotify(CurrentText);
        }
        #endregion
    }
}
