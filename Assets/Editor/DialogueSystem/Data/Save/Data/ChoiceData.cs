using UnityEngine;
using System.Collections.Generic;


namespace DS.Editor.Data
{
    using Runtime.Data;
    using System;

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

        public ChoiceData() 
        {
            choiceID = Guid.NewGuid().ToString();
            ChoiceTexts = LenguageUtilities.InitLenguageDataSet<string>();
        }
        public ChoiceData(string defaultChoiceText)
        {
            choiceID = Guid.NewGuid().ToString();
            ChoiceTexts = LenguageUtilities.InitLenguageDataSet(defaultChoiceText);
        }
        public ChoiceData(ChoiceData choice)
        {
            choiceID = choice.ChoiceID;
            ChoiceTexts = new List<LenguageData<string>>(choice.ChoiceTexts);
            NextNodeID = choice.NextNodeID;          
        }
    }
}