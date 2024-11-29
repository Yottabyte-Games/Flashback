using System.Collections.Generic;
using DialogueSystem.Enumerations;
using DialogueSystem.Enumerations.StoryEnum;
using DialogueSystem.Scripts.Data;
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
        [field: SerializeField] public NarratorEnumSO narratorEnum { get; set; }

        public void Initialize(string dialogueName, string text, List<DSDialogueChoiceData> choices, DSDialogueType dialogueType, bool isStartingDialogue, EventReference voiceEvent, NarratorEnumSO narratorEnum)
        {
            this.dialogueName = dialogueName;
            this.text = text;
            this.choices = choices;
            this.dialogueType = dialogueType;
            this.voiceEvent = voiceEvent;
            this.narratorEnum = narratorEnum;
            this.isStartingDialogue = isStartingDialogue;
        }
    }
}
