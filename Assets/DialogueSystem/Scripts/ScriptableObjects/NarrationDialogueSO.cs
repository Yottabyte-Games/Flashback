using System.Collections.Generic;
using UnityEngine;

namespace Narration.ScriptableObjects
{
    using Data;
    using Enumerations;

    public class NarrationDialogueSO : ScriptableObject
    {
        [field: SerializeField] public string DialogueName { get; set; }
        [field: SerializeField] [field: TextArea()] public string Text { get; set; }
        [field: SerializeField] public List<NarrationDialogueChoiceData> Choices { get; set; }
        [field: SerializeField] public NarrationDialogueType DialogueType { get; set; }
        [field: SerializeField] public bool IsStartingDialogue { get; set; }

        public void Initialize(string dialogueName, string text, List<NarrationDialogueChoiceData> choices, NarrationDialogueType dialogueType, bool isStartingDialogue)
        {
            DialogueName = dialogueName;
            Text = text;
            Choices = choices;
            DialogueType = dialogueType;
            IsStartingDialogue = isStartingDialogue;
        }
    }
}