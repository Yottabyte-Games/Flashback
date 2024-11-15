using System.Collections.Generic;
using DialogueSystem.Scripts.Data;
using DS.Enumerations;
using UnityEngine;

namespace DialogueSystem.Scripts.ScriptableObjects
{
    public class DSDialogueSo : ScriptableObject
    {
        [field: SerializeField] public string dialogueName { get; set; }
        [field: SerializeField] [field: TextArea()] public string text { get; set; }
        [field: SerializeField] public List<DSDialogueChoiceData> choices { get; set; }
        [field: SerializeField] public DSDialogueType dialogueType { get; set; }
        [field: SerializeField] public bool isStartingDialogue { get; set; }
        [field: SerializeField] public int voiceClipIndex { get; set; }


        public void Initialize(string dialogueName, string text, List<DSDialogueChoiceData> choices, DSDialogueType dialogueType, bool isStartingDialogue)
        {
            this.dialogueName = dialogueName;
            this.text = text;
            this.choices = choices;
            this.dialogueType = dialogueType;
            this.isStartingDialogue = isStartingDialogue;
            this.voiceClipIndex = voiceClipIndex;

        }
    }
}