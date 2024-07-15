using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.Search;
using UnityEditor;

namespace DS.Editor.Windows.Elements
{
    using Runtime.Data;
    using Editor.Data;
    using Runtime.ScriptableObjects;
    using Enums;


    public class DS_EventNode : DS_BaseNode
    {
        [SerializeField] private List<ObjectField> objectFields;
        [SerializeField] private List<DS_EventSO> dialogueEvents;

        public List<DS_EventSO> DialogueEvents 
        { get { return dialogueEvents; } set { dialogueEvents = value; } }


        #region Override
        public override void Initialize(string nodeName, DS_GraphView context, Vector2 spawnPosition)
        {
            base.Initialize(nodeName, context, spawnPosition);

            Texts = LenguageUtilities.InitLenguageDataSet("Event Text");
            SetDialogueType(DialogueType.Event);
            ChoiceData choiceData = new ChoiceData("Event Output");
            Choices.Add(choiceData);

            objectFields = new();
            dialogueEvents = new();
        }

        public override void Draw()
        {
            base.Draw();
            CreateInputPort("Event Connection");
            
            Button addEventButton = ElementsUtilities.CreateButton("Add Event", () => OnAddEventButtonPressed());
            addEventButton.AddToClassList("ds-node-button");
            mainContainer.Insert(1, addEventButton);
            
            foreach(var _event in dialogueEvents)
            {
                ObjectField field = CreateObjectField(_event);
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
            if (eventSO != null && dialogueEvents.Contains(eventSO))
            {
                dialogueEvents.Remove(eventSO);
            }
            mainContainer.Remove(objectField);
        }
        #endregion

        #region Elements creation
        private ObjectField CreateObjectField(DS_EventSO _event = null)
        {
            ObjectField objectField = new ObjectField()
            {
                objectType = typeof(DS_EventSO),
                value = _event
            };

            objectField.RegisterValueChangedCallback(OnFieldEventChanged(_event, objectField));
            objectField.SetValueWithoutNotify(_event);


            Button deleteEventButton = ElementsUtilities.CreateButton("X", () => OnDeleteEventPressed(objectField, (DS_EventSO)objectField.value));
            deleteEventButton.AddToClassList("ds-node-button");
            objectField.Add(deleteEventButton);

            return objectField;
        }

        private EventCallback<ChangeEvent<Object>> OnFieldEventChanged( DS_EventSO _event, ObjectField objectField)
        {
            return value =>
            {
                _event = objectField.value as DS_EventSO;
                if (objectField.value == null && dialogueEvents.Contains(_event) == false)
                {
                    objectField.value = _event;
                    dialogueEvents.Add(_event);
                }
                else if (objectField.value != null && dialogueEvents.Contains(_event) == false)
                {
                    dialogueEvents.Remove((DS_EventSO)objectField.value);
                    objectField.value = _event;
                    dialogueEvents.Add(_event);
                }
                else
                {
                    EditorUtility.DisplayDialog("Event Repetition Error", "You could not add the same event more than once for the same node!", "Close");
                }
            };
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
