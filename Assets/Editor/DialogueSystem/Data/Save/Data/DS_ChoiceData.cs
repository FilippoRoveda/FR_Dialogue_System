using UnityEngine;
using System.Collections.Generic;


namespace DS.Editor.Data
{
    using Runtime.Data;
    /// <summary>
    /// Class that hold choice informations to be saved inside DS_Node_SaveData.
    /// </summary>
    [System.Serializable]
    public class DS_ChoiceData
    { 
        [SerializeField] private List<LenguageData<string>> choiceTexts;
        public List<LenguageData<string>> ChoiceTexts 
        {
            get {  return choiceTexts; } 
            set { choiceTexts = value; }
        }

        [SerializeField] private string nextNodeID;
        public string NextNodeID 
        {
            get { return nextNodeID; } 
            set {  nextNodeID = value; }
        }

        public DS_ChoiceData() 
        {
            ChoiceTexts = DS_LenguageUtilities.InitLenguageDataSet<string>();
        }
        public DS_ChoiceData(string defaultChoiceText)
        {
            ChoiceTexts = DS_LenguageUtilities.InitLenguageDataSet(defaultChoiceText);
        }
        public DS_ChoiceData(DS_ChoiceData choice)
        {
            ChoiceTexts = new List<LenguageData<string>>(choice.ChoiceTexts);
            NextNodeID = choice.NextNodeID;          
        }
    }
}
