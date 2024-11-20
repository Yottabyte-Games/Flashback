using _Scripts.Fishing;
using NaughtyAttributes;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class DialoguePlayer : MonoBehaviour
{
    [SerializeField] List<DialogueStarter> dialogueStarters = new();
    [SerializeField] FishingStoryManager manager;
    public void PlayDialogue()
    {
        print(manager.fishCaught);
        dialogueStarters[manager.fishCaught].StartDialogue();
    }
}
