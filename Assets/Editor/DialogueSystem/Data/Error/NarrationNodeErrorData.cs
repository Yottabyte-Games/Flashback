using System.Collections.Generic;

namespace Narration.Data.Error
{
    using Elements;

    public class NarrationNodeErrorData
    {
        public NarrationErrorData ErrorData { get; set; }
        public List<NarrationNode> Nodes { get; set; }

        public NarrationNodeErrorData()
        {
            ErrorData = new NarrationErrorData();
            Nodes = new List<NarrationNode>();
        }
    }
}