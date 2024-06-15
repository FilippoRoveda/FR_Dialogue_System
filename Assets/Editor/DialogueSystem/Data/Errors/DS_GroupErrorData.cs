using System.Collections.Generic;

namespace DS.Data.Error
{
    /// <summary>
    /// Class that hold error color for a list or DS_Groups.
    /// </summary>
    public class DS_GroupErrorData
    {
        public ErrorColor ErrorColor { get; set; }
        public List<DS_Group> Groups { get; set; }
        public DS_GroupErrorData()
        {
            ErrorColor = new ErrorColor();
            Groups = new List<DS_Group>();
        }
    }
}
