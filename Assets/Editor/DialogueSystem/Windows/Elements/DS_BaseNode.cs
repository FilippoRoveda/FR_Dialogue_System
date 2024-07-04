using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using System.Linq;


namespace DS.Editor.Windows.Elements
{
    using Editor.Data;
    using Runtime.Data;
    using Enums;
    using Editor.Utilities;


    /// <summary>
    /// Base dialogue system node class.
    /// </summary>
    public class DS_BaseNode : Node
    {
        [SerializeField] public string ID {  get; set; }
        [SerializeField] public string DialogueName { get; set; }

        /// <summary>
        /// List of DS_Choice_SaveData representing the output choice for the node.
        /// </summary>
        [SerializeField] public List<DS_ChoiceData> Choices;
        /// <summary>
        /// Content text of this node.
        /// </summary>
        [SerializeField] public List<LenguageData<string>> Texts = new List<LenguageData<string>>();
        /// <summary>
        /// 
        /// </summary>
        public string CurrentText
        {
            get
            {
                return Texts.GetLenguageData(graphView.GetEditorCurrentLenguage()).Data;
            }
            private set 
            {
                Texts.SetLenguageData(graphView.GetEditorCurrentLenguage(), value);
            }
        }
        [SerializeField] public DS_DialogueType DialogueType { get; private set; }
        [SerializeField] public DS_Group Group { get; set; } //Da far diventare group ID come stringa


        protected TextField dialogueTextTextField;

        protected DS_GraphView graphView;
        protected StyleColor defaultColor;


        public virtual void Initialize(string nodeName, DS_GraphView context, Vector2 spawnPosition)
        {
            ID = Guid.NewGuid().ToString();
            DialogueName = nodeName;        
            Choices = new List<DS_ChoiceData>();
            SetPosition(new Rect(spawnPosition, Vector2.zero));
            graphView = context;

            Texts = DS_LenguageUtilities.InitLenguageDataSet("Dialogue Text"); ///
            SetNodeStyle();
            graphView.GraphLenguageChanged.AddListener(OnGraphViewLenguageChanged);
        }



        public virtual void Draw()
        {
            //Dialogue name text field 
            TextField dialogueNameField = DS_ElementsUtilities.CreateTextField(DialogueName, null, callback => OnDialogueNameChanged(callback));

            dialogueNameField.AddToClassLists("ds-node-textfield", "ds-node-filename-textfield", "ds-node-textfield_hidden");

            titleContainer.Insert(0, dialogueNameField);
           
            VisualElement customDataContainer = new VisualElement();
            customDataContainer.AddToClassList("ds-node-custom-data-container");

            //Dialogue text foldout and text field
            Foldout dialogueTextFoldout = DS_ElementsUtilities.CreateFoldout("DialogueText");

            dialogueTextTextField = DS_ElementsUtilities.CreateTextArea(CurrentText, null, callback =>
            {
                Texts.GetLenguageData(graphView.GetEditorCurrentLenguage()).Data = callback.newValue; 
            });

            dialogueTextTextField.AddToClassLists("ds-node-textfield", "ds-node-quote-textfield");

            dialogueTextFoldout.Add(dialogueTextTextField);
            customDataContainer.Add(dialogueTextFoldout);
            extensionContainer.Add(customDataContainer);
        }



        #region Overrides
        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            evt.menu.AppendAction("Disconnect Input Ports", actionEvent => DisconnectPorts(inputContainer));
            evt.menu.AppendAction("Disconnect Output Ports", actionEvent => DisconnectPorts(outputContainer));
            base.BuildContextualMenu(evt);
        }

        public override void SetPosition(Rect newPos)
        {
            base.SetPosition(newPos);
        }
        #endregion

        #region Callbacks

        /// <summary>
        /// Callback called when the dialogue name changes.
        /// </summary>
        /// <param name="newDialogueName"></param>
        protected void OnDialogueNameChanged(ChangeEvent<string> callback)
        {
            TextField target = (TextField)callback.target;
            target.value = callback.newValue.RemoveWhitespaces().RemoveSpecialCharacters();

            if(string.IsNullOrEmpty(target.value))
            {
                if(string.IsNullOrEmpty(DialogueName) == false)
                {
                    graphView.NameErrorsAmount++;
                }
            }
            else
            {
                if (string.IsNullOrEmpty(DialogueName) == true)
                {
                    graphView.NameErrorsAmount--;
                }
            }

            if (Group == null)
            {
                graphView.Remove_Node_FromUngrouped(this);
                DialogueName = target.value;
                graphView.Add_Node_ToUngrouped(this);
            }
            else
            {
                DS_Group groupRef = Group;
                graphView.Remove_Node_FromGroup(this, Group);
                DialogueName = target.value;
                graphView.Add_Node_ToGroup(this, groupRef);
            }
        }

