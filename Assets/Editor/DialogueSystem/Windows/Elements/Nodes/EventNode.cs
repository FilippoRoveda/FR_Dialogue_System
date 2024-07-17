using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;

namespace DS.Editor.Elements
{
    using Enums;
    using Runtime.ScriptableObjects;
    using Editor.Data;
    using Editor.Windows;
    using UnityEditor.UIElements;
    using UnityEditor.Experimental.GraphView;

    public class EventNode : DialogueNode
    {
        [SerializeField] private List<ObjectField> objectFields;

        [SerializeField] private EventNodeData data = new();
        public new EventNodeData Data { get { return data; } }

        protected Button addEventButton;


        #region Override
        public override void Initialize(string nodeName, DS_GraphView context, Vector2 spawnPosition)
        {
            base.Initialize(nodeName, context, spawnPosition);
            SetDialogueType(DialogueType.Event);

            ChoiceData choiceData = new ChoiceData("EventOutput");
            Data.Choices.Add(choiceData);

            objectFields = new();     
        }

        public override void Draw()
        {
            base.Draw();
            CreateInputPort("Event Connection");
            
            addEventButton = ElementsUtilities.CreateButton("Add Event", () => OnAddEventButtonPressed());
            addEventButton.AddToClassList("ds-node-button");
            mainContainer.Insert(1, addEventButton);
            
            foreach(var _event in data.Events)
            {
                ObjectField field = CreateObjectField(_event);
                objectFields.Add(field);
                mainContainer.Add(field);
            }

            this.CreateOutputPortFromChoices();
            RefreshExpandedState();
        }
        protected override List<Port> CreateOutputPortFromChoices()
        {
            List<Port> choices = new List<Port>();
            foreach (ChoiceData choice in Data.Choices)
            {
                choices.Add(CreateChoicePort(choice));
            }
            return choices;
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
            if (eventSO != null && Data.Events.Contains(eventSO))
            {
                Data.Events.Remove(eventSO);
            }
            objectFields.Remove(objectField);
            mainContainer.Remove(objectField);
        }

        private EventCallback<ChangeEvent<Object>> OnFieldEventChanged(DS_EventSO _event, ObjectField objectField)
        {
            return value =>
            {
                _event = objectField.value as DS_EventSO;
                if (objectField.value == null && Data.Events.Contains(_event) == false)
                {
                    objectField.value = _event;
                    Data.Events.Add(_event);
                }
                else if (objectField.value != null && Data.Events.Contains(_event) == false)
                {
                    Data.Events.Remove((DS_EventSO)objectField.value);
                    objectField.value = _event;
                    Data.Events.Add(_event);
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
