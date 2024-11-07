using System.Collections.Generic;

namespace Narration.Data.Error
{
    using Elements;

    public class NarrationGroupErrorData
    {
        public NarrationErrorData ErrorData { get; set; }
        public List<NarrationGroup> Groups { get; set; }

        public NarrationGroupErrorData()
        {
            ErrorData = new NarrationErrorData();
            Groups = new List<NarrationGroup>();
        }
    }
}