        protected virtual void OnGraphViewLenguageChanged(DS_LenguageType newLenguage)
        {
            dialogueTextTextField.SetValueWithoutNotify(CurrentText);

            foreach (var element in outputContainer.Children())
            {
                var port = (Port)element;
                var field = port.contentContainer.Children().ToList().Find(x => x.GetType() == typeof(TextField)) as TextField;
                field.SetValueWithoutNotify(((DS_ChoiceData)port.userData).ChoiceTexts.Find(x => x.LenguageType == newLenguage).Data);
            }
        }
        #endregion

        #region Appearence style
        protected virtual void SetNodeStyle()
        {
            extensionContainer.AddToClassList("ds-node_extension-container");
            mainContainer.AddToClassList("ds-node_main-container");
            SetDefaultColor(mainContainer.style.backgroundColor);
        }
        public void SetDefaultColor(StyleColor defaultColor)
        {
            this.defaultColor = defaultColor;

        }
        public void SetErrorStyle(Color errorColor)
        {
            mainContainer.style.backgroundColor = errorColor;
        }
        public void ResetStyle()
        {
            mainContainer.style.backgroundColor = defaultColor;
        }
        #endregion

        #region Node parts creation
        protected void CreateInputPort(string inputPortName = "DialogueConnection", Port.Capacity capacity = Port.Capacity.Multi)
        {
            Port choicePort = this.CreatePort(inputPortName, Orientation.Horizontal, Direction.Input, capacity);
            choicePort.portName = inputPortName;
            inputContainer.Add(choicePort);
        }

        protected void CreateOutputPortFromChoices()
        {
            foreach (DS_ChoiceData choice in Choices)
            {
                CreateChoicePort(choice);
            }
        }

        protected virtual Port CreateChoicePort(object _choice)
        {
            DS_ChoiceData choice = (DS_ChoiceData)_choice;

            Port choicePort = this.CreatePort(choice.ChoiceTexts.GetLenguageData(graphView.GetEditorCurrentLenguage()).Data,
                                Orientation.Horizontal, Direction.Output, Port.Capacity.Single);
            choicePort.portName = "";
            choicePort.userData = choice;

            TextField choiceTextField = DS_ElementsUtilities.CreateTextField(choice.ChoiceTexts.GetLenguageData(graphView.GetEditorCurrentLenguage()).Data,
                                            null,
                                            callback => UpdateChoiceLenguageData(callback, choice));


            choiceTextField.AddToClassLists("ds-node-textfield", "ds-node-choice-textfield", "ds-node-textfield_hidden");
            choiceTextField.style.flexDirection = FlexDirection.Column;

            choicePort.Insert(1, choiceTextField);
            outputContainer.Add(choicePort);
            return choicePort;
        }


        #endregion

        #region Utilities

        /// <summary>
        /// Set the variable which indicates the group that owns this DS_Node.
        /// </summary>
        /// <param name="group"></param>
        public void SetGroup(DS_Group group)
        {
            Group = group;
        }
        public void RemoveFromGroup()
        {
            Group = null;
        }
        protected void SetDialogueType(DS_DialogueType dialogueType)
        {
            DialogueType = dialogueType;
        }

        protected void AddNodeChoice(string choiceText)
        {
            DS_ChoiceData choiceData = new DS_ChoiceData(choiceText);
            Choices.Add(choiceData);
        }

        /// <summary>
        /// Disconnect all ports in the passed container.
        /// </summary>
        /// <param name="container"></param>
        public void DisconnectPorts(VisualElement container)
        {
            foreach(Port port in container.Children())
            {
                if(port.connected == true)
                {
                    graphView.DeleteElements(port.connections);
                }
            }
        }
        /// <summary>
        /// Disconnect all ports in both input container and output container.
        /// </summary>
        public void DisconnectAllPorts()
        {
            DisconnectPorts(inputContainer);
            DisconnectPorts(outputContainer);
        }

        protected void UpdateChoiceLenguageData(ChangeEvent<string> callback, DS_ChoiceData choice)
        {
            choice.ChoiceTexts.Find(x => x.LenguageType == graphView.GetEditorCurrentLenguage()).Data = callback.newValue;
        }

        /// <summary>
        /// Return true if this node is a starting node.
        /// </summary>
        /// <returns></returns>
        public virtual bool IsStartingNode()
        {
            Port inputPort = (Port) inputContainer.Children().First();
            return !inputPort.connected;
        }

        /// <summary>
        /// Return true if the node is overlapping with the passed node.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public bool IsOverlapping(DS_BaseNode node)
        {
            return GetPosition().Overlaps(node.GetPosition());
        }
        #endregion
    }
}
