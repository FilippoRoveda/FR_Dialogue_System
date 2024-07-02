using UnityEngine;


namespace DS.Editor.Data
{
    /// <summary>
    /// Class that hold group informations to be contained in a graph scriptable object.
    /// </summary>
    [System.Serializable]
    public class DS_GroupData
    {
        [SerializeField] private string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        [SerializeField] private string id;
        public string ID 
        { 
            get {  return id; } 
            set {  id = value; } 
        }

        [SerializeField] private Vector2 position;
        public Vector2 Position 
        { 
            get { return position; } 
            set {  position = value; } 
        }

        // lista di nodi che ha all'interno? per poi instanziarli nuovamente come suoi

        public DS_GroupData() { }
        public DS_GroupData(string groupID, string groupTitle, Vector2 groupPosition)
        {
            ID = groupID;
            Name = groupTitle;
            Position = groupPosition;
        }
    }
}
