using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;

namespace DS.Editor.Errors
{
    /// <summary>
    /// Class that hold error color for a list or DS_Nodes.
    /// </summary>
    [System.Serializable]
    public class DS_NodeErrorData
    {
        public ErrorColor ErrorData {  get; set; }
        public List<Node> Nodes { get; set; }

        public DS_NodeErrorData() 
        {
            ErrorData = new ErrorColor();
            Nodes = new List<Node>();
        }
    }
}
