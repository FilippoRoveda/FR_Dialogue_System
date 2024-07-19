using System.Collections.Generic;

namespace DS.Editor.Errors
{
    using Editor.Elements;
    /// <summary>
    /// Class that hold error color for a list or DS_Nodes.
    /// </summary>
    [System.Serializable]
    public class NodeErrorData
    {
        public ErrorColor ErrorData {  get; set; }
        public List<BaseNode> Nodes { get; set; }

        public NodeErrorData() 
        {
            ErrorData = new ErrorColor();
            Nodes = new List<BaseNode>();
        }
    }
}
