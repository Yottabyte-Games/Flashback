using System;
using System.Collections.Generic;
using Narration.Enumerations;
using UnityEngine;

namespace Editor.DialogueSystem.Data.Save
{
    [Serializable]
    public class NarrationNodeSaveData
    {
        [field: SerializeField] public string ID { get; set; }
        [field: SerializeField] public string Name { get; set; }
        [field: SerializeField] public string Text { get; set; }
        [field: SerializeField] public List<NarrationChoiceSaveData> Choices { get; set; }
        [field: SerializeField] public string GroupID { get; set; }
        [field: SerializeField] public NarrationDialogueType DialogueType { get; set; }
        [field: SerializeField] public Vector2 Position { get; set; }
    }
}