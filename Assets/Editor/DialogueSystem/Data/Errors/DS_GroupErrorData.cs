using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;

namespace DS.Editor.Errors
{
    /// <summary>
    /// Class that hold error color for a list or DS_Groups.
    /// </summary>
    public class DS_GroupErrorData
    {
        public ErrorColor ErrorColor { get; set; }
        public List<Group> Groups { get; set; }
        public DS_GroupErrorData()
        {
            ErrorColor = new ErrorColor();
            Groups = new List<Group>();
        }
    }
}
