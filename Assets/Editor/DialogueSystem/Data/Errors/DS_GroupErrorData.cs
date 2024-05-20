using System.Collections.Generic;

namespace DS.Data.Error
{
    public class DS_GroupErrorData
    {
        public DS_ErrorData ErrorData { get; set; }
        public List<DS_Group> Groups { get; set; }
        public DS_GroupErrorData()
        {
            ErrorData = new DS_ErrorData();
            Groups = new List<DS_Group>();
        }
    }
}
