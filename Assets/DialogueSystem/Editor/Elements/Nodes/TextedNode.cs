using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UIElements;

namespace DS.Editor.Elements
{
    using Editor.Enumerations;
    using Editor.Data;
    using Editor.Windows;
    using Editor.Utilities;


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

        protected TextField dialogueTextTextField;
        protected VisualElement customDataContainer;
        protected Foldout dialogueTextFoldout;

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
            //SetPosition(new Rect(data.Position, Vector2.zero));
            _graphView = context;
            SetNodeStyle();

            _texts = new System.Collections.Generic.List<LenguageData<string>>(_data.Texts);
            _graphView.GraphLenguageChanged.AddListener(OnGraphViewLenguageChanged);
            Debug.Log("Calling texted node initializer with data");
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
                _texts.GetLenguageData(_graphView.GetEditorCurrentLenguage()).Data = callback.newValue;
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
