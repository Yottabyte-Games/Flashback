using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem.Scripts.ScriptableObjects
{
    public class DSDialogueContainerSo : ScriptableObject
    {
        [field: SerializeField] public string fileName { get; set; }
        [field: SerializeField] public SerializableDictionary<DSDialogueGroupSo, List<DSDialogueSo>> dialogueGroups { get; set; }
        [field: SerializeField] public List<DSDialogueSo> ungroupedDialogues { get; set; }

        public void Initialize(string fileName)
        {
            this.fileName = fileName;

            dialogueGroups = new SerializableDictionary<DSDialogueGroupSo, List<DSDialogueSo>>();
            ungroupedDialogues = new List<DSDialogueSo>();
        }

        public List<string> GetDialogueGroupNames()
        {
            List<string> dialogueGroupNames = new List<string>();

            foreach (DSDialogueGroupSo dialogueGroup in dialogueGroups.Keys)
            {
                dialogueGroupNames.Add(dialogueGroup.groupName);
            }

            return dialogueGroupNames;
        }

        public List<string> GetGroupedDialogueNames(DSDialogueGroupSo dialogueGroup, bool startingDialoguesOnly)
        {
            List<DSDialogueSo> groupedDialogues = dialogueGroups[dialogueGroup];

            List<string> groupedDialogueNames = new List<string>();

            foreach (DSDialogueSo groupedDialogue in groupedDialogues)
            {
                if (startingDialoguesOnly && !groupedDialogue.isStartingDialogue)
                {
                    continue;
                }

                groupedDialogueNames.Add(groupedDialogue.dialogueName);
            }

            return groupedDialogueNames;
        }

        public List<string> GetUngroupedDialogueNames(bool startingDialoguesOnly)
        {
            List<string> ungroupedDialogueNames = new List<string>();

            foreach (DSDialogueSo ungroupedDialogue in ungroupedDialogues)
            {
                if (startingDialoguesOnly && !ungroupedDialogue.isStartingDialogue)
                {
                    continue;
                }

                ungroupedDialogueNames.Add(ungroupedDialogue.dialogueName);
            }

            return ungroupedDialogueNames;
        }
    }
}