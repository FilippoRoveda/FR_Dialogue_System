using UnityEngine;


namespace DS.Data.Save
{
    /// <summary>
    /// Class that hold choice informations to be saved inside DS_Node_SaveData.
    /// </summary>
    [System.Serializable]
    public class DS_Choice_SaveData
    {
        [SerializeField] private string choiceName;
        public string ChoiceName 
        {
            get {  return choiceName; } 
            set { choiceName = value; }
        }

        //Linked choice name?
        [SerializeField] private string nodeID;
        public string NodeID 
        {
            get { return nodeID; } 
            set {  nodeID = value; }
        }

        public DS_Choice_SaveData() { }
        public DS_Choice_SaveData(DS_Choice_SaveData choice)
        {
            ChoiceName = choice.ChoiceName;
            NodeID = choice.NodeID;
        }
    }
}
