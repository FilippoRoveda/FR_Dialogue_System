using System.Collections.Generic;

namespace Converter.Editor
{
    using DS.Editor.Data;
    using DS.Editor.Conditions;
    using DS.Editor.Events;

    using DS.Runtime.Data;
    using DS.Runtime.Conditions;
    using DS.Runtime.Enumerations;
    using DS.Runtime.Events;


    using VariableEnum = Variables.Generated.VariablesGenerated.VariablesKey;
    using EditorLenguageData = DS.Editor.Data.LenguageData<string>;
    using RuntimeLenguageData = DS.Runtime.Data.LenguageData<string>;

    /// <summary>
    /// Utility class that handle data conversion between Editor only datas and runtime datas.
    /// </summary>
    public class DataConversion
    {
        /// <summary>
        /// Convert node choices to runtime dialogue choices.
        /// </summary>
        /// <param name="nodeChoices"></param>
        /// <returns></returns>iu
        public List<DialogueChoice> ConvertNodeChoices(List<ChoiceData> nodeChoices)
        {
            List<DialogueChoice> dialogueChoices = new();

            foreach (ChoiceData choiceData in nodeChoices)
            {
                DialogueChoice dialogueChoice = new() 
                { 
                    ChoiceTexts = new(ConvertLenguageData(choiceData.ChoiceTexts)),
                    ChoiceID = choiceData.ChoiceID,
                    Conditions = ConvertConditions(choiceData.Conditions)
                };
                dialogueChoices.Add(dialogueChoice);
            }
            return dialogueChoices;
        }

        /// <summary>
        /// Convert Editor Lenguage string datas to Runtime lenguage datas to implement multilenguage.
        /// </summary>
        /// <param name="lenguageDatas"></param>
        /// <returns></returns>
        public List<RuntimeLenguageData> ConvertLenguageData(List<EditorLenguageData> lenguageDatas)
        {
            List<RuntimeLenguageData> runtimeLenguageDatas = new();
            foreach (var data in lenguageDatas)
            {
                var newData = new RuntimeLenguageData();
                newData.Data = data.Data;
                newData.LenguageType = (LenguageType)data.LenguageType;
                runtimeLenguageDatas.Add(newData);
            }
            return runtimeLenguageDatas;
        }
        /// <summary>
        /// Convert GameEvent editor scriptable object to GameEvent class usefull at runtime.
        /// </summary>
        /// <param name="eventSOs"></param>
        /// <returns></returns>
        public List<GameEvent> ConvertGameEvents(List<GameEventSO> eventSOs)
        {
            var events = new List<GameEvent>();
            foreach (var so in eventSOs)
            {
                var _event = new GameEvent();
                _event._eventString = so.eventExecutionString;
                events.Add(_event);
            }
            return events;
        }

        /// <summary>
        /// Convert Editor variables events container in to runtime Dialogue variable event container.
        /// </summary>
        /// <param name="editorEvents"></param>
        /// <returns></returns>
        public DialogueVariableEvents ConvertVariableEvents(VariableEvents editorEvents)
        {
            DialogueVariableEvents dialogueEvents = new();
            foreach (var _event in editorEvents.IntEvents)
            {
                var varEnum = GetEnumFromVariableName(_event.Variable.Name);
                var intEvent = new DialogueVariableEvent<int>(varEnum, (DS.Runtime.Events.VariableEventType)_event.EventType, _event.EventValue);
                dialogueEvents.AddIntEvent(intEvent);
            }
            foreach (var _event in editorEvents.FloatEvents)
            {
                var varEnum = GetEnumFromVariableName(_event.Variable.Name);
                var floatEvent = new DialogueVariableEvent<float>(varEnum, (DS.Runtime.Events.VariableEventType)_event.EventType, _event.EventValue);
                dialogueEvents.AddFloatEvent(floatEvent);
            }
            foreach (var _event in editorEvents.BoolEvents)
            {
                var varEnum = GetEnumFromVariableName(_event.Variable.Name);
                var boolEvent = new DialogueVariableEvent<bool>(varEnum, (DS.Runtime.Events.VariableEventType)_event.EventType, _event.EventValue);
                dialogueEvents.AddBoolEvent(boolEvent);
            }
            return dialogueEvents;
        }

        /// <summary>
        /// Convert editor side Conditions in to runtime DialogueConditions
        /// </summary>
        /// <param name="editorConditions"></param>
        /// <returns></returns>
        public DialogueConditions ConvertConditions(Conditions editorConditions)
        {
            DialogueConditions dialogueConditions = new();
            foreach (var condition in editorConditions.IntConditions)
            {
                var varEnum = GetEnumFromVariableName(condition.Variable.Name);
                var intCondition = new IntDialogueCondition(varEnum, condition.ComparisonValue, (DS.Runtime.Conditions.ComparisonType)condition.ComparisonType);
                dialogueConditions.AddIntCondition(intCondition);
            }
            foreach (var condition in editorConditions.FloatConditions)
            {
                var varEnum = GetEnumFromVariableName(condition.Variable.Name);
                var floatCondition = new FloatDialogueCondition(varEnum, condition.ComparisonValue, (DS.Runtime.Conditions.ComparisonType)condition.ComparisonType);
                dialogueConditions.AddFloatCondition(floatCondition);
            }
            foreach (var condition in editorConditions.BoolConditions)
            {
                var varEnum = GetEnumFromVariableName(condition.Variable.Name);
                var boolCondition = new BoolDialogueCondition(varEnum, condition.ComparisonValue);
                dialogueConditions.AddBoolCondition(boolCondition);
            }
            return dialogueConditions;
        }

        /// <summary>
        /// Get the specific VaribaleEnum, if existing, from a variable name.
        /// </summary>
        /// <param name="variableName"></param>
        /// <returns></returns>
        private VariableEnum GetEnumFromVariableName(string variableName)
        {
            string enumKey = variableName.ToUpper();
            enumKey = enumKey.Replace(" ", "_");
            foreach (VariableEnum lenguage in (VariableEnum[])System.Enum.GetValues(typeof(VariableEnum)))
            {
                if(lenguage.ToString() == enumKey)
                {
                    return lenguage;
                }
            }
            return default;
        }
    }
}
