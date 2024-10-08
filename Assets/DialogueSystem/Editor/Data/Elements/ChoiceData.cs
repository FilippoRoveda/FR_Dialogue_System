using System;
using UnityEngine;
using System.Collections.Generic;


namespace DS.Editor.Data
{
    using Editor.Conditions;
    /// <summary>
    /// Class that hold choice informations to be saved inside DS_Node_SaveData.
    /// </summary>
    [System.Serializable]
    public class ChoiceData
    {

        [SerializeField] private List<LenguageData<string>> choiceTexts;
        public List<LenguageData<string>> ChoiceTexts 
        {
            get {  return choiceTexts; } 
            set { choiceTexts = value; }
        }

        [SerializeField] private string choiceID;
        public string ChoiceID
        {
            get { return choiceID; }
        }

        [SerializeField] private string nextNodeID;
        public string NextNodeID 
        {
            get { return nextNodeID; } 
            set {  nextNodeID = value; }
        }

        [SerializeField] private Conditions conditions;
        public Conditions Conditions { get { return conditions; } }

        public ChoiceData() 
        {
            choiceID = Guid.NewGuid().ToString();
            ChoiceTexts = LenguageUtilities.InitLenguageDataSet<string>();
            conditions = new Conditions();
            conditions.Initialize();
        }
        public ChoiceData(string defaultChoiceText)
        {
            choiceID = Guid.NewGuid().ToString();
            ChoiceTexts = LenguageUtilities.InitLenguageDataSet(defaultChoiceText);
            conditions = new Conditions();
            conditions.Initialize();
        }
        public ChoiceData(ChoiceData choice)
        {
            choiceID = choice.ChoiceID;
            choiceTexts = new List<LenguageData<string>>(choice.ChoiceTexts);
            nextNodeID = choice.NextNodeID;
            conditions = new Conditions();
            conditions.Reload(choice.Conditions);
        }
        public void UpdateTextsLenguages()
        {
            ChoiceTexts = LenguageUtilities.UpdateLenguageDataSet(ChoiceTexts);
        }
    }
}
