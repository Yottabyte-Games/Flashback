using UnityEngine;

namespace DS
{
    using ScriptableObjects;

    public class DSDialogue : MonoBehaviour
    {
        /* Dialogue Scriptable Objects */
        [SerializeField] DSDialogueContainerSO dialogueContainer;
        [SerializeField] DSDialogueGroupSO dialogueGroup;
        [SerializeField] DSDialogueSO dialogue;

        /* Filters */
        [SerializeField] bool groupedDialogues;
        [SerializeField] bool startingDialoguesOnly;

        /* Indexes */
        [SerializeField] int selectedDialogueGroupIndex;
        [SerializeField] int selectedDialogueIndex;
    }
}