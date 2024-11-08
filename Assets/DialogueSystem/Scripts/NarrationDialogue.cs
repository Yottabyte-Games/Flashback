using UnityEngine;

namespace Narration
{
    using ScriptableObjects;

    public class NarrationDialogue : MonoBehaviour
    {
        /* Dialogue Scriptable Objects */
        [SerializeField] NarrationDialogueContainerSO dialogueContainer;
        [SerializeField] NarrationDialogueGroupSO dialogueGroup;
        [SerializeField] NarrationDialogueSO dialogue;

        /* Filters */
        [SerializeField] bool groupedDialogues;
        [SerializeField] bool startingDialoguesOnly;

        /* Indexes */
        [SerializeField] int selectedDialogueGroupIndex;
        [SerializeField] int selectedDialogueIndex;
    }
}