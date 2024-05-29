using UnityEngine;


namespace DS.Data.Save
{
    [System.Serializable]
    public class DS_ChoiceData
    {
        [SerializeField] public string ChoiceName { get; set; }

        //Linked choice name?
        [SerializeField] public string NodeID { get; set; }

        public DS_ChoiceData() { }
        public DS_ChoiceData(DS_ChoiceData choice)
        {
            ChoiceName = choice.ChoiceName;
            NodeID = choice.NodeID;
        }
    }
}
