using System.Collections.Generic;
using UnityEngine;

namespace Narration.ScriptableObjects
{
    public class NarrationDialogueContainerSO : ScriptableObject
    {
        [field: SerializeField] public string FileName { get; set; }
        [field: SerializeField] public SerializableDictionary<NarrationDialogueGroupSO, List<NarrationDialogueSO>> DialogueGroups { get; set; }
        [field: SerializeField] public List<NarrationDialogueSO> UngroupedDialogues { get; set; }

        public void Initialize(string fileName)
        {
            FileName = fileName;

            DialogueGroups = new SerializableDictionary<NarrationDialogueGroupSO, List<NarrationDialogueSO>>();
            UngroupedDialogues = new List<NarrationDialogueSO>();
        }

        public List<string> GetDialogueGroupNames()
        {
            List<string> dialogueGroupNames = new List<string>();

            foreach (NarrationDialogueGroupSO dialogueGroup in DialogueGroups.Keys)
            {
                dialogueGroupNames.Add(dialogueGroup.GroupName);
            }

            return dialogueGroupNames;
        }

        public List<string> GetGroupedDialogueNames(NarrationDialogueGroupSO dialogueGroup, bool startingDialoguesOnly)
        {
            List<NarrationDialogueSO> groupedDialogues = DialogueGroups[dialogueGroup];

            List<string> groupedDialogueNames = new List<string>();

            foreach (NarrationDialogueSO groupedDialogue in groupedDialogues)
            {
                if (startingDialoguesOnly && !groupedDialogue.IsStartingDialogue)
                {
                    continue;
                }

                groupedDialogueNames.Add(groupedDialogue.DialogueName);
            }

            return groupedDialogueNames;
        }

        public List<string> GetUngroupedDialogueNames(bool startingDialoguesOnly)
        {
            List<string> ungroupedDialogueNames = new List<string>();

            foreach (NarrationDialogueSO ungroupedDialogue in UngroupedDialogues)
            {
                if (startingDialoguesOnly && !ungroupedDialogue.IsStartingDialogue)
                {
                    continue;
                }

                ungroupedDialogueNames.Add(ungroupedDialogue.DialogueName);
            }

            return ungroupedDialogueNames;
        }
    }
}