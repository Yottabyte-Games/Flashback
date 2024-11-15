using System;
using DialogueSystem.Scripts.ScriptableObjects;
using UnityEngine;

namespace DialogueSystem.Scripts.Data
{
    [Serializable]
    public class DSDialogueChoiceData
    {
        [field: SerializeField] public string text { get; set; }
        [field: SerializeField] public DSDialogueSo nextDialogue { get; set; }
    }
}