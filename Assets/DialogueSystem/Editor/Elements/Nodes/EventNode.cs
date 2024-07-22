using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.UIElements;

namespace DS.Editor.Elements
{
    using Editor.Enumerations;
    using Editor.Data;
    using Editor.Windows;
    using Editor.ScriptableObjects;

    public class EventNode : DialogueNode
    {
        public List<DS_EventSO> _events;

        [SerializeField] private List<ObjectField> objectFields;
        protected Button addEventButton;

        public EventNode() { }

        #region Override
        public override void Initialize(string nodeName, DS_GraphView context, Vector2 spawnPosition)
        {
            _nodeID = System.Guid.NewGuid().ToString();
            _nodeName = nodeName;
            _position = spawnPosition;
            SetPosition(new Rect(spawnPosition, Vector2.zero));
            _graphView = context;
            SetNodeStyle();

            _texts = LenguageUtilities.InitLenguageDataSet("Event Dialogue Text");
            _graphView.GraphLenguageChanged.AddListener(OnGraphViewLenguageChanged);

            _nodeType = NodeType.Event;
            _choices = new();
            ChoiceData choiceData = new ChoiceData("EventOutput");
            _choices.Add(choiceData);

            _events = new List<DS_EventSO>();
            objectFields = new();
        }

        public void Initialize(EventNodeData _data, DS_GraphView context)
        {
            _nodeID = _data.NodeID;
            _nodeName = _data.Name;
            _position = _data.Position;
            SetPosition(new Rect(_position, Vector2.zero));
            _graphView = context;
            SetNodeStyle();

            _texts = new System.Collections.Generic.List<LenguageData<string>>(_data.Texts);
            _graphView.GraphLenguageChanged.AddListener(OnGraphViewLenguageChanged);

            _choices = new List<ChoiceData>(_data.Choices);
            if (_data.Events == null || _data.Events.Count == 0) _events = new();
            else _events = new List<DS_EventSO>(_data.Events);
            Debug.Log("Calling event node initializer with data");
            objectFields = new();
        }

        public override void Draw()
        {
            base.Draw();

            CreateInputPort("Event Connection");
            
            addEventButton = ElementsUtilities.CreateButton("Add Event", () => OnAddEventButtonPressed());
            addEventButton.AddToClassList("ds-node-button");
            mainContainer.Insert(1, addEventButton);
            
            foreach(var _event in _events)
            {
                ObjectField field = CreateObjectField(_event);
                objectFields.Add(field);
                mainContainer.Add(field);
            }

            CreateOutputPortFromChoices();
            RefreshExpandedState();
        }
        
        protected override void SetNodeStyle()
        {
            extensionContainer.AddToClassList("ds-node_extension-container");
            mainContainer.AddToClassList("ds-event-node_main-container");
            SetDefaultColor(mainContainer.style.backgroundColor);
        }

        #endregion

        #region Callbacks

        private void OnAddEventButtonPressed()
        {
            ObjectField objectField = CreateObjectField();
            mainContainer.Add(objectField);
        }
        private void OnDeleteEventPressed(ObjectField objectField, DS_EventSO eventSO)
        {
            if (objectFields.Count == 1) return;
            objectFields.Remove(objectField);
            if (eventSO != null && _events.Contains(eventSO))
            {
                _events.Remove(eventSO);
            }
            objectFields.Remove(objectField);
            mainContainer.Remove(objectField);
        }
        private EventCallback<ChangeEvent<Object>> OnFieldEventChanged(DS_EventSO _event, ObjectField objectField)
        {
            return value =>
            {
                _event = objectField.value as DS_EventSO;
                if (objectField.value == null && _events.Contains(_event) == false)
                {
                    objectField.value = _event;
                    _events.Add(_event);
                }
                else if (objectField.value != null && _events.Contains(_event) == false)
                {
                    _events.Remove((DS_EventSO)objectField.value);
                    objectField.value = _event;
                    _events.Add(_event);
                }
                else
                {
                    EditorUtility.DisplayDialog("Event Repetition Error", "You could not add the same event more than once for the same node!", "Close");
                }
            };
        }
        #endregion

        #region Elements creation
        private ObjectField CreateObjectField(DS_EventSO _event = null)
        {
            ObjectField objectField = new ObjectField()
            {
                objectType = typeof(DS_EventSO),
                allowSceneObjects = true,
                value = _event
            };

            objectField.RegisterValueChangedCallback(OnFieldEventChanged(_event, objectField));
            objectField.SetValueWithoutNotify(_event);


            Button deleteEventButton = ElementsUtilities.CreateButton("X", () => OnDeleteEventPressed(objectField, (DS_EventSO)objectField.value));
            deleteEventButton.AddToClassList("ds-node-button");
            objectField.Add(deleteEventButton);

            return objectField;
        }    
        #endregion
    }
}
