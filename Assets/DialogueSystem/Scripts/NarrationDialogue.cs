using UnityEngine;

namespace Narration
{
    using ScriptableObjects;

    public class NarrationDialogue : MonoBehaviour
    {
        /* Dialogue Scriptable Objects */
        [SerializeField] private NarrationDialogueContainerSO dialogueContainer;
        [SerializeField] private NarrationDialogueGroupSO dialogueGroup;
        [SerializeField] private NarrationDialogueSO dialogue;

        /* Filters */
        [SerializeField] private bool groupedDialogues;
        [SerializeField] private bool startingDialoguesOnly;

        /* Indexes */
        [SerializeField] private int selectedDialogueGroupIndex;
        [SerializeField] private int selectedDialogueIndex;
    }
}