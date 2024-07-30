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

    public class DataConversion
    {
        public List<DialogueChoice> NodeToDialogueChoice(List<ChoiceData> nodeChoices)
        {
            List<DialogueChoice> dialogueChoices = new();

            foreach (ChoiceData choiceData in nodeChoices)
            {
                DialogueChoice dialogueChoice = new() { ChoiceTexts = new(LenguageDataConvert(choiceData.ChoiceTexts)) };
                dialogueChoice.ChoiceID = choiceData.ChoiceID;

                var conditionContainer = ConditionToDialogueConditionContainer(choiceData.Conditions);
                dialogueChoice.Conditions = conditionContainer;

                dialogueChoices.Add(dialogueChoice);
            }
            return dialogueChoices;
        }
        public List<DS.Runtime.Data.LenguageData<string>> LenguageDataConvert(List<DS.Editor.Data.LenguageData<string>> lenguageDatas)
        {
            List<DS.Runtime.Data.LenguageData<string>> list = new();
            foreach (var data in lenguageDatas)
            {
                var newData = new DS.Runtime.Data.LenguageData<string>();
                newData.Data = data.Data;
                newData.LenguageType = (LenguageType)data.LenguageType;
                list.Add(newData);
            }
            return list;
        }
        public List<GameEvent> ConvertEvents(List<GameEventSO> eventSOs)
        {
            var events = new List<GameEvent>();
            foreach (var so in eventSOs)
            {
                var _event = new GameEvent();
                _event.eventName = so.eventName;
                events.Add(_event);
            }
            return events;
        }


        public DialogueVariableEventsContainer VarEventsToDialogueVarEvents(VariableEventsContainer container)
        {
            DialogueVariableEventsContainer eventsContainer = new();
            foreach (var _event in container.IntEvents)
            {
                var varEnum = GetEnumFromVariableName(_event.Variable.Name);
                var intEvent = new DialogueVariableEvent<int>(varEnum, (DS.Runtime.Events.VariableEventType)_event.EventType, _event.EventValue);
                eventsContainer.AddIntEvent(intEvent);
            }
            foreach (var _event in container.FloatEvents)
            {
                var varEnum = GetEnumFromVariableName(_event.Variable.Name);
                var floatEvent = new DialogueVariableEvent<float>(varEnum, (DS.Runtime.Events.VariableEventType)_event.EventType, _event.EventValue);
                eventsContainer.AddFloatEvent(floatEvent);
            }
            foreach (var _event in container.BoolEvents)
            {
                var varEnum = GetEnumFromVariableName(_event.Variable.Name);
                var boolEvent = new DialogueVariableEvent<bool>(varEnum, (DS.Runtime.Events.VariableEventType)_event.EventType, _event.EventValue);
                eventsContainer.AddBoolEvent(boolEvent);
            }
            return eventsContainer;
        }

        public DialogueConditionContainer ConditionToDialogueConditionContainer(ConditionsContainer container)
        {
            DialogueConditionContainer conditionContainer = new();
            foreach (var condition in container.IntConditions)
            {
                var varEnum = GetEnumFromVariableName(condition.Variable.Name);
                var intCondition = new IntDialogueCondition(varEnum, condition.ComparisonValue, (DS.Runtime.Conditions.ComparisonType)condition.ComparisonType);
                conditionContainer.AddIntCondition(intCondition);
            }
            foreach (var condition in container.FloatConditions)
            {
                var varEnum = GetEnumFromVariableName(condition.Variable.Name);
                var floatCondition = new FloatDialogueCondition(varEnum, condition.ComparisonValue, (DS.Runtime.Conditions.ComparisonType)condition.ComparisonType);
                conditionContainer.AddFloatCondition(floatCondition);
            }
            foreach (var condition in container.BoolConditions)
            {
                var varEnum = GetEnumFromVariableName(condition.Variable.Name);
                var boolCondition = new BoolDialogueCondition(varEnum, condition.ComparisonValue);
                conditionContainer.AddBoolCondition(boolCondition);
            }
            return conditionContainer;
        }
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
            return default(VariableEnum);
        }
    }
}