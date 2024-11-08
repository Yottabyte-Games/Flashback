using System;
using UnityEngine;

namespace Editor.DialogueSystem.Data.Save
{
    [Serializable]
    public class NarrationGroupSaveData
    {
        [field: SerializeField] public string ID { get; set; }
        [field: SerializeField] public string Name { get; set; }
        [field: SerializeField] public Vector2 Position { get; set; }
    }
}