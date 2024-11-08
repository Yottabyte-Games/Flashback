using System;
using DialogueSystem.Scripts.ScriptableObjects;
using UnityEngine;

namespace DialogueSystem.Scripts.Data
{
    [Serializable]
    public class DSDialogueChoiceData
    {
        [field: SerializeField] public string Text { get; set; }
        [field: SerializeField] public DSDialogueSO NextDialogue { get; set; }
    }
}