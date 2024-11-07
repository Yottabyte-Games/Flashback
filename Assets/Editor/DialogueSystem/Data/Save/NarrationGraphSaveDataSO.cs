using System.Collections.Generic;
using UnityEngine;

namespace Narration.Data.Save
{
    public class NarrationGraphSaveDataSO : ScriptableObject
    {
        [field: SerializeField] public string FileName { get; set; }
        [field: SerializeField] public List<NarrationGroupSaveData> Groups { get; set; }
        [field: SerializeField] public List<NarrationNodeSaveData> Nodes { get; set; }
        [field: SerializeField] public List<string> OldGroupNames { get; set; }
        [field: SerializeField] public List<string> OldUngroupedNodeNames { get; set; }
        [field: SerializeField] public SerializableDictionary<string, List<string>> OldGroupedNodeNames { get; set; }

        public void Initialize(string fileName)
        {
            FileName = fileName;

            Groups = new List<NarrationGroupSaveData>();
            Nodes = new List<NarrationNodeSaveData>();
        }
    }
}