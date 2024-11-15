using System.Collections.Generic;
using DialogueSystem.Scripts.Data;
using DS.Enumerations;
using FMODUnity;
using UnityEngine;

namespace DialogueSystem.Scripts.ScriptableObjects
{
    public class DSDialogueSO : ScriptableObject
    {
        [field: SerializeField] public string dialogueName { get; set; }
        [field: SerializeField] [field: TextArea()] public string text { get; set; }
        [field: SerializeField] public List<DSDialogueChoiceData> choices { get; set; }
        [field: SerializeField] public DSDialogueType dialogueType { get; set; }
        [field: SerializeField] public bool isStartingDialogue { get; set; }
        [field: SerializeField] public EventReference voiceEvent { get; set; }

        public void Initialize(string dialogueName, string text, List<DSDialogueChoiceData> choices, DSDialogueType dialogueType, bool isStartingDialogue, EventReference voiceEvent)
        {
            this.dialogueName = dialogueName;
            this.text = text;
            this.choices = choices;
            this.dialogueType = dialogueType;
            this.voiceEvent = voiceEvent;
            this.isStartingDialogue = isStartingDialogue;
        }
    }
}