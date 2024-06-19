using UnityEngine;


namespace DS.Data.Save
{
    /// <summary>
    /// Class that hold choice informations to be saved inside DS_Node_SaveData.
    /// </summary>
    [System.Serializable]
    public class DS_NodeChoiceData
    {
        [SerializeField] private string choiceText;
        public string ChoiceText 
        {
            get {  return choiceText; } 
            set { choiceText = value; }
        }

        //Linked choice name?
        [SerializeField] private string nextNodeID;
        public string NextNodeID 
        {
            get { return nextNodeID; } 
            set {  nextNodeID = value; }
        }

        public DS_NodeChoiceData() { }
        public DS_NodeChoiceData(DS_NodeChoiceData choice)
        {
            ChoiceText = choice.ChoiceText;
            NextNodeID = choice.NextNodeID;
        }
    }
}
