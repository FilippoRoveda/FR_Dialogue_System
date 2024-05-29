using UnityEngine;


namespace DS.Data.Save
{
    [System.Serializable]
    public class DS_Group_SaveData
    {
        [SerializeField] public string ID { get; set; }
        [SerializeField] public string Name { get; set; }
        [SerializeField] public Vector2 Position { get; set; }

        //Non manca una lista di nodi che ha all'interno? per poi instanziarli nuovamente come suoi

        public DS_Group_SaveData() { }
        public DS_Group_SaveData(DS_Group group)
        {
            ID = group.ID;
            Name = group.title;
            Position = group.GetPosition().position;
        }
    }
}
