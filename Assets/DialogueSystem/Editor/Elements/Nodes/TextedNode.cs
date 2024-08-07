using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UIElements;

namespace DS.Editor.Elements
{
    using Editor.Enumerations;
    using Editor.Data;
    using Editor.Windows;
    using Editor.Utilities;

    /// <summary>
    /// Abstract TextNode class for nodes that holds a dialogue text field.
    /// </summary>
    public abstract class TextedNode : BaseNode
    {
        public List<LenguageData<string>> _texts;

            public string CurrentText
            {
                get
                {
                    return _texts.GetLenguageData(_graphView.GetEditorCurrentLenguage()).Data;
                }
                private set
                {
                    _texts.SetLenguageData(_graphView.GetEditorCurrentLenguage(), value);
                }
            }

        protected TextField dialogueTextField;
        protected Foldout textFoldout;
        protected VisualElement customContainer;


        public override void Initialize(string nodeName, DS_GraphView context, Vector2 spawnPosition)
        {
            _nodeID = System.Guid.NewGuid().ToString();
            _nodeName = nodeName;
            _position = spawnPosition;
            SetPosition(new Rect(spawnPosition, Vector2.zero));
            _graphView = context;
            SetNodeStyle();

            _texts = LenguageUtilities.InitLenguageDataSet("Dialogue Text");
            _graphView.GraphLenguageChanged.AddListener(OnGraphViewLenguageChanged);
        }

        public void Initialize(TextedNodeData _data, DS_GraphView context)
        {
            _nodeID = _data.NodeID;
            _nodeName = _data.Name;
            _position = _data.Position;
            //SetPosition(new Rect(_position, Vector2.zero));
            _graphView = context;
            SetNodeStyle();

            _texts = new List<LenguageData<string>>(_data.Texts);
            _graphView.GraphLenguageChanged.AddListener(OnGraphViewLenguageChanged);
        }
        public override void Draw()
        {
            base.Draw();

            customContainer = new VisualElement();
            customContainer.AddToClassList("ds-node-custom-data-container");

            //Dialogue text foldout and text field
            textFoldout = ElementsUtilities.CreateFoldout("DialogueText");

            dialogueTextField = ElementsUtilities.CreateTextArea(CurrentText, null, callback =>
            {
                _texts.GetLenguageData(_graphView.GetEditorCurrentLenguage()).Data = callback.newValue;
            });

            dialogueTextField.AddToClassLists("ds-node-textfield", "ds-node-quote-textfield");

            textFoldout.Add(dialogueTextField);
            customContainer.Add(textFoldout);
            extensionContainer.Add(customContainer);
        }

        #region Overrides
        protected virtual void OnGraphViewLenguageChanged(LenguageType newLenguage)
        {
            dialogueTextField.SetValueWithoutNotify(CurrentText);
        }
        #endregion
    }
}
