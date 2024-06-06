using UnityEngine;


namespace DS.Data.Save
{
    [System.Serializable]
    public class DS_Group_SaveData
    {
        [SerializeField] private string id;
        public string ID 
        { 
            get {  return id; } 
            set {  id = value; } 
        }
        [SerializeField] private string name;
        public string Name 
        { 
            get { return name; } 
            set {  name = value; } 
        }
        [SerializeField] private Vector2 position;
        public Vector2 Position 
        { 
            get { return position; } 
            set {  position = value; } 
        }

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
