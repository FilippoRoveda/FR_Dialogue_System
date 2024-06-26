using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.Search;

namespace DS.Elements
{
    using Data.Save;
    using ScriptableObjects;
    using Utilities;
    using Enumerations;
    using Windows;
    using UnityEditor;

    public class DS_EventNode : DS_BaseNode
    {
        private List<ObjectField> objectFields;
        [SerializeField] private List<DS_DialogueEventSO> dialogueEvents;
        public List<DS_DialogueEventSO> DialogueEvents 
        { get { return dialogueEvents; } set { dialogueEvents = value; } }

        public override void Initialize(string nodeName, DS_GraphView context, Vector2 spawnPosition)
        {
            base.Initialize(nodeName, context, spawnPosition);

            Texts = DS_LenguageUtilities.InitLenguageDataSet("Event Text");
            SetDialogueType(DS_DialogueType.Event);
            DS_NodeChoiceData choiceData = new DS_NodeChoiceData("Event Output");
            Choices.Add(choiceData);

            objectFields = new();
            dialogueEvents = new();
        }

        public override void Draw()
        {
            base.Draw();
            CreateInputPort("Event Connection");
            
            Button addEventButton = DS_ElementsUtilities.CreateButton("Add Event", () => OnAddEventButtonPressed());
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

        private void OnAddEventButtonPressed()
        {
            ObjectField objectField = CreateObjectField();
            mainContainer.Add(objectField);
        }

        private ObjectField CreateObjectField(DS_DialogueEventSO _event = null)
        {
            ObjectField objectField = new ObjectField()
            {
                objectType = typeof(DS_DialogueEventSO),              
                value = _event
            };

            objectField.RegisterValueChangedCallback(value => 
            {
                _event = objectField.value as DS_DialogueEventSO;
                if (objectField.value == null && dialogueEvents.Contains(_event) == false)
                {
                    objectField.value = _event;
                    dialogueEvents.Add(_event);
                }
                else if(objectField.value != null && dialogueEvents.Contains(_event) == false)
                {
                    dialogueEvents.Remove((DS_DialogueEventSO)objectField.value);
                    objectField.value = _event;
                    dialogueEvents.Add(_event);
                }
                else
                {
                    EditorUtility.DisplayDialog("Event Repetition Error", "You could not add the same event more than once for the same node!", "Close");
                }
                
            });
            objectField.SetValueWithoutNotify(_event);


            Button deleteEventButton = DS_ElementsUtilities.CreateButton("X", () => OnDeleteEventPressed(objectField, (DS_DialogueEventSO)objectField.value));
            deleteEventButton.AddToClassList("ds-node-button");
            objectField.Add(deleteEventButton);

            return objectField;
        }

        private void OnDeleteEventPressed(ObjectField objectField, DS_DialogueEventSO eventSO)
        {
            if (objectFields.Count == 1) return;
            objectFields.Remove(objectField);
            if (eventSO != null && dialogueEvents.Contains(eventSO))
            {
                dialogueEvents.Remove(eventSO);
            }
            mainContainer.Remove(objectField);
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
