using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEditor.Experimental.GraphView;

namespace DS.Editor.Elements
{
    using Editor.Enumerations;
    using Editor.Data;
    using Editor.Windows;
    using Editor.ScriptableObjects;
    using DS.Editor.Utilities;

    public class EventNode : DialogueNode
    {
        [SerializeField] private List<ObjectField> objectFields;

        private EventNodeData data = new();
        public new EventNodeData Data { get { return data; } }

        protected Button addEventButton;


        #region Override
        public override void Initialize(string nodeName, DS_GraphView context, Vector2 spawnPosition)
        {
            base.Initialize(nodeName, context, spawnPosition);

            if(Data.NodeID == null || Data.NodeID == string.Empty) Data.NodeID = System.Guid.NewGuid().ToString();
            Data.Name = nodeName;
            Data.DialogueType = NodeType.Event;
            ChoiceData choiceData = new ChoiceData("EventOutput");
            Data.Choices.Add(choiceData);

            objectFields = new();
            Debug.Log($"Node ID is = {Data.NodeID}");
            Debug.Log($"Node Name is = {Data.Name}");
            Debug.Log($"Node group is = {Data.GroupID}");
            Debug.Log($"Node Type = {Data.DialogueType}");
            Debug.Log($"Node TextCount is = {Data.Texts.Count}");
            Debug.Log($"Created event node with {Data.Choices.Count}");
        }



        public override void Draw()
        {
            dialogueNameField = ElementsUtilities.CreateTextField(Data.Name, null, callback => OnDialogueNameChanged(callback));
            dialogueNameField.AddToClassLists("ds-node-textfield", "ds-node-filename-textfield", "ds-node-textfield_hidden");
            titleContainer.Insert(0, dialogueNameField);

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
        /// <summary>
        /// Callback called when the dialogue name changes.
        /// </summary>
        /// <param name="newDialogueName"></param>
        protected new void OnDialogueNameChanged(ChangeEvent<string> callback)
        {
            TextField target = (TextField)callback.target;
            target.value = callback.newValue.RemoveWhitespaces().RemoveSpecialCharacters();

            if (string.IsNullOrEmpty(target.value))
            {
                if (string.IsNullOrEmpty(Data.Name) == false)
                {
                    graphView.NameErrorsAmount++;
                }
            }
            else
            {
                if (string.IsNullOrEmpty(Data.Name) == true)
                {
                    graphView.NameErrorsAmount--;
                }
            }

            if (Group == null)
            {
                graphView.Remove_Node_FromUngrouped(this);
                Data.Name = target.value;
                graphView.Add_Node_ToUngrouped(this);
            }
            else
            {
                DS_Group groupRef = Group;
                graphView.Remove_Node_FromGroup(this, Group);
                Data.Name = target.value;
                graphView.Add_Node_ToGroup(this, groupRef);
            }
        }
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
