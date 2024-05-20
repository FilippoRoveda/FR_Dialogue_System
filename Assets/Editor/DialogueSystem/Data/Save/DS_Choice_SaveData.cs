using UnityEngine;


namespace DS.Data.Save
{
    [System.Serializable]
    public class DS_Choice_SaveData
    {
        [SerializeField] public string ChoiceName { get; set; }
        [SerializeField] public string LinkedNodeID { get; set; }
    }
}
