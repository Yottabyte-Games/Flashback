using System.Collections.Generic;
using Editor.DialogueSystem.Elements;

namespace Editor.DialogueSystem.Data.Error
{
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