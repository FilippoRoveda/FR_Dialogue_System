using UnityEngine;


namespace DS.Data.Save
{
    [System.Serializable]
    public class DS_ChoiceData
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

        public DS_ChoiceData() { }
        public DS_ChoiceData(DS_ChoiceData choice)
        {
            ChoiceName = choice.ChoiceName;
            NodeID = choice.NodeID;
        }
    }
}
