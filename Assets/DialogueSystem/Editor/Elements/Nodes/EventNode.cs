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
    using Variables.Editor;

    public class EventNode : DialogueNode
    {
        public List<GameEventSO> gameEvents;
        public VariableEventsContainer variableEvents;

        [SerializeField] private List<ObjectField> objectFields;
        //[SerializeField] private List<ObjectField> varEventFields;

        protected Button addEventButton;
        protected ToolbarMenu addVariableEventMenu;

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

            gameEvents = new List<GameEventSO>();
            variableEvents = new VariableEventsContainer();

            objectFields = new();
            //varEventFields = new();
        }

        public void Initialize(EventNodeData _data, DS_GraphView context)
        {
            _nodeID = _data.NodeID;
            _nodeName = _data.Name;
            _position = _data.Position;
            SetPosition(new Rect(_position, Vector2.zero));
            _graphView = context;
            SetNodeStyle();

            _texts = new List<LenguageData<string>>(_data.Texts);
            _graphView.GraphLenguageChanged.AddListener(OnGraphViewLenguageChanged);

            _choices = new List<ChoiceData>(_data.Choices);

            if (_data.Events == null || _data.Events.Count == 0) gameEvents = new();
            else gameEvents = new List<GameEventSO>(_data.Events);

            variableEvents = new VariableEventsContainer();
            variableEvents.Reload(_data.EventsContainer);

            objectFields = new();
            //varEventFields = new();
        }

        public override void Draw()
        {
            base.Draw();

            CreateInputPort("Event Connection");
            

            Box box = new Box();
            box.style.flexDirection = FlexDirection.Row;
            box.style.alignSelf = Align.Center;

            addEventButton = ElementsUtilities.CreateButton("Add Game Event", () => OnAddEventButtonPressed());
            addEventButton.AddToClassList("ds-node-button");
            
            box.Add(addEventButton);

            addVariableEventMenu = new ToolbarMenu();
            addVariableEventMenu.text = "ADD VARIABLE EVENT";
            addVariableEventMenu.menu.AppendAction("Int Event", callback => { ElementsUtilities.AddIntVarEvent(variableEvents, mainContainer); });
            addVariableEventMenu.menu.AppendAction("Float Event", callback => { ElementsUtilities.AddFloatVarEvent(variableEvents, mainContainer); });
            addVariableEventMenu.menu.AppendAction("Bool Event", callback => { ElementsUtilities.AddBoolVarEvent(variableEvents, mainContainer); });

            box.Add(addVariableEventMenu);

            mainContainer.Insert(1, box);

            foreach (var _event in gameEvents)
            {
                ObjectField field = CreateObjectField(_event);
                objectFields.Add(field);
                mainContainer.Add(field);
            }

            foreach (var intEvent in variableEvents.IntEvents) { ElementsUtilities.AddIntVarEvent(variableEvents, contentContainer, intEvent); }
            foreach (var floatEvent in variableEvents.FloatEvents) { ElementsUtilities.AddFloatVarEvent(variableEvents, contentContainer, floatEvent); }
            foreach (var boolEvent in variableEvents.BoolEvents) { ElementsUtilities.AddBoolVarEvent(variableEvents, contentContainer, boolEvent); }


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
            objectFields.Add(objectField);
            mainContainer.Add(objectField);
        }
        private void OnDeleteEventPressed(ObjectField objectField, GameEventSO eventSO)
        {
            if (objectFields.Count == 1) return;

            objectFields.Remove(objectField);
            if (eventSO != null && gameEvents.Contains(eventSO))
            {
                gameEvents.Remove(eventSO);
            }
            objectFields.Remove(objectField);
            mainContainer.Remove(objectField);
        }
        private EventCallback<ChangeEvent<Object>> OnFieldEventChanged(GameEventSO _event, ObjectField objectField)
        {
            return value =>
            {
                _event = objectField.value as GameEventSO;
                if (objectField.value == null && gameEvents.Contains(_event) == false)
                {
                    objectField.value = _event;
                    gameEvents.Add(_event);
                }
                else if (objectField.value != null && gameEvents.Contains(_event) == false)
                {
                    gameEvents.Remove((GameEventSO)objectField.value);
                    objectField.value = _event;
                    gameEvents.Add(_event);
                }
                else
                {
                    EditorUtility.DisplayDialog("Event Repetition Error", "You could not add the same event more than once for the same node!", "Close");
                }
            };
        }
        #endregion

        #region Elements creation
        private ObjectField CreateObjectField(GameEventSO _event = null)
        {
            ObjectField objectField = new ObjectField()
            {
                objectType = typeof(GameEventSO),
                allowSceneObjects = true,
                value = _event
            };

            objectField.RegisterValueChangedCallback(OnFieldEventChanged(_event, objectField));
            objectField.SetValueWithoutNotify(_event);


            Button deleteEventButton = ElementsUtilities.CreateButton("X", () => OnDeleteEventPressed(objectField, (GameEventSO)objectField.value));
            deleteEventButton.AddToClassList("ds-node-button");
            objectField.Add(deleteEventButton);

            return objectField;
        }    
        #endregion
    }
}
