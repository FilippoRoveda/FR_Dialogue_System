using DS.Elements;
using System.Collections.Generic;

namespace DS.Data.Error
{
    [System.Serializable]
    public class DS_NodeErrorData
    {
        public DS_ErrorData ErrorData {  get; set; }
        public List<DS_Node> Nodes { get; set; }

        public DS_NodeErrorData() 
        {
            ErrorData = new DS_ErrorData();
            Nodes = new List<DS_Node>();
        }
    }
}
