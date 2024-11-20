using _Scripts.Fishing;
using UnityEngine;

public class DialoguePlayer : MonoBehaviour
{
    [SerializeField] DialogueStarter[] dialogueStarters;
    [SerializeField] FishingStoryManager manager;
    public void PlayDialogue()
    {
        dialogueStarters[manager.fishCaught - 1].StartDialogue();
    }
}
