using DS.Elements;
using System.Collections.Generic;

namespace DS.Data.Error
{
    /// <summary>
    /// Class that hold error color for a list or DS_Nodes.
    /// </summary>
    [System.Serializable]
    public class DS_NodeErrorData
    {
        public ErrorColor ErrorData {  get; set; }
        public List<DS_BaseNode> Nodes { get; set; }

        public DS_NodeErrorData() 
        {
            ErrorData = new ErrorColor();
            Nodes = new List<DS_BaseNode>();
        }
    }
}
