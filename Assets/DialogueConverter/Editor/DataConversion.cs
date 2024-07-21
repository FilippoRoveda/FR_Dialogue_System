using System.Collections.Generic;

namespace Converter.Editor
{
    using DS.Editor.Data;
    using DS.Editor.ScriptableObjects;
    using DS.Runtime.Data;
    public class DataConversion
    {
        public List<DialogueChoiceData> NodeToDialogueChoice(List<ChoiceData> nodeChoices)
        {
            List<DialogueChoiceData> dialogueChoices = new();
            foreach (ChoiceData choiceData in nodeChoices)
            {
                DialogueChoiceData dialogueChoice = new() { ChoiceTexts = new(LenguageDataConvert(choiceData.ChoiceTexts)) };
                dialogueChoice.ChoiceID = choiceData.ChoiceID;
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
                newData.LenguageType = (DS.Runtime.Enumerations.LenguageType)data.LenguageType;
                list.Add(newData);
            }
            return list;
        }
        public List<GameEvent> ConvertEvents(List<DS_EventSO> eventSOs)
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
    }
}
