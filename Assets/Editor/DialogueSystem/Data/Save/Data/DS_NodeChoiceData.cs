using UnityEngine;
using System.Collections.Generic;


namespace DS.Editor.Data
{
    using Runtime.Data;
    /// <summary>
    /// Class that hold choice informations to be saved inside DS_Node_SaveData.
    /// </summary>
    [System.Serializable]
    public class DS_NodeChoiceData
    { 
        [SerializeField] private List<LenguageData<string>> choiceTexts;
        public List<LenguageData<string>> ChoiceTexts 
        {
            get {  return choiceTexts; } 
            set { choiceTexts = value; }
        }

        //Linked choice name?
        [SerializeField] private string nextNodeID;
        public string NextNodeID 
        {
            get { return nextNodeID; } 
            set {  nextNodeID = value; }
        }

        public DS_NodeChoiceData() 
        {
            ChoiceTexts = DS_LenguageUtilities.InitLenguageDataSet<string>();
        }
        public DS_NodeChoiceData(string defaultChoiceText)
        {
            ChoiceTexts = DS_LenguageUtilities.InitLenguageDataSet(defaultChoiceText);
        }
        public DS_NodeChoiceData(DS_NodeChoiceData choice)
        {
            ChoiceTexts = new List<LenguageData<string>>(choice.ChoiceTexts);
            NextNodeID = choice.NextNodeID;          
        }
    }
}
