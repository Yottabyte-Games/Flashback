using System;
using UnityEngine;

namespace Editor.DialogueSystem.Data.Save
{
    [Serializable]
    public class NarrationChoiceSaveData
    {
        [field: SerializeField] public string Text { get; set; }
        [field: SerializeField] public string NodeID { get; set; }
    }
}