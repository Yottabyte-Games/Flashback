using System;
using UnityEngine;

namespace Narration.Data
{
    using ScriptableObjects;

    [Serializable]
    public class NarrationDialogueChoiceData
    {
        [field: SerializeField] public string Text { get; set; }
        [field: SerializeField] public NarrationDialogueSO NextDialogue { get; set; }
    }
}