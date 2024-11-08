using System.Collections.Generic;
using Editor.DialogueSystem.Elements;

namespace Editor.DialogueSystem.Data.Error
{
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