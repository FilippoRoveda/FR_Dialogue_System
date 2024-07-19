using System.Collections.Generic;


namespace DS.Editor.Errors
{
    using Editor.Elements;
    /// <summary>
    /// Class that hold error color for a list or DS_Groups.
    /// </summary>
    public class GroupErrorData
    {
        public ErrorColor ErrorColor { get; set; }
        public List<DS_Group> Groups { get; set; }
        public GroupErrorData()
        {
            ErrorColor = new ErrorColor();
            Groups = new List<DS_Group>();
        }
    }
}
